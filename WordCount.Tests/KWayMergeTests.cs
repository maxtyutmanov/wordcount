using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.Utils;

namespace WordCount.Tests
{
    [TestClass]
    public class KWayMergeTests
    {
        [TestMethod]
        public void CorrectlyMergesMultipleSortedEnumerators()
        {
            var seq1 = new[] {10, 7, 1};
            var seq2 = new[] {12, 8, 1};
            var seq3 = new[] {9, 5, 4};

            var sequenceEnumerators = new[] {seq1, seq2, seq3}
                .Select(seq => ((IEnumerable<int>)seq).GetEnumerator())
                .ToArray();

            KWayMerge<int> sut = new KWayMerge<int>(sequenceEnumerators, (x, y) => x > y);

            int[] actual = sut.Execute().ToArray();

            CollectionAssert.AreEqual(new[] { 12, 10, 9, 8, 7, 5, 4, 1, 1 }, actual);
        }
    }
}
