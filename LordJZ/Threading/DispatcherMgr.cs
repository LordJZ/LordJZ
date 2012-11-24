using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Threading
{
    public static class DispatcherMgr
    {
        readonly static List<IDispatcher> s_dispatchers = new List<IDispatcher>();

        public static void RegisterDispatcher(IDispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null, "dispatcher");
            Contract.Requires(!Contract.Exists(Dispatchers, d => d == dispatcher), "This dispatcher is already registered.");
            Contract.Ensures(Contract.Exists(Dispatchers, d => d == dispatcher));

            s_dispatchers.Add(dispatcher);
        }

        public static void UnregisterDispatcher(IDispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null, "dispatcher");
            Contract.Requires(Contract.Exists(Dispatchers, d => d == dispatcher), "This dispatcher is not registered.");
            Contract.Ensures(!Contract.Exists(Dispatchers, d => d == dispatcher));

            s_dispatchers.Remove(dispatcher);
        }

        public static IDispatcher MainDispatcher { get; set; }

        public static IReadOnlyCollection<IDispatcher> Dispatchers { get { return s_dispatchers; } }
    }
}
