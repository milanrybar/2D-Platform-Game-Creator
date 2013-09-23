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
    /// Multiplies vector variables together and returns the result.
    /// </summary>
    [FriendlyName("Multiply Vector")]
    [Description("Multiplies vector variables together and returns the result.")]
    [Category("Actions/Math")]
    public class MultiplyVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// The first values to multiply.
        /// </summary>
        [Description("The first values to multiply.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueVector2(1f, 1f)]
        public Variable<Vector2>[] A;

        /// <summary>
        /// The second values to multiply.
        /// </summary>
        [Description("The second values to multiply.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueVector2(1f, 1f)]
        public Variable<Vector2>[] B;

        /// <summary>
        /// Outputs the result of the multiplication.
        /// </summary>
        [Description("Outputs the result of the multiplication.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Vector2 result = Vector2.One;

            for (int i = 0; i < A.Length; ++i)
            {
                result *= A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                result *= B[i].Value;
            }

            SetOutputVariable(result, Result);

            if (Out != null) Out();
        }
    }
}
