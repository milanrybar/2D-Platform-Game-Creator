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
    /// Counts the amount of time that elapses between starting and stopping.
    /// </summary>
    [FriendlyName("Timer")]
    [Description("Counts the amount of time that elapses between starting and stopping.")]
    [Category("Actions/Time")]
    public class TimerAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Outputs the amount of time in seconds elapsed. 
        /// </summary>
        [Description("Outputs the amount of time in seconds elapsed. ")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] Time;

        // elapsed time from starting the timer
        private double elapsedTime;

        /// <summary>
        /// Begins counting the elapsed time.
        /// </summary>
        [Description("Begins counting the elapsed time.")]
        public void Start()
        {
            elapsedTime = 0;

            StartUpdating();

            if (Out != null) Out();
        }

        /// <summary>
        /// Stops counting the elapsed time.
        /// </summary>
        [Description("Stops counting the elapsed time.")]
        public void Stop()
        {
            StopUpdating();

            SetOutputVariable((float)elapsedTime, Time);

            if (Out != null) Out();
        }

        /// <inheritdoc />
        /// <summary>
        /// Counts the elapsed time.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Clear the elapsed time when the node is interrupted.
        /// </remarks>
        protected override void OnUpdateStopped()
        {
            elapsedTime = 0;
        }
    }
}
