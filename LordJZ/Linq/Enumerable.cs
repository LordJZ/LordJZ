using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    public static partial class Enumerable
    {
        public static void Enumerate<T>(this IEnumerable<T> collection)
        {
            Contract.Requires(collection != null);

            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                // do nothing
            }
        }
    }
}
