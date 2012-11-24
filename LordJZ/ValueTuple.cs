
namespace LordJZ
{
    public static class ValueTuple
    {
        public static ValueTuple<T> Create<T>(T item1)
        {
            return new ValueTuple<T>(item1);
        }

        public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new ValueTuple<T1, T2>(item1, item2);
        }

        public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new ValueTuple<T1, T2, T3>(item1, item2, item3);
        }
    }

    /// <summary>
    /// Represents a 1-tuple, or singleton.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the tuple's only component.
    /// </typeparam>
    public struct ValueTuple<T>
    {
        /// <summary>
        /// Creates a new 1-tuple, or singleton.
        /// </summary>
        /// <param name="item1">
        /// The value of the only component of the tuple.
        /// </param>
        public ValueTuple(T item1)
        {
            this.Item1 = item1;
        }

        public readonly T Item1;
    }

    public struct ValueTuple<T1, T2>
    {
        public ValueTuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public readonly T1 Item1;
        public readonly T2 Item2;
    }

    public struct ValueTuple<T1, T2, T3>
    {
        public ValueTuple(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }

        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
    }
}
