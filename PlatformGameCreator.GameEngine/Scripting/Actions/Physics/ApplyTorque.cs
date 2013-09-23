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
    /// Applies a torque to the dynamic body of the specified actors.
    /// </summary>
    [FriendlyName("Apply Torque")]
    [Description("Applies a torque to the dynamic body of the specified actors.")]
    [Category("Actions/Physics")]
    public class ApplyTorqueAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to apply a torque to.
        /// </summary>
        [Description("Actors to apply a torque to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Torque in units of N-m to apply to the specified actors.
        /// </summary>
        [Description("Torque in units of N-m to apply to the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Torque;

        /// <summary>
        /// Indicates whether torque is multiplied by the actor inertia before applying to the specified actors.
        /// </summary>
        [FriendlyName("Multiply By Inertia")]
        [Description("Indicates whether torque is multiplied by the actor inertia before applying to the specified actors.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(true)]
        public Variable<bool> MultiplyByInertia;

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
                        if (MultiplyByInertia.Value)
                        {
                            Instance[i].Value.Body.ApplyTorque(Instance[i].Value.Body.Inertia * Torque.Value);
                        }
                        else
                        {
                            Instance[i].Value.Body.ApplyTorque(Torque.Value);
                        }
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
