using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.Implementations;

namespace WordCount.Tests
{
    [TestClass]
    public class InputReaderTests
    {
        [TestMethod]
        public void InputIsTokenizedByNewlinesAndSpaces()
        {
            var input = TestUtils.StringToStream(
                "аисты свили гнездо \r\nаисты \nсвили гнездо",
                Encoding.GetEncoding("windows-1251"));

            var sut = new InputReader();
            var words = sut.ReadWords(input, Encoding.GetEncoding("windows-1251")).ToArray();

            CollectionAssert.AreEqual(new [] {"аисты", "свили", "гнездо", "аисты", "свили", "гнездо" }, words);
        }

        [TestMethod]
        public void InputIsBroughtToLowercase()
        {
            var input = TestUtils.StringToStream(
                "Аисты сВили гнездО",
                Encoding.GetEncoding("windows-1251"));

            var sut = new InputReader();
            var words = sut.ReadWords(input, Encoding.GetEncoding("windows-1251")).ToArray();

            CollectionAssert.AreEqual(new[] { "аисты", "свили", "гнездо" }, words);
        }

        [TestMethod]
        public void ToleratesEmptyInput()
        {
            var input = TestUtils.StringToStream(
                "",
                Encoding.GetEncoding("windows-1251"));

            var sut = new InputReader();
            var words = sut.ReadWords(input, Encoding.GetEncoding("windows-1251")).ToArray();

            CollectionAssert.AreEqual(new string[0], words);
        }

        [TestMethod]
        public void DoesNotProduceEmptyWords()
        {
            var input = TestUtils.StringToStream(
                "аисты\r\n\r\n   свили   гнездо",
                Encoding.GetEncoding("windows-1251"));

            var sut = new InputReader();
            var words = sut.ReadWords(input, Encoding.GetEncoding("windows-1251")).ToArray();

            CollectionAssert.AreEqual(new[] { "аисты", "свили", "гнездо" }, words);
        }

        
    }
}
