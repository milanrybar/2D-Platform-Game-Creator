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
    /// Gets the individual X and Y components of the specified vector variable.
    /// </summary>
    [FriendlyName("Get Vector Components")]
    [Description("Gets the individual X and Y components of the specified vector variable.")]
    [Category("Actions/Variables")]
    public class GetVectorComponentsAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Vector value from which to obtain the components.
        /// </summary>
        [Description("Vector value from which to obtain the components.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Value;

        /// <summary>
        /// Outputs the X component of the specified vector.
        /// </summary>
        [Description("Outputs the X component of the specified vector.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] X;

        /// <summary>
        /// Outputs the Y component of the specified vector.
        /// </summary>
        [Description("Outputs the Y component of the specified vector.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Y;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Value.Value.X, X);
            SetOutputVariable(Value.Value.Y, Y);

            if (Out != null) Out();
        }
    }
}
