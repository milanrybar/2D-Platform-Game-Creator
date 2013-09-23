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
using FarseerPhysics.Common;
using FarseerPhysics.Common.ConvexHull;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using PlatformGameCreator.Editor.Common;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// State for <see cref="Polygon"/> editing.
    /// </summary>
    /// <remarks>
    /// Polygon editing:
    /// <list type="bullet">
    /// <item><description>Left Mouse - New vertex.</description></item>
    /// <item><description>Right Mouse / Delete - Delete selected vertex.</description></item>
    /// </list>
    /// </remarks>
    class PolygonEditState : ShapeState
    {
        /// <summary>
        /// Gets the underlying polygon shape.
        /// </summary>
        public Polygon Polygon
        {
            get { return _polygon; }
        }
        private Polygon _polygon;

        /// <summary>
        /// Gets the underlying polygon shape.
        /// </summary>
        public override Shape Shape
        {
            get { return _polygon; }
        }

        /// <summary>
        /// Indicates whether the convex hull of the polygon is visible.
        /// </summary>
        public static bool ShowConvexHull = true;

        /// <summary>
        /// Indicates whether the convex decomposition of the polygon is visible.
        /// </summary>
        public static bool ShowConvexDecomposition = true;

        /// <summary>
        /// Vertices of the polygon. Pair of ( vertex, is valid? (in convex hull) ).
        /// </summary>
        private List<KeyValuePair<PointF, bool>> vertices = new List<KeyValuePair<PointF, bool>>();

        // lines for drawing the polygon
        private PointF[] lines = new PointF[0];
        private PointF[] convexHullLines = new PointF[0];
        private List<PointF[]> convexDecompositionLines = new List<PointF[]>();

        /// <summary>
        /// Selected vertex of the polygon.
        /// </summary>
        private int selectedVertexIndex = -1;

        /// <summary>
        /// Default status text.
        /// </summary>
        private string defaultStatusText = "Polygon editing. Left Mouse - New vertex. Right Mouse / Delete - Delete selected vertex.";

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonEditState"/> class.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="shapesEditingScreen"><see cref="ShapesEditingScreen"/> where the state is used.</param>
        public PolygonEditState(Polygon polygon, ShapesEditingScreen shapesEditingScreen)
        {
            _polygon = polygon;
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
                Polygon.Vertices.Add(Parent.MouseScreenPosition.ToVector2());

                // is polygon simple? (no crossing edges)
                if (Polygon.Vertices.Count <= 3 || Polygon.Vertices.IsSimple())
                {
                    vertices.Add(new KeyValuePair<PointF, bool>(Parent.MouseScreenPosition, true));

                    // update data
                    UpdatePolygonLines();
                    UpdateConvexHull();
                    UpdateConvexDecomposition();

                    Parent.Invalidate();
                    Messages.ShowInfo(defaultStatusText);
                }
                else
                {
                    // polygon has crossing edges => remove added vertex
                    Polygon.Vertices.RemoveAt(Polygon.Vertices.Count - 1);

                    Messages.ShowWarning("Unable to add vertex. Polygon would contain crossing edges.");
                }
            }
            else if (e.Button == MouseButtons.Right && selectedVertexIndex != -1 && AcceptRightMouseButton && !ActionInProgress)
            {
                RemoveSelectedVertex();
            }
        }

        /// <summary>
        /// Removes the selected vertex from the polygon.
        /// </summary>
        private void RemoveSelectedVertex()
        {
            // remove selected vertex
            Microsoft.Xna.Framework.Vector2 vertexToRemove = Polygon.Vertices[selectedVertexIndex];
            Polygon.Vertices.RemoveAt(selectedVertexIndex);

            // is polygon simple? (no crossing edges)
            if (Polygon.Vertices.Count <= 3 || Polygon.Vertices.IsSimple())
            {
                vertices.RemoveAt(selectedVertexIndex);

                // no vertex selected
                selectedVertexIndex = -1;

                // update data
                UpdatePolygonLines();
                UpdateConvexHull();
                UpdateConvexDecomposition();

                Parent.Invalidate();
                Messages.ShowInfo(defaultStatusText);
            }
            else
            {
                // polygon has crossing edges => add back removed vertex
                Polygon.Vertices.Insert(selectedVertexIndex, vertexToRemove);

                Messages.ShowWarning("Unable to delete vertex. Polygon would contain crossing edges.");
            }
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
        /// Paints the editing of the polygon.
        /// </summary>
        public override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            // draw convex decomposition
            if (ShowConvexDecomposition && vertices.Count > 3 && convexDecompositionLines.Count > 0)
            {
                DrawingTools.ShapeLinePen.Color = ColorSettings.PolygonConvexDecompositionLineColor;
                for (int i = 0; i < convexDecompositionLines.Count; ++i)
                {
                    pe.Graphics.DrawPolygon(DrawingTools.ShapeLinePen, convexDecompositionLines[i]);
                }
            }

            // draw polygon
            if (vertices.Count >= 2)
            {
                DrawingTools.ShapeLinePen.Color = ColorSettings.ShapeLineColor;
                pe.Graphics.DrawPolygon(DrawingTools.ShapeLinePen, lines);
            }

            // draw convex hull
            if (ShowConvexHull && vertices.Count >= 2)
            {
                DrawingTools.ShapeLinePen.Color = ColorSettings.PolygonConvexHullLineColor;
                pe.Graphics.DrawPolygon(DrawingTools.ShapeLinePen, convexHullLines);
            }

            // draw vertices
            for (int i = 0; i < vertices.Count; ++i)
            {
                // set vertex color:
                // selected vertex
                if (i == selectedVertexIndex) DrawingTools.SolidBrush.Color = ColorSettings.ShapeVertexSelected;
                // vertices that will connect new vertex
                else if (i == vertices.Count - 1 || i == 0) DrawingTools.SolidBrush.Color = ColorSettings.ShapeVertexLast;
                // valid vertex of convex hull is hidden
                else if (!ShowConvexHull || vertices[i].Value) DrawingTools.SolidBrush.Color = ColorSettings.PolygonVertexValid;
                // invalid vertex (in convex hull)
                else DrawingTools.SolidBrush.Color = ColorSettings.PolygonVertexInvalid;

                // draw vertex
                PaintVertex(vertices[i].Key, pe.Graphics);
            }
        }

        /// <summary>
        /// Gets index of vertex of the polygon at the specified point.
        /// </summary>
        /// <param name="point">Point where to find vertex.</param>
        /// <returns>Returns index of vertex or -1 when no vertex is found.</returns>
        private int IndexOf(PointF point)
        {
            for (int i = 0; i < vertices.Count; ++i)
            {
                float squareDistance = (vertices[i].Key.X - point.X) * (vertices[i].Key.X - point.X) + (vertices[i].Key.Y - point.Y) * (vertices[i].Key.Y - point.Y);
                if (squareDistance <= Parent.VertexRadius * Parent.VertexRadius) return i;
            }

            return -1;
        }

        /// <summary>
        /// Updates vertices. (Updates all data of the polygon - convex hull and convex decomposition.)
        /// </summary>
        private void UpdateVertices()
        {
            vertices.Clear();
            lines = new PointF[Polygon.Vertices.Count];

            // update vertices
            for (int i = 0; i < Polygon.Vertices.Count; ++i)
            {
                vertices.Add(new KeyValuePair<PointF, bool>(new PointF(Polygon.Vertices[i].X, Polygon.Vertices[i].Y), true));
                lines[i] = new PointF(vertices[i].Key.X, vertices[i].Key.Y);
            }

            // update other data
            UpdateConvexHull();
            UpdateConvexDecomposition();
        }

        /// <summary>
        /// Updates lines for drawing the polygon.
        /// </summary>
        private void UpdatePolygonLines()
        {
            lines = new PointF[vertices.Count];

            for (int i = 0; i < vertices.Count; ++i)
            {
                lines[i] = new PointF(vertices[i].Key.X, vertices[i].Key.Y);
            }
        }

        /// <summary>
        /// Updates data for the convex hull of the polygon.
        /// </summary>
        private void UpdateConvexHull()
        {
            // compute convex hull
            Vertices hull = GiftWrap.GetConvexHull(Polygon.Vertices);

            // update polygon lines
            convexHullLines = new PointF[hull.Count];
            for (int i = 0; i < hull.Count; ++i)
            {
                convexHullLines[i] = new PointF(hull[i].X, hull[i].Y);
            }

            // update valid/invalid vertices
            for (int i = 0; i < vertices.Count; ++i)
            {
                int index = hull.IndexOf(Polygon.Vertices[i]);
                if (index == -1) vertices[i] = new KeyValuePair<PointF, bool>(vertices[i].Key, false);
                else vertices[i] = new KeyValuePair<PointF, bool>(vertices[i].Key, true);
            }
        }

        /// <summary>
        /// Makes convex hull from the polygon.
        /// </summary>
        public void MakeConvexHull()
        {
            // compute convex hull
            Vertices hull = GiftWrap.GetConvexHull(Polygon.Vertices);

            // remove all unused vertices from convex hull
            for (int i = 0; i < Polygon.Vertices.Count; ++i)
            {
                int index = hull.IndexOf(Polygon.Vertices[i]);
                if (index == -1)
                {
                    Polygon.Vertices.RemoveAt(i);
                    --i;
                }
            }

            // update data
            UpdateVertices();
        }

        /// <summary>
        /// Updates data for convex decomposition of the polygon.
        /// </summary>
        private void UpdateConvexDecomposition()
        {
            if (vertices.Count <= 3) return;

            Debug.Assert(Polygon.Vertices.IsSimple(), "Polygon must be simple (no crossing edges)");

            // compute convex decomposition
            List<Vertices> decomposition = BayazitDecomposer.ConvexPartition(new Vertices(Polygon.Vertices));

            // update convex decomposition lines
            convexDecompositionLines.Clear();
            foreach (Vertices decompositionVertices in decomposition)
            {
                PointF[] convexLines = new PointF[decompositionVertices.Count];
                for (int i = 0; i < decompositionVertices.Count; ++i)
                {
                    convexLines[i] = new PointF(decompositionVertices[i].X, decompositionVertices[i].Y);
                }
                convexDecompositionLines.Add(convexLines);
            }
        }

        /// <summary>
        /// Makes convex decomposition from the polygon.
        /// </summary>
        /// <returns>Convex polygons from convex decomposition.</returns>
        public List<Polygon> MakeConvexDecomposition()
        {
            return Polygon.ConvexDecomposition();
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the polygon.
        /// </summary>
        public override void PaintShape(Graphics graphics)
        {
            // draw polygon
            DrawingTools.LinePen.Color = Color;
            if (lines.Length > 2) graphics.DrawPolygon(DrawingTools.LinePen, lines);
        }

        /// <inheritdoc />
        public override string ShapeText()
        {
            return String.Format("Polygon - {0} vertices", vertices.Count);
        }

        /// <inheritdoc />
        public override void MoveShape(Microsoft.Xna.Framework.Vector2 move)
        {
            for (int i = 0; i < Polygon.Vertices.Count; ++i)
            {
                Polygon.Vertices[i] += move;
            }

            UpdateVertices();
        }

        /// <inheritdoc />
        public override bool IsShapeValid()
        {
            return Polygon.IsValid();
        }

        /// <inheritdoc />
        public override void OnInvalidShape()
        {
            Messages.ShowWarning("Invalid polygon. Polygon must have at least 3 vertices and no crossing edges. Left Mouse - New vertex.");
        }
    }
}
