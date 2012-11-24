using System.Diagnostics.Contracts;

namespace LordJZ
{
    /// <summary>
    /// Represents an object only one instance of which can exist.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the object.
    /// </typeparam>
    public abstract class Singleton<T> : Model where T : Singleton<T>, new()
    {
        internal static string ContractFailedMessage_InvalidType
        {
            get { return "An object that derives from LordJZ.SocialClient.Singleton<T> must be of type T."; }
        }

        internal static string ContractFailedMessage_SecondInstance
        {
            get { return "Cannot create a second instance of LordJZ.SocialClient.Singleton<" + typeof(T).FullName + ">."; }
        }

        static volatile T s_instance;
        static object s_syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of <see cref="Singleton{T}"/>.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The derived class is not of type <c>T</c>.
        /// -or-
        /// An instance of <see cref="Singleton{T}"/> has already been created.
        /// </exception>
        protected Singleton()
        {
            Contract.Assert(this.GetType() == typeof(T), ContractFailedMessage_InvalidType);
            Contract.Assume(s_instance == null, ContractFailedMessage_SecondInstance);

            s_instance = (T)this;

            // No need of sync root any more.
            s_syncRoot = null;
        }

        /// <summary>
        /// Gets the only instance of <see cref="Singleton{T}"/>.
        /// </summary>
        public static T Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<T>() != null);

                if (s_instance == null)
                {
                    lock (s_syncRoot)
                    {
                        if (s_instance == null)
                            return new T();
                    }
                }

                return s_instance;
            }
        }
    }
}
