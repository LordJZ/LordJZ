using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    public static class CollectionExtensions
    {
        #region CopyTo

        public static void CopyTo<T>(
            this IReadOnlyCollection<T> source,
            IList<T> list, int listIndex)
        {
            Contract.Requires(source != null);
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex + source.Count <= list.Count);

            CopyTo(source, 0, list, listIndex, source.Count);
        }

        public static void CopyTo<T>(
            this IReadOnlyCollection<T> source,
            IList<T> list, int listIndex,
            int count)
        {
            Contract.Requires(source != null);
            Contract.Requires(count >= 0);
            Contract.Requires(count <= source.Count);
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex + count <= list.Count);

            CopyTo(source, 0, list, listIndex, count);
        }

        public static void CopyTo<T>(
            this IReadOnlyCollection<T> source, int sourceIndex,
            IList<T> list, int listIndex,
            int count)
        {
            Contract.Requires(source != null);
            Contract.Requires(count >= 0);
            Contract.Requires(sourceIndex >= 0);
            Contract.Requires(sourceIndex + count <= source.Count);
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex + count <= list.Count);

            if (count == 0)
                return;

            int skipped = 0;
            int copied = 0;
            foreach (var item in source)
            {
                if (skipped < sourceIndex)
                {
                    ++skipped;
                    continue;
                }

                if (copied < count)
                {
                    list[listIndex + copied] = item;

                    ++copied;
                    continue;
                }

                break;
            }

            Contract.Assert(skipped == sourceIndex);
            Contract.Assert(copied == count);
        }

        #endregion

        #region BinarySearch<IList>

        public static int BinarySearch<TItem, TKey>(this IList<TItem> list, TKey key, Func<TKey, TItem, int> comparer)
        {
            Contract.Requires(list != null);
            Contract.Requires(comparer != null);
            Contract.Ensures(Contract.Result<int>() >= ~list.Count);
            Contract.Ensures(Contract.Result<int>() < list.Count);

            int lower = 0;
            int upper = list.Count - 1;

            while (lower <= upper)
            {
                int middle = lower + (upper - lower) / 2;
                int result = comparer(key, list[middle]);
                if (result < 0)
                    upper = middle - 1;
                else if (result > 0)
                    lower = middle + 1;
                else
                    return middle;
            }

            return ~lower;
        }

        public static int BinarySearch<TItem>(this IList<TItem> list, TItem value)
        {
            return BinarySearch(list, value, Comparer<TItem>.Default);
        }

        public static int BinarySearch<TItem>(this IList<TItem> list, TItem value, IComparer<TItem> comparer)
        {
            return list.BinarySearch(value, comparer.Compare);
        }

        #endregion

        #region BinarySearch<IReadOnlyList>

        public static int BinarySearch<TItem, TKey>(this IReadOnlyList<TItem> list, TKey key, Func<TKey, TItem, int> comparer)
        {
            Contract.Requires(list != null);
            Contract.Requires(comparer != null);
            Contract.Ensures(Contract.Result<int>() >= ~list.Count);
            Contract.Ensures(Contract.Result<int>() < list.Count);

            int lower = 0;
            int upper = list.Count - 1;

            while (lower <= upper)
            {
                int middle = lower + (upper - lower) / 2;
                int result = comparer(key, list[middle]);
                if (result < 0)
                    upper = middle - 1;
                else if (result > 0)
                    lower = middle + 1;
                else
                    return middle;
            }

            return ~lower;
        }

        public static int BinarySearch<TItem>(this IReadOnlyList<TItem> list, TItem value)
        {
            return BinarySearch(list, value, Comparer<TItem>.Default);
        }

        public static int BinarySearch<TItem>(this IReadOnlyList<TItem> list, TItem value, IComparer<TItem> comparer)
        {
            return list.BinarySearch(value, comparer.Compare);
        }

        #endregion

    }
}
