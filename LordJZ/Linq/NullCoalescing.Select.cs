using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class NullCoalescing
    {
        public static TResult Select<T, TResult>(this NullAllowed<T> nullable, Func<T, TResult> selector)
            where T : class
        {
            Contract.Requires(selector != null);

            T target = nullable.Target;
            if (target == null)
                return default(TResult);

            return selector(target);
        }
    }
}
