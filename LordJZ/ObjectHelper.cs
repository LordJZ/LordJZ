using System.Diagnostics.Contracts;
using System.Reflection;

namespace LordJZ
{
    public static class ObjectHelper
    {
        public static void CopyTo<T>(T destination, T source) where T : class
        {
            Contract.Requires(destination != null);
            Contract.Requires(source != null);

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int propertyCount = properties.Length;
            for (int i = 0; i < propertyCount; i++)
            {
                PropertyInfo property = properties[i];
                property.SetValue(destination, property.GetValue(source));
            }
        }
    }
}
