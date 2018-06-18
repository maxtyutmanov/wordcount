using System;
using System.Collections.Generic;
using System.Text;

namespace WordCount
{
    public class WordWithCount
    {
        public string Word { get;}
        public int Count { get; }

        public WordWithCount(string word, int count)
        {
            Word = word;
            Count = count;
        }
    }
}
