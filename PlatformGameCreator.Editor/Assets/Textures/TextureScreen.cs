/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Screen for editing collision shapes and origin of the <see cref="Texture"/>.
    /// </summary>
    class TextureScreen : ShapesEditingScreen
    {
        /// <summary>
        /// Gets or sets the underlying texture.
        /// </summary>
        public Texture Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Invalidate();
            }
        }
        private Texture _texture;

        /// <inheritdoc />
        /// <summary>
        /// Paints the texture at the screen.
        /// </summary>
        protected override void PaintImage(PaintEventArgs pe)
        {
            if (Texture != null)
            {
                pe.Graphics.DrawImage(Texture.TextureGdi, -Texture.Origin.X, -Texture.Origin.Y);
            }
        }
    }
}
