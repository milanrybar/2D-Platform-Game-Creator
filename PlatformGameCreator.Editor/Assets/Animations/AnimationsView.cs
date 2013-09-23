/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Assets.Textures;
using System.Drawing;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Animations
{
    /// <summary>
    /// View of list of <see cref="Animation">animations</see>.
    /// Everything is done automatically by setting manager (<see cref="BaseTexturesView{DrawableAssetType}.DrawableAssets"/>) of animations.
    /// </summary>
    class AnimationsView : BaseTexturesView<Animation>
    {
        /// <summary>
        /// Item of <see cref="Animation"/> for using in the <see cref="ListView"/>.
        /// </summary>
        private class AnimationListViewItem : AssetListViewItem<Animation>
        {
            /// <summary>
            /// <see cref="AnimationsView"/> where is the item used.
            /// </summary>
            private AnimationsView animationsView;

            /// <summary>
            /// First frame of the animation. Important for creating thumbnail.
            /// </summary>
            private Texture currentFirstFrame;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnimationListViewItem"/> class.
            /// </summary>
            /// <param name="animation">The animation.</param>
            /// <param name="animationsView"><see cref="AnimationsView"/> where is the item used.</param>
            public AnimationListViewItem(Animation animation, AnimationsView animationsView)
                : base(animation)
            {
                this.animationsView = animationsView;

                asset.DrawableAssetChanged += new EventHandler(animation_DrawableAssetChanged);

                if (asset.Frames.Count != 0) currentFirstFrame = asset.Frames[0];
            }

            /// <summary>
            /// Removes the item from its associated <see cref="T:System.Windows.Forms.ListView"/> control.
            /// </summary>
            public override void Remove()
            {
                asset.DrawableAssetChanged -= new EventHandler(animation_DrawableAssetChanged);
                animationsView = null;

                base.Remove();
            }

            /// <summary>
            /// Called when the animations changes.
            /// Updates thumbnail if necessary.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            private void animation_DrawableAssetChanged(object sender, EventArgs e)
            {
                if (asset.Frames.Count != 0)
                {
                    if (asset.Frames[0] != currentFirstFrame)
                    {
                        currentFirstFrame = asset.Frames[0];
                        animationsView.listView.LargeImageList.Images[animationsView.listView.LargeImageList.Images.IndexOfKey(asset.Id.ToString())] = asset.Frames[0].TextureGdi.CreateThumbnail(animationsView.listView.LargeImageList.ImageSize.Width, animationsView.listView.LargeImageList.ImageSize.Height);
                    }
                }
                else
                {
                    if (currentFirstFrame != null)
                    {
                        currentFirstFrame = null;
                        animationsView.listView.LargeImageList.Images[animationsView.listView.LargeImageList.Images.IndexOfKey(asset.Id.ToString())] = animationsView.EmptyThumbnail;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the empty thumbnail which is used when the animation has no frame.
        /// </summary>
        private Image EmptyThumbnail
        {
            get
            {
                if (_emptyThumbnail == null)
                {
                    int thumbWidth = listView.LargeImageList.ImageSize.Width;
                    int thumbHeight = listView.LargeImageList.ImageSize.Height;
                    _emptyThumbnail = new Bitmap(thumbWidth, thumbHeight);
                }

                return _emptyThumbnail;
            }
        }
        private Image _emptyThumbnail;

        /// <summary>
        /// Shows the specified animation in the ListView.
        /// </summary>
        /// <param name="drawableAsset">The animation to show.</param>
        protected override void ShowItem(Animation drawableAsset)
        {
            if (!listView.LargeImageList.Images.ContainsKey(drawableAsset.Id.ToString()))
            {
                listView.LargeImageList.Images.Add(drawableAsset.Id.ToString(), GetDrawableAssetImage(drawableAsset) != null ? GetDrawableAssetImage(drawableAsset).CreateThumbnail(listView.LargeImageList.ImageSize.Width, listView.LargeImageList.ImageSize.Height) : EmptyThumbnail);
            }

            listView.Items.Add(new AnimationListViewItem(drawableAsset, this));
        }

        /// <summary>
        /// Gets the <see cref="Image"/> from the specified animation.
        /// </summary>
        /// <param name="animation">The animation to get image from.</param>
        /// <returns>Returns <see cref="Image"/> of the animation.</returns>
        protected override Image GetDrawableAssetImage(Animation animation)
        {
            return animation.Frames.Count != 0 ? animation.Frames[0].TextureGdi : null;
        }

        /// <summary>
        /// Removes the specified animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        protected override void RemoveDrawableAsset(Animation animation)
        {
            if (new ConsistentDeletionForm(new ConsistentDeletionHelper.AnimationForDeletion(animation)).ShowDialog() == DialogResult.OK)
            {
                Messages.ShowInfo("Animation deleted.");
            }
        }

        /// <summary>
        /// Opens editor (<see cref="AnimationForm"/>) for the specified animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        protected override void OpenDrawableAsset(Animation animation)
        {
            new AnimationForm(animation).ShowDialog();
        }
    }
}
