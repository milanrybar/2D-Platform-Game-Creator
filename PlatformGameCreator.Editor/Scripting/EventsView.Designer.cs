namespace PlatformGameCreator.Editor.Scripting
{
    partial class EventsView
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
            this.eventNameLabel = new System.Windows.Forms.Label();
            this.eventNameTextBox = new System.Windows.Forms.TextBox();
            this.addEventButton = new System.Windows.Forms.Button();
            this.settingsView = new PlatformGameCreator.Editor.Scripting.BaseSettingsView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.eventNameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.eventNameTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.addEventButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.settingsView, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(309, 362);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // eventNameLabel
            // 
            this.eventNameLabel.AutoSize = true;
            this.eventNameLabel.Location = new System.Drawing.Point(0, 342);
            this.eventNameLabel.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.eventNameLabel.Name = "eventNameLabel";
            this.eventNameLabel.Size = new System.Drawing.Size(66, 13);
            this.eventNameLabel.TabIndex = 0;
            this.eventNameLabel.Text = "Event Name";
            // 
            // eventNameTextBox
            // 
            this.eventNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventNameTextBox.Location = new System.Drawing.Point(74, 340);
            this.eventNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.eventNameTextBox.Name = "eventNameTextBox";
            this.eventNameTextBox.Size = new System.Drawing.Size(191, 20);
            this.eventNameTextBox.TabIndex = 1;
            this.eventNameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eventNameTextBox_KeyPress);
            // 
            // addEventButton
            // 
            this.addEventButton.AutoSize = true;
            this.addEventButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addEventButton.Location = new System.Drawing.Point(273, 339);
            this.addEventButton.Margin = new System.Windows.Forms.Padding(5, 3, 0, 0);
            this.addEventButton.Name = "addEventButton";
            this.addEventButton.Size = new System.Drawing.Size(36, 23);
            this.addEventButton.TabIndex = 2;
            this.addEventButton.Text = "Add";
            this.addEventButton.UseVisualStyleBackColor = true;
            this.addEventButton.Click += new System.EventHandler(this.addEventButton_Click);
            // 
            // settingsView
            // 
            this.settingsView.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel1.SetColumnSpan(this.settingsView, 3);
            this.settingsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsView.Location = new System.Drawing.Point(0, 0);
            this.settingsView.Margin = new System.Windows.Forms.Padding(0);
            this.settingsView.Name = "settingsView";
            this.settingsView.Padding = new System.Windows.Forms.Padding(1);
            this.settingsView.Size = new System.Drawing.Size(309, 336);
            this.settingsView.TabIndex = 3;
            // 
            // EventsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EventsView";
            this.Size = new System.Drawing.Size(309, 362);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label eventNameLabel;
        private System.Windows.Forms.TextBox eventNameTextBox;
        private System.Windows.Forms.Button addEventButton;
        private BaseSettingsView settingsView;
    }
}
