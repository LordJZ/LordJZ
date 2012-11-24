using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using LordJZ.ObjectManagement.Contracts;

namespace LordJZ.ObjectManagement
{
    /// <summary>
    /// Defines a method to delete objects by their keys.
    /// </summary>
    [ContractClass(typeof(IObjectDeleterContract))]
    public interface IObjectDeleter
    {
        /// <summary>
        /// Deletes an object by its key.
        /// </summary>
        /// <param name="key">
        /// Key of the object to delete.
        /// </param>
        void Delete(object key);
    }

    /// <summary>
    /// Defines a method to delete objects by their keys of type TKey.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type of keys of objects to delete.
    /// </typeparam>
    [ContractClass(typeof(IObjectDeleterContract<>))]
    public interface IObjectDeleter<in TKey> : IObjectDeleter
    {
        /// <summary>
        /// Deletes an object by its key.
        /// </summary>
        /// <param name="key">
        /// Key of the object to delete.
        /// </param>
        void Delete(TKey key);
    }

    /// <summary>
    /// Asynchronously defines a method to delete objects by their keys of type TKey.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type of keys of objects to delete.
    /// </typeparam>
    [ContractClass(typeof(IAsyncObjectDeleterContract<>))]
    public interface IAsyncObjectDeleter<in TKey> : IObjectDeleter<TKey>
    {
        /// <summary>
        /// Asynchronously deletes an object by its key.
        /// </summary>
        /// <param name="key">
        /// Key of the object to delete.
        /// </param>
        Task AsyncDelete(TKey key);
    }
}
