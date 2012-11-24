using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using LordJZ.ObjectManagement.Contracts;

namespace LordJZ.ObjectManagement
{
    /// <summary>
    /// Defines a method to create new objects.
    /// </summary>
    [ContractClass(typeof(IObjectFactoryContract))]
    public interface IObjectFactory
    {
        /// <summary>
        /// Creates a new instance of an object.
        /// </summary>
        /// <returns>
        /// A new instance of an object.
        /// </returns>
        object Create();
    }

    /// <summary>
    /// Defines a method to create new objects of type TObject.
    /// </summary>
    /// <typeparam name="TObject">
    /// Type of the object this factory creates.
    /// </typeparam>
    [ContractClass(typeof(IObjectFactoryContract<>))]
    public interface IObjectFactory<out TObject> : IObjectFactory where TObject : class
    {
        /// <summary>
        /// Creates a new instance of an object of type TObject.
        /// </summary>
        /// <returns>
        /// A new instance of an object of type TObject.
        /// </returns>
        new TObject Create();
    }

    /// <summary>
    /// Defines a method to asynchronously create new objects of type TObject.
    /// </summary>
    /// <typeparam name="TObject">
    /// Type of the object this factory creates.
    /// </typeparam>
    [ContractClass(typeof(IAsyncObjectFactoryContract<>))]
    public interface IAsyncObjectFactory<TObject> : IObjectFactory<TObject> where TObject : class
    {
        /// <summary>
        /// Asynchronously creates a new instance of an object of type TObject.
        /// </summary>
        /// <returns>
        /// A new instance of an object of type TObject.
        /// </returns>
        Task<TObject> AsyncCreate();
    }
}
