using System.IO;
using System.Xml.Serialization;

namespace LordJZ.ObjectManagement
{
    public abstract class AbstractSingletonXmlSerializableAsyncObjectAccessor<T, TObject>
        : SingletonAsyncObjectAccessor<T, string, TObject>
        where T : AbstractSingletonXmlSerializableAsyncObjectAccessor<T, TObject>, new()
        where TObject : class, new()
    {
        readonly LazyWeak<XmlSerializer> m_ser
            = new LazyWeak<XmlSerializer>(() => new XmlSerializer(typeof(TObject)));

        public override void Delete(string key)
        {
            IOHelper.PerformIOOperation(() => File.Delete(key));
        }

        public override TObject Create()
        {
            return new TObject();
        }

        public override void Save(string key, TObject info)
        {
            var data = this.InternalSave(info);
            IOHelper.PerformIOOperation(() => File.WriteAllBytes(key, data));
        }

        public override TObject Load(string key)
        {
            var data = IOHelper.PerformIOOperation(() => File.ReadAllBytes(key));
            return this.InternalLoad(data);
        }

        byte[] InternalSave(TObject info)
        {
            using (var stream = new MemoryStream())
            {
                this.m_ser.Target.Serialize(stream, info);
                return stream.ToArray();
            }
        }

        TObject InternalLoad(byte[] data)
        {
            using (var stream = new MemoryStream(data))
                return (TObject)this.m_ser.Target.Deserialize(stream);
        }
    }
}
