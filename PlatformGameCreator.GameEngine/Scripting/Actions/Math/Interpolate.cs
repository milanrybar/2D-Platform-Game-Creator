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
    /// Interpolates between two values.
    /// </summary>
    /// <typeparam name="T">Type of the value to interpolate.</typeparam>
    [Category("Actions/Math")]
    public abstract class InterpolateBaseAction<T> : ActionNode
    {
        /// <summary>
        /// Fires when the interpolation is completed.
        /// </summary>
        [Description("Fires when the interpolation is completed.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Fires when the interpolation is aborted.
        /// </summary>
        [Description("Fires when the interpolation is aborted.")]
        public ScriptSocketHandler Aborted;

        /// <summary>
        /// Fires for every interpolated value.
        /// </summary>
        [Description("Fires for every interpolated value.")]
        public ScriptSocketHandler Interpolating;

        /// <summary>
        /// Start value for the interpolation.
        /// </summary>
        [FriendlyName("Start Value")]
        [Description("Start value for the interpolation.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> StartValue;

        /// <summary>
        /// End value for the interpolation.
        /// </summary>
        [FriendlyName("End Value")]
        [Description("End value for the interpolation.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> EndValue;

        /// <summary>
        /// Amount of time in seconds for the interpolation.
        /// </summary>
        [Description("Amount of time in seconds for the interpolation.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Time;

        /// <summary>
        /// Outputs the interpolated value.
        /// </summary>
        [FriendlyName("Output Value")]
        [Description("Outputs the interpolated value.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<T>[] Output;

        /// <summary>
        /// Whole interpolation time in seconds.
        /// </summary>
        protected double time;

        /// <summary>
        /// Remaining interpolation time in seconds.
        /// </summary>
        protected double remainingTime = -1;

        /// <summary>
        /// Start value for the interpolation.
        /// </summary>
        protected T start;

        /// <summary>
        /// End value for the interpolation.
        /// </summary>
        protected T end;

        /// <summary>
        /// Current interpolated value.
        /// </summary>
        protected T output;

        // indicates whether the interpolation is in progress
        private bool running = false;

        /// <summary>
        /// Starts the interpolation between two specified values.
        /// </summary>
        [Description("Starts the interpolation between two specified values.")]
        public void Start()
        {
            start = StartValue.Value;
            end = EndValue.Value;
            time = Time.Value;
            remainingTime = Time.Value;
            running = true;

            StartUpdating();
        }

        /// <summary>
        /// Stops the running interpolation if any is in progress.
        /// </summary>
        [Description("Stops the running interpolation if any is in progress.")]
        public void Stop()
        {
            if (running)
            {
                running = false;
                StopUpdating();

                if (Aborted != null) Aborted();
            }
        }

        /// <summary>
        /// Pauses or resumes the last interpolation if any is available.
        /// </summary>
        [Description("Pauses or resumes the last interpolation if any is available.")]
        public void Pause()
        {
            if (remainingTime >= 0f)
            {
                running = !running;

                if (running) StartUpdating();
                else StopUpdating();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Interpolates new value or finishes the interpolation.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (running)
            {
                remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;

                if (remainingTime < 0f)
                {
                    running = false;
                    StopUpdating();

                    if (Finished != null) Finished();
                }
                else
                {
                    ComputeOutput();
                    SetOutputVariable(output, Output);

                    if (Interpolating != null) Interpolating();
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Stops running interpolation when node is interrupted.
        /// </summary>
        protected override void OnUpdateStopped()
        {
            running = false;
        }

        /// <summary>
        /// Computes the interpolated value for the current interpolating time and save the result to the <see cref="output"/> variable.
        /// </summary>
        protected abstract void ComputeOutput();
    }

    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    [FriendlyName("Interpolate Int Linear")]
    [Description("Linearly interpolates between two values.")]
    public class InterpolateIntLinearAction : InterpolateBaseAction<int>
    {
        /// <inheritdoc />
        protected override void ComputeOutput()
        {
            output = (int)MathHelper.Lerp(start, end, 1f - (float)(remainingTime / time));
        }
    }

    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    [FriendlyName("Interpolate Float Linear")]
    [Description("Linearly interpolates between two values.")]
    public class InterpolateFloatLinearAction : InterpolateBaseAction<float>
    {
        /// <inheritdoc />
        protected override void ComputeOutput()
        {
            output = MathHelper.Lerp(start, end, 1f - (float)(remainingTime / time));
        }
    }

    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    [FriendlyName("Interpolate Vector Linear")]
    [Description("Linearly interpolates between two values.")]
    public class InterpolateVectorLinearAction : InterpolateBaseAction<Vector2>
    {
        /// <inheritdoc />
        protected override void ComputeOutput()
        {
            Vector2.Lerp(ref start, ref end, 1f - (float)(remainingTime / time), out output);
        }
    }

    /// <summary>
    /// Interpolates between two values using a cubic equation.
    /// </summary>
    [FriendlyName("Interpolate Int Cubic")]
    [Description("Interpolates between two values using a cubic equation.")]
    public class InterpolateIntCubicAction : InterpolateBaseAction<int>
    {
        /// <inheritdoc />
        protected override void ComputeOutput()
        {
            output = (int)MathHelper.SmoothStep(start, end, 1f - (float)(remainingTime / time));
        }
    }

    /// <summary>
    /// Interpolates between two values using a cubic equation.
    /// </summary>
    [FriendlyName("Interpolate Float Cubic")]
    [Description("Interpolates between two values using a cubic equation.")]
    public class InterpolateFloatCubicAction : InterpolateBaseAction<float>
    {
        /// <inheritdoc />
        protected override void ComputeOutput()
        {
            output = MathHelper.SmoothStep(start, end, 1f - (float)(remainingTime / time));
        }
    }

    /// <summary>
    /// Interpolates between two values using a cubic equation.
    /// </summary>
    [FriendlyName("Interpolate Vector Cubic")]
    [Description("Interpolates between two values using a cubic equation.")]
    public class InterpolateVectorCubicAction : InterpolateBaseAction<Vector2>
    {
        /// <inheritdoc />
        protected override void ComputeOutput()
        {
            Vector2.SmoothStep(ref start, ref end, 1f - (float)(remainingTime / time), out output);
        }
    }
}
