using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using LordJZ.ObjectManagement.Contracts;

namespace LordJZ.ObjectManagement
{
    /// <summary>
    /// Defines a method to persist (save) an object by its key.
    /// </summary>
    [ContractClass(typeof(IObjectSaverContract))]
    public interface IObjectSaver
    {
        /// <summary>
        /// Persists the object using the specified key.
        /// </summary>
        /// <param name="key">
        /// Key to be used when persisting the object.
        /// </param>
        /// <param name="obj">
        /// Object to be persisted.
        /// </param>
        /// <exception cref="CannotSaveObjectException">
        /// The object cannot be saved.
        /// </exception>
        void Save(object key, object obj);
    }

    /// <summary>
    /// Defines a method to persist (save) an object
    /// of type TObject by its key of type TKey.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type of the key of the objects to save.
    /// </typeparam>
    /// <typeparam name="TObject">
    /// Type of the objects to save.
    /// </typeparam>
    [ContractClass(typeof(IObjectSaverContract<,>))]
    public interface IObjectSaver<in TKey, in TObject>
        : IObjectSaver where TObject : class
    {
        /// <summary>
        /// Persists the object using the specified key.
        /// </summary>
        /// <param name="key">
        /// Key to be used when persisting the object.
        /// </param>
        /// <param name="obj">
        /// Object to be persisted.
        /// </param>
        /// <exception cref="CannotSaveObjectException">
        /// The object cannot be saved.
        /// </exception>
        void Save(TKey key, TObject obj);
    }

    /// <summary>
    /// Defines a method to asynchronously persist (save) an object
    /// of type TObject by its key of type TKey.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type of the key of the objects to save.
    /// </typeparam>
    /// <typeparam name="TObject">
    /// Type of the objects to save.
    /// </typeparam>
    [ContractClass(typeof(IAsyncObjectSaverContract<,>))]
    public interface IAsyncObjectSaver<in TKey, in TObject>
        : IObjectSaver<TKey, TObject> where TObject : class
    {
        /// <summary>
        /// Asynchronously persists the object using the specified key.
        /// </summary>
        /// <param name="key">
        /// Key to be used when persisting the object.
        /// </param>
        /// <param name="obj">
        /// Object to be persisted.
        /// </param>
        /// <exception cref="CannotSaveObjectException">
        /// The object cannot be saved.
        /// </exception>
        Task AsyncSave(TKey key, TObject obj);
    }
}
