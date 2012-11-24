
namespace LordJZ.ObjectManagement
{
    public sealed class SingletonXmlSerializableAsyncObjectAccessor<TObject> :
        AbstractSingletonXmlSerializableAsyncObjectAccessor<SingletonXmlSerializableAsyncObjectAccessor<TObject>, TObject>
        where TObject : class, new()
    {
    }
}
