/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Math
{
    /// <summary>
    /// Subtracts one float variable from another and returns the result.
    /// </summary>
    [FriendlyName("Subtract Float")]
    [Description("Subtracts one float variable from another and returns the result.")]
    [Category("Actions/Math")]
    public class SubtractFloatAction : ActionNode
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
        public Variable<float>[] A;

        /// <summary>
        /// Sum of given values is subtrahend.
        /// </summary>
        [Description("Sum of given values is subtrahend.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float>[] B;

        /// <summary>
        /// Outputs the result of the subtraction.
        /// </summary>
        [Description("Outputs the result of the subtraction.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the result of the subtraction cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the result of the subtraction cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float result, a = 0, b = 0;

            for (int i = 0; i < A.Length; ++i)
            {
                a += A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                b += B[i].Value;
            }

            result = a - b;

            SetOutputVariable(result, Result);
            SetOutputVariable((int)result, IntResult);

            if (Out != null) Out();
        }
    }
}
