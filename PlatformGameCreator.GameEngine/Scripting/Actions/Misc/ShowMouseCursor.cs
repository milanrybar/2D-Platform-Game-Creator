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
    /// Shows or hides the mouse cursor.
    /// </summary>
    [FriendlyName("Show Mouse Cursor")]
    [Description("Shows or hides the mouse cursor.")]
    [Category("Actions/Misc")]
    public class ShowMouseCursorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Indicates whether the mouse cursor will be shown.
        /// </summary>
        [Description("Indicates whether the mouse cursor will be shown.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(true)]
        public Variable<bool> Show;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Container.Actor.Screen.ScreenManager.Game.IsMouseVisible = Show.Value;

            if (Out != null) Out();
        }
    }
}
