using System;
using System.Collections.Generic;
using System.Text;

namespace WordCount.Interfaces
{
    public interface IFreqDictionaryBuilder
    {
        IEnumerable<WordWithCount> BuildOrderedDict(IEnumerable<string> words);
    }
}
