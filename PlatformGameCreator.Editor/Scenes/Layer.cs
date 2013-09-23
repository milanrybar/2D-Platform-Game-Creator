/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using PlatformGameCreator.Editor.Common;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.GameObjects.Actors;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Represents layer of the scene.
    /// </summary>
    [Serializable]
    class Layer : ObservableIndexedList<Actor>, IName, IVisible, IIndex, ISerializable
    {
        /// <summary>
        /// Gets the scene where the layer is used.
        /// </summary>
        public Scene Scene
        {
            get { return _scene; }
        }
        private Scene _scene;

        /// <summary>
        /// Gets or sets the name of the layer.
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
        private string _name;

        /// <summary>
        /// Gets or sets a value indicating whether the layer and all its actors are visible at the scene.
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
        /// Gets or sets the index of the layer at the scene.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets a value indicating whether the layer is parallax layer.
        /// </summary>
        public bool IsParallax
        {
            get { return _isParallax; }
        }
        private bool _isParallax;

        /// <summary>
        /// Parallax coefficient for actors when the layer is parallax layer.
        /// </summary>
        public Vector2 ParallaxCoefficient = Vector2.Zero;

        /// <summary>
        /// Special graphics effect for actors when the layer is parallax layer.
        /// </summary>
        public SceneElementEffect GraphicsEffect = SceneElementEffect.None;

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        /// <param name="scene">The scene where will be the layer stored.</param>
        /// <param name="isParallax">If set to <c>true</c> the layer will be parallax layer.</param>
        public Layer(Scene scene, bool isParallax = false)
        {
            _scene = scene;
            _isParallax = isParallax;
        }

        /// <inheritdoc />
        protected Layer(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _scene = (Scene)info.GetValue("Scene", typeof(Scene));
            _name = info.GetString("Name");
            _visible = info.GetBoolean("Visible");

            _isParallax = info.GetBoolean("ParallaxLayer");
            ParallaxCoefficient = (Vector2)info.GetValue("ParallaxCoefficient", typeof(Vector2));
            GraphicsEffect = (SceneElementEffect)info.GetValue("GraphicsEffect", typeof(SceneElementEffect));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Scene", Scene);
            info.AddValue("Name", Name);
            info.AddValue("Visible", Visible);
            info.AddValue("ParallaxLayer", IsParallax);
            info.AddValue("ParallaxCoefficient", ParallaxCoefficient);
            info.AddValue("GraphicsEffect", GraphicsEffect);
        }

        /// <inheritdoc />
        public override void Add(Actor item)
        {
            if (item == null) throw new ArgumentNullException("Actor cannot be null.");

            item.Layer = this;
            base.Add(item);
        }

        /// <inheritdoc />
        public override void Insert(int index, Actor item)
        {
            if (item == null) throw new ArgumentNullException("Actor cannot be null.");

            item.Layer = this;
            base.Insert(index, item);
        }

        /// <inheritdoc />
        public override void RemoveAt(int index)
        {
            this[index].Layer = null;
            base.RemoveAt(index);
        }

        /// <summary>
        /// Finds the actor by the specified id at the scene. Also is finding at the actor children.
        /// </summary>
        /// <param name="id">The id of the actor to find.</param>
        /// <returns>Actor if found; otherwise <c>null</c>.</returns>
        public Actor FindById(int id)
        {
            Actor result = null;

            foreach (Actor actor in this)
            {
                if (actor.Id == id)
                {
                    return actor;
                }

                result = actor.Children.FindById(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the name of the layer.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
