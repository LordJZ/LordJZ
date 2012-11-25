using System.Diagnostics.Contracts;

namespace LordJZ
{
    public static class BooleanBoxes
    {
        public static readonly object True = true;
        public static readonly object False = false;

        [Pure]
        public static object Box(bool value)
        {
            Contract.Ensures(Contract.Result<object>() != null);

            return value ? True : False;
        }
    }
}
