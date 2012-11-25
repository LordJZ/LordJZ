using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    public static class EmptyArray<T>
    {
        static T[] s_instance;

        public static T[] Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<T[]>() != null);

                return s_instance ?? (s_instance = new T[0]);
            }
        }
    }
}
