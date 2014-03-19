using System;
using System.Collections.Generic;

namespace LordJZ.Linq
{
    public delegate TKey KeySelector<in TValue, out TKey>(TValue value);

    /// <param name="haystackItem"></param>
    /// <param name="needleItem"></param>
    /// <param name="key"></param>
    /// <param name="haystackIndex">
    /// Index of the item found in the haystack.
    /// </param>
    /// <param name="needlesIndex">
    /// Index of the item found in the needles.
    /// </param>
    /// <returns></returns>
    public delegate TResult MatchCallback<in THaystack, in TNeedle, in TKey, out TResult>(
        THaystack haystackItem, TNeedle needleItem, TKey key, int haystackIndex, int needlesIndex);

    /// <param name="haystackIndex">
    /// Index of the haystack lowest item that's greater than needle
    /// (i.e. index of the needle if it was in the haystack).
    /// </param>
    /// <param name="needlesIndex">
    /// Index of the item found in the needles.
    /// </param>
    public delegate TResult NeedleMismatchCallback<in TNeedle, in TKey, out TResult>(
        TNeedle needleItem, TKey key, int haystackIndex, int needlesIndex);

    public delegate TResult HaystackMismatchCallback<in THaystack, in TKey, out TResult>(
        THaystack haystackItem, TKey key, int haystackIndex);

    partial class Enumerable
    {
        public static IEnumerable<TResult> OrderedMatch<THaystack, TNeedle, TKey, TResult>(
            this IEnumerable<THaystack> haystack,
            IEnumerable<TNeedle> needles,
            KeySelector<THaystack, TKey> haystackKeySelector,
            KeySelector<TNeedle, TKey> needleKeySelector,
            IComparer<TKey> keyComparer,
            MatchCallback<THaystack, TNeedle, TKey, TResult> matchCallback = null,
            NeedleMismatchCallback<TNeedle, TKey, TResult> needleMismatchCallback = null,
            HaystackMismatchCallback<THaystack, TKey, TResult> haystackMismatchCallback = null)
        {
            using (IEnumerator<THaystack> eHaystack = haystack.GetEnumerator())
            {
                bool haystackEmpty;
                THaystack haystackItem = default(THaystack);
                TKey haystackKey = default(TKey);

                if (!(haystackEmpty = !eHaystack.MoveNext()))
                {
                    haystackItem = eHaystack.Current;
                    haystackKey = haystackKeySelector(haystackItem);
                }

                int haystackIndex = 0;
                int needlesIndex = -1;
                foreach (TNeedle needle in needles)
                {
                    ++needlesIndex;
                    TKey needleKey = needleKeySelector(needle);

                    for (;;)
                    {
                        if (haystackEmpty)
                        {
                            if (needleMismatchCallback != null)
                                yield return needleMismatchCallback(needle, needleKey,
                                                                    haystackIndex, needlesIndex);
                            break;
                        }

                        // check the current haystack element
                        int comparison = keyComparer.Compare(needleKey, haystackKey);

                        if (comparison >= 0)
                        {
                            // needleItem >= haystackItem, always adjust haystack

                            if (comparison == 0)
                            {
                                // found a match
                                if (matchCallback != null)
                                    yield return matchCallback(haystackItem,
                                                               needle, needleKey,
                                                               haystackIndex, needlesIndex);
                            }
                            else // if (comparison > 0)
                            {
                                // unmatched haystack
                                if (haystackMismatchCallback != null)
                                    yield return haystackMismatchCallback(haystackItem, haystackKey, haystackIndex);
                            }

                            // adjust haystack
                            if (!(haystackEmpty = !eHaystack.MoveNext()))
                            {
                                haystackItem = eHaystack.Current;
                                haystackKey = haystackKeySelector(haystackItem);
                            }

                            ++haystackIndex;

                            if (comparison > 0)
                                // not found a match, try another haystack item
                                continue;
                        }
                        else
                        {
                            // needleItem < haystackItem, no haystack item matches this needle
                            // do not adjust haystack
                            if (needleMismatchCallback != null)
                                yield return needleMismatchCallback(needle, needleKey,
                                                                    haystackIndex, needlesIndex);
                        }

                        break;
                    }
                }

                if (!haystackEmpty && haystackMismatchCallback != null)
                {
                    while (eHaystack.MoveNext())
                    {
                        haystackItem = eHaystack.Current;
                        haystackKey = haystackKeySelector(haystackItem);
                        ++haystackIndex;

                        haystackMismatchCallback(haystackItem, haystackKey, haystackIndex);
                    }
                }
            }
        }
    }
}
