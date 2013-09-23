/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PlatformGameCreator.Editor.Common
{
    /// <summary>
    /// Specifies how the list changed.
    /// </summary>
    enum ObservableListChangedType
    {
        /// <summary>
        ///  Much of the list has changed. Any listener should refresh all their data from the list.
        /// </summary>
        Reset,

        /// <summary>
        ///  An item added to the list. 
        ///  <see cref="ObservableListChangedEventArgs{T}.Index"/> contains the index of the item that was added and
        ///  <see cref="ObservableListChangedEventArgs{T}.Item"/> contains the added item.
        /// </summary>
        ItemAdded,

        /// <summary>
        ///  An item deleted from the list. 
        ///  <see cref="ObservableListChangedEventArgs{T}.Index"/> contains the index of the item that was deleted and 
        ///  <see cref="ObservableListChangedEventArgs{T}.Item"/> contains the deleted item.
        /// </summary>
        ItemDeleted,

        /// <summary>
        /// An item changed in the list. 
        /// <see cref="ObservableListChangedEventArgs{T}.Index"/> contains the index of the item that was changed and 
        /// <see cref="ObservableListChangedEventArgs{T}.Item"/> contains the old item.
        /// </summary>
        ItemChanged
    };

    /// <summary>
    /// Provides data for the <see cref="ObservableList{T}.ListChanged"/> event.
    /// </summary>
    /// <typeparam name="T">Type of an item at <see cref="ObservableList{T}"/>.</typeparam>
    class ObservableListChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the type that specifies how the list changed.
        /// </summary>
        public ObservableListChangedType ListChangedType
        {
            get { return _listChangedType; }
        }
        private ObservableListChangedType _listChangedType;

        /// <summary>
        /// Gets the index of the item. Value depends on <see cref="ListChangedType"/>.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }
        private int _index;

        /// <summary>
        /// Gets the item. Value depends on <see cref="ListChangedType"/>.
        /// </summary>
        public T Item
        {
            get { return _item; }
        }
        private T _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableListChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="listChangedType">Type that specifies how the list changed.</param>
        public ObservableListChangedEventArgs(ObservableListChangedType listChangedType)
        {
            _index = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableListChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="listChangedType">Type that specifies how the list changed.</param>
        /// <param name="item">The item, depends on <paramref name="listChangedType"/>.</param>
        /// <param name="index">The index of the item, depends on <paramref name="listChangedType"/>.</param>
        public ObservableListChangedEventArgs(ObservableListChangedType listChangedType, T item, int index)
        {
            _item = item;
            _listChangedType = listChangedType;
            _index = index;
        }
    }

    /// <summary>
    /// Represents a dynamic data list that provides notifications when items get added, removed or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">Type of an item.</typeparam>
    [Serializable]
    class ObservableList<T> : IList<T>, ISerializable
    {
        /// <summary>
        /// List for storing the values.
        /// </summary>
        private List<T> items;

        /// <summary>
        /// Occurs when the list changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;T&gt;"/> instance containing the event data.</param>
        public delegate void ListChangedEventHandler(object sender, ObservableListChangedEventArgs<T> e);

        /// <summary>
        /// Occurs when an item is added, removed, changed or the entire list is refreshed.
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList&lt;T&gt;"/> class.
        /// </summary>
        public ObservableList()
        {
            items = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList&lt;T&gt;"/> class and copies initial values from the specified collection.
        /// </summary>
        /// <param name="collection">The collection to get initial values from.</param>
        public ObservableList(IEnumerable<T> collection)
        {
            items = new List<T>(collection);
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected ObservableList(SerializationInfo info, StreamingContext ctxt)
        {
            items = (List<T>)info.GetValue("Items", typeof(List<T>));
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="ObservableList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ObservableList{T}"/>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        /// <inheritdoc />
        /// <summary>
        /// Inserts an item to the <see cref="ObservableList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="ObservableList{T}"/>.</param>
        public virtual void Insert(int index, T item)
        {
            items.Insert(index, item);

            ListChangedHandler(new ObservableListChangedEventArgs<T>(ObservableListChangedType.ItemAdded, item, index));
        }

        /// <inheritdoc />
        /// <summary>
        /// Removes the <see cref="ObservableList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public virtual void RemoveAt(int index)
        {
            T deletedItem = items[index];

            items.RemoveAt(index);

            ListChangedHandler(new ObservableListChangedEventArgs<T>(ObservableListChangedType.ItemDeleted, deletedItem, index));
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        public T this[int index]
        {
            get { return items[index]; }
            set
            {
                T oldValue = items[index];
                items[index] = value;
                ListChangedHandler(new ObservableListChangedEventArgs<T>(ObservableListChangedType.ItemChanged, oldValue, index));
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="ObservableList{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ObservableList{T}"/>.</param>
        public virtual void Add(T item)
        {
            items.Add(item);

            ListChangedHandler(new ObservableListChangedEventArgs<T>(ObservableListChangedType.ItemAdded, item, items.Count - 1));
        }

        /// <summary>
        /// Removes all items from the <see cref="ObservableList{T}"/>.
        /// </summary>
        public void Clear()
        {
            items.Clear();

            ListChangedHandler(new ObservableListChangedEventArgs<T>(ObservableListChangedType.Reset));
        }

        /// <summary>
        /// Determines whether the <see cref="ObservableList{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ObservableList{T}"/>.</param>
        /// <returns>true if item is found in the <see cref="ObservableList{T}"/>; otherwise false.</returns>
        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ObservableList{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ObservableList{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ObservableList{T}"/>.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        ///  Gets a value indicating whether the <see cref="ObservableList{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ObservableList{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ObservableList{T}"/>.</param>
        /// <returns>true if item was successfully removed from the <see cref="ObservableList{T}"/>; otherwise, false. This method also returns false if item is not found in the original <see cref="ObservableList{T}"/>.</returns>
        public bool Remove(T item)
        {
            int index = items.IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            else
            {
                RemoveAt(index);
                return true;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Items", items);
        }

        /// <summary>
        /// Invokes the <see cref="ListChanged"/> with the specified data.
        /// </summary>
        /// <param name="observableListChangedEventArgs">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;T&gt;"/> instance containing the event data.</param>
        private void ListChangedHandler(ObservableListChangedEventArgs<T> observableListChangedEventArgs)
        {
            if (ListChanged != null)
            {
                ListChanged(this, observableListChangedEventArgs);
            }
        }
    }

    /// <summary>
    /// Represents a dynamic data list that provides notifications when items get added, removed or when the whole list is refreshed and
    /// every items contains its index in the list.
    /// </summary>
    /// <typeparam name="T">Type of an item.</typeparam>
    [Serializable]
    class ObservableIndexedList<T> : ObservableList<T>, IDeserializationCallback where T : IIndex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableIndexedList&lt;T&gt;"/> class.
        /// </summary>
        public ObservableIndexedList()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableIndexedList&lt;T&gt;"/> class and copies initial values from the specified collection.
        /// </summary>
        /// <param name="collection">The collection to get initial values from.</param>
        public ObservableIndexedList(IEnumerable<T> collection)
            : base(collection)
        {
            UpdateIndexes(0);
        }

        /// <inheritdoc />
        protected ObservableIndexedList(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {

        }

        /// <inheritdoc />
        public override void Insert(int index, T item)
        {
            if (index >= 0 && index <= Count)
            {
                UpdateIndexes(index, 1);
                item.Index = index;
            }

            base.Insert(index, item);
        }

        /// <inheritdoc />
        public override void Add(T item)
        {
            item.Index = Count;

            base.Add(item);
        }

        /// <inheritdoc />
        public override void RemoveAt(int index)
        {
            if (index >= 0 && index <= Count)
            {
                UpdateIndexes(index, -1);
                this[index].Index = -1;
            }

            base.RemoveAt(index);
        }

        /// <summary>
        /// Updates the indexes of all items.
        /// </summary>
        /// <param name="startIndex">The start index to start updating.</param>
        /// <param name="offset">The offset to add to the index of an item.</param>
        private void UpdateIndexes(int startIndex, int offset = 0)
        {
            for (int i = startIndex; i < Count; ++i)
            {
                this[i].Index = i + offset;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// Sets indexes of all items.
        /// </summary>
        public virtual void OnDeserialization(object sender)
        {
            UpdateIndexes(0);
        }
    }
}
