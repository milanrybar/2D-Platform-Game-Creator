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
    /// Applies a linear impulse to the dynamic body of the specified actors.
    /// </summary>
    [FriendlyName("Apply Linear Impulse")]
    [Description("Applies a linear impulse to the dynamic body of the specified actors.")]
    [Category("Actions/Physics")]
    public class ApplyLinearImpulseAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to apply a linear impulse to.
        /// </summary>
        [Description("Actors to apply a linear impulse to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Linear impulse usually in N-seconds or kg-m/s to apply to the specified actors.
        /// </summary>
        [Description("Linear impulse usually in N-seconds or kg-m/s to apply to the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Impulse;

        /// <summary>
        /// Indicates whether linear impulse is multiplied by the actor mass before applying to the specified actors.
        /// </summary>
        [FriendlyName("Multiply By Mass")]
        [Description("Indicates whether linear impulse is multiplied by the actor mass before applying to the specified actors.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(true)]
        public Variable<bool> MultiplyByMass;

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
                        if (MultiplyByMass.Value)
                        {
                            Instance[i].Value.Body.ApplyLinearImpulse(Instance[i].Value.Body.Mass * Impulse.Value);
                        }
                        else
                        {
                            Instance[i].Value.Body.ApplyLinearImpulse(Impulse.Value);
                        }
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
