/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Screens;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Misc
{
    /// <summary>
    /// Changes scene by the specified scene.
    /// </summary>
    [FriendlyName("Change Scene")]
    [Description("Changes scene by the specified scene.")]
    [Category("Actions/Misc")]
    public class SceneChangeAction : ActionNode
    {
        /// <summary>
        /// Scene to change to.
        /// </summary>
        [Description("Scene to change to.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Screen> Scene;

        /// <summary>
        /// Changes scene by the specified scene.
        /// </summary>
        [Description("Changes scene by the specified scene.")]
        public void Change()
        {
            if (Scene != null && Scene.Value != null)
            {
                Container.Actor.Screen.ScreenManager.PopScreen();
                Container.Actor.Screen.ScreenManager.AddScreen(Scene.Value);
            }
        }
    }
}
