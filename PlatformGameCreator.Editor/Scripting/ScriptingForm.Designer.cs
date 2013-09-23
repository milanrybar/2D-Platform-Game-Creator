namespace PlatformGameCreator.Editor.Scripting
{
    partial class ScriptingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptingForm));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.scriptingTabControl = new System.Windows.Forms.TabControl();
            this.scriptingSettingsTabPage = new System.Windows.Forms.TabPage();
            this.nodeSettingsView1 = new PlatformGameCreator.Editor.Scripting.NodeSettingsView();
            this.actorVariablesTabPage = new System.Windows.Forms.TabPage();
            this.namedVariablesView = new PlatformGameCreator.Editor.Scripting.NamedVariablesView();
            this.stateMachineTabControl = new System.Windows.Forms.TabControl();
            this.stateTabPage = new System.Windows.Forms.TabPage();
            this.stateSettingsView = new PlatformGameCreator.Editor.Scripting.StateSettingsView();
            this.eventsInTabPage = new System.Windows.Forms.TabPage();
            this.eventsInView = new PlatformGameCreator.Editor.Scripting.EventsView();
            this.eventsOutTabPage = new System.Windows.Forms.TabPage();
            this.eventsOutView = new PlatformGameCreator.Editor.Scripting.EventsView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer3 = new System.Windows.Forms.ToolStripContainer();
            this.stateMachineViewToolStrip = new System.Windows.Forms.ToolStrip();
            this.newStateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.addTransitionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.setStartingStateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.scriptingScreenToolStrip = new System.Windows.Forms.ToolStrip();
            this.undoScriptingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.redoScriptingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addVariableDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.globalVariableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedActorVariableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.fitToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rightTabControl = new System.Windows.Forms.TabControl();
            this.nodesTabPage = new System.Windows.Forms.TabPage();
            this.nodesView = new System.Windows.Forms.TreeView();
            this.stateMachinesTabPage = new System.Windows.Forms.TabPage();
            this.stateMachinesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.stateMachinesView = new PlatformGameCreator.Editor.Scripting.StateMachinesView();
            this.addStateMachineButton = new System.Windows.Forms.Button();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.scriptingTabControl.SuspendLayout();
            this.scriptingSettingsTabPage.SuspendLayout();
            this.actorVariablesTabPage.SuspendLayout();
            this.stateMachineTabControl.SuspendLayout();
            this.stateTabPage.SuspendLayout();
            this.eventsInTabPage.SuspendLayout();
            this.eventsOutTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripContainer3.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer3.SuspendLayout();
            this.stateMachineViewToolStrip.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.scriptingScreenToolStrip.SuspendLayout();
            this.rightTabControl.SuspendLayout();
            this.nodesTabPage.SuspendLayout();
            this.stateMachinesTabPage.SuspendLayout();
            this.stateMachinesTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.tableLayoutPanel2);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1084, 615);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size(1084, 662);
            this.toolStripContainer.TabIndex = 3;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1084, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(48, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 202F));
            this.tableLayoutPanel2.Controls.Add(this.scriptingTabControl, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.stateMachineTabControl, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rightTabControl, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1084, 615);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // scriptingTabControl
            // 
            this.scriptingTabControl.Controls.Add(this.scriptingSettingsTabPage);
            this.scriptingTabControl.Controls.Add(this.actorVariablesTabPage);
            this.scriptingTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptingTabControl.Location = new System.Drawing.Point(444, 438);
            this.scriptingTabControl.Name = "scriptingTabControl";
            this.scriptingTabControl.SelectedIndex = 0;
            this.scriptingTabControl.Size = new System.Drawing.Size(435, 174);
            this.scriptingTabControl.TabIndex = 3;
            // 
            // scriptingSettingsTabPage
            // 
            this.scriptingSettingsTabPage.Controls.Add(this.nodeSettingsView1);
            this.scriptingSettingsTabPage.Location = new System.Drawing.Point(4, 22);
            this.scriptingSettingsTabPage.Name = "scriptingSettingsTabPage";
            this.scriptingSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scriptingSettingsTabPage.Size = new System.Drawing.Size(427, 148);
            this.scriptingSettingsTabPage.TabIndex = 0;
            this.scriptingSettingsTabPage.Text = "VS Settings";
            this.scriptingSettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // nodeSettingsView1
            // 
            this.nodeSettingsView1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.nodeSettingsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeSettingsView1.Location = new System.Drawing.Point(3, 3);
            this.nodeSettingsView1.Margin = new System.Windows.Forms.Padding(0);
            this.nodeSettingsView1.Name = "nodeSettingsView1";
            this.nodeSettingsView1.Padding = new System.Windows.Forms.Padding(1);
            this.nodeSettingsView1.Size = new System.Drawing.Size(421, 142);
            this.nodeSettingsView1.TabIndex = 2;
            // 
            // actorVariablesTabPage
            // 
            this.actorVariablesTabPage.Controls.Add(this.namedVariablesView);
            this.actorVariablesTabPage.Location = new System.Drawing.Point(4, 22);
            this.actorVariablesTabPage.Name = "actorVariablesTabPage";
            this.actorVariablesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.actorVariablesTabPage.Size = new System.Drawing.Size(427, 148);
            this.actorVariablesTabPage.TabIndex = 1;
            this.actorVariablesTabPage.Text = "Actor Variables";
            this.actorVariablesTabPage.UseVisualStyleBackColor = true;
            // 
            // namedVariablesView
            // 
            this.namedVariablesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namedVariablesView.Location = new System.Drawing.Point(3, 3);
            this.namedVariablesView.Margin = new System.Windows.Forms.Padding(0);
            this.namedVariablesView.Name = "namedVariablesView";
            this.namedVariablesView.ScriptingComponent = null;
            this.namedVariablesView.Size = new System.Drawing.Size(421, 142);
            this.namedVariablesView.TabIndex = 3;
            // 
            // stateMachineTabControl
            // 
            this.stateMachineTabControl.Controls.Add(this.stateTabPage);
            this.stateMachineTabControl.Controls.Add(this.eventsInTabPage);
            this.stateMachineTabControl.Controls.Add(this.eventsOutTabPage);
            this.stateMachineTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateMachineTabControl.Location = new System.Drawing.Point(3, 438);
            this.stateMachineTabControl.Name = "stateMachineTabControl";
            this.stateMachineTabControl.SelectedIndex = 0;
            this.stateMachineTabControl.Size = new System.Drawing.Size(435, 174);
            this.stateMachineTabControl.TabIndex = 4;
            // 
            // stateTabPage
            // 
            this.stateTabPage.Controls.Add(this.stateSettingsView);
            this.stateTabPage.Location = new System.Drawing.Point(4, 22);
            this.stateTabPage.Name = "stateTabPage";
            this.stateTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stateTabPage.Size = new System.Drawing.Size(427, 148);
            this.stateTabPage.TabIndex = 0;
            this.stateTabPage.Text = "State";
            this.stateTabPage.UseVisualStyleBackColor = true;
            // 
            // stateSettingsView
            // 
            this.stateSettingsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateSettingsView.Location = new System.Drawing.Point(3, 3);
            this.stateSettingsView.Margin = new System.Windows.Forms.Padding(0);
            this.stateSettingsView.Name = "stateSettingsView";
            this.stateSettingsView.Size = new System.Drawing.Size(421, 142);
            this.stateSettingsView.State = null;
            this.stateSettingsView.TabIndex = 0;
            // 
            // eventsInTabPage
            // 
            this.eventsInTabPage.Controls.Add(this.eventsInView);
            this.eventsInTabPage.Location = new System.Drawing.Point(4, 22);
            this.eventsInTabPage.Name = "eventsInTabPage";
            this.eventsInTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.eventsInTabPage.Size = new System.Drawing.Size(427, 148);
            this.eventsInTabPage.TabIndex = 1;
            this.eventsInTabPage.Text = "Events In";
            this.eventsInTabPage.UseVisualStyleBackColor = true;
            // 
            // eventsInView
            // 
            this.eventsInView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsInView.Events = null;
            this.eventsInView.Location = new System.Drawing.Point(3, 3);
            this.eventsInView.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.eventsInView.Name = "eventsInView";
            this.eventsInView.Size = new System.Drawing.Size(421, 142);
            this.eventsInView.TabIndex = 4;
            // 
            // eventsOutTabPage
            // 
            this.eventsOutTabPage.Controls.Add(this.eventsOutView);
            this.eventsOutTabPage.Location = new System.Drawing.Point(4, 22);
            this.eventsOutTabPage.Name = "eventsOutTabPage";
            this.eventsOutTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.eventsOutTabPage.Size = new System.Drawing.Size(427, 148);
            this.eventsOutTabPage.TabIndex = 2;
            this.eventsOutTabPage.Text = "Events Out";
            this.eventsOutTabPage.UseVisualStyleBackColor = true;
            // 
            // eventsOutView
            // 
            this.eventsOutView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsOutView.Events = null;
            this.eventsOutView.Location = new System.Drawing.Point(3, 3);
            this.eventsOutView.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.eventsOutView.Name = "eventsOutView";
            this.eventsOutView.Size = new System.Drawing.Size(421, 142);
            this.eventsOutView.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.splitContainer1, 2);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.toolStripContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(876, 429);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 5;
            // 
            // toolStripContainer3
            // 
            this.toolStripContainer3.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer3.ContentPanel
            // 
            this.toolStripContainer3.ContentPanel.Size = new System.Drawing.Size(289, 402);
            this.toolStripContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer3.LeftToolStripPanelVisible = false;
            this.toolStripContainer3.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer3.Name = "toolStripContainer3";
            this.toolStripContainer3.RightToolStripPanelVisible = false;
            this.toolStripContainer3.Size = new System.Drawing.Size(289, 427);
            this.toolStripContainer3.TabIndex = 0;
            this.toolStripContainer3.Text = "toolStripContainer3";
            // 
            // toolStripContainer3.TopToolStripPanel
            // 
            this.toolStripContainer3.TopToolStripPanel.Controls.Add(this.stateMachineViewToolStrip);
            // 
            // stateMachineViewToolStrip
            // 
            this.stateMachineViewToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.stateMachineViewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStateToolStripButton,
            this.addTransitionToolStripButton,
            this.setStartingStateToolStripButton});
            this.stateMachineViewToolStrip.Location = new System.Drawing.Point(3, 0);
            this.stateMachineViewToolStrip.Name = "stateMachineViewToolStrip";
            this.stateMachineViewToolStrip.Size = new System.Drawing.Size(246, 25);
            this.stateMachineViewToolStrip.TabIndex = 0;
            // 
            // newStateToolStripButton
            // 
            this.newStateToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newStateToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newStateToolStripButton.Image")));
            this.newStateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newStateToolStripButton.Name = "newStateToolStripButton";
            this.newStateToolStripButton.Size = new System.Drawing.Size(64, 22);
            this.newStateToolStripButton.Text = "New State";
            this.newStateToolStripButton.Click += new System.EventHandler(this.newStateToolStripButton_Click);
            // 
            // addTransitionToolStripButton
            // 
            this.addTransitionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addTransitionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addTransitionToolStripButton.Image")));
            this.addTransitionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addTransitionToolStripButton.Name = "addTransitionToolStripButton";
            this.addTransitionToolStripButton.Size = new System.Drawing.Size(89, 22);
            this.addTransitionToolStripButton.Text = "Add Transition";
            this.addTransitionToolStripButton.Click += new System.EventHandler(this.addTransitionToolStripButton_Click);
            // 
            // setStartingStateToolStripButton
            // 
            this.setStartingStateToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setStartingStateToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("setStartingStateToolStripButton.Image")));
            this.setStartingStateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setStartingStateToolStripButton.Name = "setStartingStateToolStripButton";
            this.setStartingStateToolStripButton.Size = new System.Drawing.Size(81, 22);
            this.setStartingStateToolStripButton.Text = "Starting State";
            this.setStartingStateToolStripButton.Click += new System.EventHandler(this.setStartingStateToolStripButton_Click);
            // 
            // toolStripContainer2
            // 
            this.toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(579, 402);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.LeftToolStripPanelVisible = false;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.RightToolStripPanelVisible = false;
            this.toolStripContainer2.Size = new System.Drawing.Size(579, 427);
            this.toolStripContainer2.TabIndex = 0;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.scriptingScreenToolStrip);
            // 
            // scriptingScreenToolStrip
            // 
            this.scriptingScreenToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.scriptingScreenToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoScriptingToolStripButton,
            this.redoScriptingToolStripButton,
            this.toolStripSeparator1,
            this.addVariableDropDownButton,
            this.toolStripSeparator2,
            this.fitToolStripButton});
            this.scriptingScreenToolStrip.Location = new System.Drawing.Point(3, 0);
            this.scriptingScreenToolStrip.Name = "scriptingScreenToolStrip";
            this.scriptingScreenToolStrip.Size = new System.Drawing.Size(280, 25);
            this.scriptingScreenToolStrip.TabIndex = 0;
            // 
            // undoScriptingToolStripButton
            // 
            this.undoScriptingToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoScriptingToolStripButton.Image = global::PlatformGameCreator.Editor.Properties.Resources.Edit_UndoHS;
            this.undoScriptingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoScriptingToolStripButton.Name = "undoScriptingToolStripButton";
            this.undoScriptingToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.undoScriptingToolStripButton.Text = "Undo";
            this.undoScriptingToolStripButton.Click += new System.EventHandler(this.undoScriptingToolStripButton_Click);
            // 
            // redoScriptingToolStripButton
            // 
            this.redoScriptingToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoScriptingToolStripButton.Image = global::PlatformGameCreator.Editor.Properties.Resources.Edit_RedoHS;
            this.redoScriptingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redoScriptingToolStripButton.Name = "redoScriptingToolStripButton";
            this.redoScriptingToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.redoScriptingToolStripButton.Text = "Redo";
            this.redoScriptingToolStripButton.Click += new System.EventHandler(this.redoScriptingToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // addVariableDropDownButton
            // 
            this.addVariableDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addVariableDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalVariableToolStripMenuItem,
            this.selectedActorVariableToolStripMenuItem});
            this.addVariableDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("addVariableDropDownButton.Image")));
            this.addVariableDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addVariableDropDownButton.Name = "addVariableDropDownButton";
            this.addVariableDropDownButton.Size = new System.Drawing.Size(87, 22);
            this.addVariableDropDownButton.Text = "Add Variable";
            this.addVariableDropDownButton.DropDownOpening += new System.EventHandler(this.addVariableDropDownButton_DropDownOpening);
            // 
            // globalVariableToolStripMenuItem
            // 
            this.globalVariableToolStripMenuItem.Name = "globalVariableToolStripMenuItem";
            this.globalVariableToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.globalVariableToolStripMenuItem.Text = "Global Variable";
            // 
            // selectedActorVariableToolStripMenuItem
            // 
            this.selectedActorVariableToolStripMenuItem.Name = "selectedActorVariableToolStripMenuItem";
            this.selectedActorVariableToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.selectedActorVariableToolStripMenuItem.Text = "Selected Actor Variable";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // fitToolStripButton
            // 
            this.fitToolStripButton.Image = global::PlatformGameCreator.Editor.Properties.Resources.view;
            this.fitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fitToolStripButton.Name = "fitToolStripButton";
            this.fitToolStripButton.Size = new System.Drawing.Size(92, 22);
            this.fitToolStripButton.Text = "Zoom To Fit";
            this.fitToolStripButton.Click += new System.EventHandler(this.fitToolStripButton_Click);
            // 
            // rightTabControl
            // 
            this.rightTabControl.Controls.Add(this.nodesTabPage);
            this.rightTabControl.Controls.Add(this.stateMachinesTabPage);
            this.rightTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightTabControl.Location = new System.Drawing.Point(885, 3);
            this.rightTabControl.Name = "rightTabControl";
            this.tableLayoutPanel2.SetRowSpan(this.rightTabControl, 2);
            this.rightTabControl.SelectedIndex = 0;
            this.rightTabControl.Size = new System.Drawing.Size(196, 609);
            this.rightTabControl.TabIndex = 6;
            // 
            // nodesTabPage
            // 
            this.nodesTabPage.Controls.Add(this.nodesView);
            this.nodesTabPage.Location = new System.Drawing.Point(4, 22);
            this.nodesTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.nodesTabPage.Name = "nodesTabPage";
            this.nodesTabPage.Size = new System.Drawing.Size(188, 583);
            this.nodesTabPage.TabIndex = 0;
            this.nodesTabPage.Text = "Nodes";
            this.nodesTabPage.UseVisualStyleBackColor = true;
            // 
            // nodesView
            // 
            this.nodesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodesView.Location = new System.Drawing.Point(0, 0);
            this.nodesView.Name = "nodesView";
            this.nodesView.Size = new System.Drawing.Size(188, 583);
            this.nodesView.TabIndex = 3;
            this.nodesView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nodesView_MouseDown);
            // 
            // stateMachinesTabPage
            // 
            this.stateMachinesTabPage.Controls.Add(this.stateMachinesTableLayoutPanel);
            this.stateMachinesTabPage.Location = new System.Drawing.Point(4, 22);
            this.stateMachinesTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.stateMachinesTabPage.Name = "stateMachinesTabPage";
            this.stateMachinesTabPage.Size = new System.Drawing.Size(188, 583);
            this.stateMachinesTabPage.TabIndex = 1;
            this.stateMachinesTabPage.Text = "State Machines";
            this.stateMachinesTabPage.UseVisualStyleBackColor = true;
            // 
            // stateMachinesTableLayoutPanel
            // 
            this.stateMachinesTableLayoutPanel.ColumnCount = 1;
            this.stateMachinesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stateMachinesTableLayoutPanel.Controls.Add(this.stateMachinesView, 0, 0);
            this.stateMachinesTableLayoutPanel.Controls.Add(this.addStateMachineButton, 0, 1);
            this.stateMachinesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateMachinesTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.stateMachinesTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.stateMachinesTableLayoutPanel.Name = "stateMachinesTableLayoutPanel";
            this.stateMachinesTableLayoutPanel.RowCount = 2;
            this.stateMachinesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stateMachinesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stateMachinesTableLayoutPanel.Size = new System.Drawing.Size(188, 583);
            this.stateMachinesTableLayoutPanel.TabIndex = 1;
            // 
            // stateMachinesView
            // 
            this.stateMachinesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateMachinesView.Location = new System.Drawing.Point(0, 0);
            this.stateMachinesView.Margin = new System.Windows.Forms.Padding(0);
            this.stateMachinesView.Name = "stateMachinesView";
            this.stateMachinesView.Size = new System.Drawing.Size(188, 557);
            this.stateMachinesView.TabIndex = 0;
            // 
            // addStateMachineButton
            // 
            this.addStateMachineButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addStateMachineButton.Location = new System.Drawing.Point(0, 560);
            this.addStateMachineButton.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.addStateMachineButton.Name = "addStateMachineButton";
            this.addStateMachineButton.Size = new System.Drawing.Size(188, 23);
            this.addStateMachineButton.TabIndex = 1;
            this.addStateMachineButton.Text = "Add State Machine";
            this.addStateMachineButton.UseVisualStyleBackColor = true;
            this.addStateMachineButton.Click += new System.EventHandler(this.addStateMachineButton_Click);
            // 
            // ScriptingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 662);
            this.Controls.Add(this.toolStripContainer);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ScriptingForm";
            this.Text = "Visual Scripting Editor";
            this.Activated += new System.EventHandler(this.ScriptingForm_Activated);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.scriptingTabControl.ResumeLayout(false);
            this.scriptingSettingsTabPage.ResumeLayout(false);
            this.actorVariablesTabPage.ResumeLayout(false);
            this.stateMachineTabControl.ResumeLayout(false);
            this.stateTabPage.ResumeLayout(false);
            this.eventsInTabPage.ResumeLayout(false);
            this.eventsOutTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.PerformLayout();
            this.toolStripContainer3.ResumeLayout(false);
            this.toolStripContainer3.PerformLayout();
            this.stateMachineViewToolStrip.ResumeLayout(false);
            this.stateMachineViewToolStrip.PerformLayout();
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.scriptingScreenToolStrip.ResumeLayout(false);
            this.scriptingScreenToolStrip.PerformLayout();
            this.rightTabControl.ResumeLayout(false);
            this.nodesTabPage.ResumeLayout(false);
            this.stateMachinesTabPage.ResumeLayout(false);
            this.stateMachinesTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private NodeSettingsView nodeSettingsView1;
        private NamedVariablesView namedVariablesView;
        private EventsView eventsInView;
        private EventsView eventsOutView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabControl scriptingTabControl;
        private System.Windows.Forms.TabPage scriptingSettingsTabPage;
        private System.Windows.Forms.TabPage actorVariablesTabPage;
        private System.Windows.Forms.TabControl stateMachineTabControl;
        private System.Windows.Forms.TabPage stateTabPage;
        private System.Windows.Forms.TabPage eventsInTabPage;
        private System.Windows.Forms.TabPage eventsOutTabPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStrip scriptingScreenToolStrip;
        private System.Windows.Forms.ToolStripButton undoScriptingToolStripButton;
        private System.Windows.Forms.ToolStripContainer toolStripContainer3;
        private System.Windows.Forms.ToolStrip stateMachineViewToolStrip;
        private System.Windows.Forms.ToolStripButton newStateToolStripButton;
        private System.Windows.Forms.ToolStripButton addTransitionToolStripButton;
        private System.Windows.Forms.ToolStripButton redoScriptingToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private StateSettingsView stateSettingsView;
        private System.Windows.Forms.ToolStripButton setStartingStateToolStripButton;
        private System.Windows.Forms.ToolStripButton fitToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabControl rightTabControl;
        private System.Windows.Forms.TabPage nodesTabPage;
        private System.Windows.Forms.TreeView nodesView;
        private System.Windows.Forms.TabPage stateMachinesTabPage;
        private StateMachinesView stateMachinesView;
        private System.Windows.Forms.TableLayoutPanel stateMachinesTableLayoutPanel;
        private System.Windows.Forms.Button addStateMachineButton;
        private System.Windows.Forms.ToolStripDropDownButton addVariableDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem globalVariableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedActorVariableToolStripMenuItem;
    }
}