using System;
using System.Collections.Generic;
using System.Linq;
using LordJZ.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LordJZ.Tests
{
    [TestClass]
    public class ObservableListTests
    {
        static readonly Random random = new Random(Environment.TickCount * 3 / 2);

        #region Helpers

        void TestHelper<T>(IList<T> proper, IList<T> tested, Action<IList<T>> action)
        {
            action(proper);
            action(tested);

            this.TestHelper(proper, tested);
        }

        void TestHelper<T>(IList<T> proper, IList<T> tested)
        {
            Assert.AreEqual(proper.Count, tested.Count);
            Assert.IsTrue(proper.SequenceEqual(tested));

            var properArr = new T[proper.Count];
            var testedArr = new T[tested.Count];

            proper.CopyTo(properArr, 0);
            tested.CopyTo(testedArr, 0);

            Assert.IsTrue(properArr.SequenceEqual(testedArr));
            Assert.IsTrue(testedArr.SequenceEqual(tested));

            for (int i = 0; i < proper.Count; i++)
                Assert.AreEqual(proper[i], tested[i]);

            int index = 0;
            foreach (var item in tested)
            {
                Assert.AreEqual(proper[index], item);
                Assert.AreEqual(proper[index], tested[index]);
                ++index;
            }
        }

        void TestHelper(Action<IList<int>, IList<int>> action)
        {
            IList<int> proper;
            IList<int> tested;

            CreateLists(out proper, out tested);

            action(proper, tested);
        }

        void CreateLists(out IList<int> proper, out IList<int> tested)
        {
            proper = new List<int>();
#pragma warning disable 612,618
            tested = new ObservableList<int>();
#pragma warning restore 612,618

            for (int i = 0; i < 50; i++)
            {
                int value = random.Next();
                int index = random.Next(0, proper.Count);
                this.TestHelper(proper, tested, list => list.Insert(index, value));
            }
        }

        #endregion

        #region Add

        void DoAdd(IList<int> proper, IList<int> tested)
        {
            for (int i = 0; i < 50; i++)
            {
                var value = random.Next();
                this.TestHelper(proper, tested, list => list.Add(value));
            }
        }

        [TestMethod]
        public void TestAdd()
        {
            TestHelper(DoAdd);
        }

        #endregion

        #region Insert

        void DoInsert(IList<int> proper, IList<int> tested)
        {
            for (int i = 0; i < 150; i++)
            {
                var value = random.Next();
                int index;
                switch (random.Next(1, 3))
                {
                    case 1:
                        index = 0;
                        break;
                    case 2:
                        index = proper.Count;
                        break;
                    case 3:
                        index = random.Next(1, proper.Count - 1);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                this.TestHelper(proper, tested, list => list.Insert(index, value));
                Assert.AreEqual(value, tested[index]);
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            TestHelper(DoInsert);
        }

        #endregion

        #region Contains

        void DoContains(IList<int> proper, IList<int> tested)
        {
            for (int i = 0; i < 150; i++)
            {
                var value = random.Next();
                Assert.AreEqual(proper.Contains(value), tested.Contains(value));
                Assert.AreEqual(proper.IndexOf(value), tested.IndexOf(value));
            }

            for (int i = 0; i < proper.Count; i++)
                Assert.IsTrue(tested.Contains(tested[i]));

            for (int i = 0; i < proper.Count; i++)
            {
                int index = tested.IndexOf(tested[i]);
                Assert.IsTrue(index >= 0 && index <= i);
            }
        }

        [TestMethod]
        public void TestContains()
        {
            TestHelper(DoContains);
        }

        #endregion

        #region Test Indexer

        void DoIndexer(IList<int> proper, IList<int> tested)
        {
            for (int i = 0; i < 50; i++)
            {
                int value = random.Next();
                int index = random.Next(0, proper.Count - 1);
                this.TestHelper(proper, tested, list => list[index] = value);
            }
        }

        [TestMethod]
        public void TestIndexer()
        {
            TestHelper(DoIndexer);
        }

        #endregion

        #region Cumulative

        void DoCumulativeTest(IList<int> one, IList<int> two)
        {
            var tests =
                new Action<IList<int>, IList<int>>[]
                {
                    DoAdd,
                    DoContains,
                    DoInsert,
                    DoIndexer
                };

            DoCumulativeTest(one, two, tests);
        }

        void DoCumulativeTest(IList<int> one, IList<int> two, Action<IList<int>, IList<int>>[] tests)
        {
            for (int i = 0; i < tests.Length; i++)
            {
                tests[i](one, two);

                DoCumulativeTest(one, two, tests.Take(i).Concat(tests.Skip(Math.Min(i + 1, tests.Length))).ToArray());
            }
        }

        [TestMethod]
        public void CumulativeTest()
        {
            TestHelper(DoCumulativeTest);
        }

        #endregion
    }
}
