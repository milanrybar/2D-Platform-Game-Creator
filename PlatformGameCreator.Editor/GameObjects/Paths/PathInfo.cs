/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Scenes;

namespace PlatformGameCreator.Editor.GameObjects.Paths
{
    /// <summary>
    /// Control for editing <see cref="Paths.Path"/> settings.
    /// Everything is done automatically by setting <see cref="PathInfo.Path"/> property.
    /// </summary>
    partial class PathInfo : UserControl
    {
        /// <summary>
        /// Gets or sets the path to edit.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Path Path
        {
            get { return _path; }
            set
            {
                if (_path != null)
                {
                    _path.NameChanged -= new EventHandler(Path_NameChanged);
                    _path.LoopChanged -= new EventHandler(Path_LoopChanged);
                }

                _path = value;

                if (_path != null)
                {
                    _path.NameChanged += new EventHandler(Path_NameChanged);
                    _path.LoopChanged += new EventHandler(Path_LoopChanged);

                    nameTextBox.Text = _path.Name;
                    loopCheckbox.Checked = _path.Loop;
                }
            }
        }
        private Path _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathInfo"/> class.
        /// </summary>
        public PathInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the LoopChanged event of the path.
        /// Updates loop value in the checkbox for editing path loop value.
        /// </summary>
        private void Path_LoopChanged(object sender, EventArgs e)
        {
            loopCheckbox.Checked = Path.Loop;
        }

        /// <summary>
        /// Handles the NameChanged event of the path.
        /// Updates name value in the textbox for editing path name.
        /// </summary>
        private void Path_NameChanged(object sender, EventArgs e)
        {
            nameTextBox.Text = Path.Name;
        }

        /// <summary>
        /// Handles the Click event of the editPathButton control.
        /// Begins editing the path at the scene.
        /// </summary>
        private void editPathButton_Click(object sender, EventArgs e)
        {
            EditorApplication.Editor.BeginEditPath(Path);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the loopCheckbox control.
        /// Updates Loop property in the path.
        /// </summary>
        private void loopCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Path.Loop = loopCheckbox.Checked;
        }

        /// <summary>
        /// Handles the Leave event of the nameTextBox control.
        /// Updates the name of the path.
        /// </summary>
        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (nameTextBox.Text != String.Empty)
            {
                Path.Name = nameTextBox.Text;
            }
            else
            {
                nameTextBox.Text = Path.Name;
            }
        }

        /// <summary>
        /// Handles the ParentChanged event of the PathInfo control.
        /// Unset the <see cref="Path"/> property.
        /// </summary>
        private void PathInfo_ParentChanged(object sender, EventArgs e)
        {
            if (Parent == null)
            {
                Path = null;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Path = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
