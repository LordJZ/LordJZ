using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class NullCoalescing
    {
        [Pure]
        public static NullAllowed<T> From<T>(T target) where T : class
        {
            Contract.Requires(target != null);
            Contract.Ensures(Contract.Result<NullAllowed<T>>().Target != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<NullAllowed<T>>().Target, target));

            return new NullAllowed<T>(target);
        }
    }
}
