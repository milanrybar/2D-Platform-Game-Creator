namespace PlatformGameCreator.Editor
{
    partial class IntroForm
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
            this.newProjectbutton = new System.Windows.Forms.Button();
            this.openProjectButton = new System.Windows.Forms.Button();
            this.openLastProjectButton = new System.Windows.Forms.Button();
            this.lastProjectTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // newProjectbutton
            // 
            this.newProjectbutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newProjectbutton.Location = new System.Drawing.Point(3, 3);
            this.newProjectbutton.Name = "newProjectbutton";
            this.newProjectbutton.Size = new System.Drawing.Size(162, 72);
            this.newProjectbutton.TabIndex = 0;
            this.newProjectbutton.Text = "New Project";
            this.newProjectbutton.UseVisualStyleBackColor = true;
            this.newProjectbutton.Click += new System.EventHandler(this.newProjectbutton_Click);
            // 
            // openProjectButton
            // 
            this.openProjectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openProjectButton.Location = new System.Drawing.Point(171, 3);
            this.openProjectButton.Name = "openProjectButton";
            this.openProjectButton.Size = new System.Drawing.Size(162, 72);
            this.openProjectButton.TabIndex = 1;
            this.openProjectButton.Text = "Open Project";
            this.openProjectButton.UseVisualStyleBackColor = true;
            this.openProjectButton.Click += new System.EventHandler(this.openProjectButton_Click);
            // 
            // openLastProjectButton
            // 
            this.tableLayoutPanel.SetColumnSpan(this.openLastProjectButton, 2);
            this.openLastProjectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openLastProjectButton.Location = new System.Drawing.Point(3, 81);
            this.openLastProjectButton.Name = "openLastProjectButton";
            this.openLastProjectButton.Size = new System.Drawing.Size(330, 27);
            this.openLastProjectButton.TabIndex = 2;
            this.openLastProjectButton.Text = "Open Last Project";
            this.openLastProjectButton.UseVisualStyleBackColor = true;
            this.openLastProjectButton.Click += new System.EventHandler(this.openLastProjectButton_Click);
            // 
            // lastProjectTextBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.lastProjectTextBox, 2);
            this.lastProjectTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lastProjectTextBox.Location = new System.Drawing.Point(3, 114);
            this.lastProjectTextBox.Name = "lastProjectTextBox";
            this.lastProjectTextBox.ReadOnly = true;
            this.lastProjectTextBox.Size = new System.Drawing.Size(330, 20);
            this.lastProjectTextBox.TabIndex = 3;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.newProjectbutton, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lastProjectTextBox, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.openProjectButton, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.openLastProjectButton, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(336, 138);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 138);
            this.Controls.Add(this.tableLayoutPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 176);
            this.Name = "IntroForm";
            this.Text = "2D Platform Game Creator ~ Intro";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newProjectbutton;
        private System.Windows.Forms.Button openProjectButton;
        private System.Windows.Forms.Button openLastProjectButton;
        private System.Windows.Forms.TextBox lastProjectTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}