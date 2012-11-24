using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class NullCoalescing
    {
        public static NullAllowed<T> From<T>(this T target) where T : class
        {
            Contract.Requires(target != null);
            Contract.Ensures(Contract.Result<NullAllowed<T>>().Target != null);

            return new NullAllowed<T>(target);
        }
    }
}
