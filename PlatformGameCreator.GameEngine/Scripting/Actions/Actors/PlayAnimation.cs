/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Assets;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Plays animation. Sets the specified actors graphics to the specified animation.
    /// </summary>
    [FriendlyName("Play Animation")]
    [Description("Plays animation. Sets the specified actors graphics to the specified animation.")]
    [Category("Actions/Actors")]
    public class PlayAnimationAction : ActionNode
    {
        /// <summary>
        /// Fires when the animation is set to the actors.
        /// </summary>
        [Description("Fires when the animation is set to the actors.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Fires when the playing of the animation is finished.
        /// </summary>
        [Description("Fires when the playing of the animation is finished.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Actors to set the specified animation to.
        /// </summary>
        [Description("Actors to set the specified animation to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Animation to be used as graphics in the specified actors.
        /// </summary>
        [Description("Animation to be used as graphics in the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<AnimationData> Animation;

        /// <summary>
        /// Indicates whether the animation will be looped.
        /// </summary>
        [Description("Indicates whether the animation will be looped.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(false)]
        public Variable<bool> Loop;

        /// <summary>
        /// Speed in miliseconds per frame of the animation. Value 0 use the set speed of the animation.
        /// </summary>
        [Description("Speed in miliseconds per frame of the animation. Value 0 use the set speed of the animation.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(0f)]
        public Variable<float> Speed;

        /// <summary>
        /// Indicates whether the animation will be flipped horizontally.
        /// </summary>
        [FriendlyName("Flip Horizontally")]
        [Description("Indicates whether the animation will be flipped horizontally.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<bool> FlipHorizontally;

        /// <summary>
        /// Indicates whether the animation will be flipped vertically.
        /// </summary>
        [FriendlyName("Flip Vertically")]
        [Description("Indicates whether the animation will be flipped vertically.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<bool> FlipVertically;

        // last set actor renderer (set animation as graphics for the actor)
        private AnimationActorRenderer lastActorRenderer;

        /// <summary>
        /// Begins playing the specified animations as graphics in the specified actors.
        /// </summary>
        [Description("Begins playing the specified animations as graphics in the specified actors.")]
        public void Play()
        {
            if (Animation != null && Animation.Value != null && Instance != null)
            {
                for (int i = 0; i < Instance.Length; ++i)
                {
                    if (Instance[i].Value != null)
                    {
                        // the same animations is already used, only set node settings
                        if (Instance[i].Value.Renderer != null && Instance[i].Value.Renderer.GraphicsAsset == Animation.Value)
                        {
                            lastActorRenderer = (AnimationActorRenderer)Instance[i].Value.Renderer;

                            if (Finished != null && !Loop.Value)
                            {
                                lastActorRenderer.Finished -= AnimationFinished;
                                lastActorRenderer.Finished += AnimationFinished;
                            }
                        }
                        // create graphics for the actor
                        else
                        {
                            lastActorRenderer = (AnimationActorRenderer)Animation.Value.CreateActorRenderer(Instance[i].Value);

                            if (Finished != null && !Loop.Value) lastActorRenderer.Finished += AnimationFinished;
                        }

                        lastActorRenderer.Loop = Loop.Value;
                        lastActorRenderer.FlipHorizontally = FlipHorizontally.Value;
                        lastActorRenderer.FlipVertically = FlipVertically.Value;
                        if (Speed.Value != 0f && Speed.Value > 0f) lastActorRenderer.Speed = Speed.Value;

                        lastActorRenderer.Start();

                        Instance[i].Value.Renderer = lastActorRenderer;
                    }
                }
            }

            if (Out != null) Out();
        }

        /// <summary>
        /// Pauses playing the animation in the last set actor.
        /// </summary>
        [Description("Pauses playing the animation in the last set actor.")]
        public void Pause()
        {
            if (lastActorRenderer != null)
            {
                lastActorRenderer.Pause = !lastActorRenderer.Pause;
            }

            if (Out != null) Out();
        }

        /// <summary>
        /// Animation has finished playing.
        /// </summary>
        private void AnimationFinished()
        {
            if (Finished != null) Finished();
        }
    }
}
