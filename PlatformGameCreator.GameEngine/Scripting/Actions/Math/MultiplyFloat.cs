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
    /// Multiplies float variables together and returns the result.
    /// </summary>
    [FriendlyName("Multiply Float")]
    [Description("Multiplies float variables together and returns the result.")]
    [Category("Actions/Math")]
    public class MultiplyFloatAction : ActionNode
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
        [DefaultValue(1f)]
        public Variable<float>[] A;

        /// <summary>
        /// The second values to multiply.
        /// </summary>
        [Description("The second values to multiply.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float>[] B;

        /// <summary>
        /// Outputs the result of the multiplication.
        /// </summary>
        [Description("Outputs the result of the multiplication.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the result of the multiplication cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the result of the multiplication cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float result = 1f;

            for (int i = 0; i < A.Length; ++i)
            {
                result *= A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                result *= B[i].Value;
            }

            SetOutputVariable(result, Result);
            SetOutputVariable((int)result, IntResult);

            if (Out != null) Out();
        }
    }
}
