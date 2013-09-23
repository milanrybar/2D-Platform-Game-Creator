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
    /// View of list of <see cref="Actor">protypes</see>.
    /// Everything is done automatically by setting container (<see cref="Prototypes"/>) of prototypes.
    /// </summary>
    partial class ActorPrototypesView : UserControl
    {
        /// <summary>
        /// Gets or sets the container of prototypes.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableList<Actor> Prototypes
        {
            get { return _prototypes; }
            set
            {
                if (_prototypes != null)
                {
                    _prototypes.ListChanged -= new ObservableList<Actor>.ListChangedEventHandler(Prototypes_ListChanged);
                    listView.Clear();
                }

                _prototypes = value;

                if (_prototypes != null)
                {
                    _prototypes.ListChanged += new ObservableList<Actor>.ListChangedEventHandler(Prototypes_ListChanged);

                    foreach (Actor prototype in Prototypes)
                    {
                        ShowItem(prototype);
                    }
                }
            }
        }
        private ObservableList<Actor> _prototypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorPrototypesView"/> class.
        /// </summary>
        public ActorPrototypesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the container of prototypes changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;Actor&gt;"/> instance containing the event data.</param>
        private void Prototypes_ListChanged(object sender, ObservableListChangedEventArgs<Actor> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowItem(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    ListViewItem deletedItem = FindItem(e.Item);
                    if (deletedItem != null) deletedItem.Remove();
                    break;

                case ObservableListChangedType.ItemChanged:
                    Debug.Assert(true, "Operation not supported.");
                    break;

                case ObservableListChangedType.Reset:
                    listView.Clear();
                    foreach (Actor prototype in Prototypes)
                    {
                        ShowItem(prototype);
                    }
                    break;
            }
        }

        /// <summary>
        /// Shows the specified actor in the ListView.
        /// </summary>
        /// <param name="actor">The actor to show.</param>
        private void ShowItem(Actor actor)
        {
            listView.Items.Add(new ListViewItem(actor.Name) { Tag = actor });
        }

        /// <summary>
        /// Finds the actor in the ListView.
        /// </summary>
        /// <param name="actor">The actor to find.</param>
        /// <returns>Item of ListView if found; otherwise null.</returns>
        private ListViewItem FindItem(Actor actor)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Tag == actor) return item;
            }

            return null;
        }

        /// <summary>
        /// Removes the specified actor.
        /// </summary>
        /// <param name="actor">The actor.</param>
        private void RemoveItem(Actor actor)
        {
            Prototypes.Remove(actor);
            Messages.ShowInfo("Prototype deleted.");
        }

        /// <summary>
        /// Handles the ItemDrag event of the listView control.
        /// Begins drag drop operation.
        /// </summary>
        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item != null && e.Item is ListViewItem)
            {
                listView.DoDragDrop(((ListViewItem)e.Item).Tag, DragDropEffects.All);
            }
        }

        /// <summary>
        /// Handles the MouseClick event of the listView control.
        /// Shows context menu of the selected item, if right mouse is down.
        /// </summary>
        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                itemContextMenuStrip.Show(this, e.Location);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the listView control.
        /// Delete - Remove the selected item.
        /// </summary>
        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                RemoveItem((Actor)listView.SelectedItems[0].Tag);
            }
        }

        /// <summary>
        /// Handles the AfterLabelEdit event of the listView control.
        /// Sets new name to the edited prototype.
        /// </summary>
        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            Actor actor = listView.Items[e.Item].Tag as Actor;

            if (e.Label != null && actor.Name != e.Label)
            {
                if (e.Label == String.Empty)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    actor.Name = e.Label;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the renameToolStripMenuItem control.
        /// Begins renaming the selected item.
        /// </summary>
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                listView.SelectedItems[0].BeginEdit();
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteToolStripMenuItem control.
        /// Removes the selected item.
        /// </summary>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                RemoveItem((Actor)listView.SelectedItems[0].Tag);
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
                Prototypes = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
