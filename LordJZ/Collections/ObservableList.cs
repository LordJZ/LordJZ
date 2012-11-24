using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections
{
    [Obsolete]
    public class ObservableList<T> : GenericCollectionModel<T>, IList<T>, IReadOnlyList<T>
    {
        #region Enumerator

        public struct Enumerator : IEnumerator<T>
        {
            ObservableList<T> m_list;
            int m_index;
            int m_version;

            internal Enumerator(ObservableList<T> list)
            {
                m_list = list;
                m_index = -1;
                m_version = list.m_version;
            }

            public T Current
            {
                get
                {
                    this.CheckDisposed();

                    this.CheckVersion();

                    return m_list[m_index];
                }
            }

            public bool MoveNext()
            {
                this.CheckDisposed();
                this.CheckVersion();

                ++m_index;

                return m_index < m_list.Count;
            }

            public void Reset()
            {
                this.CheckDisposed();

                m_index = -1;
                m_version = m_list.m_version;
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            void CheckVersion()
            {
                if (m_version != m_list.m_version)
                    throw new InvalidOperationException("The ObservableList has changed.");
            }

            void CheckDisposed()
            {
                if (m_list == null)
                    throw new ObjectDisposedException("Enumerator");
            }

            public void Dispose()
            {
                m_list = null;
            }
        }

        #endregion

        #region Fields

        T[] m_data;
        IEqualityComparer<T> m_comparer;
        int m_pointer;
        int m_size;
        int m_version;

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(m_data != null);
            Contract.Invariant(m_comparer != null);
            Contract.Invariant(m_pointer >= 0);
            Contract.Invariant(this.Count >= 0);
            Contract.Invariant(this.Capacity >= 0);
            Contract.Invariant(this.Count <= this.Capacity);
        }

        #endregion

        #region Constructor

        const int s_defaultCapacity = 4;
        static readonly IEqualityComparer<T> s_defaultComparer = EqualityComparer<T>.Default;

        /// <summary>
        /// Initializes a new instance of <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
            this.Construct(s_defaultCapacity, s_defaultComparer);
        }

        public ObservableList(int capacity)
        {
            Contract.Requires(capacity > 0, "capacity");

            this.Construct(capacity, s_defaultComparer);
        }

        public ObservableList(IEqualityComparer<T> comparer)
        {
            Contract.Requires(comparer != null, "comparer");

            this.Construct(s_defaultCapacity, comparer);
        }

        public ObservableList(int capacity, IEqualityComparer<T> comparer)
        {
            Contract.Requires(capacity > 0, "capacity");
            Contract.Requires(comparer != null, "comparer");

            this.Construct(capacity, comparer);
        }

        void Construct(int capacity, IEqualityComparer<T> comparer)
        {
            Contract.Requires(capacity > 0, "capacity");
            Contract.Requires(comparer != null, "comparer");

            m_data = new T[capacity];
            m_comparer = comparer;
            m_pointer = 0;
            m_size = 0;
        }

        #endregion

        #region Public synchronized interface

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </returns>
        public int Count
        {
            get
            {
                return m_size;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </exception>
        public T this[int index]
        {
            get
            {
                Contract.Ensures(m_version == Contract.OldValue(m_version),
                                 "ObservableList version must not be modified by this method.");

                return this.InternalIndexer(index);
            }
            set
            {
                this.InternalIndexer(index, value);
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </param>
        public void Add(T item)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            this.InternalAdd(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </summary>
        public void Clear()
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            this.InternalClear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:LordJZ.Collections.ObservableList`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:LordJZ.Collections.ObservableList`1"/>;
        /// otherwise, false.
        /// </returns>
        /// <param name="item">
        /// The object to locate in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </param>
        public bool Contains(T item)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            return this.InternalContains(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from
        /// the <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, false. This method also returns false if <paramref name="item"/> is
        /// not found in the original <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </returns>
        /// <param name="item">
        /// The object to remove from the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </param>
        public bool Remove(T item)
        {
            Contract.Ensures(Contract.Result<bool>() || m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");
            Contract.Ensures(!Contract.Result<bool>() || m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            return this.InternalRemove(item);
        }

        /// <summary>
        /// Removes the <see cref="T:LordJZ.Collections.ObservableList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            this.InternalRemoveAt(index);
        }

        public Enumerator GetEnumerator()
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            return new Enumerator(this);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">
        /// The object to locate in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </param>
        public int IndexOf(T item)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            return this.InternalIndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:LordJZ.Collections.ObservableList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// </exception>
        public void Insert(int index, T item)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            this.InternalInsert(item, index);
        }

        #region CopyTo

        public void CopyTo(T[] array)
        {
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(array.Length >= this.Count);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            this.InternalCopyTo(array, 0, 0, this.Count);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:LordJZ.Collections.ObservableList`1"/> to
        /// an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements
        /// copied from <see cref="T:LordJZ.Collections.ObservableList`1"/>.
        /// The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The number of elements in the source <see cref="T:LordJZ.Collections.ObservableList`1"/>
        /// is greater than the available space from <paramref name="arrayIndex"/>
        /// to the end of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            this.InternalCopyTo(array, arrayIndex, 0, this.Count);
        }

        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            Contract.Requires(count >= 0 && count <= this.Count);
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(arrayIndex >= 0 && arrayIndex < array.Length);
            Contract.Requires(arrayIndex + count <= array.Length);
            Contract.Requires(count <= this.Count);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            this.InternalCopyTo(array, arrayIndex, 0, count);
        }

        public void CopyTo(T[] array, int arrayIndex, int listIndex, int count)
        {
            Contract.Requires(count >= 0 && count <= this.Count);
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(arrayIndex >= 0 && arrayIndex < array.Length);
            Contract.Requires(arrayIndex + count <= array.Length);
            Contract.Requires(listIndex >= 0 && listIndex < this.Count);
            Contract.Requires(listIndex + count <= this.Count);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            this.InternalCopyTo(array, arrayIndex, listIndex, count);
        }

        #endregion

        #endregion

        #region Private thread-unsafe interface

        T InternalIndexer(int index)
        {
            Contract.Requires(index >= 0 && index < this.Count, "index");
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            return m_data[this.ListIndexToArrayIndex(index)];
        }

        void InternalIndexer(int index, T value)
        {
            Contract.Requires(index >= 0 && index < this.Count, "index");
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            var arrayIdx = this.ListIndexToArrayIndex(index);

            var oldItem = m_data[arrayIdx];
            m_data[arrayIdx] = value;

            ++m_version;

            this.RaiseCollectionItemsReplaced(oldItem, value, index);
        }

        void InternalAdd(T item)
        {
            this.EnsureHaveSpaceForMoreItems(1);

            int index = this.Count;

            m_data[this.ListIndexToArrayIndexForInsert(index)] = item;

            ++m_size;
            ++m_version;

            this.RaiseCollectionItemsAdded(item, index);
        }

        void InternalClear()
        {
            Array.Clear(m_data, m_pointer, this.NonOverflowCount);
            Array.Clear(m_data, 0, this.OverflowCount);

            m_pointer = 0;
            m_size = 0;

            ++m_version;

            this.RaiseCollectionReset();
        }

        bool InternalContains(T item)
        {
            Contract.Ensures(!Contract.Result<bool>() || this.Count > 0);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            for (int i = 0, index = m_pointer, capacity = this.Capacity; i < this.Count; ++i, ++index)
            {
                // overflowed
                if (index == capacity)
                    index -= capacity;

                if (m_comparer.Equals(m_data[index], item))
                    return true;
            }

            return false;
        }

        bool InternalRemove(T item)
        {
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            int itemArrayIndex = -1;

            for (int i = 0, index = m_pointer, capacity = this.Capacity; i < this.Count; ++i, ++index)
            {
                if (index == capacity)
                    index -= capacity;

                if (!m_comparer.Equals(m_data[index], item))
                    continue;

                itemArrayIndex = index;
                break;
            }

            if (itemArrayIndex < 0)
                return false;

            Contract.Assume(itemArrayIndex < this.OverflowCount || itemArrayIndex >= m_pointer);

            this.InternalRemoveAtArrayIndex(itemArrayIndex);
            return true;
        }

        void InternalRemoveAt(int index)
        {
            Contract.Requires(index >= 0 && index < this.Count);
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            this.InternalRemoveAtArrayIndex(this.ListIndexToArrayIndex(index));
        }

        int InternalIndexOf(T item)
        {
            Contract.Ensures(Contract.Result<int>() == -1 ||
                             Contract.Result<int>() >= 0 && Contract.Result<int>() < this.Count);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            for (int i = 0, index = m_pointer, capacity = this.Capacity; i < this.Count; ++i, ++index)
            {
                if (index == capacity)
                    index -= capacity;

                if (m_comparer.Equals(m_data[index], item))
                    return i;
            }

            return -1;
        }

        void InternalInsert(T item, int listIndex)
        {
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex <= this.Count);
            Contract.Ensures(this.Count == Contract.OldValue(this.Count) + 1);
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            if (this.ExcessiveSpace < 1 || (listIndex != 0 && listIndex != this.Count))
            {
                // Reallocate the data storage

                var data = MakeStorageToStoreMoreItems(1);

                this.InternalCopyTo(data, 0, 0, listIndex);

                data[listIndex] = item;

                this.InternalCopyTo(data, listIndex + 1, listIndex, this.Count - listIndex);

                m_data = data;
                m_pointer = 0;
            }
            else if (listIndex == 0)
            {
                // Copy the elements into the beginning of the list

                m_pointer -= 1;

                if (m_pointer < 0)
                    m_pointer += this.Capacity;

                m_data[m_pointer] = item;
            }
            else if (listIndex == this.Count)
            {
                // Copy the elements into the end of the list

                int arrayIndex = this.ListIndexToArrayIndexForInsert(listIndex);

                m_data[arrayIndex] = item;
            }
            //else
            //{
            //    // Insert into the middle of the list

            //    const int crossOverflowBorderComplexity = 5;
            //    int overflow = this.OverflowCount;
            //    int nonOverflow = this.NonOverflowCount;

            //    // Calculate complexity of moving elements to the left (backwards)
            //    int toLeftComplexity = listIndex;
            //    // If the index at overflow part
            //    if (toLeftComplexity >= nonOverflow)
            //        toLeftComplexity += crossOverflowBorderComplexity;

            //    // Calculate complexity of moving elements to the right (forwards)
            //    int toRightComplexity = this.Count - listIndex;
            //    // If the index at non-overflow part
            //    if (toRightComplexity >= overflow)
            //        toRightComplexity += crossOverflowBorderComplexity;

            //    if (toLeftComplexity > toRightComplexity)
            //    {
            //        // Move the elements forwards (to right)

            //        throw new NotImplementedException();
            //    }
            //    else
            //    {
            //        // Move the elements backwards (to left)

            //        throw new NotImplementedException();
            //    }
            //}

            m_size += 1;
            ++m_version;
        }

        void InternalInsert(IReadOnlyCollection<T> collection, int listIndex)
        {
            Contract.Requires(collection != null);
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex <= this.Count);
            Contract.Ensures(this.Count == Contract.OldValue(this.Count) + collection.Count);
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            var collectionCount = collection.Count;

            if (this.ExcessiveSpace < collectionCount || (listIndex != 0 && listIndex != this.Count))
            {
                // Reallocate the data storage

                var data = MakeStorageToStoreMoreItems(collectionCount);

                this.InternalCopyTo(data, 0, 0, listIndex);

                collection.CopyTo(0, data, listIndex, collectionCount);

                this.InternalCopyTo(data, listIndex + collectionCount, listIndex, this.Count - listIndex);

                m_data = data;
                m_pointer = 0;
            }
            else if (listIndex == 0)
            {
                // Copy the elements into the beginning of the list

                m_pointer -= collectionCount;

                if (m_pointer >= 0)
                {
                    // Didn't overflow, copy everything in one turn

                    collection.CopyTo(0, m_data, m_pointer, collectionCount);
                }
                else
                {
                    // Overflowed

                    int nonOverflow = -m_pointer;
                    int overflow = m_pointer + collectionCount;

                    m_pointer += this.Capacity;

                    // Copy the new non-overflowing part
                    collection.CopyTo(0, m_data, m_pointer, nonOverflow);
                    // Copy the new overflowing part
                    collection.CopyTo(nonOverflow, m_data, 0, overflow);
                }
            }
            else if (listIndex == this.Count)
            {
                // Copy the elements into the end of the list

                int arrayIndex = this.ListIndexToArrayIndexForInsert(listIndex);

                if (arrayIndex + collectionCount <= this.Capacity)
                {
                    // Didn't overflow, copy everything in one turn

                    collection.CopyTo(0, m_data, arrayIndex, collectionCount);
                }
                else
                {
                    // Overflowed

                    int nonOverflow = this.Capacity - arrayIndex;
                    int overflow = collectionCount - nonOverflow;

                    // Copy the new non-overflowing part
                    collection.CopyTo(0, m_data, arrayIndex, nonOverflow);
                    // Copy the new overflowing part
                    collection.CopyTo(nonOverflow, m_data, 0, overflow);
                }
            }
            //else
            //{
            //    // Insert into the middle of the list

            //    const int crossOverflowBorderComplexity = 5;
            //    int overflow = this.OverflowCount;
            //    int nonOverflow = this.NonOverflowCount;

            //    // Calculate complexity of moving elements to the left (backwards)
            //    int toLeftComplexity = listIndex;
            //    // If the index at overflow part
            //    if (toLeftComplexity >= nonOverflow)
            //        toLeftComplexity += crossOverflowBorderComplexity;

            //    // Calculate complexity of moving elements to the right (forwards)
            //    int toRightComplexity = this.Count - listIndex;
            //    // If the index at non-overflow part
            //    if (toRightComplexity >= overflow)
            //        toRightComplexity += crossOverflowBorderComplexity;

            //    if (toLeftComplexity > toRightComplexity)
            //    {
            //        // Move the elements forwards (to right)

            //        throw new NotImplementedException();
            //    }
            //    else
            //    {
            //        // Move the elements backwards (to left)

            //        throw new NotImplementedException();
            //    }
            //}

            m_size += collectionCount;
            ++m_version;
        }

        void InternalCopyTo(T[] array, int arrayIndex, int listIndex, int count)
        {
            Contract.Requires(count >= 0 && count <= this.Count);
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);

            Contract.Requires(arrayIndex >= 0);
            Contract.Requires(arrayIndex + count <= array.Length);

            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex + count <= this.Count);
            //Contract.Requires(count == 0 || listIndex < this.Count);

            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            if (count == 0)
                return;

            Contract.Assume(listIndex < this.Count);

            this.InternalCopyToByListArrayIndex(array, arrayIndex, this.ListIndexToArrayIndex(listIndex), count);
        }

        #endregion

        #region Internal helpers

        protected int Capacity { get { return m_data.Length; } }

        protected int ExcessiveSpace
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return this.Capacity - this.Count;
            }
        }

        protected int OverflowCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() < this.Count);
                Contract.Ensures(Contract.Result<int>() <= m_pointer);

                var possibleNonOverflowCount = this.Capacity - m_pointer;

                Contract.Assert(possibleNonOverflowCount >= 0);

                return Math.Max(0, this.Count - possibleNonOverflowCount);
            }
        }

        protected int NonOverflowCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() <= this.Count);
                Contract.Ensures(Contract.Result<int>() <= this.Capacity - m_pointer);

                return Math.Min(this.Capacity - m_pointer, this.Count);
            }
        }

        /// <summary>
        /// Converts an index of an item in the ObservableList to an index of an item in the array.
        /// </summary>
        /// <param name="index">
        /// Index of an item in the ObservableList.
        /// </param>
        /// <returns>
        /// Index of an item in the array.
        /// </returns>
        protected int ListIndexToArrayIndex(int index)
        {
            Contract.Requires(index >= 0, "index");
            Contract.Requires(index < this.Count, "index");
            //Contract.Requires(index < this.Capacity);
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() < this.Capacity);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            // index < size <= capacity
            // index < capacity
            Contract.Assert(index < this.Capacity);

            var capacity = this.Capacity;

            index += m_pointer;
            if (index >= capacity)
                index -= capacity;

            return index;
        }

        protected int ListIndexToArrayIndexForInsert(int index)
        {
            Contract.Requires(index >= 0, "index");
            Contract.Requires(index <= this.Count, "index");
            Contract.Requires(this.ExcessiveSpace >= 1);
            //Contract.Requires(index < this.Capacity);
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() < this.Capacity);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            // index <= size < capacity
            // index < capacity
            Contract.Assume(index < this.Capacity);

            var capacity = this.Capacity;

            index += m_pointer;
            if (index >= capacity)
                index -= capacity;

            return index;
        }

        protected void EnsureHaveSpaceForMoreItems(int count)
        {
            Contract.Requires(count >= 0);
            Contract.Ensures(this.ExcessiveSpace >= count);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            if (this.ExcessiveSpace >= count)
                return;

            var data = this.MakeStorageToStoreMoreItems(count);

            this.CopyTo(data);

            m_data = data;
            m_pointer = 0;
        }

        protected void EnsureHaveSpaceForMoreItemsAt(int listIndex, int count)
        {
            Contract.Requires(listIndex >= 0);
            Contract.Requires(listIndex <= this.Count);
            Contract.Requires(count >= 0);
            Contract.Ensures(this.ExcessiveSpace >= count);
            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            if (listIndex == 0 || listIndex == this.Count)
            {
                // Allocating space for items in the beginning or the end

                this.EnsureHaveSpaceForMoreItems(count);
            }
            else
            {
                if (this.ExcessiveSpace < count)
                {
                    // Reallocate the storage

                    var data = MakeStorageToStoreMoreItems(count);

                    this.InternalCopyTo(data, 0, 0, listIndex);
                    this.InternalCopyTo(data, listIndex + count, listIndex, this.Count - listIndex);

                    m_data = data;
                    m_pointer = 0;
                }
            }
        }

        protected T[] MakeStorageToStoreMoreItems(int count)
        {
            Contract.Requires(count >= 0);

            // Double the size of the storage,
            // and if that is not enough, allocate exact required size
            var newCount = Math.Max(this.Count * 2, this.Count + count);

            // Align to 4 elements
            newCount = (newCount + 3) & ~3;

            Contract.Assume(newCount - this.Count >= count);

            return new T[newCount];
        }

        void InternalCopyToByListArrayIndex(T[] array, int arrayIndex, int listArrayIndex, int count)
        {
            Contract.Requires(count >= 0 && count <= this.Count);
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);

            Contract.Requires(arrayIndex >= 0);
            Contract.Requires(arrayIndex + count <= array.Length);

            Contract.Requires(listArrayIndex >= 0 && listArrayIndex < this.Capacity);

            Contract.Ensures(m_version == Contract.OldValue(m_version),
                             "ObservableList version must not be modified by this method.");

            var copyCount = Math.Min(this.Capacity - listArrayIndex, count);
            Array.Copy(m_data, listArrayIndex, array, arrayIndex, copyCount);
            arrayIndex += copyCount;
            count -= copyCount;

            Array.Copy(m_data, 0, array, arrayIndex, count);
        }

        void InternalRemoveAtArrayIndex(int arrayIndex)
        {
            Contract.Requires(arrayIndex >= 0 && arrayIndex < this.Capacity);
            Contract.Requires(arrayIndex < this.OverflowCount || arrayIndex >= m_pointer);
            Contract.Ensures(m_version == Contract.OldValue(m_version) + 1,
                             "ObservableList version must be incremented by this method.");

            var oldItem = m_data[arrayIndex];

            if (arrayIndex >= m_pointer)
            {
                // Removing from not overflowing part

                var copyCount = arrayIndex - m_pointer;
                if (copyCount == 0)
                    // Removing from the beginning of the list
                    m_data[arrayIndex] = default(T);
                else
                {
                    // Move elements forward
                    Array.Copy(m_data, m_pointer, m_data, m_pointer + 1, copyCount);

                    // Clear the leftmost item
                    m_data[m_pointer] = default(T);
                }

                ++m_pointer;
            }
            else
            {
                // Removing from overflowing part

                // Number of elements to the right of the removed element
                var copyCount = this.OverflowCount - arrayIndex - 1;

                Contract.Assert(copyCount >= 0);

                if (copyCount == 0)
                    // Removing from the end of the list
                    m_data[arrayIndex] = default(T);
                else
                {
                    // Move elements backward
                    Array.Copy(m_data, arrayIndex + 1, m_data, arrayIndex, copyCount);

                    // Clear the rightmost item
                    m_data[arrayIndex + copyCount] = default(T);
                }
            }

            --m_size;
            ++m_version;

            this.RaiseCollectionItemsRemoved(oldItem, arrayIndex);
        }

        #endregion

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly { get { return false; } }

        #endregion

        #region IEnumerable

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        T IReadOnlyList<T>.this[int index]
        {
            get { return this[index]; }
        }
    }
}
