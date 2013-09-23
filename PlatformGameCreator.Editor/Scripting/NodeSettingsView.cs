/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Control for editing script nodes settings.
    /// </summary>
    class NodeSettingsView : BaseSettingsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeSettingsView"/> class.
        /// </summary>
        public NodeSettingsView()
        {
            table.Columns.Add(new DataGridViewCheckBoxColumn() { HeaderText = "", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells });
            table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Name", ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Value", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Type", ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }
    }
}
