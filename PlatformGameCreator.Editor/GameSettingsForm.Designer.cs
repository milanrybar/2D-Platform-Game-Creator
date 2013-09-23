namespace PlatformGameCreator.Editor
{
    partial class GameSettingsForm
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
            this.gameWindowLabel = new System.Windows.Forms.Label();
            this.simulationUnitsLabel = new System.Windows.Forms.Label();
            this.defaultGravityLabel = new System.Windows.Forms.Label();
            this.gameWindowWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.gameWindowHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.defaultGravityYFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.defaultGravityXFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.simulationUnitsFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.backgroundColorLabel = new System.Windows.Forms.Label();
            this.backgroundColorRLabel = new System.Windows.Forms.Label();
            this.backgroundColorGLabel = new System.Windows.Forms.Label();
            this.backgroundColorBLabel = new System.Windows.Forms.Label();
            this.backgroundColorRNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.backgroundColorGNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.backgroundColorBNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.continuousCollisionDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.fullScreenCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gameWindowWidthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameWindowHeightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorRNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorGNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorBNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // gameWindowLabel
            // 
            this.gameWindowLabel.AutoSize = true;
            this.gameWindowLabel.Location = new System.Drawing.Point(12, 9);
            this.gameWindowLabel.Name = "gameWindowLabel";
            this.gameWindowLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gameWindowLabel.Size = new System.Drawing.Size(80, 13);
            this.gameWindowLabel.TabIndex = 0;
            this.gameWindowLabel.Text = "Game Window:";
            // 
            // simulationUnitsLabel
            // 
            this.simulationUnitsLabel.AutoSize = true;
            this.simulationUnitsLabel.Location = new System.Drawing.Point(12, 65);
            this.simulationUnitsLabel.Name = "simulationUnitsLabel";
            this.simulationUnitsLabel.Size = new System.Drawing.Size(85, 13);
            this.simulationUnitsLabel.TabIndex = 1;
            this.simulationUnitsLabel.Text = "Simulation Units:";
            // 
            // defaultGravityLabel
            // 
            this.defaultGravityLabel.AutoSize = true;
            this.defaultGravityLabel.Location = new System.Drawing.Point(12, 93);
            this.defaultGravityLabel.Name = "defaultGravityLabel";
            this.defaultGravityLabel.Size = new System.Drawing.Size(80, 13);
            this.defaultGravityLabel.TabIndex = 2;
            this.defaultGravityLabel.Text = "Default Gravity:";
            // 
            // gameWindowWidthNumericUpDown
            // 
            this.gameWindowWidthNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gameWindowWidthNumericUpDown.Location = new System.Drawing.Point(155, 7);
            this.gameWindowWidthNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.gameWindowWidthNumericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.gameWindowWidthNumericUpDown.Name = "gameWindowWidthNumericUpDown";
            this.gameWindowWidthNumericUpDown.Size = new System.Drawing.Size(74, 20);
            this.gameWindowWidthNumericUpDown.TabIndex = 4;
            this.gameWindowWidthNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.gameWindowWidthNumericUpDown.ValueChanged += new System.EventHandler(this.gameWindowWidthNumericUpDown_ValueChanged);
            // 
            // gameWindowHeightNumericUpDown
            // 
            this.gameWindowHeightNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gameWindowHeightNumericUpDown.Location = new System.Drawing.Point(155, 33);
            this.gameWindowHeightNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.gameWindowHeightNumericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.gameWindowHeightNumericUpDown.Name = "gameWindowHeightNumericUpDown";
            this.gameWindowHeightNumericUpDown.Size = new System.Drawing.Size(74, 20);
            this.gameWindowHeightNumericUpDown.TabIndex = 5;
            this.gameWindowHeightNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.gameWindowHeightNumericUpDown.ValueChanged += new System.EventHandler(this.gameWindowHeightNumericUpDown_ValueChanged);
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(98, 9);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(38, 13);
            this.widthLabel.TabIndex = 9;
            this.widthLabel.Text = "Width:";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(98, 35);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(41, 13);
            this.heightLabel.TabIndex = 10;
            this.heightLabel.Text = "Height:";
            // 
            // xLabel
            // 
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(98, 93);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(17, 13);
            this.xLabel.TabIndex = 11;
            this.xLabel.Text = "X:";
            // 
            // yLabel
            // 
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(98, 120);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(17, 13);
            this.yLabel.TabIndex = 12;
            this.yLabel.Text = "Y:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(156, 283);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // defaultGravityYFloatBox
            // 
            this.defaultGravityYFloatBox.Location = new System.Drawing.Point(121, 117);
            this.defaultGravityYFloatBox.Name = "defaultGravityYFloatBox";
            this.defaultGravityYFloatBox.Size = new System.Drawing.Size(108, 20);
            this.defaultGravityYFloatBox.TabIndex = 8;
            this.defaultGravityYFloatBox.Text = "0";
            this.defaultGravityYFloatBox.Value = 0F;
            this.defaultGravityYFloatBox.ValueChanged += new PlatformGameCreator.Editor.Winforms.FloatBox.ValueChangedHandler(this.defaultGravityYFloatBox_ValueChanged);
            // 
            // defaultGravityXFloatBox
            // 
            this.defaultGravityXFloatBox.Location = new System.Drawing.Point(121, 88);
            this.defaultGravityXFloatBox.Name = "defaultGravityXFloatBox";
            this.defaultGravityXFloatBox.Size = new System.Drawing.Size(108, 20);
            this.defaultGravityXFloatBox.TabIndex = 7;
            this.defaultGravityXFloatBox.Text = "0";
            this.defaultGravityXFloatBox.Value = 0F;
            this.defaultGravityXFloatBox.ValueChanged += new PlatformGameCreator.Editor.Winforms.FloatBox.ValueChangedHandler(this.defaultGravityXFloatBox_ValueChanged);
            // 
            // simulationUnitsFloatBox
            // 
            this.simulationUnitsFloatBox.Location = new System.Drawing.Point(101, 62);
            this.simulationUnitsFloatBox.Name = "simulationUnitsFloatBox";
            this.simulationUnitsFloatBox.Size = new System.Drawing.Size(128, 20);
            this.simulationUnitsFloatBox.TabIndex = 6;
            this.simulationUnitsFloatBox.Text = "0";
            this.simulationUnitsFloatBox.Value = 0F;
            this.simulationUnitsFloatBox.ValueChanged += new PlatformGameCreator.Editor.Winforms.FloatBox.ValueChangedHandler(this.simulationUnitsFloatBox_ValueChanged);
            // 
            // backgroundColorLabel
            // 
            this.backgroundColorLabel.AutoSize = true;
            this.backgroundColorLabel.Location = new System.Drawing.Point(12, 154);
            this.backgroundColorLabel.Name = "backgroundColorLabel";
            this.backgroundColorLabel.Size = new System.Drawing.Size(95, 13);
            this.backgroundColorLabel.TabIndex = 14;
            this.backgroundColorLabel.Text = "Background Color:";
            // 
            // backgroundColorRLabel
            // 
            this.backgroundColorRLabel.AutoSize = true;
            this.backgroundColorRLabel.Location = new System.Drawing.Point(118, 154);
            this.backgroundColorRLabel.Name = "backgroundColorRLabel";
            this.backgroundColorRLabel.Size = new System.Drawing.Size(18, 13);
            this.backgroundColorRLabel.TabIndex = 15;
            this.backgroundColorRLabel.Text = "R:";
            // 
            // backgroundColorGLabel
            // 
            this.backgroundColorGLabel.AutoSize = true;
            this.backgroundColorGLabel.Location = new System.Drawing.Point(118, 180);
            this.backgroundColorGLabel.Name = "backgroundColorGLabel";
            this.backgroundColorGLabel.Size = new System.Drawing.Size(18, 13);
            this.backgroundColorGLabel.TabIndex = 16;
            this.backgroundColorGLabel.Text = "G:";
            // 
            // backgroundColorBLabel
            // 
            this.backgroundColorBLabel.AutoSize = true;
            this.backgroundColorBLabel.Location = new System.Drawing.Point(118, 206);
            this.backgroundColorBLabel.Name = "backgroundColorBLabel";
            this.backgroundColorBLabel.Size = new System.Drawing.Size(17, 13);
            this.backgroundColorBLabel.TabIndex = 17;
            this.backgroundColorBLabel.Text = "B:";
            // 
            // backgroundColorRNumericUpDown
            // 
            this.backgroundColorRNumericUpDown.Location = new System.Drawing.Point(155, 152);
            this.backgroundColorRNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.backgroundColorRNumericUpDown.Name = "backgroundColorRNumericUpDown";
            this.backgroundColorRNumericUpDown.Size = new System.Drawing.Size(74, 20);
            this.backgroundColorRNumericUpDown.TabIndex = 18;
            this.backgroundColorRNumericUpDown.ValueChanged += new System.EventHandler(this.backgroundColorRNumericUpDown_ValueChanged);
            // 
            // backgroundColorGNumericUpDown
            // 
            this.backgroundColorGNumericUpDown.Location = new System.Drawing.Point(155, 178);
            this.backgroundColorGNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.backgroundColorGNumericUpDown.Name = "backgroundColorGNumericUpDown";
            this.backgroundColorGNumericUpDown.Size = new System.Drawing.Size(74, 20);
            this.backgroundColorGNumericUpDown.TabIndex = 19;
            this.backgroundColorGNumericUpDown.ValueChanged += new System.EventHandler(this.backgroundColorGNumericUpDown_ValueChanged);
            // 
            // backgroundColorBNumericUpDown
            // 
            this.backgroundColorBNumericUpDown.Location = new System.Drawing.Point(155, 204);
            this.backgroundColorBNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.backgroundColorBNumericUpDown.Name = "backgroundColorBNumericUpDown";
            this.backgroundColorBNumericUpDown.Size = new System.Drawing.Size(74, 20);
            this.backgroundColorBNumericUpDown.TabIndex = 20;
            this.backgroundColorBNumericUpDown.ValueChanged += new System.EventHandler(this.backgroundColorBNumericUpDown_ValueChanged);
            // 
            // continuousCollisionDetectionCheckBox
            // 
            this.continuousCollisionDetectionCheckBox.AutoSize = true;
            this.continuousCollisionDetectionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.continuousCollisionDetectionCheckBox.Location = new System.Drawing.Point(15, 235);
            this.continuousCollisionDetectionCheckBox.Name = "continuousCollisionDetectionCheckBox";
            this.continuousCollisionDetectionCheckBox.Size = new System.Drawing.Size(175, 17);
            this.continuousCollisionDetectionCheckBox.TabIndex = 21;
            this.continuousCollisionDetectionCheckBox.Text = "Continuous Collision Detection: ";
            this.continuousCollisionDetectionCheckBox.UseVisualStyleBackColor = true;
            this.continuousCollisionDetectionCheckBox.CheckedChanged += new System.EventHandler(this.continuousCollisionDetectionCheckBox_CheckedChanged);
            // 
            // fullScreenCheckBox
            // 
            this.fullScreenCheckBox.AutoSize = true;
            this.fullScreenCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fullScreenCheckBox.Location = new System.Drawing.Point(15, 258);
            this.fullScreenCheckBox.Name = "fullScreenCheckBox";
            this.fullScreenCheckBox.Size = new System.Drawing.Size(77, 17);
            this.fullScreenCheckBox.TabIndex = 22;
            this.fullScreenCheckBox.Text = "Fullscreen:";
            this.fullScreenCheckBox.UseVisualStyleBackColor = true;
            this.fullScreenCheckBox.CheckedChanged += new System.EventHandler(this.fullScreenCheckBox_CheckedChanged);
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 318);
            this.ControlBox = false;
            this.Controls.Add(this.fullScreenCheckBox);
            this.Controls.Add(this.continuousCollisionDetectionCheckBox);
            this.Controls.Add(this.backgroundColorBNumericUpDown);
            this.Controls.Add(this.backgroundColorGNumericUpDown);
            this.Controls.Add(this.backgroundColorRNumericUpDown);
            this.Controls.Add(this.backgroundColorBLabel);
            this.Controls.Add(this.backgroundColorGLabel);
            this.Controls.Add(this.backgroundColorRLabel);
            this.Controls.Add(this.backgroundColorLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.Controls.Add(this.defaultGravityYFloatBox);
            this.Controls.Add(this.defaultGravityXFloatBox);
            this.Controls.Add(this.simulationUnitsFloatBox);
            this.Controls.Add(this.gameWindowHeightNumericUpDown);
            this.Controls.Add(this.gameWindowWidthNumericUpDown);
            this.Controls.Add(this.defaultGravityLabel);
            this.Controls.Add(this.simulationUnitsLabel);
            this.Controls.Add(this.gameWindowLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GameSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Game Settings";
            ((System.ComponentModel.ISupportInitialize)(this.gameWindowWidthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameWindowHeightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorRNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorGNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorBNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label gameWindowLabel;
        private System.Windows.Forms.Label simulationUnitsLabel;
        private System.Windows.Forms.Label defaultGravityLabel;
        private System.Windows.Forms.NumericUpDown gameWindowWidthNumericUpDown;
        private System.Windows.Forms.NumericUpDown gameWindowHeightNumericUpDown;
        private PlatformGameCreator.Editor.Winforms.FloatBox simulationUnitsFloatBox;
        private PlatformGameCreator.Editor.Winforms.FloatBox defaultGravityXFloatBox;
        private PlatformGameCreator.Editor.Winforms.FloatBox defaultGravityYFloatBox;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label backgroundColorLabel;
        private System.Windows.Forms.Label backgroundColorRLabel;
        private System.Windows.Forms.Label backgroundColorGLabel;
        private System.Windows.Forms.Label backgroundColorBLabel;
        private System.Windows.Forms.NumericUpDown backgroundColorRNumericUpDown;
        private System.Windows.Forms.NumericUpDown backgroundColorGNumericUpDown;
        private System.Windows.Forms.NumericUpDown backgroundColorBNumericUpDown;
        private System.Windows.Forms.CheckBox continuousCollisionDetectionCheckBox;
        private System.Windows.Forms.CheckBox fullScreenCheckBox;
    }
}