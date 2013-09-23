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
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.GameObjects.Paths;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Scene state that provides selecting scene nodes at the <see cref="SceneScreen"/> control.
    /// </summary>
    /// <remarks>
    /// Selects the scene nodes at the scene by left mouse button.
    /// Select or deselect scene node by left mouse button + Ctrl.
    /// Selects the scene nodes by rectangle even on hovered scene node by left mouse button + left Alt.
    /// Deletes the selected scene nodes from the scene by Delete key.
    /// Saves the selected scene nodes to the clipboard by Ctrl+C.
    /// Pastes the scene nodes from the clipboard by Ctrl+V.
    /// </remarks>
    class SelectingNodesSceneState : GlobalBehaviourSceneState
    {
        /// <inheritdoc />
        public override bool CanBeInterrupted
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanBeInStack
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the action of selecting scene nodes is active.
        /// </summary>
        public bool SelectingNodes
        {
            get { return _selectingNodes; }
            set
            {
                if (value && !ActionInProgress)
                {
                    _selectingNodes = true;
                    ActionInProgress = true;
                }
                else if (!value)
                {
                    _selectingNodes = false;
                    if (ActionInProgress) ActionInProgress = false;
                }
            }
        }
        private bool _selectingNodes;

        private Vector2 selectingNodesStartPoint;
        private Vector2 selectingNodesEndPoint;

        // data for drawing selecting rectangle
        private static VertexPositionColor[] drawSelectingRectangle = new VertexPositionColor[4];

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectingNodesSceneState"/> class.
        /// </summary>
        public SelectingNodesSceneState()
        {
            SelectingNodes = false;

            drawSelectingRectangle[0].Color = ColorSettings.SelectingNodesRectangle;
            drawSelectingRectangle[1].Color = ColorSettings.SelectingNodesRectangle;
            drawSelectingRectangle[2].Color = ColorSettings.SelectingNodesRectangle;
            drawSelectingRectangle[3].Color = ColorSettings.SelectingNodesRectangle;
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse button is down and no action is in progress 
        /// we will begin selecting scene nodes at the scene.
        /// </summary>
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);

            // If left mouse button is down and no action is in progress
            // we will make some action.
            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                // If left mouse button is down, Ctrl key is down and no action is in progress
                // we will select or deselect node under the mouse cursor at the scene.
                if (Form.ModifierKeys == Keys.Control)
                {
                    Debug.Assert(Screen.HoveredNodes.Count <= 1, "Only one node can be hovered.");

                    foreach (SceneNode hoveredNode in Screen.HoveredNodes)
                    {
                        // select node under mouse cursor
                        if (!Screen.SelectedNodes.Contains(hoveredNode))
                        {
                            hoveredNode.SceneSelect = SelectState.Select;
                            Screen.SelectedNodes.Add(hoveredNode);

                            // selected nodes changed
                            Screen.InvokeSelectedNodesChanged();
                        }
                        // deselect node under mouse cursor
                        else
                        {
                            hoveredNode.SceneSelect = SelectState.Hover;
                            Screen.SelectedNodes.Remove(hoveredNode);

                            // selected nodes changed
                            Screen.InvokeSelectedNodesChanged();
                        }
                    }
                }

                // If left mouse button is down, no action is in progress and no node is under mouse cursor or left Alt key is down
                // we will start selecting objects at the scene.
                else if (Screen.HoveredNodes.Count == 0 || Form.ModifierKeys == Keys.Alt)
                {
                    // action of selecting nodes is active
                    SelectingNodes = true;

                    // init points for selecting rectangle
                    selectingNodesStartPoint = Screen.MouseScenePosition;
                    selectingNodesEndPoint = selectingNodesStartPoint;

                    UpdateSelectingRectangleToDraw();
                }

                // If left mouse button is down, no action is in progress and some node is under mouse cursor 
                // we will check node under mouse cursor:
                // 1. If no nodes are selected we will select node under the mouse cursor.
                // 2. If some nodes are selected but under mouse is not selected node 
                // we will deselect the current selected nodes and select node under mouse cursor.
                else if (Screen.SelectedNodes.Count == 0 || !Screen.SelectedNodes.Contains(Screen.HoveredNodes[0]))
                {
                    // select hovered nodes
                    SelectHoveredNodes();

                    // selected nodes changed
                    Screen.InvokeSelectedNodesChanged();
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If left mouse is up and action of selecting nodes at the scene is active 
        /// we will finish this action.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            // If left mouse is up and action of selecting nodes at the scene is active 
            // we will finish this action.
            if (e.Button == MouseButtons.Left && SelectingNodes)
            {
                // end point for selecting rectangle
                selectingNodesEndPoint = Screen.MouseScenePosition;

                // clear all hovered nodes
                ClearHoveredNodes();

                // clear all selected nodes
                ClearSelectedNodes();

                // set selected nodes by selecting rectangle
                Screen.SelectedNodes.AddRange(Screen.FindAt(selectingNodesStartPoint, selectingNodesEndPoint));

                foreach (SceneNode node in Screen.SelectedNodes)
                {
                    node.SceneSelect = SelectState.Select;
                }

                // this action is over
                SelectingNodes = false;

                // selected nodes changed
                Screen.InvokeSelectedNodesChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If no action is in progress we will hover a scene node at the scene under mouse cursor.
        /// If an action of selecting objects is active 
        /// we will update selecting rectangle and hover scene nodes within selecting rectangle.
        /// </summary>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            // If no action is in progress we will hover an object at the scene under mouse cursor.
            if (!ActionInProgress)
            {
                Debug.Assert(Screen.HoveredNodes.Count == 0 || Screen.HoveredNodes.Count == 1, "Selecting state assume that only one SceneNode can be hovered (if action is not in progress).");

                // find object at the scene under mouse cursor
                SceneNode hoverNode = Screen.FindAt(Screen.MouseScenePosition);

                // change hovered object only when different object is under mouse cursor
                if (Screen.HoveredNodes.Count == 0 || (Screen.HoveredNodes.Count == 1 && Screen.HoveredNodes[0] != hoverNode))
                {
                    // clear previous hovered object
                    ClearHoveredNodes();

                    if (hoverNode != null)
                    {
                        // set as hovered
                        if (hoverNode.SceneSelect != SelectState.Select) hoverNode.SceneSelect = SelectState.Hover;
                        Screen.HoveredNodes.Add(hoverNode);
                    }
                }
            }
            // If an action of selecting objects is active 
            // we will update selecting rectangle and hover objects within selecting rectangle.
            else if (SelectingNodes)
            {
                // new end point for selecting rectangle
                selectingNodesEndPoint = Screen.MouseScenePosition;

                // clear all hovered nodes
                ClearHoveredNodes();

                // set new hovered object within selecting rectangle
                Screen.HoveredNodes.AddRange(Screen.FindAt(selectingNodesStartPoint, selectingNodesEndPoint));
                // set them as hovered
                foreach (SceneNode hoverNode in Screen.HoveredNodes)
                {
                    hoverNode.SceneSelect = SelectState.Hover;
                }

                UpdateSelectingRectangleToDraw();
            }
        }

        /// <summary>
        /// Removed all scene nodes from the hovered scene nodes.
        /// </summary>
        protected void ClearHoveredNodes()
        {
            // clear hovered object
            foreach (SceneNode hoveredNode in Screen.HoveredNodes)
            {
                if (hoveredNode.SceneSelect != SelectState.Select) hoveredNode.SceneSelect = SelectState.Default;
            }
            Screen.HoveredNodes.Clear();
        }

        /// <summary>
        /// Removed all scene nodes from the selected scene nodes.
        /// </summary>
        protected void ClearSelectedNodes()
        {
            // clear all selected nodes
            foreach (SceneNode selectedNode in Screen.SelectedNodes)
            {
                selectedNode.SceneSelect = SelectState.Default;
            }
            Screen.SelectedNodes.Clear();
        }

        /// <summary>
        /// Changes the current hovered scene nodes to selected scene nodes.
        /// </summary>
        protected void SelectHoveredNodes()
        {
            // clear the old selected nodes
            ClearSelectedNodes();

            // select hovered nodes
            foreach (SceneNode hoveredNode in Screen.HoveredNodes)
            {
                hoveredNode.SceneSelect = SelectState.Select;
                Screen.SelectedNodes.Add(hoveredNode);
            }
        }

        /// <summary>
        /// Updates vertices for drawing selecting rectangle.
        /// </summary>
        private void UpdateSelectingRectangleToDraw()
        {
            drawSelectingRectangle[0].Position = new Vector3(selectingNodesEndPoint, 0.9f);
            drawSelectingRectangle[3].Position = new Vector3(selectingNodesStartPoint, 0.9f);

            if ((selectingNodesStartPoint.X >= selectingNodesEndPoint.X && selectingNodesStartPoint.Y >= selectingNodesEndPoint.Y) ||
                (selectingNodesStartPoint.X <= selectingNodesEndPoint.X && selectingNodesStartPoint.Y <= selectingNodesEndPoint.Y))
            {
                drawSelectingRectangle[1].Position = new Vector3(selectingNodesStartPoint.X, selectingNodesEndPoint.Y, 0.9f);
                drawSelectingRectangle[2].Position = new Vector3(selectingNodesEndPoint.X, selectingNodesStartPoint.Y, 0.9f);
            }
            else
            {
                drawSelectingRectangle[2].Position = new Vector3(selectingNodesStartPoint.X, selectingNodesEndPoint.Y, 0.9f);
                drawSelectingRectangle[1].Position = new Vector3(selectingNodesEndPoint.X, selectingNodesStartPoint.Y, 0.9f);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If the action of selecting scene nodes at the scene is active 
        /// we will draw selecting rectangle.
        /// </summary>
        public override void Draw(SceneBatch sceneBatch)
        {
            // If the action of selecting objects at the scene is active
            // we will draw selecting rectangle.
            if (SelectingNodes)
            {
                sceneBatch.ApplyBasicEffect();
                sceneBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, drawSelectingRectangle, 0, 2);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Saves the selected scene nodes to the clipboard by Ctrl+C.
        /// Pastes the scene nodes from the clipboard by Ctrl+V.
        /// Deletes the selected scene nodes from the scene by Delete.
        /// </summary>
        public override void KeyDown(object sender, KeyEventArgs e)
        {
            base.KeyDown(sender, e);

            // copy selected objects at the scene to the clipboard
            if (e.Control && e.KeyCode == Keys.C)
            {
                // clear the current clipboard
                Screen.Clipboard.Clear();
                // rectangle of selected nodes for calculate clipboard position
                Vector2? lowerBound = null, upperBound = null;

                // add selected objects at the scene to the clipboard
                foreach (SceneNode selectedNode in Screen.SelectedNodes)
                {
                    if (selectedNode.Visible)
                    {
                        Screen.Clipboard.Add(selectedNode);

                        if (lowerBound == null)
                        {
                            lowerBound = selectedNode.Rectangle.LowerBound;
                            upperBound = selectedNode.Rectangle.UpperBound;
                        }
                        else
                        {
                            lowerBound = Vector2.Min(lowerBound.Value, selectedNode.Rectangle.LowerBound);
                            upperBound = Vector2.Max(upperBound.Value, selectedNode.Rectangle.UpperBound);
                        }
                    }
                }
                // calculate clipboard position 
                if (lowerBound != null) Screen.ClipboardPosition = 0.5f * (lowerBound.Value + upperBound.Value);
            }

            // paste objects from clipboard to the scene
            else if (e.Control && e.KeyCode == Keys.V && Screen.Clipboard.Count != 0)
            {
                // compute move vector for placing objects at the correct position at the scene
                Vector2 moveVector = Screen.MouseScenePosition - Screen.ClipboardPosition;

                // save copied objects to the history
                CompositeCommand command = new CompositeCommand();

                List<GameObject> objectsToCloned = new List<GameObject>();

                // copy all objects from clipboard to the scene
                foreach (SceneNode node in Screen.Clipboard)
                {
                    if (node.Visible && !ContainsAnyParent(Screen.Clipboard, node))
                    {
                        objectsToCloned.Add(node.Tag);
                    }
                }

                foreach (GameObject clonedObject in CloningHelper.Clone(objectsToCloned, Screen.Scene.SelectedLayer, true, true))
                {
                    // move object to the new position
                    Screen.Find(clonedObject).Move(moveVector);

                    command.Commands.Add(clonedObject.CreateAddCommand());
                }

                // add command to history
                if (command.Commands.Count != 0) Screen.History.Add(command);
            }

            // delete selected objects from the scene and copy them to the clipboard
            // NOT WORKING when safe deleting is on because it will clear clipboard
            /*else if (e.Control && e.KeyCode == Keys.X)
            {
                // clear the current clipboard
                Screen.Clipboard.Clear();
                // rectangle of selected nodes for calculate clipboard position
                Vector2? lowerBound = null, upperBound = null;
                // save deleted objects to the history
                CompositeCommand command = new CompositeCommand();

                List<ItemForDeletion> itemsForDeletion = new List<ItemForDeletion>();

                // add selected objects at the scene to the clipboard
                foreach (SceneNode selectedNode in Screen.SelectedNodes)
                {
                    if (selectedNode.Visible)
                    {
                        command.Commands.Add(selectedNode.Tag.CreateDeleteCommand());

                        if (selectedNode.Tag is Actor) itemsForDeletion.Add(new ConsistentDeletionHelper.ActorForDeletion((Actor)selectedNode.Tag));
                        else if (selectedNode.Tag is Path) itemsForDeletion.Add(new ConsistentDeletionHelper.PathForDeletion((Path)selectedNode.Tag));
                        else
                        {
                            Debug.Assert(true, "Not supported game objects deleting.");
                            selectedNode.Tag.Remove();
                        }

                        // add to the clipboard
                        Screen.Clipboard.Add(selectedNode);

                        if (lowerBound == null)
                        {
                            lowerBound = selectedNode.Rectangle.LowerBound;
                            upperBound = selectedNode.Rectangle.UpperBound;
                        }
                        else
                        {
                            lowerBound = Vector2.Min(lowerBound.Value, selectedNode.Rectangle.LowerBound);
                            upperBound = Vector2.Max(upperBound.Value, selectedNode.Rectangle.UpperBound);
                        }
                    }
                }
                // calculate clipboard position 
                if (lowerBound != null) Screen.ClipboardPosition = 0.5f * (lowerBound.Value + upperBound.Value);

                if ((new ConsistentDeletionForm(itemsForDeletion) { ProcessWhenEmptyList = true }).ShowDialog() == DialogResult.OK)
                {
                    if (command.Commands.Count != 0) Screen.History.Add(command);

                    // clear selected objects at the scene
                    Screen.SelectedNodes.Clear();

                    // selected nodes changed
                    Screen.InvokeSelectedNodesChanged();
                }
            }*/

            // delete selected objects from the scene
            else if (e.KeyCode == Keys.Delete)
            {
                // save deleted objects to the history
                CompositeCommand command = new CompositeCommand();

                List<ItemForDeletion> itemsForDeletion = new List<ItemForDeletion>();

                // remove all selected objects from the scene
                foreach (SceneNode selectedNode in Screen.SelectedNodes)
                {
                    if (selectedNode.Visible)
                    {
                        command.Commands.Add(selectedNode.Tag.CreateDeleteCommand());

                        if (selectedNode.Tag is Actor) itemsForDeletion.Add(new ConsistentDeletionHelper.ActorForDeletion((Actor)selectedNode.Tag));
                        else if (selectedNode.Tag is Path) itemsForDeletion.Add(new ConsistentDeletionHelper.PathForDeletion((Path)selectedNode.Tag));
                        else
                        {
                            Debug.Assert(true, "Not supported game objects deleting.");
                            selectedNode.Tag.Remove();
                        }
                    }
                }

                if ((new ConsistentDeletionForm(itemsForDeletion) { ProcessWhenEmptyList = true }).ShowDialog() == DialogResult.OK)
                {
                    if (command.Commands.Count != 0) Screen.History.Add(command);

                    // clear selected objects at the scene
                    Screen.SelectedNodes.Clear();

                    // selected nodes changed
                    Screen.InvokeSelectedNodesChanged();
                }
            }
        }

        /// <inheritdoc />
        public override void OnSet()
        {
            Messages.ShowInfo("Scene Select state");
        }
    }
}
