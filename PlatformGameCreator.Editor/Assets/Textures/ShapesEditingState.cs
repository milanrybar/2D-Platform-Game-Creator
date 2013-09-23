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
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Interface containing drawing tools for <see cref="ShapesEditingScreen"/> and its <see cref="ShapesEditingState"/>.
    /// </summary>
    interface IScreenDrawingTools
    {
        /// <summary>
        /// Gets the solid brush.
        /// </summary>
        SolidBrush SolidBrush { get; }

        /// <summary>
        /// Gets the pen for drawing line.
        /// </summary>
        Pen LinePen { get; }

        /// <summary>
        /// Gets the pen for drawing line of a shape.
        /// </summary>
        Pen ShapeLinePen { get; }
    }

    /// <summary>
    /// Represents state of the <see cref="ShapesEditingScreen"/> control.
    /// </summary>
    abstract class ShapesEditingState
    {
        /// <summary>
        /// Gets or sets the <see cref="ShapesEditingScreen"/> where the state is used.
        /// </summary>
        public ShapesEditingScreen Parent { get; set; }

        /// <summary>
        /// Gets or sets drawing tools for the state.
        /// </summary>
        public static IScreenDrawingTools DrawingTools { get; set; }

        /// <summary>
        /// Handles the MouseMove event of the <see cref="ShapesEditingScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseMove(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the MouseUp event of the <see cref="ShapesEditingScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseUp(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the MouseDown event of the <see cref="ShapesEditingScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseDown(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the KeyDown event of the <see cref="ShapesEditingScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        public virtual void KeyDown(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Called when the <see cref="ShapesEditingScreen"/> is painted. Override this method with state-specific painting code.
        /// </summary>
        /// <param name="pe">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        public virtual void OnPaint(PaintEventArgs pe) { }

        /// <summary>
        /// Called when this state is set as the state of the <see cref="ShapesEditingScreen"/>. Override this method with state-specific behaviour.
        /// </summary>
        public virtual void OnSet() { }
    }

    /// <summary>
    /// Default state of the <see cref="ShapesEditingScreen"/> control. It has behaviour that all states should contain.
    /// </summary>
    /// <remarks>
    /// Changes the position at the screen by right mouse button.
    /// </remarks>
    class GlobalScreenState : ShapesEditingState
    {
        /// <summary>
        /// Gets or sets a value indicating whether any action is in progress.
        /// </summary>
        public bool ActionInProgress { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether an action of moving screen is in progress.
        /// </summary>
        public bool MovingScreen
        {
            get { return _movingScreen; }
        }
        private bool _movingScreen;

        /// <summary>
        /// Gets a value indicating whether this state should accept right mouse button.
        /// </summary>
        /// <remarks>
        /// Gets a value indicating whether right button can be used for different action.
        /// Right mouse button can provide more action. 
        /// For example: Moving at the screen and deleting a shape vertex.
        /// When we are moving screen right button should not be used for the another action.
        /// </remarks>
        public bool AcceptRightMouseButton
        {
            get { return _acceptRightMouseButton; }
        }
        private bool _acceptRightMouseButton;

        // previous position of mouse when the action of moving screen is active
        private PointF lastPosition;
        // position of the screen before the action of moving the screen started
        private PointF initialPosition;

        /// <inheritdoc />
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            // If right mouse button is down and no action is in progress
            // we will start changing the position of the screen (moving screen).
            if (e.Button == MouseButtons.Right && !ActionInProgress)
            {
                // activate this action
                ActionInProgress = true;
                _movingScreen = true;
                // init
                lastPosition = e.Location;
                initialPosition = Parent.Position;
            }
        }

        /// <inheritdoc />
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            _acceptRightMouseButton = true;

            // If right mouse button is up and the action of changing the position of the screen is active
            // we will finish it.
            if (e.Button == MouseButtons.Right && MovingScreen)
            {
                // this action is over
                ActionInProgress = false;
                _movingScreen = false;

                // if we change position of the screen then state should not used right mouse button for another action
                PointF wholeMovement = initialPosition.Sub(Parent.Position);
                if (wholeMovement.X != 0f || wholeMovement.Y != 0f) _acceptRightMouseButton = false;
            }
        }

        /// <inheritdoc />
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            // If the action of changing the position of the screen is active
            // we will change the position of the screen by distance we moved from the last changing.
            if (MovingScreen)
            {
                // change the position of the screen
                Parent.Position = Parent.Position.Add(lastPosition.Sub(e.Location).Mul(Parent.ScaleInversFactor));
                // set actual position for the next changing
                lastPosition = e.Location;

                Parent.Invalidate();
            }
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Ready");
        }
    }

    /// <summary>
    /// Base class for state which represent a shape.
    /// </summary>
    abstract class ShapeState : GlobalScreenState
    {
        /// <summary>
        /// Gets the underlying shape.
        /// </summary>
        public abstract Shape Shape { get; }

        /// <summary>
        /// Gets or sets the color of the shape.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Paints the shape. Called when painting the inactive shape.
        /// </summary>
        /// <param name="graphics"><see cref="Graphics"/> instance for drawing the shape.</param>
        public abstract void PaintShape(Graphics graphics);

        /// <summary>
        /// Moves the shape by <paramref name="move"/> vector.
        /// </summary>
        /// <param name="move">The move vector to the shape by.</param>
        public abstract void MoveShape(Microsoft.Xna.Framework.Vector2 move);

        /// <summary>
        /// Gets human-readable information about the shape.
        /// </summary>
        /// <returns>Human-readable information about the shape.</returns>
        public abstract string ShapeText();

        /// <summary>
        /// Determines whether the shape is valid.
        /// </summary>
        /// <returns>true if the shape is valid; otherwise false</returns>
        public abstract bool IsShapeValid();

        /// <summary>
        /// Called when we find out the shape is invalid. Provides information to the user.
        /// </summary>
        public abstract void OnInvalidShape();

        /// <summary>
        /// Paints the vertex at the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">The position to paint the vertex.</param>
        /// <param name="graphics"><see cref="Graphics"/> instance for drawing the vertex.</param>
        protected void PaintVertex(PointF position, Graphics graphics)
        {
            graphics.FillEllipse(DrawingTools.SolidBrush, position.X - Parent.VertexRadius, position.Y - Parent.VertexRadius, Parent.VertexRadius * 2, Parent.VertexRadius * 2);
        }
    }
}
