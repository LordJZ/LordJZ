using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    public static class CollectionExtensions
    {
        public static void CopyTo<T>(
            this IReadOnlyCollection<T> collection, int collectionIndex,
            IList<T> list, int listIndex,
            int count)
        {
            Contract.Requires(collection != null);
            Contract.Requires(count >= 0);
            Contract.Requires(collectionIndex >= 0);
            Contract.Requires(collectionIndex + count <= collection.Count);
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex + count <= list.Count);

            if (count == 0)
                return;

            int skipped = 0;
            int copied = 0;
            foreach (var item in collection)
            {
                if (skipped < collectionIndex)
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

            Contract.Assert(skipped == collectionIndex);
            Contract.Assert(copied == count);
        }
    }
}
