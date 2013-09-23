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
    /// Computes the minimum value among float variables and returns the result. 
    /// </summary>
    [FriendlyName("Min Float")]
    [Description("Computes the minimum value among float variables and returns the result.")]
    [Category("Actions/Math")]
    public class MinFloatAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Values to compute the minimum value from.
        /// </summary>
        [Description("Values to compute the minimum value from.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float>[] Values;

        /// <summary>
        /// Outputs the minimum value of the specified values.
        /// </summary>
        [Description("Outputs the minimum value of the specified values.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the minimum value of the specified values cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the minimum value of the specified values cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float min = float.MaxValue;

            for (int i = 0; i < Values.Length; ++i)
            {
                if (Values[i].Value < min) min = Values[i].Value;
            }

            SetOutputVariable(min, Result);
            SetOutputVariable((int)min, IntResult);

            if (Out != null) Out();
        }
    }
}
