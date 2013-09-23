/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Scene state that provides moving selected scene nodes at the <see cref="SceneScreen"/> control.
    /// </summary>
    /// <remarks>
    /// Moves the selected scene nodes at the scene by left mouse button.
    /// </remarks>
    class MovingNodesSceneState : SelectingNodesSceneState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action of moving selected scene nodes at the scene is active.
        /// </summary>
        public bool MovingNodes
        {
            get { return _movingNodes; }
            set
            {
                if (value && !ActionInProgress)
                {
                    _movingNodes = true;
                    ActionInProgress = true;
                }
                else if (!value)
                {
                    _movingNodes = false;
                    if (ActionInProgress) ActionInProgress = false;
                }
            }
        }
        private bool _movingNodes;

        // last position at the scene for calculate move vector to the new position
        private Vector2 movingNodesLastPosition;
        private Vector2 wholeMovement;

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is down, no action is in progress and under mouse cursor is selected scene node
        /// we will start moving selected scene nodes at the scene.
        /// </summary>
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);

            // If left mouse button is down, no action is in progress and under mouse cursor is selected object
            // we will start moving selected objects at the scene.
            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                // object under mouse cursor at the scene
                SceneNode node = Screen.FindAt(Screen.MouseScenePosition);

                // is object under mouse cursor at the scene selected object?
                if (node != null && Screen.SelectedNodes.Contains(node))
                {
                    // activate this action
                    MovingNodes = true;
                    // init 
                    movingNodesLastPosition = new Vector2(e.X, e.Y);
                    wholeMovement = new Vector2();
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is up and the action of moving selected scene nodes at the scene is active 
        /// we will finish it.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            // If left mouse button is up and the action of moving selected objects at the scene is active
            // we will finish it.
            if (e.Button == MouseButtons.Left && MovingNodes)
            {
                // this action is over
                MovingNodes = false;

                if (wholeMovement.X != 0f || wholeMovement.Y != 0f)
                {
                    // save movement to the history
                    CompositeCommand command = new CompositeCommand();

                    foreach (SceneNode selectedNode in Screen.SelectedNodes)
                    {
                        if (selectedNode.CanMove && !ContainsAnyParent(Screen.SelectedNodes, selectedNode))
                        {
                            command.Commands.Add(new SceneNodeMoveCommand(selectedNode, wholeMovement));
                        }
                    }

                    if (command.Commands.Count != 0) Screen.History.Add(command);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If the action of moving selected scene nodes at the scene is active 
        /// we will change the positions of selected scene nodes by distance we moved from the last changing.
        /// </summary>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            // If the action of moving selected objects at the scene is active
            // we will change the positions of selected objects by distance we moved from the last changing.
            if (MovingNodes)
            {
                // actual position
                Vector2 actualScenePosition = new Vector2(e.X, e.Y);
                // calculate move vector for selected objects at the scene
                Vector2 move = (actualScenePosition - movingNodesLastPosition) * Screen.ScaleInversFactor;

                // change the position of every selected object at the scene
                foreach (SceneNode selectedNode in Screen.SelectedNodes)
                {
                    // can be selected object moved?
                    if (selectedNode.CanMove && !ContainsAnyParent(Screen.SelectedNodes, selectedNode))
                    {
                        selectedNode.Move(move);
                    }
                }

                // set actual position for the next changing
                movingNodesLastPosition = actualScenePosition;
                wholeMovement += move;
            }
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Scene Move state");
        }
    }
}
