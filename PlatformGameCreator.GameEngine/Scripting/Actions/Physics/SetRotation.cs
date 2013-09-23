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
    /// Sets the rotation to the specified actors.
    /// </summary>
    [FriendlyName("Set Rotation")]
    [Description("Sets the rotation to the specified actors.")]
    [Category("Actions/Physics")]
    public class SetRotationAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to set the rotation to.
        /// </summary>
        [Description("Actors to set the rotation to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Target;

        /// <summary>
        /// Rotation angle in radians to set to the specified actors.
        /// </summary>
        [Description("Rotation angle in radians to to the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Rotation;

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
                        Target[i].Value.Angle = Rotation.Value;
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
