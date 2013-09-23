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
    /// Computes trigonometry function and returns the result.
    /// </summary>
    [FriendlyName("Trigonometry")]
    [Description("Computes trigonometry function and returns the result.")]
    [Category("Actions/Math")]
    public class TrigonometryAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Value for the trigonometry function.
        /// </summary>
        [Description("Value for the trigonometry function.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Value;

        /// <summary>
        /// Outputs the result of the trigonometry function.
        /// </summary>
        [Description("Outputs the result of the trigonometry function.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Result;

        /// <summary>
        /// Computes Acos function by specified value and returns the result.
        /// </summary>
        [Description("Computes Acos function by specified value and returns the result.")]
        public void Acos()
        {
            SetOutputVariable((float)System.Math.Acos(Value.Value), Result);

            if (Out != null) Out();
        }

        /// <summary>
        /// Computes Asin function by specified value and returns the result.
        /// </summary>
        [Description("Computes Asin function by specified value and returns the result.")]
        public void Asin()
        {
            SetOutputVariable((float)System.Math.Asin(Value.Value), Result);

            if (Out != null) Out();
        }

        /// <summary>
        /// Computes Atan function by specified value and returns the result.
        /// </summary>
        [Description("Computes Atan function by specified value and returns the result.")]
        public void Atan()
        {
            SetOutputVariable((float)System.Math.Atan(Value.Value), Result);

            if (Out != null) Out();
        }

        /// <summary>
        /// Computes Cos function by specified value and returns the result.
        /// </summary>
        [Description("Computes Cos function by specified value and returns the result.")]
        public void Cos()
        {
            SetOutputVariable((float)System.Math.Cos(Value.Value), Result);

            if (Out != null) Out();
        }

        /// <summary>
        /// Computes Sin function by specified value and returns the result.
        /// </summary>
        [Description("Computes Sin function by specified value and returns the result.")]
        public void Sin()
        {
            SetOutputVariable((float)System.Math.Sin(Value.Value), Result);

            if (Out != null) Out();
        }

        /// <summary>
        /// Computes Tan function by specified value and returns the result.
        /// </summary>
        [Description("Computes Tan function by specified value and returns the result.")]
        public void Tan()
        {
            SetOutputVariable((float)System.Math.Tan(Value.Value), Result);

            if (Out != null) Out();
        }
    }
}
