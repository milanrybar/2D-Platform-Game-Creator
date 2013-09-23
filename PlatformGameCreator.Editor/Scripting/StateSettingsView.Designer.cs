namespace PlatformGameCreator.Editor.Scripting
{
    partial class StateSettingsView
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.stateNameLabel = new System.Windows.Forms.Label();
            this.stateNameTextBox = new System.Windows.Forms.TextBox();
            this.settingsView = new PlatformGameCreator.Editor.Scripting.BaseSettingsView();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.stateNameLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.stateNameTextBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.settingsView, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(247, 256);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // stateNameLabel
            // 
            this.stateNameLabel.AutoSize = true;
            this.stateNameLabel.Location = new System.Drawing.Point(0, 2);
            this.stateNameLabel.Margin = new System.Windows.Forms.Padding(0, 2, 6, 0);
            this.stateNameLabel.Name = "stateNameLabel";
            this.stateNameLabel.Size = new System.Drawing.Size(63, 13);
            this.stateNameLabel.TabIndex = 0;
            this.stateNameLabel.Text = "State Name";
            // 
            // stateNameTextBox
            // 
            this.stateNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateNameTextBox.Enabled = false;
            this.stateNameTextBox.Location = new System.Drawing.Point(69, 0);
            this.stateNameTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.stateNameTextBox.Name = "stateNameTextBox";
            this.stateNameTextBox.Size = new System.Drawing.Size(178, 20);
            this.stateNameTextBox.TabIndex = 1;
            this.stateNameTextBox.TextChanged += new System.EventHandler(this.stateNameTextBox_TextChanged);
            // 
            // settingsView
            // 
            this.settingsView.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel.SetColumnSpan(this.settingsView, 2);
            this.settingsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsView.Location = new System.Drawing.Point(0, 23);
            this.settingsView.Margin = new System.Windows.Forms.Padding(0);
            this.settingsView.Name = "settingsView";
            this.settingsView.Padding = new System.Windows.Forms.Padding(1);
            this.settingsView.Size = new System.Drawing.Size(247, 233);
            this.settingsView.TabIndex = 2;
            // 
            // StateSettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StateSettingsView";
            this.Size = new System.Drawing.Size(247, 256);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label stateNameLabel;
        private System.Windows.Forms.TextBox stateNameTextBox;
        private BaseSettingsView settingsView;
    }
}
