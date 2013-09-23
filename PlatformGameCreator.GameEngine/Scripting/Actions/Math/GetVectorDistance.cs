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
    /// Calculates the distance between two vector variables and returns the result.
    /// </summary>
    [FriendlyName("Get Vector Distance")]
    [Description("Calculates the distance between two vector variables and returns the result.")]
    [Category("Actions/Math")]
    public class GetVectorDistanceAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// The first vector variable for calculating distance.
        /// </summary>
        [Description("The first vector variable for calculating distance.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> A;

        /// <summary>
        /// The second vector variable for calculating distance.
        /// </summary>
        [Description("The second vector variable for calculating distance.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> B;

        /// <summary>
        /// Outputs the distance between two specified vector variables.
        /// </summary>
        [Description("Outputs the distance between two specified vector variables.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Distance;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable((B.Value - A.Value).Length(), Distance);

            if (Out != null) Out();
        }
    }
}
