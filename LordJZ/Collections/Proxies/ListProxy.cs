using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LordJZ.ObjectManagement;

namespace LordJZ.Collections.Proxies
{
    public sealed class ListProxy : IList, IObjectProxy<IList>
    {
        readonly IList m_list;

        public ListProxy(IList list)
        {
            Contract.Requires(list != null);

            this.m_list = list;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_list.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            this.m_list.CopyTo(array, index);
        }

        public int Count { get { return this.m_list.Count; } }
        public object SyncRoot { get { return this.m_list.SyncRoot; } }
        public bool IsSynchronized { get { return this.m_list.IsSynchronized; } }

        object IObjectProxy.ProxifiedObject
        {
            get { return this.ProxifiedObject; }
        }

        public IList ProxifiedObject { get { return this.m_list; } }

        public int Add(object value)
        {
            return this.m_list.Add(value);
        }

        public bool Contains(object value)
        {
            return this.m_list.Contains(value);
        }

        public void Clear()
        {
            this.m_list.Clear();
        }

        public int IndexOf(object value)
        {
            return this.m_list.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            this.m_list.Insert(index, value);
        }

        public void Remove(object value)
        {
            this.m_list.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_list.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return this.m_list[index]; }
            set { this.m_list[index] = value; }
        }

        public bool IsReadOnly { get { return this.m_list.IsReadOnly; } }
        public bool IsFixedSize { get { return this.m_list.IsFixedSize; } }
    }

    public sealed class ListProxy<T> : IList<T>, IObjectProxy<IList<T>>
    {
        readonly IList<T> m_list;

        public ListProxy(IList<T> list)
        {
            Contract.Requires(list != null);

            this.m_list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.m_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.m_list).GetEnumerator();
        }

        public void Add(T item)
        {
            this.m_list.Add(item);
        }

        public void Clear()
        {
            this.m_list.Clear();
        }

        public bool Contains(T item)
        {
            return this.m_list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.m_list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return this.m_list.Remove(item);
        }

        public int Count { get { return this.m_list.Count; } }
        public bool IsReadOnly { get { return this.m_list.IsReadOnly; } }

        object IObjectProxy.ProxifiedObject
        {
            get { return this.ProxifiedObject; }
        }

        public IList<T> ProxifiedObject { get { return this.m_list; } }

        public int IndexOf(T item)
        {
            return this.m_list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.m_list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.m_list.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return this.m_list[index]; }
            set { this.m_list[index] = value; }
        }
    }
}
