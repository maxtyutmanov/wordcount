using System.Collections.Generic;
using System.IO;
using System.Text;
using WordCount.Interfaces;

namespace WordCount.Implementations
{
    public class OutputWriter : IOutputWriter
    {
        public void WriteFrequencyDict(IEnumerable<WordWithCount> words, Stream outStream, Encoding encoding)
        {
            using (var writer = new StreamWriter(outStream, encoding, 1024, true))
            {
                foreach (WordWithCount wordWithCount in words)
                {
                    writer.Write(wordWithCount.Word + "," + wordWithCount.Count + "\n");
                }
            }
        }
    }
}