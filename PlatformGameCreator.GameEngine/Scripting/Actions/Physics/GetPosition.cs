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
    /// Gets the position and the rotation of the specified actor.
    /// </summary>
    [FriendlyName("Get Position and Rotation")]
    [Description("Gets the position and the rotation of the specified actor.")]
    [Category("Actions/Physics")]
    public class GetPositionAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the position and the rotation from.
        /// </summary>
        [Description("Actor to get the position and the rotation from.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Target;

        /// <summary>
        /// Outputs the positions in meters of the specified actor.
        /// </summary>
        [Description("Outputs the positions in meters of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Position;

        /// <summary>
        /// Outputs the rotation angle in radians of the specified actor.
        /// </summary>
        [Description("Outputs the rotation angle in radians of the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Rotation;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Target != null && Target.Value != null)
            {
                SetOutputVariable(Target.Value.Position, Position);

                SetOutputVariable(Target.Value.Angle, Rotation);
            }

            if (Out != null) Out();
        }
    }
}
