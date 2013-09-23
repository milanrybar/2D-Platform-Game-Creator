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
    /// Inverts the float variable and returns the result.
    /// </summary>
    [FriendlyName("Invert Float")]
    [Description("Inverts the float variable and returns the result.")]
    [Category("Actions/Math")]
    public class InvertFloatAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Float variable to invert.
        /// </summary>
        [Description("Float variable to invert.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Value;

        /// <summary>
        /// Outputs the result of the inverting.
        /// </summary>
        [Description("Outputs the result of the inverting.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the result of the inverting cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the result of the inverting cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(-Value.Value, Result);
            SetOutputVariable(-(int)Value.Value, IntResult);

            if (Out != null) Out();
        }
    }
}
