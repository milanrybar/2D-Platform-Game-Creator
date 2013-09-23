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

namespace PlatformGameCreator.Editor.GameObjects.Paths
{
    /// <summary>
    /// Path that can be used in the game.
    /// </summary>
    [Serializable]
    class Path : GameObject, ISerializable
    {
        /// <summary>
        /// Gets the vertices of the path.
        /// </summary>
        public List<Vector2> Vertices
        {
            get { return _vertices; }
        }
        private List<Vector2> _vertices;

        /// <summary>
        /// Gets or sets a value indicating whether this path is looped.
        /// </summary>
        public bool Loop
        {
            get { return _loop; }
            set
            {
                _loop = value;
                if (LoopChanged != null) LoopChanged(this, EventArgs.Empty);
            }
        }
        private bool _loop;

        /// <summary>
        /// Occurs when the <see cref="Loop"/> property value changes.
        /// </summary>
        public event EventHandler LoopChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        public Path()
        {
            _vertices = new List<Vector2>();

            _name = String.Format("Path {0}", Id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class from the specified path instance.
        /// </summary>
        /// <param name="path">The path.</param>
        private Path(Path path)
        {
            _vertices = new List<Vector2>(path.Vertices);
            _name = path.Name;
            _loop = path.Loop;
        }

        /// <inheritdoc />
        protected Path(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _vertices = (List<Vector2>)info.GetValue("Vertices", typeof(List<Vector2>));
            _loop = info.GetBoolean("Loop");
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Vertices", Vertices);
            info.AddValue("Loop", Loop);
        }

        /// <summary>
        /// Creates the <see cref="PathView"/> representing this path for using at the scene.
        /// </summary>
        /// <returns>Created <see cref="PathView"/> of this game object.</returns>
        public override SceneNode CreateSceneView()
        {
            return new PathView(this);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Cloning is only possible for path of the current selected scene.
        /// If <paramref name="addToContainer"/> is <c>true</c> cloned path is added to the current selected scene.
        /// </remarks>
        public override GameObject Clone(bool addToContainer = false)
        {
            Path clonedPath = new Path(this);

            if (addToContainer)
            {
                Project.Singleton.Scenes.SelectedScene.Paths.Add(clonedPath);
            }

            return clonedPath;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Removing is only possible for path of the current selected scene.
        /// </remarks>
        public override void Remove()
        {
            Project.Singleton.Scenes.SelectedScene.Paths.Remove(this);
        }

        /// <inheritdoc />
        public override Command CreateDeleteCommand()
        {
            int index;
            if ((index = Project.Singleton.Scenes.SelectedScene.Paths.IndexOf(this)) != -1)
            {
                return new DeleteGameObjectCommand<Path>(this, Project.Singleton.Scenes.SelectedScene.Paths, index);
            }

            return null;
        }

        /// <inheritdoc />
        public override Command CreateAddCommand()
        {
            int index;
            if ((index = Project.Singleton.Scenes.SelectedScene.Paths.IndexOf(this)) != -1)
            {
                return new AddGameObjectCommand<Path>(this, Project.Singleton.Scenes.SelectedScene.Paths, index);
            }

            return null;
        }
    }
}
