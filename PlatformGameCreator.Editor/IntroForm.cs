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

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Intro form form choosing the starting project.
    /// </summary>
    partial class IntroForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntroForm"/> class.
        /// </summary>
        public IntroForm()
        {
            InitializeComponent();

            Icon = Properties.Resources._2DPGC_Logo;

            if (!String.IsNullOrEmpty(Properties.Settings.Default.EditorApplication_LastOpenedProject) &&
                File.Exists(Properties.Settings.Default.EditorApplication_LastOpenedProject))
            {
                lastProjectTextBox.Text = Properties.Settings.Default.EditorApplication_LastOpenedProject;
            }
            else
            {
                openLastProjectButton.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the newProjectbutton control.
        /// Creates the new project.
        /// </summary>
        private void newProjectbutton_Click(object sender, EventArgs e)
        {
            EditorApplication.Editor.CreateNewProject();

            if (EditorApplication.Editor.Project != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the openProjectButton control.
        /// Opens the project.
        /// </summary>
        private void openProjectButton_Click(object sender, EventArgs e)
        {
            EditorApplication.Editor.OpenProject();

            if (EditorApplication.Editor.Project != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the openLastProjectButton control.
        /// Opens the last used project.
        /// </summary>
        private void openLastProjectButton_Click(object sender, EventArgs e)
        {
            EditorApplication.Editor.OpenProject(Properties.Settings.Default.EditorApplication_LastOpenedProject);

            if (EditorApplication.Editor.Project != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
