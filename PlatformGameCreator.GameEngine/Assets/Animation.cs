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
using PlatformGameCreator.GameEngine.Scripting;

namespace PlatformGameCreator.GameEngine.Assets
{
    /// <summary>
    /// Represents animation defined in the editor.
    /// </summary>
    public class AnimationData : IGraphicsAsset
    {
        /// <summary>
        /// Frames of the animation.
        /// </summary>
        public TextureData[] Textures;

        /// <summary>
        /// Speed in miliseconds per frame of the animation.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Indicates whether the animation is looped.
        /// </summary>
        public bool Loop;

        /// <summary>
        /// Creates the <see cref="AnimationActorRenderer"/> to render the specified actor by this animation.
        /// </summary>
        /// <param name="actor">The actor to render.</param>
        /// <returns>Created <see cref="AnimationActorRenderer"/> to render the specified actor.</returns>
        public ActorRenderer CreateActorRenderer(Actor actor)
        {
            AnimationActorRenderer actorRenderer = new AnimationActorRenderer(this, actor);
            actorRenderer.Loop = Loop;
            actorRenderer.Speed = Speed;
            return actorRenderer;
        }
    }

    /// <summary>
    /// Renders the specified actor by the specified animation.
    /// </summary>
    public class AnimationActorRenderer : ActorRenderer
    {
        /// <summary>
        /// Gets the <see cref="AnimationData"/> which is used for the actor.
        /// </summary>
        public override IGraphicsAsset GraphicsAsset
        {
            get { return animationData; }
        }

        /// <summary>
        /// Indicates whether the animation is looped.
        /// </summary>
        public bool Loop;

        /// <summary>
        /// Speed in miliseconds per frame of the animation.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Gets a value indicating whether the playing of the specified animation is finished.
        /// </summary>
        public bool Finish
        {
            get { return _finish; }
        }
        private bool _finish;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AnimationActorRenderer"/> is paused.
        /// </summary>
        public bool Pause
        {
            get { return _pause; }
            set
            {
                if (_pause != value && !_finish)
                {
                    _pause = value;
                }
            }
        }
        private bool _pause;

        /// <summary>
        /// Occurs when the playing of the specified animation is finished.
        /// </summary>
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Gets or sets the actual frame of the animation.
        /// </summary>
        private int ActualFrame
        {
            get { return _actualFrame; }
            set
            {
                _actualFrame = value;
                UpdateOrigin();
            }
        }
        private int _actualFrame;

        /// <summary>
        /// Animation which is used for the actor.
        /// </summary>
        private AnimationData animationData;

        /// <summary>
        /// Time in miliseconds elapsed from the playing the actual frame of the animation.
        /// </summary>
        private double timeElapsed;

        /// <summary>
        /// Origin of the animation. Value depends on the <see cref="ActorRenderer.spriteEffects"/> value.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationActorRenderer"/> class and starts the animation.
        /// </summary>
        /// <param name="animationData">The animation to use for the specified actor.</param>
        /// <param name="actor">The actor to render.</param>
        public AnimationActorRenderer(AnimationData animationData, Actor actor)
            : base(actor)
        {
            this.animationData = animationData;

            Start();
        }

        /// <summary>
        /// Starts the animation from the first frame.
        /// </summary>
        public void Start()
        {
            ActualFrame = 0;
            timeElapsed = 0.0;
            _finish = false;
            _pause = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// Updates the animation.
        /// When the animation is not looped and just finished then fires <see cref="Finished"/>.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (!Pause && !Finish)
            {
                timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeElapsed >= Speed)
                {
                    // change frame
                    timeElapsed -= Speed;
                    ++ActualFrame;

                    if (ActualFrame >= animationData.Textures.Length)
                    {
                        // end of the animation
                        if (!Loop)
                        {
                            _finish = true;
                            --ActualFrame;
                            if (Finished != null) Finished();
                        }
                        // loops animation
                        else
                        {
                            ActualFrame = 0;
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Draws the actor by the animation.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Draw(animationData.Textures[ActualFrame].Texture, ref origin);
        }

        /// <summary>
        /// Called when the <see cref="ActorRenderer.FlipHorizontally"/> or <see cref="ActorRenderer.FlipVertically"/> changes.
        /// Updates the <see cref="ActorRenderer.spriteEffects"/> value and <see cref="origin"/> value.
        /// </summary>
        protected override void UpdateSpriteEffects()
        {
            base.UpdateSpriteEffects();

            UpdateOrigin();
        }

        /// <summary>
        /// Updates the <see cref="origin"/> value.
        /// </summary>
        private void UpdateOrigin()
        {
            if (ActualFrame >= 0 && ActualFrame < animationData.Textures.Length)
            {
                origin = animationData.Textures[ActualFrame].Origin;
                if (FlipHorizontally) origin.X = animationData.Textures[ActualFrame].Texture.Width - origin.X;
                if (FlipVertically) origin.Y = animationData.Textures[ActualFrame].Texture.Height - origin.Y;
            }
        }
    }
}
