using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.Implementations;

namespace WordCount.Tests
{
    [TestClass]
    public class ByFirstCharPartitionerTests
    {
        [TestMethod]
        public void WordsStartingWithSameLetterGoToSamePartition()
        {
            var sut = new ByFirstCharPartitioner();
            var partitionQueues = new BlockingCollection<string>[2];
            partitionQueues[0] = new BlockingCollection<string>();
            partitionQueues[1] = new BlockingCollection<string>();

            var testWords = GenerateWordsStartingWith('a').ToArray();

            sut.PartitionIntoQueues(testWords, partitionQueues);

            Assert.IsTrue(
                partitionQueues[0].Count == testWords.Length && partitionQueues[1].Count == 0 ||
                partitionQueues[0].Count == 0 && partitionQueues[1].Count == testWords.Length);
        }

        [TestMethod]
        public void WordsStartingWithAdjacentCodepointsGoToDifferentPartitions()
        {
            var sut = new ByFirstCharPartitioner();
            var partitionQueues = new BlockingCollection<string>[2];
            partitionQueues[0] = new BlockingCollection<string>();
            partitionQueues[1] = new BlockingCollection<string>();

            var testWords = GenerateWordsStartingWith('a').Concat(GenerateWordsStartingWith('b')).ToArray();

            sut.PartitionIntoQueues(testWords, partitionQueues);

            Assert.IsTrue(
                partitionQueues[0].Count == testWords.Length / 2 && partitionQueues[1].Count == testWords.Length / 2);
        }

        private IEnumerable<string> GenerateWordsStartingWith(char firstLetter)
        {
            return new[] {"first", "second", "third"}.Select(word => firstLetter.ToString() + word);
        }
    }
}
