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
    /// Inverts the bool variable and returns the result.
    /// </summary>
    [FriendlyName("Invert Bool")]
    [Description("Inverts the bool variable and returns the result.")]
    [Category("Actions/Math")]
    public class InvertBoolAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Bool variable to invert.
        /// </summary>
        [Description("Bool variable to invert.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<bool> Value;

        /// <summary>
        /// Outputs the result of the inverting.
        /// </summary>
        [Description("Outputs the result of the inverting.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<bool>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(!Value.Value, Result);

            if (Out != null) Out();
        }
    }
}
