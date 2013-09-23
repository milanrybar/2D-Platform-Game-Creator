/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a script connection used at visual scripting.
    /// </summary>
    class ConnectionView : SceneNode
    {
        /// <summary>
        /// Gets the starting <see cref="IConnecting"/> instance of the connection.
        /// </summary>
        public IConnecting From
        {
            get { return _from; }
        }
        private IConnecting _from;

        /// <summary>
        /// Gets the ending <see cref="IConnecting"/> instance of the connection.
        /// </summary>
        public IConnecting To
        {
            get { return _to; }
        }
        private IConnecting _to;

        /// <summary>
        /// Gets or sets the location of the scene node at the scene. Should not be used.
        /// </summary>
        /// <exception cref="NotImplementedException">Should not be used.</exception>
        public override PointF Location
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the size and location of the scene node. Should not be used.
        /// </summary>
        /// <exception cref="NotImplementedException">Should not be used.</exception>
        public override RectangleF Bounds
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public override bool CanMove
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool CanConnect
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool CanClone
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool CanEditSettings
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool IsSocketsContainer
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override IConnecting IConnecting
        {
            get { return null; }
        }

        /// <inheritdoc />
        public override INodeSocketsContainer INodeSocketsContainer
        {
            get { return null; }
        }

        /// <inheritdoc />
        public override IEditSettings IEditSettings
        {
            get { return null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionView"/> class.
        /// </summary>
        /// <param name="from">The starting <see cref="IConnecting"/> instance of the connection.</param>
        /// <param name="to">The ending <see cref="IConnecting"/> instance of the connection.</param>
        public ConnectionView(IConnecting from, IConnecting to)
        {
            _from = from;
            _to = to;
        }

        /// <summary>
        /// Called when the scene node is added to the <see cref="ScriptingScreen"/>.
        /// Informs connector that the connection is added.
        /// </summary>
        public override void OnSceneNodeAdded()
        {
            Debug.Assert(!From.ContainsConnection(From, To), "Connection already exists between IConnecting ends.");
            Debug.Assert(!To.ContainsConnection(From, To), "Connection already exists between IConnecting ends.");

            From.Connections.Add(this);
            To.Connections.Add(this);

            From.OnConnectionAdded(To);
            To.OnConnectionAdded(From);
        }

        /// <summary>
        /// Called when the scene node is removed from the <see cref="ScriptingScreen"/>.
        /// Informs connector that the connection removed added.
        /// </summary>
        public override void OnSceneNodeRemoved()
        {
            Debug.Assert(From.ContainsConnection(From, To), "Connection does not exist between IConnecting ends.");
            Debug.Assert(To.ContainsConnection(From, To), "Connection does not exist between IConnecting ends.");

            From.Connections.Remove(this);
            To.Connections.Remove(this);

            From.OnConnectionRemoved(To);
            To.OnConnectionRemoved(From);
        }

        /// <inheritdoc />
        public override bool Contains(PointF point)
        {
            PointF vectorLine = To.Center.Sub(From.Center);

            float tX = (point.X - From.Center.X) / vectorLine.X;
            float tY = (point.Y - From.Center.Y) / vectorLine.Y;

            if (tX > 1f || tX < 0f || tY > 1f || tY < 0f) return false;

            float y = From.Center.Y + vectorLine.Y * tX;
            float x = From.Center.X + vectorLine.X * tY;

            if (Math.Abs(point.Y - y) <= 4f || Math.Abs(point.X - x) <= 4f) return true;

            return false;
        }

        /// <inheritdoc />
        public override void Paint(Graphics graphics)
        {
            if (SelectState == Scripting.SelectState.Default) DrawingTools.LinePen.Color = From.Color;
            else if (SelectState == Scripting.SelectState.Hover) DrawingTools.LinePen.Color = ColorSettings.Hover;
            else if (SelectState == Scripting.SelectState.Select) DrawingTools.LinePen.Color = ColorSettings.Select;
            graphics.DrawLine(DrawingTools.LinePen, From.Center, To.Center);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Cannot be cloned.
        /// </remarks>
        public override SceneNode Clone()
        {
            return null;
        }

        /// <inheritdoc />
        public override bool IntersectsWith(ref RectangleF rectangle)
        {
            return rectangle.Contains(From.Center) || rectangle.Contains(To.Center) ||
                LineSegmentIntersect(From.Center, To.Center, rectangle.Location, rectangle.Location.Add(new PointF(rectangle.Width, 0f))) ||
                LineSegmentIntersect(From.Center, To.Center, rectangle.Location.Add(new PointF(rectangle.Width, rectangle.Height)), rectangle.Location.Add(new PointF(rectangle.Width, 0f))) ||
                LineSegmentIntersect(From.Center, To.Center, rectangle.Location.Add(new PointF(rectangle.Width, rectangle.Height)), rectangle.Location.Add(new PointF(0f, rectangle.Height))) ||
                LineSegmentIntersect(From.Center, To.Center, rectangle.Location, rectangle.Location.Add(new PointF(0f, rectangle.Height)));
        }

        /// <summary>
        /// Determines whether two specified line segments intersect.
        /// </summary>
        /// <param name="point1">The first point of the first line segment.</param>
        /// <param name="point2">The second point of the first line segment.</param>
        /// <param name="point3">The first point of the second line segment.</param>
        /// <param name="point4">The second point of the second line segment.</param>
        /// <returns>Returns <c>true</c> if two specified line segments intersect; otherwise <c>false</c>.</returns>
        public static bool LineSegmentIntersect(PointF point1, PointF point2, PointF point3, PointF point4)
        {
            // these are reused later.
            // each lettered sub-calculation is used twice, except
            // for b and d, which are used 3 times
            float a = point4.Y - point3.Y;
            float b = point2.X - point1.X;
            float c = point4.X - point3.X;
            float d = point2.Y - point1.Y;

            // denominator to solution of linear system
            float denom = (a * b) - (c * d);

            // if denominator is 0, then lines are parallel
            if (!(denom >= -1.192092896e-07f && denom <= 1.192092896e-07f))
            {
                float e = point1.Y - point3.Y;
                float f = point1.X - point3.X;
                float oneOverDenom = 1.0f / denom;

                // numerator of first equation
                float ua = (c * e) - (a * f);
                ua *= oneOverDenom;

                // check if intersection point of the two lines is on line segment 1
                if (ua >= 0.0f && ua <= 1.0f)
                {
                    // numerator of second equation
                    float ub = (b * e) - (d * f);
                    ub *= oneOverDenom;

                    // check if intersection point of the two lines is on line segment 2
                    // means the line segments intersect, since we know it is on
                    // segment 1 as well.
                    if (ub >= 0.0f && ub <= 1.0f)
                    {
                        // check if they are coincident (no collision in this case)
                        if (ua != 0f || ub != 0f)
                        {
                            //There is an intersection
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
        }
    }
}
