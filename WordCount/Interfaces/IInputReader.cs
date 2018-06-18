using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordCount.Interfaces
{
    public interface IInputReader
    {
        IEnumerable<string> ReadWords(Stream inputStream, Encoding encoding);
    }
}
