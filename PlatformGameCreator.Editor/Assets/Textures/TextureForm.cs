/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Form for completely editing the <see cref="Texture"/>
    /// Editing collision shapes, origin and name of the <see cref="Texture"/>.
    /// </summary>
    partial class TextureForm : Form
    {
        /// <summary>
        /// <see cref="TextureScreen"/> for editing collision shapes and origin of the <see cref="Texture"/>. 
        /// </summary>
        private TextureScreen textureScreen;

        /// <summary>
        /// Controller for the <see cref="TextureScreen"/>.
        /// </summary>
        private TextureControllerForShapesEditing shapesController;

        /// <summary>
        /// Texture that is edited.
        /// </summary>
        private Texture texture;

        /// <summary>
        /// Messages manager for this form.
        /// </summary>
        private IMessagesManager messagesManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureForm"/> class.
        /// </summary>
        /// <param name="texture">The texture to edit.</param>
        public TextureForm(Texture texture)
        {
            InitializeComponent();

            Icon = Properties.Resources._2DPGC_Logo;

            // messages manager
            messagesManager = new DefaultMessagesManager(statusLabel);
            Messages.MessagesManager = messagesManager;

            // texture
            this.texture = texture;

            // init controls settings
            nameTextBox.Text = texture.Name;

            // texture screen
            textureScreen = new TextureScreen();
            textureScreen.Texture = texture;
            textureScreen.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(textureScreen, 0, 0);
            textureScreen.Zoom = 100;
            textureScreen.Position = new PointF(-textureScreen.Width / 2f, -textureScreen.Height / 2f);

            // shapes controller 
            shapesController = new TextureControllerForShapesEditing(textureScreen);
            shapesController.Location = new Point(3, 81);
            textureSettingsPanel.Controls.Add(shapesController);

            // title (window text)
            UpdateTitle();

            // init shapes from texture
            foreach (Shape shape in texture.Shapes)
            {
                ShapeState newItem = textureScreen.AddShape(shape);
                shapesController.ShapesList.Items.Add(newItem);

            }
            shapesController.ShapesList.SelectedIndex = shapesController.ShapesList.Items.Count - 1;
        }

        /// <summary>
        /// Called when the name of the texture changes. Updates title of form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (nameTextBox.Text != texture.Name)
            {
                texture.Name = nameTextBox.Text;
                UpdateTitle();
            }
        }

        /// <summary>
        /// Updates title of form.
        /// </summary>
        private void UpdateTitle()
        {
            Text = "Texture Editor - " + texture.Name;
        }

        /// <summary>
        /// Handles the FormClosing event of the TextureForm control.
        /// Checks if collision shapes are valid. If not form is not closed until all shapes are valid.
        /// If yes, saves data to the texture and invokes its <see cref="DrawableAsset.DrawableAssetChanged"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void TextureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // check if every shape is valid
            for (int i = 0; i < shapesController.ShapesList.Items.Count; ++i)
            {
                ShapeState shapeState = shapesController.ShapesList.Items[i] as ShapeState;
                if (shapeState != null)
                {
                    // shape is not valid
                    if (!shapeState.IsShapeValid())
                    {
                        // select invalid shape
                        shapesController.ShapesList.SelectedIndex = i;
                        // show message
                        shapeState.OnInvalidShape();
                        // form will not be closed until all shapes are valid
                        e.Cancel = true;
                        return;
                    }
                }
            }

            // texture is valid => update texture
            UpdateTexture();

            // texture changed
            texture.InvokeDrawableAssetChanged();
        }

        /// <summary>
        /// Sets state for changing the origin of the texture.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void changeOriginButton_Click(object sender, EventArgs e)
        {
            textureScreen.State = new ChangeOriginState(textureScreen.State, textureScreen);
        }

        /// <summary>
        /// Updates underlying texture. Saves all data to texture.
        /// </summary>
        private void UpdateTexture()
        {
            // save shapes to the texture
            texture.Shapes.Clear();

            foreach (ShapeState shape in textureScreen.Shapes)
            {
                texture.Shapes.Add(shape.Shape);
            }
        }

        /// <summary>
        /// Handles the Activated event of the TextureForm control.
        /// </summary>
        private void TextureForm_Activated(object sender, EventArgs e)
        {
            Messages.MessagesManager = messagesManager;
            shapesController.SetSettings();
            ShapesEditingState.DrawingTools = textureScreen;
        }
    }
}
