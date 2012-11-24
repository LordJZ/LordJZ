using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    /// <summary>
    /// Represents a data object which properties can be bound on.
    /// </summary>
    public abstract class Model : NotifyPropertyChangedObject
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Model"/> class.
        /// </summary>
        protected Model()
        {
        }

        /// <summary>
        /// Sets the property to the specified value and raises
        /// the <see cref="NotifyPropertyChangedObject.PropertyChanged"/> event if
        /// the new value is not equal to the old value of the property
        /// according to the default equality comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The underlying type of the property.
        /// </typeparam>
        /// <param name="property">
        /// The backing field of the property, passed by reference.
        /// </param>
        /// <param name="value">
        /// The new value of the property.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property to set.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the value has been changed; otherwise, <c>false</c>.
        /// </returns>
        protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            Contract.Requires(propertyName != null, "propertyName");
            Contract.Requires(propertyName != string.Empty, "propertyName");

            return this.SetProperty(ref property, value, propertyName, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Sets the property to the specified value and raises
        /// the <see cref="NotifyPropertyChangedObject.PropertyChanged"/> event if
        /// the new value is not equal to the old value of the property
        /// according to the specified equality comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The underlying type of the property.
        /// </typeparam>
        /// <param name="property">
        /// The backing field of the property, passed by reference.
        /// </param>
        /// <param name="value">
        /// The new value of the property.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property to set.
        /// </param>
        /// <param name="equalityComparer">
        /// The equality comparer to use when comparing the old value and the new value of the property.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the value has been changed; otherwise, <c>false</c>.
        /// </returns>
        protected bool SetProperty<T>(ref T property, T value, string propertyName,
            IEqualityComparer<T> equalityComparer)
        {
            Contract.Requires(propertyName != null, "propertyName");
            Contract.Requires(propertyName != string.Empty, "propertyName");
            Contract.Requires(equalityComparer != null, "equalityComparer");

            if (!equalityComparer.Equals(property, value))
            {
                property = value;
                this.OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected bool SetProperty<T>(ref T property, T value, IEqualityComparer<T> equalityComparer,
                                      [CallerMemberName] string propertyName = null)
        {
            return this.SetProperty(ref property, value, propertyName, equalityComparer);
        }
    }
}
