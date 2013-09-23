/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scenes;
using System.Windows.Forms;
using FarseerPhysics.Collision;

namespace PlatformGameCreator.Editor.GameObjects.Paths
{
    /// <summary>
    /// Represents <see cref="Paths.Path"/> at the scene (at <see cref="SceneScreen"/> control).
    /// </summary>
    class PathView : SceneNode
    {
        /// <summary>
        /// Gets the underlying <see cref="Paths.Path"/>.
        /// </summary>
        public Path Path
        {
            get { return _path; }
        }
        private Path _path;

        /// <summary>
        /// Gets the underlying <see cref="Paths.Path"/>.
        /// </summary>
        public override GameObject Tag
        {
            get { return _path; }
        }

        /// <inheritdoc />
        public override bool CanMove
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanRotate
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanScale
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the position of the path at the scene.
        /// </summary>
        public override Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle in radians of the path.
        /// </summary>
        public override float Angle { get; set; }

        /// <summary>
        /// Gets or sets the scale factor of the path.
        /// </summary>
        public override Vector2 ScaleFactor { get; set; }

        /// <inheritdoc />
        public override bool Visible
        {
            get { return Path.Visible; }
        }

        /// <inheritdoc />
        public override int Index
        {
            get { return Path.Index; }
        }

        /// <summary>
        /// Shape that represents the path.
        /// </summary>
        private ShapeBasedOnVertices pathShape;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathView"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public PathView(Path path)
        {
            _path = path;

            Path.LoopChanged += new EventHandler(Path_LoopChanged);

            Invalidate();
        }

        /// <summary>
        /// Handles the <see cref="Paths.Path.LoopChanged"/> event of the <see cref="Path"/>.
        /// Updates the shape that represents the path.
        /// </summary>
        private void Path_LoopChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Draws the path and its bounding rectangle.
        /// </remarks>
        public override void Draw(SceneBatch sceneBatch)
        {
            sceneBatch.Draw(Rectangle, ColorSettings.ForBounds(SceneSelect), 0f);

            sceneBatch.Draw(pathShape, ColorSettings.ForShape(SceneSelect), 0f);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Nothing to draw.
        /// </remarks>
        public override void DrawShapes(SceneBatch sceneBatch)
        {

        }

        /// <summary>
        /// Updates the shape that represents the path.
        /// </summary>
        public void Invalidate()
        {
            if (Path.Loop) pathShape = new Polygon(new FarseerPhysics.Common.Vertices(Path.Vertices));
            else pathShape = new Edge(new FarseerPhysics.Common.Vertices(Path.Vertices));

            if (Path.Vertices.Count != 0) Position = Path.Vertices[0];

            UpdateRectangle();
        }

        /// <inheritdoc />
        public override void Move(Vector2 move)
        {
            // new position
            Position += move;

            // move shape
            pathShape.Move(move);

            // update AABB rectangle
            Rectangle = new AABB(Rectangle.LowerBound + move, Rectangle.UpperBound + move);

            UpdatePath();
        }

        /// <inheritdoc />
        public override void Rotate(float angle)
        {
            // rotate shape
            pathShape.Rotate(angle, Position);

            // update AABB rectangle
            UpdateRectangle();

            UpdatePath();
        }

        /// <inheritdoc />
        public override void Scale(Vector2 scale)
        {
            // scale shape
            pathShape.Scale(scale, Position);

            // update AABB rectangle
            UpdateRectangle();

            UpdatePath();
        }

        /// <summary>
        /// Updates the <see cref="SceneNode.Rectangle"/>.
        /// </summary>
        protected virtual void UpdateRectangle()
        {
            // update AABB rectangle
            Rectangle = pathShape.Rectangle;
        }

        /// <summary>
        /// Updates the vertices of the underlying <see cref="Path"/>.
        /// </summary>
        private void UpdatePath()
        {
            Path.Vertices.Clear();
            Path.Vertices.AddRange(pathShape.Vertices);
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Path.LoopChanged -= new EventHandler(Path_LoopChanged);
                _path = null;
            }

            base.Dispose(disposing);
        }
    }
}
