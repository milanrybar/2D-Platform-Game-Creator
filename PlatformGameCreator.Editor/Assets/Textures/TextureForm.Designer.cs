namespace PlatformGameCreator.Editor.Assets.Textures
{
    partial class TextureForm
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textureSettingsPanel = new System.Windows.Forms.Panel();
            this.originLabel = new System.Windows.Forms.Label();
            this.changeOriginButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel.SuspendLayout();
            this.textureSettingsPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textureSettingsPanel, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(773, 571);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // textureSettingsPanel
            // 
            this.textureSettingsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.textureSettingsPanel.Controls.Add(this.originLabel);
            this.textureSettingsPanel.Controls.Add(this.changeOriginButton);
            this.textureSettingsPanel.Controls.Add(this.label2);
            this.textureSettingsPanel.Controls.Add(this.nameTextBox);
            this.textureSettingsPanel.Controls.Add(this.nameLabel);
            this.textureSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureSettingsPanel.Location = new System.Drawing.Point(553, 0);
            this.textureSettingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.textureSettingsPanel.Name = "textureSettingsPanel";
            this.textureSettingsPanel.Size = new System.Drawing.Size(220, 571);
            this.textureSettingsPanel.TabIndex = 2;
            // 
            // originLabel
            // 
            this.originLabel.AutoSize = true;
            this.originLabel.Location = new System.Drawing.Point(3, 43);
            this.originLabel.Name = "originLabel";
            this.originLabel.Size = new System.Drawing.Size(37, 13);
            this.originLabel.TabIndex = 21;
            this.originLabel.Text = "Origin:";
            // 
            // changeOriginButton
            // 
            this.changeOriginButton.AutoSize = true;
            this.changeOriginButton.Location = new System.Drawing.Point(44, 38);
            this.changeOriginButton.Name = "changeOriginButton";
            this.changeOriginButton.Size = new System.Drawing.Size(84, 23);
            this.changeOriginButton.TabIndex = 20;
            this.changeOriginButton.Text = "Change Origin";
            this.changeOriginButton.UseVisualStyleBackColor = true;
            this.changeOriginButton.Click += new System.EventHandler(this.changeOriginButton_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(6, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 2);
            this.label2.TabIndex = 19;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(44, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(164, 20);
            this.nameTextBox.TabIndex = 7;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Name:";
            // 
            // statusStrip
            // 
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 571);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(773, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // TextureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 593);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "TextureForm";
            this.Text = "Texture Editor";
            this.Activated += new System.EventHandler(this.TextureForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextureForm_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.textureSettingsPanel.ResumeLayout(false);
            this.textureSettingsPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel textureSettingsPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button changeOriginButton;
        private System.Windows.Forms.Label originLabel;


    }
}