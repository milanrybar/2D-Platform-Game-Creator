/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// State for <see cref="Edge"/> editing.
    /// </summary>
    /// <remarks>
    /// Edge editing:
    /// <list type="bullet">
    /// <item><description>Left Mouse - New vertex.</description></item>
    /// <item><description>Right Mouse / Delete - Delete selected vertex.</description></item>
    /// </list>
    /// </remarks>
    class EdgeEditState : ShapeState
    {
        /// <summary>
        /// Gets the underlying edge shape.
        /// </summary>
        public Edge Edge
        {
            get { return _edge; }
        }
        private Edge _edge;

        /// <summary>
        /// Gets the underlying edge shape.
        /// </summary>
        public override Shape Shape
        {
            get { return _edge; }
        }

        /// <summary>
        /// Vertices of the edge.
        /// </summary>
        private List<PointF> vertices = new List<PointF>();

        // lines for drawing the edge
        private PointF[] lines = new PointF[0];

        /// <summary>
        /// Selected vertex of the edge.
        /// </summary>
        private int selectedVertexIndex = -1;

        /// <summary>
        /// Default status text.
        /// </summary>
        private string defaultStatusText = "Edge editing. Left Mouse - New vertex. Right Mouse / Delete - Delete selected vertex.";

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEditState"/> class.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="shapesEditingScreen"><see cref="ShapesEditingScreen"/> where the state is used.</param>
        public EdgeEditState(Edge edge, ShapesEditingScreen shapesEditingScreen)
        {
            _edge = edge;
            Parent = shapesEditingScreen;

            UpdateVertices();
        }

        /// <summary>
        /// Shows information about editing this shape.
        /// </summary>
        public override void OnSet()
        {
            Messages.ShowInfo(defaultStatusText);
        }

        /// <inheritdoc/>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            if (e.Button == MouseButtons.Left && selectedVertexIndex == -1 && !ActionInProgress)
            {
                // add new vertex
                Edge.Vertices.Add(Parent.MouseScreenPosition.ToVector2());
                vertices.Add(Parent.MouseScreenPosition);

                UpdateLines();

                Parent.Invalidate();
                Messages.ShowInfo(defaultStatusText);
            }
            else if (e.Button == MouseButtons.Right && selectedVertexIndex != -1 && AcceptRightMouseButton && !ActionInProgress)
            {
                RemoveSelectedVertex();
            }
        }

        /// <summary>
        /// Removes the selected vertex from the edge.
        /// </summary>
        private void RemoveSelectedVertex()
        {
            // remove selected vertex
            Edge.Vertices.RemoveAt(selectedVertexIndex);
            vertices.RemoveAt(selectedVertexIndex);

            selectedVertexIndex = -1;

            UpdateLines();

            Parent.Invalidate();
            Messages.ShowInfo(defaultStatusText);
        }

        /// <inheritdoc/>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            if (!ActionInProgress)
            {
                int index = IndexOf(Parent.MouseScreenPosition);

                // new selected vertex?
                if (index != selectedVertexIndex)
                {
                    selectedVertexIndex = index;

                    Parent.Invalidate();
                }
            }
        }

        /// <inheritdoc/>
        public override void KeyDown(object sender, KeyEventArgs e)
        {
            base.KeyDown(sender, e);

            if (e.KeyCode == Keys.Delete && selectedVertexIndex != -1 && !ActionInProgress)
            {
                RemoveSelectedVertex();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the editing of the edge.
        /// </summary>
        public override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw lines
            if (vertices.Count >= 2)
            {
                DrawingTools.ShapeLinePen.Color = ColorSettings.ShapeLineColor;
                pe.Graphics.DrawLines(DrawingTools.ShapeLinePen, lines);
            }

            // draw vertices
            for (int i = 0; i < vertices.Count; ++i)
            {
                // set vertex color:
                // selected vertex
                if (i == selectedVertexIndex) DrawingTools.SolidBrush.Color = ColorSettings.ShapeVertexSelected;
                // vertex that will connect new vertex (last vertex)
                else if (i == vertices.Count - 1) DrawingTools.SolidBrush.Color = ColorSettings.ShapeVertexLast;
                // no special vertex
                else DrawingTools.SolidBrush.Color = ColorSettings.EdgeVertex;

                // draw vertex
                PaintVertex(vertices[i], pe.Graphics);
            }
        }

        /// <summary>
        ///  Gets index of vertex of the edge at the specified point.
        /// </summary>
        /// <param name="point">Point to find vertex.</param>
        /// <returns>Returns index of vertex or -1 when no vertex is found.</returns>
        private int IndexOf(PointF point)
        {
            for (int i = 0; i < vertices.Count; ++i)
            {
                float squareDistance = (vertices[i].X - point.X) * (vertices[i].X - point.X) + (vertices[i].Y - point.Y) * (vertices[i].Y - point.Y);
                if (squareDistance <= Parent.VertexRadius * Parent.VertexRadius) return i;
            }

            return -1;
        }

        /// <summary>
        /// Update vertices. (Update all data of the edge.)
        /// </summary>
        private void UpdateVertices()
        {
            vertices.Clear();
            lines = new PointF[Edge.Vertices.Count];

            for (int i = 0; i < Edge.Vertices.Count; ++i)
            {
                vertices.Add(new PointF(Edge.Vertices[i].X, Edge.Vertices[i].Y));
                lines[i] = vertices[i];
            }
        }

        /// <summary>
        /// Updates lines for drawing the edge.
        /// </summary>
        private void UpdateLines()
        {
            lines = new PointF[vertices.Count];

            for (int i = 0; i < vertices.Count; ++i)
            {
                lines[i] = vertices[i];
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the edge.
        /// </summary>
        public override void PaintShape(Graphics graphics)
        {
            // draw lines
            DrawingTools.LinePen.Color = Color;
            if (lines.Length > 2) graphics.DrawLines(DrawingTools.LinePen, lines);
        }

        /// <inheritdoc />
        public override string ShapeText()
        {
            return String.Format("Edge - {0} vertices", vertices.Count);
        }

        /// <inheritdoc />
        public override void MoveShape(Microsoft.Xna.Framework.Vector2 move)
        {
            for (int i = 0; i < Edge.Vertices.Count; ++i)
            {
                Edge.Vertices[i] += move;
            }

            UpdateVertices();
        }

        /// <inheritdoc />
        public override bool IsShapeValid()
        {
            return Edge.IsValid();
        }

        /// <inheritdoc />
        public override void OnInvalidShape()
        {
            Messages.ShowWarning("Invalid edge. Edge must have at least 2 vertices. Left Mouse - New vertex.");
        }
    }
}
