/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FarseerPhysics.Common;
using FarseerPhysics.Common.ConvexHull;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Controller for the <see cref="TextureScreen"/>.
    /// </summary>
    partial class TextureControllerForShapesEditing : BasicControllerForShapesEditing
    {
        /// <summary>
        /// <see cref="TextureScreen"/> for which is the controller used. 
        /// </summary>
        private TextureScreen textureScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureControllerForShapesEditing"/> class.
        /// </summary>
        /// <param name="textureScreen"><see cref="TextureScreen"/> for which is the controller used.</param>
        public TextureControllerForShapesEditing(TextureScreen textureScreen)
            : base(textureScreen)
        {
            this.textureScreen = textureScreen;

            InitializeComponent();
        }

        /// <summary>
        /// Creates polygon from the texture.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void createPolygonFromTextureButton_Click(object sender, EventArgs e)
        {
            ShapeState newItem = CreatePolygonFromTexture();

            if (newItem != null)
            {
                ShapesList.Items.Add(newItem);
                ShapesList.SelectedIndex = ShapesList.Items.Count - 1;
            }

            Invalidate();
        }

        /// <summary>
        /// Creates polygon from the texture.
        /// </summary>
        /// <returns>Returns created polygon as <see cref="ShapeState"/> or null when unsuccessful.</returns>
        public ShapeState CreatePolygonFromTexture()
        {
            // texture data
            uint[] data = new uint[textureScreen.Texture.TextureXna.Width * textureScreen.Texture.TextureXna.Height];
            textureScreen.Texture.TextureXna.GetData(data);

            // compute vertices from texture
            Vertices textureVertices;
            try
            {
                textureVertices = PolygonTools.CreatePolygon(data, textureScreen.Texture.TextureXna.Width, true);
            }
            catch
            {
                Messages.ShowError("Unable to convert texture to polygon.");
                return null;
            }

            // no polygon
            if (textureVertices.Count < 3)
            {
                Messages.ShowError("Result polygon would have less then 3 vertices.");
                return null;
            }
            // polygon is not simple
            else if (!textureVertices.IsSimple())
            {
                Messages.ShowError("Result polygon would have crossing edges. Probably texture cointains separate regions.");
                return null;
            }
            // polygon is correct => move polygon to correct position
            else
            {
                for (int i = 0; i < textureVertices.Count; ++i)
                {
                    textureVertices[i] -= textureScreen.Texture.Origin;
                }
            }

            return textureScreen.AddShape(new Polygon(textureVertices));
        }
    }
}
