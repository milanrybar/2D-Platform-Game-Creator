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
    /// View of list of <see cref="Texture">textures</see>.
    /// Everything is done automatically by setting manager (<see cref="BaseTexturesView{DrawableAssetType}.DrawableAssets"/>) of textures.
    /// </summary>
    class TexturesView : BaseTexturesView<Texture>
    {
        /// <summary>
        /// Gets the <see cref="Image"/> from the specified texture.
        /// </summary>
        /// <param name="texture">The texture to get image from.</param>
        /// <returns>Returns <see cref="Image"/> of the texture.</returns>
        protected override Image GetDrawableAssetImage(Texture texture)
        {
            return texture.TextureGdi;
        }

        /// <summary>
        /// Removes the specified texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        protected override void RemoveDrawableAsset(Texture texture)
        {
            if (new ConsistentDeletionForm(new ConsistentDeletionHelper.TextureForDeletion(texture)).ShowDialog() == DialogResult.OK)
            {
                Messages.ShowInfo("Texture deleted.");
            }
        }

        /// <summary>
        /// Opens editor (<see cref="TextureForm"/>) for the specified texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        protected override void OpenDrawableAsset(Texture texture)
        {
            new TextureForm(texture).ShowDialog();
        }
    }
}
