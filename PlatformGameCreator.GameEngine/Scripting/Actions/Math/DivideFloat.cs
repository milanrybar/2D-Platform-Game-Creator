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
    /// Divides float variables and returns the result.
    /// </summary>
    [FriendlyName("Divide Float")]
    [Description("Divides float variables and returns the result.")]
    [Category("Actions/Math")]
    public class DivideFloatAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Multiplication of given values is dividend.
        /// </summary>
        [Description("Multiplication of given values is dividend.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float>[] A;

        /// <summary>
        /// Multiplication of given values is divisor. If divisor is value of 0 then the result of dividing is value of 0. 
        /// </summary>
        [Description("Multiplication of given values is divisor.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float>[] B;

        /// <summary>
        /// Outputs the result of the dividing.
        /// </summary>
        [Description("Outputs the result of the dividing.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the result of the dividing cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the result of the dividing cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float a = 1f, b = 1f;

            for (int i = 0; i < A.Length; ++i)
            {
                a *= A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                b *= B[i].Value;
            }

            if (b == 0)
            {
                SetOutputVariable(0f, Result);
                SetOutputVariable(0, IntResult);
            }
            else
            {
                float result = a / b;
                SetOutputVariable(result, Result);
                SetOutputVariable((int)result, IntResult);
            }

            if (Out != null) Out();
        }
    }
}
