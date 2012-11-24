using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;
using LordJZ.Collections;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static IEnumerable<TResult> Select<T, TResult>(this IEnumerable<T> enumerable,
                                                              Func<T, TResult> selector)
        {
            return InternalSelect(enumerable, selector);
        }

        public static IReadOnlyCollection<TResult> Select<T, TResult>(this IReadOnlyCollection<T> collection,
                                                                      Func<T, TResult> selector)
        {
            return (IReadOnlyCollection<TResult>)InternalSelect(collection, selector);
        }

        public static IReadOnlyList<TResult> Select<T, TResult>(this IReadOnlyList<T> list,
                                                                Func<T, TResult> selector)
        {
            return (IReadOnlyList<TResult>)InternalSelect(list, selector);
        }

        static IEnumerable<TResult> InternalSelect<T, TResult>(this IEnumerable<T> enumerable,
                                                               Func<T, TResult> selector)
        {
            Contract.Requires(enumerable != null);
            Contract.Requires(selector != null);

            if (enumerable is EmptyCollection<T>)
                return EmptyCollection<TResult>.Instance;

            if (enumerable is Iterator<T>)
                return ((Iterator<T>)enumerable).Select(selector);

            if (enumerable is IReadOnlyList<T>)
                return new SelectReadOnlyListIterator<T, TResult>((IReadOnlyList<T>)enumerable, selector);

            if (enumerable is IReadOnlyCollection<T>)
                return new SelectReadOnlyCollectionIterator<T, TResult>(
                    (IReadOnlyCollection<T>)enumerable, selector);

            if (enumerable is IList<T>)
                return new SelectListIterator<T, TResult>((IList<T>)enumerable, selector);

            if (enumerable is ICollection<T>)
                return new SelectCollectionIterator<T, TResult>((ICollection<T>)enumerable, selector);

            return new SelectEnumerableIterator<T, TResult>(enumerable, selector);
        }
    }
}
