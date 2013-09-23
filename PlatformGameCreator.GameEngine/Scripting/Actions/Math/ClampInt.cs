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
    /// Clamps int variable and returns the result.
    /// </summary>
    [FriendlyName("Clamp Int")]
    [Description("Clamps int variable and returns the result.")]
    [Category("Actions/Math")]
    public class ClampIntAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Value to clamp.
        /// </summary>
        [Description("Value to clamp.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<int> Target;

        /// <summary>
        /// Minimum value of the range.
        /// </summary>
        [Description("Minimum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<int> Min;

        /// <summary>
        /// Maximum value of the range.
        /// </summary>
        [Description("Maximum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<int> Max;

        /// <summary>
        /// Outputs the result of the clamping.
        /// </summary>
        [Description("Outputs the result of the clamping.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<int>[] Result;

        /// <summary>
        /// Outputs the result of the clamping cast to a float variable.
        /// </summary>
        [FriendlyName("Float Result")]
        [Description("Outputs the result of the clamping cast to a float variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<float>[] FloatResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            int result = System.Math.Max(Min.Value, System.Math.Min(Target.Value, Max.Value));

            SetOutputVariable(result, Result);
            SetOutputVariable(result, FloatResult);

            if (Out != null) Out();
        }
    }
}
