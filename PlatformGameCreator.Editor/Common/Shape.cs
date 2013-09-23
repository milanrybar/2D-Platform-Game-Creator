/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace PlatformGameCreator.Editor.Common
{
    /// <summary>
    /// Base class for a shape.
    /// </summary>
    [Serializable]
    abstract class Shape : ISerializable
    {
        /// <summary>
        /// Represent type of shape.
        /// </summary>
        public enum ShapeType
        {
            /// <summary>
            /// Polygon shape. Only used with the <see cref="Polygon"/> class.
            /// </summary>
            Polygon,

            /// <summary>
            /// Circle shape. Only used with the <see cref="Circle"/> class.
            /// </summary>
            Circle,

            /// <summary>
            /// Edge shape. Only used with the <see cref="Edge"/> class.
            /// </summary>
            Edge
        };

        /// <summary>
        /// Gets the type of the shape.
        /// </summary>
        public abstract ShapeType Type { get; }

        /// <summary>
        /// Bounding rectangle of the shape.
        /// </summary>
        public AABB Rectangle;

        /// <summary>
        /// Moves the shape by the specified move vector.
        /// </summary>
        /// <param name="moveVector">The move vector to move the shape by.</param>
        public abstract void Move(Vector2 moveVector);

        /// <summary>
        /// Rotates the the shape by the specified angle around the specified point.
        /// </summary>
        /// <param name="angle">The angle to rotate the shape by.</param>
        /// <param name="origin">The origin to rotate the shape around.</param>
        public abstract void Rotate(float angle, Vector2 origin);

        /// <summary>
        /// Scales the the shape by the specified scale factor.
        /// </summary>
        /// <param name="scale">The scale factor to scale the shape by.</param>
        /// <param name="origin">The origin of the scaling.</param>
        public abstract void Scale(Vector2 scale, Vector2 origin);

        /// <summary>
        /// Updates the bounding rectangle of the shape.
        /// </summary>
        public abstract void UpdateAABB();

        /// <summary>
        /// Clones this shape.
        /// </summary>
        /// <returns>Cloned shape.</returns>
        public abstract Shape Clone();

        /// <summary>
        /// Determines whether this shape is valid.
        /// </summary>
        /// <returns><c>true</c> if this shape is valid; otherwise <c>false</c>.</returns>
        public abstract bool IsValid();

        /// <inheritdoc />
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    }

    /// <summary>
    /// Shape that is composed from vertices. Used by <see cref="Polygon"/> and <see cref="Edge"/>.
    /// </summary>
    [Serializable]
    abstract class ShapeBasedOnVertices : Shape
    {
        /// <summary>
        /// Vertices of the shape.
        /// </summary>
        public Vertices Vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeBasedOnVertices"/> class.
        /// </summary>
        public ShapeBasedOnVertices()
        {
            Vertices = new Vertices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeBasedOnVertices"/> class and sets the specified vertices.
        /// </summary>
        /// <param name="vertices">The vertices to set.</param>
        public ShapeBasedOnVertices(Vertices vertices)
        {
            Vertices = vertices;

            UpdateAABB();
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected ShapeBasedOnVertices(SerializationInfo info, StreamingContext ctxt)
        {
            Vertices = new Vertices((List<Vector2>)info.GetValue("Vertices", typeof(List<Vector2>)));

            UpdateAABB();
        }

        /// <summary>
        /// Applies the geometric transform represented by the specified matrix to the shape.
        /// </summary>
        /// <param name="transform">Matrix to transform the shape by.</param>
        protected void Transform(ref Matrix transform)
        {
            for (int i = 0; i < Vertices.Count; ++i)
            {
                Vertices[i] = Vector2.Transform(Vertices[i], transform);
            }
        }

        /// <inheritdoc />
        public override void Move(Vector2 moveVector)
        {
            for (int i = 0; i < Vertices.Count; ++i)
            {
                Vertices[i] += moveVector;
            }

            UpdateAABB();
        }

        /// <inheritdoc />
        public override void Rotate(float angle, Vector2 origin)
        {
            Matrix transform = Matrix.CreateTranslation(new Vector3(-origin, 0f))
                * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(new Vector3(origin, 0f));
            Transform(ref transform);
            UpdateAABB();
        }

        /// <inheritdoc />
        public override void Scale(Vector2 scale, Vector2 origin)
        {
            Matrix transform = Matrix.CreateTranslation(new Vector3(-origin, 0f)) *
                Matrix.CreateScale(scale.X, scale.Y, 1f) * Matrix.CreateTranslation(new Vector3(origin, 0f));
            Transform(ref transform);
            UpdateAABB();
        }

        /// <inheritdoc />
        public override void UpdateAABB()
        {
            if (Vertices.Count == 0) return;

            Vector2 lower = Vertices[0];
            Vector2 upper = lower;

            for (int i = 1; i < Vertices.Count; ++i)
            {
                Vector2 v = Vertices[i];
                lower = Vector2.Min(lower, v);
                upper = Vector2.Max(upper, v);
            }

            Rectangle.LowerBound = lower;
            Rectangle.UpperBound = upper;
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Vertices", new List<Vector2>(Vertices));
        }
    }

    /// <summary>
    /// Polygon shape.
    /// </summary>
    [Serializable]
    sealed class Polygon : ShapeBasedOnVertices
    {
        /// <inheritdoc />
        public override ShapeType Type
        {
            get { return ShapeType.Polygon; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        public Polygon()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class and sets the specified vertices.
        /// </summary>
        /// <param name="vertices">The vertices to set.</param>
        public Polygon(Vertices vertices)
            : base(vertices)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class and sets the vertices from the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon to get the vertices from.</param>
        public Polygon(Polygon polygon)
            : base(polygon.Vertices)
        {
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Polygon(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override Shape Clone()
        {
            return new Polygon(new Vertices(Vertices));
        }

        /// <summary>
        /// Creates a rectangle from the specifed left upper corner and size.
        /// </summary>
        /// <param name="position">The left upper corner of the rectangle</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns><see cref="Polygon"/> representing the specified rectangle.</returns>
        public static Polygon CreateAsRectangle(Vector2 position, float width, float height)
        {
            Vertices rectangleVertices = new Vertices();
            rectangleVertices.Add(position);
            rectangleVertices.Add(new Vector2(position.X + width, position.Y));
            rectangleVertices.Add(new Vector2(position.X + width, position.Y + height));
            rectangleVertices.Add(new Vector2(position.X, position.Y + height));

            return new Polygon(rectangleVertices);
        }

        /// <summary>
        /// Makes convex hull from this polygon.
        /// </summary>
        public void ConvexHull()
        {
            Vertices = FarseerPhysics.Common.ConvexHull.GiftWrap.GetConvexHull(Vertices);
        }

        /// <summary>
        /// Makes convex decomposition from this polygon and returns the result.
        /// </summary>
        /// <returns>Convex polygon from the convex decomposition.</returns>
        public List<Polygon> ConvexDecomposition()
        {
            if (Vertices.Count <= 3) return null;

            Debug.Assert(Vertices.IsSimple(), "Polygon must be simple (no crossing edges)");

            // compute convex decomposition
            List<Vertices> decomposition = FarseerPhysics.Common.Decomposition.BayazitDecomposer.ConvexPartition(new Vertices(Vertices));

            // create new polygons
            List<Polygon> newPolygons = new List<Polygon>(decomposition.Count);
            foreach (Vertices decompositionVertices in decomposition)
            {
                newPolygons.Add(new Polygon(decompositionVertices));
            }

            return newPolygons;
        }

        /// <inheritdoc />
        /// <summary>
        /// Determines whether this shape is valid.
        /// Checks if the polygon has at least 3 vertices and has no crossing edges. 
        /// </summary>
        public override bool IsValid()
        {
            if (Vertices.Count >= 3 && Vertices.IsSimple()) return true;
            else return false;
        }
    }

    /// <summary>
    /// Circle shape.
    /// </summary>
    [Serializable]
    sealed class Circle : Shape
    {
        /// <inheritdoc />
        public override ShapeType Type
        {
            get { return ShapeType.Circle; }
        }

        /// <summary>
        /// Origin of the circle.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// Radius of the circle.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> class.
        /// </summary>
        public Circle()
        {
            Origin = new Vector2();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> class and sets the specified origin and radius.
        /// </summary>
        /// <param name="origin">The origin of the circle to set.</param>
        /// <param name="radius">The radius of the circle to set.</param>
        public Circle(Vector2 origin, float radius)
        {
            Origin = origin;
            Radius = radius;

            UpdateAABB();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> class and sets origin and radius from the specified circle.
        /// </summary>
        /// <param name="circle">The circle to get origin and radius from.</param>
        public Circle(Circle circle)
        {
            Origin = circle.Origin;
            Radius = circle.Radius;

            UpdateAABB();
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Circle(SerializationInfo info, StreamingContext ctxt)
        {
            Origin = (Vector2)info.GetValue("Origin", typeof(Vector2));
            Radius = info.GetSingle("Radius");

            UpdateAABB();
        }

        /// <inheritdoc />
        public override void Rotate(float angle, Vector2 origin)
        {
            Matrix transform = Matrix.CreateTranslation(new Vector3(-origin, 0))
                * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(new Vector3(origin, 0));

            Origin = Vector2.Transform(Origin, transform);

            UpdateAABB();
        }

        /// <inheritdoc />
        /// <summary>
        /// Scales the the shape by the specified scale factor.
        /// Circle do not support non-uniform scaling (scaling to ellipse). Takes only X coordinate of the scale factor.
        /// </summary>
        public override void Scale(Vector2 scale, Vector2 origin)
        {
            Matrix transform = Matrix.CreateTranslation(new Vector3(-origin, 0f)) *
                Matrix.CreateScale(scale.X, scale.X, 1f) * Matrix.CreateTranslation(new Vector3(origin, 0f));
            Origin = Vector2.Transform(Origin, transform);

            Radius *= scale.X;

            UpdateAABB();
        }

        /// <inheritdoc />
        public override void UpdateAABB()
        {
            Rectangle.LowerBound = new Vector2(Origin.X - Radius, Origin.Y - Radius);
            Rectangle.UpperBound = new Vector2(Origin.X + Radius, Origin.Y + Radius);
        }

        /// <inheritdoc />
        public override Shape Clone()
        {
            return new Circle(Origin, Radius);
        }

        /// <inheritdoc />
        public override void Move(Vector2 moveVector)
        {
            Origin += moveVector;

            UpdateAABB();
        }

        /// <inheritdoc />
        /// <summary>
        /// Determines whether this shape is valid.
        /// Checks if the radius is not too small.
        /// </summary>
        public override bool IsValid()
        {
            if (Radius >= 1f) return true;
            else return false;
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Origin", Origin);
            info.AddValue("Radius", Radius);
        }
    }

    /// <summary>
    /// Edge shape.
    /// </summary>
    [Serializable]
    sealed class Edge : ShapeBasedOnVertices
    {
        /// <inheritdoc />
        public override ShapeType Type
        {
            get { return ShapeType.Edge; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        public Edge()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class and sets the specified vertices.
        /// </summary>
        /// <param name="vertices">The vertices to set.</param>
        public Edge(Vertices vertices)
            : base(vertices)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class and sets the vertices from the specified edge.
        /// </summary>
        /// <param name="edge">The edge to get the vertices from.</param>
        public Edge(Edge edge)
            : base(edge.Vertices)
        {
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Edge(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override Shape Clone()
        {
            return new Edge(new Vertices(Vertices));
        }

        /// <inheritdoc />
        /// <summary>
        /// Determines whether this shape is valid.
        /// Checks if the edge has at least two vertices.
        /// </summary>
        public override bool IsValid()
        {
            if (Vertices.Count >= 2) return true;
            else return false;
        }
    }
}
