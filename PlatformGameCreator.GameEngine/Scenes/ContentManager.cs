/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scenes
{
    /// <summary>
    /// Represents a collection of items that are stored under the unique id.
    /// </summary>
    /// <remarks>
    /// Every item of the content for the game has the unique id. <see cref="SceneScreen"/> stores content in this collection.
    /// </remarks>
    /// <typeparam name="T">The type of the item in <see cref="ContentManager{T}"/>.</typeparam>
    public class ContentManager<T> : IEnumerable<T> where T : class
    {
        /// <summary>
        /// Internal Dictionary for storing the values.
        /// </summary>
        private Dictionary<int, T> content = new Dictionary<int, T>();

        /// <summary>
        /// Determines whether the <see cref="ContentManager{T}"/> contains the specified id.
        /// </summary>
        /// <param name="id">The id to locate in the <see cref="ContentManager{T}"/>.</param>
        /// <returns><c>true</c> if the <see cref="ContentManager{T}"/> contains an item with the specified id; otherwise <c>false</c>.</returns>
        public bool Contains(int id)
        {
            return content.ContainsKey(id);
        }

        /// <summary>
        /// Determines whether the <see cref="ContentManager{T}"/> contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="ContentManager{T}"/>.</param>
        /// <returns><c>true</c> if the <see cref="ContentManager{T}"/> contains the specified value; otherwise <c>false</c>.</returns>
        public bool Contains(T value)
        {
            return content.ContainsValue(value);
        }

        /// <summary>
        /// Adds the specified value with the specified id to the <see cref="ContentManager{T}"/>.
        /// </summary>
        /// <param name="id">The id of the value.</param>
        /// <param name="value">The value to add.</param>
        /// <exception cref="ArgumentException">An item with the same id already exists in the <see cref="ContentManager{T}"/>.</exception>
        public void Add(int id, T value)
        {
            if (Contains(id))
            {
                throw new ArgumentException("An element with the same id already exists.");
            }

            content[id] = value;
        }

        /// <summary>
        /// Gets the value associated with the specified id.
        /// </summary>
        /// <param name="id">The id of the value to get.</param>
        /// <returns>The value associated with the specified id, if the key is found; otherwise <c>null</c>.</returns>
        public T Get(int id)
        {
            T value;
            content.TryGetValue(id, out value);
            return value;
        }

        /// <summary>
        /// Removes all values from the <see cref="ContentManager{T}"/>.
        /// </summary>
        public void Clear()
        {
            content.Clear();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return content.Values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return content.Values.GetEnumerator();
        }
    }
}
