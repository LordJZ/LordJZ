using System;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    /// <summary>
    /// Represents a simple service locator.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the service.
    /// </typeparam>
    public static class Service<T> where T : class
    {
        static Func<T> s_factory;
        static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    if (s_factory == null)
                        throw new InvalidOperationException("Access to undefined service.");

                    s_instance = s_factory();
                    Contract.Assume(s_instance != null);

                    s_factory = null;
                }

                return s_instance;
            }
        }

        public static bool Installed
        {
            get { return s_instance != null || s_factory != null; }
        }

        public static void Install(Func<T> serviceFactory)
        {
            Contract.Requires(serviceFactory != null);
            Contract.Requires(!Installed);
            Contract.Ensures(Installed);

            s_factory = serviceFactory;
        }

        public static void Install(T service)
        {
            Contract.Requires(service != null);
            Contract.Requires(!Installed);
            Contract.Ensures(Installed);

            s_instance = service;
        }
    }
}
