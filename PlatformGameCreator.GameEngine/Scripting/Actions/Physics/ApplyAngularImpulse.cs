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
    /// Applies an angular impulse to the dynamic body of the specified actors.
    /// </summary>
    [FriendlyName("Apply Angular Impulse")]
    [Description("Applies an angular impulse to the dynamic body of the specified actors.")]
    [Category("Actions/Physics")]
    public class ApplyAngularImpulseAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to apply an angular impulse to.
        /// </summary>
        [Description("Actors to apply an angular impulse to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Angular impulse in units of kg*m*m/s to apply to the specified actors.
        /// </summary>
        [Description("Angular impulse in units of kg*m*m/s to apply to the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Impulse;

        /// <summary>
        /// Indicates whether angular impuse is multiplied by the actor inertia before applying to the specified actors.
        /// </summary>
        [FriendlyName("Multiply By Inertia")]
        [Description("Indicates whether angular impuse is multiplied by the actor inertia before applying to the specified actors.")]
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
                            Instance[i].Value.Body.ApplyAngularImpulse(Instance[i].Value.Body.Inertia * Impulse.Value);
                        }
                        else
                        {
                            Instance[i].Value.Body.ApplyAngularImpulse(Impulse.Value);
                        }
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
