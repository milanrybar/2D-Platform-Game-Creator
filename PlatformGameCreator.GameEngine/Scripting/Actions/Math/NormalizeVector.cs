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
    /// Normalizes vector variable and returns the result.
    /// </summary>
    [FriendlyName("Normalize Vector")]
    [Description("Normalizes vector variable and returns the result.")]
    [Category("Actions/Math")]
    public class NormalizeVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Vector to normalize.
        /// </summary>
        [Description("Vector to normalize.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Value;

        /// <summary>
        /// Outputs the normalized vector.
        /// </summary>
        [Description("Outputs the normalized vector.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Vector2 normalizedVector = Vector2.Normalize(Value.Value);

            SetOutputVariable(float.IsNaN(normalizedVector.X) || float.IsNaN(normalizedVector.Y) ? Vector2.Zero : normalizedVector, Result);

            if (Out != null) Out();
        }
    }
}
