using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    internal class SelectListIterator<T, TResult>
        : SelectCollectionIterator<T, TResult>, IReadOnlyList<TResult>
    {
        public SelectListIterator(IList<T> list, Func<T, TResult> selector)
            : base(list, selector)
        {
        }

        public TResult this[int index]
        {
            get { return this.Map(((IList<T>)m_enumerable)[index]); }
        }
    }
}
