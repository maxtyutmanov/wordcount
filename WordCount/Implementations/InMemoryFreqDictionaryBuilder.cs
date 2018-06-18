using System;
using System.Collections.Generic;
using System.Linq;
using WordCount.Interfaces;

namespace WordCount.Implementations
{
    public class InMemoryFreqDictionaryBuilder : IFreqDictionaryBuilder
    {
        public IEnumerable<WordWithCount> BuildOrderedDict(IEnumerable<string> words)
        {
            var internalDict = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (string word in words)
            {
                if (internalDict.TryGetValue(word, out int currentCount))
                {
                    internalDict[word] = currentCount + 1;
                }
                else
                {
                    internalDict.Add(word, 1);
                }
            }

            return internalDict
                .OrderByDescending(entry => entry.Value)
                .Select(entry => new WordWithCount(entry.Key, entry.Value));
        }
    }
}