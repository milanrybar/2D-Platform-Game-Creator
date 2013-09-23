/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Conditions
{
    /// <summary>
    /// This action allows you to pass through a signal to the Out signal out socket depending on the state of the gate. The gate is automatically closed for the specified time when a signal pass through the gate.
    /// </summary>
    [FriendlyName("Timed Gate")]
    [Description("This action allows you to pass through a signal to the Out signal out socket depending on the state of the gate. The gate is automatically closed for the specified time when a signal pass through the gate.")]
    [Category("Actions/Conditions")]
    public class TimedGateAction : ActionNode
    {
        /// <summary>
        /// Fires when the signal went through the gate.
        /// </summary>
        [Description("Fires when the signal went through the gate.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Time in seconds for how long the gate is closed before it opens automatically.
        /// </summary>
        [Description("Time in seconds for how long the gate is closed before it opens automatically.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Duration;

        // indicates whether the gate is opened
        private bool gateOpen = true;
        // remaining time when the gate is closed
        private double remainingTime = -1;

        /// <summary>
        /// Passes signal through to the Out signal out socket and closes the gate for the time of <see cref="Duration"/> if the gate is currently in the open state.
        /// </summary>
        [Description("Passes signal through to the Out signal out socket and closes the gate for the time of Duration property if the gate is currently in the open state.")]
        public void In()
        {
            if (gateOpen)
            {
                gateOpen = false;
                remainingTime = Duration.Value;

                StartUpdating();

                if (Out != null) Out();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Opens the gate after <see cref="Duration"/> time elapsed.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (remainingTime < 0f)
            {
                gateOpen = true;
                StopUpdating();
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Opens gate when the node is interrupted.
        /// </remarks>
        protected override void OnUpdateStopped()
        {
            gateOpen = true;
        }
    }
}
