using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections.Proxies
{
    public sealed class EnumerableProxy : IEnumerable, IObjectProxy<IEnumerable>
    {
        readonly IEnumerable m_enumerable;

        public EnumerableProxy(IEnumerable enumerable)
        {
            Contract.Requires(enumerable != null);

            this.m_enumerable = enumerable;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_enumerable.GetEnumerator();
        }

        object IObjectProxy.ProxifiedObject
        {
            get { return this.ProxifiedObject; }
        }

        public IEnumerable ProxifiedObject { get { return this.m_enumerable; } }
    }

    public sealed class EnumerableProxy<T> : IEnumerable<T>, IObjectProxy<IEnumerable<T>>
    {
        readonly IEnumerable<T> m_enumerable;

        public EnumerableProxy(IEnumerable<T> enumerable)
        {
            Contract.Requires(enumerable != null);

            this.m_enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.m_enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.m_enumerable).GetEnumerator();
        }

        object IObjectProxy.ProxifiedObject
        {
            get { return this.ProxifiedObject; }
        }

        public IEnumerable<T> ProxifiedObject { get { return this.m_enumerable; } }
    }
}
