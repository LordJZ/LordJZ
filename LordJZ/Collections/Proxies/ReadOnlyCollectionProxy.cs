using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.Collections.Proxies
{
    public sealed class ReadOnlyCollectionProxy<T> : IReadOnlyCollection<T>,
        IObjectProxy<IReadOnlyCollection<T>>
    {
        readonly IReadOnlyCollection<T> m_collection;

        public ReadOnlyCollectionProxy(IReadOnlyCollection<T> collection)
        {
            Contract.Requires(collection != null);

            m_collection = collection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count { get { return m_collection.Count; } }

        object IObjectProxy.ProxifiedObject
        {
            get { return ProxifiedObject; }
        }

        public IReadOnlyCollection<T> ProxifiedObject { get { return m_collection; } }
    }
}
