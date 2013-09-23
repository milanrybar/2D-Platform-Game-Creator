namespace PlatformGameCreator.Editor.Scenes
{
    partial class LayerParallaxSettings
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
            this.parallaxCoefficientXFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.parallaxCoefficientYFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.parallaxCoefficientLabel = new System.Windows.Forms.Label();
            this.parallaxCoefficientXLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.parallaxCoefficientYLabel = new System.Windows.Forms.Label();
            this.graphicsEffectLabel = new System.Windows.Forms.Label();
            this.graphicsEffectRepeatHorizontallyCheckBox = new System.Windows.Forms.CheckBox();
            this.graphicsEffectRepeatVerticallyCheckBox = new System.Windows.Forms.CheckBox();
            this.graphicsEffectFillCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // parallaxCoefficientXFloatBox
            // 
            this.parallaxCoefficientXFloatBox.Location = new System.Drawing.Point(141, 6);
            this.parallaxCoefficientXFloatBox.Name = "parallaxCoefficientXFloatBox";
            this.parallaxCoefficientXFloatBox.Size = new System.Drawing.Size(100, 20);
            this.parallaxCoefficientXFloatBox.TabIndex = 0;
            this.parallaxCoefficientXFloatBox.Text = "0";
            this.parallaxCoefficientXFloatBox.Value = 0F;
            this.parallaxCoefficientXFloatBox.ValueChanged += new PlatformGameCreator.Editor.Winforms.FloatBox.ValueChangedHandler(this.parallaxCoefficientXFloatBox_ValueChanged);
            // 
            // parallaxCoefficientYFloatBox
            // 
            this.parallaxCoefficientYFloatBox.Location = new System.Drawing.Point(141, 32);
            this.parallaxCoefficientYFloatBox.Name = "parallaxCoefficientYFloatBox";
            this.parallaxCoefficientYFloatBox.Size = new System.Drawing.Size(100, 20);
            this.parallaxCoefficientYFloatBox.TabIndex = 1;
            this.parallaxCoefficientYFloatBox.Text = "0";
            this.parallaxCoefficientYFloatBox.Value = 0F;
            this.parallaxCoefficientYFloatBox.ValueChanged += new PlatformGameCreator.Editor.Winforms.FloatBox.ValueChangedHandler(this.parallaxCoefficientYFloatBox_ValueChanged);
            // 
            // parallaxCoefficientLabel
            // 
            this.parallaxCoefficientLabel.AutoSize = true;
            this.parallaxCoefficientLabel.Location = new System.Drawing.Point(12, 9);
            this.parallaxCoefficientLabel.Name = "parallaxCoefficientLabel";
            this.parallaxCoefficientLabel.Size = new System.Drawing.Size(100, 13);
            this.parallaxCoefficientLabel.TabIndex = 2;
            this.parallaxCoefficientLabel.Text = "Parallax Coefficient:";
            // 
            // parallaxCoefficientXLabel
            // 
            this.parallaxCoefficientXLabel.AutoSize = true;
            this.parallaxCoefficientXLabel.Location = new System.Drawing.Point(118, 9);
            this.parallaxCoefficientXLabel.Name = "parallaxCoefficientXLabel";
            this.parallaxCoefficientXLabel.Size = new System.Drawing.Size(17, 13);
            this.parallaxCoefficientXLabel.TabIndex = 3;
            this.parallaxCoefficientXLabel.Text = "X:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(141, 146);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // parallaxCoefficientYLabel
            // 
            this.parallaxCoefficientYLabel.AutoSize = true;
            this.parallaxCoefficientYLabel.Location = new System.Drawing.Point(118, 35);
            this.parallaxCoefficientYLabel.Name = "parallaxCoefficientYLabel";
            this.parallaxCoefficientYLabel.Size = new System.Drawing.Size(17, 13);
            this.parallaxCoefficientYLabel.TabIndex = 5;
            this.parallaxCoefficientYLabel.Text = "Y:";
            // 
            // graphicsEffectLabel
            // 
            this.graphicsEffectLabel.AutoSize = true;
            this.graphicsEffectLabel.Location = new System.Drawing.Point(12, 63);
            this.graphicsEffectLabel.Name = "graphicsEffectLabel";
            this.graphicsEffectLabel.Size = new System.Drawing.Size(83, 13);
            this.graphicsEffectLabel.TabIndex = 7;
            this.graphicsEffectLabel.Text = "Graphics Effect:";
            // 
            // graphicsEffectRepeatHorizontallyCheckBox
            // 
            this.graphicsEffectRepeatHorizontallyCheckBox.AutoSize = true;
            this.graphicsEffectRepeatHorizontallyCheckBox.Location = new System.Drawing.Point(121, 62);
            this.graphicsEffectRepeatHorizontallyCheckBox.Name = "graphicsEffectRepeatHorizontallyCheckBox";
            this.graphicsEffectRepeatHorizontallyCheckBox.Size = new System.Drawing.Size(118, 17);
            this.graphicsEffectRepeatHorizontallyCheckBox.TabIndex = 8;
            this.graphicsEffectRepeatHorizontallyCheckBox.Text = "Repeat Horizontally";
            this.graphicsEffectRepeatHorizontallyCheckBox.UseVisualStyleBackColor = true;
            this.graphicsEffectRepeatHorizontallyCheckBox.CheckedChanged += new System.EventHandler(this.graphicsEffectRepeatHorizontallyCheckBox_CheckedChanged);
            // 
            // graphicsEffectRepeatVerticallyCheckBox
            // 
            this.graphicsEffectRepeatVerticallyCheckBox.AutoSize = true;
            this.graphicsEffectRepeatVerticallyCheckBox.Location = new System.Drawing.Point(121, 85);
            this.graphicsEffectRepeatVerticallyCheckBox.Name = "graphicsEffectRepeatVerticallyCheckBox";
            this.graphicsEffectRepeatVerticallyCheckBox.Size = new System.Drawing.Size(106, 17);
            this.graphicsEffectRepeatVerticallyCheckBox.TabIndex = 9;
            this.graphicsEffectRepeatVerticallyCheckBox.Text = "Repeat Vertically";
            this.graphicsEffectRepeatVerticallyCheckBox.UseVisualStyleBackColor = true;
            this.graphicsEffectRepeatVerticallyCheckBox.CheckedChanged += new System.EventHandler(this.graphicsEffectRepeatVerticallyCheckBox_CheckedChanged);
            // 
            // graphicsEffectFillCheckBox
            // 
            this.graphicsEffectFillCheckBox.AutoSize = true;
            this.graphicsEffectFillCheckBox.Location = new System.Drawing.Point(121, 108);
            this.graphicsEffectFillCheckBox.Name = "graphicsEffectFillCheckBox";
            this.graphicsEffectFillCheckBox.Size = new System.Drawing.Size(38, 17);
            this.graphicsEffectFillCheckBox.TabIndex = 10;
            this.graphicsEffectFillCheckBox.Text = "Fill";
            this.graphicsEffectFillCheckBox.UseVisualStyleBackColor = true;
            this.graphicsEffectFillCheckBox.CheckedChanged += new System.EventHandler(this.graphicsEffectFillCheckBox_CheckedChanged);
            // 
            // LayerParallaxSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 181);
            this.ControlBox = false;
            this.Controls.Add(this.graphicsEffectFillCheckBox);
            this.Controls.Add(this.graphicsEffectRepeatVerticallyCheckBox);
            this.Controls.Add(this.graphicsEffectRepeatHorizontallyCheckBox);
            this.Controls.Add(this.graphicsEffectLabel);
            this.Controls.Add(this.parallaxCoefficientYLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.parallaxCoefficientXLabel);
            this.Controls.Add(this.parallaxCoefficientLabel);
            this.Controls.Add(this.parallaxCoefficientYFloatBox);
            this.Controls.Add(this.parallaxCoefficientXFloatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LayerParallaxSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Layer Parallax Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Winforms.FloatBox parallaxCoefficientXFloatBox;
        private Winforms.FloatBox parallaxCoefficientYFloatBox;
        private System.Windows.Forms.Label parallaxCoefficientLabel;
        private System.Windows.Forms.Label parallaxCoefficientXLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label parallaxCoefficientYLabel;
        private System.Windows.Forms.Label graphicsEffectLabel;
        private System.Windows.Forms.CheckBox graphicsEffectRepeatHorizontallyCheckBox;
        private System.Windows.Forms.CheckBox graphicsEffectRepeatVerticallyCheckBox;
        private System.Windows.Forms.CheckBox graphicsEffectFillCheckBox;
    }
}