/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scenes;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.GameObjects
{
    /// <summary>
    /// Base class for a game object that can be used at the scene (in the game).
    /// </summary>
    /// <remarks>
    /// Every game object has unique id in the whole project.
    /// </remarks>
    [Serializable]
    abstract class GameObject : IName, IVisible, IIndex, ISerializable
    {
        /// <summary>
        /// Gets the unique game object id in the whole project.
        /// </summary>
        public int Id
        {
            get { return _id; }
        }
        private int _id;

        /// <summary>
        /// Gets or sets the name of the game object.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    if (NameChanged != null) NameChanged(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Name of the game object.
        /// </summary>
        protected string _name;

        /// <summary>
        /// Gets or sets a value whether the game object is visible at the scene at the editor.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    if (VisibleChanged != null) VisibleChanged(this, EventArgs.Empty);
                }
            }
        }
        private bool _visible = true;

        /// <summary>
        /// Gets or sets the index of the game object in the container where is stored.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        protected GameObject()
        {
            _id = ScenesManager.GetUniqueGameObjectId();

            Debug.Assert(Id <= ScenesManager.LastUniqueGameObjectId, "Invalid id.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        /// <param name="id">The unique id for the game object.</param>
        protected GameObject(int id)
        {
            Debug.Assert(id <= ScenesManager.LastUniqueGameObjectId, "Invalid id.");

            _id = id;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected GameObject(SerializationInfo info, StreamingContext ctxt)
        {
            _id = info.GetInt32("Id");
            _name = info.GetString("Name");
            _visible = info.GetBoolean("Visible");
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("Visible", Visible);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the name of the game object.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Removes this game object from the container where is stored.
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// Creates the <see cref="SceneNode"/> representing this game object for using at the scene.
        /// </summary>
        /// <returns>Created <see cref="SceneNode"/> of this game object.</returns>
        public abstract SceneNode CreateSceneView();

        /// <summary>
        /// Creates a command that adds this game object to the container where is stored.
        /// </summary>
        /// <returns>Commnand for adding this game object to the container or <c>null</c> when not possible.</returns>
        public abstract Command CreateAddCommand();

        /// <summary>
        /// Creates a command that removes this game object to the container where is stored.
        /// </summary>
        /// <returns>Commnand for removing this game object to the container or <c>null</c> when not possible.</returns>
        public abstract Command CreateDeleteCommand();

        /// <summary>
        /// Clones this game object. Cloned object has new unique id.
        /// If <paramref name="addToContainer"/> is <c>true</c> cloned object is added to the container where this game object is stored.
        /// </summary>
        /// <param name="addToContainer">If set to <c>true</c> cloned object is added to the container.</param>
        /// <returns>Cloned game object.</returns>
        public abstract GameObject Clone(bool addToContainer = false);
    }

    /// <summary>
    /// Command for removing the game object from the container where is stored.
    /// </summary>
    /// <typeparam name="GameObjectType">The type of the game object.</typeparam>
    class DeleteGameObjectCommand<GameObjectType> : Command where GameObjectType : GameObject
    {
        /// <summary>
        /// Game object for this command.
        /// </summary>
        private GameObjectType gameObject;

        /// <summary>
        /// The container where the game object is stored.
        /// </summary>
        private IList<GameObjectType> container;

        /// <summary>
        /// The index of the game object in the container.
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteGameObjectCommand&lt;GameObjectType&gt;"/> class.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="container">The container where the game object is stored.</param>
        /// <param name="index">The index of the game object in the container.</param>
        public DeleteGameObjectCommand(GameObjectType gameObject, IList<GameObjectType> container, int index)
        {
            this.gameObject = gameObject;
            this.container = container;
            this.index = index;
        }

        /// <summary>
        /// Removes the game object from the container.
        /// </summary>
        public override void Do()
        {
            if (index < container.Count && container[index] == gameObject)
            {
                container.RemoveAt(index);
            }
            else
            {
                container.Remove(gameObject);
            }
        }

        /// <summary>
        /// Adds the game object to the container.
        /// </summary>
        public override void Undo()
        {
            if (index < container.Count)
            {
                container.Insert(index, gameObject);
            }
            else
            {
                container.Add(gameObject);
            }
        }
    }

    /// <summary>
    /// Command for adding the game object to the container where will be stored.
    /// </summary>
    /// <typeparam name="GameObjectType">The type of the game object.</typeparam>
    class AddGameObjectCommand<GameObjectType> : Command where GameObjectType : GameObject
    {
        /// <summary>
        /// Game object for this command.
        /// </summary>
        private GameObjectType gameObject;

        /// <summary>
        /// The container where the game object is stored.
        /// </summary>
        private IList<GameObjectType> container;

        /// <summary>
        /// The index of the game object in the container.
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddGameObjectCommand&lt;GameObjectType&gt;"/> class.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="container">The container where the game object will be stored.</param>
        /// <param name="index">The index of the game object in the container.</param>
        public AddGameObjectCommand(GameObjectType gameObject, IList<GameObjectType> container, int index)
        {
            this.gameObject = gameObject;
            this.container = container;
            this.index = index;
        }

        /// <summary>
        /// Adds the game object to the container.
        /// </summary>
        public override void Do()
        {
            if (index < container.Count)
            {
                container.Insert(index, gameObject);
            }
            else
            {
                container.Add(gameObject);
            }
        }

        /// <summary>
        /// Removes the game object from the container.
        /// </summary>
        public override void Undo()
        {
            if (index < container.Count && container[index] == gameObject)
            {
                container.RemoveAt(index);
            }
            else
            {
                container.Remove(gameObject);
            }
        }
    }
}
