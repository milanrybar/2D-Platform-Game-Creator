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
    /// Sets the position to the specified actors.
    /// </summary>
    [FriendlyName("Set Position")]
    [Description("Sets the position to the specified actors.")]
    [Category("Actions/Physics")]
    public class SetPositionAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to set the position to.
        /// </summary>
        [Description("Actors to set the position to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Target;

        /// <summary>
        /// Positions in meters to set to the specified actors.
        /// </summary>
        [Description("Positions in meters to set to the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Position;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Target != null)
            {
                for (int i = 0; i < Target.Length; ++i)
                {
                    if (Target[i].Value != null)
                    {
                        Target[i].Value.Position = Position.Value;
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
