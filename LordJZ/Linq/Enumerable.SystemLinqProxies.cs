using System;
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
    }
}
