using LordJZ.ObjectManagement.Contracts;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.ObjectManagement
{
    /// <summary>
    /// Defines a method to load an object by its key.
    /// </summary>
    [ContractClass(typeof(IObjectLoaderContract))]
    public interface IObjectLoader
    {
        /// <summary>
        /// Loads an object by the specified key.
        /// </summary>
        /// <param name="key">
        /// Key of the object to load.
        /// </param>
        /// <returns>
        /// An object that was loaded.
        /// </returns>
        /// <exception cref="CannotLoadObjectException">
        /// The object cannot be loaded.
        /// </exception>
        object Load(object key);
    }

    /// <summary>
    /// Defines a method to load on object
    /// of type TObject by its key of type TKey.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type of the key of the objects to load.
    /// </typeparam>
    /// <typeparam name="TObject">
    /// Type of the objects to load.
    /// </typeparam>
    [ContractClass(typeof(IObjectLoaderContract<,>))]
    public interface IObjectLoader<in TKey, out TObject>
        : IObjectLoader where TObject : class
    {
        /// <summary>
        /// Loads an object by the specified key.
        /// </summary>
        /// <param name="key">
        /// Key of the object to load.
        /// </param>
        /// <returns>
        /// An object that was loaded.
        /// </returns>
        /// <exception cref="CannotLoadObjectException">
        /// The object cannot be loaded.
        /// </exception>
        TObject Load(TKey key);
    }

    /// <summary>
    /// Defines a method to asynchronously load on object
    /// of type TObject by its key of type TKey.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type of the key of the objects to load.
    /// </typeparam>
    /// <typeparam name="TObject">
    /// Type of the objects to load.
    /// </typeparam>
    [ContractClass(typeof(IAsyncObjectLoaderContract<,>))]
    public interface IAsyncObjectLoader<in TKey, TObject>
        : IObjectLoader<TKey, TObject> where TObject : class
    {
        /// <summary>
        /// Asynchronously loads an object by the specified key.
        /// </summary>
        /// <param name="key">
        /// Key of the object to load.
        /// </param>
        /// <returns>
        /// An object that was loaded.
        /// </returns>
        /// <exception cref="CannotLoadObjectException">
        /// The object cannot be loaded.
        /// </exception>
        Task<TObject> AsyncLoad(TKey key);
    }
}
