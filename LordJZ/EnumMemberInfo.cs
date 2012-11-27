using System.Diagnostics.Contracts;

namespace LordJZ
{
    public class EnumMemberInfo<T> : IEnumMemberInfo
    {
        public EnumMemberInfo(string name, object value, T member)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(value != null);

            this.Name = name;
            this.Value = member;
            this.BoxedValue = value;
        }

        public string Name { get; private set; }
        public T Value { get; private set; }
        public object BoxedValue { get; private set; }

        object IEnumMemberInfo.Value
        {
            get { return this.BoxedValue; }
        }
    }
}
