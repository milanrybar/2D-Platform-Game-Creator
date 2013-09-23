/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Audio
{
    /// <summary>
    /// Plays the specified song as the background song. Only one song can be playing as the background song. 
    /// </summary>
    [FriendlyName("Play Background Song")]
    [Description("Plays the specified song as the background song. Only one song can be playing as the background song.")]
    [Category("Actions/Audio")]
    public class PlayBackgroundSongAction : ActionNode
    {
        /// <summary>
        /// Fires when the song is set as background song.
        /// </summary>
        [Description("Fires when the song is set as background song.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Fires when the playing of the song is finished.
        /// </summary>
        [Description("Fires when the playing of the song is finished.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Song to used as background song.
        /// </summary>
        [Description("Song to used as background song.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Song> Song;

        /// <summary>
        /// Indicates whether the song will be looped.
        /// </summary>
        [Description("Indicates whether the song will be looped.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(false)]
        public Variable<bool> Loop;

        /// <summary>
        /// Volume of the song. Value 1 is 100% volume.
        /// </summary>
        [Description("Volume of the song. Value 1 is 100% volume.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(1f)]
        public Variable<float> Volume;

        /// <summary>
        /// Begins playing the specified song as the background song.
        /// </summary>
        [Description("Begins playing the specified song as the background song.")]
        public void Play()
        {
            if (Song != null && Song.Value != null)
            {
                MediaPlayer.Play(Song.Value);

                MediaPlayer.IsRepeating = Loop.Value;
                MediaPlayer.Volume = Volume.Value;

                if (Finished != null) StartUpdating();
            }

            if (Out != null) Out();
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if the song is still playing.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                StopUpdating();

                if (Finished != null) Finished();
            }
        }
    }
}
