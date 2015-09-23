using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return System.Linq.Enumerable.SequenceEqual(first, second);
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer)
        {
            return System.Linq.Enumerable.SequenceEqual(first, second, comparer);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return System.Linq.Enumerable.Empty<T>();
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains elements from the input sequence that satisfy the condition.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to filter.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition.</param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.
        /// </exception>
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Where(source, predicate);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains elements from the input sequence that satisfy the condition.
        /// </returns>
        /// <param name="source">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to filter.
        /// </param>
        /// <param name="predicate">
        /// A function to test each source element for a condition;
        /// the second parameter of the function represents the index of the source element.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.
        /// </exception>
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return System.Linq.Enumerable.Where(source, predicate);
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.Any(source);
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Any(source, predicate);
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.FirstOrDefault(source);
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.FirstOrDefault(source, predicate);
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.First(source);
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.First(source, predicate);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.OrderBy(source, keySelector);
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.ThenBy(source, keySelector);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.OrderBy(source, keySelector, comparer);
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.ThenBy(source, keySelector, comparer);
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.OrderByDescending(source, keySelector);
        }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.ThenByDescending(source, keySelector);
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.OrderByDescending(source, keySelector, comparer);
        }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.ThenByDescending(source, keySelector, comparer);
        }

	    public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
	    {
		    return System.Linq.Enumerable.SelectMany(source, selector);
	    }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return System.Linq.Enumerable.SelectMany(source, selector);
        }
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return System.Linq.Enumerable.SelectMany(source, collectionSelector, resultSelector);

        }

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
	    {
		    return System.Linq.Enumerable.SelectMany(source, collectionSelector, resultSelector);
	    }
    }
}
