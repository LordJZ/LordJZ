using System.Collections;
using System.Collections.Generic;
using LordJZ.Collections;

namespace LordJZ.Linq
{
    partial class NullCoalescing
    {
        public static NullAllowed<T> AllowNull<T>(this T target) where T : class
        {
            return new NullAllowed<T>(target);
        }

        public static IList<T> AllowNull<T>(this IList<T> target)
        {
            return target ?? EmptyCollection<T>.Instance;
        }

        public static ICollection<T> AllowNull<T>(this ICollection<T> target)
        {
            return target ?? EmptyCollection<T>.Instance;
        }

        public static IEnumerable<T> AllowNull<T>(this IEnumerable<T> target)
        {
            return target ?? EmptyCollection<T>.Instance;
        }

        public static IList AllowNull(this IList target)
        {
            return target ?? EmptyCollection.Instance;
        }

        public static ICollection AllowNull(this ICollection target)
        {
            return target ?? EmptyCollection.Instance;
        }

        public static IEnumerable AllowNull(this IEnumerable target)
        {
            return target ?? EmptyCollection.Instance;
        }
    }
}
