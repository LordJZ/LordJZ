
namespace LordJZ.ObjectManagement
{
    public abstract class XmlSerializable<T> where T : XmlSerializable<T>, new()
    {
        public static IAsyncObjectAccessor<string, T> Accessor
        {
            get { return SingletonXmlSerializableAsyncObjectAccessor<T>.Instance; }
        }
    }
}
