using System;
using System.Collections;
using System.Collections.Generic;
using LordJZ.Linq;
using System.Text;
using System.Threading.Tasks;
using LordJZ.Collections.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LordJZ.Tests.Linq
{
    public static class TestHelpers
    {
        public static TResult TestCollectionConvertion<T, TResult>
            (IEnumerable<T> collection, CollectionProxyKind proxyAs, Func<IEnumerable<T>, TResult> func)
            where TResult : IEnumerable<T>
        {
            TResult result = func(collection.MakeProxy(proxyAs));
            Assert.IsNotNull(result);
            Assert.IsTrue(collection.SequenceEqual(result));
            return result;
        }
    }
}
