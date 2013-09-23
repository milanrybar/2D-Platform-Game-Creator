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

namespace PlatformGameCreator.Editor.Assets.Sounds
{
    /// <summary>
    /// View of list of <see cref="Sound">sounds</see>.
    /// Everything is done automatically by setting manager (<see cref="SoundsView.Sounds"/>) of sounds.
    /// </summary>
    partial class SoundsView : UserControl
    {
        /// <summary>
        /// Item of a sound for using in the <see cref="TreeView"/>.
        /// </summary>
        private class SoundTreeNode : TreeNode
        {
            private Sound sound;

            /// <summary>
            /// Initializes a new instance of the <see cref="SoundTreeNode"/> class.
            /// </summary>
            /// <param name="sound">The sound.</param>
            public SoundTreeNode(Sound sound)
                : base(sound.Name)
            {
                this.sound = sound;
                Tag = sound;
                sound.NameChanged += new EventHandler(sound_NameChanged);
            }

            /// <summary>
            /// Removes the current tree node from the tree view control.
            /// </summary>
            public new void Remove()
            {
                if (sound != null)
                {
                    sound.NameChanged -= new EventHandler(sound_NameChanged);
                    sound = null;
                }

                base.Remove();
            }

            /// <summary>
            /// Called when the name of the sound changes.
            /// </summary>
            private void sound_NameChanged(object sender, EventArgs e)
            {
                if (sound.Name != Text)
                {
                    Text = sound.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets the manager of sounds.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SoundsManager Sounds
        {
            get { return _sounds; }
            set
            {
                if (_sounds != value)
                {
                    if (_sounds != null)
                    {
                        TreeViewClear();

                        _sounds.ListChanged -= new Common.ObservableList<Sound>.ListChangedEventHandler(Sounds_ListChanged);
                    }

                    _sounds = value;

                    if (_sounds != null)
                    {
                        _sounds.ListChanged += new Common.ObservableList<Sound>.ListChangedEventHandler(Sounds_ListChanged);

                        foreach (Sound sound in _sounds)
                        {
                            ShowItem(sound);
                        }
                    }
                }
            }
        }
        private SoundsManager _sounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundsView"/> class.
        /// </summary>
        public SoundsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified sound in the TreeView.
        /// </summary>
        /// <param name="sound">The sound to show.</param>
        private void ShowItem(Sound sound)
        {
            treeView.Nodes.Add(new SoundTreeNode(sound));
        }

        /// <summary>
        /// Finds the sound in the TreeNode.
        /// </summary>
        /// <param name="sound">The sound to find.</param>
        /// <returns>Item of TreeNode if found; otherwise null.</returns>
        private SoundTreeNode FindItem(Sound sound)
        {
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                if (treeNode.Tag == sound)
                {
                    return (SoundTreeNode)treeNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Called when the manager of sounds changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;Sound&gt;"/> instance containing the event data.</param>
        private void Sounds_ListChanged(object sender, ObservableListChangedEventArgs<Sound> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowItem(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    SoundTreeNode deletedItem = FindItem(e.Item);
                    if (deletedItem != null) deletedItem.Remove();
                    break;

                case ObservableListChangedType.ItemChanged:
                    Debug.Assert(true, "Not supported operation.");
                    break;

                case ObservableListChangedType.Reset:
                    TreeViewClear();
                    foreach (Sound sound in Sounds)
                    {
                        ShowItem(sound);
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
                ((SoundTreeNode)treeView.Nodes[0]).Remove();
            }
        }

        /// <summary>
        /// Opens editor (<see cref="SoundForm"/>) for the specified sound.
        /// </summary>
        /// <param name="sound">The sound.</param>
        private void OpenItem(Sound sound)
        {
            new SoundForm(sound).ShowDialog();
        }

        /// <summary>
        /// Removes the specified sound.
        /// </summary>
        /// <param name="sound">The sound.</param>
        private void RemoveItem(Sound sound)
        {
            if (new ConsistentDeletionForm(new ConsistentDeletionHelper.SoundForDeletion(sound)).ShowDialog() == DialogResult.OK)
            {
                Messages.ShowInfo("Sound deleted.");
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
        /// Opens editor (<see cref="SoundForm"/>) for the specified sound, if left mouse is down.
        /// </summary>
        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && treeView.SelectedNode != null)
            {
                TreeNode node = treeView.GetNodeAt(e.Location);
                if (node != null && treeView.SelectedNode == node)
                {
                    OpenItem((Sound)treeView.SelectedNode.Tag);
                }
            }
        }

        /// <summary>
        /// Handles the AfterLabelEdit event of the treeView control.
        /// Sets new name to the edited sound.
        /// </summary>
        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            Sound sound = e.Node.Tag as Sound;

            if (e.Label != null && sound.Name != e.Label)
            {
                if (e.Label == String.Empty)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    sound.Name = e.Label;
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the treeView control.
        /// Delete - Remove the selected item.
        /// Enter - Open editor for the selected item.
        /// </summary>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeView.SelectedNode != null)
            {
                RemoveItem((Sound)treeView.SelectedNode.Tag);
            }
            else if (e.KeyCode == Keys.Enter && treeView.SelectedNode != null)
            {
                OpenItem((Sound)treeView.SelectedNode.Tag);
            }
        }

        /// <summary>
        /// Handles the Click event of the openToolStripMenuItem control.
        /// Opens editor (<see cref="SoundForm"/>) for the selected item.
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                OpenItem((Sound)treeView.SelectedNode.Tag);
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
                RemoveItem((Sound)treeView.SelectedNode.Tag);
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
                Sounds = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
