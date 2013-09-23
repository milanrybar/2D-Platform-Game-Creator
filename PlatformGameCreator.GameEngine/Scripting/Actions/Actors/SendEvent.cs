/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Sends event from the actor owner to the world.
    /// </summary>
    [FriendlyName("Send Event")]
    [Description("Sends event from the actor owner to the world.")]
    [Category("Actions/Actors")]
    public class SendEventAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Name of the event (event out) to send.
        /// </summary>
        [Description("Name of the event (event out) to send.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string> Name;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            EventWrapper eventWrapper = Container.Actor.GetEvent(Name.Value);

            if (eventWrapper != null)
            {
                eventWrapper.Invoke();
            }

            if (Out != null) Out();
        }
    }
}
