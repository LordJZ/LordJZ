using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.ObjectManagement.Contracts
{
    [ContractClassFor(typeof(IObjectFactory))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectFactoryContract : IObjectFactory
    {
        object IObjectFactory.Create()
        {
            Contract.Ensures(Contract.Result<object>() != null);

            return null;
        }
    }

    [ContractClassFor(typeof(IObjectFactory<>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectFactoryContract<TObject>
        : IObjectFactory<TObject> where TObject : class
    {
        TObject IObjectFactory<TObject>.Create()
        {
            Contract.Ensures(Contract.Result<TObject>() != null);

            return null;
        }

        public abstract object Create();
    }

    [ContractClassFor(typeof(IAsyncObjectFactory<>))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IAsyncObjectFactoryContract<TObject>
        : IAsyncObjectFactory<TObject> where TObject : class
    {
        Task<TObject> IAsyncObjectFactory<TObject>.AsyncCreate()
        {
            Contract.Ensures(Contract.Result<Task<TObject>>() != null);

            return null;
        }

        public abstract TObject Create();
        object IObjectFactory.Create()
        {
            return this.Create();
        }
    }
}
