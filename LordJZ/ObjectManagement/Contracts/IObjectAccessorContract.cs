using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace LordJZ.ObjectManagement.Contracts
{
    [ContractClassFor(typeof(IObjectAccessor))]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract class IObjectAccessorContract : IObjectAccessor
    {
        Type IObjectAccessor.ObjectType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                return null;
            }
        }

        Type IObjectAccessor.KeyType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                return null;
            }
        }

        public abstract object Load(object key);
        public abstract void Save(object key, object obj);
        public abstract object Create();
    }
}
