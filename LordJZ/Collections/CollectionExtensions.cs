using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    public static class CollectionExtensions
    {
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
    }
}
