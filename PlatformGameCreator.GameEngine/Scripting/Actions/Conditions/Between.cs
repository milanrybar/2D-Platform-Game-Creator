/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Conditions
{
    /// <summary>
    /// Checks if the specified value is in the specified range.
    /// </summary>
    /// <typeparam name="T">Type of value to check.</typeparam>
    [Description("Checks if the specified value is in the specified range.")]
    [Category("Actions/Conditions")]
    public abstract class BetweenAction<T> : ActionNode
    {
        /// <summary>
        /// Fires when the specified value is in the specified range.
        /// </summary>
        [Description("Fires when the specified value is in the specified range.")]
        public ScriptSocketHandler True;

        /// <summary>
        /// Fires when the specified value is not in the specified range.
        /// </summary>
        [Description("Fires when the specified value is not in the specified range.")]
        public ScriptSocketHandler False;

        /// <summary>
        /// Value to check if it is in the specified range.
        /// </summary>
        [Description("Value to check if it is in the specified range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> Value;

        /// <summary>
        /// Minimum value of the range.
        /// </summary>
        [Description("Minimum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> Min;

        /// <summary>
        /// Maximum value of the range.
        /// </summary>
        [Description("Maximum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> Max;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (ComputeResult())
            {
                if (True != null) True();
            }
            else if (False != null) False();
        }

        /// <summary>
        /// Compute result of the action.
        /// </summary>
        /// <returns>Indicates whether the specified value is in the specified range.</returns>
        protected abstract bool ComputeResult();
    }

    /// <inheritdoc />
    [FriendlyName("Between Ints")]
    public class BetweenIntsAction : BetweenAction<int>
    {
        /// <inheritdoc />
        protected override bool ComputeResult()
        {
            if (Min.Value > Max.Value || Min.Value == Max.Value) return false;
            else if (Value.Value >= Min.Value && Value.Value <= Max.Value) return true;
            else return false;
        }
    }

    /// <inheritdoc />
    [FriendlyName("Between Floats")]
    public class BetweenFloatsAction : BetweenAction<float>
    {
        /// <inheritdoc />
        protected override bool ComputeResult()
        {
            if (Min.Value > Max.Value || Min.Value == Max.Value) return false;
            else if (Value.Value >= Min.Value && Value.Value <= Max.Value) return true;
            else return false;
        }
    }
}
