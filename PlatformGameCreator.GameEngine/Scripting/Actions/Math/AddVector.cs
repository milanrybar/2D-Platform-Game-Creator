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
    /// Adds vector variables together and returns the result.
    /// </summary>
    [FriendlyName("Add Vector")]
    [Description("Adds vector variables together and returns the result.")]
    [Category("Actions/Math")]
    public class AddVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// The first values to add.
        /// </summary>
        [Description("The first values to add.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2>[] A;

        /// <summary>
        /// The second values to add.
        /// </summary>
        [Description("The second values to add.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2>[] B;

        /// <summary>
        /// Outputs the result of the addition.
        /// </summary>
        [Description("Outputs the result of the addition.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            Vector2 result = Vector2.Zero;

            for (int i = 0; i < A.Length; ++i)
            {
                result += A[i].Value;
            }

            for (int i = 0; i < B.Length; ++i)
            {
                result += B[i].Value;
            }

            SetOutputVariable(result, Result);

            if (Out != null) Out();
        }
    }
}
