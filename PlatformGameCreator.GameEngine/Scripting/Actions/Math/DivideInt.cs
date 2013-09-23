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
    /// Divides int variables together and returns the result.
    /// </summary>
    [FriendlyName("Divide Int")]
    [Description("Divides int variables together and returns the result.")]
    [Category("Actions/Math")]
    public class DivideIntAction : ActionNode
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
        [DefaultValue(1)]
        public Variable<int>[] A;

        /// <summary>
        /// Multiplication of given values is divisor. If divisor is value of 0 then the result of dividing is value of 0. 
        /// </summary>
        [Description("Multiplication of given values is divisor.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1)]
        public Variable<int>[] B;

        /// <summary>
        /// Outputs the result of the dividing.
        /// </summary>
        [Description("Outputs the result of the dividing.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Result;

        /// <summary>
        /// Outputs the result of the dividing cast to a float variable.
        /// </summary>
        [FriendlyName("Float Result")]
        [Description("Outputs the result of the dividing cast to a float variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] FloatResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            int a = 1, b = 1;

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
                SetOutputVariable(0, Result);
                SetOutputVariable(0f, FloatResult);
            }
            else
            {
                SetOutputVariable(a / b, Result);
                SetOutputVariable((float)a / (float)b, FloatResult);
            }

            if (Out != null) Out();
        }
    }
}
