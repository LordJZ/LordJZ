using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class NullCoalescing
    {
        public static NullAllowed<TResult> SelectNullable<T, TResult>(this NullAllowed<T> nullable,
                                                                      Func<T, TResult> selector)
            where T : class
            where TResult : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }

        public static IList<TResult> SelectNullable<T, TResult>(this NullAllowed<T> nullable,
                                                                Func<T, IList<TResult>> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }

        public static ICollection<TResult> SelectNullable<T, TResult>(this NullAllowed<T> nullable,
                                                                      Func<T, ICollection<TResult>> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }

        public static IEnumerable<TResult> SelectNullable<T, TResult>(this NullAllowed<T> nullable,
                                                                      Func<T, IEnumerable<TResult>> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }

        public static IList SelectNullable<T>(this NullAllowed<T> nullable,
                                                       Func<T, IList> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }

        public static ICollection SelectNullable<T>(this NullAllowed<T> nullable,
                                                             Func<T, ICollection> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }

        public static IEnumerable SelectNullable<T>(this NullAllowed<T> nullable,
                                                             Func<T, IEnumerable> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            return nullable.Select(selector).AllowNull();
        }
    }
}
