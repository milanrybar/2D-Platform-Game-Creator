namespace PlatformGameCreator.Editor.Scripting
{
    partial class NamedVariablesView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.variableTypeLabel = new System.Windows.Forms.Label();
            this.variableNameTextBox = new System.Windows.Forms.TextBox();
            this.variableTypeComboBox = new System.Windows.Forms.ComboBox();
            this.variableAddButton = new System.Windows.Forms.Button();
            this.variableNameLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.settingsView = new PlatformGameCreator.Editor.Scripting.BaseSettingsView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.variableTypeLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.variableNameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.variableTypeComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.variableAddButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.variableNameLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 313);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(341, 52);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // variableTypeLabel
            // 
            this.variableTypeLabel.AutoSize = true;
            this.variableTypeLabel.Location = new System.Drawing.Point(0, 32);
            this.variableTypeLabel.Margin = new System.Windows.Forms.Padding(0, 6, 15, 0);
            this.variableTypeLabel.Name = "variableTypeLabel";
            this.variableTypeLabel.Size = new System.Drawing.Size(72, 13);
            this.variableTypeLabel.TabIndex = 1;
            this.variableTypeLabel.Text = "Variable Type";
            // 
            // variableNameTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.variableNameTextBox, 2);
            this.variableNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableNameTextBox.Location = new System.Drawing.Point(91, 3);
            this.variableNameTextBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.variableNameTextBox.Name = "variableNameTextBox";
            this.variableNameTextBox.Size = new System.Drawing.Size(250, 20);
            this.variableNameTextBox.TabIndex = 2;
            this.variableNameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.variableNameTextBox_KeyPress);
            // 
            // variableTypeComboBox
            // 
            this.variableTypeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variableTypeComboBox.FormattingEnabled = true;
            this.variableTypeComboBox.Location = new System.Drawing.Point(91, 29);
            this.variableTypeComboBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.variableTypeComboBox.Name = "variableTypeComboBox";
            this.variableTypeComboBox.Size = new System.Drawing.Size(165, 21);
            this.variableTypeComboBox.TabIndex = 3;
            // 
            // variableAddButton
            // 
            this.variableAddButton.Location = new System.Drawing.Point(266, 29);
            this.variableAddButton.Margin = new System.Windows.Forms.Padding(10, 3, 0, 0);
            this.variableAddButton.Name = "variableAddButton";
            this.variableAddButton.Size = new System.Drawing.Size(75, 23);
            this.variableAddButton.TabIndex = 4;
            this.variableAddButton.Text = "Add";
            this.variableAddButton.UseVisualStyleBackColor = true;
            this.variableAddButton.Click += new System.EventHandler(this.variableAddButton_Click);
            // 
            // variableNameLabel
            // 
            this.variableNameLabel.AutoSize = true;
            this.variableNameLabel.Location = new System.Drawing.Point(0, 6);
            this.variableNameLabel.Margin = new System.Windows.Forms.Padding(0, 6, 15, 0);
            this.variableNameLabel.Name = "variableNameLabel";
            this.variableNameLabel.Size = new System.Drawing.Size(76, 13);
            this.variableNameLabel.TabIndex = 0;
            this.variableNameLabel.Text = "Variable Name";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.settingsView, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(341, 365);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // settingsView
            // 
            this.settingsView.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.settingsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsView.Location = new System.Drawing.Point(0, 0);
            this.settingsView.Margin = new System.Windows.Forms.Padding(0);
            this.settingsView.Name = "settingsView";
            this.settingsView.Padding = new System.Windows.Forms.Padding(1);
            this.settingsView.Size = new System.Drawing.Size(341, 313);
            this.settingsView.TabIndex = 2;
            // 
            // NamedVariablesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NamedVariablesView";
            this.Size = new System.Drawing.Size(341, 365);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label variableTypeLabel;
        private System.Windows.Forms.TextBox variableNameTextBox;
        private System.Windows.Forms.ComboBox variableTypeComboBox;
        private System.Windows.Forms.Button variableAddButton;
        private System.Windows.Forms.Label variableNameLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private BaseSettingsView settingsView;
    }
}
