
namespace LordJZ.Collections
{
    public static class EmptyArray<T>
    {
        static T[] s_instance;

        public static T[] Instance
        {
            get { return s_instance ?? (s_instance = new T[0]); }
        }
    }
}
