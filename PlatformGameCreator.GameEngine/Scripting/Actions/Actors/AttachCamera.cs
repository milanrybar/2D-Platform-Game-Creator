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
    /// Attachs camera to the specified actor. The position of the specified actor will be in the center of the game window.
    /// </summary>
    [FriendlyName("Attach Camera")]
    [Description("Attachs camera to the specified actor. The position of the specified actor will be in the center of the game window.")]
    [Category("Actions/Actors")]
    public class AttachCameraAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to attach camera to.
        /// </summary>
        [Description("Actor to attach camera to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Instance;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Instance != null && Instance.Value != null)
            {
                Instance.Value.Screen.Camera.Actor = Instance.Value;
            }

            if (Out != null) Out();
        }
    }
}
