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
    /// Scene state that provides rotating selected scene nodes at the <see cref="SceneScreen"/> control.
    /// </summary>
    /// <remarks>
    /// Rotates the selected scene nodes at the scene by left mouse button.
    /// </remarks>
    class RotatingNodesSceneState : SelectingNodesSceneState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action of rotating selected scene nodes at the scene is active.
        /// </summary>
        public bool RotatingNodes
        {
            get { return _rotatingNodes; }
            set
            {
                if (value && !ActionInProgress)
                {
                    _rotatingNodes = true;
                    ActionInProgress = true;
                }
                else if (!value)
                {
                    _rotatingNodes = false;
                    if (ActionInProgress) ActionInProgress = false;
                }
            }
        }
        private bool _rotatingNodes;

        // center of rotation
        private Vector2 rotatingNodesCenter;
        // init vector (from center to init position)
        private Vector2 rotatingNodesInitVector;
        // last used angle
        private float rotatingNodesLastAngle;

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is down, no action is in progress and under mouse cursor is selected scene node
        /// we will start rotating selected scene nodes at the scene.
        /// </summary>
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);

            // If left mouse button is down, no action is in progress and under mouse cursor is selected object
            // we will start rotating selected objects at the scene.
            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                // object under mouse cursor at the scene
                SceneNode node = Screen.FindAt(Screen.MouseScenePosition);

                // is object under mouse cursor at the scene selected object?
                if (node != null && Screen.SelectedNodes.Contains(node))
                {
                    // activate this action
                    RotatingNodes = true;
                    // init 
                    rotatingNodesCenter = node.Position;
                    rotatingNodesInitVector = Screen.MouseScenePosition - rotatingNodesCenter;
                    rotatingNodesLastAngle = 0f;
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is up and the action of rotating selected scene nodes at the scene is active 
        /// we will finish it.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            // If left mouse button is up and the action of rotating selected objects at the scene is active
            // we will finish it.
            if (e.Button == MouseButtons.Left && RotatingNodes)
            {
                // this action is over
                RotatingNodes = false;

                if (rotatingNodesLastAngle != 0f)
                {
                    // save rotation to the history
                    CompositeCommand command = new CompositeCommand();

                    foreach (SceneNode selectedNode in Screen.SelectedNodes)
                    {
                        if (selectedNode.CanRotate && !ContainsAnyParent(Screen.SelectedNodes, selectedNode))
                        {
                            command.Commands.Add(new SceneNodeRotateCommand(selectedNode, rotatingNodesLastAngle));
                        }
                    }

                    if (command.Commands.Count != 0) Screen.History.Add(command);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If the action of rotating selected scene nodes at the scene is active 
        /// we will change the angle of selected scene nodes by angle we moved from initial position about center point.
        /// </summary>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            // If the action of rotating selected objects at the scene is active
            // we will change the angle of selected objects by angle we moved from initial position about center point.
            if (RotatingNodes)
            {
                // actual vector
                Vector2 actualVector = Screen.MouseScenePosition - rotatingNodesCenter;
                // calculate angle between rotatingNodesInitVector and actualVector
                float angle = (float)(Math.Atan2(actualVector.Y, actualVector.X) - Math.Atan2(rotatingNodesInitVector.Y, rotatingNodesInitVector.X));

                // change the angle of every selected object at the scene
                foreach (SceneNode selectedNode in Screen.SelectedNodes)
                {
                    // can be selected object rotated?
                    if (selectedNode.CanRotate && !ContainsAnyParent(Screen.SelectedNodes, selectedNode))
                    {
                        selectedNode.Rotate(angle - rotatingNodesLastAngle);
                    }
                }

                // set last used angle
                rotatingNodesLastAngle = angle;
            }
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Scene Rotate state");
        }
    }
}
