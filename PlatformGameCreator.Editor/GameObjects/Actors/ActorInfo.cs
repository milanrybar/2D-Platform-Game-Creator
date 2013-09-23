/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Scripting;
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Assets;

namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    /// <summary>
    /// Control for editing <see cref="Actors.Actor"/> settings.
    /// Everything is done automatically by setting <see cref="ActorInfo.Actor"/> property.
    /// </summary>
    partial class ActorInfo : UserControl
    {
        /// <summary>
        /// Gets or sets the actor to edit.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Actor Actor
        {
            get { return _actor; }
            set
            {
                if (_actor != value)
                {
                    if (_actor != null)
                    {
                        _actor.NameChanged -= new EventHandler(Actor_NameChanged);
                        _actor.ParentChanged -= new ValueChangedHandler<Actor>(Actor_ParentChanged);
                        _actor.DrawableAssetChanged -= new ValueChangedHandler<DrawableAsset>(Actor_DrawableAssetChanged);

                        if (_actor.DrawableAsset != null) _actor.DrawableAsset.NameChanged -= new EventHandler(DrawableAsset_NameChanged);
                    }

                    _actor = value;

                    if (_actor != null)
                    {
                        _actor.NameChanged += new EventHandler(Actor_NameChanged);
                        _actor.ParentChanged += new ValueChangedHandler<Actor>(Actor_ParentChanged);
                        _actor.DrawableAssetChanged += new ValueChangedHandler<DrawableAsset>(Actor_DrawableAssetChanged);

                        if (_actor.DrawableAsset != null) _actor.DrawableAsset.NameChanged += new EventHandler(DrawableAsset_NameChanged);

                        UpdatePhysicsTypeSelection();
                        UpdatePhysicsSettingsVisible();

                        nameTextBox.Text = _actor.Name;
                        drawableAssetVisibleCheckBox.Checked = _actor.DrawableAssetVisible;
                        actorTypeComboBox.SelectedItem = _actor.Type;
                        physicsTypeComboBox.SelectedItem = _actor.Physics.Type;

                        densityFloatBox.Value = _actor.Physics.Density;
                        frictionFloatBox.Value = _actor.Physics.Friction;
                        restitutionFloatBox.Value = _actor.Physics.Restitution;
                        linearDampingFloatBox.Value = _actor.Physics.LinearDamping;
                        angularDampingFloatBox.Value = _actor.Physics.AngularDamping;

                        oneWayPlatformCheckBox.Checked = _actor.Physics.OneWayPlatform;
                        fixedRotationCheckBox.Checked = _actor.Physics.FixedRotation;
                        bulletCheckBox.Checked = _actor.Physics.Bullet;
                        sensorCheckBox.Checked = _actor.Physics.Sensor;

                        SetDrawableAssetName();
                    }
                }
            }
        }
        private Actor _actor;

        /// <summary>
        /// Physics types of an actor.
        /// </summary>
        private object[] allPhysicsTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorInfo"/> class.
        /// </summary>
        public ActorInfo()
        {
            InitializeComponent();

            // init actor types field
            actorTypeComboBox.DisplayMember = "NameWithLevelInfo";
            ActorTypes_ItemsChanged(null, null);
            Project.Singleton.ActorTypes.ItemsChanged += new EventHandler(ActorTypes_ItemsChanged);

            // init physics type field
            var physicsTypes = Enum.GetValues(typeof(PhysicsComponent.BodyPhysicsType));
            allPhysicsTypes = new object[physicsTypes.Length];
            for (int i = 0; i < physicsTypes.Length; ++i)
            {
                physicsTypeComboBox.Items.Add(physicsTypes.GetValue(i));
                allPhysicsTypes[i] = physicsTypes.GetValue(i);
            }

            densityFloatBox.ValueChanged += (float value) => { Actor.Physics.Density = value; };
            frictionFloatBox.ValueChanged += (float value) => { Actor.Physics.Friction = value; };
            restitutionFloatBox.ValueChanged += (float value) => { Actor.Physics.Restitution = value; };
            linearDampingFloatBox.ValueChanged += (float value) => { Actor.Physics.LinearDamping = value; };
            angularDampingFloatBox.ValueChanged += (float value) => { Actor.Physics.AngularDamping = value; };

            oneWayPlatformCheckBox.CheckedChanged += (object sender, EventArgs e) => { Actor.Physics.OneWayPlatform = oneWayPlatformCheckBox.Checked; };
            fixedRotationCheckBox.CheckedChanged += (object sender, EventArgs e) => { Actor.Physics.FixedRotation = fixedRotationCheckBox.Checked; };
            bulletCheckBox.CheckedChanged += (object sender, EventArgs e) => { Actor.Physics.Bullet = bulletCheckBox.Checked; };
            sensorCheckBox.CheckedChanged += (object sender, EventArgs e) => { Actor.Physics.Sensor = sensorCheckBox.Checked; };
        }

        /// <summary>
        /// Handles the <see cref="ActorTypesManager.ItemsChanged"/> event of the <see cref="ActorTypesManager"/> used at the current project.
        /// Updates combobox items of actor types.
        /// </summary>
        private void ActorTypes_ItemsChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender == Project.Singleton.ActorTypes || sender == null, "Invalid actor type items changed event.");

            foreach (ActorType actorType in actorTypeComboBox.Items)
            {
                actorType.NameChanged -= new EventHandler(ActorType_NameChanged);
            }

            actorTypeComboBox.Items.Clear();

            foreach (ActorType actorType in Project.Singleton.ActorTypes.Items)
            {
                actorTypeComboBox.Items.Add(actorType);
                actorType.NameChanged += new EventHandler(ActorType_NameChanged);
            }

            if (Actor != null)
            {
                actorTypeComboBox.SelectedItem = Actor.Type;
            }
        }

        /// <summary>
        /// Handles the <see cref="ActorType.NameChanged"/> event of the <see cref="ActorType"/>.
        /// Updates the name of the specified actor type in combobox items of actor types.
        /// </summary>
        private void ActorType_NameChanged(object sender, EventArgs e)
        {
            ActorType actorType = sender as ActorType;
            Debug.Assert(actorType != null, "Invalid name changed event.");
            int index = actorTypeComboBox.Items.IndexOf(actorType);
            Debug.Assert(index != -1, "Actor type not used.");

            actorTypeComboBox.Items[index] = actorType;

            if (Actor != null)
            {
                actorTypeComboBox.SelectedItem = Actor.Type;
            }
        }

        /// <summary>
        /// Handles the <see cref="GameObject.NameChanged"/> event of the <see cref="Actors.Actor"/>.
        /// Updates the name of the actor at the textbox for editing actor name.
        /// </summary>
        private void Actor_NameChanged(object sender, EventArgs e)
        {
            if (Actor.Name != nameTextBox.Text)
            {
                nameTextBox.Text = Actor.Name;
            }
        }

        /// <summary>
        /// Handles the <see cref="Actors.Actor.ParentChanged"/> event of the <see cref="Actors.Actor"/>.
        /// Updates the possible physics types that can be used and updates the possible settings for physics.
        /// </summary>
        private void Actor_ParentChanged(object sender, ValueChangedEventArgs<Actor> e)
        {
            UpdatePhysicsTypeSelection();
            UpdatePhysicsSettingsVisible();
        }

        /// <summary>
        /// Handles the <see cref="Actors.Actor.DrawableAssetChanged"/> event of the <see cref="Actors.Actor"/>.
        /// Updates the possible physics types that can be used and updates the possible settings for physics.
        /// </summary>
        private void Actor_DrawableAssetChanged(object sender, ValueChangedEventArgs<DrawableAsset> e)
        {
            if (e.OldValue != null)
            {
                e.OldValue.NameChanged -= new EventHandler(DrawableAsset_NameChanged);
            }

            if (Actor.DrawableAsset != null)
            {
                Actor.DrawableAsset.NameChanged += new EventHandler(DrawableAsset_NameChanged);
            }

            SetDrawableAssetName();
        }

        /// <summary>
        /// Handles the <see cref="Asset.NameChanged"/> event of the <see cref="DrawableAsset"/> used at the actor.
        /// Updates the name of the actor drawable asset (graphics) to the textbox for editing actor drawable asset.
        /// </summary>
        private void DrawableAsset_NameChanged(object sender, EventArgs e)
        {
            SetDrawableAssetName();
        }

        /// <summary>
        /// Sets the name of the actor drawable asset (graphics) to the textbox for editing actor drawable asset.
        /// </summary>
        private void SetDrawableAssetName()
        {
            if (Actor.DrawableAsset != null) drawableAssetBox.Text = Actor.DrawableAsset.Name;
            else drawableAssetBox.Text = "None";
        }

        /// <summary>
        /// Handles the Click event of the makePrototypeButton control.
        /// Makes the actor a prototype.
        /// First checks if the actor contains any scene specific variable values, if yes then it shows an option to automatically clear this values. 
        /// </summary>
        private void makePrototypeButton_Click(object sender, EventArgs e)
        {
            // clone actor
            Actor clonedActor = (Actor)CloningHelper.Clone(Actor);

            // check if actor contains scene specific variable values (for actor and path)
            HashSet<ItemForDeletion> itemsToClear = new HashSet<ItemForDeletion>();
            List<Actor> allActors = new List<Actor>();
            AddAllActors(clonedActor, allActors);

            CheckActorBeforeMakingPrototype(clonedActor, allActors, itemsToClear);

            if (itemsToClear.Count == 0)
            {
                Project.Singleton.Prototypes.Add(clonedActor);
                Messages.ShowInfo("Prototype created.");
            }
            else
            {
                if (MessageBox.Show("Actor contains scene specific variable values. Do you want to clear these values to create prototype?", "Make Prototype", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    foreach (ItemForDeletion item in itemsToClear)
                    {
                        item.Delete();
                    }

                    Project.Singleton.Prototypes.Add(clonedActor);
                    Messages.ShowInfo("Prototype created.");
                }
            }
        }

        /// <summary>
        /// Checks if the actor contains any scene specific variable values (for actor and path).
        /// If yes creates the <see cref="ItemForDeletion"/> for these values.
        /// </summary>
        /// <param name="actor">The actor to check.</param>
        /// <param name="allActors">All actors that can be used at the actor.</param>
        /// <param name="itemsToClear">Stores <see cref="ItemForDeletion"/> to clear problematic values.</param>
        private void CheckActorBeforeMakingPrototype(Actor actor, List<Actor> allActors, HashSet<ItemForDeletion> itemsToClear)
        {
            // all variables
            foreach (NamedVariable variable in actor.Scripting.Variables)
            {
                // path or actor variable
                if ((variable.VariableType == VariableType.Path && variable.Value.GetValue() != null) ||
                    (variable.VariableType == VariableType.Actor && variable.Value.GetValue() != null && !allActors.Contains((Actor)variable.Value.GetValue())))
                {
                    itemsToClear.Add(new ConsistentDeletionHelper.ScriptNamedVariableForDeletion(variable) { Type = DeletionType.Clear });
                }
            }

            // all state machines
            foreach (StateMachine stateMachine in actor.Scripting.StateMachines)
            {
                // all states
                foreach (State state in stateMachine.States)
                {
                    foreach (BaseNode baseNode in state.Nodes)
                    {
                        // all script nodes
                        if (baseNode is Node)
                        {
                            Node node = baseNode as Node;

                            foreach (NodeSocket nodeSocket in node.Sockets)
                            {
                                // all variable node sockets
                                if (nodeSocket is VariableNodeSocket)
                                {
                                    VariableNodeSocket variableNodeSocket = nodeSocket as VariableNodeSocket;

                                    // path or actor variable
                                    if ((variableNodeSocket.VariableType == VariableType.Path && variableNodeSocket.Value.GetValue() != null) ||
                                        (variableNodeSocket.VariableType == VariableType.Actor && variableNodeSocket.Value.GetValue() != null && !allActors.Contains((Actor)variableNodeSocket.Value.GetValue())))
                                    {
                                        itemsToClear.Add(new ConsistentDeletionHelper.ScriptVariableNodeSocketForDeletion(variableNodeSocket) { Type = DeletionType.Clear });
                                    }
                                }
                            }
                        }
                        // all script variables
                        else if (baseNode is Variable)
                        {
                            Variable variable = baseNode as Variable;

                            if (variable.NamedVariable == null)
                            {
                                // path or actor variable
                                if ((variable.VariableType == VariableType.Path && variable.Value.GetValue() != null) ||
                                    (variable.VariableType == VariableType.Actor && variable.Value.GetValue() != null && !allActors.Contains((Actor)variable.Value.GetValue())))
                                {
                                    itemsToClear.Add(new ConsistentDeletionHelper.ScriptVariableForDeletion(variable) { Type = DeletionType.Clear });
                                }
                            }
                            else
                            {
                                // variable contains named variable from different actor
                                if (!allActors.Contains(variable.NamedVariable.ScriptingComponent.Actor))
                                {
                                    itemsToClear.Add(new ConsistentDeletionHelper.ScriptVariableOfNamedVariableForDeletion(variable) { Type = DeletionType.Clear });
                                }
                            }
                        }
                    }
                }
            }

            foreach (Actor child in actor.Children)
            {
                CheckActorBeforeMakingPrototype(child, allActors, itemsToClear);
            }
        }

        /// <summary>
        /// Adds the specified actor and its children to the <paramref name="actors"/> list.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        /// <param name="actors">The list to store all actors.</param>
        private void AddAllActors(Actor actor, List<Actor> actors)
        {
            actors.Add(actor);

            foreach (Actor child in actor.Children)
            {
                AddAllActors(child, actors);
            }
        }

        /// <summary>
        /// Handles the Click event of the openScriptingButton control.
        /// Opens visual scripting editor for the actor.
        /// </summary>
        private void openScriptingButton_Click(object sender, EventArgs e)
        {
            EditorApplication.Editor.OpenScriptingForm(Actor.Scripting);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the actorTypeComboBox control.
        /// Updates the actor type of the actor.
        /// </summary>
        private void actorTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actorTypeComboBox.SelectedItem != null)
            {
                Actor.Type = (ActorType)actorTypeComboBox.SelectedItem;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the physicsTypeComboBox control.
        /// Update the physics type of the actor and updates visibility of the physics settings.
        /// </summary>
        private void physicsTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (physicsTypeComboBox.SelectedItem != null)
            {
                Actor.Physics.Type = (PhysicsComponent.BodyPhysicsType)physicsTypeComboBox.SelectedItem;
                UpdateChildrenPhysicsType(Actor, Actor.Physics.Type);

                if (Actor.Physics.Type == PhysicsComponent.BodyPhysicsType.None)
                {
                    physicsSettingsTableLayoutPanel.Visible = false;
                }
                else
                {
                    physicsSettingsTableLayoutPanel.Visible = true;

                    UpdatePhysicsSettingsVisible();
                }
            }
        }

        /// <summary>
        /// Updates the physics type of the specified actor children according to physics type restriction.
        /// </summary>
        /// <param name="actor">The actor children to update.</param>
        /// <param name="type">The physics type of the parent.</param>
        private void UpdateChildrenPhysicsType(Actor actor, PhysicsComponent.BodyPhysicsType type)
        {
            foreach (Actor child in actor.Children)
            {
                if (child.Physics.Type != PhysicsComponent.BodyPhysicsType.None && child.Physics.Type != type)
                {
                    child.Physics.Type = type;
                }

                UpdateChildrenPhysicsType(child, type);
            }
        }

        /// <summary>
        /// Updates the visibility of physics settings that depends on the physics type of the actor.
        /// </summary>
        private void UpdatePhysicsSettingsVisible()
        {
            physicsSettingsTableLayoutPanel.SuspendLayout();

            bool isParent = Actor.Parent == null;

            linearDampingLabel.Visible = isParent;
            linearDampingFloatBox.Visible = isParent;
            angularDampingLabel.Visible = isParent;
            angularDampingFloatBox.Visible = isParent;

            if (Actor.Physics.Type == PhysicsComponent.BodyPhysicsType.Dynamic)
            {
                oneWayPlatformCheckBox.Visible = false;
                fixedRotationCheckBox.Visible = isParent;
                bulletCheckBox.Visible = true;
            }
            else
            {
                oneWayPlatformCheckBox.Visible = true;
                fixedRotationCheckBox.Visible = false;
                bulletCheckBox.Visible = false;
            }

            physicsSettingsTableLayoutPanel.ResumeLayout(true);
        }

        /// <summary>
        /// Updates the items in combobox that can be selected for the physics type of the actor.
        /// </summary>
        private void UpdatePhysicsTypeSelection()
        {
            if (Actor.Layer != null && Actor.Layer.IsParallax)
            {
                physicsTypeComboBox.Items.Clear();
                physicsTypeComboBox.Items.Add(PhysicsComponent.BodyPhysicsType.None);
                if (Actor.Physics.Type != PhysicsComponent.BodyPhysicsType.None) Actor.Physics.Type = PhysicsComponent.BodyPhysicsType.None;
            }
            else if (Actor.Parent == null)
            {
                if (physicsTypeComboBox.Items.Count != allPhysicsTypes.Length)
                {
                    physicsTypeComboBox.Items.Clear();
                    physicsTypeComboBox.Items.AddRange(allPhysicsTypes);
                }
            }
            else
            {
                physicsTypeComboBox.Items.Clear();
                physicsTypeComboBox.Items.Add(PhysicsComponent.BodyPhysicsType.None);
                if (Actor.Parent.Physics.Type != PhysicsComponent.BodyPhysicsType.None) physicsTypeComboBox.Items.Add(Actor.Parent.Physics.Type);
            }
        }

        /// <summary>
        /// Handles the ParentChanged event of the ActorInfo control.
        /// Unset the <see cref="Actor"/> property.
        /// </summary>
        private void ActorInfo_ParentChanged(object sender, EventArgs e)
        {
            if (Parent == null)
            {
                Actor = null;
            }
        }

        /// <summary>
        /// Handles the Leave event of the nameTextBox control.
        /// Updates the name of the actor.
        /// </summary>
        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (nameTextBox.Text != String.Empty)
            {
                Actor.Name = nameTextBox.Text;
            }
            else
            {
                nameTextBox.Text = Actor.Name;
            }
        }

        /// <summary>
        /// Handles the DragEnter event of the drawableAssetBox control.
        /// </summary>
        private void drawableAssetBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DrawableAsset))) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Handles the DragDrop event of the drawableAssetBox control.
        /// If <see cref="DrawableAsset"/> is dropped then updates the drawable asset of the actor.
        /// </summary>
        private void drawableAssetBox_DragDrop(object sender, DragEventArgs e)
        {
            DrawableAsset drawableAsset = (DrawableAsset)e.Data.GetData(typeof(DrawableAsset));

            if (drawableAsset != null && Actor.DrawableAsset != drawableAsset)
            {
                Actor.DrawableAsset = drawableAsset;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the drawableAssetVisibleCheckBox control.
        /// Updates the <see cref="Actors.Actor.DrawableAssetVisible"/> property of the actor.
        /// </summary>
        private void drawableAssetVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Actor.DrawableAssetVisible = drawableAssetVisibleCheckBox.Checked;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Project.Singleton.ActorTypes.ItemsChanged -= new EventHandler(ActorTypes_ItemsChanged);
                Actor = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
