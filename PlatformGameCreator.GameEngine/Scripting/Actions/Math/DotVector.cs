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
    /// Calculates the dot product of two vector variables and returns the result.
    /// </summary>
    [FriendlyName("Dot Vector")]
    [Description("Calculates the dot product of two vector variables and returns the result.")]
    [Category("Actions/Math")]
    public class DotVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// The first vector variable for dot product.
        /// </summary>
        [Description("The first vector variable for dot product.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> A;

        /// <summary>
        /// The second vector variable for dot product.
        /// </summary>
        [Description("The second vector variable for dot product.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> B;

        /// <summary>
        /// Outputs the result of the dot product.
        /// </summary>
        [Description("Outputs the result of the dot product.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Vector2.Dot(A.Value, B.Value), Result);

            if (Out != null) Out();
        }
    }
}
