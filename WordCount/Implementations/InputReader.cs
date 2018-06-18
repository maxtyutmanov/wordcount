using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordCount.Interfaces;

namespace WordCount.Implementations
{
    public class InputReader : IInputReader
    {
        private const int BufferSize = 1024;

        public IEnumerable<string> ReadWords(Stream inputStream, Encoding encoding)
        {
            var reader = new StreamReader(inputStream, encoding, false, BufferSize);

            var currentWord = new StringBuilder();

            // not reading by entire lines in order to account for extremely large lines (who knows?)
            // reading char by char instead (relying on the buffering mechanism inside the streamreader)
            while (!reader.EndOfStream)
            {
                var currentChar = (char) reader.Read();

                if (IsDelimiterChar(currentChar))
                {
                    if (currentWord.Length != 0)
                    {
                        yield return currentWord.ToString();
                        currentWord.Clear();
                    }
                }
                else
                {
                    // converting to lowercase to support case-insensitivity
                    currentWord.Append(Char.ToLower(currentChar));
                }
            }

            if (currentWord.Length != 0)
            {
                yield return currentWord.ToString();
            }
        }

        private bool IsDelimiterChar(char c)
        {
            return c == '\r' || c == '\n' || c == ' ';
        }
    }
}
