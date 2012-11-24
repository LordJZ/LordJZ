using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    internal class SelectCollectionIterator<T, TResult>
        : SelectEnumerableIterator<T, TResult>, IReadOnlyCollection<TResult>
    {
        public SelectCollectionIterator(ICollection<T> collection, Func<T, TResult> selector)
            : base(collection, selector)
        {
        }

        public int Count { get { return ((ICollection<T>)m_enumerable).Count; } }
    }
}
