using LordJZ.Threading;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    public abstract class DispatchedNotifyPropertyChangedObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DispatchedNotifyPropertyChangedObject"/> class.
        /// </summary>
        protected DispatchedNotifyPropertyChangedObject()
        {
        }

        ValueTuple<PropertyChangedEventHandler, IDispatcher>[] m_propertyChanged;

        object SyncRoot { get { return this; } }

        /// <summary>
        /// Raises the <see cref="DispatchedNotifyPropertyChangedObject.PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires(propertyName != null, "propertyName");
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName), "propertyName");

            if (!DispatchedEventHelper.FastIsEmpty(ref m_propertyChanged))
                DispatchedEventHelper.Raise(this.SyncRoot, ref m_propertyChanged, this,
                                            new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when a property value changes. Event handlers attached to this event are not dispatched.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { DispatchedEventHelper.AddNotDispatched(this.SyncRoot, ref m_propertyChanged, value); }
            remove { DispatchedEventHelper.Remove(this.SyncRoot, ref m_propertyChanged, value); }
        }

        /// <summary>
        /// Occurs when a property value changes. Event handlers attached to this event are dispatched.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChangedDispatched
        {
            add { DispatchedEventHelper.AddDispatched(this.SyncRoot, ref m_propertyChanged, value); }
            remove { DispatchedEventHelper.Remove(this.SyncRoot, ref m_propertyChanged, value); }
        }

        /// <summary>
        /// Occurs when a property value changes. Event handlers attached to this event are dispatched.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { DispatchedEventHelper.AddDispatched(this.SyncRoot, ref m_propertyChanged, value); }
            remove { DispatchedEventHelper.Remove(this.SyncRoot, ref m_propertyChanged, value); }
        }
    }
}
