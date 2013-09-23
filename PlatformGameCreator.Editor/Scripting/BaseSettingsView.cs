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
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a table for editing settings.
    /// </summary>
    partial class BaseSettingsView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSettingsView"/> class.
        /// </summary>
        public BaseSettingsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the CurrentCellDirtyStateChanged event of the table control.
        /// If the cell is dirty the edited value is commited.
        /// </summary>
        private void table_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (table.IsCurrentCellDirty)
            {
                table.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Handles the CellValueChanged event of the table control.
        /// Informs the cell the its value changes.
        /// </summary>
        private void table_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ICellValueChanged cell = table.Rows[e.RowIndex].Cells[e.ColumnIndex] as ICellValueChanged;

            if (cell != null)
            {
                cell.CellValueChanged();
            }
        }

        /// <summary>
        /// Handles the DataError event of the table control.
        /// </summary>
        private void table_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Messages.ShowError(String.Format("DataGrid Error: {0}", e.Exception != null ? e.Exception.Message : "Info not available."));
        }

        /// <summary>
        /// Removes all rows from the table.
        /// </summary>
        public void ClearRows()
        {
            foreach (DataGridViewRow row in table.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Dispose();
                }

                row.Dispose();
            }

            try
            {
                table.Rows.Clear();
            }
            catch (InvalidOperationException)
            {
                Debug.Assert(table.IsCurrentCellInEditMode, "Only possible exception is when current cell is in edit mode and application is quiting.");
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
                ClearRows();
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Informs the <see cref="DataGridViewCell"/> that its value changes.
    /// </summary>
    interface ICellValueChanged
    {
        /// <summary>
        /// Called when the Value changes.
        /// </summary>
        void CellValueChanged();
    }

    /// <summary>
    /// <see cref="DataGridViewCheckBoxCell"/> that is informed when its value changes.
    /// </summary>
    class DataGridViewCheckBoxCellValueChanged : DataGridViewCheckBoxCell, ICellValueChanged
    {
        /// <summary>
        /// Occurs when the Value changes.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <inheritdoc />
        public void CellValueChanged()
        {
            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Represents <see cref="DataGridViewCheckBoxColumn"/> for <see cref="DataGridViewDisableCheckBoxCell"/> cells.
    /// </summary>
    class DataGridViewDisableCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDisableCheckBoxColumn"/> class.
        /// </summary>
        public DataGridViewDisableCheckBoxColumn()
        {
            this.CellTemplate = new DataGridViewDisableCheckBoxCell();
        }
    }

    /// <summary>
    /// <see cref="DataGridViewCheckBoxCell"/> that can be disabled.
    /// </summary>
    class DataGridViewDisableCheckBoxCell : DataGridViewCheckBoxCellValueChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataGridViewDisableCheckBoxCell"/> is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDisableCheckBoxCell"/> class.
        /// </summary>
        public DataGridViewDisableCheckBoxCell()
        {
            Enabled = true;
        }

        /// <inheritdoc />
        public override object Clone()
        {
            DataGridViewDisableCheckBoxCell cell = (DataGridViewDisableCheckBoxCell)base.Clone();
            cell.Enabled = Enabled;
            return cell;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Paints the checkbox only if the <see cref="Enabled"/> property is <c>true</c>.
        /// </remarks>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (Enabled)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            }
            else
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// Represents the <see cref="DataGridViewCell"/> for showing the title in the table.
    /// </summary>
    class DataGridViewTitleCell : DataGridViewCheckBoxCell
    {
        /// <summary>
        /// Gets or sets column Index of the left-most cell to be merged.
        /// This cell controls the merged text.
        /// </summary>
        public int LeftColumn { get; set; }

        /// <summary>
        /// Gets or sets column Index of the right-most cell to be merged
        /// </summary>
        public int RightColumn { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewTitleCell"/> class.
        /// </summary>
        public DataGridViewTitleCell()
        {
            LeftColumn = 0;
            RightColumn = 0;
            Value = true;
        }

        /// <inheritdoc />
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (ColumnIndex == LeftColumn)
            {
                // Determine the total width of the merged cell
                int width = 0;
                for (int i = LeftColumn; i <= RightColumn; i++) width += this.OwningRow.Cells[i].Size.Width;

                // cell rectangle
                RectangleF cell = new RectangleF(cellBounds.Left, cellBounds.Top, width, cellBounds.Height);

                // Draw the background
                graphics.FillRectangle(SystemBrushes.ControlLight, cell);

                // Draw the separator for rows
                graphics.DrawLine(SystemPens.ControlDark, cell.Left, cell.Top, cell.Right, cell.Top);
                graphics.DrawLine(SystemPens.ControlDark, cell.Left, cell.Bottom - 1, cell.Right, cell.Bottom - 1);

                // Draw the text
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;

                graphics.DrawString(Title, cellStyle.Font, SystemBrushes.ControlText, cell, sf);
            }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            DataGridViewTitleCell cell = (DataGridViewTitleCell)base.Clone();
            cell.Title = Title;
            cell.LeftColumn = LeftColumn;
            cell.RightColumn = RightColumn;
            return cell;
        }
    }
}
