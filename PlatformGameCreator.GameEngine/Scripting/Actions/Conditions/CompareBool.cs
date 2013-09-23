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
    /// Checks the value of the bool variable.
    /// </summary>
    [FriendlyName("Compare Bool")]
    [Description("Checks the value of the bool variable.")]
    [Category("Actions/Conditions")]
    public class CompareBoolAction : ActionNode
    {
        /// <summary>
        /// Fires if the specified value is true.
        /// </summary>
        [Description("Fires if the specified value is true.")]
        public ScriptSocketHandler True;

        /// <summary>
        /// Fires if the specified value is false.
        /// </summary>
        [Description("Fires if the specified value is false.")]
        public ScriptSocketHandler False;

        /// <summary>
        /// Value of the bool variable to check.
        /// </summary>
        [Description("Value of the bool variable to check.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<bool> Bool;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            bool value = Bool.Value;

            if (value && True != null) True();
            else if (!value && False != null) False();
        }
    }
}
