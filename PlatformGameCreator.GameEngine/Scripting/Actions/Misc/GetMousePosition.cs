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
    /// Gets the mouse position in scene units.
    /// </summary>
    [FriendlyName("Get Mouse Position")]
    [Description("Gets the mouse position in scene units.")]
    [Category("Actions/Misc")]
    public class GetMousePositionAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Outputs the mouse position.
        /// </summary>
        [FriendlyName("Position")]
        [Description("Outputs the mouse position.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Position;

        /// <summary>
        /// Outputs the x coordinate of the mouse position.
        /// </summary>
        [FriendlyName("Position X")]
        [Description("Outputs the x coordinate of the mouse position.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] PositionX;

        /// <summary>
        /// Outputs the y coordinate of the mouse position.
        /// </summary>
        [FriendlyName("Position Y")]
        [Description("Outputs the y coordinate of the mouse position.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] PositionY;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Vector2 mousePosition = Container.Actor.Screen.SceneMousePosition;

            SetOutputVariable(mousePosition, Position);
            SetOutputVariable(mousePosition.X, PositionX);
            SetOutputVariable(mousePosition.Y, PositionY);

            if (Out != null) Out();
        }
    }
}
