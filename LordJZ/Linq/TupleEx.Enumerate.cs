using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    partial class TupleEx
    {
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T, T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static IEnumerable<T> Enumerate<T>(this Tuple<T, T, T, T, T, T, T, T> tuple)
        {
            Contract.Requires(tuple != null);

            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
            yield return tuple.Item4;
            yield return tuple.Item5;
            yield return tuple.Item6;
            yield return tuple.Item7;
            yield return tuple.Rest;
        }
    }
}
