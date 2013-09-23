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
    /// Gets the linear velocity of the body of the specified actor.
    /// </summary>
    [FriendlyName("Get Linear Velocity")]
    [Description("Gets the linear velocity of the body of the specified actor.")]
    [Category("Actions/Physics")]
    public class GetLinearVelocityAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the linear velocity from.
        /// </summary>
        [Description("Actor to get the linear velocity from.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Instance;

        /// <summary>
        /// Outputs the linear velocity in Newtons (N) of the specified actor.
        /// </summary>
        [Description("Outputs the linear velocity in Newtons (N) of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Velocity;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Instance != null && Instance.Value != null && Instance.Value.Body != null)
            {
                SetOutputVariable(Instance.Value.Body.LinearVelocity, Velocity);
            }

            if (Out != null) Out();
        }
    }
}
