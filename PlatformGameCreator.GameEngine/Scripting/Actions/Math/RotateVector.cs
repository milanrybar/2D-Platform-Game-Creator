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
    /// Rotates the vector by the specified angle and returns the result.
    /// </summary>
    [FriendlyName("Rotate Vector")]
    [Description("Rotates the vector by the specified angle and returns the result.")]
    [Category("Actions/Math")]
    public class RotateVectorAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Vector to rotate by the specified angle.
        /// </summary>
        [Description("Vector to rotate by the specified angle.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Value;

        /// <summary>
        /// Angle to rotate the specified vector by.
        /// </summary>
        [Description("Angle to rotate the specified vector by.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<float> Angle;

        /// <summary>
        /// Outputs the rotated vector by the specified angle.
        /// </summary>
        [Description("Outputs the rotated vector by the specified angle.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] Result;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Vector2.Transform(Value.Value, Matrix.CreateRotationZ(Angle.Value)), Result);

            if (Out != null) Out();
        }
    }
}
