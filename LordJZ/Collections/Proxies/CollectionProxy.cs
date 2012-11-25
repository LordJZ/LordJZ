using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LordJZ.ObjectManagement;

namespace LordJZ.Collections.Proxies
{
    public sealed class CollectionProxy : ICollection, IObjectProxy<ICollection>
    {
        readonly ICollection m_collection;

        public CollectionProxy(ICollection collection)
        {
            Contract.Requires(collection != null);

            this.m_collection = collection;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_collection.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            this.m_collection.CopyTo(array, index);
        }

        public int Count { get { return this.m_collection.Count; } }
        public object SyncRoot { get { return this.m_collection.SyncRoot; } }
        public bool IsSynchronized { get { return this.m_collection.IsSynchronized; } }

        object IObjectProxy.ProxifiedObject
        {
            get { return this.ProxifiedObject; }
        }

        public ICollection ProxifiedObject { get { return this.m_collection; } }
    }

    public sealed class CollectionProxy<T> : ICollection<T>, IObjectProxy<ICollection<T>>
    {
        readonly ICollection<T> m_collection;

        public CollectionProxy(ICollection<T> collection)
        {
            Contract.Requires(collection != null);

            this.m_collection = collection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.m_collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.m_collection).GetEnumerator();
        }

        public void Add(T item)
        {
            this.m_collection.Add(item);
        }

        public void Clear()
        {
            this.m_collection.Clear();
        }

        public bool Contains(T item)
        {
            return this.m_collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.m_collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return this.m_collection.Remove(item);
        }

        public int Count { get { return this.m_collection.Count; } }
        public bool IsReadOnly { get { return this.m_collection.IsReadOnly; } }

        object IObjectProxy.ProxifiedObject
        {
            get { return this.ProxifiedObject; }
        }

        public ICollection<T> ProxifiedObject { get { return this.m_collection; } }
    }
}
