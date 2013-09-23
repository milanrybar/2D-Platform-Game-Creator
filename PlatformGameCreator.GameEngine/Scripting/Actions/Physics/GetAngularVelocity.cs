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

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Physics
{
    /// <summary>
    /// Gets the angular velocity of the body of the specified actor.
    /// </summary>
    [FriendlyName("Get Angular Velocity")]
    [Description("Gets the angular velocity of the body of the specified actor.")]
    [Category("Actions/Physics")]
    public class GetAngularVelocityAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the angular velocity from.
        /// </summary>
        [Description("Actor to get the angular velocity from.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Instance;

        /// <summary>
        /// Outputs the angular velocity in radians/second of the specified actor.
        /// </summary>
        [Description("Outputs the angular velocity in radians/second of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Velocity;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Instance != null && Instance.Value != null && Instance.Value.Body != null)
            {
                SetOutputVariable(Instance.Value.Body.AngularVelocity, Velocity);
            }

            if (Out != null) Out();
        }
    }
}
