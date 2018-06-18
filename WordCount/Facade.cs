using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCount.Interfaces;
using WordCount.Utils;

namespace WordCount
{
    public class Facade
    {
        private static readonly Encoding InputEncoding = Encoding.GetEncoding("windows-1251");
        private static readonly Encoding OutputEncoding = Encoding.GetEncoding("windows-1251");
        private static readonly int PartitionCount = Environment.ProcessorCount;

        private const int PartitionQueuesCapacity = 1024;
        private const int OutputQueueCapacity = 1024;

        private readonly IInputReader _inputReader;
        private readonly IInputPartitioner _inputPartitioner;
        private readonly IFreqDictionaryBuilder _freqDictBuilder;
        private readonly IOutputWriter _outputWriter;

        public Facade(
            IInputReader inputReader, IInputPartitioner inputPartitioner, 
            IFreqDictionaryBuilder freqDictBuilder, IOutputWriter outputWriter)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _inputPartitioner = inputPartitioner ?? throw new ArgumentNullException(nameof(inputPartitioner));
            _freqDictBuilder = freqDictBuilder ?? throw new ArgumentNullException(nameof(freqDictBuilder));
            _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
        }

        public void Run(Stream input, Stream output)
        {
            // main idea: all CPU-bound operations run in the background threads, possibly with parallelism
            // all IO-bound operations use the current thread

            BlockingCollection<string>[] partitionQueues = CreatePartitionQueues();
            var outputQueue = new BlockingCollection<WordWithCount>(OutputQueueCapacity);

            Task<IEnumerable<WordWithCount>>[] freqDictBuildTasks = partitionQueues
                .Select(BuildFreqDictionaryFromWordsInQueue)
                .ToArray();

            Instrument.This(() =>
            {
                ReadWordsFromFileIntoQueues(input, partitionQueues);
                Task.WaitAll(freqDictBuildTasks);
            }, "building freq dictionaries for each partition");

            var partitionDicts = freqDictBuildTasks.Select(t => t.Result).ToArray();

            Instrument.This(() =>
            {
                // run merge task in the background thread
                var mergeTask = MergePartitionDictionariesIntoOutputQueue(partitionDicts, outputQueue);
                WriteOutputQueueToOutputStream(outputQueue, output);
                // make sure the merge task completed successfully
                mergeTask.Wait();
            }, "merging freq dictionaries to output file");
        }

        private void WriteOutputQueueToOutputStream(BlockingCollection<WordWithCount> outputQueue, Stream output)
        {
            _outputWriter.WriteFrequencyDict(outputQueue.GetConsumingEnumerable(), output, OutputEncoding);
        }

        private Task MergePartitionDictionariesIntoOutputQueue(
            IEnumerable<WordWithCount>[] partitionDicts,
            BlockingCollection<WordWithCount> outputQueue)
        {
            return Task.Run(() =>
            {
                try
                {
                    var partitionEnumerators = partitionDicts.Select(pd => pd.GetEnumerator()).ToArray();
                    var kwayMerge = new KWayMerge<WordWithCount>(partitionEnumerators, WordGreatherThanFunc);

                    foreach (WordWithCount wordWithCount in kwayMerge.Execute())
                    {
                        outputQueue.Add(wordWithCount);
                    }
                }
                finally
                {
                    outputQueue.CompleteAdding();
                }
            });
        }

        private static bool WordGreatherThanFunc(WordWithCount w1, WordWithCount w2)
        {
            return w1.Count > w2.Count;
        }

        private void ReadWordsFromFileIntoQueues(Stream input, BlockingCollection<string>[] partitionQueues)
        {
            try
            {
                IEnumerable<string> inputWords = _inputReader.ReadWords(input, InputEncoding);
                _inputPartitioner.PartitionIntoQueues(inputWords, partitionQueues);
            }
            finally
            {
                foreach (var outputQueue in partitionQueues)
                {
                    outputQueue.CompleteAdding();
                }
            }
        }

        private Task<IEnumerable<WordWithCount>> BuildFreqDictionaryFromWordsInQueue(BlockingCollection<string> wordsQueue)
        {
            return Task.Run(() =>
            {
                var freqDict = _freqDictBuilder.BuildOrderedDict(wordsQueue.GetConsumingEnumerable());
                // force execution in the background task to gain more parallelism
                return (IEnumerable<WordWithCount>)freqDict.ToArray();
            });
        }

        private BlockingCollection<string>[] CreatePartitionQueues()
        {
            var partitionQueues = new BlockingCollection<string>[PartitionCount];
            for (var i = 0; i < partitionQueues.Length; i++)
            {
                partitionQueues[i] = new BlockingCollection<string>(PartitionQueuesCapacity);
            }

            return partitionQueues;
        }
    }
}
