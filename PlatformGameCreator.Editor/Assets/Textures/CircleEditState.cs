/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// State for <see cref="Circle"/> editing.
    /// </summary>
    /// <remarks>
    /// Circle editing:
    /// <list type="bullet">
    /// <item><description>Left Mouse - Choose center.</description></item>
    /// <item><description>Right Mouse - Set radius.</description></item>
    /// </list>
    /// </remarks>
    class CircleEditState : ShapeState
    {
        /// <summary>
        /// Gets the underlying circle shape.
        /// </summary>
        public Circle Circle
        {
            get { return _circle; }
        }
        private Circle _circle;

        /// <summary>
        /// Gets the underlying circle shape.
        /// </summary>
        public override Shape Shape
        {
            get { return _circle; }
        }

        /// <summary>
        /// Origin of the circle at the screen.
        /// </summary>
        private PointF origin;

        /// <summary>
        /// Default status text.
        /// </summary>
        private string defaultStatusText = "Circle editing. Left Mouse - Choose center. Right Mouse - Set radius.";

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleEditState"/> class.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="shapesEditingScreen"><see cref="ShapesEditingScreen"/> where the state is used.</param>
        public CircleEditState(Circle circle, ShapesEditingScreen shapesEditingScreen)
        {
            _circle = circle;
            Parent = shapesEditingScreen;

            UpdateOrigin();
        }

        /// <summary>
        /// Shows information about editing this shape.
        /// </summary>
        public override void OnSet()
        {
            Messages.ShowInfo(defaultStatusText);
        }

        /// <summary>
        /// Updates the origin of the circle.
        /// </summary>
        private void UpdateOrigin()
        {
            origin = new PointF(Circle.Origin.X, Circle.Origin.Y);
        }

        /// <inheritdoc/>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            // If left mouse button is pressed and no action is in progress
            // we will set the origin of the circle by currect position under the mouse cursor
            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                // add new circle origin
                Circle.Origin = Parent.MouseScreenPosition.ToVector2();
                UpdateOrigin();

                Parent.Invalidate();
                Messages.ShowInfo(defaultStatusText);
            }
            // If right mouse button is pressed and no action is in progress
            // we will set the radius of the circle by distance from the origin to currect position under the mouse cursor
            else if (e.Button == MouseButtons.Right && AcceptRightMouseButton && !ActionInProgress)
            {
                // change circle radius
                Circle.Radius = (Parent.MouseScreenPosition.ToVector2() - Circle.Origin).Length();

                Parent.Invalidate();
                Messages.ShowInfo(defaultStatusText);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the editing of the circle.
        /// </summary>
        public override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw vertex
            DrawingTools.SolidBrush.Color = ColorSettings.CircleOriginVertexColor;
            PaintVertex(origin, pe.Graphics);

            // draw circle
            DrawingTools.ShapeLinePen.Color = ColorSettings.ShapeLineColor;
            pe.Graphics.DrawEllipse(DrawingTools.ShapeLinePen, origin.X - Circle.Radius, origin.Y - Circle.Radius, Circle.Radius * 2f, Circle.Radius * 2f);
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the circle.
        /// </summary>
        public override void PaintShape(Graphics graphics)
        {
            // draw circle
            DrawingTools.LinePen.Color = Color;
            graphics.DrawEllipse(DrawingTools.LinePen, origin.X - Circle.Radius, origin.Y - Circle.Radius, Circle.Radius * 2f, Circle.Radius * 2f);
        }

        /// <inheritdoc />
        public override string ShapeText()
        {
            return String.Format("Circle - {0} radius", Circle.Radius);
        }

        /// <inheritdoc />
        public override void MoveShape(Microsoft.Xna.Framework.Vector2 move)
        {
            Circle.Origin += move;
            UpdateOrigin();
        }

        /// <inheritdoc />
        public override bool IsShapeValid()
        {
            return Circle.IsValid();
        }

        /// <inheritdoc />
        public override void OnInvalidShape()
        {
            Messages.ShowWarning("Invalid circle. Circle has small radius. Left Mouse - Choose center. Right Mouse - Set radius.");
        }
    }
}
