using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    internal class SelectReadOnlyCollectionIterator<T, TResult>
        : SelectEnumerableIterator<T, TResult>, IReadOnlyCollection<TResult>
    {
        public SelectReadOnlyCollectionIterator(IReadOnlyCollection<T> collection,
                                                 Func<T, TResult> selector)
            : base(collection, selector)
        {
        }

        public int Count { get { return ((IReadOnlyCollection<T>)m_enumerable).Count; } }
    }
}
