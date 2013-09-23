namespace PlatformGameCreator.Editor.GameObjects.Paths
{
    partial class PathInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.editPathButton = new System.Windows.Forms.Button();
            this.loopCheckbox = new System.Windows.Forms.CheckBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // editPathButton
            // 
            this.editPathButton.Location = new System.Drawing.Point(0, 70);
            this.editPathButton.Name = "editPathButton";
            this.editPathButton.Size = new System.Drawing.Size(190, 23);
            this.editPathButton.TabIndex = 0;
            this.editPathButton.Text = "Edit Path";
            this.editPathButton.UseVisualStyleBackColor = true;
            this.editPathButton.Click += new System.EventHandler(this.editPathButton_Click);
            // 
            // loopCheckbox
            // 
            this.loopCheckbox.AutoSize = true;
            this.loopCheckbox.Location = new System.Drawing.Point(6, 34);
            this.loopCheckbox.Name = "loopCheckbox";
            this.loopCheckbox.Size = new System.Drawing.Size(50, 17);
            this.loopCheckbox.TabIndex = 1;
            this.loopCheckbox.Text = "Loop";
            this.loopCheckbox.UseVisualStyleBackColor = true;
            this.loopCheckbox.CheckedChanged += new System.EventHandler(this.loopCheckbox_CheckedChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 11);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(47, 8);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(140, 20);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(3, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 2);
            this.label2.TabIndex = 33;
            // 
            // PathInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.loopCheckbox);
            this.Controls.Add(this.editPathButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PathInfo";
            this.Size = new System.Drawing.Size(190, 200);
            this.ParentChanged += new System.EventHandler(this.PathInfo_ParentChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button editPathButton;
        private System.Windows.Forms.CheckBox loopCheckbox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label2;
    }
}
