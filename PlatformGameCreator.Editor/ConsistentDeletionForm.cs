/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;
using System.Collections.ObjectModel;
using Aga.Controls.Tree.NodeControls;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Represents the item on the path of the object that is for deletion.
    /// </summary>
    struct ItemOnPath
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name;

        /// <summary>
        /// Type of the item as string.
        /// </summary>
        public string Type;

        /// <summary>
        /// Object representing this unique item.
        /// </summary>
        public object Tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemOnPath"/> struct.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="type">The type of the item.</param>
        /// <param name="tag">Object representing this unique item.</param>
        public ItemOnPath(string name, string type, object tag)
        {
            Name = name;
            Type = type;
            Tag = tag;
        }
    }

    /// <summary>
    /// Form for safe deleting items from the project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// We only set items that we want to remove from the project.
    /// Form shows items that need to be also deleted and offers the option to choose what to do with an item.
    /// </para>
    /// <para>
    /// Safe deleting can have more steps. 
    /// For example: we want to remove a texture, texture is used at the animation, 
    /// we choose that the animation should be removed from the project, animation is used at scripting, 
    /// more other items are shown, etc.
    /// </para>
    /// </remarks>
    partial class ConsistentDeletionForm : Form
    {
        /// <summary>
        /// Tree node of an object to delete for using in the tree.
        /// </summary>
        private class ItemForDeletionTreeNode : Node
        {
            /// <summary>
            /// Gets the underlying <see cref="ItemForDeletion"/>.
            /// </summary>
            public ItemForDeletion ItemForDeletion
            {
                get { return _itemForDeletion; }
            }
            private ItemForDeletion _itemForDeletion;

            /// <summary>
            /// Gets or sets the type of the action. Corresponding to <see cref="DeletionType"/> as string.
            /// </summary>
            public string ActionType
            {
                get { return _actionType; }
                set
                {
                    _actionType = value;

                    if (value == "Remove") ItemForDeletion.Type = DeletionType.Remove;
                    else if (value == "Clear") ItemForDeletion.Type = DeletionType.Clear;
                    else Debug.Assert(true, "Not supported deletion action.");
                }
            }
            private string _actionType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemForDeletionTreeNode"/> class.
            /// </summary>
            /// <param name="itemForDeletion">The <see cref="ItemForDeletion"/> to use.</param>
            public ItemForDeletionTreeNode(ItemForDeletion itemForDeletion)
                : base(itemForDeletion.Name)
            {
                _itemForDeletion = itemForDeletion;
                _actionType = itemForDeletion.Type.ToString();
                Tag = itemForDeletion;
            }
        }

        /// <summary>
        /// Gets or sets the description for the deleting items.
        /// </summary>
        public string Description
        {
            get { return descriptionLabel.Text; }
            set { descriptionLabel.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation can be canceled (cancel button is visible).
        /// </summary>
        public bool AllowCancel
        {
            get { return cancelButton.Enabled; }
            set
            {
                cancelButton.Enabled = value;
                cancelButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is automatically done when the list of items that need to be also deleted is empty.
        /// </summary>
        public bool ProcessWhenEmptyList { get; set; }

        // initial items to delete
        private List<ItemForDeletion> itemsToDelete;
        // items that need to be also deleted (items that we get from the inital items or other items in the processing)
        private HashSet<ItemForDeletion> items = new HashSet<ItemForDeletion>();
        // model for the tree
        private TreeModel treeModel = new TreeModel();
        // list for storing the actual path when processing an item.
        private List<ItemOnPath> itemPath = new List<ItemOnPath>();

        // indicates whether the form can be closed
        private bool canClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsistentDeletionForm"/> class.
        /// </summary>
        /// <param name="itemForDeletion">The <see cref="ItemForDeletion"/> to remove from the project.</param>
        public ConsistentDeletionForm(ItemForDeletion itemForDeletion)
            : this(new List<ItemForDeletion>() { itemForDeletion })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsistentDeletionForm"/> class.
        /// </summary>
        /// <param name="itemsForDeletion">The list of <see cref="ItemForDeletion"/> to remove from the project.</param>
        public ConsistentDeletionForm(List<ItemForDeletion> itemsForDeletion)
        {
            InitializeComponent();

            nodeComboBox.IsVisibleValueNeeded += new EventHandler<NodeControlValueEventArgs>(nodeComboBox_IsVisibleValueNeeded);

            treeView.Model = treeModel;

            itemsToDelete = itemsForDeletion;

            string description = "Deleting: ";
            for (int i = 0; i < itemsToDelete.Count; ++i)
            {
                itemsToDelete[i].Type = DeletionType.Remove;
                itemsToDelete[i].ItemsToDelete(items);
                if (i == 0) description += itemsToDelete[i].Name;
                else description += String.Format(", {0}", itemsToDelete[i].Name);
            }

            Description = description;

            treeView.BeginUpdate();

            var listItems = items.ToList();
            foreach (ItemForDeletion item in listItems)
            {
                if (!InsertItem(item))
                {
                    items.Remove(item);
                }
            }

            treeView.ExpandAll();
            treeView.EndUpdate();
        }

        /// <summary>
        /// Determines whether the specified collection of nodes contains the specified item.
        /// </summary>
        /// <param name="nodesCollection">The collection of nodes.</param>
        /// <param name="item">The item to find.</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        private Node ContainsItem(Collection<Node> nodesCollection, ItemOnPath item)
        {
            foreach (Node node in nodesCollection)
            {
                if (node.Tag == item.Tag && node.GetType() == typeof(Node))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Inserts the specified item at the tree.
        /// Also creates the tree nodes for the path of the item, if needed.
        /// Item is not inserted if is contained in the another item to delete. 
        /// For example: actor with its children is removing, children are not shown because ther are automatically removed by their parent.
        /// </summary>
        /// <param name="item">The item to insert.</param>
        /// <returns><c>true</c> if the item has been inserted; otherwise <c>false</c>.</returns>
        private bool InsertItem(ItemForDeletion item)
        {
            itemPath.Clear();
            item.GetPath(itemPath);

            for (int i = itemPath.Count - 1; i >= 0; --i)
            {
                if (ContainsItemsToDelete(itemPath[i].Tag))
                {
                    return false;
                }
            }

            bool creatingNodes = false;
            Collection<Node> currentLevel = treeModel.Nodes;
            Node node;
            for (int i = itemPath.Count - 1; i >= 0; --i)
            {
                if (!creatingNodes)
                {
                    node = ContainsItem(currentLevel, itemPath[i]);

                    if (node != null)
                    {
                        currentLevel = node.Nodes;
                    }
                    else
                    {
                        creatingNodes = true;
                    }
                }

                if (creatingNodes)
                {
                    node = new Node(String.IsNullOrEmpty(itemPath[i].Type) ? itemPath[i].Name : String.Format("{0} - {1}", itemPath[i].Type, itemPath[i].Name)) { Tag = itemPath[i].Tag };
                    currentLevel.Add(node);
                    currentLevel = node.Nodes;
                }
            }

            currentLevel.Add(new ItemForDeletionTreeNode(item));

            return true;
        }

        /// <summary>
        /// Determines whether the items to delete contains the specified object.
        /// </summary>
        private bool ContainsItemsToDelete(object tag)
        {
            if (itemsToDelete == null) return false;

            foreach (ItemForDeletion itemToDelete in itemsToDelete)
            {
                if (itemToDelete.Tag == tag)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles the Click event of the cancelButton control.
        /// Closes the form.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            canClose = true;
            Close();
        }

        /// <summary>
        /// Handles the Click event of the okButton control.
        /// Makes the operation of deleting items from the project.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (itemsToDelete != null)
            {
                foreach (ItemForDeletion itemToDelete in itemsToDelete)
                {
                    itemToDelete.Delete();
                }
                itemsToDelete = null;
            }

            foreach (ItemForDeletion item in items)
            {
                item.Delete();
            }

            HashSet<ItemForDeletion> nextItems = new HashSet<ItemForDeletion>();
            foreach (ItemForDeletion item in items)
            {
                item.ItemsToDelete(nextItems);
            }

            if (nextItems.Count != 0)
            {
                AllowCancel = false;
                Description = "Another items to delete";
                treeView.BeginUpdate();

                treeModel.Nodes.Clear();
                items = nextItems;

                var listItems = items.ToList();
                foreach (ItemForDeletion item in listItems)
                {
                    if (!InsertItem(item))
                    {
                        items.Remove(item);
                    }
                }

                treeView.ExpandAll();
                treeView.EndUpdate();
            }
            else
            {
                DialogResult = DialogResult.OK;
                EditorApplication.Editor.UnderlyingDataChange();
                canClose = true;
                Close();
            }
        }

        /// <summary>
        /// Handles the Shown event of the ConsistentDeletionForm control.
        /// </summary>
        private void ConsistentDeletionForm_Shown(object sender, EventArgs e)
        {
            if (ProcessWhenEmptyList && items.Count == 0) okButton_Click(null, null);
        }

        /// <summary>
        /// Determines whether the combobox is visible for the specified tree node. 
        /// </summary>
        private void nodeComboBox_IsVisibleValueNeeded(object sender, NodeControlValueEventArgs e)
        {
            e.Value = e.Node.Tag is ItemForDeletionTreeNode;
        }

        /// <summary>
        /// Handles the FormClosing event of the ConsistentDeletionForm control.
        /// </summary>
        private void ConsistentDeletionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !canClose)
            {
                e.Cancel = true;
            }
        }
    }
}
