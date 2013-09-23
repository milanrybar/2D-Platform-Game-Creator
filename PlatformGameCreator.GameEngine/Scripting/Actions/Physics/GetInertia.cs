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
    /// Gets the rotational inertia of the body of the specified actor.
    /// </summary>
    [FriendlyName("Get Inertia")]
    [Description("Gets the rotational inertia of the body of the specified actor.")]
    [Category("Actions/Physics")]
    public class GetInertiaAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the inertia from.
        /// </summary>
        [Description("Actor to get the inertia from.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Target;

        /// <summary>
        /// Outputs the rotational inertia in kg-m^2 of the specified actor.
        /// </summary>
        [Description("Outputs the rotational inertia in kg-m^2 of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Inertia;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Target != null && Target.Value != null && Target.Value.Body != null)
            {
                SetOutputVariable(Target.Value.Body.Inertia, Inertia);
            }

            if (Out != null) Out();
        }
    }
}
