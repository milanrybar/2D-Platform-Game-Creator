/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Changes state by the specified transition in the state machines of the specified actors.
    /// </summary>
    [FriendlyName("Change State")]
    [Description("Changes state by the specified transition in the state machines of the specified actors.")]
    [Category("Actions/Actors")]
    public class ChangeStateAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors that will change state by the specified transition.
        /// </summary>
        [Description("Actors that will change state by the specified transition.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Name of the transition (event in) to change state of the specified actors.
        /// </summary>
        [Description("Name of the transition (event in) to change state of the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string> Transition;

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
                        Instance[i].Value.ChangeState(Transition.Value);
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
