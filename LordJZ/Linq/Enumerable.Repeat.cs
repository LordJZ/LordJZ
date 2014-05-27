using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        class RepeatIterator<T, TResult> : Iterator<TResult>, IReadOnlyCollection<TResult>, IReadOnlyList<TResult>
        {
            readonly T m_item;
            readonly int m_count;
            readonly Func<T, TResult> m_selector;
            int m_current;

            public RepeatIterator(int mCount, T mItem, Func<T, TResult> mSelector)
            {
                m_count = mCount;
                m_item = mItem;
                m_selector = mSelector;
                m_current = -1;
            }

            public override object Clone()
            {
                RepeatIterator<T, TResult> iterator = (RepeatIterator<T, TResult>)base.Clone();
                iterator.m_current = -1;
                return iterator;
            }

            public override bool MoveNext()
            {
                return ++m_current < m_count;
            }

            public override TResult Current
            {
                get { return m_selector(m_item); }
            }

            protected internal override IEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new RepeatIterator<T, TResult1>(m_count, m_item, CombineSelectors(m_selector, selector));
            }

            protected internal override IEnumerable<TResult> Where<TResult1>(Predicate<TResult> predicate)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return m_count; }
            }

            public TResult this[int index]
            {
                get { return m_selector(m_item); }
            }
        }

        public static IReadOnlyCollection<T> Repeat<T>(T element, int times)
        {
            return new RepeatIterator<T, T>(times, element, _ => _);
        }
    }
}
