using System;
using System.Collections;
using System.Collections.Generic;

namespace LordJZ.Collections
{
    public sealed class EmptyCollection<T> : IList<T>, IReadOnlyList<T>, IEnumerator<T>
    {
        /// <summary>
        /// Returns zero.
        /// </summary>
        public int Count
        {
            get { return 0; }
        }

        static EmptyCollection<T> s_instance;

        public static EmptyCollection<T> Instance
        {
            get { return s_instance ?? (s_instance = new EmptyCollection<T>()); }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
        }

        bool ICollection<T>.Remove(T item)
        {
            return false;
        }

        int ICollection<T>.Count
        {
            get { return 0; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        int IList<T>.IndexOf(T item)
        {
            return -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
        }

        T IList<T>.this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        int IReadOnlyCollection<T>.Count
        {
            get { return 0; }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get { throw new NotSupportedException(); }
        }

        void IDisposable.Dispose()
        {
        }

        bool IEnumerator.MoveNext()
        {
            return false;
        }

        void IEnumerator.Reset()
        {
        }

        T IEnumerator<T>.Current
        {
            get { throw new NotSupportedException(); }
        }

        object IEnumerator.Current
        {
            get { throw new NotSupportedException(); }
        }
    }
}
