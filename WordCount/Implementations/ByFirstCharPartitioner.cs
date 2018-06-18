using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WordCount.Interfaces;

namespace WordCount.Implementations
{
    public class ByFirstCharPartitioner : IInputPartitioner
    {
        public void PartitionIntoQueues(IEnumerable<string> words, BlockingCollection<string>[] queues)
        {
            foreach (string word in words)
            {
                var partitionId = Convert.ToUInt32(word[0]) % queues.Length;
                queues[partitionId].Add(word);
            }
        }
    }
}
