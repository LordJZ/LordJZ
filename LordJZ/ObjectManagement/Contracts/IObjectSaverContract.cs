using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.ObjectManagement.Contracts
{
    [ContractClassFor(typeof(IObjectSaver))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectSaverContract : IObjectSaver
    {
        void IObjectSaver.Save(object key, object obj)
        {
            Contract.Requires(key != null);
            Contract.Requires(obj != null);
        }
    }

    [ContractClassFor(typeof(IObjectSaver<,>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectSaverContract<TKey, TObject>
        : IObjectSaver<TKey, TObject> where TObject : class
    {
        void IObjectSaver<TKey, TObject>.Save(TKey key, TObject obj)
        {
            Contract.Requires(obj != default(TObject));
            Contract.Requires(typeof(TKey).IsValueType || key != null);
        }

        public abstract void Save(object key, object obj);
    }

    [ContractClassFor(typeof(IAsyncObjectSaver<,>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IAsyncObjectSaverContract<TKey, TObject>
        : IAsyncObjectSaver<TKey, TObject> where TObject : class
    {
        Task IAsyncObjectSaver<TKey, TObject>.AsyncSave(TKey key, TObject obj)
        {
            Contract.Requires(obj != default(TObject));
            Contract.Requires(typeof(TKey).IsValueType || key != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            return null;
        }

        public abstract void Save(TKey key, TObject obj);
        public abstract void Save(object key, object obj);
    }
}
