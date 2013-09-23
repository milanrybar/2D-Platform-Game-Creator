/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Events
{
    /// <summary>
    /// Fires when the state is starting or ending.
    /// </summary>
    [FriendlyName("State Events")]
    [Description("Fires when the state is starting or ending.")]
    [Category("Events")]
    public class StateEvents : EventNode
    {
        /// <summary>
        /// Fires when the state is starting.
        /// </summary>
        [FriendlyName("On Start")]
        [Description("Fires when the state is starting.")]
        public ScriptSocketHandler OnStart;

        /// <summary>
        /// Fires when the state is ending.
        /// </summary>
        [FriendlyName("On End")]
        [Description("Fires when the state is ending.")]
        public ScriptSocketHandler OnEnd;

        /// <inheritdoc />
        /// <remarks>
        /// Connects to the <see cref="State"/> <see cref="State.OnStart"/> and <see cref="State.OnEnd"/> events.
        /// </remarks>
        public override void Connect()
        {
            Container.OnStart += OnStarting;
            Container.OnEnd += OnEnding;
        }

        /// <summary>
        /// The state is starting => fires <see cref="OnStart"/>.
        /// </summary>
        private void OnStarting()
        {
            if (OnStart != null) OnStart();
        }

        /// <summary>
        /// The state is ending => fires <see cref="OnEnd"/>.
        /// </summary>
        private void OnEnding()
        {
            if (OnEnd != null) OnEnd();
        }
    }
}
