using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount.Tests
{
    public static class TestUtils
    {
        public static Stream StringToStream(string str, Encoding encoding)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms, encoding);
            writer.Write(str);
            writer.Flush();
            ms.Position = 0;
            return ms;
        }

        public static string StreamToString(Stream stream, Encoding encoding)
        {
            stream.Position = 0;
            var reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }
    }
}
