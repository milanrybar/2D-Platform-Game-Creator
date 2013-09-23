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
    /// Clamps vector variable and returns the result.
    /// </summary>
    [FriendlyName("Clamp Vector")]
    [Description("Clamps vector variable and returns the result.")]
    [Category("Actions/Math")]
    public class ClampVectorAction : ActionNode
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
        public Variable<Vector2> Target;

        /// <summary>
        /// Minimum value of the range.
        /// </summary>
        [Description("Minimum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Min;

        /// <summary>
        /// Maximum value of the range.
        /// </summary>
        [Description("Maximum value of the range.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Max;

        /// <summary>
        /// Outputs the result of the clamping.
        /// </summary>
        [Description("Outputs the result of the clamping.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Vector2 result = Vector2.Max(Min.Value, Vector2.Min(Target.Value, Max.Value));

            SetOutputVariable(result, Result);

            if (Out != null) Out();
        }
    }
}
