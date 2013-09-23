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

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Imports methods for setting spacing of the <see cref="ListView"/>.
    /// </summary>
    /// <remarks>
    /// Own class because generic class (<see cref="BaseTexturesView{DrawableAssetType}"/>) cannot contains DllImport.
    /// </remarks>
    class ControlAllowsSpacingForListView : UserControl
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        /// <summary>
        /// Sets spacing to the specified <see cref="ListView"/>.
        /// </summary>
        /// <param name="listview">The listview to set spacing to</param>
        /// <param name="cx">The cx.</param>
        /// <param name="cy">The cy.</param>
        protected void ListView_SetSpacing(ListView listview, short cx, short cy)
        {
            const int LVM_FIRST = 0x1000;
            const int LVM_SETICONSPACING = LVM_FIRST + 53;
            // http://msdn.microsoft.com/en-us/library/bb761176(VS.85).aspx
            SendMessage(listview.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(cx, cy));
        }
    }

    /// <summary>
    /// Base class for view of list of drawable assets.
    /// Everything is done automatically by setting manager (<see cref="DrawableAssets"/>) of drawable assets.
    /// </summary>
    /// <typeparam name="DrawableAssetType">Type of the <see cref="DrawableAsset"/>.</typeparam>
    abstract partial class BaseTexturesView<DrawableAssetType> : ControlAllowsSpacingForListView where DrawableAssetType : DrawableAsset
    {
        /// <summary>
        /// Item of the specified drawable asset type for using in the <see cref="ListView"/>.
        /// </summary>
        /// <typeparam name="AssetType">Type of the item of the <see cref="DrawableAsset"/>.</typeparam>
        protected class AssetListViewItem<AssetType> : ListViewItem where AssetType : Asset
        {
            /// <summary>
            /// Drawable asset.
            /// </summary>
            protected AssetType asset;

            /// <summary>
            /// Initializes a new instance of the <see cref="BaseTexturesView&lt;DrawableAssetType&gt;.AssetListViewItem&lt;AssetType&gt;"/> class.
            /// </summary>
            /// <param name="asset">The drawable asset.</param>
            public AssetListViewItem(AssetType asset)
                : base(asset.Name, asset.Id.ToString())
            {
                this.asset = asset;
                Tag = asset;
                asset.NameChanged += new EventHandler(asset_NameChanged);
            }

            /// <summary>
            /// Removes the item from its associated <see cref="T:System.Windows.Forms.ListView"/> control.
            /// </summary>
            public override void Remove()
            {
                asset.NameChanged -= new EventHandler(asset_NameChanged);
                asset = null;

                base.Remove();
            }

            /// <summary>
            /// Called when the name of the drawable asset changes.
            /// </summary>
            private void asset_NameChanged(object sender, EventArgs e)
            {
                if (asset.Name != Text)
                {
                    Text = asset.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets the manager of drawable assets.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ContentManager<DrawableAssetType> DrawableAssets
        {
            get { return _drawableAssets; }
            set
            {
                if (_drawableAssets != value)
                {
                    if (_drawableAssets != null)
                    {
                        ListViewClear();
                        listView.LargeImageList.Images.Clear();

                        _drawableAssets.ListChanged -= new ObservableList<DrawableAssetType>.ListChangedEventHandler(DrawableAssets_ListChanged);
                    }

                    _drawableAssets = value;

                    if (_drawableAssets != null)
                    {
                        _drawableAssets.ListChanged += new ObservableList<DrawableAssetType>.ListChangedEventHandler(DrawableAssets_ListChanged);

                        foreach (DrawableAssetType drawableAsset in _drawableAssets)
                        {
                            ShowItem(drawableAsset);
                        }
                    }
                }
            }
        }
        private ContentManager<DrawableAssetType> _drawableAssets;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTexturesView&lt;DrawableAssetType&gt;"/> class.
        /// </summary>
        public BaseTexturesView()
        {
            InitializeComponent();

            listView.LargeImageList = new ImageList();
            listView.LargeImageList.ImageSize = new Size(50, 50);
            listView.LargeImageList.ColorDepth = ColorDepth.Depth24Bit;

            ListView_SetSpacing(this.listView, 50 + 8, 50 + 4 + 20);
        }

        /// <summary>
        /// Shows the specified item in the ListView.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to show.</param>
        protected virtual void ShowItem(DrawableAssetType drawableAsset)
        {
            if (!listView.LargeImageList.Images.ContainsKey(drawableAsset.Id.ToString()))
            {
                listView.LargeImageList.Images.Add(drawableAsset.Id.ToString(), GetDrawableAssetImage(drawableAsset).CreateThumbnail(listView.LargeImageList.ImageSize.Width, listView.LargeImageList.ImageSize.Height));
            }

            listView.Items.Add(new AssetListViewItem<DrawableAssetType>(drawableAsset));
        }

        /// <summary>
        /// Gets the <see cref="Image"/> from the specified drawable asset.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to get image from.</param>
        /// <returns>Returns <see cref="Image"/> of the drawable asset.</returns>
        protected abstract Image GetDrawableAssetImage(DrawableAssetType drawableAsset);

        /// <summary>
        /// Finds the drawable asset in the ListView.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to find.</param>
        /// <returns>Item of ListView if found; otherwise null.</returns>
        private ListViewItem FindDrawableAsset(DrawableAssetType drawableAsset)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Tag == drawableAsset) return item;
            }

            return null;
        }

        /// <summary>
        /// Removes all items from ListView.
        /// </summary>
        private void ListViewClear()
        {
            while (listView.Items.Count > 0)
            {
                listView.Items[0].Remove();
            }
        }

        /// <summary>
        /// Called when the manager of drawable assets changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ObservableListChangedEventArgs&lt;DrawableAssetType&gt;"/> instance containing the event data.</param>
        void DrawableAssets_ListChanged(object sender, ObservableListChangedEventArgs<DrawableAssetType> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowItem(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    ListViewItem deletedItem = FindDrawableAsset(e.Item);
                    if (deletedItem != null) deletedItem.Remove();
                    break;

                case ObservableListChangedType.ItemChanged:
                    Debug.Assert(true, "Operation not supported.");
                    break;

                case ObservableListChangedType.Reset:
                    ListViewClear();
                    foreach (DrawableAssetType drawableAsset in DrawableAssets)
                    {
                        ShowItem(drawableAsset);
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles the ItemDrag event of the listView control.
        /// Begins drag drop operation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemDragEventArgs"/> instance containing the event data.</param>
        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item != null && e.Item is ListViewItem)
            {
                Debug.Assert(((ListViewItem)e.Item).Tag != null && ((ListViewItem)e.Item).Tag is DrawableAsset, "Not drawable asset.");
                listView.DoDragDrop(new DataObject(typeof(DrawableAsset).FullName, ((ListViewItem)e.Item).Tag), DragDropEffects.All);
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
        /// Handles the MouseDoubleClick event of the listView control.
        /// Open editor for the selected item, if left mouse is down.
        /// </summary>
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                OpenDrawableAsset((DrawableAssetType)listView.SelectedItems[0].Tag);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the listView control.
        /// Delete - Remove the selected item.
        /// Enter - Open editor for the selected item.
        /// </summary>
        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                RemoveDrawableAsset((DrawableAssetType)listView.SelectedItems[0].Tag);
            }
            else if (e.KeyCode == Keys.Enter && listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                OpenDrawableAsset((DrawableAssetType)listView.SelectedItems[0].Tag);
            }
        }

        /// <summary>
        /// Handles the AfterLabelEdit event of the listView control.
        /// Sets new name to the edited drawable asset.
        /// </summary>
        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            DrawableAssetType drawableAsset = listView.Items[e.Item].Tag as DrawableAssetType;

            if (e.Label != null && drawableAsset.Name != e.Label)
            {
                if (e.Label == String.Empty)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    drawableAsset.Name = e.Label;
                }
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
                RemoveDrawableAsset((DrawableAssetType)listView.SelectedItems[0].Tag);
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
        /// Handles the Click event of the openToolStripMenuItem control.
        /// Opens editor for the selected item (drawable asset).
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems != null && listView.SelectedItems.Count == 1)
            {
                OpenDrawableAsset((DrawableAssetType)listView.SelectedItems[0].Tag);
            }
        }

        /// <summary>
        /// Removes the specified drawable asset.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset.</param>
        protected abstract void RemoveDrawableAsset(DrawableAssetType drawableAsset);

        /// <summary>
        /// Opens editor for the specified drawable asset.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset.</param>
        protected abstract void OpenDrawableAsset(DrawableAssetType drawableAsset);

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DrawableAssets = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
