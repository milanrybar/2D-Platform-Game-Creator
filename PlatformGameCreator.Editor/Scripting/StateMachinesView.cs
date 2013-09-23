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

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// View of list of <see cref="StateMachine">state machines</see>.
    /// Everything is done automatically by setting list (<see cref="StateMachinesView.StateMachines"/>) of state machines.
    /// </summary>
    partial class StateMachinesView : UserControl
    {
        /// <summary>
        /// Item of a state machine for using in the <see cref="TreeView"/>.
        /// </summary>
        private class StateMachineTreeNode : TreeNode
        {
            private StateMachine stateMachine;

            /// <summary>
            /// Initializes a new instance of the <see cref="StateMachineTreeNode"/> class.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            public StateMachineTreeNode(StateMachine stateMachine)
                : base(stateMachine.Name)
            {
                this.stateMachine = stateMachine;
                Tag = stateMachine;
                stateMachine.NameChanged += new EventHandler(stateMachine_NameChanged);
            }

            /// <summary>
            /// Removes the current tree node from the tree view control.
            /// </summary>
            public new void Remove()
            {
                if (stateMachine != null)
                {
                    stateMachine.NameChanged -= new EventHandler(stateMachine_NameChanged);
                    stateMachine = null;
                }

                base.Remove();
            }

            /// <summary>
            /// Called when the name of the state machine changes.
            /// </summary>
            private void stateMachine_NameChanged(object sender, EventArgs e)
            {
                if (stateMachine.Name != Text)
                {
                    Text = stateMachine.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of state machines.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableList<StateMachine> StateMachines
        {
            get { return _stateMachines; }
            set
            {
                if (_stateMachines != null)
                {
                    TreeViewClear();

                    _stateMachines.ListChanged -= new ObservableList<StateMachine>.ListChangedEventHandler(StateMachines_ListChanged);
                }

                _stateMachines = value;

                if (_stateMachines != null)
                {
                    _stateMachines.ListChanged += new ObservableList<StateMachine>.ListChangedEventHandler(StateMachines_ListChanged);

                    foreach (StateMachine stateMachine in _stateMachines)
                    {
                        ShowItem(stateMachine);
                    }
                }
            }
        }
        private ObservableList<StateMachine> _stateMachines;

        /// <summary>
        /// Represents operation for the specified state machine.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="stateMachine">The state machine.</param>
        public delegate void StateMachineHandler(object sender, StateMachine stateMachine);

        /// <summary>
        /// Occurs when a state machine wants to be opened.
        /// </summary>
        public event StateMachineHandler OpenStateMachine;

        /// <summary>
        /// Occurs after a state machine is deleted.
        /// </summary>
        public event StateMachineHandler AfterDeletingStateMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachinesView"/> class.
        /// </summary>
        public StateMachinesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified state machine in the TreeView.
        /// </summary>
        /// <param name="stateMachine">The state machine to show.</param>
        private void ShowItem(StateMachine stateMachine)
        {
            treeView.Nodes.Add(new StateMachineTreeNode(stateMachine));
        }

        /// <summary>
        /// Finds the state machine in the TreeNode.
        /// </summary>
        /// <param name="stateMachine">The state machine to find.</param>
        /// <returns>Item of TreeNode if found; otherwise null.</returns>
        private StateMachineTreeNode FindItem(StateMachine stateMachine)
        {
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                if (treeNode.Tag == stateMachine)
                {
                    return (StateMachineTreeNode)treeNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Called when the list of state machines changes.
        /// </summary>
        private void StateMachines_ListChanged(object sender, ObservableListChangedEventArgs<StateMachine> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowItem(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    StateMachineTreeNode deletedItem = FindItem(e.Item);
                    if (deletedItem != null) deletedItem.Remove();
                    break;

                case ObservableListChangedType.ItemChanged:
                    Debug.Assert(true, "Not supported operation.");
                    break;

                case ObservableListChangedType.Reset:
                    TreeViewClear();
                    foreach (StateMachine stateMachine in StateMachines)
                    {
                        ShowItem(stateMachine);
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
                ((StateMachineTreeNode)treeView.Nodes[0]).Remove();
            }
        }

        /// <summary>
        /// The specified state machine wants to be opened. Fires <see cref="OpenStateMachine"/> event.
        /// </summary>
        /// <param name="stateMachine">The state machine.</param>
        private void OpenItem(StateMachine stateMachine)
        {
            if (OpenStateMachine != null) OpenStateMachine(this, stateMachine);
        }

        /// <summary>
        /// Removes the specified state machine, if possible.
        /// </summary>
        /// <param name="stateMachine">The state machine.</param>
        private void RemoveItem(StateMachine stateMachine)
        {
            if (StateMachines.Count <= 1)
            {
                Messages.ShowWarning("State machine cannot be removed. At least one state machine must exist.");
                return;
            }

            if (MessageBox.Show("Are you sure?", "Delete State Machine", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                StateMachines.Remove(stateMachine);
                Messages.ShowInfo("State machine deleted.");

                if (AfterDeletingStateMachine != null) AfterDeletingStateMachine(this, stateMachine);
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
        /// The specified state machine will be shown, if left mouse is down.
        /// </summary>
        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && treeView.SelectedNode != null)
            {
                TreeNode node = treeView.GetNodeAt(e.Location);
                if (node != null && treeView.SelectedNode == node)
                {
                    OpenItem((StateMachine)treeView.SelectedNode.Tag);
                }
            }
        }

        /// <summary>
        /// Handles the AfterLabelEdit event of the treeView control.
        /// Sets new name to the edited state machine.
        /// </summary>
        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            StateMachine stateMachine = e.Node.Tag as StateMachine;

            if (e.Label != null && stateMachine.Name != e.Label)
            {
                if (e.Label == String.Empty)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    stateMachine.Name = e.Label;
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the treeView control.
        /// Delete - Remove the selected item.
        /// Enter - Show the selected item.
        /// </summary>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeView.SelectedNode != null)
            {
                RemoveItem((StateMachine)treeView.SelectedNode.Tag);
            }
            else if (e.KeyCode == Keys.Enter && treeView.SelectedNode != null)
            {
                OpenItem((StateMachine)treeView.SelectedNode.Tag);
            }
        }

        /// <summary>
        /// Handles the Click event of the openToolStripMenuItem control.
        /// The specified state machine wants to be opened.
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                OpenItem((StateMachine)treeView.SelectedNode.Tag);
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
                RemoveItem((StateMachine)treeView.SelectedNode.Tag);
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
                StateMachines = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
