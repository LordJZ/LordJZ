using System.Collections;
using System.Collections.Generic;

namespace LordJZ.Collections
{
    public struct InfiniteEnumerable<T> : IEnumerable<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            readonly T m_value;

            internal Enumerator(T value)
            {
                m_value = value;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return true;
            }

            public void Reset()
            {
            }

            public T Current
            {
                get { return m_value; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }
        }

        readonly T m_value;

        public InfiniteEnumerable(T value)
        {
            m_value = value;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_value);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
