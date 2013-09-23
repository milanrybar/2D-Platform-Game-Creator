/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Form for a visual scripting.
    /// </summary>
    partial class ScriptingForm : Form
    {
        /// <summary>
        /// Gets the underlying <see cref="Scripting.ScriptingComponent"/>.
        /// </summary>
        public ScriptingComponent ScriptingComponent
        {
            get { return _scriptingComponent; }
        }
        private ScriptingComponent _scriptingComponent;

        // view od state machines
        private StateMachineView stateMachineView;
        // screen for editing scripting nodes
        private ScriptingScreen scriptingScreen;

        // messages manager for the form
        private IMessagesManager messagesManager;

        /// <summary>
        /// Gets or sets the selected state machine.
        /// </summary>
        private StateMachine SelectedStateMachine
        {
            get { return _selectedStateMachine; }
            set
            {
                if (_selectedStateMachine != value)
                {
                    if (_selectedStateMachine != null)
                    {
                        stateMachineView.StateMachine = null;
                        scriptingScreen.State = null;
                        stateSettingsView.State = null;
                    }

                    _selectedStateMachine = value;

                    if (_selectedStateMachine != null)
                    {
                        stateMachineView.StateMachine = _selectedStateMachine;
                        scriptingScreen.State = _selectedStateMachine.StartingState != null ? _selectedStateMachine.StartingState : _selectedStateMachine.States[0];
                    }
                }
            }
        }
        private StateMachine _selectedStateMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingForm"/> class.
        /// </summary>
        /// <param name="scriptingComponent">The scripting component.</param>
        public ScriptingForm(ScriptingComponent scriptingComponent)
        {
            _scriptingComponent = scriptingComponent;

            InitializeComponent();

            Icon = Properties.Resources._2DPGC_Logo;

            if (ScriptingComponent.Actor != null)
            {
                Text = String.Format("Visual Scripting Editor - Actor - {0}", ScriptingComponent.Actor.Name);
                ScriptingComponent.Actor.NameChanged += new EventHandler(Actor_NameChanged);
            }
            else
            {
                Text = "Visual Scripting Editor - Scene Scripting";
            }

            messagesManager = new DefaultMessagesManager(toolStripStatusLabel);
            Messages.MessagesManager = messagesManager;

            // create scripting nodes tree
            CreateTreeOfScriptingNodes(nodesView.Nodes, ScriptingNodes.Root);
            CreateTreeOfVariablesNodes(nodesView.Nodes);
            nodesView.ExpandAll();

            // create scripting screen
            scriptingScreen = new ScriptingScreen();
            scriptingScreen.Dock = DockStyle.Fill;
            toolStripContainer2.ContentPanel.Controls.Add(scriptingScreen);

            scriptingScreen.SelectedNodesChanged += new EventHandler(scriptingScreen_SelectedNodesChanged);

            // create state machine screen
            stateMachineView = new StateMachineView();
            stateMachineView.Dock = DockStyle.Fill;
            toolStripContainer3.ContentPanel.Controls.Add(stateMachineView);

            stateMachineView.SelectedStateChanged += new EventHandler(stateView_SelectedStateChanged);
            stateMachineView.StateDoubleClicked += new EventHandler(stateView_StateDoubleClicked);
            stateMachineView.AfterDeletingState += new EventHandler(stateMachineView_AfterDeletingState);

            // init actor variables
            namedVariablesView.ScriptingComponent = scriptingComponent;
            namedVariablesView.VariableToScene += new NamedVariablesView.AddVariableToScene(namedVariablesView_VariableToScene);

            // init actor events
            eventsInView.Events = scriptingComponent.EventsIn;
            eventsInView.OnEventRemoving += new EventsView.EventRemovingHandler(eventsInView_OnEventRemoved);
            eventsOutView.Events = scriptingComponent.EventsOut;

            // init state machines view
            stateMachinesView.StateMachines = ScriptingComponent.StateMachines;
            stateMachinesView.OpenStateMachine += new StateMachinesView.StateMachineHandler(stateMachinesView_OpenStateMachine);
            stateMachinesView.AfterDeletingStateMachine += new StateMachinesView.StateMachineHandler(stateMachinesView_AfterDeletingStateMachine);

            SelectedStateMachine = scriptingComponent.StateMachines[0];
        }

        /// <summary>
        /// Handles the AfterDeletingState event of the stateMachineView control.
        /// Updates the state of the scriptingScreen, if needed.
        /// </summary>
        private void stateMachineView_AfterDeletingState(object sender, EventArgs e)
        {
            stateView_StateDoubleClicked(null, null);
            stateMachineView.Invalidate();
        }

        /// <summary>
        /// Handles the AfterDeletingStateMachine event of the stateMachinesView control.
        /// Change the state machine if the deleted one is the selected one.
        /// </summary>
        private void stateMachinesView_AfterDeletingStateMachine(object sender, StateMachine stateMachine)
        {
            if (SelectedStateMachine == stateMachine)
            {
                SelectedStateMachine = ScriptingComponent.StateMachines[0];

                stateMachineView.Invalidate();
                scriptingScreen.Invalidate();
            }
        }

        /// <summary>
        /// Handles the OpenStateMachine event of the stateMachinesView control.
        /// Selects the specified state machine.
        /// </summary>
        private void stateMachinesView_OpenStateMachine(object sender, StateMachine stateMachine)
        {
            SelectedStateMachine = stateMachine;

            stateMachineView.Invalidate();
            scriptingScreen.Invalidate();
        }

        /// <summary>
        /// Underlyings collection changes from the outside.
        /// Informs used <see cref="ScriptingScreen"/> about the change.
        /// </summary>
        public void UnderlyingCollectionChange()
        {
            scriptingScreen.UnderlyingCollectionChange();
        }

        /// <summary>
        /// Handles the NameChanged event of the Actor of ScriptingComponent.
        /// Updates the title of the form.
        /// </summary>
        private void Actor_NameChanged(object sender, EventArgs e)
        {
            Text = String.Format("Visual Scripting Editor - Actor - {0}", ScriptingComponent.Actor.Name);
        }

        /// <summary>
        /// Handles the OnEventRemoved event of the eventsInView control.
        /// Removes the specified event in from all states of the state machine.
        /// </summary>
        private void eventsInView_OnEventRemoved(object sender, Event scriptEvent)
        {
            foreach (StateMachine stateMachine in ScriptingComponent.StateMachines)
            {
                foreach (State state in stateMachine.States)
                {
                    foreach (Transition transition in state.Transitions)
                    {
                        if (transition.Event == scriptEvent) transition.Event = null;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the StateDoubleClicked event of the stateView control.
        /// Updates the state of the scriptingScreen, if needed.
        /// </summary>
        private void stateView_StateDoubleClicked(object sender, EventArgs e)
        {
            if (stateMachineView.SelectedState != null && stateMachineView.SelectedState.State != scriptingScreen.State)
            {
                scriptingScreen.State = stateMachineView.SelectedState.State;
                scriptingScreen.Invalidate();
            }
        }

        /// <summary>
        /// Handles the SelectedStateChanged event of the stateView control.
        /// Updates the view of state settings.
        /// </summary>
        private void stateView_SelectedStateChanged(object sender, EventArgs e)
        {
            if (stateMachineView.SelectedState != null && stateSettingsView.State != stateMachineView.SelectedState.State)
            {
                stateSettingsView.State = stateMachineView.SelectedState.State;
            }
            else if (stateMachineView.SelectedState == null && stateSettingsView.State != null)
            {
                stateSettingsView.State = null;
            }
        }

        /// <summary>
        /// Handles the VariableToScene event of the namedVariablesView control.
        /// Adds the specified named variable to the scripting screen.
        /// </summary>
        private void namedVariablesView_VariableToScene(NamedVariable variable)
        {
            if (variable != null)
            {
                scriptingScreen.AddSceneNodeToCenter((new Variable(scriptingScreen.State, variable)).CreateView());

                scriptingScreen.Invalidate();
            }
        }

        /// <summary>
        /// Handles the SelectedNodesChanged event of the scriptingScreen control.
        /// Updates the view of settings of the selected nodes of the scriptingScreen control. 
        /// </summary>
        private void scriptingScreen_SelectedNodesChanged(object sender, EventArgs e)
        {
            nodeSettingsView1.ClearRows();

            int count = 0;
            SceneNode sceneNodeForSettings = null;
            foreach (SceneNode sceneNode in scriptingScreen.SelectedNodes)
            {
                if (sceneNode.CanEditSettings)
                {
                    count++;
                    sceneNodeForSettings = sceneNode;

                    sceneNode.IEditSettings.ShowSettings(nodeSettingsView1.table.Rows);
                }
            }
        }

        /// <summary>
        /// Creates the tree representing all scripting nodes.
        /// </summary>
        /// <param name="treeNodes">Collection where to store scripting nodes.</param>
        /// <param name="category">The category of scripting nodes.</param>
        private void CreateTreeOfScriptingNodes(TreeNodeCollection treeNodes, CategoryData category)
        {
            foreach (ScriptData baseNode in category.Items)
            {
                // category
                CategoryData cat = baseNode as CategoryData;
                if (cat != null)
                {
                    TreeNode newCategoryNode = new TreeNode(cat.Name) { Tag = null };

                    treeNodes.Add(newCategoryNode);

                    CreateTreeOfScriptingNodes(newCategoryNode.Nodes, cat);

                    continue;
                }

                // node
                NodeData node = baseNode as NodeData;
                if (node != null)
                {
                    Node guiNode = (Node)new Node(null, node);

                    treeNodes.Add(new TreeNode(guiNode.Name) { Tag = guiNode });

                    continue;
                }

                throw new Exception();
            }
        }

        /// <summary>
        /// Creates the tree representing all scripting variables.
        /// </summary>
        /// <param name="treeNodes">Collection where to store scripting variables.</param>
        private void CreateTreeOfVariablesNodes(TreeNodeCollection treeNodes)
        {
            TreeNode variablesCategory = new TreeNode("Variables") { Tag = null };

            foreach (VariableType variableType in Enum.GetValues(typeof(VariableType)))
            {
                Variable variableNode = new Variable(null, variableType);

                if (variableType == VariableType.String) variableNode.Value.SetValue("string");

                variablesCategory.Nodes.Add(new TreeNode(VariableTypeHelper.FriendlyName(variableType)) { Tag = variableNode });
            }

            treeNodes.Add(variablesCategory);
        }

        /// <summary>
        /// Handles the MouseDown event of the nodesView control.
        /// Begins the drag drop operation, if the left mouse button.
        /// </summary>
        private void nodesView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = nodesView.GetNodeAt(e.Location);
                if (node != null && node.Tag != null)
                {
                    nodesView.SelectedNode = node;

                    DataObject dataObject = new DataObject();
                    dataObject.SetData(typeof(BaseNode), node.Tag);
                    DragDropEffects dragDrop = DoDragDrop(dataObject, DragDropEffects.All);
                }
            }
        }

        /// <summary>
        /// Handles the DropDownOpening event of the addVariableDropDownButton control.
        /// Shows global variables and variables of the actor selected at the scene.
        /// </summary>
        private void addVariableDropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            globalVariableToolStripMenuItem.DropDownItems.Clear();
            selectedActorVariableToolStripMenuItem.DropDownItems.Clear();

            // global variables
            foreach (NamedVariable variable in Project.Singleton.Scenes.SelectedScene.GlobalScript.Variables)
            {
                globalVariableToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(variable.Name) { Tag = variable });
                globalVariableToolStripMenuItem.DropDownItems[globalVariableToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(addVariableToolStripMenuItem_Click);
            }

            // selected actor at the scene variables
            if (EditorApplicationForm.SceneSingleton.SelectedNodes.Count == 1 && EditorApplicationForm.SceneSingleton.SelectedNodes[0] is ActorView)
            {
                Actor selectedActor = ((ActorView)EditorApplicationForm.SceneSingleton.SelectedNodes[0]).Actor;

                if (selectedActor != null)
                {
                    foreach (NamedVariable variable in selectedActor.Scripting.Variables)
                    {
                        selectedActorVariableToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(variable.Name) { Tag = variable });
                        selectedActorVariableToolStripMenuItem.DropDownItems[selectedActorVariableToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(addVariableToolStripMenuItem_Click);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the addVariableToolStripMenuItem control.
        /// Adds the selected named variable to the scriting screen.
        /// </summary>
        private void addVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (item != null)
            {
                NamedVariable variable = item.Tag as NamedVariable;

                if (variable != null)
                {
                    scriptingScreen.AddSceneNodeToCenter((new Variable(scriptingScreen.State, variable)).CreateView());

                    scriptingScreen.Invalidate();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the newStateToolStripButton control.
        /// Creates and adds the new state to the selected state machine.
        /// </summary>
        private void newStateToolStripButton_Click(object sender, EventArgs e)
        {
            stateMachineView.CreateNewState();
        }

        /// <summary>
        /// Handles the Click event of the newStateToolStripButton addTransitionToolStripButton.
        /// Creates and adds the new transition to the selected state.
        /// </summary>
        private void addTransitionToolStripButton_Click(object sender, EventArgs e)
        {
            stateMachineView.CreateTransition();
        }

        /// <summary>
        /// Handles the Click event of the newStateToolStripButton undoScriptingToolStripButton.
        /// Undoes the history.
        /// </summary>
        private void undoScriptingToolStripButton_Click(object sender, EventArgs e)
        {
            scriptingScreen.UnderlyingCollectionChanging = true;
            scriptingScreen.History.Undo();
            scriptingScreen.Invalidate();
            scriptingScreen.UnderlyingCollectionChanging = false;
        }

        /// <summary>
        /// Handles the Click event of the newStateToolStripButton redoScriptingToolStripButton.
        /// Redoes the history.
        /// </summary>
        private void redoScriptingToolStripButton_Click(object sender, EventArgs e)
        {
            scriptingScreen.UnderlyingCollectionChanging = true;
            scriptingScreen.History.Redo();
            scriptingScreen.Invalidate();
            scriptingScreen.UnderlyingCollectionChanging = false;
        }

        /// <summary>
        /// Handles the Activated event of the ScriptingForm control.
        /// </summary>
        private void ScriptingForm_Activated(object sender, EventArgs e)
        {
            Messages.MessagesManager = messagesManager;
            SceneNode.DrawingTools = scriptingScreen;
        }

        /// <summary>
        /// Handles the Click event of the setStartingStateToolStripButton control.
        /// Sets the selected state as the starting state of the state machine.
        /// </summary>
        private void setStartingStateToolStripButton_Click(object sender, EventArgs e)
        {
            if (stateMachineView.SelectedState != null)
            {
                stateMachineView.StateMachine.StartingState = stateMachineView.SelectedState.State;
                stateMachineView.Invalidate();
            }
        }

        /// <summary>
        /// Handles the Click event of the fitToolStripButton control.
        /// Zooms and centers the scripting scene to be able to see the whole scene at the <see cref="ScriptingScreen"/>, if possible.
        /// </summary>
        private void fitToolStripButton_Click(object sender, EventArgs e)
        {
            scriptingScreen.Fit();
            scriptingScreen.Invalidate();
        }

        /// <summary>
        /// Handles the Click event of the addStateMachineButton control.
        /// Creates and adds the new state machine to the scripting component.
        /// </summary>
        private void addStateMachineButton_Click(object sender, EventArgs e)
        {
            StateMachine newStateMachine = new StateMachine(ScriptingComponent);
            newStateMachine.Name = "New State Machine";
            newStateMachine.States.Add(new State(newStateMachine) { Name = "Default", Location = new Point(100, 100) });
            newStateMachine.StartingState = newStateMachine.States[0];

            ScriptingComponent.StateMachines.Add(newStateMachine);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SelectedStateMachine = null;

                if (ScriptingComponent.Actor != null)
                {
                    ScriptingComponent.Actor.NameChanged -= new EventHandler(Actor_NameChanged);
                }
                _scriptingComponent = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
