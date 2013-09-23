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
    /// Subtracts one vector variable from another and returns the result.
    /// </summary>
    [FriendlyName("Subtract Vector")]
    [Description("Subtracts one vector variable from another and returns the result.")]
    [Category("Actions/Math")]
    public class SubtractVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Sum of given values is minuend.
        /// </summary>
        [Description("Sum of given values is minuend.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2>[] A;

        /// <summary>
        /// Sum of given values is subtrahend.
        /// </summary>
        [Description("Sum of given values is subtrahend.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2>[] B;

        /// <summary>
        /// Outputs the result of the subtraction.
        /// </summary>
        [Description("Outputs the result of the subtraction.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Vector2 a = Vector2.Zero, b = Vector2.Zero;

            for (int i = 0; i < A.Length; ++i)
            {
                a += A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                b += B[i].Value;
            }

            SetOutputVariable(a - b, Result);

            if (Out != null) Out();
        }
    }
}
