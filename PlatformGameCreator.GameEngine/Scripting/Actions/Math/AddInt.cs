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
    /// Adds int variables together and returns the result.
    /// </summary>
    [FriendlyName("Add Int")]
    [Description("Adds int variables together and returns the result.")]
    [Category("Actions/Math")]
    public class AddIntAction : ActionNode
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
        public Variable<int>[] A;

        /// <summary>
        /// The second values to add.
        /// </summary>
        [Description("The second values to add.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<int>[] B;

        /// <summary>
        /// Outputs the result of the addition.
        /// </summary>
        [Description("Outputs the result of the addition.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Result;

        /// <summary>
        /// Outputs the result of the addition cast to a float variable.
        /// </summary>
        [FriendlyName("Float Result")]
        [Description("Outputs the result of the addition cast to a float variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] FloatResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            int result = 0;

            for (int i = 0; i < A.Length; ++i)
            {
                result += A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                result += B[i].Value;
            }

            SetOutputVariable(result, Result);
            SetOutputVariable(result, FloatResult);

            if (Out != null) Out();
        }
    }
}
