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
    /// Scales vector by the specified scale coefficient (multiplies the vector by the scalar value) and returns the result.
    /// </summary>
    [FriendlyName("Scale Vector")]
    [Description("Scales vector by the specified scale coefficient (multiplies the vector by the scalar value) and returns the result.")]
    [Category("Actions/Math")]
    public class ScaleVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Vector to scale by the specified scale coefficient. 
        /// </summary>
        [Description("Vector to scale by the specified scale coefficient. ")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Value;

        /// <summary>
        /// Scale coefficient to scale the specified vector by.
        /// </summary>
        [Description("Scale coefficient to scale the specified vector by.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Scale;

        /// <summary>
        /// Outputs the scaled vector by the specified scale coefficient.
        /// </summary>
        [Description("Outputs the scaled vector by the specified scale coefficient.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Scale.Value * Value.Value, Result);

            if (Out != null) Out();
        }
    }
}
