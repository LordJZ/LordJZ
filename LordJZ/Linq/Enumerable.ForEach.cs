using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LordJZ.Collections;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Contract.Requires(enumerable != null);
            Contract.Requires(action != null);

            if (enumerable is EmptyCollection<T>)
                return;

            if (enumerable is T[])
            {
                ForEach((T[])enumerable, action);
                return;
            }

            if (enumerable is IReadOnlyList<T>)
            {
                ForEach((IReadOnlyList<T>)enumerable, action);
                return;
            }

            if (enumerable is IList<T>)
            {
                ForEach((IList<T>)enumerable, action);
                return;
            }

            foreach (var item in enumerable)
                action(item);
        }

        public static void ForEach<T>(this IReadOnlyList<T> list, Action<T> action)
        {
            Contract.Requires(list != null);
            Contract.Requires(action != null);

            for (int i = 0; i < list.Count; i++)
                action(list[i]);
        }

        private static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            Contract.Requires(list != null);
            Contract.Requires(action != null);

            for (int i = 0; i < list.Count; i++)
                action(list[i]);
        }

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            Contract.Requires(array != null);
            Contract.Requires(action != null);

            for (int i = 0; i < array.Length; i++)
                action(array[i]);
        }
    }
}
