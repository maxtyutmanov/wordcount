using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.Implementations;

namespace WordCount.Tests
{
    [TestClass]
    public class InMemoryFreqDictionaryBuilderTests
    {
        [TestMethod]
        public void FreqDictionaryIsBuilt()
        {
            var sut = new InMemoryFreqDictionaryBuilder();
            var freqDict = sut.BuildOrderedDict(new[] {"аисты", "свили", "гнездо"}).ToArray();

            Assert.AreEqual(3, freqDict.Length);
            Assert.AreEqual(1, freqDict.First(entry => entry.Word == "аисты").Count);
            Assert.AreEqual(1, freqDict.First(entry => entry.Word == "свили").Count);
            Assert.AreEqual(1, freqDict.First(entry => entry.Word == "гнездо").Count);
        }

        [TestMethod]
        public void FreqDictionaryIsOrdered()
        {
            var sut = new InMemoryFreqDictionaryBuilder();
            var freqDict = sut.BuildOrderedDict(new[] { "аисты", "свили", "гнездо", "гнездо" }).ToArray();

            Assert.AreEqual(3, freqDict.Length);
            Assert.AreEqual("гнездо", freqDict[0].Word);
            Assert.AreEqual(2, freqDict[0].Count);
        }
    }
}
