using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static IEnumerable<TResult> OrderedMatch<THaystack, TNeedle, TKey, TResult>(
            this IEnumerable<THaystack> haystack,
            IEnumerable<TNeedle> needles,
            Func<THaystack, TKey> haystackKeySelector,
            Func<TNeedle, TKey> needleKeySelector,
            IComparer<TKey> keyComparer,
            Func<THaystack, TNeedle, TKey, TResult> matchedResultSelector,
            Func<TNeedle, TKey, TResult> mismatchedResultSelector)
        {
            IEnumerator<THaystack> eHaystack = haystack.GetEnumerator();
            bool haystackEmpty;
            THaystack haystackItem = default(THaystack);
            TKey haystackKey = default(TKey);

            if (!(haystackEmpty = !eHaystack.MoveNext()))
            {
                haystackItem = eHaystack.Current;
                haystackKey = haystackKeySelector(haystackItem);
            }

            foreach (TNeedle needle in needles)
            {
                TKey needleKey = needleKeySelector(needle);

                for (;;)
                {
                    if (haystackEmpty)
                    {
                        yield return mismatchedResultSelector(needle, needleKey);
                        break;
                    }

                    // check the current haystack element
                    int comparison = keyComparer.Compare(needleKey, haystackKey);

                    if (comparison >= 0)
                    {
                        // needleItem >= haystackItem, always adjust haystack

                        if (comparison == 0)
                            // found a match
                            yield return matchedResultSelector(haystackItem, needle, needleKey);

                        // adjust haystack
                        if (!(haystackEmpty = !eHaystack.MoveNext()))
                        {
                            haystackItem = eHaystack.Current;
                            haystackKey = haystackKeySelector(haystackItem);
                        }

                        if (comparison > 0)
                            // not found a match, try another haystack item
                            continue;
                    }
                    else
                        // needleItem < haystackItem, no haystack item matches this needle
                        // do not adjust haystack
                        yield return mismatchedResultSelector(needle, needleKey);

                    break;
                }
            }
        }
    }
}
