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
    /// Scene state that provides scaling selected scene nodes at the <see cref="SceneScreen"/> control.
    /// </summary>
    /// <remarks>
    /// Scales the selected scene nodes at the scene by left mouse button.
    /// Uniform scaling is active by holding left Shift key.
    /// </remarks>
    class ScalingNodesSceneState : SelectingNodesSceneState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action of scaling selected scene nodes at the scene is active.
        /// </summary>
        public bool ScalingNodes
        {
            get { return _scalingNodes; }
            set
            {
                if (value && !ActionInProgress)
                {
                    _scalingNodes = true;
                    ActionInProgress = true;
                }
                else if (!value)
                {
                    _scalingNodes = false;
                    if (ActionInProgress) ActionInProgress = false;
                }
            }
        }
        private bool _scalingNodes;

        /// <summary>
        /// Gets or sets a value indicating whether the uniform scaling is active; otherwise non-uniform is active.
        /// </summary>
        public bool UniformScaling
        {
            get { return _uniformScaling; }
            set
            {
                // If we change the uniform value while scaling selected objects at the scene
                // we will update their scaling to the new selected value.
                if (ScalingNodes && _uniformScaling != value)
                {
                    ScaleSelectedNodes(value);
                }
                _uniformScaling = value;
            }
        }
        private bool _uniformScaling;

        // origin of scaling
        private Vector2 scalingNodesCenter;
        // uniform scaling initial size
        private float scalingNodesUniformInitLength;
        // non-uniform scaling initial size
        private Vector2 scalingNodesInitLenght;
        // last scale factor for calculate new scale factor
        private Vector2 scalingNodesLastScale;

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is down, no action is in progress and under mouse cursor is selected scene node
        /// we will start scaling selected scene nodes at the scene.
        /// </summary>
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);

            // If left mouse button is down, no action is in progress and under mouse cursor is selected object
            // we will start scaling selected objects at the scene.
            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                // object under mouse cursor at the scene
                SceneNode node = Screen.FindAt(Screen.MouseScenePosition);

                // is object under mouse cursor at the scene selected object?
                if (node != null && Screen.SelectedNodes.Contains(node))
                {
                    // activate this action
                    ScalingNodes = true;
                    // init scaling data
                    scalingNodesCenter = node.Position;
                    scalingNodesUniformInitLength = (Screen.MouseScenePosition - scalingNodesCenter).Length();
                    scalingNodesInitLenght = new Vector2(Math.Abs(Screen.MouseScenePosition.X - scalingNodesCenter.X), Math.Abs(Screen.MouseScenePosition.Y - scalingNodesCenter.Y));
                    scalingNodesLastScale = new Vector2(1f, 1f);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is up and the action of scaling selected scene nodes at the scene is active 
        /// we will finish it.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            // If left mouse button is up and the action of scaling selected objects at the scene is active
            // we will finish it.
            if (e.Button == MouseButtons.Left && ScalingNodes)
            {
                // this action is over
                ScalingNodes = false;

                if (scalingNodesLastScale.X != 1f || scalingNodesLastScale.Y != 1f)
                {
                    // save scaling to the history
                    CompositeCommand command = new CompositeCommand();

                    foreach (SceneNode selectedNode in Screen.SelectedNodes)
                    {
                        if (selectedNode.CanScale && !ContainsAnyParent(Screen.SelectedNodes, selectedNode))
                        {
                            command.Commands.Add(new SceneNodeScaleCommand(selectedNode, scalingNodesLastScale));
                        }
                    }

                    if (command.Commands.Count != 0) Screen.History.Add(command);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If the action of scaling selected scene nodes at the scene is active 
        /// we will change the scale factor of selected scene nodes.
        /// </summary>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            // If the action of scaling selected objects at the scene is active
            // we will change the scale factor of selected objects.
            if (ScalingNodes)
            {
                ScaleSelectedNodes(UniformScaling);
            }
        }

        /// <summary>
        /// Scales the selected scene nodes at the scene.
        /// </summary>
        /// <param name="uniformScale">If set to <c>true</c> uniform scale is active.</param>
        private void ScaleSelectedNodes(bool uniformScale)
        {
            Vector2 scaleNodes;

            if (uniformScale)
            {
                // uniform scale
                float scaleNodesUniform = (Screen.MouseScenePosition - scalingNodesCenter).Length() / scalingNodesUniformInitLength;
                scaleNodes = new Vector2(scaleNodesUniform, scaleNodesUniform);
            }
            else
            {
                // non-uniform scale
                scaleNodes = new Vector2(Math.Abs(Screen.MouseScenePosition.X - scalingNodesCenter.X), Math.Abs(Screen.MouseScenePosition.Y - scalingNodesCenter.Y)) / scalingNodesInitLenght;
            }

            // do not scale too close to 0 because we would not be able to scale back
            if (scaleNodes.X <= 0.001f) scaleNodes.X = 0.001f;
            if (scaleNodes.Y <= 0.001f) scaleNodes.Y = 0.001f;

            Vector2 scale = scaleNodes / scalingNodesLastScale;

            if (float.IsNaN(scale.X) || float.IsInfinity(scale.X) || float.IsNaN(scale.Y) || float.IsInfinity(scale.Y)) return;

            // change the scale factor of every selected object at the scene
            foreach (SceneNode selectedNode in Screen.SelectedNodes)
            {
                // can be selected object scaled?
                if (selectedNode.CanScale && !ContainsAnyParent(Screen.SelectedNodes, selectedNode))
                {
                    selectedNode.Scale(scale);
                }
            }

            // save last scale factor
            scalingNodesLastScale = scaleNodes;
        }

        /// <inheritdoc />
        /// <summary>
        /// Changes the <see cref="UniformScaling"/> value by holding left Shift.
        /// </summary>
        public override void KeyboardState(GameTime gameTime, ref Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            base.KeyboardState(gameTime, ref keyboardState);

            // If left shift key is down the uniform scale is on.
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
            {
                UniformScaling = true;
            }
            // If left shift key is up the non-uniform scale is on.
            else if (keyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftShift))
            {
                UniformScaling = false;
            }
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Scene Scale state");
        }
    }
}
