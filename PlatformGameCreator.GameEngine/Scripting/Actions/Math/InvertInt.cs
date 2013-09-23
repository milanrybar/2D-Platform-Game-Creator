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
    /// Inverts the int variable and returns the result.
    /// </summary>
    [FriendlyName("Invert Int")]
    [Description("Inverts the int variable and returns the result.")]
    [Category("Actions/Math")]
    public class InvertIntAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Int variable to invert.
        /// </summary>
        [Description("Int variable to invert.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<int> Value;

        /// <summary>
        /// Outputs the result of the inverting.
        /// </summary>
        [Description("Outputs the result of the inverting.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Result;

        /// <summary>
        /// Outputs the result of the inverting cast to a float variable.
        /// </summary>
        [FriendlyName("Float Result")]
        [Description("Outputs the result of the inverting cast to a float variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] FloatResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(-Value.Value, Result);
            SetOutputVariable(-Value.Value, FloatResult);

            if (Out != null) Out();
        }
    }
}
