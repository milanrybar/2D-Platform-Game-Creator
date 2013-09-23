/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Assets.Textures;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Scenes;
using Microsoft.Xna.Framework;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Animations
{
    /// <summary>
    /// Simple animation that can be used in game.
    /// </summary>
    /// <remarks>
    /// Current implementation of the animation has several limitations:
    /// <list type="bullet">
    ///     <item>
    ///     <description>Every frame (represented by <see cref="Texture"/>) is displayed at initial position and by its <see cref="Texture.Origin"/>. It is not possible to change any frame (change position, rotate or scale).</description>
    ///     </item>
    ///     <item>
    ///     <description>Every frame is displayed by the same amount of time define by <see cref="Animation.Speed"/> of the animation.</description>
    ///     </item>
    /// </list>
    /// 
    /// Abstract origin of the animation is at (0, 0) and every frame has its origin positioned at the animation origin.
    /// </remarks>
    [Serializable]
    class Animation : DrawableAsset, ISerializable
    {
        /// <inheritdoc />
        public override AssetType Type
        {
            get { return AssetType.Animation; }
        }

        /// <summary>
        /// Gets the frames of the animation. Every frame is <see cref="Textures.Texture"/> class.
        /// </summary>
        public List<Texture> Frames
        {
            get { return _frames; }
        }
        private List<Texture> _frames;

        /// <summary>
        /// Gets or sets the speed of the animation in miliseconds.
        /// </summary>
        /// <value>
        /// Time in miliseconds per one frame. Default value is 170.
        /// </value>
        public uint Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (SpeedChanged != null) SpeedChanged(this, EventArgs.Empty);
            }
        }
        private uint _speed;

        /// <summary>
        /// Occurs when the Speed value changes.
        /// </summary>
        public event EventHandler SpeedChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        public Animation()
        {
            _frames = new List<Texture>();
            _speed = 170;
        }

        /// <inheritdoc />
        protected Animation(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _frames = (List<Texture>)info.GetValue("Frames", typeof(List<Texture>));
            _speed = info.GetUInt32("Speed");
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Frames", Frames);
            info.AddValue("Speed", Speed);
        }

        /// <inheritdoc />
        public override ISceneDrawable CreateView()
        {
            return new AnimationSceneDrawable(this);
        }

        /// <summary>
        /// Implements <see cref="ISceneDrawable"/> that represents drawable element for the <see cref="SceneScreen"/>.
        /// </summary>
        private class AnimationSceneDrawable : ISceneDrawable
        {
            /// <summary>
            /// Gets the animation.
            /// </summary>
            public Animation Animation
            {
                get { return _animation; }
            }
            private Animation _animation;

            /// <summary>
            /// Actual frame of the playing animation.
            /// </summary>
            private int actualFrame;
            private double elapsedTime;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnimationSceneDrawable"/> class.
            /// </summary>
            /// <param name="animation">The animation to play.</param>
            public AnimationSceneDrawable(Animation animation)
            {
                _animation = animation;
            }

            /// <summary>
            /// Draws the animation (located by <paramref name="position"/> parameter, rotated by <paramref name="rotation"/> parameter and scaled by <paramref name="scale"/> parameter) by <paramref name="sceneBatch"/> parameter on the scene.
            /// </summary>
            /// <param name="sceneBatch">The scene batch for drawing the animation.</param>
            /// <param name="position">The position of the animation.</param>
            /// <param name="rotation">The rotation of the animation.</param>
            /// <param name="scale">The scale of the animation.</param>
            /// <param name="effect">Effect to apply to the animation.</param>
            public void Draw(SceneBatch sceneBatch, Vector2 position, float rotation, Vector2 scale, SceneElementEffect effect)
            {
                if (sceneBatch.GameTime != null) elapsedTime += sceneBatch.GameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsedTime >= Animation.Speed)
                {
                    ++actualFrame;
                    elapsedTime -= Animation.Speed;

                    if (actualFrame >= Animation.Frames.Count)
                    {
                        actualFrame = 0;
                    }
                }

                if (actualFrame < Animation.Frames.Count)
                {
                    sceneBatch.DrawTexture(Animation.Frames[actualFrame].TextureXna, ref position, rotation, ref scale, Animation.Frames[actualFrame].Origin, effect);
                }
            }

            /// <summary>
            /// Gets the bounds of the animation.
            /// </summary>
            /// <param name="position">The position of the animation.</param>
            /// <returns>Polygon representing bounds of the first frame or null when the animation has no frame.</returns>
            public Polygon GetBounds(Vector2 position)
            {
                if (Animation.Frames.Count != 0)
                {
                    return Polygon.CreateAsRectangle(position - Animation.Frames[0].Origin, Animation.Frames[0].Width, Animation.Frames[0].Height);
                }
                else return null;
            }
        }
    }
}
