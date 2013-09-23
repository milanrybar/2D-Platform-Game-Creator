/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Variables
{
    /// <summary>
    /// Sets the value of a vector variable from two variables, X and Y.
    /// </summary>
    [FriendlyName("Set Vector Components")]
    [Description("Sets the value of a vector variable from two variables, X and Y.")]
    [Category("Actions/Variables")]
    public class SetVectorComponentsAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Value for the X component of the vector.
        /// </summary>
        [Description("Value for the X component of the vector.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> X;

        /// <summary>
        /// Value for the Y component of the vector.
        /// </summary>
        [Description("Value for the Y component of the vector.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Y;

        /// <summary>
        /// Outputs the resulting vector from the combined components.
        /// </summary>
        [Description("Outputs the resulting vector from the combined components.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Output;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(new Vector2(X.Value, Y.Value), Output);

            if (Out != null) Out();
        }
    }
}
