/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scenes
{
    /// <summary>
    /// Defines special graphics effect for a texture.
    /// </summary>
    public enum GraphicsEffect
    {
        /// <summary>
        /// No special effect.
        /// </summary>
        None = 0,

        /// <summary>
        /// Texture is repeated horizontally.
        /// </summary>
        RepeatHorizontally = 1,

        /// <summary>
        /// Texture is repeated vertically.
        /// </summary>
        RepeatVertically = 2,

        /// <summary>
        /// Texture is repeated to fill the whole screen. 
        /// </summary>
        Fill = 4
    };

    /// <summary>
    /// Base class for scene node used at the scene.
    /// </summary>
    abstract public class SceneNode : IComparable<SceneNode>
    {
        /// <summary>
        /// Gets or sets the screen where is this <see cref="SceneNode"/> used.
        /// </summary>
        public SceneScreen Screen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SceneNode"/> is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets layer for drawing of the scene node.
        /// </summary>
        public float Layer { get; set; }

        /// <summary>
        /// Last number of the update cycle when the <see cref="SceneNode"/> was updated.
        /// </summary>
        public int UpdateCycle = -1;

        /// <summary>
        /// Called when the <see cref="SceneNode"/> needs to be updated. Override this method with node-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Called when the <see cref="SceneNode"/> needs to be drawn. Override this method with node-specific drawing code.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Called when the <see cref="SceneNode"/> should be initialized. Override this method with node-specific initializing code.
        /// </summary>
        public abstract void Initialize();

        /// <inheritdoc />
        /// <summary>
        /// Compares this <see cref="SceneNode"/> <see cref="Layer"/> with another <see cref="SceneNode"/> <see cref="Layer"/>.
        /// </summary>
        /// <param name="other">The <see cref="SceneNode"/> compare with this <see cref="SceneNode"/>.</param>
        public int CompareTo(SceneNode other)
        {
            return Layer.CompareTo(other.Layer);
        }
    }
}
