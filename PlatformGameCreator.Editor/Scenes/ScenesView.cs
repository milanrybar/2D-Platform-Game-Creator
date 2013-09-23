/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// View of list of <see cref="Scene">scenes</see>.
    /// Everything is done automatically by setting manager (<see cref="ScenesView.Scenes"/>) of scenes.
    /// </summary>
    partial class ScenesView : UserControl
    {
        /// <summary>
        /// Item of a scene for using in the <see cref="TreeView"/>.
        /// </summary>
        private class SceneTreeNode : TreeNode
        {
            private Scene scene;

            /// <summary>
            /// Initializes a new instance of the <see cref="SceneTreeNode"/> class.
            /// </summary>
            /// <param name="scene">The scene.</param>
            public SceneTreeNode(Scene scene)
                : base(scene.Name)
            {
                this.scene = scene;
                Tag = scene;
                scene.NameChanged += new EventHandler(scene_NameChanged);
            }

            /// <summary>
            /// Removes the current tree node from the tree view control.
            /// </summary>
            public new void Remove()
            {
                if (scene != null)
                {
                    scene.NameChanged -= new EventHandler(scene_NameChanged);
                    scene = null;
                }

                base.Remove();
            }

            /// <summary>
            /// Called when the name of the scene changes.
            /// </summary>
            private void scene_NameChanged(object sender, EventArgs e)
            {
                if (scene.Name != Text)
                {
                    Text = scene.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets the manager of scenes.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ScenesManager Scenes
        {
            get { return _scenes; }
            set
            {
                if (_scenes != null)
                {
                    TreeViewClear();

                    _scenes.ListChanged -= new ObservableList<Scene>.ListChangedEventHandler(Scenes_ListChanged);
                }

                _scenes = value;

                if (_scenes != null)
                {
                    _scenes.ListChanged += new ObservableList<Scene>.ListChangedEventHandler(Scenes_ListChanged);

                    foreach (Scene scene in _scenes)
                    {
                        ShowItem(scene);
                    }
                }
            }
        }
        private ScenesManager _scenes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenesView"/> class.
        /// </summary>
        public ScenesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified scene in the TreeView.
        /// </summary>
        /// <param name="scene">The scene to show.</param>
        private void ShowItem(Scene scene)
        {
            treeView.Nodes.Add(new SceneTreeNode(scene));
        }

        /// <summary>
        /// Finds the scene in the TreeNode.
        /// </summary>
        /// <param name="scene">The scene to find.</param>
        /// <returns>Item of TreeNode if found; otherwise null.</returns>
        private SceneTreeNode FindItem(Scene scene)
        {
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                if (treeNode.Tag == scene)
                {
                    return (SceneTreeNode)treeNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Called when the manager of scenes changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;Scene&gt;"/> instance containing the event data.</param>
        private void Scenes_ListChanged(object sender, ObservableListChangedEventArgs<Scene> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowItem(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    SceneTreeNode deletedItem = FindItem(e.Item);
                    if (deletedItem != null) deletedItem.Remove();
                    break;

                case ObservableListChangedType.ItemChanged:
                    Debug.Assert(true, "Not supported operation.");
                    break;

                case ObservableListChangedType.Reset:
                    TreeViewClear();
                    foreach (Scene scene in Scenes)
                    {
                        ShowItem(scene);
                    }
                    break;
            }
        }

        /// <summary>
        /// Removes all items from TreeView.
        /// </summary>
        private void TreeViewClear()
        {
            while (treeView.Nodes.Count > 0)
            {
                ((SceneTreeNode)treeView.Nodes[0]).Remove();
            }
        }

        /// <summary>
        /// Selects the scene as the selected scene of the <see cref="Scenes"/>.
        /// </summary>
        /// <param name="scene">The scene.</param>
        private void OpenItem(Scene scene)
        {
            Scenes.SelectedScene = scene;
        }

        /// <summary>
        /// Removes the specified scene.
        /// At least one scene must exist.
        /// </summary>
        /// <param name="scene">The scene.</param>
        private void RemoveItem(Scene scene)
        {
            if (Scenes.Count <= 1)
            {
                Messages.ShowWarning("Scene cannot be removed. At least one scene must exist.");
                return;
            }

            if (new ConsistentDeletionForm(new ConsistentDeletionHelper.SceneForDeletion(scene)).ShowDialog() == DialogResult.OK)
            {
                if (Scenes.SelectedScene == scene)
                {
                    Scenes.SelectedScene = Scenes[0];
                }

                Messages.ShowInfo("Scene deleted.");
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the treeView control.
        /// Shows context menu of the selected item, if right mouse is down.
        /// </summary>
        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView.GetNodeAt(e.Location);
                if (node != null)
                {
                    treeView.SelectedNode = node;
                    itemContextMenuStrip.Show(this, e.Location);
                }
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the treeView control.
        /// Selects the specified scene as the selected scene of the <see cref="Scenes"/>.
        /// </summary>
        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && treeView.SelectedNode != null)
            {
                TreeNode node = treeView.GetNodeAt(e.Location);
                if (node != null && treeView.SelectedNode == node)
                {
                    OpenItem((Scene)treeView.SelectedNode.Tag);
                }
            }
        }

        /// <summary>
        /// Handles the AfterLabelEdit event of the treeView control.
        /// Sets new name to the edited scene.
        /// </summary>
        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            Scene scene = e.Node.Tag as Scene;

            if (e.Label != null && scene.Name != e.Label)
            {
                if (e.Label == String.Empty)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    scene.Name = e.Label;
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the treeView control.
        /// Delete - Remove the selected item.
        /// Enter - Selects the specified scene as the selected scene of the <see cref="Scenes"/>.
        /// </summary>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeView.SelectedNode != null)
            {
                RemoveItem((Scene)treeView.SelectedNode.Tag);
            }
            else if (e.KeyCode == Keys.Enter && treeView.SelectedNode != null)
            {
                OpenItem((Scene)treeView.SelectedNode.Tag);
            }
        }

        /// <summary>
        /// Handles the Click event of the openToolStripMenuItem control.
        /// Selects the specified scene as the selected scene of the <see cref="Scenes"/>.
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                OpenItem((Scene)treeView.SelectedNode.Tag);
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
                treeView.SelectedNode.BeginEdit();
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
                RemoveItem((Scene)treeView.SelectedNode.Tag);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Scenes = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
