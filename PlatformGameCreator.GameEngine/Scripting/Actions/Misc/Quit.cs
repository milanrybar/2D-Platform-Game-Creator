/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Misc
{
    /// <summary>
    /// Quits the game application.
    /// </summary>
    [FriendlyName("Quit")]
    [Description("Quits the game application.")]
    [Category("Actions/Misc")]
    public class QuitAction : ActionNode
    {
        /// <summary>
        /// Quits the game.
        /// </summary>
        [Description("Quits the game.")]
        public void Quit()
        {
            Container.Actor.Screen.Exit();
        }
    }
}
