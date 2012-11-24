using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="NotifyPropertyChangedObject"/> class.
        /// </summary>
        protected NotifyPropertyChangedObject()
        {
        }

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

            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
