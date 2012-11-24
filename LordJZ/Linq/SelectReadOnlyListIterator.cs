using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    internal sealed class SelectReadOnlyListIterator<T, TResult>
        : SelectReadOnlyCollectionIterator<T, TResult>, IReadOnlyList<TResult>
    {
        public SelectReadOnlyListIterator(IReadOnlyList<T> list,
                                           Func<T, TResult> selector)
            : base(list, selector)
        {
        }

        public TResult this[int index]
        {
            get { return this.Map(((IReadOnlyList<T>)m_enumerable)[index]); }
        }
    }
}
