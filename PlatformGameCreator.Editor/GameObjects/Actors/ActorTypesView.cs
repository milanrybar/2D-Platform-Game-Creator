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

namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    /// <summary>
    /// View of list of <see cref="ActorType">actor types</see>.
    /// Everything is done automatically by setting manager (<see cref="ActorTypes"/>) of actor types.
    /// </summary>
    partial class ActorTypesView : UserControl
    {
        /// <summary>
        /// Item of a actor type for using in the <see cref="TreeView"/>.
        /// </summary>
        private class ActorTypeTreeNode : TreeNode
        {
            private ActorType actorType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActorTypeTreeNode"/> class.
            /// </summary>
            /// <param name="actorType">Type of the actor.</param>
            public ActorTypeTreeNode(ActorType actorType)
                : base(actorType.Name)
            {
                this.actorType = actorType;
                Tag = actorType;

                actorType.NameChanged += new EventHandler(actorType_NameChanged);
                actorType.Children.ListChanged += new ObservableList<ActorType>.ListChangedEventHandler(Children_ListChanged);

                foreach (ActorType child in actorType.Children)
                {
                    ShowItem(child);
                }
            }

            /// <summary>
            /// Removes the current tree node from the tree view control.
            /// </summary>
            public new void Remove()
            {
                if (actorType != null)
                {
                    actorType.NameChanged -= new EventHandler(actorType_NameChanged);
                    actorType.Children.ListChanged -= new ObservableList<ActorType>.ListChangedEventHandler(Children_ListChanged);

                    ClearChildren();

                    actorType = null;
                }

                base.Remove();
            }

            /// <summary>
            /// Handles the <see cref="ActorType.NameChanged"/> event of the actor type.
            /// Updates the name of the actor type in the TreeView.
            /// </summary>
            private void actorType_NameChanged(object sender, EventArgs e)
            {
                if (actorType.Name != Text)
                {
                    Text = actorType.Name;
                }
            }

            /// <summary>
            /// Called when the children of the actor types changes.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;ActorType&gt;"/> instance containing the event data.</param>
            private void Children_ListChanged(object sender, ObservableListChangedEventArgs<ActorType> e)
            {
                switch (e.ListChangedType)
                {
                    case ObservableListChangedType.ItemAdded:
                        ShowItem(e.Item);
                        break;

                    case ObservableListChangedType.ItemDeleted:
                        ActorTypeTreeNode deletedItem = FindItem(e.Item);
                        if (deletedItem != null) deletedItem.Remove();
                        break;

                    case ObservableListChangedType.ItemChanged:
                        Debug.Assert(true, "Not supported operation.");
                        break;

                    case ObservableListChangedType.Reset:
                        ClearChildren();
                        foreach (ActorType child in actorType.Children)
                        {
                            ShowItem(child);
                        }
                        break;
                }
            }

            /// <summary>
            /// Shows the specified actor type in the TreeView.
            /// </summary>
            /// <param name="actorType">The actor type to show.</param>
            private void ShowItem(ActorType actorType)
            {
                Nodes.Add(new ActorTypeTreeNode(actorType));
            }

            /// <summary>
            /// Finds the actor type in the TreeNode.
            /// </summary>
            /// <param name="actorType">The actor type to find.</param>
            /// <returns>Item of TreeNode if found; otherwise null.</returns>
            public ActorTypeTreeNode FindItem(ActorType actorType)
            {
                foreach (TreeNode treeNode in Nodes)
                {
                    if (treeNode.Tag == actorType)
                    {
                        return (ActorTypeTreeNode)treeNode;
                    }
                }

                return null;
            }

            /// <summary>
            /// Removed all children of the actor types from the TreeView.
            /// </summary>
            public void ClearChildren()
            {
                while (Nodes.Count > 0)
                {
                    ((ActorTypeTreeNode)Nodes[0]).Remove();
                }
            }
        }

        /// <summary>
        /// Gets or sets the manager of actor types.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActorTypesManager ActorTypes
        {
            get { return _actorTypes; }
            set
            {
                if (_actorTypes != null)
                {
                    ((ActorTypeTreeNode)treeView.Nodes[0]).Remove();
                }

                _actorTypes = value;

                if (_actorTypes != null)
                {
                    treeView.Nodes.Add(new ActorTypeTreeNode(_actorTypes.Root));
                    treeView.ExpandAll();
                }
            }
        }
        private ActorTypesManager _actorTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTypesView"/> class.
        /// </summary>
        public ActorTypesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the addToolStripMenuItem control.
        /// Creates new actor type to the children of the selected item.
        /// </summary>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                ActorType selectedActorType = (ActorType)treeView.SelectedNode.Tag;

                ActorType newActorType = new ActorType(selectedActorType.ActorTypes) { Name = "New Actor Type" };
                selectedActorType.Children.Add(newActorType);

                treeView.ExpandAll();
                ActorTypeTreeNode newTreeNode = ((ActorTypeTreeNode)treeView.SelectedNode).FindItem(newActorType);
                treeView.SelectedNode = newTreeNode;
                newTreeNode.BeginEdit();
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
                RemoveItem((ActorType)treeView.SelectedNode.Tag);
            }
        }

        /// <summary>
        /// Removes the specified actor type.
        /// </summary>
        /// <param name="actorType">The actor type.</param>
        private void RemoveItem(ActorType actorType)
        {
            if (actorType.Level > 0)
            {
                List<ItemForDeletion> actorTypesToDelete = new List<ItemForDeletion>();
                PrepareItemsToDelete(actorType, actorTypesToDelete);

                if (new ConsistentDeletionForm(actorTypesToDelete).ShowDialog() == DialogResult.OK)
                {
                    Messages.ShowInfo("Actor Type deleted.");
                }
            }
            else
            {
                Messages.ShowWarning("Root actor type cannot be deleted.");
            }
        }

        /// <summary>
        /// Creates list of <see cref="ItemForDeletion"/> for the specified actor type and its children.
        /// </summary>
        /// <param name="actorType">Actor type to delete.</param>
        /// <param name="items">List of <see cref="ItemForDeletion"/> for deleting.</param>
        private void PrepareItemsToDelete(ActorType actorType, List<ItemForDeletion> items)
        {
            items.Add(new ConsistentDeletionHelper.ActorTypeForDeletion(actorType));

            foreach (ActorType child in actorType.Children)
            {
                PrepareItemsToDelete(child, items);
            }
        }

        /// <summary>
        /// Handles the AfterLabelEdit event of the treeView control.
        /// Sets new name to the edited actor type.
        /// </summary>
        private void actorTypeTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ActorType actorType = e.Node.Tag as ActorType;

            if (e.Label != null && actorType.Name != e.Label)
            {
                if (e.Label == String.Empty)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    actorType.Name = e.Label;
                }
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the treeView control.
        /// Shows context menu of the selected item, if right mouse is down.
        /// </summary>
        private void actorTypeTreeView_MouseDown(object sender, MouseEventArgs e)
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
        /// Handles the KeyDown event of the treeView control.
        /// Delete - Remove the selected item.
        /// </summary>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeView.SelectedNode != null)
            {
                RemoveItem((ActorType)treeView.SelectedNode.Tag);
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
                ActorTypes = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
