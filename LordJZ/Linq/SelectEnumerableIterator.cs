using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    internal class SelectEnumerableIterator<T, TResult> : Iterator<TResult>
    {
        protected readonly IEnumerable<T> m_enumerable;
        readonly Func<T, TResult> m_selector;
        IEnumerator<T> m_enumerator;

        public SelectEnumerableIterator(IEnumerable<T> enumerable, Func<T, TResult> selector)
        {
            Contract.Requires(enumerable != null);
            Contract.Requires(selector != null);

            m_enumerable = enumerable;
            m_selector = selector;
        }

        public override void StartEnumeration()
        {
            base.StartEnumeration();

            m_enumerator = m_enumerable.GetEnumerator();

            Contract.Assert(m_enumerator != null);
        }

        public override void Reset()
        {
            base.Reset();

            m_enumerator = null;
        }

        public override TResult Current
        {
            get
            {
                this.CheckEnumerator();

                return m_selector(m_enumerator.Current);
            }
        }

        public override bool MoveNext()
        {
            this.CheckEnumerator();

            return m_enumerator.MoveNext();
        }

        void CheckEnumerator()
        {
            if (m_enumerator == null)
                throw new InvalidOperationException();
        }

        public override object Clone()
        {
            var obj = (SelectEnumerableIterator<T, TResult>)base.Clone();
            obj.m_enumerator = null;
            return obj;
        }

        protected TResult Map(T value)
        {
            return m_selector(value);
        }

        protected internal override IEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
        {
            return m_enumerable.Select(Enumerable.CombineSelectors(m_selector, selector));
        }

        protected internal override IEnumerable<TResult> Where<TResult1>(Predicate<TResult> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
