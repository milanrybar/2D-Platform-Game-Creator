namespace PlatformGameCreator.Editor.Assets.Animations
{
    partial class AnimationForm
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
            this.components = new System.ComponentModel.Container();
            this.framesView = new System.Windows.Forms.ListView();
            this.nameLabel = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.playButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.speedBox = new System.Windows.Forms.NumericUpDown();
            this.speedInfoLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.animationSettingsPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.speedBox)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.animationSettingsPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // framesView
            // 
            this.framesView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.framesView.AllowDrop = true;
            this.framesView.AutoArrange = false;
            this.tableLayoutPanel.SetColumnSpan(this.framesView, 2);
            this.framesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framesView.Location = new System.Drawing.Point(0, 473);
            this.framesView.Margin = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.framesView.Name = "framesView";
            this.framesView.ShowGroups = false;
            this.framesView.Size = new System.Drawing.Size(553, 100);
            this.framesView.TabIndex = 1;
            this.framesView.UseCompatibleStateImageBehavior = false;
            this.framesView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.framesView_ItemDrag);
            this.framesView.DragDrop += new System.Windows.Forms.DragEventHandler(this.framesView_DragDrop);
            this.framesView.DragEnter += new System.Windows.Forms.DragEventHandler(this.framesView_DragEnter);
            this.framesView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.framesView_KeyDown);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name:";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(3, 40);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(41, 13);
            this.speedLabel.TabIndex = 3;
            this.speedLabel.Text = "Speed:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(44, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(164, 20);
            this.nameTextBox.TabIndex = 4;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(6, 80);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(99, 23);
            this.playButton.TabIndex = 7;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(110, 80);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(99, 23);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.AutoSize = true;
            this.loopCheckBox.Location = new System.Drawing.Point(6, 109);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(99, 17);
            this.loopCheckBox.TabIndex = 9;
            this.loopCheckBox.Text = "Loop Animation";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // speedBox
            // 
            this.speedBox.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.speedBox.Location = new System.Drawing.Point(44, 38);
            this.speedBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.speedBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speedBox.Name = "speedBox";
            this.speedBox.Size = new System.Drawing.Size(164, 20);
            this.speedBox.TabIndex = 10;
            this.speedBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speedBox.ValueChanged += new System.EventHandler(this.speedBox_ValueChanged);
            // 
            // speedInfoLabel
            // 
            this.speedInfoLabel.AutoSize = true;
            this.speedInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.speedInfoLabel.Location = new System.Drawing.Point(41, 61);
            this.speedInfoLabel.Name = "speedInfoLabel";
            this.speedInfoLabel.Size = new System.Drawing.Size(120, 13);
            this.speedInfoLabel.TabIndex = 11;
            this.speedInfoLabel.Text = "( miliseconds per frame )";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel.Controls.Add(this.framesView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.animationSettingsPanel, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(773, 593);
            this.tableLayoutPanel.TabIndex = 12;
            // 
            // animationSettingsPanel
            // 
            this.animationSettingsPanel.Controls.Add(this.label2);
            this.animationSettingsPanel.Controls.Add(this.nameLabel);
            this.animationSettingsPanel.Controls.Add(this.playButton);
            this.animationSettingsPanel.Controls.Add(this.stopButton);
            this.animationSettingsPanel.Controls.Add(this.speedBox);
            this.animationSettingsPanel.Controls.Add(this.loopCheckBox);
            this.animationSettingsPanel.Controls.Add(this.speedLabel);
            this.animationSettingsPanel.Controls.Add(this.nameTextBox);
            this.animationSettingsPanel.Controls.Add(this.speedInfoLabel);
            this.animationSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animationSettingsPanel.Location = new System.Drawing.Point(553, 0);
            this.animationSettingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.animationSettingsPanel.Name = "animationSettingsPanel";
            this.tableLayoutPanel.SetRowSpan(this.animationSettingsPanel, 2);
            this.animationSettingsPanel.Size = new System.Drawing.Size(220, 593);
            this.animationSettingsPanel.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(6, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 2);
            this.label2.TabIndex = 20;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 571);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(773, 22);
            this.statusStrip.TabIndex = 13;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // AnimationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 593);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "AnimationForm";
            this.Text = "Animation Editor";
            this.Activated += new System.EventHandler(this.AnimationForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnimationForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.speedBox)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.animationSettingsPanel.ResumeLayout(false);
            this.animationSettingsPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView framesView;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.CheckBox loopCheckBox;
        private System.Windows.Forms.NumericUpDown speedBox;
        private System.Windows.Forms.Label speedInfoLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel animationSettingsPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}