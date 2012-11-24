using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.ObjectManagement.Contracts
{
    [ContractClassFor(typeof(IObjectDeleter))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectDeleterContract : IObjectDeleter
    {
        void IObjectDeleter.Delete(object key)
        {
            Contract.Requires(key != null);
        }
    }

    [ContractClassFor(typeof(IObjectDeleter<>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectDeleterContract<TKey> : IObjectDeleter<TKey>
    {
        void IObjectDeleter<TKey>.Delete(TKey key)
        {
            Contract.Requires(typeof(TKey).IsValueType || key != null);
        }

        public abstract void Delete(object key);
    }

    [ContractClassFor(typeof(IAsyncObjectDeleter<>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IAsyncObjectDeleterContract<TKey> : IAsyncObjectDeleter<TKey>
    {
        Task IAsyncObjectDeleter<TKey>.AsyncDelete(TKey key)
        {
            Contract.Requires(typeof(TKey).IsValueType || key != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            return null;
        }

        public abstract void Delete(TKey key);
        public abstract void Delete(object key);
    }
}
