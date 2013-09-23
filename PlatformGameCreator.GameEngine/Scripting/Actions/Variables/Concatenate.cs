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
    /// Concatenates string variables together and return the result.
    /// </summary>
    [FriendlyName("Concatenate")]
    [Description("Concatenates string variables together and return the result.")]
    [Category("Actions/Variables")]
    public class Concatenate : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// First string variables to concatenate.
        /// </summary>
        [Description("First string variables to concatenate.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string>[] A;

        /// <summary>
        /// Second string variables to concatenate.
        /// </summary>
        [Description("Second string variables to concatenate.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string>[] B;

        /// <summary>
        /// Separator used for separating one string variable from the next.
        /// </summary>
        [Description("Separator used for separating one string variable from the next.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string> Separator;

        /// <summary>
        /// Outputs the result of concatenation.
        /// </summary>
        [Description("Outputs the result of concatenation.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<string>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            StringBuilder result = new StringBuilder();

            if (A != null)
            {
                for (int i = 0; i < A.Length; ++i)
                {
                    if (result.Length != 0) result.Append(Separator.Value);
                    result.Append(A[i].Value);
                }
            }

            if (B != null)
            {
                for (int i = 0; i < B.Length; ++i)
                {
                    if (result.Length != 0) result.Append(Separator.Value);
                    result.Append(B[i].Value);
                }
            }

            SetOutputVariable(result.ToString(), Result);

            if (Out != null) Out();
        }
    }
}
