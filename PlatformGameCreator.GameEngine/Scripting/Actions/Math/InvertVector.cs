/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Math
{
    /// <summary>
    /// Inverts the vector variable and returns the result.
    /// </summary>
    [FriendlyName("Invert Vector")]
    [Description("Inverts the vector variable and returns the result.")]
    [Category("Actions/Math")]
    public class InvertVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Vector variable to invert.
        /// </summary>
        [Description("Vector variable to invert.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Value;

        /// <summary>
        /// Outputs the result of the inverting.
        /// </summary>
        [Description("Outputs the result of the inverting.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(-Value.Value, Result);

            if (Out != null) Out();
        }
    }
}
