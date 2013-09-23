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
    /// Computes the maximum value among int variables and returns the result. 
    /// </summary>
    [FriendlyName("Max Int")]
    [Description("Computes the maximum value among int variables and returns the result.")]
    [Category("Actions/Math")]
    public class MaxIntAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Values to compute the maximum value from.
        /// </summary>
        [Description("Values to compute the maximum value from.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<int>[] Values;

        /// <summary>
        /// Outputs the maximum value of the specified values.
        /// </summary>
        [Description("Outputs the maximum value of the specified values.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Result;

        /// <summary>
        /// Outputs the maximum value of the specified values cast to a float variable.
        /// </summary>
        [FriendlyName("Float Result")]
        [Description("Outputs the maximum value of the specified values cast to a float variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] FloatResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            int max = Int32.MinValue;

            for (int i = 0; i < Values.Length; ++i)
            {
                if (Values[i].Value > max) max = Values[i].Value;
            }

            SetOutputVariable(max, Result);
            SetOutputVariable(max, FloatResult);

            if (Out != null) Out();
        }
    }
}
