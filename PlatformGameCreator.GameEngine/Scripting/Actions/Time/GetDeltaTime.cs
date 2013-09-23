/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Time
{
    /// <summary>
    /// Gets the current delta time (time elapsed from the previous game frame).
    /// </summary>
    [FriendlyName("Get Delta Time")]
    [Description("Gets the current delta time (time elapsed from the previous game frame).")]
    [Category("Actions/Time")]
    public class GetDeltaTimeAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Outputs the delta time in seconds.
        /// </summary>
        [Description("Outputs the delta time in seconds.")]
        [FriendlyName("Delta Time")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] DeltaTime;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Container.Actor.Screen.ElapsedTime, DeltaTime);

            if (Out != null) Out();
        }
    }
}
