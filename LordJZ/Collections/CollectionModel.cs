using LordJZ.Threading;
using System.Collections;
using System.Collections.Specialized;

namespace LordJZ.Collections
{
    public abstract class CollectionModel : Model, INotifyCollectionChanged
    {
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:LordJZ.Collections.CollectionModel"/>.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:LordJZ.Collections.CollectionModel"/>.
        /// </returns>
        /// <remarks>
        /// Returns the current instance of <see cref="T:LordJZ.Collections.CollectionModel"/>.
        /// </remarks>
        /// <filterpriority>2</filterpriority>
        public object SyncRoot { get { return this; } }

        ValueTuple<NotifyCollectionChangedEventHandler, IDispatcher>[] m_collectionChanged;

        /// <summary>
        /// Returns true if there are no event handlers attached to CollectionChanged event; false otherwise.
        /// </summary>
        protected bool FastIsCollectionChangedEmpty { get { return DispatchedEventHelper.FastIsEmpty(ref m_collectionChanged); } }

        /// <summary>
        /// Occurs when collection changes. Event handlers attached to this event are not dispatched.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { DispatchedEventHelper.AddNotDispatched(this.SyncRoot, ref m_collectionChanged, value); }
            remove { DispatchedEventHelper.Remove(this.SyncRoot, ref m_collectionChanged, value); }
        }

        /// <summary>
        /// Occurs when collection changes. Event handlers attached to this event are dispatched.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChangedDispatched
        {
            add { DispatchedEventHelper.AddDispatched(this.SyncRoot, ref m_collectionChanged, value); }
            remove { DispatchedEventHelper.Remove(this.SyncRoot, ref m_collectionChanged, value); }
        }

        /// <summary>
        /// Occurs when collection changes. Event handlers attached to this event are dispatched.
        /// </summary>
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { DispatchedEventHelper.AddDispatched(this.SyncRoot, ref m_collectionChanged, value); }
            remove { DispatchedEventHelper.Remove(this.SyncRoot, ref m_collectionChanged, value); }
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (!this.FastIsCollectionChangedEmpty)
                DispatchedEventHelper.Raise(this.SyncRoot, ref m_collectionChanged, this, args);
        }

        protected void RaiseItemsChanged()
        {
            this.OnPropertyChanged("Item[]");
        }

        protected void RaiseCountChanged()
        {
            this.OnPropertyChanged("Count");
        }

        protected void RaiseCollectionReset()
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #region Add

        protected void RaiseCollectionItemsAdded(IList addedItems)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems));
        }

        protected void RaiseCollectionItemsAdded(object addedItem)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItem));
        }

        protected void RaiseCollectionItemsAdded(IList addedItems, int startingIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems, startingIndex)
                    );
        }

        protected void RaiseCollectionItemsAdded(object addedItem, int startingIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItem, startingIndex)
                    );
        }

        #endregion

        #region Remove

        protected void RaiseCollectionItemsRemoved(IList removedItems)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
        }

        protected void RaiseCollectionItemsRemoved(object removedItem)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem));
        }

        protected void RaiseCollectionItemsRemoved(IList removedItems, int startingIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, startingIndex)
                    );
        }

        protected void RaiseCollectionItemsRemoved(object removedItem, int startingIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, startingIndex)
                    );
        }

        #endregion

        #region Replace

        protected void RaiseCollectionItemsReplaced(IList newItems, IList oldItems)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems)
                    );
        }

        protected void RaiseCollectionItemsReplaced(object newItem, object oldItem)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem)
                    );
        }

        protected void RaiseCollectionItemsReplaced(IList newItems, IList oldItems, int startingIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, startingIndex)
                    );
        }

        protected void RaiseCollectionItemsReplaced(object newItem, object oldItem, int index)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index)
                    );
        }

        #endregion

        #region Move

        protected void RaiseCollectionItemsMoved(IList changedItems, int index, int oldIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, changedItems, index, oldIndex)
                    );
        }

        protected void RaiseCollectionItemsMoved(object changedItem, int index, int oldIndex)
        {
            if (!this.FastIsCollectionChangedEmpty)
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, changedItem, index, oldIndex)
                    );
        }

        #endregion
    }
}
