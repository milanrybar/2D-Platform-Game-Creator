/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Common;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Scenes;

namespace PlatformGameCreator.Editor.Assets
{
    /// <summary>
    /// Represent a drawable asset that can be used in game.
    /// </summary>
    /// <remarks>
    /// Drawable asset contains list of collision shapes (see <see cref="Shape"/>).
    /// </remarks>
    [Serializable]
    abstract class DrawableAsset : Asset
    {
        /// <summary>
        /// Gets the collision shapes of the drawable asset.
        /// </summary>
        public ObservableList<Shape> Shapes
        {
            get { return _shapes; }
        }
        private ObservableList<Shape> _shapes;

        /// <summary>
        /// Occurs when some important value (for appearance) of drawable asset changes.
        /// </summary>
        /// <remarks>
        /// Occurs when the collision shapes of the drawable asset changes or some important value in derived class changes.
        /// For example: <see cref="Textures.Texture.Origin"/> in <see cref="Textures.Texture"/>. <see cref="Animations.Animation.Frames"/> or <see cref="Animations.Animation.Speed"/> in <see cref="Animations.Animation"/>.
        /// 
        /// It usually occurs after editing drawable asset (<see cref="Textures.Texture"/> in <see cref="Textures.TextureForm"/> or <see cref="Animations.Animation"/> in <see cref="Animations.AnimationForm"/>).
        /// </remarks>
        public event EventHandler DrawableAssetChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableAsset"/> class.
        /// </summary>
        protected DrawableAsset()
        {
            _shapes = new ObservableList<Shape>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableAsset"/> class.
        /// </summary>
        protected DrawableAsset(int id)
            : base(id)
        {
            _shapes = new ObservableList<Shape>();
        }

        /// <inheritdoc />
        protected DrawableAsset(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _shapes = (ObservableList<Shape>)info.GetValue("Shapes", typeof(ObservableList<Shape>));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Shapes", Shapes);
        }

        /// <summary>
        /// Invokes the <see cref="DrawableAssetChanged"/> event.
        /// </summary>
        public void InvokeDrawableAssetChanged()
        {
            if (DrawableAssetChanged != null) DrawableAssetChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates the drawable element for the <see cref="SceneScreen"/>.
        /// </summary>
        /// <returns>Drawable element for the <see cref="SceneScreen"/></returns>
        public abstract ISceneDrawable CreateView();
    }
}
