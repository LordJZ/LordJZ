using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace LordJZ.Threading
{
    internal static class DispatchedEventHelper
    {
        internal static bool IsEmpty<T>(object @lock, ref ValueTuple<T, IDispatcher>[] array) where T : class
        {
            lock (@lock)
            {
                var derefArray = array;
                if (derefArray == null || derefArray.Length == 0)
                    return true;

                for (int i = 0; i < derefArray.Length; i++)
                {
                    if (derefArray[i].Item1 != null)
                        return false;
                }

                array = null;
                return true;
            }
        }

        internal static bool FastIsEmpty<T>(ref ValueTuple<T, IDispatcher>[] array) where T : class
        {
            var copy = array;
            return copy == null || copy.Length == 0;
        }

        internal static void AddDispatched<T>(object @lock, ref ValueTuple<T, IDispatcher>[] array, T handler) where T : class
        {
            if (handler == null)
                return;

            lock (@lock)
            {
                var dispatcher = DispatcherMgr.Dispatchers
#if DEBUG
                    .SingleOrDefault(element => !element.InvokeRequired);
#else
                    .FirstOrDefault(element => !element.InvokeRequired);
#endif

                Add(ref array, handler, dispatcher);
            }
        }

        internal static void AddDispatched<T>(object @lock, ref ValueTuple<T, IDispatcher>[] array, T handler, IDispatcher dispatcher)
            where T : class
        {
            Contract.Requires(dispatcher != null, "dispatcher");

            if (handler == null)
                return;

            lock (@lock)
                Add(ref array, handler, dispatcher);
        }

        internal static void AddNotDispatched<T>(object @lock, ref ValueTuple<T, IDispatcher>[] array, T handler) where T : class
        {
            if (handler == null)
                return;

            lock (@lock)
            {
                Add(ref array, handler, null);
            }
        }

        static void Add<T>(ref ValueTuple<T, IDispatcher>[] array, T handler, IDispatcher dispatcher) where T : class
        {
            var derefArray = array;
            var len = derefArray != null ? derefArray.Length : 0;
            for (int i = 0; i < len; i++)
            {
                if (derefArray[i].Item1 != null)
                    continue;

                derefArray[i] = ValueTuple.Create(handler, dispatcher);
                return;
            }

            Array.Resize(ref array, len + 2);

            array[len] = ValueTuple.Create(handler, dispatcher);
        }

        internal static void Remove<T>(object @lock, ref ValueTuple<T, IDispatcher>[] array, T handler) where T : class
        {
            if (handler == null)
                return;

            lock (@lock)
            {
                var derefArray = array;
                if (derefArray == null)
                    return;

                bool removed = false;
                int existing = 0;

                for (int i = derefArray.Length - 1; i >= 0; i--)
                {
                    var existingHandler = derefArray[i].Item1;

                    if (!removed && (Delegate)(object)existingHandler == (Delegate)(object)handler)
                    {
                        derefArray[i] = default(ValueTuple<T, IDispatcher>);

                        removed = true;
                    }
                    else if (existingHandler != null)
                        ++existing;

                    if (removed && existing > 0)
                        return;
                }

                if (existing == 0)
                    array = null;
            }
        }

        class RaiseClosure
        {
            readonly Delegate m_del;
            readonly object[] m_args;

            internal RaiseClosure(Delegate del, object[] args)
            {
                m_del = del;
                m_args = args;
            }

            internal void Invoke()
            {
                m_del.DynamicInvoke(m_args);
            }
        }

        internal static void Raise<T, TArgs>(object @lock, ref ValueTuple<T, IDispatcher>[] array, object source, TArgs args)
            where T : class
            where TArgs : class
        {
            lock (@lock)
            {
                if (FastIsEmpty(ref array))
                    return;

                var derefArray = array;
                object[] invokeArgs = null;
                var len = derefArray.Length;
                for (int i = 0; i < len; i++)
                {
                    var handler = derefArray[i].Item1;
                    if (handler == null)
                        continue;

                    // Invocation will now happen, dispatched or not.
                    var del = (Delegate)(object)handler;
                    if (invokeArgs == null)
                        invokeArgs = new[] { source, args };

                    var dispatcher = derefArray[i].Item2;
                    if (dispatcher != null && dispatcher.InvokeRequired)
                        dispatcher.BeginInvoke(new RaiseClosure(del, invokeArgs).Invoke);
                    else
                        del.DynamicInvoke(invokeArgs);
                }
            }
        }
    }
}
