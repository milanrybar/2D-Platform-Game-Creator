namespace PlatformGameCreator.Editor.Assets.Textures
{
    partial class BasicControllerForShapesEditing
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
            this.shapesLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.polygonSettings = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.showConvexHull = new System.Windows.Forms.CheckBox();
            this.showConvexDecomposition = new System.Windows.Forms.CheckBox();
            this.makeConvexHullButton = new System.Windows.Forms.Button();
            this.makeConvexDecompositionButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.showOrigin = new System.Windows.Forms.CheckBox();
            this.showAllShapes = new System.Windows.Forms.CheckBox();
            this.showGrid = new System.Windows.Forms.CheckBox();
            this.shapesList = new System.Windows.Forms.ListBox();
            this.selectShapeBox = new System.Windows.Forms.ComboBox();
            this.addShapeButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.polygonSettings.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // shapesLabel
            // 
            this.shapesLabel.AutoSize = true;
            this.shapesLabel.Location = new System.Drawing.Point(0, 0);
            this.shapesLabel.Name = "shapesLabel";
            this.shapesLabel.Size = new System.Drawing.Size(87, 13);
            this.shapesLabel.TabIndex = 27;
            this.shapesLabel.Text = "Collision Shapes:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.polygonSettings);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 229);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(202, 153);
            this.flowLayoutPanel1.TabIndex = 26;
            // 
            // polygonSettings
            // 
            this.polygonSettings.Controls.Add(this.label1);
            this.polygonSettings.Controls.Add(this.showConvexHull);
            this.polygonSettings.Controls.Add(this.showConvexDecomposition);
            this.polygonSettings.Controls.Add(this.makeConvexHullButton);
            this.polygonSettings.Controls.Add(this.makeConvexDecompositionButton);
            this.polygonSettings.Location = new System.Drawing.Point(0, 0);
            this.polygonSettings.Margin = new System.Windows.Forms.Padding(0);
            this.polygonSettings.Name = "polygonSettings";
            this.polygonSettings.Size = new System.Drawing.Size(202, 73);
            this.polygonSettings.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(3, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 2);
            this.label1.TabIndex = 20;
            // 
            // showConvexHull
            // 
            this.showConvexHull.AutoSize = true;
            this.showConvexHull.Checked = true;
            this.showConvexHull.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showConvexHull.Location = new System.Drawing.Point(0, 3);
            this.showConvexHull.Name = "showConvexHull";
            this.showConvexHull.Size = new System.Drawing.Size(113, 17);
            this.showConvexHull.TabIndex = 3;
            this.showConvexHull.Text = "Show Convex Hull";
            this.showConvexHull.UseVisualStyleBackColor = true;
            this.showConvexHull.CheckedChanged += new System.EventHandler(this.showConvexHull_CheckedChanged);
            // 
            // showConvexDecomposition
            // 
            this.showConvexDecomposition.AutoSize = true;
            this.showConvexDecomposition.Checked = true;
            this.showConvexDecomposition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showConvexDecomposition.Location = new System.Drawing.Point(0, 26);
            this.showConvexDecomposition.Name = "showConvexDecomposition";
            this.showConvexDecomposition.Size = new System.Drawing.Size(165, 17);
            this.showConvexDecomposition.TabIndex = 4;
            this.showConvexDecomposition.Text = "Show Convex Decomposition";
            this.showConvexDecomposition.UseVisualStyleBackColor = true;
            this.showConvexDecomposition.CheckedChanged += new System.EventHandler(this.showConvexDecomposition_CheckedChanged);
            // 
            // makeConvexHullButton
            // 
            this.makeConvexHullButton.AutoSize = true;
            this.makeConvexHullButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.makeConvexHullButton.Location = new System.Drawing.Point(171, -1);
            this.makeConvexHullButton.Name = "makeConvexHullButton";
            this.makeConvexHullButton.Size = new System.Drawing.Size(31, 23);
            this.makeConvexHullButton.TabIndex = 10;
            this.makeConvexHullButton.Text = "Do";
            this.makeConvexHullButton.UseVisualStyleBackColor = true;
            this.makeConvexHullButton.Click += new System.EventHandler(this.makeConvexHullButton_Click);
            // 
            // makeConvexDecompositionButton
            // 
            this.makeConvexDecompositionButton.AutoSize = true;
            this.makeConvexDecompositionButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.makeConvexDecompositionButton.Location = new System.Drawing.Point(171, 22);
            this.makeConvexDecompositionButton.Name = "makeConvexDecompositionButton";
            this.makeConvexDecompositionButton.Size = new System.Drawing.Size(31, 23);
            this.makeConvexDecompositionButton.TabIndex = 11;
            this.makeConvexDecompositionButton.Text = "Do";
            this.makeConvexDecompositionButton.UseVisualStyleBackColor = true;
            this.makeConvexDecompositionButton.Click += new System.EventHandler(this.makeConvexDecompositionButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.showOrigin);
            this.panel2.Controls.Add(this.showAllShapes);
            this.panel2.Controls.Add(this.showGrid);
            this.panel2.Location = new System.Drawing.Point(0, 73);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(202, 78);
            this.panel2.TabIndex = 16;
            // 
            // showOrigin
            // 
            this.showOrigin.AutoSize = true;
            this.showOrigin.Checked = true;
            this.showOrigin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOrigin.Location = new System.Drawing.Point(0, 49);
            this.showOrigin.Name = "showOrigin";
            this.showOrigin.Size = new System.Drawing.Size(83, 17);
            this.showOrigin.TabIndex = 10;
            this.showOrigin.Text = "Show Origin";
            this.showOrigin.UseVisualStyleBackColor = true;
            this.showOrigin.CheckedChanged += new System.EventHandler(this.showOriginButton_CheckedChanged);
            // 
            // showAllShapes
            // 
            this.showAllShapes.AutoSize = true;
            this.showAllShapes.Checked = true;
            this.showAllShapes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAllShapes.Location = new System.Drawing.Point(0, 3);
            this.showAllShapes.Name = "showAllShapes";
            this.showAllShapes.Size = new System.Drawing.Size(106, 17);
            this.showAllShapes.TabIndex = 8;
            this.showAllShapes.Text = "Show All Shapes";
            this.showAllShapes.UseVisualStyleBackColor = true;
            this.showAllShapes.CheckedChanged += new System.EventHandler(this.showAllShapes_CheckedChanged);
            // 
            // showGrid
            // 
            this.showGrid.AutoSize = true;
            this.showGrid.Checked = true;
            this.showGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGrid.Location = new System.Drawing.Point(0, 26);
            this.showGrid.Name = "showGrid";
            this.showGrid.Size = new System.Drawing.Size(75, 17);
            this.showGrid.TabIndex = 9;
            this.showGrid.Text = "Show Grid";
            this.showGrid.UseVisualStyleBackColor = true;
            this.showGrid.CheckedChanged += new System.EventHandler(this.showGrid_CheckedChanged);
            // 
            // shapesList
            // 
            this.shapesList.CausesValidation = false;
            this.shapesList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.shapesList.FormattingEnabled = true;
            this.shapesList.Location = new System.Drawing.Point(3, 43);
            this.shapesList.Name = "shapesList";
            this.shapesList.Size = new System.Drawing.Size(202, 173);
            this.shapesList.TabIndex = 25;
            this.shapesList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.shapesList_DrawItem);
            this.shapesList.SelectedIndexChanged += new System.EventHandler(this.shapesList_SelectedIndexChanged);
            this.shapesList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.shapesList_KeyDown);
            // 
            // selectShapeBox
            // 
            this.selectShapeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectShapeBox.FormattingEnabled = true;
            this.selectShapeBox.Items.AddRange(new object[] {
            "Polygon",
            "Circle",
            "Edge"});
            this.selectShapeBox.Location = new System.Drawing.Point(3, 16);
            this.selectShapeBox.Name = "selectShapeBox";
            this.selectShapeBox.Size = new System.Drawing.Size(121, 21);
            this.selectShapeBox.TabIndex = 24;
            // 
            // addShapeButton
            // 
            this.addShapeButton.Location = new System.Drawing.Point(130, 14);
            this.addShapeButton.Name = "addShapeButton";
            this.addShapeButton.Size = new System.Drawing.Size(75, 23);
            this.addShapeButton.TabIndex = 23;
            this.addShapeButton.Text = "Add";
            this.addShapeButton.UseVisualStyleBackColor = true;
            this.addShapeButton.Click += new System.EventHandler(this.addShapeButton_Click);
            // 
            // BasicControllerForShapesEditing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.shapesLabel);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.shapesList);
            this.Controls.Add(this.selectShapeBox);
            this.Controls.Add(this.addShapeButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BasicControllerForShapesEditing";
            this.Size = new System.Drawing.Size(213, 397);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.polygonSettings.ResumeLayout(false);
            this.polygonSettings.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label shapesLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel polygonSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox showConvexHull;
        private System.Windows.Forms.CheckBox showConvexDecomposition;
        private System.Windows.Forms.Button makeConvexHullButton;
        private System.Windows.Forms.Button makeConvexDecompositionButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox showOrigin;
        private System.Windows.Forms.CheckBox showAllShapes;
        private System.Windows.Forms.CheckBox showGrid;
        private System.Windows.Forms.ListBox shapesList;
        private System.Windows.Forms.ComboBox selectShapeBox;
        private System.Windows.Forms.Button addShapeButton;
    }
}
