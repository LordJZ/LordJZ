using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LordJZ.Linq;

namespace LordJZ.Tests.Linq
{
    [TestClass]
    public class OrderedMatchTests
    {
        static void Test(int[] haystack, int[] needles, int?[] result)
        {
            int?[] got = haystack.OrderedMatch(needles, _ => _, _ => _, Comparer<int>.Default,
                                               (v1, v2, r, i1, i2) =>
                                               {
                                                   Assert.AreEqual(r, v2);
                                                   Assert.AreEqual(r, v1);

                                                   return (int?)r;
                                               },
                                               (v, k, i1, i2) => (int?)null)
                                 .ToArray();
            Assert.IsTrue(got.SequenceEqual(result));
        }

        [TestMethod]
        public void TestSimpleSequence()
        {
            Test(new[] { 1, 2, 3 }, new[] { 2, 3, 4 }, new int?[] { 2, 3, null });
            Test(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, new int?[] { 1, 2, 3, null });
            Test(new[] { 1, 3, 4 }, new[] { 1, 2, 4 }, new int?[] { 1, null, 4 });
        }

        [TestMethod]
        public void TestEmptySequences()
        {
            Test(new[] { 1, 2, 3 }, new int[0], new int?[0]);
            Test(new int[0], new[] { 1, 2, 3, 4 }, new int?[] { null, null, null, null });
        }

        [TestMethod]
        public void TestDuplicates()
        {
            Test(new[] { 1, 2, 2, 3 }, new[] { 1, 2, 3, 4 }, new int?[] { 1, 2, 3, null });
            Test(new[] { 1, 2, 2, 3 }, new[] { 1, 2 }, new int?[] { 1, 2 });
            Test(new[] { 2, 2, 3 }, new[] { 2 }, new int?[] { 2 });
            Test(new[] { 1, 2, 4 }, new[] { 1, 2, 2, 3, 4 }, new int?[] { 1, 2, null, null, 4 });
            Test(new[] { 1, 2, 2, 4 }, new[] { 1, 2, 2, 3, 4 }, new int?[] { 1, 2, 2, null, 4 });
        }
    }
}
