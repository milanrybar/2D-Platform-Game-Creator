namespace PlatformGameCreator.Editor
{
    partial class ConsistentDeletionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.nameTreeColumn = new Aga.Controls.Tree.TreeColumn();
            this.actionTreeColumn = new Aga.Controls.Tree.TreeColumn();
            this.nodeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeComboBox = new Aga.Controls.Tree.NodeControls.NodeComboBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(209, 413);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(200, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(3, 413);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(200, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.cancelButton, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.okButton, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.descriptionLabel, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(412, 439);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // treeView
            // 
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.nameTreeColumn);
            this.treeView.Columns.Add(this.actionTreeColumn);
            this.tableLayoutPanel.SetColumnSpan(this.treeView, 2);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.Location = new System.Drawing.Point(3, 27);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeTextBox);
            this.treeView.NodeControls.Add(this.nodeComboBox);
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(406, 380);
            this.treeView.TabIndex = 2;
            this.treeView.Text = "treeView";
            this.treeView.UseColumns = true;
            // 
            // nameTreeColumn
            // 
            this.nameTreeColumn.Header = "Name";
            this.nameTreeColumn.MinColumnWidth = 300;
            this.nameTreeColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.nameTreeColumn.TooltipText = null;
            this.nameTreeColumn.Width = 300;
            // 
            // actionTreeColumn
            // 
            this.actionTreeColumn.Header = "Action";
            this.actionTreeColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.actionTreeColumn.TooltipText = null;
            this.actionTreeColumn.Width = 100;
            // 
            // nodeTextBox
            // 
            this.nodeTextBox.DataPropertyName = "Text";
            this.nodeTextBox.IncrementalSearchEnabled = true;
            this.nodeTextBox.LeftMargin = 3;
            this.nodeTextBox.ParentColumn = this.nameTreeColumn;
            this.nodeTextBox.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // nodeComboBox
            // 
            this.nodeComboBox.DataPropertyName = "ActionType";
            this.nodeComboBox.DropDownItems.Add("Clear");
            this.nodeComboBox.DropDownItems.Add("Remove");
            this.nodeComboBox.EditEnabled = true;
            this.nodeComboBox.EditOnClick = true;
            this.nodeComboBox.EditorWidth = 50;
            this.nodeComboBox.IncrementalSearchEnabled = true;
            this.nodeComboBox.LeftMargin = 3;
            this.nodeComboBox.ParentColumn = this.actionTreeColumn;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(3, 6);
            this.descriptionLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 5);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "Description";
            // 
            // ConsistentDeletionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 439);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "ConsistentDeletionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Safe Deleting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsistentDeletionForm_FormClosing);
            this.Shown += new System.EventHandler(this.ConsistentDeletionForm_Shown);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Aga.Controls.Tree.TreeViewAdv treeView;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox;
        private Aga.Controls.Tree.NodeControls.NodeComboBox nodeComboBox;
        private Aga.Controls.Tree.TreeColumn nameTreeColumn;
        private Aga.Controls.Tree.TreeColumn actionTreeColumn;
        private System.Windows.Forms.Label descriptionLabel;
    }
}