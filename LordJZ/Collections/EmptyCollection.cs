using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    public sealed class EmptyCollection : IList, IEnumerator
    {
        /// <summary>
        /// Returns zero.
        /// </summary>
        public int Count { get { return 0; } }

        static EmptyCollection s_instance;

        public static EmptyCollection Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<EmptyCollection>() != null);

                return s_instance ?? (s_instance = new EmptyCollection());
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        void ICollection.CopyTo(Array array, int index)
        {
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return false;
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        int IList.IndexOf(object value)
        {
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        object IList.this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        bool IList.IsFixedSize
        {
            get { return true; }
        }

        bool IEnumerator.MoveNext()
        {
            return false;
        }

        void IEnumerator.Reset()
        {
        }

        object IEnumerator.Current
        {
            get { throw new NotSupportedException(); }
        }
    }
}
