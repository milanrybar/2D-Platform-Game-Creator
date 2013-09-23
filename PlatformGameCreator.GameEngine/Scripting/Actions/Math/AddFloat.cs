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
    /// Adds float variables together and returns the result.
    /// </summary>
    [FriendlyName("Add Float")]
    [Description("Adds float variables together and returns the result.")]
    [Category("Actions/Math")]
    public class AddFloatAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// The first values to add.
        /// </summary>
        [Description("The first values to add.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float>[] A;

        /// <summary>
        /// The second values to add.
        /// </summary>
        [Description("The second values to add.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float>[] B;

        /// <summary>
        /// Outputs the result of the addition.
        /// </summary>
        [Description("Outputs the result of the addition.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the result of the addition cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the result of the addition cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float result = 0;

            for (int i = 0; i < A.Length; ++i)
            {
                result += A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                result += B[i].Value;
            }

            SetOutputVariable(result, Result);
            SetOutputVariable((int)result, IntResult);

            if (Out != null) Out();
        }
    }
}
