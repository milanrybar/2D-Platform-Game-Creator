/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Gets the actor type of the specified actor.
    /// </summary>
    [FriendlyName("Get Actor Type")]
    [Description("Gets the actor type of the specified actor.")]
    [Category("Actions/Actors")]
    public class GetActorTypeAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the actor type from.
        /// </summary>
        [Description("Actor to get the actor type from.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Actor> Actor;

        /// <summary>
        /// Outputs the actor type of the specified actor.
        /// </summary>
        [FriendlyName("Actor Type")]
        [Description("Outputs the actor type of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<uint>[] ActorType;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Actor != null && Actor.Value != null)
            {
                SetOutputVariable(Actor.Value.ActorType, ActorType);
            }

            if (Out != null) Out();
        }
    }
}
