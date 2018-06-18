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
    public class IntegrationTests
    {
        private static readonly Encoding InputEncoding = Encoding.GetEncoding("windows-1251");
        private static readonly Encoding OutputEncoding = Encoding.GetEncoding("windows-1251");

        [TestMethod]
        public void CreatesFreqDictionary()
        {
            var sut = CreateRealSut();

            var input = TestUtils.StringToStream("это тест Это тест this iS test and it is All it is", InputEncoding);
            var output = new MemoryStream();
            
            sut.Run(input, output);

            var actual = TestUtils.StreamToString(output, OutputEncoding);

            Assert.AreEqual("is,3\nэто,2\nit,2\nтест,2\nand,1\nall,1\nthis,1\ntest,1\n", actual);
        }

        private Facade CreateRealSut()
        {
            return new Facade(
                new InputReader(), new ByFirstCharPartitioner(), 
                new InMemoryFreqDictionaryBuilder(), new OutputWriter());
        }
    }
}
