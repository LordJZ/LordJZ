using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    public struct SimpleLinkedList<T> : IEnumerable<T> where T : class
    {
        public struct Enumerator : IEnumerator<T>
        {
            T m_item;
            readonly Func<T, T> m_nextSelector;
            bool m_enumerationStarted;

            internal Enumerator(T firstItem, Func<T, T> selector)
            {
                Contract.Requires(selector != null);

                m_item = firstItem;
                m_nextSelector = selector;
                m_enumerationStarted = false;
            }

            void IDisposable.Dispose()
            {
            }

            public bool MoveNext()
            {
                if (!m_enumerationStarted)
                    m_enumerationStarted = true;
                else
                    m_item = m_nextSelector(m_item);

                return m_item != null;
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            public T Current
            {
                get { return m_item; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }
        }

        readonly T m_firstItem;
        readonly Func<T, T> m_nextSelector;

        public SimpleLinkedList(T firstItem, Func<T, T> nextSelector)
        {
            Contract.Requires(nextSelector != null);

            m_firstItem = firstItem;
            m_nextSelector = nextSelector;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_firstItem, m_nextSelector);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
