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

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Form for editing the game (project) settings.
    /// Everything is done automatically by setting <see cref="GameSettingsForm.Settings"/> property.
    /// </summary>
    partial class GameSettingsForm : Form
    {
        /// <summary>
        /// Gets or sets the <see cref="ProjectSettings"/> to edit.
        /// </summary>
        public ProjectSettings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;

                if (_settings != null)
                {
                    gameWindowWidthNumericUpDown.Value = _settings.GameWindowWidth;
                    gameWindowHeightNumericUpDown.Value = _settings.GameWindowHeight;
                    simulationUnitsFloatBox.Value = _settings.SimulationUnits;
                    defaultGravityXFloatBox.Value = _settings.DefaultGravity.X;
                    defaultGravityYFloatBox.Value = _settings.DefaultGravity.Y;
                    backgroundColorRNumericUpDown.Value = _settings.BackgroundColor.R;
                    backgroundColorGNumericUpDown.Value = _settings.BackgroundColor.G;
                    backgroundColorBNumericUpDown.Value = _settings.BackgroundColor.B;
                    continuousCollisionDetectionCheckBox.Checked = _settings.ContinuousCollisionDetection;
                    fullScreenCheckBox.Checked = _settings.GameIsFullScreen;
                }
            }
        }
        private ProjectSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettingsForm"/> class.
        /// </summary>
        public GameSettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the ValueChanged event of the gameWindowWidthNumericUpDown control.
        /// Updates the <see cref="ProjectSettings.GameWindowWidth"/> property.
        /// </summary>
        private void gameWindowWidthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Settings.GameWindowWidth = (int)gameWindowWidthNumericUpDown.Value;
        }

        /// <summary>
        /// Handles the ValueChanged event of the gameWindowHeightNumericUpDown control.
        /// Updates the <see cref="ProjectSettings.GameWindowHeight"/> property.
        /// </summary>
        private void gameWindowHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Settings.GameWindowHeight = (int)gameWindowHeightNumericUpDown.Value;
        }

        /// <summary>
        /// Handles the ValueChanged event of the simulationUnitsFloatBox control.
        /// Updates the <see cref="ProjectSettings.SimulationUnits"/> property.
        /// </summary>
        private void simulationUnitsFloatBox_ValueChanged(float value)
        {
            if (value >= 1)
            {
                Settings.SimulationUnits = value;
            }
            else
            {
                simulationUnitsFloatBox.Value = Settings.SimulationUnits;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the defaultGravityXFloatBox control.
        /// Updates the <see cref="ProjectSettings.DefaultGravity"/> property of the X coordinate.
        /// </summary>
        private void defaultGravityXFloatBox_ValueChanged(float value)
        {
            Settings.DefaultGravity.X = value;
        }

        /// <summary>
        /// Handles the ValueChanged event of the defaultGravityYFloatBox control.
        /// Updates the <see cref="ProjectSettings.DefaultGravity"/> property of the Y coordinate.
        /// </summary>
        private void defaultGravityYFloatBox_ValueChanged(float value)
        {
            Settings.DefaultGravity.Y = value;
        }

        /// <summary>
        /// Handles the Click event of the okButton control.
        /// Closes the form.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the ValueChanged event of the backgroundColorRNumericUpDown control.
        /// Updates the <see cref="ProjectSettings.BackgroundColor"/> property of the Red part.
        /// </summary>
        private void backgroundColorRNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Settings.BackgroundColor.R = (byte)backgroundColorRNumericUpDown.Value;
        }

        /// <summary>
        /// Handles the ValueChanged event of the backgroundColorGNumericUpDown control.
        /// Updates the <see cref="ProjectSettings.BackgroundColor"/> property of the Green part.
        /// </summary>
        private void backgroundColorGNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Settings.BackgroundColor.G = (byte)backgroundColorGNumericUpDown.Value;
        }

        /// <summary>
        /// Handles the ValueChanged event of the backgroundColorBNumericUpDown control.
        /// Updates the <see cref="ProjectSettings.BackgroundColor"/> property of the Blue part.
        /// </summary>
        private void backgroundColorBNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Settings.BackgroundColor.B = (byte)backgroundColorBNumericUpDown.Value;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the continuousCollisionDetectionCheckBox control.
        /// Updates the <see cref="ProjectSettings.ContinuousCollisionDetection"/> property.
        /// </summary>
        private void continuousCollisionDetectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ContinuousCollisionDetection = continuousCollisionDetectionCheckBox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the fullScreenCheckBox control.
        /// Updates the <see cref="ProjectSettings.GameIsFullScreen"/> property.
        /// </summary>
        private void fullScreenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.GameIsFullScreen = fullScreenCheckBox.Checked;
        }
    }
}
