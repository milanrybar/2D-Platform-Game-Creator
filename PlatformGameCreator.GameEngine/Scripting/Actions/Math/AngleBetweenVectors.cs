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
    /// Computes angle between two vector variables and returns the result.
    /// </summary>
    [FriendlyName("Angle Between Vectors")]
    [Description("Computes angle between two vector variables and returns the result.")]
    [Category("Actions/Math")]
    public class AngleBetweenVectorsAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// The first vector variable to compute angle.
        /// </summary>
        [Description("The first vector variable to compute angle.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> A;

        /// <summary>
        /// The second vector variable to compute angle.
        /// </summary>
        [Description("The second vector variable to compute angle.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> B;

        /// <summary>
        /// Outputs the result angle between two vector variables.
        /// </summary>
        [Description("Outputs the result angle between two vector variables.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Angle;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            double angle = System.Math.Atan2(B.Value.Y, B.Value.X) - System.Math.Atan2(A.Value.Y, A.Value.X);

            SetOutputVariable((float)angle, Angle);

            if (Out != null) Out();
        }
    }
}
