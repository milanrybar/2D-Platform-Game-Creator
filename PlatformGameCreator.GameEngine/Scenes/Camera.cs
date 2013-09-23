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
    /// Represents the camera used by the <see cref="SceneScreen"/>.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Gets or sets the width in pixels of the viewport.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels of the viewport.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets the projection matrix of the camera.
        /// </summary>
        public Matrix Projection
        {
            get { return _projection; }
        }
        private Matrix _projection = Matrix.Identity;

        /// <summary>
        /// Gets the world matrix of the camera.
        /// </summary>
        public Matrix World
        {
            get { return _world; }
        }
        private Matrix _world = Matrix.Identity;

        /// <summary>
        /// Gets the invers matrix of the world matrix of the camera.
        /// </summary>
        public Matrix InversWorld
        {
            get { return _inversWorld; }
        }
        private Matrix _inversWorld = Matrix.Identity;

        /// <summary>
        /// Gets the position in pixels of the camera.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
        }
        private Vector2 _position;

        /// <summary>
        /// Gets or sets the scale factor of the camera.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                if (_scale <= 0f) _scale = 0.01f;
                _inversScale = 1f / _scale;
            }
        }
        private float _scale = 1f;

        /// <summary>
        /// Gets the invers scale factor of the camera.
        /// </summary>
        public float InversScale
        {
            get { return _inversScale; }
        }
        private float _inversScale = 1f;

        /// <summary>
        /// Gets or sets the actor that the camera is centered by.
        /// </summary>
        public Actor Actor { get; set; }

        /// <summary>
        /// Updates the camera.
        /// Centers the camera by <see cref="Camera.Actor"/>, if any and updates matrices.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public void Update(GameTime gameTime)
        {
            if (Actor != null)
            {
                _position.X = ConvertUnits.ToDisplayUnits(Actor.Position.X) - Width * InversScale / 2f;
                _position.Y = ConvertUnits.ToDisplayUnits(Actor.Position.Y) - Height * InversScale / 2f;
            }

            _projection = Matrix.CreateOrthographicOffCenter(0f, Width, Height, 0f, 0f, 1f);

            _world = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) * Matrix.CreateScale(Scale, Scale, 0f);
            _inversWorld = Matrix.CreateScale(InversScale, InversScale, 0f) * Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }
    }
}
