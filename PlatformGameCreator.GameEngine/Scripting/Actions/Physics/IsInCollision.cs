/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Physics
{
    /// <summary>
    /// Checks if the specified actor is in collision with any other actor.
    /// </summary>
    [FriendlyName("Is In Collision")]
    [Description("Checks if the specified actor is in collision with any other actor.")]
    [Category("Actions/Physics")]
    public class IsInCollisionAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to check if it is in collision.
        /// </summary>
        [Description("Actor to check if it is in collision.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Target;

        /// <summary>
        /// Indicates whether the specified actor is in collision with any other actor.
        /// </summary>
        [Description("Indicates whether the specified actor is in collision with any other actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<bool>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Target != null && Target.Value != null)
            {
                SetOutputVariable(Target.Value.InCollision(), Result);
            }

            if (Out != null) Out();
        }
    }
}
