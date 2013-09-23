namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    partial class ActorInfo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActorInfo));
            this.makePrototypeButton = new System.Windows.Forms.Button();
            this.openScriptingButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.oneWayPlatformCheckBox = new System.Windows.Forms.CheckBox();
            this.fixedRotationCheckBox = new System.Windows.Forms.CheckBox();
            this.bulletCheckBox = new System.Windows.Forms.CheckBox();
            this.sensorCheckBox = new System.Windows.Forms.CheckBox();
            this.physicsTypeLabel = new System.Windows.Forms.Label();
            this.densityLabel = new System.Windows.Forms.Label();
            this.frictionLabel = new System.Windows.Forms.Label();
            this.restitutionLabel = new System.Windows.Forms.Label();
            this.linearDampingLabel = new System.Windows.Forms.Label();
            this.angularDampingLabel = new System.Windows.Forms.Label();
            this.drawableAssetLabel = new System.Windows.Forms.Label();
            this.drawableAssetVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.actorTypeComboBox = new System.Windows.Forms.ComboBox();
            this.physicsTypeComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.drawableAssetBox = new System.Windows.Forms.TextBox();
            this.physicsSettingsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.densityFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.frictionFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.restitutionFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.angularDampingFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.linearDampingFloatBox = new PlatformGameCreator.Editor.Winforms.FloatBox();
            this.tableLayoutPanel.SuspendLayout();
            this.physicsSettingsTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // makePrototypeButton
            // 
            this.makePrototypeButton.AutoSize = true;
            this.makePrototypeButton.Location = new System.Drawing.Point(0, 126);
            this.makePrototypeButton.Name = "makePrototypeButton";
            this.makePrototypeButton.Size = new System.Drawing.Size(191, 23);
            this.makePrototypeButton.TabIndex = 2;
            this.makePrototypeButton.Text = "Make Prototype";
            this.makePrototypeButton.UseVisualStyleBackColor = true;
            this.makePrototypeButton.Click += new System.EventHandler(this.makePrototypeButton_Click);
            // 
            // openScriptingButton
            // 
            this.openScriptingButton.AutoSize = true;
            this.openScriptingButton.Location = new System.Drawing.Point(0, 97);
            this.openScriptingButton.Name = "openScriptingButton";
            this.openScriptingButton.Size = new System.Drawing.Size(190, 23);
            this.openScriptingButton.TabIndex = 3;
            this.openScriptingButton.Text = "Open Visual Scripting";
            this.openScriptingButton.UseVisualStyleBackColor = true;
            this.openScriptingButton.Click += new System.EventHandler(this.openScriptingButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 6);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name:";
            this.toolTip.SetToolTip(this.nameLabel, "Name of the actor.");
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(3, 32);
            this.typeLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(34, 13);
            this.typeLabel.TabIndex = 1;
            this.typeLabel.Text = "Type:";
            this.toolTip.SetToolTip(this.typeLabel, "Type of the actor.");
            // 
            // nameTextBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.nameTextBox, 2);
            this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameTextBox.Location = new System.Drawing.Point(61, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(126, 20);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // oneWayPlatformCheckBox
            // 
            this.oneWayPlatformCheckBox.AutoSize = true;
            this.physicsSettingsTableLayoutPanel.SetColumnSpan(this.oneWayPlatformCheckBox, 2);
            this.oneWayPlatformCheckBox.Location = new System.Drawing.Point(3, 133);
            this.oneWayPlatformCheckBox.Name = "oneWayPlatformCheckBox";
            this.oneWayPlatformCheckBox.Size = new System.Drawing.Size(112, 17);
            this.oneWayPlatformCheckBox.TabIndex = 23;
            this.oneWayPlatformCheckBox.Text = "One Way Platform";
            this.toolTip.SetToolTip(this.oneWayPlatformCheckBox, "Simple one-way platform.\r\nNote: Works only for default gravity.");
            this.oneWayPlatformCheckBox.UseVisualStyleBackColor = true;
            // 
            // fixedRotationCheckBox
            // 
            this.fixedRotationCheckBox.AutoSize = true;
            this.physicsSettingsTableLayoutPanel.SetColumnSpan(this.fixedRotationCheckBox, 2);
            this.fixedRotationCheckBox.Location = new System.Drawing.Point(3, 156);
            this.fixedRotationCheckBox.Name = "fixedRotationCheckBox";
            this.fixedRotationCheckBox.Size = new System.Drawing.Size(94, 17);
            this.fixedRotationCheckBox.TabIndex = 24;
            this.fixedRotationCheckBox.Text = "Fixed Rotation";
            this.toolTip.SetToolTip(this.fixedRotationCheckBox, "Actor will not rotate, even under load.");
            this.fixedRotationCheckBox.UseVisualStyleBackColor = true;
            // 
            // bulletCheckBox
            // 
            this.bulletCheckBox.AutoSize = true;
            this.physicsSettingsTableLayoutPanel.SetColumnSpan(this.bulletCheckBox, 2);
            this.bulletCheckBox.Location = new System.Drawing.Point(3, 179);
            this.bulletCheckBox.Name = "bulletCheckBox";
            this.bulletCheckBox.Size = new System.Drawing.Size(52, 17);
            this.bulletCheckBox.TabIndex = 25;
            this.bulletCheckBox.Text = "Bullet";
            this.toolTip.SetToolTip(this.bulletCheckBox, "Fast moving objects can be labeled as bullets.");
            this.bulletCheckBox.UseVisualStyleBackColor = true;
            // 
            // sensorCheckBox
            // 
            this.sensorCheckBox.AutoSize = true;
            this.physicsSettingsTableLayoutPanel.SetColumnSpan(this.sensorCheckBox, 2);
            this.sensorCheckBox.Location = new System.Drawing.Point(3, 202);
            this.sensorCheckBox.Name = "sensorCheckBox";
            this.sensorCheckBox.Size = new System.Drawing.Size(59, 17);
            this.sensorCheckBox.TabIndex = 26;
            this.sensorCheckBox.Text = "Sensor";
            this.toolTip.SetToolTip(this.sensorCheckBox, "Sometimes game logic needs to know when two actors overlap yet there should be no" +
        " collision \r\nresponse. A sensor detects collision but does not produce a respons" +
        "e.");
            this.sensorCheckBox.UseVisualStyleBackColor = true;
            // 
            // physicsTypeLabel
            // 
            this.physicsTypeLabel.AutoSize = true;
            this.physicsTypeLabel.Location = new System.Drawing.Point(3, 170);
            this.physicsTypeLabel.Name = "physicsTypeLabel";
            this.physicsTypeLabel.Size = new System.Drawing.Size(73, 13);
            this.physicsTypeLabel.TabIndex = 8;
            this.physicsTypeLabel.Text = "Physics Type:";
            this.toolTip.SetToolTip(this.physicsTypeLabel, "Physics type of the actor.");
            // 
            // densityLabel
            // 
            this.densityLabel.AutoSize = true;
            this.densityLabel.Location = new System.Drawing.Point(3, 6);
            this.densityLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.densityLabel.Name = "densityLabel";
            this.densityLabel.Size = new System.Drawing.Size(45, 13);
            this.densityLabel.TabIndex = 13;
            this.densityLabel.Text = "Density:";
            this.toolTip.SetToolTip(this.densityLabel, "Density is used to compute the mass properties of the actor body.");
            // 
            // frictionLabel
            // 
            this.frictionLabel.AutoSize = true;
            this.frictionLabel.Location = new System.Drawing.Point(3, 32);
            this.frictionLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.frictionLabel.Name = "frictionLabel";
            this.frictionLabel.Size = new System.Drawing.Size(44, 13);
            this.frictionLabel.TabIndex = 14;
            this.frictionLabel.Text = "Friction:";
            this.toolTip.SetToolTip(this.frictionLabel, resources.GetString("frictionLabel.ToolTip"));
            // 
            // restitutionLabel
            // 
            this.restitutionLabel.AutoSize = true;
            this.restitutionLabel.Location = new System.Drawing.Point(3, 58);
            this.restitutionLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.restitutionLabel.Name = "restitutionLabel";
            this.restitutionLabel.Size = new System.Drawing.Size(60, 13);
            this.restitutionLabel.TabIndex = 15;
            this.restitutionLabel.Text = "Restitution:";
            this.toolTip.SetToolTip(this.restitutionLabel, resources.GetString("restitutionLabel.ToolTip"));
            // 
            // linearDampingLabel
            // 
            this.linearDampingLabel.AutoSize = true;
            this.linearDampingLabel.Location = new System.Drawing.Point(3, 84);
            this.linearDampingLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.linearDampingLabel.Name = "linearDampingLabel";
            this.linearDampingLabel.Size = new System.Drawing.Size(84, 13);
            this.linearDampingLabel.TabIndex = 16;
            this.linearDampingLabel.Text = "Linear Damping:";
            this.toolTip.SetToolTip(this.linearDampingLabel, "Linear damping is used to reduce the linear velocity of the actor.");
            // 
            // angularDampingLabel
            // 
            this.angularDampingLabel.AutoSize = true;
            this.angularDampingLabel.Location = new System.Drawing.Point(3, 110);
            this.angularDampingLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.angularDampingLabel.Name = "angularDampingLabel";
            this.angularDampingLabel.Size = new System.Drawing.Size(91, 13);
            this.angularDampingLabel.TabIndex = 17;
            this.angularDampingLabel.Text = "Angular Damping:";
            this.toolTip.SetToolTip(this.angularDampingLabel, "Angular damping is used to reduce the angular velocity of the actor.");
            // 
            // drawableAssetLabel
            // 
            this.drawableAssetLabel.AutoSize = true;
            this.drawableAssetLabel.Location = new System.Drawing.Point(3, 59);
            this.drawableAssetLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.drawableAssetLabel.Name = "drawableAssetLabel";
            this.drawableAssetLabel.Size = new System.Drawing.Size(52, 13);
            this.drawableAssetLabel.TabIndex = 35;
            this.drawableAssetLabel.Text = "Graphics:";
            this.toolTip.SetToolTip(this.drawableAssetLabel, "Graphics of the actor. Texture or Animation.");
            // 
            // drawableAssetVisibleCheckBox
            // 
            this.drawableAssetVisibleCheckBox.AutoSize = true;
            this.drawableAssetVisibleCheckBox.Location = new System.Drawing.Point(172, 59);
            this.drawableAssetVisibleCheckBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.drawableAssetVisibleCheckBox.Name = "drawableAssetVisibleCheckBox";
            this.drawableAssetVisibleCheckBox.Size = new System.Drawing.Size(15, 14);
            this.drawableAssetVisibleCheckBox.TabIndex = 36;
            this.toolTip.SetToolTip(this.drawableAssetVisibleCheckBox, "Is graphics of the actor visible?");
            this.drawableAssetVisibleCheckBox.UseVisualStyleBackColor = true;
            this.drawableAssetVisibleCheckBox.CheckedChanged += new System.EventHandler(this.drawableAssetVisibleCheckBox_CheckedChanged);
            // 
            // actorTypeComboBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.actorTypeComboBox, 2);
            this.actorTypeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actorTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actorTypeComboBox.FormattingEnabled = true;
            this.actorTypeComboBox.Location = new System.Drawing.Point(61, 29);
            this.actorTypeComboBox.Name = "actorTypeComboBox";
            this.actorTypeComboBox.Size = new System.Drawing.Size(126, 21);
            this.actorTypeComboBox.TabIndex = 7;
            this.actorTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.actorTypeComboBox_SelectedIndexChanged);
            // 
            // physicsTypeComboBox
            // 
            this.physicsTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.physicsTypeComboBox.FormattingEnabled = true;
            this.physicsTypeComboBox.Location = new System.Drawing.Point(100, 167);
            this.physicsTypeComboBox.Name = "physicsTypeComboBox";
            this.physicsTypeComboBox.Size = new System.Drawing.Size(87, 21);
            this.physicsTypeComboBox.TabIndex = 7;
            this.physicsTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.physicsTypeComboBox_SelectedIndexChanged);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.drawableAssetBox, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.drawableAssetLabel, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.actorTypeComboBox, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.nameLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.typeLabel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.nameTextBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.drawableAssetVisibleCheckBox, 2, 2);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 5);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(190, 78);
            this.tableLayoutPanel.TabIndex = 6;
            // 
            // drawableAssetBox
            // 
            this.drawableAssetBox.AllowDrop = true;
            this.drawableAssetBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawableAssetBox.Location = new System.Drawing.Point(61, 56);
            this.drawableAssetBox.Name = "drawableAssetBox";
            this.drawableAssetBox.ReadOnly = true;
            this.drawableAssetBox.Size = new System.Drawing.Size(105, 20);
            this.drawableAssetBox.TabIndex = 34;
            this.drawableAssetBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.drawableAssetBox_DragDrop);
            this.drawableAssetBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.drawableAssetBox_DragEnter);
            // 
            // physicsSettingsTableLayoutPanel
            // 
            this.physicsSettingsTableLayoutPanel.ColumnCount = 2;
            this.physicsSettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.physicsSettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.sensorCheckBox, 0, 8);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.bulletCheckBox, 0, 7);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.fixedRotationCheckBox, 0, 6);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.oneWayPlatformCheckBox, 0, 5);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.densityLabel, 0, 0);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.densityFloatBox, 1, 0);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.frictionLabel, 0, 1);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.frictionFloatBox, 1, 1);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.restitutionLabel, 0, 2);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.restitutionFloatBox, 1, 2);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.linearDampingLabel, 0, 3);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.angularDampingFloatBox, 1, 4);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.linearDampingFloatBox, 1, 3);
            this.physicsSettingsTableLayoutPanel.Controls.Add(this.angularDampingLabel, 0, 4);
            this.physicsSettingsTableLayoutPanel.Location = new System.Drawing.Point(0, 194);
            this.physicsSettingsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.physicsSettingsTableLayoutPanel.Name = "physicsSettingsTableLayoutPanel";
            this.physicsSettingsTableLayoutPanel.RowCount = 9;
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.physicsSettingsTableLayoutPanel.Size = new System.Drawing.Size(190, 225);
            this.physicsSettingsTableLayoutPanel.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(3, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 2);
            this.label2.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(3, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 2);
            this.label1.TabIndex = 33;
            // 
            // densityFloatBox
            // 
            this.densityFloatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.densityFloatBox.Location = new System.Drawing.Point(100, 3);
            this.densityFloatBox.Name = "densityFloatBox";
            this.densityFloatBox.Size = new System.Drawing.Size(87, 20);
            this.densityFloatBox.TabIndex = 9;
            this.densityFloatBox.Text = "0";
            this.densityFloatBox.Value = 0F;
            // 
            // frictionFloatBox
            // 
            this.frictionFloatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frictionFloatBox.Location = new System.Drawing.Point(100, 29);
            this.frictionFloatBox.Name = "frictionFloatBox";
            this.frictionFloatBox.Size = new System.Drawing.Size(87, 20);
            this.frictionFloatBox.TabIndex = 10;
            this.frictionFloatBox.Text = "0";
            this.frictionFloatBox.Value = 0F;
            // 
            // restitutionFloatBox
            // 
            this.restitutionFloatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restitutionFloatBox.Location = new System.Drawing.Point(100, 55);
            this.restitutionFloatBox.Name = "restitutionFloatBox";
            this.restitutionFloatBox.Size = new System.Drawing.Size(87, 20);
            this.restitutionFloatBox.TabIndex = 11;
            this.restitutionFloatBox.Text = "0";
            this.restitutionFloatBox.Value = 0F;
            // 
            // angularDampingFloatBox
            // 
            this.angularDampingFloatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.angularDampingFloatBox.Location = new System.Drawing.Point(100, 107);
            this.angularDampingFloatBox.Name = "angularDampingFloatBox";
            this.angularDampingFloatBox.Size = new System.Drawing.Size(87, 20);
            this.angularDampingFloatBox.TabIndex = 18;
            this.angularDampingFloatBox.Text = "0";
            this.angularDampingFloatBox.Value = 0F;
            // 
            // linearDampingFloatBox
            // 
            this.linearDampingFloatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linearDampingFloatBox.Location = new System.Drawing.Point(100, 81);
            this.linearDampingFloatBox.Name = "linearDampingFloatBox";
            this.linearDampingFloatBox.Size = new System.Drawing.Size(87, 20);
            this.linearDampingFloatBox.TabIndex = 12;
            this.linearDampingFloatBox.Text = "0";
            this.linearDampingFloatBox.Value = 0F;
            // 
            // ActorInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.physicsSettingsTableLayoutPanel);
            this.Controls.Add(this.physicsTypeLabel);
            this.Controls.Add(this.physicsTypeComboBox);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.openScriptingButton);
            this.Controls.Add(this.makePrototypeButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ActorInfo";
            this.Size = new System.Drawing.Size(190, 436);
            this.ParentChanged += new System.EventHandler(this.ActorInfo_ParentChanged);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.physicsSettingsTableLayoutPanel.ResumeLayout(false);
            this.physicsSettingsTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button makePrototypeButton;
        private System.Windows.Forms.Button openScriptingButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox actorTypeComboBox;
        private System.Windows.Forms.ComboBox physicsTypeComboBox;
        private System.Windows.Forms.Label physicsTypeLabel;
        private PlatformGameCreator.Editor.Winforms.FloatBox densityFloatBox;
        private PlatformGameCreator.Editor.Winforms.FloatBox frictionFloatBox;
        private PlatformGameCreator.Editor.Winforms.FloatBox restitutionFloatBox;
        private PlatformGameCreator.Editor.Winforms.FloatBox linearDampingFloatBox;
        private System.Windows.Forms.Label densityLabel;
        private System.Windows.Forms.Label frictionLabel;
        private System.Windows.Forms.Label restitutionLabel;
        private System.Windows.Forms.Label linearDampingLabel;
        private System.Windows.Forms.Label angularDampingLabel;
        private PlatformGameCreator.Editor.Winforms.FloatBox angularDampingFloatBox;
        private System.Windows.Forms.CheckBox oneWayPlatformCheckBox;
        private System.Windows.Forms.CheckBox fixedRotationCheckBox;
        private System.Windows.Forms.CheckBox bulletCheckBox;
        private System.Windows.Forms.CheckBox sensorCheckBox;
        private System.Windows.Forms.TableLayoutPanel physicsSettingsTableLayoutPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox drawableAssetBox;
        private System.Windows.Forms.Label drawableAssetLabel;
        private System.Windows.Forms.CheckBox drawableAssetVisibleCheckBox;
    }
}
