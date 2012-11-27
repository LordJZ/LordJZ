using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class NullCoalescing
    {
        public static void Do<T>(this NullAllowed<T> nullable, Action<T> action)
            where T : class
        {
            Contract.Requires(action != null);

            T target = nullable.Target;
            if (target != null)
                action(target);
        }
    }
}
