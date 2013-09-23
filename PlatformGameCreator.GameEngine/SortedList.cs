/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine
{
    /// <summary>
    /// Simple sorted list.
    /// </summary>
    /// <remarks>
    /// All operation use binary search for searching an item = O(log N).
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sorted list which implements <see cref="IComparable{T}"/>.</typeparam>
    public class SortedList<T> : IList<T> where T : class, IComparable<T>
    {
        // internal list for storing the values
        private List<T> items = new List<T>();

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            int index = items.BinarySearch(item);
            return index < 0 ? -1 : index;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Not supported operation. Cannot insert at the specified index because sorted list must preserve order.
        /// </remarks>
        /// <exception cref="NotImplementedException">operation of <see cref="IList{T}"/> is not allowed.</exception>
        public void Insert(int index, T item)
        {
            throw new NotImplementedException("Cannot insert at index (must preserve order).");
        }

        /// <inheritdoc />
        public virtual void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get { return items[index]; }
            set
            {
                RemoveAt(index);
                Add(value);
            }
        }

        /// <inheritdoc />
        public virtual void Add(T item)
        {
            int index = ~items.BinarySearch(item);

            if (index < 0) items.Insert(-index, item);
            else items.Insert(index, item);
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            items.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return items.BinarySearch(item) >= 0;
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return items.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        /// <summary>
        /// Removes the first occurrence of the specific item from the <see cref="SortedList{T}"/>.
        /// </summary>
        public bool Remove(T item)
        {
            var index = items.BinarySearch(item);
            if (index < 0) return false;

            if (item == items[index] && item.CompareTo(items[index]) == 0)
            {
                items.RemoveAt(index);
                return true;
            }
            else
            {
                items.Remove(item);
            }

            return false;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
