/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Defines special graphics effect for a texture.
    /// </summary>
    enum SceneElementEffect
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
    /// Scene drawable element for the <see cref="SceneScreen"/>.
    /// </summary>
    interface ISceneDrawable
    {
        /// <summary>
        /// Draws the element (located by <paramref name="position"/> parameter, rotated by <paramref name="rotation"/> parameter and scaled by <paramref name="scale"/> parameter) by <paramref name="sceneBatch"/> parameter on the scene.
        /// </summary>
        /// <param name="sceneBatch">The scene batch for drawing the element.</param>
        /// <param name="position">The position of the element.</param>
        /// <param name="rotation">The rotation of the element.</param>
        /// <param name="scale">The scale of the element.</param>
        /// <param name="effect">Effect to apply to the graphics of the element.</param>
        void Draw(SceneBatch sceneBatch, Vector2 position, float rotation, Vector2 scale, SceneElementEffect effect);

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <param name="position">The position of the element.</param>
        /// <returns>Polygon representing bounds of the element. (typically rectangle).</returns>
        Polygon GetBounds(Vector2 position);
    }
}
