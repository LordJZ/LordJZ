using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LordJZ.Linq;
using LordJZ.Collections.Proxies;

namespace LordJZ.Tests.Linq
{
    [TestClass]
    public class MakeProxyTests
    {
        #region ReadOnlyCollection
        class ReadOnlyCollection<T> : IReadOnlyCollection<T>
        {
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            int IReadOnlyCollection<T>.Count
            {
                get { throw new NotImplementedException(); }
            }
        }
        #endregion
        #region ReadOnlyList
        class ReadOnlyList<T> : IReadOnlyList<T>
        {
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            int IReadOnlyCollection<T>.Count
            {
                get { throw new NotImplementedException(); }
            }

            T IReadOnlyList<T>.this[int index]
            {
                get { throw new NotImplementedException(); }
            }
        }
        #endregion

        static readonly Type Co_RO = typeof(ReadOnlyCollection<int>);
        static readonly Type Co_RO_Proxy = typeof(ReadOnlyCollectionProxy<int>);
        static readonly Type Li_RO = typeof(ReadOnlyList<int>);
        static readonly Type Li_RO_Proxy = typeof(ReadOnlyListProxy<int>);

        static Tuple<Type, Type, Type> _(Type t1, Type t2, Type t3)
        {
            return Tuple.Create(t1, t2, t3);
        }

        // source collection type, any proxy type, mutable proxy type, readonly proxy type
        readonly Dictionary<Type, Tuple<Type, Type, Type>> m_types =
            new Dictionary<Type, Tuple<Type, Type, Type>>
            {
                { Co_RO, _(Co_RO_Proxy, null, Co_RO_Proxy) },
                { Li_RO, _(Co_RO_Proxy, null, Co_RO_Proxy) },
            };

        readonly bool?[] m_nullableBoolValues = new bool?[] { null, false, true };

        void AssertType(Type t, Func<object> obj)
        {
            Assert.IsTrue(obj().GetType() == t);
        }

        void AssertException<T>(Func<object> obj) where T : Exception
        {
            bool? exception = null;

            try
            {
                obj();
            }
            catch (Exception e)
            {
                exception = e is T;

                if (!exception.Value)
                    throw new Exception("Exception of unexpected type was thrown");
            }

            if (!exception.HasValue)
                throw new Exception("Exception was not thrown.");
        }

        [TestMethod]
        public void Test_Enumerable_Of_T()
        {
            foreach (var pair in this.m_types)
            {
                var collectionType = pair.Key;
                var tuple = pair.Value;
                var array = tuple.ToArray();
                foreach (var kvp in array.Select((_, i) => Tuple.Create(i, _)))
                {
                    int index = kvp.Item1;
                    Type type = kvp.Item2;

                    bool? readOnly = m_nullableBoolValues[index];

                    IEnumerable<int> obj = (IEnumerable<int>)Activator.CreateInstance(collectionType);
                    Func<object> factory = () => obj.MakeProxy(CollectionProxyKind.Collection, readOnly);
                    if (type == null)
                        AssertException<ArgumentException>(factory);
                    else
                        AssertType(type, factory);
                }
            }
        }
    }
}
