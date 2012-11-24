
namespace LordJZ
{
    public class EnumMemberInfo<T> : IEnumMemberHelper
    {
        public EnumMemberInfo(string name, object value, T member)
        {
            this.Name = name;
            this.Value = member;
            this.BoxedValue = value;
        }

        public string Name { get; private set; }
        public T Value { get; private set; }
        public object BoxedValue { get; private set; }

        object IEnumMemberHelper.Value
        {
            get { return this.BoxedValue; }
        }
    }
}
