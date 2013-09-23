/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Math
{
    /// <summary>
    /// Clamps float variable and returns the result.
    /// </summary>
    [FriendlyName("Clamp Float")]
    [Description("Clamps float variable and returns the result.")]
    [Category("Actions/Math")]
    public class ClampFloatAction : ActionNode
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
        public Variable<float> Target;

        /// <summary>
        /// Minimum value of the range.
        /// </summary>
        [Description("Minimum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Min;

        /// <summary>
        /// Maximum value of the range.
        /// </summary>
        [Description("Maximum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Max;

        /// <summary>
        /// Outputs the result of the clamping.
        /// </summary>
        [Description("Outputs the result of the clamping.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Outputs the result of the clamping cast to an int variable.
        /// </summary>
        [FriendlyName("Int Result")]
        [Description("Outputs the result of the clamping cast to an int variable.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<int>[] IntResult;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float result = MathHelper.Clamp(Target.Value, Min.Value, Max.Value);

            SetOutputVariable(result, Result);
            SetOutputVariable((int)result, IntResult);

            if (Out != null) Out();
        }
    }
}
