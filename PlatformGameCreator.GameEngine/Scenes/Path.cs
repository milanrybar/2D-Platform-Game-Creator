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
    /// Represents the path defined in the editor which is used at the scene.
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Indicates whether the path is looped.
        /// </summary>
        public bool Loop;

        /// <summary>
        /// Vertices of the path.
        /// </summary>
        public Vector2[] Vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        /// <param name="loop">Indicates whether the path is looped.</param>
        /// <param name="vertices">Vertices of the path.</param>
        public Path(bool loop, Vector2[] vertices)
        {
            Loop = loop;
            Vertices = vertices;
        }
    }
}
