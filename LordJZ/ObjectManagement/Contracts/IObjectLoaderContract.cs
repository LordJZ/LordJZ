using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.ObjectManagement.Contracts
{
    [ContractClassFor(typeof(IObjectLoader))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectLoaderContract : IObjectLoader
    {
        object IObjectLoader.Load(object key)
        {
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<object>() != null);

            return null;
        }
    }

    [ContractClassFor(typeof(IObjectLoader<,>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectLoaderContract<TKey, TObject>
        : IObjectLoader<TKey, TObject> where TObject : class
    {
        TObject IObjectLoader<TKey, TObject>.Load(TKey key)
        {
            Contract.Requires(typeof(TKey).IsValueType || key != null);
            Contract.Ensures(Contract.Result<TObject>() != default(TObject));

            return null;
        }

        public abstract object Load(object key);
    }

    [ContractClassFor(typeof(IAsyncObjectLoader<,>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IAsyncObjectLoaderContract<TKey, TObject>
        : IAsyncObjectLoader<TKey, TObject> where TObject : class
    {
        Task<TObject> IAsyncObjectLoader<TKey, TObject>.AsyncLoad(TKey key)
        {
            Contract.Requires(typeof(TKey).IsValueType || key != null);
            Contract.Ensures(Contract.Result<Task<TObject>>() != null);

            return null;
        }

        public abstract TObject Load(TKey key);
        public abstract object Load(object key);
    }
}
