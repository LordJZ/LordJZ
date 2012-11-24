using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.Collections.Proxies
{
    public sealed class ReadOnlyListProxy<T> : IReadOnlyList<T>, IObjectProxy<IReadOnlyList<T>>
    {
        readonly IReadOnlyList<T> m_list;

        public ReadOnlyListProxy(IReadOnlyList<T> list)
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
            return this.GetEnumerator();
        }

        public int Count { get { return this.m_list.Count; } }

        public T this[int index]
        {
            get { return this.m_list[index]; }
        }

        object IObjectProxy.ProxifiedObject
        {
            get { return ProxifiedObject; }
        }

        public IReadOnlyList<T> ProxifiedObject { get { return this.m_list; } }
    }
}
