using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static List<T> ToList<T>(this IEnumerable<T> enumerable)
        {
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<List<T>>() != null);

            int count;
            if (enumerable is T[])
                count = ((T[])enumerable).Length;
            else if (enumerable is IReadOnlyCollection<T>)
                count = ((IReadOnlyCollection<T>)enumerable).Count;
            else if (enumerable is ICollection<T>)
                count = ((ICollection<T>)enumerable).Count;
            else
                count = 0;

            List<T> list = new List<T>();

            if (count != 0 && list.Capacity < count)
                list.Capacity = count;

            list.AddRange(enumerable);
            return list;
        }
    }
}
