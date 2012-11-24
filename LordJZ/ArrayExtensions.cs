using System;

namespace LordJZ
{
    public static class ArrayExtensions
    {
        public static void CheckBoundaries<T>(this T[] array, int offset, int count)
        {
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
