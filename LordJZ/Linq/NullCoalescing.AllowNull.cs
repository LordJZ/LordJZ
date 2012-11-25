using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            Contract.Ensures(Contract.Result<IList<T>>() != null);

            return target ?? EmptyCollection<T>.Instance;
        }

        public static ICollection<T> AllowNull<T>(this ICollection<T> target)
        {
            Contract.Ensures(Contract.Result<ICollection<T>>() != null);

            return target ?? EmptyCollection<T>.Instance;
        }

        public static IEnumerable<T> AllowNull<T>(this IEnumerable<T> target)
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return target ?? EmptyCollection<T>.Instance;
        }

        public static IList AllowNull(this IList target)
        {
            Contract.Ensures(Contract.Result<IList>() != null);

            return target ?? EmptyCollection.Instance;
        }

        public static ICollection AllowNull(this ICollection target)
        {
            Contract.Ensures(Contract.Result<ICollection>() != null);

            return target ?? EmptyCollection.Instance;
        }

        public static IEnumerable AllowNull(this IEnumerable target)
        {
            Contract.Ensures(Contract.Result<IEnumerable>() != null);

            return target ?? EmptyCollection.Instance;
        }
    }
}
