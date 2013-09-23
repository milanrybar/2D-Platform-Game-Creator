/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.GameObjects.Paths;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Common;
using Aga.Controls.Tree.NodeControls;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// View of the <see cref="Scene"/> and all its content (paths, layers and their actors).
    /// Everything is done automatically by setting <see cref="SceneTreeView.Scene"/> property.
    /// </summary>
    partial class SceneTreeView : UserControl
    {
        /// <summary>
        /// Base tree node for using in the <see cref="TreeView"/>.
        /// </summary>
        private abstract class BaseSceneTreeNode : Node, IDisposable
        {
            /// <summary>
            /// Gets the <see cref="SceneTreeView"/> where the tree node is used.
            /// </summary>
            public SceneTreeView SceneTree
            {
                get { return _sceneTree; }
            }
            private SceneTreeView _sceneTree;

            /// <summary>
            /// Gets or sets the text of the tree node.
            /// </summary>
            /// <remarks>
            /// Represents the name of the underlying scene content.
            /// For example: name of the actor, layer or path.
            /// </remarks>
            public override string Text
            {
                get { return base.Text; }
                set
                {
                    base.Text = value;
                    TextChanged();
                }
            }

            /// <summary>
            /// Gets or sets the visible of the tree node.
            /// </summary>
            /// <remarks>
            /// Represents the visible property of the underlying scene content.
            /// For example: visible property of the actor, layer or path.
            /// </remarks>
            public bool Visible
            {
                get { return _visible; }
                set
                {
                    if (_visible != value)
                    {
                        _visible = value;
                        VisibleChanged();
                        NotifyModel();
                    }
                }
            }
            protected bool _visible;

            /// <summary>
            /// Gets or sets a value indicating whether the <see cref="Visible"/> property can be set.
            /// </summary>
            public bool AllowVisible { get; set; }

            /// <summary>
            /// Gets a value indicating whether the instance has been disposed of.
            /// </summary>
            public bool IsDisposed
            {
                get { return _disposed; }
            }
            private bool _disposed = false;

            /// <summary>
            /// Initializes a new instance of the <see cref="BaseSceneTreeNode"/> class.
            /// </summary>
            /// <param name="sceneTree">The <see cref="SceneTreeView"/> where the tree node will be used.</param>
            /// <param name="text">The text of the tree node.</param>
            public BaseSceneTreeNode(SceneTreeView sceneTree, string text)
                : base(text)
            {
                _sceneTree = sceneTree;
                AllowVisible = true;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (!IsDisposed)
                {
                    Dispose(true);
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
            }

            /// <summary>
            /// Occurs when the <see cref="Visible"/> property value changes.
            /// </summary>
            protected abstract void VisibleChanged();

            /// <summary>
            /// Occurs when the <see cref="Text"/> property value changes.
            /// </summary>
            protected abstract void TextChanged();

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise <c>false</c>.</param>
            protected abstract void Dispose(bool disposing);
        }

        /// <summary>
        /// Tree node for using in the <see cref="TreeView"/> that fires its event when some internal change happens.
        /// </summary>
        private class NodeWithEvents : BaseSceneTreeNode
        {
            /// <summary>
            /// Occurs when the <see cref="BaseSceneTreeNode.Visible"/> property value changes.
            /// </summary>
            public event EventHandler OnVisibleChanged;

            /// <summary>
            /// Occurs when the <see cref="BaseSceneTreeNode.Text"/> property value changes.
            /// </summary>
            public event EventHandler OnTextChanged;

            /// <summary>
            /// Initializes a new instance of the <see cref="NodeWithEvents"/> class.
            /// </summary>
            /// <param name="sceneTree">The <see cref="SceneTreeView"/> where the tree node will be used.</param>
            /// <param name="text">The text of the tree node.</param>
            public NodeWithEvents(SceneTreeView sceneTree, string text)
                : base(sceneTree, text)
            {
            }

            /// <inheritdoc />
            protected override void VisibleChanged()
            {
                if (OnVisibleChanged != null) OnVisibleChanged(this, EventArgs.Empty);
            }

            /// <inheritdoc />
            protected override void TextChanged()
            {
                if (OnTextChanged != null) OnTextChanged(this, EventArgs.Empty);
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
            }
        }

        /// <summary>
        /// Tree node of the scene content for using in the <see cref="TreeView"/>.
        /// </summary>
        /// <typeparam name="T">Type of the scene content.</typeparam>
        private class SceneNode<T> : BaseSceneTreeNode where T : class, IName, IVisible
        {
            /// <summary>
            /// Gets the scene content.
            /// </summary>
            public T Value
            {
                get { return _value; }
            }
            private T _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="SceneNode&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="sceneTree">The <see cref="SceneTreeView"/> where the tree node will be used.</param>
            /// <param name="value">The scene content.</param>
            public SceneNode(SceneTreeView sceneTree, T value)
                : base(sceneTree, value.Name)
            {
                _value = value;
                _visible = value.Visible;

                Value.NameChanged += new EventHandler(Value_NameChanged);
                Value.VisibleChanged += new EventHandler(Value_VisibleChanged);
            }

            /// <summary>
            /// Called when the visible property of the scene content changes.
            /// Updates the visible in the tree node.
            /// </summary>
            private void Value_VisibleChanged(object sender, EventArgs e)
            {
                if (Value.Visible != Visible)
                {
                    Visible = Value.Visible;
                }
            }

            /// <summary>
            /// Called when the name property of the scene content changes.
            /// Updates the text in the tree node.
            /// </summary>
            private void Value_NameChanged(object sender, EventArgs e)
            {
                if (Value.Name != Text)
                {
                    Text = Value.Name;
                }
            }

            /// <inheritdoc />
            /// <remarks>
            /// Updates the name property in the scene content.
            /// </remarks>
            protected override void TextChanged()
            {
                if (Value.Name != Text)
                {
                    if (Text != String.Empty) Value.Name = Text;
                    else Text = Value.Name;
                }
            }

            /// <inheritdoc />
            /// <remarks>
            /// Updates the visible property in the scene content.
            /// </remarks>
            protected override void VisibleChanged()
            {
                if (Value.Visible != Visible)
                {
                    Value.Visible = Visible;
                }
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (Value != null)
                    {
                        Value.NameChanged -= new EventHandler(Value_NameChanged);
                        Value.VisibleChanged -= new EventHandler(Value_VisibleChanged);
                        _value = null;
                    }
                }
            }
        }

        /// <summary>
        /// Tree node that represents the <see cref="Actor"/>.
        /// </summary>
        private class ActorSceneNode : SceneNode<Actor>
        {
            /// <summary>
            /// Icon of the actor used for the tree node.
            /// </summary>
            private static Image actorIcon;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActorSceneNode"/> class.
            /// Creates the nodes for the actor children.
            /// </summary>
            /// <param name="sceneTree">The <see cref="SceneTreeView"/> where the tree node will be used.</param>
            /// <param name="value">The actor.</param>
            public ActorSceneNode(SceneTreeView sceneTree, Actor value)
                : base(sceneTree, value)
            {
                Value.Children.ListChanged += new ObservableList<Actor>.ListChangedEventHandler(Children_ListChanged);

                foreach (Actor child in Value.Children)
                {
                    Nodes.Add(SceneTree.CreateNodeByType(child));
                }

                if (actorIcon == null) actorIcon = Properties.Resources.actor;
                Image = actorIcon;
            }

            /// <summary>
            /// Called when the children of the actor changes (child is added or removed).
            /// </summary>
            private void Children_ListChanged(object sender, ObservableListChangedEventArgs<Actor> e)
            {
                SceneTree.BasicListChanged(this, e);
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                if (disposing && Value != null)
                {
                    Value.Children.ListChanged -= new ObservableList<Actor>.ListChangedEventHandler(Children_ListChanged);
                }

                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Tree node that represents the <see cref="Layer"/>.
        /// </summary>
        private class LayerSceneNode : SceneNode<Layer>
        {
            /// <summary>
            /// Icon of the layer used for the tree node.
            /// </summary>
            private static Image layerIcon;

            /// <summary>
            /// Icon of the parallax layer used for the tree node.
            /// </summary>
            private static Image layerParallaxIcon;

            /// <summary>
            /// Initializes a new instance of the <see cref="LayerSceneNode"/> class.
            /// </summary>
            /// <param name="sceneTree">The <see cref="SceneTreeView"/> where the tree node will be used.</param>
            /// <param name="value">The layer.</param>
            public LayerSceneNode(SceneTreeView sceneTree, Layer value)
                : base(sceneTree, value)
            {
                Value.ListChanged += new ObservableList<Actor>.ListChangedEventHandler(Value_ListChanged);

                foreach (Actor actor in Value)
                {
                    Nodes.Add(SceneTree.CreateNodeByType(actor));
                }

                if (value.IsParallax)
                {
                    if (layerParallaxIcon == null) layerParallaxIcon = Properties.Resources.layerParallax;
                    Image = layerParallaxIcon;
                }
                else
                {
                    if (layerIcon == null) layerIcon = Properties.Resources.layer;
                    Image = layerIcon;
                }
            }

            /// <summary>
            /// Called when the layer changes (actor is added or removed).
            /// </summary>
            private void Value_ListChanged(object sender, ObservableListChangedEventArgs<Actor> e)
            {
                SceneTree.BasicListChanged(this, e);
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                if (disposing && Value != null)
                {
                    Value.ListChanged -= new ObservableList<Actor>.ListChangedEventHandler(Value_ListChanged);
                }

                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Tree node that represents the <see cref="Path"/>.
        /// </summary>
        private class PathSceneNode : SceneNode<Path>
        {
            /// <summary>
            /// Icon of the path used for the tree node.
            /// </summary>
            private static Image pathIcon;

            /// <summary>
            /// Initializes a new instance of the <see cref="PathSceneNode"/> class.
            /// </summary>
            /// <param name="sceneTree">The <see cref="SceneTreeView"/> where the tree node will be used.</param>
            /// <param name="path">The path.</param>
            public PathSceneNode(SceneTreeView sceneTree, Path path)
                : base(sceneTree, path)
            {
                if (pathIcon == null) pathIcon = Properties.Resources.path;
                Image = pathIcon;
            }
        }

        /// <summary>
        /// Gets or sets the scene to view.
        /// </summary>
        public Scene Scene
        {
            get { return _scene; }
            set
            {
                treeView.BeginUpdate();

                if (_scene != null)
                {
                    _scene.Paths.ListChanged -= new ObservableList<Path>.ListChangedEventHandler(Paths_ListChanged);
                    _scene.Layers.ListChanged -= new ObservableList<Layer>.ListChangedEventHandler(Layers_ListChanged);
                    _scene.SelectedLayerChanged -= new EventHandler(Scene_SelectedLayerChanged);

                    foreach (BaseSceneTreeNode node in treeModel.Nodes)
                    {
                        Clear(node);
                    }

                    treeModel.Nodes.Clear();
                }

                _scene = value;

                if (_scene != null)
                {
                    // paths
                    pathsNode = new NodeWithEvents(this, "Paths") { AllowVisible = false };
                    treeModel.Nodes.Add(pathsNode);

                    _scene.Paths.ListChanged += new ObservableList<Path>.ListChangedEventHandler(Paths_ListChanged);

                    foreach (Path path in _scene.Paths)
                    {
                        pathsNode.Nodes.Add(CreateNodeByType(path));
                    }

                    // layers
                    layersNode = new NodeWithEvents(this, "Layers") { AllowVisible = false };
                    treeModel.Nodes.Add(layersNode);

                    _scene.Layers.ListChanged += new ObservableList<Layer>.ListChangedEventHandler(Layers_ListChanged);
                    _scene.SelectedLayerChanged += new EventHandler(Scene_SelectedLayerChanged);

                    foreach (Layer layer in _scene.Layers)
                    {
                        layersNode.Nodes.Add(CreateNodeByType(layer));
                    }

                    treeView.ExpandAll();
                }

                treeView.EndUpdate();
            }
        }
        private Scene _scene;

        // model for the tree
        private TreeModel treeModel = new TreeModel();
        // root node for layers
        private BaseSceneTreeNode layersNode;
        // root node for paths
        private BaseSceneTreeNode pathsNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneTreeView"/> class.
        /// </summary>
        public SceneTreeView()
        {
            InitializeComponent();

            nodeCheckBox.IsVisibleValueNeeded += new EventHandler<NodeControlValueEventArgs>(nodeCheckBox_IsVisibleValueNeeded);
            nodeTextBox.DrawText += new EventHandler<DrawEventArgs>(nodeTextBox_DrawText);

            treeView.Model = treeModel;
        }

        /// <summary>
        /// Removes all nodes from the specified node.
        /// </summary>
        /// <param name="node">The node to remove the nodes from.</param>
        private void Clear(BaseSceneTreeNode node)
        {
            foreach (BaseSceneTreeNode child in node.Nodes)
            {
                Clear(child);
            }
            node.Nodes.Clear();
            node.Dispose();
        }

        /// <summary>
        /// Creates the tree node with the specified scene content by the specified scene content type.
        /// </summary>
        /// <typeparam name="T">Type of the scene content.</typeparam>
        /// <param name="value">The scene content.</param>
        /// <returns>Created tree node.</returns>
        private BaseSceneTreeNode CreateNodeByType<T>(T value) where T : class, IName, IVisible
        {
            if (value.GetType() == typeof(Actor)) return new ActorSceneNode(this, value as Actor);
            else if (value.GetType() == typeof(Layer)) return new LayerSceneNode(this, value as Layer);
            else if (value.GetType() == typeof(Path)) return new PathSceneNode(this, value as Path);
            else return new SceneNode<T>(this, value);
        }

        /// <summary>
        /// Handles the ListChanged event of the <see cref="Scenes.Scene.Paths"/> at the scene.
        /// Updates paths of the scene in the tree.
        /// </summary>
        private void Paths_ListChanged(object sender, ObservableListChangedEventArgs<Path> e)
        {
            BasicListChanged(pathsNode, e);
        }

        /// <summary>
        /// Handles the ListChanged event of the <see cref="Scenes.Scene.Layers"/> at the scene.
        /// Updates layers of the scene in the tree.
        /// </summary>
        private void Layers_ListChanged(object sender, ObservableListChangedEventArgs<Layer> e)
        {
            BasicListChanged(layersNode, e);
        }

        /// <summary>
        /// Updates the scene content list that changes in the specified node.
        /// </summary>
        /// <typeparam name="T">Type of the scene content.</typeparam>
        private void BasicListChanged<T>(Node node, ObservableListChangedEventArgs<T> e) where T : class, IName, IVisible
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    node.Nodes.Insert(e.Index, CreateNodeByType(e.Item));
                    break;

                case ObservableListChangedType.ItemDeleted:
                    Clear((BaseSceneTreeNode)node.Nodes[e.Index]);
                    node.Nodes.RemoveAt(e.Index);
                    break;

                case ObservableListChangedType.ItemChanged:
                    ((BaseSceneTreeNode)node.Nodes[e.Index]).Dispose();
                    node.Nodes.RemoveAt(e.Index);
                    node.Nodes.Add(CreateNodeByType(e.Item));
                    break;

                case ObservableListChangedType.Reset:
                    foreach (BaseSceneTreeNode child in node.Nodes)
                    {
                        Clear(child);
                    }
                    node.Nodes.Clear();
                    break;
            }
        }

        /// <summary>
        /// Determines whether the nodeCheckBox control is visible for the specified node.
        /// </summary>
        private void nodeCheckBox_IsVisibleValueNeeded(object sender, NodeControlValueEventArgs e)
        {
            BaseSceneTreeNode node = e.Node.Tag as BaseSceneTreeNode;
            e.Value = node != null ? node.AllowVisible : false;
        }

        /// <summary>
        /// Handles the ItemDrag event of the treeView control.
        /// Begins a drag drop operation.
        /// </summary>
        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (treeView.SelectedNodes.Count == 1)
            {
                // actor can be dragged
                if (treeView.SelectedNode.Tag is SceneNode<Actor>)
                {
                    treeView.DoDragDropSelectedNodes(DragDropEffects.Move);
                }
                // path can be dragged
                else if (treeView.SelectedNode.Tag is SceneNode<Path>)
                {
                    treeView.DoDragDropSelectedNodes(DragDropEffects.Move);
                }
                // layer can be dragged
                else if (treeView.SelectedNode.Tag is SceneNode<Layer>)
                {
                    treeView.DoDragDropSelectedNodes(DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Handles the DragOver event of the treeView control.
        /// </summary>
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && treeView.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];

                if (nodes != null && nodes.Length == 1 && CanNodeBeDropped(nodes[0], treeView.DropPosition))
                {
                    e.Effect = e.AllowedEffect;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified dragged node can be dropped to the specified position.
        /// </summary>
        /// <param name="draggedNode">The dragged node.</param>
        /// <param name="dropPosition">The drop position.</param>
        /// <returns><c>true</c> if the dragged node can be dropped to the specified position; otherwise <c>false</c>.</returns>
        private bool CanNodeBeDropped(TreeNodeAdv draggedNode, DropPosition dropPosition)
        {
            if (draggedNode == dropPosition.Node) return false;
            else if (NodeContains((Node)draggedNode.Tag, (Node)dropPosition.Node.Tag)) return false;
            else if (dropPosition.Position == NodePosition.Inside)
            {
                if (draggedNode.Parent == dropPosition.Node) return true;
                else if (draggedNode.Tag is SceneNode<Actor> && dropPosition.Node.Tag is SceneNode<Layer>) return true;
                else if (draggedNode.Tag is SceneNode<Actor> && dropPosition.Node.Tag is SceneNode<Actor>)
                {
                    Actor dropInsideActor = ((SceneNode<Actor>)dropPosition.Node.Tag).Value;
                    if (dropInsideActor.Layer != null && dropInsideActor.Layer.IsParallax) return false;
                    else return true;
                }
            }
            else
            {
                if (draggedNode.Tag.GetType() == dropPosition.Node.Tag.GetType()) return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the children of the specified node contains the <paramref name="nodeToFound"/> node.
        /// </summary>
        private bool NodeContains(Node parentNode, Node nodeToFound)
        {
            foreach (Node child in parentNode.Nodes)
            {
                if (child == nodeToFound || NodeContains(child, nodeToFound))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles the DragDrop event of the treeView control.
        /// Makes the dropped operation, if possible.
        /// </summary>
        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && treeView.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];

                if (nodes != null && nodes.Length == 1 && CanNodeBeDropped(nodes[0], treeView.DropPosition))
                {
                    treeView.BeginUpdate();

                    // drop actor
                    if (nodes[0].Tag is SceneNode<Actor>)
                    {
                        Actor draggedActor = ((SceneNode<Actor>)nodes[0].Tag).Value;

                        // drop actor to layer
                        if (treeView.DropPosition.Node.Tag is SceneNode<Layer> && treeView.DropPosition.Position == NodePosition.Inside)
                        {
                            Layer destinationLayer = ((SceneNode<Layer>)treeView.DropPosition.Node.Tag).Value;

                            draggedActor.Remove();
                            destinationLayer.Add(draggedActor);
                        }
                        // drop actor within actors (in layer or actor children)
                        else if (treeView.DropPosition.Node.Tag is SceneNode<Actor> && treeView.DropPosition.Position != NodePosition.Inside)
                        {
                            Actor droppedNeighbourActor = ((SceneNode<Actor>)treeView.DropPosition.Node.Tag).Value;

                            int newIndex = 0;
                            if (treeView.DropPosition.Position == NodePosition.Before)
                            {
                                newIndex = treeView.DropPosition.Node.Index;
                            }
                            else if (treeView.DropPosition.Position == NodePosition.After)
                            {
                                newIndex = treeView.DropPosition.Node.Index + 1;
                            }

                            // drop actor to layer
                            if (droppedNeighbourActor.Parent == null)
                            {
                                // same layer
                                if (draggedActor.Layer == droppedNeighbourActor.Layer)
                                {
                                    if (newIndex > nodes[0].Index) newIndex--;
                                }

                                draggedActor.Remove();

                                if (newIndex <= droppedNeighbourActor.Layer.Count) droppedNeighbourActor.Layer.Insert(newIndex, draggedActor);
                                else droppedNeighbourActor.Layer.Add(draggedActor);
                            }
                            // drop actor to actor
                            else
                            {
                                // same parent
                                if (draggedActor.Parent == droppedNeighbourActor.Parent)
                                {
                                    if (newIndex > nodes[0].Index) newIndex--;
                                }

                                draggedActor.Remove();

                                if (newIndex <= droppedNeighbourActor.Parent.Children.Count) droppedNeighbourActor.Parent.Children.Insert(newIndex, draggedActor);
                                else droppedNeighbourActor.Parent.Children.Add(draggedActor);
                            }
                        }
                        // drop actor to actor (as child)
                        else if (treeView.DropPosition.Node.Tag is SceneNode<Actor> && treeView.DropPosition.Position == NodePosition.Inside)
                        {
                            Actor destinationActor = ((SceneNode<Actor>)treeView.DropPosition.Node.Tag).Value;

                            draggedActor.Remove();
                            destinationActor.Children.Add(draggedActor);
                        }
                    }
                    // drop path
                    else if (nodes[0].Tag is SceneNode<Path>)
                    {
                        Path draggedPath = ((SceneNode<Path>)nodes[0].Tag).Value;

                        // drop path to paths
                        if (treeView.DropPosition.Node.Tag == pathsNode && treeView.DropPosition.Position == NodePosition.Inside)
                        {
                            draggedPath.Remove();
                            Scene.Paths.Add(draggedPath);
                        }
                        // drop path to paths (within paths)
                        else if (treeView.DropPosition.Node.Tag is SceneNode<Path> && treeView.DropPosition.Position != NodePosition.Inside)
                        {
                            int newIndex = 0;
                            if (treeView.DropPosition.Position == NodePosition.Before)
                            {
                                newIndex = treeView.DropPosition.Node.Index;
                            }
                            else if (treeView.DropPosition.Position == NodePosition.After)
                            {
                                newIndex = treeView.DropPosition.Node.Index + 1;
                            }
                            if (newIndex > nodes[0].Index) newIndex--;

                            draggedPath.Remove();

                            if (newIndex <= Scene.Paths.Count) Scene.Paths.Insert(newIndex, draggedPath);
                            else Scene.Paths.Add(draggedPath);
                        }
                    }
                    // drop layer
                    else if (nodes[0].Tag is SceneNode<Layer>)
                    {
                        Layer draggedLayer = ((SceneNode<Layer>)nodes[0].Tag).Value;

                        // drop layer to layers
                        if (treeView.DropPosition.Node.Tag == layersNode && treeView.DropPosition.Position == NodePosition.Inside)
                        {
                            Scene.Layers.Remove(draggedLayer);
                            Scene.Layers.Add(draggedLayer);
                        }
                        // drop layer to layers (within layers)
                        else if (treeView.DropPosition.Node.Tag is SceneNode<Layer> && treeView.DropPosition.Position != NodePosition.Inside)
                        {
                            int newIndex = 0;
                            if (treeView.DropPosition.Position == NodePosition.Before)
                            {
                                newIndex = treeView.DropPosition.Node.Index;
                            }
                            else if (treeView.DropPosition.Position == NodePosition.After)
                            {
                                newIndex = treeView.DropPosition.Node.Index + 1;
                            }
                            if (newIndex > nodes[0].Index) newIndex--;

                            Scene.Layers.Remove(draggedLayer);

                            if (newIndex <= Scene.Layers.Count) Scene.Layers.Insert(newIndex, draggedLayer);
                            else Scene.Layers.Add(draggedLayer);
                        }
                    }

                    treeView.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the treeView control.
        /// Shows context menu of the selected item, if right mouse is down.
        /// </summary>
        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && treeView.SelectedNode != null)
            {
                // actor menu
                if (treeView.SelectedNode.Tag is SceneNode<Actor>)
                {
                    actorNodeMenuStrip.Show(this, e.Location);
                }
                // layer menu
                else if (treeView.SelectedNode.Tag is SceneNode<Layer>)
                {
                    // layer parallax
                    if (((SceneNode<Layer>)treeView.SelectedNode.Tag).Value.IsParallax) layerParallaxMenuStrip.Show(this, e.Location);
                    // layer
                    else layerMenuStrip.Show(this, e.Location);
                }
                // path menu
                else if (treeView.SelectedNode.Tag is SceneNode<Path>)
                {
                    pathMenuStrip.Show(this, e.Location);
                }
                // layers menu
                else if (treeView.SelectedNode.Tag == layersNode)
                {
                    layersMenuStrip.Show(this, e.Location);
                }
                // paths menu
                else if (treeView.SelectedNode.Tag == pathsNode)
                {
                    pathsMenuStrip.Show(this, e.Location);
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the treeView control.
        /// Delete - Remove the selected item.
        /// </summary>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeView.SelectedNode != null)
            {
                DeleteNode(treeView.SelectedNode);
            }
        }

        /// <summary>
        /// Removed the specified node from the tree.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        private void DeleteNode(TreeNodeAdv node)
        {
            if (treeView.SelectedNode.Tag is SceneNode<Actor>)
            {
                if (new ConsistentDeletionForm(new ConsistentDeletionHelper.ActorForDeletion(((SceneNode<Actor>)treeView.SelectedNode.Tag).Value)).ShowDialog() == DialogResult.OK)
                {
                    Messages.ShowInfo("Actor deleted.");
                }
            }
            else if (treeView.SelectedNode.Tag is SceneNode<Path>)
            {
                if (new ConsistentDeletionForm(new ConsistentDeletionHelper.PathForDeletion(((SceneNode<Path>)treeView.SelectedNode.Tag).Value)).ShowDialog() == DialogResult.OK)
                {
                    Messages.ShowInfo("Path deleted.");
                }
            }
            else if (treeView.SelectedNode.Tag is SceneNode<Layer>)
            {
                Layer deletedLayer = ((SceneNode<Layer>)treeView.SelectedNode.Tag).Value;
                List<ItemForDeletion> itemsForDeletion = new List<ItemForDeletion>();
                foreach (Actor actor in deletedLayer)
                {
                    itemsForDeletion.Add(new ConsistentDeletionHelper.ActorForDeletion(actor));
                }

                if (new ConsistentDeletionForm(itemsForDeletion).ShowDialog() == DialogResult.OK)
                {
                    if (deletedLayer.Scene.SelectedLayer == deletedLayer) deletedLayer.Scene.SelectedLayer = null;
                    deletedLayer.Scene.Layers.Remove(deletedLayer);
                    Messages.ShowInfo("Layer deleted.");
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteToolStripMenuItem control.
        /// Removes the selected item.
        /// </summary>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                DeleteNode(treeView.SelectedNode);
            }
        }

        /// <summary>
        /// Handles the Click event of the addLayerToolStripMenuItem control.
        /// Adds new layer to the scene.
        /// </summary>
        private void addLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scene.Layers.Insert(0, new Layer(Scene) { Name = "New Layer" });
        }

        /// <summary>
        /// Handles the Click event of the addParallaxLayerToolStripMenuItem control.
        /// Adds new parallax layer to the scene.
        /// </summary>
        private void addParallaxLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scene.Layers.Insert(0, new Layer(Scene, true) { Name = "New Parallax Layer" });
        }

        /// <summary>
        /// Handles the Click event of the parallaxSettingsToolStripMenuItem control.
        /// Opens the form for editing the parallax layer settings.
        /// </summary>
        private void parallaxSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Tag is SceneNode<Layer>)
            {
                new LayerParallaxSettings(((SceneNode<Layer>)treeView.SelectedNode.Tag).Value).ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of the renameToolStripMenuItem control.
        /// Begins renaming the selected item.
        /// </summary>
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                nodeTextBox.BeginEdit();
            }
        }

        /// <summary>
        /// Handles the Click event of the addPathToolStripPathsMenuItem control.
        /// Adds new path to the scene.
        /// </summary>
        private void addPathToolStripPathsMenuItem_Click(object sender, EventArgs e)
        {
            EditorApplicationForm.SceneSingleton.State = new AddPathSceneState(EditorApplicationForm.SceneSingleton.State);
        }

        /// <summary>
        /// Handles the Click event of the showToolStripMenuItem control.
        /// Centers the selected item at the scene.
        /// </summary>
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode.Tag is SceneNode<Actor>)
            {
                EditorApplicationForm.SceneSingleton.Center(((SceneNode<Actor>)treeView.SelectedNode.Tag).Value);
            }
            else if (treeView.SelectedNode != null && treeView.SelectedNode.Tag is SceneNode<Path>)
            {
                EditorApplicationForm.SceneSingleton.Center(((SceneNode<Path>)treeView.SelectedNode.Tag).Value);
            }
        }

        /// <summary>
        /// Handles the Click event of the selectToolStripMenuItem control.
        /// Set the selected layer as the selected layer of the scene.
        /// </summary>
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Tag is SceneNode<Layer>)
            {
                Scene.SelectedLayer = ((SceneNode<Layer>)treeView.SelectedNode.Tag).Value;
            }
        }

        /// <summary>
        /// Called when the selected layer of the scene changes.
        /// Invalidates the tree view.
        /// </summary>
        private void Scene_SelectedLayerChanged(object sender, EventArgs e)
        {
            treeView.Invalidate(false);
        }

        /// <summary>
        /// Handles the DrawText event of the nodeTextBox control.
        /// Changes font (make bold) of the selected layer of the scene.
        /// </summary>
        private void nodeTextBox_DrawText(object sender, DrawEventArgs e)
        {
            if (e.Node.Tag is SceneNode<Layer> && Scene.SelectedLayer == ((SceneNode<Layer>)e.Node.Tag).Value)
            {
                e.Font = new Font(e.Font, FontStyle.Bold);
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the treeView control.
        /// Centers the selected item at the scene or set the selected layer as the selected layer of the scene.
        /// </summary>
        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && treeView.SelectedNode != null && e.Clicks == 2)
            {
                // doubleClick on actor
                if (treeView.SelectedNode.Tag is SceneNode<Actor>)
                {
                    showToolStripMenuItem_Click(null, null);
                }
                // doubleClick on path
                else if (treeView.SelectedNode.Tag is SceneNode<Path>)
                {
                    showToolStripMenuItem_Click(null, null);
                }
                // doubleClick on layer
                else if (treeView.SelectedNode.Tag is SceneNode<Layer>)
                {
                    selectToolStripMenuItem_Click(null, null);
                }
            }
        }
    }
}
