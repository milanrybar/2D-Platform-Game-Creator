namespace PlatformGameCreator.Editor.Assets.Textures
{
    partial class TextureControllerForShapesEditing
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.experimentalLabel = new System.Windows.Forms.Label();
            this.createPolygonFromTextureButton = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.experimentalLabel);
            this.panel3.Controls.Add(this.createPolygonFromTextureButton);
            this.panel3.Location = new System.Drawing.Point(3, 382);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(202, 70);
            this.panel3.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 2);
            this.label3.TabIndex = 20;
            // 
            // experimentalLabel
            // 
            this.experimentalLabel.AutoSize = true;
            this.experimentalLabel.Location = new System.Drawing.Point(-3, 15);
            this.experimentalLabel.Name = "experimentalLabel";
            this.experimentalLabel.Size = new System.Drawing.Size(70, 13);
            this.experimentalLabel.TabIndex = 12;
            this.experimentalLabel.Text = "Experimental:";
            // 
            // createPolygonFromTextureButton
            // 
            this.createPolygonFromTextureButton.AutoSize = true;
            this.createPolygonFromTextureButton.Location = new System.Drawing.Point(0, 40);
            this.createPolygonFromTextureButton.Name = "createPolygonFromTextureButton";
            this.createPolygonFromTextureButton.Size = new System.Drawing.Size(151, 23);
            this.createPolygonFromTextureButton.TabIndex = 13;
            this.createPolygonFromTextureButton.Text = "Create Polygon from Texture";
            this.createPolygonFromTextureButton.UseVisualStyleBackColor = true;
            this.createPolygonFromTextureButton.Click += new System.EventHandler(this.createPolygonFromTextureButton_Click);
            // 
            // TextureControllerForShapesEditing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.panel3);
            this.Name = "TextureControllerForShapesEditing";
            this.Size = new System.Drawing.Size(208, 452);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label experimentalLabel;
        private System.Windows.Forms.Button createPolygonFromTextureButton;
    }
}
