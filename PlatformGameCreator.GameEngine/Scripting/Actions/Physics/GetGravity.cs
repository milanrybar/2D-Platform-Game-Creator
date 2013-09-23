/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Physics
{
    /// <summary>
    /// Gets the gravity from the physics simulation world.
    /// </summary>
    [FriendlyName("Get Gravity")]
    [Description("Gets the gravity from the physics simulation world.")]
    [Category("Actions/Physics")]
    public class GetGravityAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Outputs the gravity in units of Newtons (N) from the world.
        /// </summary>
        [Description("Outputs the gravity in units of Newtons (N) from the world.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Gravity;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Container.Actor.Screen.World.Gravity, Gravity);

            if (Out != null) Out();
        }
    }
}
