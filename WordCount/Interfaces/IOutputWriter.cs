using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordCount.Interfaces
{
    public interface IOutputWriter
    {
        void WriteFrequencyDict(IEnumerable<WordWithCount> sortedFreqDict, Stream outputStream, Encoding encoding);
    }
}
