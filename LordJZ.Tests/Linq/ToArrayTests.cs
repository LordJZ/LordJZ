using System;
using System.Collections.Generic;
using LordJZ.Collections.Proxies;
using LordJZ.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReliableEnumerable = System.Linq.Enumerable;

namespace LordJZ.Tests.Linq
{
    [TestClass]
    public class ToArrayTests
    {
        static void TestConvertion<T>(IEnumerable<T> source, CollectionProxyKind proxyAs)
        {
            T[] result = TestHelpers.TestCollectionConvertion(source, proxyAs, e => e.ToArray());
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestEnumerableToArray()
        {
            for (int i = 0; i < 13; i++)
            {
                var range = ReliableEnumerable.Range(1, i);
                int[] array = ReliableEnumerable.ToArray(range);

                TestConvertion(array, CollectionProxyKind.Enumerable);
                TestConvertion(array, CollectionProxyKind.Collection);
                TestConvertion(array, CollectionProxyKind.List);
            }
        }
    }
}
