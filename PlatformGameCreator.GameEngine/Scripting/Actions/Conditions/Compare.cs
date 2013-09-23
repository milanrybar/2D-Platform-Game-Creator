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
    /// Compares two specified values.
    /// </summary>
    /// <typeparam name="T">Type of values to compare.</typeparam>
    [Description("Compares two specified values.")]
    [Category("Actions/Conditions")]
    public abstract class BaseCompareAction<T> : ActionNode
    {
        /// <summary>
        /// Fires if the first value is greater than the second value.
        /// </summary>
        [FriendlyName("A > B")]
        [Description("Fires if the first value is greater than the second value.")]
        public ScriptSocketHandler Greater;

        /// <summary>
        /// Fires if the first value is greater than or equal to the second value.
        /// </summary>
        [FriendlyName("A >= B")]
        [Description("Fires if the first value is greater than or equal to the second value.")]
        public ScriptSocketHandler GreaterOrEqual;

        /// <summary>
        /// Fires if the first value is equal to the second value.
        /// </summary>
        [FriendlyName("A = B")]
        [Description("Fires if the first value is equal to the second value.")]
        public ScriptSocketHandler Equal;

        /// <summary>
        /// Fires if the first value is not equal to the second value.
        /// </summary>
        [FriendlyName("A != B")]
        [Description("Fires if the first value is not equal to the second value.")]
        public ScriptSocketHandler NotEqual;

        /// <summary>
        /// Fires if the first value is less than or equal to the second value.
        /// </summary>
        [FriendlyName("A <= B")]
        [Description("Fires if the first value is less than or equal to the second value.")]
        public ScriptSocketHandler LessOrEqual;

        /// <summary>
        /// Fires if the first value is less than the second value.
        /// </summary>
        [FriendlyName("A < B")]
        [Description("Fires if the first value is less than the second value.")]
        public ScriptSocketHandler Less;

        /// <summary>
        /// First value to compare.
        /// </summary>
        [Description("First value to compare.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> A;

        /// <summary>
        /// Second value to compare.
        /// </summary>
        [Description("Second value to compare.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> B;
    }

    /// <inheritdoc />
    [FriendlyName("Compare Int")]
    public class CompareIntAction : BaseCompareAction<int>
    {
        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            int a = A.Value, b = B.Value;

            if (a > b && Greater != null) Greater();
            if (a >= b && GreaterOrEqual != null) GreaterOrEqual();
            if (a == b && Equal != null) Equal();
            if (a != b && NotEqual != null) NotEqual();
            if (a <= b && LessOrEqual != null) LessOrEqual();
            if (a < b && Less != null) Less();
        }
    }

    /// <inheritdoc />
    [FriendlyName("Compare Float")]
    public class CompareFloatAction : BaseCompareAction<float>
    {
        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float a = A.Value, b = B.Value;

            if (a > b && Greater != null) Greater();
            if (a >= b && GreaterOrEqual != null) GreaterOrEqual();
            if (a == b && Equal != null) Equal();
            if (a != b && NotEqual != null) NotEqual();
            if (a <= b && LessOrEqual != null) LessOrEqual();
            if (a < b && Less != null) Less();
        }
    }

    /// <summary>
    /// Increments the first value and then compares two specified values.
    /// </summary>
    [FriendlyName("Int Counter")]
    [Description("Increments the first value and then compares two specified values.")]
    public class CounterIntAction : BaseCompareAction<int>
    {
        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            int a = ++A.Value, b = B.Value;

            if (a > b && Greater != null) Greater();
            if (a >= b && GreaterOrEqual != null) GreaterOrEqual();
            if (a == b && Equal != null) Equal();
            if (a != b && NotEqual != null) NotEqual();
            if (a <= b && LessOrEqual != null) LessOrEqual();
            if (a < b && Less != null) Less();
        }
    }

    /// <summary>
    /// Increments the first value and then compares two specified values.
    /// </summary>
    [FriendlyName("Float Counter")]
    [Description("Increments the first value and then compares two specified values.")]
    public class CounterFloatAction : BaseCompareAction<float>
    {
        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            float a = ++A.Value, b = B.Value;

            if (a > b && Greater != null) Greater();
            if (a >= b && GreaterOrEqual != null) GreaterOrEqual();
            if (a == b && Equal != null) Equal();
            if (a != b && NotEqual != null) NotEqual();
            if (a <= b && LessOrEqual != null) LessOrEqual();
            if (a < b && Less != null) Less();
        }
    }
}
