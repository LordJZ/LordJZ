using LordJZ.ObjectManagement.Contracts;
using System;
using System.Diagnostics.Contracts;

namespace LordJZ.ObjectManagement
{
    /// <summary>
    /// Represents an object that is used to access another objects.
    /// </summary>
    [ContractClass(typeof(IObjectAccessorContract))]
    public interface IObjectAccessor : IObjectFactory, IObjectLoader, IObjectSaver
    {
        /// <summary>
        /// Gets the type of the objects that are accessed
        /// via this instance of <see cref="IObjectAccessor"/>.
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Gets the type of keys the objects are accessed with
        /// via this instance of <see cref="IObjectAccessor"/>.
        /// </summary>
        Type KeyType { get; }
    }

    public interface IObjectAccessor<in TKey, TObject> : IObjectAccessor,
        IObjectDeleter<TKey>, IObjectFactory<TObject>,
        IObjectLoader<TKey, TObject>, IObjectSaver<TKey, TObject>
        where TObject : class
    {
    }

    public interface IAsyncObjectAccessor<in TKey, TObject> : IObjectAccessor<TKey, TObject>,
        IAsyncObjectDeleter<TKey>, IAsyncObjectFactory<TObject>,
        IAsyncObjectLoader<TKey, TObject>, IAsyncObjectSaver<TKey, TObject>
        where TObject : class
    {
    }
}
