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

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Destroys the specified actor.
    /// </summary>
    [FriendlyName("Destroy")]
    [Description("Destroys the specified actor.")]
    [Category("Actions/Actors")]
    public class DestroyAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to destroy.
        /// </summary>
        [Description("Actors to destroy.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

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
                    if (Instance[i].Value != null)
                    {
                        Instance[i].Value.Destroy();
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
