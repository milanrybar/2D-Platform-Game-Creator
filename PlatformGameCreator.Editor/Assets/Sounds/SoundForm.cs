/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PlatformGameCreator.Editor.Assets.Sounds
{
    /// <summary>
    /// Form for playing the <see cref="Sound"/>
    /// </summary>
    partial class SoundForm : Form
    {
        /// <summary>
        /// Sound to play.
        /// </summary>
        private Sound sound;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundForm"/> class.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        public SoundForm(Sound sound)
        {
            this.sound = sound;

            InitializeComponent();

            Icon = Properties.Resources._2DPGC_Logo;

            Text = String.Format("Sound Editor - {0}", sound.Name);

            this.axWindowsMediaPlayer.URL = Path.Combine(Project.Singleton.ContentDirectory, sound.Filename);
        }

        /// <summary>
        /// Handles the FormClosing event of the SoundForm control.
        /// Stops playing of the sound.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void SoundForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // stop playing song
            axWindowsMediaPlayer.Ctlcontrols.stop();
        }
    }
}
