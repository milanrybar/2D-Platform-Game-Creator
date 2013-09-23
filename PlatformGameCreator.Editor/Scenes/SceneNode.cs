/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
using PlatformGameCreator.Editor.GameObjects;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Defines state for <see cref="SceneNode"/> when selecting objects at the scene.
    /// </summary>
    enum SelectState
    {
        /// <summary>
        /// Default state.
        /// </summary>
        Default,

        /// <summary>
        /// <see cref="SceneNode"/> is hovered.
        /// </summary>
        Hover,

        /// <summary>
        /// <see cref="SceneNode"/> is selected.
        /// </summary>
        Select
    };

    /// <summary>
    /// Base class for element that can be used and shown at the scene (at <see cref="SceneScreen"/> control).
    /// </summary>
    /// <remarks>
    /// Scene nodes are sorted by their <see cref="SceneNode.Index"/> value at the <see cref="SceneScreen"/> for correct drawing order.
    /// </remarks>
    abstract class SceneNode : Disposable, IComparable<SceneNode>
    {
        /// <summary>
        /// Gets or sets the underlying <see cref="SceneScreen"/> where the scene node is used.
        /// </summary>
        public SceneScreen SceneControl { get; set; }

        /// <summary>
        /// Gets the underlying <see cref="GameObject"/>.
        /// </summary>
        public abstract GameObject Tag { get; }

        /// <summary>
        /// Gets or sets the bounding rectangle of the scene node.
        /// </summary>
        public virtual AABB Rectangle { get; protected set; }

        /// <summary>
        /// Gets or sets the position of the scene node at the scene.
        /// </summary>
        public abstract Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle in radians of the scene node.
        /// </summary>
        public abstract float Angle { get; set; }

        /// <summary>
        /// Gets or sets the scale factor of the scene node.
        /// </summary>
        public abstract Vector2 ScaleFactor { get; set; }

        /// <summary>
        /// Gets a value indicating whether this scene node can be moved.
        /// </summary>
        public abstract bool CanMove { get; }

        /// <summary>
        /// Gets a value indicating whether this scene node can be rotated.
        /// </summary>
        public abstract bool CanRotate { get; }

        /// <summary>
        /// Gets a value indicating whether this scene node can be scaled.
        /// </summary>
        public abstract bool CanScale { get; }

        /// <summary>
        /// State of the scene node when selecting objects at the scene.
        /// </summary>
        public SelectState SceneSelect = SelectState.Default;

        /// <summary>
        /// Gets a value indicating whether this scene node is visible at the scene.
        /// </summary>
        public abstract bool Visible { get; }

        /// <summary>
        /// Gets the index of the scene node. Index is used for sorting scene nodes at the scene.
        /// </summary>
        public abstract int Index { get; }

        /// <summary>
        /// Gets the children of the scene node.
        /// </summary>
        public SceneNodesList Children
        {
            get { return _children; }
        }
        private SceneNodesList _children;

        /// <summary>
        /// Gets or sets the parent of the scene node.
        /// </summary>
        public SceneNode Parent { get; protected set; }

        /// <summary>
        /// Moves the scene node by the specified move vector.
        /// </summary>
        /// <param name="move">Move vector to move the scene node by.</param>
        public abstract void Move(Vector2 move);

        /// <summary>
        /// Moves the scene node to the specified position.
        /// </summary>
        /// <param name="newPosition">Position to move the scene node to.</param>
        public virtual void MoveTo(Vector2 newPosition)
        {
            Move(newPosition - Position);
        }

        /// <summary>
        /// Rotates the scene node by the specified angle.
        /// </summary>
        /// <param name="angle">Angle in radians to rotate the scene node by.</param>
        public abstract void Rotate(float angle);

        /// <summary>
        /// Rotates the scene node to the specified angle.
        /// </summary>
        /// <param name="newAngle">Angle in radians to rotate the scene node to.</param>
        public virtual void RotateTo(float newAngle)
        {
            Rotate(newAngle - Angle);
        }

        /// <summary>
        /// Scales the scene node by the specified scale factor.
        /// </summary>
        /// <param name="scale">Scale factor to scale the scene node by.</param>
        public abstract void Scale(Vector2 scale);

        /// <summary>
        /// Scales the scene node to the specified scale factor.
        /// </summary>
        /// <param name="newScale">Scale factor to scale the scene node to.</param>
        public virtual void ScaleTo(Vector2 newScale)
        {
            Scale(newScale / ScaleFactor);
        }

        /// <summary>
        /// Determines whether the scene node contains the specified point.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns><c>true</c> if the scene node contains the specified point; otherwise <c>false</c>.</returns>
        public virtual bool Contains(Vector2 point)
        {
            return Rectangle.Contains(ref point);
        }

        /// <summary>
        /// Draws the scene node.
        /// </summary>
        /// <param name="sceneBatch">The scene batch for drawing the scene node.</param>
        public abstract void Draw(SceneBatch sceneBatch);

        /// <summary>
        /// Draws the shapes of the scene node, if any.
        /// </summary>
        /// <param name="sceneBatch">The scene batch for drawing the shapes of the scene node.</param>
        public abstract void DrawShapes(SceneBatch sceneBatch);

        /// <summary>
        /// Initializes the scene node. Called after the scene node is used at the <see cref="SceneScreen"/>.
        /// </summary>
        public virtual void Initialize()
        {
            _children = new SceneNodesList(SceneControl);
        }

        /// <summary>
        /// Compares the scene node to the other specified scene node.
        /// Used for sorting scene nodes at the <see cref="SceneScreen"/> for correct drawing order.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter. Zero This object is equal to other. Greater than zero This object is greater than other.</returns>
        public int CompareTo(SceneNode other)
        {
            return Index.CompareTo(other.Index);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Children.Clear();
                _children = null;
            }
        }
    }

    /// <summary>
    /// Represents scene node that is composed from the collision shapes.
    /// </summary>
    abstract class ShapesSceneNode : SceneNode
    {
        /// <summary>
        /// Gets the shapes of the scene node.
        /// </summary>
        protected List<Shape> Shapes
        {
            get { return _shapes; }
        }
        private List<Shape> _shapes = new List<Shape>();

        /// <inheritdoc />
        public override void Move(Vector2 move)
        {
            if (!CanMove) return;

            // new position
            Position += move;

            // move all shapes
            foreach (Shape shape in Shapes)
            {
                shape.Move(move);
            }

            // update AABB rectangle
            Rectangle = new AABB(Rectangle.LowerBound + move, Rectangle.UpperBound + move);
        }

        /// <inheritdoc />
        public override void Rotate(float angle)
        {
            if (!CanRotate) return;

            // new angle
            Angle += angle;

            // rotate all shapes
            foreach (Shape shape in Shapes)
            {
                shape.Rotate(angle, Position);
            }

            // update AABB rectangle
            UpdateRectangle();
        }

        /// <inheritdoc />
        public override void Scale(Vector2 scale)
        {
            if (!CanScale) return;

            // new scale factor
            ScaleFactor *= scale;

            // scale all shapes
            foreach (Shape shape in Shapes)
            {
                shape.Scale(scale, Position);
            }

            // update AABB rectangle
            UpdateRectangle();
        }

        /// <summary>
        /// Updates the <see cref="SceneNode.Rectangle"/> of the scene node.
        /// </summary>
        protected virtual void UpdateRectangle()
        {
            if (Shapes.Count != 0)
            {
                AABB newRectangle = Shapes[0].Rectangle;

                foreach (Shape shape in Shapes)
                {
                    newRectangle.Combine(ref shape.Rectangle);
                }

                Rectangle = newRectangle;
            }
        }
    }

    /// <summary>
    /// Base command for manipulating scene node at the scene.
    /// </summary>
    /// <remarks>
    /// Checks before making a command if the scene node still exists at the scene. If not, finds the new scene node for the game object.
    /// The initial scene node do not have to exist at the scene. For example: moving the scene node at the scene, deleting scene node from the scene, undo the deleting, 
    /// game object is back added to the scene but the new scene node is created, then undo moving scene node, now the initial scene node do not exist 
    /// so we must find the new one that was created to apply the undo command.
    /// </remarks>
    abstract class BaseSceneNodeCommand : Command
    {
        /// <summary>
        /// Scene node of the command.
        /// </summary>
        protected SceneNode sceneNode;

        /// <summary>
        /// <see cref="GameObject"/> of the scene node.
        /// </summary>
        protected GameObject gameObject;

        /// <summary>
        /// <see cref="SceneScreen"/> of the scene node.
        /// </summary>
        protected SceneScreen sceneControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSceneNodeCommand"/> class.
        /// </summary>
        /// <param name="sceneNode">The scene node.</param>
        public BaseSceneNodeCommand(SceneNode sceneNode)
        {
            this.sceneNode = sceneNode;
            gameObject = sceneNode.Tag;
            sceneControl = sceneNode.SceneControl;
        }

        /// <summary>
        /// Checks if the scene node still exists at the scene.
        /// If not, finds the new scene node for the game object.
        /// </summary>
        protected void CheckSceneNode()
        {
            if (sceneNode.SceneControl == null || sceneNode.Tag == null)
            {
                sceneNode = sceneControl.Find(gameObject);
                Debug.Assert(sceneNode != null, "No Scene Control. Using command after scene is gone.");
            }
        }
    }

    /// <summary>
    /// Command for rotating the scene node at the scene.
    /// </summary>
    class SceneNodeRotateCommand : BaseSceneNodeCommand
    {
        /// <summary>
        /// Angle to rotate the scene node by.
        /// </summary>
        private float angle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodeRotateCommand"/> class.
        /// </summary>
        /// <param name="sceneNode">The scene node.</param>
        /// <param name="angle">The angle in radians to rotate the scene node by.</param>
        public SceneNodeRotateCommand(SceneNode sceneNode, float angle)
            : base(sceneNode)
        {
            this.angle = angle;
        }

        /// <summary>
        /// Rotates the scene node by the given angle.
        /// </summary>
        public override void Do()
        {
            CheckSceneNode();

            if (sceneNode.CanRotate)
            {
                sceneNode.Rotate(angle);
            }
        }

        /// <summary>
        /// Rotates the scene node back by the given angle.
        /// </summary>
        public override void Undo()
        {
            CheckSceneNode();

            if (sceneNode.CanRotate)
            {
                sceneNode.Rotate(-angle);
            }
        }
    }

    /// <summary>
    /// Command for moving the scene node at the scene.
    /// </summary>
    class SceneNodeMoveCommand : BaseSceneNodeCommand
    {
        /// <summary>
        /// Move vector to move the scene node by.
        /// </summary>
        private Vector2 move;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodeMoveCommand"/> class.
        /// </summary>
        /// <param name="sceneNode">The scene node.</param>
        /// <param name="move">The move vector to move the scene node by.</param>
        public SceneNodeMoveCommand(SceneNode sceneNode, Vector2 move)
            : base(sceneNode)
        {
            this.move = move;
        }

        /// <summary>
        /// Moves the scene node by the given move vector.
        /// </summary>
        public override void Do()
        {
            CheckSceneNode();

            if (sceneNode.CanMove)
            {
                sceneNode.Move(move);
            }
        }

        /// <summary>
        /// Moves the scene node back by the given move vector.
        /// </summary>
        public override void Undo()
        {
            CheckSceneNode();

            if (sceneNode.CanMove)
            {
                sceneNode.Move(-move);
            }
        }
    }

    /// <summary>
    /// Command for scaling the scene node at the scene.
    /// </summary>
    class SceneNodeScaleCommand : BaseSceneNodeCommand
    {
        /// <summary>
        /// Scale factor to scale the scene node by.
        /// </summary>
        private Vector2 scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodeScaleCommand"/> class.
        /// </summary>
        /// <param name="sceneNode">The scene node.</param>
        /// <param name="scale">The scale factor to scale the scene node by.</param>
        public SceneNodeScaleCommand(SceneNode sceneNode, Vector2 scale)
            : base(sceneNode)
        {
            this.scale = scale;
        }

        /// <summary>
        /// Scales the scene node by the given scale factor.
        /// </summary>
        public override void Do()
        {
            CheckSceneNode();

            if (sceneNode.CanScale)
            {
                sceneNode.Scale(scale);
            }
        }

        /// <summary>
        /// Scales the scene node back by the given scale factor.
        /// </summary>
        public override void Undo()
        {
            CheckSceneNode();

            if (sceneNode.CanScale)
            {
                sceneNode.Scale(new Vector2(1f, 1f) / scale);
            }
        }
    }
}
