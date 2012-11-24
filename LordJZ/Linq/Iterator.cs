using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace LordJZ.Linq
{
    /// <summary>
    /// An iterator is an object that is both an enumerable, and an enumerator.
    /// When enumeration happens during another enumeration, the Iterator clones itself.
    /// </summary>
    internal abstract class Iterator<T> : IEnumerable<T>, IEnumerator<T>, ICloneable
    {
        int m_enumerating;

        public IEnumerator<T> GetEnumerator()
        {
            int isEnumerating = Interlocked.CompareExchange(ref m_enumerating, 1, 0);
            if (isEnumerating != 0)
                return ((Iterator<T>)this.Clone()).GetEnumerator();

            this.StartEnumeration();
            return this;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual object Clone()
        {
            var obj = (Iterator<T>)this.MemberwiseClone();
            obj.m_enumerating = 0;
            return obj;
        }

        public abstract bool MoveNext();

        public virtual void StartEnumeration()
        {
        }

        public virtual void Reset()
        {
            m_enumerating = 0;
        }

        public abstract T Current { get; }

        void IDisposable.Dispose()
        {
            this.Reset();
        }

        object IEnumerator.Current { get { return this.Current; } }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected internal abstract IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector);

        protected internal abstract IEnumerable<T> Where<TResult>(Predicate<T> predicate);
    }
}
