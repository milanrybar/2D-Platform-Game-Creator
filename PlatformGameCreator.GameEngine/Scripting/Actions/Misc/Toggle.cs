/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Misc
{
    /// <summary>
    /// Toggles the specified values.
    /// </summary>
    [FriendlyName("Toggle")]
    [Description("Toggles the specified values.")]
    [Category("Actions/Misc")]
    public class ToggleAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Bool variables to toggle.
        /// </summary>
        [FriendlyName("Bool")]
        [Description("Bool variables to toggle.")]
        [VariableSocket(VariableSocketType.In, CanBeEmpty = true)]
        public Variable<bool>[] Bool;

        /// <summary>
        /// Sets the specified bool variables to true.
        /// </summary>
        [FriendlyName("Turn On")]
        [Description("Sets the specified bool variables to true.")]
        public void TurnOn()
        {
            if (Bool != null)
            {
                for (int i = 0; i < Bool.Length; ++i)
                {
                    Bool[i].Value = true;
                }
            }

            if (Out != null) Out();
        }

        /// <summary>
        /// Sets the specified bool variables to false.
        /// </summary>
        [FriendlyName("Turn Off")]
        [Description("Sets the specified bool variables to false.")]
        public void TurnOff()
        {
            if (Bool != null)
            {
                for (int i = 0; i < Bool.Length; ++i)
                {
                    Bool[i].Value = false;
                }
            }

            if (Out != null) Out();
        }

        /// <summary>
        /// Toggles the specified bool variables.
        /// </summary>
        [FriendlyName("Toggle")]
        [Description("Toggles the specified bool variables.")]
        public void Toggle()
        {
            if (Bool != null)
            {
                for (int i = 0; i < Bool.Length; ++i)
                {
                    Bool[i].Value = !Bool[i].Value;
                }
            }

            if (Out != null) Out();
        }
    }
}
