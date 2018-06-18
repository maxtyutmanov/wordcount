using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.Implementations;

namespace WordCount.Tests
{
    [TestClass]
    public class OutputWriterTests
    {
        private static readonly Encoding OutputEncoding = Encoding.GetEncoding("windows-1251");

        [TestMethod]
        public void OutputLayoutIsOk()
        {
            var outStream = new MemoryStream();
            var sut = new OutputWriter();
            sut.WriteFrequencyDict(new[]
            {
                new WordWithCount("аисты", 15),
                new WordWithCount("свили", 20),
                new WordWithCount("гнездо", 11)
            }, outStream, OutputEncoding);
            outStream.Flush();

            string actual = TestUtils.StreamToString(outStream, OutputEncoding);

            Assert.AreEqual("аисты,15\nсвили,20\nгнездо,11\n", actual);
        }
    }
}
