/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Assets
{
    /// <summary>
    /// Represents texture defined in the editor.
    /// </summary>
    public class TextureData : IGraphicsAsset
    {
        /// <summary>
        /// Texture (image).
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Origin of the texture.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// Creates the <see cref="TextureActorRenderer"/> to render the specified actor by this texture.
        /// </summary>
        /// <param name="actor">The actor to render.</param>
        /// <returns>Created <see cref="TextureActorRenderer"/> to render the specified actor.</returns>
        public ActorRenderer CreateActorRenderer(Actor actor)
        {
            return new TextureActorRenderer(this, actor);
        }
    }

    /// <summary>
    /// Renders the specified actor by the specified texture.
    /// </summary>
    public class TextureActorRenderer : ActorRenderer
    {
        /// <summary>
        /// Gets the <see cref="TextureData"/> which is used for the actor.
        /// </summary>
        public override IGraphicsAsset GraphicsAsset
        {
            get { return textureData; }
        }

        /// <summary>
        /// Texture which is used for the actor.
        /// </summary>
        private TextureData textureData;

        /// <summary>
        /// Origin of the texture. Value depends on the <see cref="ActorRenderer.spriteEffects"/> value.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureActorRenderer"/> class.
        /// </summary>
        /// <param name="textureData">The texture to use for the specified actor.</param>
        /// <param name="actor">The actor to render.</param>
        public TextureActorRenderer(TextureData textureData, Actor actor)
            : base(actor)
        {
            this.textureData = textureData;
            origin = textureData.Origin;
        }

        /// <inheritdoc />
        /// <summary>
        /// Not used for the texture. Nothing to do.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Draws the actor by the texture.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Draw(textureData.Texture, ref origin);
        }

        /// <summary>
        /// Called when the <see cref="ActorRenderer.FlipHorizontally"/> or <see cref="ActorRenderer.FlipVertically"/> changes.
        /// Updates the <see cref="ActorRenderer.spriteEffects"/> value and <see cref="origin"/> value.
        /// </summary>
        protected override void UpdateSpriteEffects()
        {
            base.UpdateSpriteEffects();

            origin = textureData.Origin;
            if (FlipHorizontally) origin.X = textureData.Texture.Width - origin.X;
            if (FlipVertically) origin.Y = textureData.Texture.Height - origin.Y;
        }
    }
}
