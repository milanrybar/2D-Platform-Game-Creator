namespace PlatformGameCreator.Editor.Scenes
{
    partial class SceneTreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.nameTreeColumn = new Aga.Controls.Tree.TreeColumn();
            this.nodeCheckBox = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nodeIcon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.actorNodeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.layersMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addParallaxLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pathsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPathToolStripPathsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerParallaxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.parallaxSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.actorNodeMenuStrip.SuspendLayout();
            this.layerMenuStrip.SuspendLayout();
            this.layersMenuStrip.SuspendLayout();
            this.pathMenuStrip.SuspendLayout();
            this.pathsMenuStrip.SuspendLayout();
            this.layerParallaxMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Columns.Add(this.nameTreeColumn);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Margin = new System.Windows.Forms.Padding(0);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeCheckBox);
            this.treeView.NodeControls.Add(this.nodeIcon);
            this.treeView.NodeControls.Add(this.nodeTextBox);
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(131, 224);
            this.treeView.TabIndex = 0;
            this.treeView.Text = "Scene Tree";
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            this.treeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseClick);
            this.treeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDoubleClick);
            // 
            // nameTreeColumn
            // 
            this.nameTreeColumn.Header = "";
            this.nameTreeColumn.MinColumnWidth = 150;
            this.nameTreeColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.nameTreeColumn.TooltipText = null;
            this.nameTreeColumn.Width = 150;
            // 
            // nodeCheckBox
            // 
            this.nodeCheckBox.DataPropertyName = "Visible";
            this.nodeCheckBox.EditEnabled = true;
            this.nodeCheckBox.LeftMargin = 0;
            this.nodeCheckBox.ParentColumn = this.nameTreeColumn;
            // 
            // nodeIcon
            // 
            this.nodeIcon.DataPropertyName = "Image";
            this.nodeIcon.LeftMargin = 3;
            this.nodeIcon.ParentColumn = this.nameTreeColumn;
            this.nodeIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBox
            // 
            this.nodeTextBox.DataPropertyName = "Text";
            this.nodeTextBox.EditEnabled = true;
            this.nodeTextBox.IncrementalSearchEnabled = true;
            this.nodeTextBox.LeftMargin = 3;
            this.nodeTextBox.ParentColumn = this.nameTreeColumn;
            this.nodeTextBox.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // actorNodeMenuStrip
            // 
            this.actorNodeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.actorNodeMenuStrip.Name = "actorNodeMenuStrip";
            this.actorNodeMenuStrip.Size = new System.Drawing.Size(118, 70);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Image = global::PlatformGameCreator.Editor.Properties.Resources.ZoomHS;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // layerMenuStrip
            // 
            this.layerMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.renameToolStripMenuItem1,
            this.deleteToolStripMenuItem1});
            this.layerMenuStrip.Name = "layerMenuStrip";
            this.layerMenuStrip.Size = new System.Drawing.Size(118, 70);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.selectToolStripMenuItem.Text = "Select";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem1.Text = "Rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // layersMenuStrip
            // 
            this.layersMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLayerToolStripMenuItem,
            this.addParallaxLayerToolStripMenuItem});
            this.layersMenuStrip.Name = "layersMenuStrip";
            this.layersMenuStrip.Size = new System.Drawing.Size(171, 48);
            // 
            // addLayerToolStripMenuItem
            // 
            this.addLayerToolStripMenuItem.Image = global::PlatformGameCreator.Editor.Properties.Resources.new_layer;
            this.addLayerToolStripMenuItem.Name = "addLayerToolStripMenuItem";
            this.addLayerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addLayerToolStripMenuItem.Text = "Add Layer";
            this.addLayerToolStripMenuItem.Click += new System.EventHandler(this.addLayerToolStripMenuItem_Click);
            // 
            // addParallaxLayerToolStripMenuItem
            // 
            this.addParallaxLayerToolStripMenuItem.Image = global::PlatformGameCreator.Editor.Properties.Resources.new_layerParallax;
            this.addParallaxLayerToolStripMenuItem.Name = "addParallaxLayerToolStripMenuItem";
            this.addParallaxLayerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addParallaxLayerToolStripMenuItem.Text = "Add Parallax Layer";
            this.addParallaxLayerToolStripMenuItem.Click += new System.EventHandler(this.addParallaxLayerToolStripMenuItem_Click);
            // 
            // pathMenuStrip
            // 
            this.pathMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem1,
            this.renameToolStripMenuItem2,
            this.deleteToolStripMenuItem2});
            this.pathMenuStrip.Name = "pathMenuStrip";
            this.pathMenuStrip.Size = new System.Drawing.Size(118, 70);
            // 
            // showToolStripMenuItem1
            // 
            this.showToolStripMenuItem1.Image = global::PlatformGameCreator.Editor.Properties.Resources.ZoomHS;
            this.showToolStripMenuItem1.Name = "showToolStripMenuItem1";
            this.showToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.showToolStripMenuItem1.Text = "Show";
            this.showToolStripMenuItem1.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem2
            // 
            this.renameToolStripMenuItem2.Name = "renameToolStripMenuItem2";
            this.renameToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem2.Text = "Rename";
            this.renameToolStripMenuItem2.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem2
            // 
            this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem2.Text = "Delete";
            this.deleteToolStripMenuItem2.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // pathsMenuStrip
            // 
            this.pathsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPathToolStripPathsMenuItem});
            this.pathsMenuStrip.Name = "pathsMenuStrip";
            this.pathsMenuStrip.Size = new System.Drawing.Size(124, 26);
            // 
            // addPathToolStripPathsMenuItem
            // 
            this.addPathToolStripPathsMenuItem.Image = global::PlatformGameCreator.Editor.Properties.Resources.new_path;
            this.addPathToolStripPathsMenuItem.Name = "addPathToolStripPathsMenuItem";
            this.addPathToolStripPathsMenuItem.Size = new System.Drawing.Size(123, 22);
            this.addPathToolStripPathsMenuItem.Text = "Add Path";
            this.addPathToolStripPathsMenuItem.Click += new System.EventHandler(this.addPathToolStripPathsMenuItem_Click);
            // 
            // layerParallaxMenuStrip
            // 
            this.layerParallaxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem1,
            this.renameToolStripMenuItem3,
            this.deleteToolStripMenuItem3,
            this.parallaxSettingsToolStripMenuItem});
            this.layerParallaxMenuStrip.Name = "layerParallaxMenuStrip";
            this.layerParallaxMenuStrip.Size = new System.Drawing.Size(160, 114);
            // 
            // renameToolStripMenuItem3
            // 
            this.renameToolStripMenuItem3.Name = "renameToolStripMenuItem3";
            this.renameToolStripMenuItem3.Size = new System.Drawing.Size(159, 22);
            this.renameToolStripMenuItem3.Text = "Rename";
            this.renameToolStripMenuItem3.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem3
            // 
            this.deleteToolStripMenuItem3.Name = "deleteToolStripMenuItem3";
            this.deleteToolStripMenuItem3.Size = new System.Drawing.Size(159, 22);
            this.deleteToolStripMenuItem3.Text = "Delete";
            this.deleteToolStripMenuItem3.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // parallaxSettingsToolStripMenuItem
            // 
            this.parallaxSettingsToolStripMenuItem.Name = "parallaxSettingsToolStripMenuItem";
            this.parallaxSettingsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.parallaxSettingsToolStripMenuItem.Text = "Parallax Settings";
            this.parallaxSettingsToolStripMenuItem.Click += new System.EventHandler(this.parallaxSettingsToolStripMenuItem_Click);
            // 
            // selectToolStripMenuItem1
            // 
            this.selectToolStripMenuItem1.Name = "selectToolStripMenuItem1";
            this.selectToolStripMenuItem1.Size = new System.Drawing.Size(159, 22);
            this.selectToolStripMenuItem1.Text = "Select";
            this.selectToolStripMenuItem1.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // SceneTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.treeView);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SceneTreeView";
            this.Size = new System.Drawing.Size(131, 224);
            this.actorNodeMenuStrip.ResumeLayout(false);
            this.layerMenuStrip.ResumeLayout(false);
            this.layersMenuStrip.ResumeLayout(false);
            this.pathMenuStrip.ResumeLayout(false);
            this.pathsMenuStrip.ResumeLayout(false);
            this.layerParallaxMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeView;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox nodeCheckBox;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox;
        private Aga.Controls.Tree.TreeColumn nameTreeColumn;
        private System.Windows.Forms.ContextMenuStrip actorNodeMenuStrip;
        private System.Windows.Forms.ContextMenuStrip layerMenuStrip;
        private System.Windows.Forms.ContextMenuStrip layersMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip pathMenuStrip;
        private System.Windows.Forms.ContextMenuStrip pathsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addPathToolStripPathsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addParallaxLayerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip layerParallaxMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem parallaxSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem1;
    }
}
