/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using PlatformGameCreator.Editor.Common;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Scenes;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Texture that can be used in game.
    /// </summary>
    [Serializable]
    class Texture : DrawableAsset, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override AssetType Type
        {
            get { return AssetType.Texture; }
        }

        /// <summary>
        /// Gets the width of texture in pixels.
        /// </summary>
        public int Width
        {
            get { return TextureXna.Width; }
        }

        /// <summary>
        /// Gets the height of texture in pixels.
        /// </summary>
        public int Height
        {
            get { return TextureXna.Height; }
        }

        /// <summary>
        /// Gets this texture in <see cref="Image"/> format for use in WinForms.
        /// </summary>
        public Image TextureGdi
        {
            get { return _textureGdi; }
        }
        private Image _textureGdi;

        /// <summary>
        /// Gets this texture in <see cref="Texture2D"/> format for use in XNA.
        /// </summary>
        public Texture2D TextureXna
        {
            get { return _textureXna; }
        }
        private Texture2D _textureXna;

        /// <summary>
        /// Gets or sets the origin of the texture.
        /// </summary>
        /// <value>
        /// The most important settings of the texture.
        /// </value>
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                if (OriginChanged != null) OriginChanged(this, EventArgs.Empty);
            }
        }
        private Vector2 _origin;

        /// <summary>
        /// Occurs when the Origin value changes.
        /// </summary>
        public event EventHandler OriginChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="texture">The texture in XNA format.</param>
        /// <param name="id">The unique id for the texture.</param>
        public Texture(Texture2D texture, int id)
            : base(id)
        {
            _textureXna = texture;
            _textureGdi = texture.ToImage();

            // default origin is in the middle of the texture
            _origin = new Vector2(TextureGdi.Width / 2f, TextureGdi.Height / 2f);
        }

        /// <inheritdoc />
        protected Texture(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _origin = (Vector2)info.GetValue("Origin", typeof(Vector2));
        }

        /// <inheritdoc />
        /// <remarks>
        /// Loads the texture image after deserialization. It must be called after <see cref="TexturesManager"/> has been completely deserialized.
        /// Checks if the texture file exists, if not then the texture is removed from the project.
        /// </remarks>
        public void OnDeserialization(object sender)
        {
            try
            {
                _textureXna = Project.Singleton.Textures.LoadXnaTextureById(Id);
                _textureGdi = _textureXna.ToImage();
            }
            catch
            {
                Messages.ShowError(String.Format(@"Unable to load texture ""{0}"". Texture will be removed from project.", Name));

                if (new ConsistentDeletionForm(new ConsistentDeletionHelper.TextureForDeletion(this)) { AllowCancel = false, ProcessWhenEmptyList = true }.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Messages.ShowInfo("Texture deleted.");
                }
                else
                {
                    Debug.Assert(true);
                }
            }
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Origin", Origin);
        }

        /// <inheritdoc />
        public override ISceneDrawable CreateView()
        {
            return new TextureSceneDrawable(this);
        }

        /// <summary>
        /// Implements <see cref="ISceneDrawable"/> that represents drawable element for the <see cref="SceneScreen"/>.
        /// </summary>
        private class TextureSceneDrawable : ISceneDrawable
        {
            /// <summary>
            /// Gets the texture.
            /// </summary>
            public Texture Texture
            {
                get { return _texture; }
            }
            private Texture _texture;

            /// <summary>
            /// Initializes a new instance of the <see cref="TextureSceneDrawable"/> class.
            /// </summary>
            /// <param name="texture">The texture to show.</param>
            public TextureSceneDrawable(Texture texture)
            {
                _texture = texture;
            }

            /// <summary>
            /// Draws the texture (located by <paramref name="position"/> parameter, rotated by <paramref name="rotation"/> parameter and scaled by <paramref name="scale"/> parameter) by <paramref name="sceneBatch"/> parameter on the scene.
            /// </summary>
            /// <param name="sceneBatch">The scene batch for drawing the texture.</param>
            /// <param name="position">The position of the texture.</param>
            /// <param name="rotation">The rotation of the texture.</param>
            /// <param name="scale">The scale of the texture.</param>
            /// <param name="effect">Effect to apply to the texture.</param>
            public void Draw(SceneBatch sceneBatch, Vector2 position, float rotation, Vector2 scale, SceneElementEffect effect)
            {
                sceneBatch.DrawTexture(Texture.TextureXna, ref position, rotation, ref scale, Texture.Origin, effect);
            }

            /// <summary>
            /// Gets the bounds of the texture.
            /// </summary>
            /// <param name="position">The position of the texture.</param>
            /// <returns>Polygon representing bounds of the texture (typically rectangle).</returns>
            public Polygon GetBounds(Vector2 position)
            {
                return Polygon.CreateAsRectangle(position - Texture.Origin, Texture.Width, Texture.Height);
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Texture2D"/>.
    /// </summary>
    static class Texture2DExtension
    {
        /// <summary>
        /// Convert this <see cref="Texture2D"/> to <see cref="Image"/> format.
        /// </summary>
        /// <param name="texture">The texture to convert.</param>
        /// <returns>Texture in <see cref="Image"/> format or null when texture is invalid.</returns>
        public static Image ToImage(this Texture2D texture)
        {
            if (texture == null || texture.IsDisposed)
            {
                return null;
            }

            // Memory stream to store the bitmap data.
            using (MemoryStream ms = new MemoryStream())
            {
                // Save the texture to the stream.
                texture.SaveAsPng(ms, texture.Width, texture.Height);

                // Seek the beginning of the stream.
                ms.Seek(0, SeekOrigin.Begin);

                // Create an image from a stream.
                Image image = Bitmap.FromStream(ms);

                return image;
            }
        }
    }
}
