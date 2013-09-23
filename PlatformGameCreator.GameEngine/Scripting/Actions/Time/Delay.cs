/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Time
{
    /// <summary>
    /// Causes a delay in a sequence with a variable duration. Delays can be paused, restarted and aborted.
    /// </summary>
    [FriendlyName("Delay")]
    [Description("Causes a delay in a sequence with a variable duration. Delays can be paused, restarted and aborted.")]
    [Category("Actions/Time")]
    public class DelayAction : ActionNode
    {
        /// <summary>
        /// Fires when the timer runs out.
        /// </summary>
        [FriendlyName("Finished")]
        [Description("Fires when the timer runs out.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Fires when the delay is aborted.
        /// </summary>
        [FriendlyName("Aborted")]
        [Description("Fires when the delay is aborted.")]
        public ScriptSocketHandler Aborted;

        /// <summary>
        /// Duration in seconds how long the action will wait before firing the <see cref="Finished"/> output signal.
        /// </summary>
        [FriendlyName("Duration")]
        [Description("Duration in seconds how long the action will wait before firing the Finished output signal.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Duration;

        /// <summary>
        /// Starts the timer for the delay.
        /// </summary>
        [FriendlyName("Start")]
        [Description("Starts the timer for the delay.")]
        public void Start()
        {
            if (!running)
            {
                remainingTime = Duration.Value;
                running = true;

                StartUpdating();
            }
        }

        /// <summary>
        /// Resets the time and aborts the delay.
        /// </summary>
        [FriendlyName("Stop")]
        [Description("Resets the time and aborts the delay.")]
        public void Stop()
        {
            if (running) StopUpdating();

            remainingTime = Duration.Value;
            running = false;

            // fires signal Aborted
            if (Aborted != null) Aborted();
        }

        /// <summary>
        /// Pauses and resumes the timer without aborting the delay.
        /// </summary>
        [FriendlyName("Pause")]
        [Description("Pauses and resumes the timer without aborting the delay.")]
        public void Pause()
        {
            if (remainingTime > 0f)
            {
                running = !running;

                if (running) StartUpdating();
                else StopUpdating();
            }
        }

        // remaining time for the delay before firing the Finished output signal
        private double remainingTime = 0.0;
        // indicates whether the delay is running
        private bool running = false;

        /// <inheritdoc />
        /// <summary>
        /// Waits <see cref="Duration"/> seconds before firing the <see cref="Finished"/> output signal.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (running)
            {
                remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;

                if (remainingTime <= 0f)
                {
                    running = false;

                    StopUpdating();

                    if (Finished != null) Finished();
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Stops the delay when the node is interrupted.
        /// </remarks>
        protected override void OnUpdateStopped()
        {
            running = false;
        }
    }
}
