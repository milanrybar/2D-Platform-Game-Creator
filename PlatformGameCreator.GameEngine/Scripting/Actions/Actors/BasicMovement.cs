/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Provides basic movement for the specified actor. 
    /// Should be used only when quick testing is needed because its <see cref="SlowingCoefficient"/> and <see cref="GradualAcceleration"/> property has negative behaviour on moving platform.
    /// </summary>
    [FriendlyName("Basic Movement")]
    [Description("Provides basic movement for the specified actor. Should be used only when quick testing is needed because its SlowingCoefficient and GradualAcceleration property has negative behaviour on moving platform.")]
    [Category("Actions/Actors")]
    public class BasicMovementAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor in physics simulation to apply basic movement.
        /// </summary>
        [Description("Actor in physics simulation to apply basic movement.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Instance;

        /// <summary>
        /// Indicates whether the actor is moving to the left.
        /// </summary>
        [FriendlyName("Moving Left")]
        [Description("Indicates whether the actor is moving to the left.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(false)]
        public Variable<bool> MovingLeft;

        /// <summary>
        /// Indicates whether the actor is moving to the right.
        /// </summary>
        [FriendlyName("Moving Right")]
        [Description("Indicates whether the actor is moving to the right.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(false)]
        public Variable<bool> MovingRight;

        /// <summary>
        /// Speed in meters per seconds to move the actor in the left.
        /// </summary>
        [FriendlyName("Left Speed")]
        [Description("Speed in meters per seconds to move the actor in the left.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(1f)]
        public Variable<float> LeftSpeed;

        /// <summary>
        /// Speed in meters per seconds to move the actor in the right.
        /// </summary>
        [FriendlyName("Right Speed")]
        [Description("Speed in meters per seconds to move the actor in the right.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(1f)]
        public Variable<float> RightSpeed;

        /// <summary>
        /// Slowing coefficient to simulate slowing down of the actor.
        /// </summary>
        [FriendlyName("Slowing Coefficient")]
        [Description("Slowing coefficient to simulate slowing down of the actor.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(0.9f)]
        public Variable<float> SlowingCoefficient;

        /// <summary>
        /// Simulates gradual acceleration of the actor. The actor accelerate by this value to max speed.
        /// </summary>
        [FriendlyName("Gradual Acceleration")]
        [Description("Simulates gradual acceleration of the actor. The actor accelerate by this value to max speed.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(0.5f)]
        public Variable<float> GradualAcceleration;

        // indicates whether the actor is moving
        private bool moving = false;
        // last used (desired) velocity for the actor
        private float lastVelocity;

        /// <summary>
        /// Apply one-time movement of the specified actor by specified settings.
        /// </summary>
        /// <remarks>
        /// Moving at constant speed is inspired by http://www.iforce2d.net/b2dtut/constant-speed
        /// </remarks>
        [Description("Apply one-time movement of the specified actor by specified settings.")]
        public void In()
        {
            if (Instance != null && Instance.Value != null && Instance.Value.Body != null)
            {
                float actorVelocityX = Instance.Value.Body.LinearVelocity.X;
                float desiredVelocity = 0f;

                // moving left
                if (MovingLeft.Value && !MovingRight.Value)
                {
                    desiredVelocity = MathHelper.Max(actorVelocityX - GradualAcceleration.Value, -LeftSpeed.Value);

                    moving = true;
                    lastVelocity = desiredVelocity;
                }
                // moving right
                else if (!MovingLeft.Value && MovingRight.Value)
                {
                    desiredVelocity = MathHelper.Min(actorVelocityX + GradualAcceleration.Value, RightSpeed.Value);

                    moving = true;
                    lastVelocity = desiredVelocity;
                }
                // slowing down (moving left and right, or not moving)
                else if (moving)
                {
                    desiredVelocity = actorVelocityX * SlowingCoefficient.Value;

                    // slowing down to zero speed
                    // it takes more time to slow down on moving platform => negative behaviour
                    lastVelocity *= SlowingCoefficient.Value;
                    if ((lastVelocity > -0.01f && lastVelocity < 0.01f) || (desiredVelocity > -0.001f && desiredVelocity < 0.001f))
                    {
                        moving = false;
                    }
                }

                if (moving)
                {
                    Instance.Value.Body.ApplyLinearImpulse(Instance.Value.Body.Mass * new Vector2(desiredVelocity - actorVelocityX, 0f));
                }
            }

            if (Out != null) Out();
        }
    }
}
