/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Misc
{
    /// <summary>
    /// Gets the size in pixels of the screen (game window).
    /// </summary>
    [FriendlyName("Get Screen Size")]
    [Description("Gets the size in pixels of the screen (game window).")]
    [Category("Actions/Misc")]
    public class GetScreenSizeAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Outputs the size in pixel of the screen as vector variable.
        /// </summary>
        [FriendlyName("Size")]
        [Description("Outputs the size in pixel of the screen as vector variable.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Size;

        /// <summary>
        /// Outputs the width in pixel of the screen.
        /// </summary>
        [FriendlyName("Width")]
        [Description("Outputs the width in pixel of the screen.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Width;

        /// <summary>
        /// Outputs the height in pixel of the screen.
        /// </summary>
        [FriendlyName("Height")]
        [Description("Outputs the height in pixel of the screen.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Height;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            PhysicsGame game = Container.Actor.Screen.ScreenManager.Game as PhysicsGame;
            if (game != null)
            {
                int width = game.WindowWidth;
                int height = game.WindowHeight;

                SetOutputVariable(width, Width);
                SetOutputVariable(height, Height);
                SetOutputVariable(new Vector2(width, height), Size);
            }

            if (Out != null) Out();
        }
    }
}
