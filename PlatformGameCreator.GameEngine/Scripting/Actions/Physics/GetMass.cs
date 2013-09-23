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
    /// Gets the mass of the body of the specified actor.
    /// </summary>
    [FriendlyName("Get Mass")]
    [Description("Gets the mass of the body of the specified actor.")]
    [Category("Actions/Physics")]
    public class GetMassAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the mass from.
        /// </summary>
        [Description("Actor to get the mass from.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Target;

        /// <summary>
        /// Outputs the mass in kilograms (kg) of the specified actor.
        /// </summary>
        [Description("Outputs the mass in kilograms (kg) of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Mass;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Target != null && Target.Value != null && Target.Value.Body != null)
            {
                SetOutputVariable(Target.Value.Body.Mass, Mass);
            }

            if (Out != null) Out();
        }
    }
}
