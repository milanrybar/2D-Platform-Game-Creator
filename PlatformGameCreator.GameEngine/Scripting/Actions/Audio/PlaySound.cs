/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Audio
{
    /// <summary>
    /// Plays the specified sound.
    /// </summary>
    [FriendlyName("Play Sound")]
    [Description("Plays the specified sound.")]
    [Category("Actions/Audio")]
    public class PlaySoundAction : ActionNode
    {
        /// <summary>
        /// Fires when the sound is set to play.
        /// </summary>
        [Description("Fires when the sound is set to play.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Fires when the playing of the sound has been stopped.
        /// </summary>
        [Description("Fires when the playing of the sound has been stopped.")]
        public ScriptSocketHandler Stopped;

        /// <summary>
        /// Fires when the playing of the sound is finished.
        /// </summary>
        [Description("Fires when the playing of the sound is finished.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Sound to play.
        /// </summary>
        [Description("Sound to play.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<SoundEffect> Sound;

        /// <summary>
        /// Indicates whether the sound will be looped.
        /// </summary>
        [Description("Indicates whether the sound will be looped.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(false)]
        public Variable<bool> Loop;

        /// <summary>
        /// Volume of the sound. Value 1 is 100% volume.
        /// </summary>
        [Description("Volume of the sound. Value 1 is 100% volume.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(1f)]
        public Variable<float> Volume;

        // last used sound instance
        private SoundEffectInstance soundInstace = null;
        // indicates whether the last used sound instance is stopped
        bool stopped = false;

        /// <summary>
        /// Begins playing the specified sound.
        /// </summary>
        [Description("Begins playing the specified sound.")]
        public void Play()
        {
            if (Sound != null && Sound.Value != null)
            {
                soundInstace = Sound.Value.CreateInstance();

                soundInstace.Volume = Volume.Value;
                soundInstace.IsLooped = Loop.Value;

                soundInstace.Play();
                stopped = false;

                if (Finished != null) StartUpdating();
            }

            if (Out != null) Out();
        }

        /// <summary>
        /// Stops playing the last used sound.
        /// </summary>
        [Description("Stops playing the last used sound.")]
        public void Stop()
        {
            if (soundInstace != null)
            {
                soundInstace.Stop();
                stopped = true;

                StopUpdating();

                if (Stopped != null) Stopped();
            }

            if (Out != null) Out();
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if the last sound is still playing.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (soundInstace.State != SoundState.Playing)
            {
                soundInstace = null;

                StopUpdating();

                if (!stopped && Finished != null) Finished();
            }
        }
    }
}
