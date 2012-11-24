using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class TupleEx
    {
        public static T[] ToArray<T>(this Tuple<T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 1);

            return new[] { tuple.Item1 };
        }

        public static T[] ToArray<T>(this Tuple<T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 2);

            return new[] { tuple.Item1, tuple.Item2 };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 3);

            return new[] { tuple.Item1, tuple.Item2, tuple.Item3 };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 4);

            return new[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4 };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 5);

            return new[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5 };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 6);

            return new[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6 };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 7);

            return new[]
                   {
                       tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5,
                       tuple.Item6, tuple.Item7
                   };
        }

        public static T[] ToArray<T>(this Tuple<T, T, T, T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Ensures(Contract.Result<T[]>().Length == 8);

            return new[]
                   {
                       tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5,
                       tuple.Item6, tuple.Item7, tuple.Rest
                   };
        }
    }
}
