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
using System.Diagnostics;

namespace PlatformGameCreator.Editor.GameObjects.Paths
{
    /// <summary>
    /// Scene state that provides editing the path at the <see cref="SceneScreen"/> control.
    /// </summary>
    /// <remarks>
    /// Left Mouse - Add path vertex. Delete - Delete selected path vertex. Esc/Enter - Exit editing.
    /// </remarks>
    class EditingPathSceneState : GlobalBehaviourSceneState
    {
        /// <inheritdoc />
        public override bool CanBeInterrupted
        {
            get { return Path.Vertices.Count >= 2; }
        }

        /// <inheritdoc />
        public override bool CanBeInStack
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the underlying <see cref="Paths.Path"/>.
        /// </summary>
        public Path Path
        {
            get { return pathView.Path; }
        }

        /// <summary>
        /// Previous state of the <see cref="SceneScreen"/> control. 
        /// </summary>
        private SceneState lastState;

        /// <summary>
        /// Gets the underlying <see cref="Paths.PathView"/>.
        /// </summary>
        private PathView pathView;

        private int hoveredVertex = -1;
        private const float vertexRadius = 5f;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditingPathSceneState"/> class.
        /// </summary>
        /// <param name="pathView">The <see cref="Paths.PathView"/> to edit.</param>
        /// <param name="lastState">Previous state of the <see cref="SceneScreen"/> control. </param>
        public EditingPathSceneState(PathView pathView, SceneState lastState)
        {
            Debug.Assert(lastState.CanBeInStack, "Invalid use.");

            this.lastState = lastState;
            this.pathView = pathView;
        }

        /// <inheritdoc />
        /// <summary>
        /// Hovers a vertex under the mouse cursor, if any.
        /// </summary>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            hoveredVertex = IndexOf(Screen.MouseScenePosition);
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is up and no action is in progress 
        /// we will add new vertex that is under the mouse cursor to the path.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                pathView.Path.Vertices.Add(Screen.MouseScenePosition);
                pathView.Invalidate();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Delete - Delete selected path vertex. 
        /// Esc/Enter - Exit editing.
        /// </summary>
        public override void KeyDown(object sender, KeyEventArgs e)
        {
            base.KeyDown(sender, e);

            // exit editing path
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                if (Path.Vertices.Count >= 2)
                {
                    Screen.State = lastState;
                }
                else
                {
                    OnTryInterrupt();
                }
            }

            // delete hovered vertex
            else if (e.KeyCode == Keys.Delete && hoveredVertex != -1)
            {
                Path.Vertices.RemoveAt(hoveredVertex);
                pathView.Invalidate();
                hoveredVertex = -1;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Draws the vertex circles.
        /// </summary>
        public override void Draw(SceneBatch sceneBatch)
        {
            base.Draw(sceneBatch);

            for (int i = 0; i < Path.Vertices.Count; ++i)
            {
                Color color = i == hoveredVertex ? Color.Yellow : i == Path.Vertices.Count - 1 ? Color.Blue : Color.Red;
                PlatformGameCreator.Editor.Xna.RenderCircle.DrawCircle(pathView.Path.Vertices[i], vertexRadius * sceneBatch.InversScale, color, sceneBatch);
            }
        }

        /// <summary>
        /// Gets index of vertex at the specified point.
        /// </summary>
        /// <param name="point">Point where to find vertex.</param>
        /// <returns>Returns index of vertex or -1 when no vertex is found.</returns>
        private int IndexOf(Vector2 point)
        {
            for (int i = 0; i < Path.Vertices.Count; ++i)
            {
                float squareDistance = (Path.Vertices[i].X - point.X) * (Path.Vertices[i].X - point.X) + (Path.Vertices[i].Y - point.Y) * (Path.Vertices[i].Y - point.Y);
                if (squareDistance <= vertexRadius * Screen.ScaleInversFactor * vertexRadius * Screen.ScaleInversFactor) return i;
            }

            return -1;
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Editing path. Left Mouse - Add path vertex. Delete - Delete selected path vertex. Esc/Enter - Exit editing.");
        }

        /// <inheritdoc />
        public override void OnTryInterrupt()
        {
            Messages.ShowWarning("Invalid path. Path must have at least 2 vertices. Left Mouse - Add path vertex. Delete - Delete selected path vertex.");
        }
    }
}
