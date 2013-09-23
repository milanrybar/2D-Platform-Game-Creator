/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scenes;
using System.Windows.Forms;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.GameObjects.Paths
{
    /// <summary>
    /// Scene state that provides adding the path to the scene.
    /// </summary>
    /// <remarks>
    /// Left Mouse - Add path vertex. Esc - Exit adding.
    /// </remarks>
    class AddPathSceneState : GlobalBehaviourSceneState
    {
        /// <inheritdoc />
        public override bool CanBeInterrupted
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanBeInStack
        {
            get { return false; }
        }

        /// <summary>
        /// Previous state of the <see cref="SceneScreen"/> control. 
        /// </summary>
        private SceneState lastState;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPathSceneState"/> class.
        /// </summary>
        /// <param name="lastState">Previous state of the <see cref="SceneScreen"/> control. </param>
        public AddPathSceneState(SceneState lastState)
        {
            Debug.Assert(lastState.CanBeInStack, "Invalid use.");

            this.lastState = lastState;
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is up and no action is in progress 
        /// we will create new path and add new vertex that is under the mouse cursor to the created path and
        /// we will change state of the <see cref="SceneScreen"/> to the <see cref="EditingPathSceneState"/> of created path.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                Path newPath = new Path();
                newPath.Vertices.Add(Screen.MouseScenePosition);

                Screen.Scene.Paths.Add(newPath);

                PathView newPathView = Screen.Find(newPath) as PathView;
                Screen.State = new EditingPathSceneState(newPathView, lastState);

                // clear all selected nodes
                foreach (SceneNode selectedNode in Screen.SelectedNodes)
                {
                    selectedNode.SceneSelect = SelectState.Default;
                }
                Screen.SelectedNodes.Clear();

                // selected new path view
                Screen.SelectedNodes.Add(newPathView);
                newPathView.SceneSelect = SelectState.Select;

                Screen.InvokeSelectedNodesChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Esc - Exit adding state.
        /// </summary>
        public override void KeyPress(object sender, KeyPressEventArgs e)
        {
            base.KeyPress(sender, e);

            // exit editing path
            if (e.KeyChar == (char)Keys.Escape)
            {
                Screen.State = lastState;
            }
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Creating path. Click to the scene to add path vertex. Press Esc to cancel creating path.");
        }
    }
}
