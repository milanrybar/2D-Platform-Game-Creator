﻿/*
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
    /// Sets the linear velocity to the body of the specified actors.
    /// </summary>
    [FriendlyName("Set Linear Velocity")]
    [Description("Sets the linear velocity to the body of the specified actors.")]
    [Category("Actions/Physics")]
    public class SetLinearVelocityAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to set the linear velocity to.
        /// </summary>
        [Description("Actors to set the linear velocity to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Linear velocity in Newtons (N) to set to the body of the specified actors.
        /// </summary>
        [Description("Linear velocity in Newtons (N) to set to the body of the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Velocity;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Instance != null)
            {
                for (int i = 0; i < Instance.Length; ++i)
                {
                    if (Instance[i].Value != null && Instance[i].Value.Body != null)
                    {
                        Instance[i].Value.Body.LinearVelocity = Velocity.Value;
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
