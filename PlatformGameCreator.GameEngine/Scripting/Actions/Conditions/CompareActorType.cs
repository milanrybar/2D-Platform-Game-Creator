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
    /// Compares two specified values.
    /// </summary>
    [FriendlyName("Compares Actor Type")]
    [Description("Compare two specified values.")]
    [Category("Actions/Conditions")]
    public class CompareActorTypeAction : ActionNode
    {
        /// <summary>
        /// Fires if the first value intersects the second value.
        /// </summary>
        [FriendlyName("Intersect")]
        [Description("Fires if the first value intersects the second value.")]
        public ScriptSocketHandler Intersect;

        /// <summary>
        /// Fires if the first value does not intersect the second value.
        /// </summary>
        [FriendlyName("Not Intersect")]
        [Description("Fires if the first value does not intersect the second value.")]
        public ScriptSocketHandler NotIntersect;

        /// <summary>
        /// Fires if the first value is equal to the second value.
        /// </summary>
        [FriendlyName("A = B")]
        [Description("Fires if the first value is equal to the second value.")]
        public ScriptSocketHandler Equal;

        /// <summary>
        /// Fires if the first value is not equal to the second value.
        /// </summary>
        [FriendlyName("A != B")]
        [Description("Fires if the first value is not equal to the second value.")]
        public ScriptSocketHandler NotEqual;

        /// <summary>
        /// First value to compare.
        /// </summary>
        [Description("First value to compare.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<uint> A;

        /// <summary>
        /// Second value to compare.
        /// </summary>
        [Description("Second value to compare.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<uint> B;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if ((A.Value & B.Value) != 0)
            {
                if (Intersect != null) Intersect();
            }
            else if (NotIntersect != null) NotIntersect();

            if (A.Value == B.Value)
            {
                if (Equal != null) Equal();
            }
            else if (NotEqual != null) NotEqual();
        }
    }
}
