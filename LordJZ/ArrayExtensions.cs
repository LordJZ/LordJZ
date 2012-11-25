using System;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Performs array boundary check that is done even in release mode with full optimizations.
        /// </summary>
        public static void CheckBoundaries<T>(this T[] array, int offset, int count)
        {
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(offset >= 0);
            Contract.Requires(count >= 0);
            Contract.Requires(array.Length - offset >= count);

            // Code below this line must always be executed.
            Contract.EndContractBlock();

            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Rank != 1)
                throw new ArgumentException();

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (array.Length - offset < count)
                throw new ArgumentOutOfRangeException();
        }
    }
}
