using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Threading
{
    public class DispatchedEvent<T> where T : class
    {
        ValueTuple<T, IDispatcher>[] m_handlers;

        /// <summary>
        /// Initializes a new instance of <see cref="LordJZ.Threading.DispatchedEvent&lt;T&gt;"/> class.
        /// </summary>
        public DispatchedEvent()
        {
        }

        /// <summary>
        /// Adds an event handler to the invocation list of the current event as a dispatched event handler.
        /// </summary>
        /// <param name="handler">
        /// The event handler to add as dispatched event handler.
        /// </param>
        public void AddDispatched(T handler)
        {
            DispatchedEventHelper.AddNotDispatched(this, ref m_handlers, handler);
        }

        /// <summary>
        /// Adds an event handler to the invocation list of the current event as a NOT dispatched event handler.
        /// </summary>
        /// <param name="handler">
        /// The event handler to add as a NOT dispatched event handler.
        /// </param>
        public void Add(T handler)
        {
            DispatchedEventHelper.AddDispatched(this, ref m_handlers, handler);
        }

        /// <summary>
        /// Adds an event handler to the invocation list of the current event as an event handler
        /// that is dispatched using the specified dispatcher.
        /// </summary>
        /// <param name="handler">
        /// The event handler to add as an event handler
        /// that is dispatched using the specified dispatcher.
        /// </param>
        /// <param name="dispatcher">
        /// Dispatcher to use when dispatching the event handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <c>dispatcher</c> is <c>null</c>.
        /// </exception>
        public void AddDispatched(T handler, IDispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null, "dispatcher");

            DispatchedEventHelper.AddDispatched(this, ref m_handlers, handler, dispatcher);
        }

        public void Remove(T handler)
        {
            DispatchedEventHelper.Remove(this, ref m_handlers, handler);
        }

        public bool IsEmpty
        {
            get { return DispatchedEventHelper.IsEmpty(this, ref m_handlers); }
        }

        public bool FastIsEmpty
        {
            get { return DispatchedEventHelper.FastIsEmpty(ref m_handlers); }
        }
    }
}
