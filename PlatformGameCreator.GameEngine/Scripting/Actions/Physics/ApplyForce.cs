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
    /// Applies a force to the dynamic body of the specified actors.
    /// </summary>
    [FriendlyName("Apply Force")]
    [Description("Applies a force to the dynamic body of the specified actors.")]
    [Category("Actions/Physics")]
    public class ApplyForceAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to apply a force to.
        /// </summary>
        [Description("Actors to apply a force to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Force in units of Newtons (N) to apply to the specified actors.
        /// </summary>
        [Description("Force in units of Newtons (N) to apply to the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Force;

        /// <summary>
        /// Indicates whether force is multiplied by the actor mass before applying to the specified actors.
        /// </summary>
        [FriendlyName("Multiply By Mass")]
        [Description("Indicates whether force is multiplied by the actor mass before applying to the specified actors.")]
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
                            Instance[i].Value.Body.ApplyForce(Instance[i].Value.Body.Mass * Force.Value);
                        }
                        else
                        {
                            Instance[i].Value.Body.ApplyForce(Force.Value);
                        }
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
