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
    /// Sets the gravity to the physics simulation world.
    /// </summary>
    [FriendlyName("Set Gravity")]
    [Description("Sets the gravity to the physics simulation world.")]
    [Category("Actions/Physics")]
    public class SetGravityAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Gravity in units of Newtons (N) to set to the world.
        /// </summary>
        [Description("Gravity in units of Newtons (N) to set to the world.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Gravity;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Container.Actor.Screen.World.Gravity = Gravity.Value;

            if (Out != null) Out();
        }
    }
}
