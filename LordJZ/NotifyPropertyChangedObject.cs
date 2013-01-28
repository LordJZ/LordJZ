using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged, ICloneable
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
        /// Raises the <see cref="NotifyPropertyChangedObject.PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName), "propertyName");

            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Called when a clone of the current instance is created using <see cref="Clone"/>.
        /// </summary>
        /// <param name="obj">
        /// A new object that is a copy of this instance.
        /// </param>
        /// <filterpriority>2</filterpriority>
        protected virtual void OnClone(ICloneable obj)
        {
            Contract.Requires(obj.GetType() == this.GetType());

            ((NotifyPropertyChangedObject)obj).PropertyChanged = null;
        }

        /// <summary>
        /// Uses <see cref="object.MemberwiseClone"/> to create a new object
        /// that is a copy of the current instance, and calls <see cref="OnClone"/>.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual object Clone()
        {
            ICloneable clone = (ICloneable)this.MemberwiseClone();
            this.OnClone(clone);
            return clone;
        }
    }
}
