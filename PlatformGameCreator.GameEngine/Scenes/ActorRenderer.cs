/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.GameEngine.Assets;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGameCreator.GameEngine.Scenes
{
    /// <summary>
    /// Represents renderer of the <see cref="Actor"/>.
    /// </summary>
    public abstract class ActorRenderer
    {
        /// <summary>
        /// Gets the actor to render.
        /// </summary>
        public Actor Actor
        {
            get { return _actor; }
        }
        private Actor _actor;

        /// <summary>
        /// Gets the graphics asset which is used for the actor.
        /// </summary>
        public abstract IGraphicsAsset GraphicsAsset { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the graphics of the actor is flipped horizontally.
        /// </summary>
        public bool FlipHorizontally
        {
            get { return _flipHorizontally; }
            set
            {
                _flipHorizontally = value;
                UpdateSpriteEffects();
            }
        }
        private bool _flipHorizontally;

        /// <summary>
        /// Gets or sets a value indicating whether the graphics of the actor is flipped vertically.
        /// </summary>
        public bool FlipVertically
        {
            get { return _flipVertically; }
            set
            {
                _flipVertically = value;
                UpdateSpriteEffects();
            }
        }
        private bool _flipVertically;

        /// <summary>
        /// <see cref="SpriteEffects"/> used for drawing a texture by <see cref="SpriteBatch"/>.
        /// </summary>
        protected SpriteEffects spriteEffects;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorRenderer"/> class.
        /// </summary>
        /// <param name="actor">The actor to render.</param>
        public ActorRenderer(Actor actor)
        {
            _actor = actor;
        }

        /// <summary>
        /// Called when the <see cref="ActorRenderer"/> needs to be updated. Override this method with asset-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Called when the <see cref="ActorRenderer"/> needs to be drawn. Override this method with asset-specific drawing code.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Called when the <see cref="FlipHorizontally"/> or <see cref="FlipVertically"/> changes.
        /// Updates the <see cref="spriteEffects"/> value.
        /// </summary>
        protected virtual void UpdateSpriteEffects()
        {
            spriteEffects = SpriteEffects.None;
            if (FlipHorizontally) spriteEffects |= SpriteEffects.FlipHorizontally;
            if (FlipVertically) spriteEffects |= SpriteEffects.FlipVertically;
        }

        /// <summary>
        /// Draws the specified texture for the actor.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="origin">The origin of the texture.</param>
        protected void Draw(Texture2D texture, ref Vector2 origin)
        {
            // actor position in display units
            Vector2 actorPosition = ConvertUnits.ToDisplayUnits(Actor.Position);

            // no effect
            if (Actor.GraphicsEffect == GraphicsEffect.None)
            {
                Actor.SpriteBatch.Draw(texture, actorPosition, null, Color.White, Actor.Angle, origin, Actor.Scale, spriteEffects, 0f);
            }
            // special graphics effect
            else
            {
                Vector2 textureSize = new Vector2(texture.Width * Actor.Scale.X, texture.Height * Actor.Scale.Y);

                if (Actor.Angle != 0f)
                {
                    Vector2[] rectangle = { new Vector2(), new Vector2(0, textureSize.Y), new Vector2(textureSize.X, 0), textureSize };
                    Matrix rotateTransform = Matrix.CreateRotationZ(Actor.Angle);
                    Vector2.Transform(rectangle, ref rotateTransform, rectangle);

                    Vector2 lowerBound = Vector2.Min(rectangle[0], Vector2.Min(rectangle[1], Vector2.Min(rectangle[2], rectangle[3])));
                    Vector2 upperBound = Vector2.Max(rectangle[0], Vector2.Max(rectangle[1], Vector2.Max(rectangle[2], rectangle[3])));

                    textureSize = new Vector2(Math.Abs(lowerBound.X - upperBound.X), Math.Abs(lowerBound.Y - upperBound.Y));
                }

                Vector2 scenePosition = Actor.Screen.Camera.Position;
                Vector2 sceneSize = new Vector2(Actor.Screen.Camera.Width * Actor.Screen.Camera.InversScale, Actor.Screen.Camera.Height * Actor.Screen.Camera.InversScale);

                Vector2 start = new Vector2(scenePosition.X - textureSize.X + ((actorPosition.X - scenePosition.X) % textureSize.X), scenePosition.Y - textureSize.Y + ((actorPosition.Y - scenePosition.Y) % textureSize.Y));
                Vector2 end = new Vector2(scenePosition.X + sceneSize.X + textureSize.X, scenePosition.Y + sceneSize.Y + textureSize.Y);

                // Fill
                if ((Actor.GraphicsEffect & GraphicsEffect.Fill) != 0)
                {
                    for (float x = start.X; x < end.X; x += textureSize.X)
                    {
                        for (float y = start.Y; y < end.Y; y += textureSize.Y)
                        {
                            Actor.SpriteBatch.Draw(texture, new Vector2(x, y), null, Color.White, Actor.Angle, origin, Actor.Scale, spriteEffects, 0f);
                        }
                    }
                }
                else
                {
                    // RepeatHorizontally
                    if ((Actor.GraphicsEffect & GraphicsEffect.RepeatHorizontally) != 0)
                    {
                        if (actorPosition.X - textureSize.X > start.X || actorPosition.X + textureSize.X < end.X)
                        {
                            for (float x = start.X; x < end.X; x += textureSize.X)
                            {
                                Actor.SpriteBatch.Draw(texture, new Vector2(x, actorPosition.Y), null, Color.White, Actor.Angle, origin, Actor.Scale, spriteEffects, 0f);
                            }
                        }
                    }

                    // RepeatVertically
                    if ((Actor.GraphicsEffect & GraphicsEffect.RepeatVertically) != 0)
                    {
                        if (actorPosition.Y - textureSize.Y > start.Y || actorPosition.Y + textureSize.Y < end.Y)
                        {
                            for (float y = start.Y; y < end.Y; y += textureSize.Y)
                            {
                                Actor.SpriteBatch.Draw(texture, new Vector2(actorPosition.X, y), null, Color.White, Actor.Angle, origin, Actor.Scale, spriteEffects, 0f);
                            }
                        }
                    }
                }
            }
        }
    }
}
