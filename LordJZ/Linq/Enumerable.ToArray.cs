using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LordJZ.Collections;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static T[] ToArray<T>(this IEnumerable<T> enumerable)
        {
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<T[]>() != null);

            if (enumerable is EmptyCollection<T>)
                return EmptyArray<T>.Instance;

            if (enumerable is T[])
                return (T[])((T[])enumerable).Clone();

            if (enumerable is IList<T>)
            {
                var list = (IList<T>)enumerable;
                T[] array = new T[list.Count];
                list.CopyTo(array, 0);
                return array;
            }

            if (enumerable is IReadOnlyList<T>)
            {
                var list = (IReadOnlyList<T>)enumerable;
                int count = list.Count;
                T[] array = new T[count];
                for (int i = 0; i < count; i++)
                    array[i] = list[i];
                return array;
            }

            if (enumerable is ICollection<T>)
            {
                var collection = (ICollection<T>)enumerable;
                T[] array = new T[collection.Count];
                int i = -1;
                foreach (var item in enumerable)
                    array[++i] = item;
                return array;
            }

            if (enumerable is IReadOnlyCollection<T>)
            {
                var collection = (IReadOnlyCollection<T>)enumerable;
                T[] array = new T[collection.Count];
                int i = -1;
                foreach (var item in enumerable)
                    array[++i] = item;
                return array;
            }

            // Construct array
            {
                T[] array = null;
                int i = 0;
                int length = 0;

                foreach (var item in enumerable)
                {
                    if (i == length)
                    {
                        Array.Resize(ref array, length = checked(length * 2));
                        Contract.Assert(array.Length == length);
                    }

                    array[i] = item;
                    ++i;
                }

                if (i != length)
                    Array.Resize(ref array, i);

                return array;
            }
        }
    }
}
