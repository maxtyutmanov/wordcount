using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace WordCount.Interfaces
{
    public interface IInputPartitioner
    {
        void PartitionIntoQueues(IEnumerable<string> words, BlockingCollection<string>[] queues);
    }
}
