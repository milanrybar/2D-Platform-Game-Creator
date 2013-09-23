/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Variables
{
    /// <summary>
    /// Converts specified variables to the string variable.
    /// </summary>
    [FriendlyName("Convert To String")]
    [Description("Converts specified variables to the string variable.")]
    [Category("Actions/Variables")]
    public class ConvertToString : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Int variable to convert to the string variable.
        /// </summary>
        [Description("Int variable to convert to the string variable.")]
        [VariableSocket(VariableSocketType.In, CanBeEmpty = true)]
        public Variable<int> Int;

        /// <summary>
        /// Float variable to convert to the string variable.
        /// </summary>
        [Description("Float variable to convert to the string variable.")]
        [VariableSocket(VariableSocketType.In, CanBeEmpty = true)]
        public Variable<float> Float;

        /// <summary>
        /// Bool variable to convert to the string variable.
        /// </summary>
        [Description("Bool variable to convert to the string variable.")]
        [VariableSocket(VariableSocketType.In, CanBeEmpty = true)]
        public Variable<bool> Bool;

        /// <summary>
        /// Outputs the converted string variable.
        /// </summary>
        [Description("Outputs the converted string variable.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<string>[] String;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Int != null) SetOutputVariable(Int.Value.ToString(), String);
            if (Float != null) SetOutputVariable(Float.Value.ToString(), String);
            if (Bool != null) SetOutputVariable(Bool.Value.ToString(), String);

            if (Out != null) Out();
        }
    }
}
