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
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Basic controller for the <see cref="ShapesEditingScreen"/>.
    /// </summary>
    /// <remarks>
    /// Contains basic functionality for controling the <see cref="ShapesEditingScreen"/>.
    /// </remarks>
    partial class BasicControllerForShapesEditing : UserControl
    {
        /// <summary>
        /// Gets the <see cref="ShapesEditingScreen"/> for which is the controller used.
        /// </summary>
        public ShapesEditingScreen ShapesScreen
        {
            get { return _shapesScreen; }
        }
        private ShapesEditingScreen _shapesScreen;

        /// <summary>
        /// Gets the list of the shapes as the used <see cref="ListBox"/> control.
        /// </summary>
        public ListBox ShapesList
        {
            get { return shapesList; }
        }

        // brush for drawing items in ListBox (shapes list)
        private SolidBrush brush = new SolidBrush(Color.Black);

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicControllerForShapesEditing"/> class.
        /// </summary>
        /// <param name="shapesScreen"><see cref="ShapesEditingScreen"/> for which is the controller used.</param>
        public BasicControllerForShapesEditing(ShapesEditingScreen shapesScreen)
        {
            _shapesScreen = shapesScreen;

            InitializeComponent();

            ShapesScreen.StateChanged += new EventHandler(ShapesScreen_StateChanged);
            ShapesScreen_StateChanged(null, null);

            // init controls settings
            selectShapeBox.SelectedIndex = 0;

            // init defaults from settings
            showGrid.Checked = Properties.Settings.Default.TextureEditor_ShowGrid;
            showAllShapes.Checked = Properties.Settings.Default.TextureEditor_ShowAllShapes;
            showConvexHull.Checked = Properties.Settings.Default.TextureEditor_ShowConvexHull;
            showConvexDecomposition.Checked = Properties.Settings.Default.TextureEditor_ShowConvexDecomposition;
            showOrigin.Checked = Properties.Settings.Default.TextureEditor_ShowOrigin;
        }

        /// <summary>
        /// Sets the settings to the controls.
        /// </summary>
        internal void SetSettings()
        {
            ShapesScreen.ShowGrid = showGrid.Checked;
            ShapesScreen.ShowAllShapes = showAllShapes.Checked;
            PolygonEditState.ShowConvexHull = showConvexHull.Checked;
            PolygonEditState.ShowConvexDecomposition = showConvexDecomposition.Checked;
            ShapesScreen.ShowOrigin = showOrigin.Checked;
        }

        /// <summary>
        /// Removes the specified shape.
        /// </summary>
        /// <param name="shape">Shape as <see cref="ShapeState"/> to remove.</param>
        private void RemoveShape(ShapeState shape)
        {
            shapesList.Items.Remove(shape);
            ShapesScreen.RemoveShape(shape);
        }

        /// <summary>
        /// Called when <see cref="ShapesEditingScreen"/> state changes.
        /// Changes visibility of controls for the current shape.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ShapesScreen_StateChanged(object sender, EventArgs e)
        {
            ShapeState shapeState = ShapesScreen.State as ShapeState;
            if (shapeState == null)
            {
                polygonSettings.Visible = false;
            }
            else
            {
                switch (shapeState.Shape.Type)
                {
                    case Shape.ShapeType.Polygon:
                        polygonSettings.Visible = true;
                        break;

                    case Shape.ShapeType.Circle:
                        polygonSettings.Visible = false;
                        break;

                    case Shape.ShapeType.Edge:
                        polygonSettings.Visible = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Creates new shape based on chosen type from checkbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void addShapeButton_Click(object sender, EventArgs e)
        {
            // find chosen shape
            foreach (Shape.ShapeType shape in Enum.GetValues(typeof(Shape.ShapeType)))
            {
                if (selectShapeBox.Text == shape.ToString())
                {
                    // add new shape
                    ShapeState newItem = ShapesScreen.AddShape(shape);
                    if (newItem != null)
                    {
                        shapesList.Items.Add(newItem);
                        shapesList.SelectedIndex = shapesList.Items.Count - 1;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Handles the DrawItem event of the shapesList control.
        /// Draws item in ListBox. Special drawing (colored items).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        private void shapesList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;

            ShapeState shape = ((ListBox)sender).Items[e.Index] as ShapeState;
            Debug.Assert(shape != null, "Item is not an instance of TextureScreen.ShapeState");

            // draw background 
            if (e.Index == shapesList.SelectedIndex)
            {
                brush.Color = Color.White;
                e.Graphics.FillRectangle(brush, e.Bounds);
                brush.Color = Color.Black;
            }
            else
            {
                brush.Color = shape.Color;
                e.Graphics.FillRectangle(brush, e.Bounds);
                brush.Color = Color.White;
            }

            // draw text    
            e.Graphics.DrawString(shape.ShapeText(), ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the shapesList control.
        /// Changes the active shape of the <see cref="ShapesEditingScreen"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void shapesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (shapesList.SelectedIndex != -1)
            {
                ShapeState shapeState = shapesList.SelectedItem as ShapeState;
                Debug.Assert(shapeState != null, "Item is not an instance of TextureScreen.ShapeState");

                ShapesScreen.State = shapeState;
                shapesList.Invalidate();
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the shapesList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void shapesList_KeyDown(object sender, KeyEventArgs e)
        {
            // delete selected shape
            if (e.KeyCode == Keys.Delete && shapesList.SelectedIndex != -1)
            {
                ShapeState shapeToRemove = shapesList.SelectedItem as ShapeState;
                if (shapeToRemove != null)
                {
                    // remove shape
                    ShapesScreen.RemoveShape(shapeToRemove);
                    shapesList.Items.RemoveAt(shapesList.SelectedIndex);
                    // select last shape if any
                    if (shapesList.SelectedIndex == -1 && shapesList.Items.Count != 0)
                    {
                        shapesList.SelectedIndex = shapesList.Items.Count - 1;
                    }
                }
            }
        }

        /// <summary>
        /// Makes convex hull of the selected polygon if any.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void makeConvexHullButton_Click(object sender, EventArgs e)
        {
            PolygonEditState polygonState = ShapesScreen.State as PolygonEditState;
            if (polygonState != null)
            {
                polygonState.MakeConvexHull();
            }

            ShapesScreen.Invalidate();
        }

        /// <summary>
        /// Makes convex decomposition of the selected polygon if any.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void makeConvexDecompositionButton_Click(object sender, EventArgs e)
        {
            PolygonEditState polygonState = ShapesScreen.State as PolygonEditState;
            if (polygonState != null)
            {
                List<Polygon> newPolygons = polygonState.MakeConvexDecomposition();
                if (newPolygons != null && newPolygons.Count > 0)
                {
                    // remove actual polygon state
                    RemoveShape(polygonState);

                    // add new polygons
                    foreach (Polygon polygon in newPolygons)
                    {
                        shapesList.Items.Add(ShapesScreen.AddShape(polygon));
                    }

                    // select last added shape
                    shapesList.SelectedIndex = shapesList.Items.Count - 1;
                }
            }

            ShapesScreen.Invalidate();
        }

        /// <summary>
        /// Changes visibility of the convex hull. (Changed value of checkbox: Show Convex Hull.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showConvexHull_CheckedChanged(object sender, EventArgs e)
        {
            PolygonEditState.ShowConvexHull = showConvexHull.Checked;
            ShapesScreen.Invalidate();
            Properties.Settings.Default.TextureEditor_ShowConvexHull = showConvexHull.Checked;
        }

        /// <summary>
        /// Changes visibility of the convex decomposition. (Changed value of checkbox: Show Convex Decomposition.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showConvexDecomposition_CheckedChanged(object sender, EventArgs e)
        {
            PolygonEditState.ShowConvexDecomposition = showConvexDecomposition.Checked;
            ShapesScreen.Invalidate();
            Properties.Settings.Default.TextureEditor_ShowConvexDecomposition = showConvexDecomposition.Checked;
        }

        /// <summary>
        /// Changes visibility of the inactive shapes. (Changed value of checkbox: Show All Shapes.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showAllShapes_CheckedChanged(object sender, EventArgs e)
        {
            ShapesScreen.ShowAllShapes = showAllShapes.Checked;
            ShapesScreen.Invalidate();
            Properties.Settings.Default.TextureEditor_ShowAllShapes = showAllShapes.Checked;
        }

        /// <summary>
        /// Changes visibility of the grid. (Changed value of checkbox: Show Grid.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showGrid_CheckedChanged(object sender, EventArgs e)
        {
            ShapesScreen.ShowGrid = showGrid.Checked;
            ShapesScreen.Invalidate();
            Properties.Settings.Default.TextureEditor_ShowGrid = showGrid.Checked;
        }

        /// <summary>
        /// Changes visibility of the origin. (Changed value of checkbox: Show Origin.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showOriginButton_CheckedChanged(object sender, EventArgs e)
        {
            ShapesScreen.ShowOrigin = showOrigin.Checked;
            ShapesScreen.Invalidate();
            Properties.Settings.Default.TextureEditor_ShowOrigin = showOrigin.Checked;
        }
    }
}
