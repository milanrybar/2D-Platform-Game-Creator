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
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Control for editing <see cref="Scripting.State"/> settings.
    /// Everything is done automatically by setting <see cref="Scripting.State"/> property.
    /// </summary>
    partial class StateSettingsView : UserControl
    {
        /// <summary>
        /// Gets or sets the state to edit.
        /// </summary>
        public State State
        {
            get { return _state; }
            set
            {
                if (_state != null)
                {
                    settingsView.ClearRows();

                    _state.Transitions.ListChanged -= new ObservableList<Transition>.ListChangedEventHandler(Transitions_ListChanged);
                }

                _state = value;

                if (_state != null)
                {
                    _state.Transitions.ListChanged += new ObservableList<Transition>.ListChangedEventHandler(Transitions_ListChanged);

                    stateNameTextBox.Text = _state.Name;
                    stateNameTextBox.Enabled = true;

                    foreach (Transition transition in _state.Transitions)
                    {
                        ShowTransition(transition);
                    }
                }
                else
                {
                    stateNameTextBox.Enabled = false;
                    stateNameTextBox.Text = String.Empty;
                }
            }
        }
        private State _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateSettingsView"/> class.
        /// </summary>
        public StateSettingsView()
        {
            InitializeComponent();

            settingsView.table.CellContentClick += new DataGridViewCellEventHandler(table_CellContentClick);

            settingsView.table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Event In", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            settingsView.table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "State", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            settingsView.table.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        /// <summary>
        /// Handles the ListChanged event of the Transitions os the <see cref="State"/>.
        /// Updates the view of transitions.
        /// </summary>
        private void Transitions_ListChanged(object sender, ObservableListChangedEventArgs<Transition> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowTransition(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    DataGridViewRow deletedRow = FindTransition(e.Item);
                    if (deletedRow != null) settingsView.table.Rows.Remove(deletedRow);
                    break;

                case ObservableListChangedType.Reset:
                    settingsView.ClearRows();
                    foreach (Transition transition in State.Transitions)
                    {
                        ShowTransition(transition);
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds the transition in the DataGridView.
        /// </summary>
        /// <param name="transition">The transition to find.</param>
        /// <returns>Item of DataGridView if found; otherwise null.</returns>
        private DataGridViewRow FindTransition(Transition transition)
        {
            foreach (DataGridViewRow row in settingsView.table.Rows)
            {
                if (row.Tag == transition)
                {
                    return row;
                }
            }

            return null;
        }

        /// <summary>
        /// Shows the specified transition in the DataGridView.
        /// </summary>
        /// <param name="transition">The transition to show.</param>
        private void ShowTransition(Transition transition)
        {
            Debug.Assert(transition != null, "Transition cannot be null.");

            DataGridViewRow row = new DataGridViewRow() { Tag = transition };

            row.Cells.Add(new DataGridViewStateTransitionEventCell(transition));
            row.Cells.Add(new DataGridViewStateTransitionStateCell(transition));
            row.Cells.Add(new DataGridViewButtonCell() { Value = "X" });

            settingsView.table.Rows.Add(row);
        }

        /// <summary>
        /// Handles the CellContentClick event of the table control.
        /// When clicked on the remove button then the transition is removed.
        /// </summary>
        private void table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                Transition transition = settingsView.table.Rows[e.RowIndex].Tag as Transition;

                if (transition != null)
                {
                    Debug.Assert(State != null, "State is not set.");

                    State.Transitions.Remove(transition);

                    Messages.ShowInfo("Transition deleted.");
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the stateNameTextBox control.
        /// Checks if the name of the state is valid.
        /// </summary>
        private void stateNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!stateNameTextBox.Enabled) return;
            Debug.Assert(State != null, "State not set.");

            if (State.Name != stateNameTextBox.Text)
            {
                if (String.IsNullOrWhiteSpace(stateNameTextBox.Text))
                {
                    Messages.ShowWarning("No state name set.");
                    return;
                }
                else if (!UniqueStateName(stateNameTextBox.Text))
                {
                    Messages.ShowWarning("State with the same name already exists.");
                    return;
                }

                State.Name = stateNameTextBox.Text;
                Messages.ShowInfo("Ready");
            }
        }

        /// <summary>
        /// Determines whether the specified name is the unique name of the state in the state machine.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns><c>true</c> if the specified name is the unique name of the state in the state machine.; otherwise <c>false</c>.</returns>
        private bool UniqueStateName(string name)
        {
            Debug.Assert(State != null, "State not set.");

            foreach (State state in State.StateMachine.States)
            {
                if (state.Name == name && state != State)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && State != null)
            {
                State = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// <see cref="DataGridViewComboBoxCell"/> for choosing the <see cref="Event"/> of the <see cref="Transition"/>.
    /// </summary>
    class DataGridViewStateTransitionEventCell : DataGridViewComboBoxCell, ICellValueChanged
    {
        private Transition transition;
        private ObservableList<Event> itemsToChooseFrom;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewStateTransitionEventCell"/> class.
        /// </summary>
        /// <param name="transition">The transition to use.</param>
        public DataGridViewStateTransitionEventCell(Transition transition)
        {
            DisplayMember = "Name";
            ValueMember = "Index";

            this.transition = transition;
            itemsToChooseFrom = transition.StateFrom.StateMachine.ScriptingComponent.EventsIn;

            transition.EventChanged += new EventHandler(transition_EventChanged);
            itemsToChooseFrom.ListChanged += new ObservableList<Event>.ListChangedEventHandler(itemsToChooseFrom_ListChanged);

            if (transition.Event != null) Value = transition.Event.Index;

            foreach (Event eventItem in itemsToChooseFrom)
            {
                Items.Add(eventItem);
            }
        }

        /// <summary>
        /// Handles the EventChanged event of the Transition.
        /// Updates the event of the transition.
        /// </summary>
        private void transition_EventChanged(object sender, EventArgs e)
        {
            if (transition.Event != null) Value = transition.Event.Index;
            else Value = null;
        }

        /// <summary>
        /// Handles the ListChanged event of the itemsToChooseFrom.
        /// Updates the items to choose from.
        /// </summary>
        private void itemsToChooseFrom_ListChanged(object sender, ObservableListChangedEventArgs<Event> e)
        {
            if (DataGridView == null) return;

            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    Items.Add(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    Items.Remove(e.Item);
                    transition_EventChanged(null, null);
                    break;

                case ObservableListChangedType.Reset:
                    Items.Clear();
                    foreach (Event eventItem in itemsToChooseFrom)
                    {
                        Items.Add(eventItem);
                    }
                    transition_EventChanged(null, null);
                    break;
            }
        }

        /// <inheritdoc />
        public void CellValueChanged()
        {
            if (Value != null) transition.Event = itemsToChooseFrom[(int)Value];
            else transition.Event = null;
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new DataGridViewStateTransitionEventCell(transition);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                transition.EventChanged -= new EventHandler(transition_EventChanged);
                itemsToChooseFrom.ListChanged -= new ObservableList<Event>.ListChangedEventHandler(itemsToChooseFrom_ListChanged);

                transition = null;
                itemsToChooseFrom = null;
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// <see cref="DataGridViewComboBoxCell"/> for choosing the <see cref="State"/> of the <see cref="Transition"/>.
    /// </summary>
    class DataGridViewStateTransitionStateCell : DataGridViewComboBoxCell, ICellValueChanged
    {
        private Transition transition;
        private ObservableList<State> itemsToChooseFrom;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewStateTransitionStateCell"/> class.
        /// </summary>
        /// <param name="transition">The transition to use.</param>
        public DataGridViewStateTransitionStateCell(Transition transition)
        {
            DisplayMember = "Name";
            ValueMember = "Index";

            this.transition = transition;
            itemsToChooseFrom = transition.StateFrom.StateMachine.States;

            transition.StateToChanged += new EventHandler(transition_StateToChanged);
            itemsToChooseFrom.ListChanged += new ObservableList<Scripting.State>.ListChangedEventHandler(itemsToChooseFrom_ListChanged);

            if (transition.StateTo != null) Value = transition.StateTo.Index;

            foreach (State stateItem in itemsToChooseFrom)
            {
                if (transition.StateFrom != stateItem)
                {
                    Items.Add(stateItem);
                }
            }
        }

        /// <summary>
        /// Handles the StateToChanged event of the Transition.
        /// Updates the state to.
        /// </summary>
        private void transition_StateToChanged(object sender, EventArgs e)
        {
            if (transition.StateTo != null) Value = transition.StateTo.Index;
            else Value = null;
        }

        /// <summary>
        /// Handles the ListChanged event of the itemsToChooseFrom.
        /// Updates the items to choose from.
        /// </summary>
        private void itemsToChooseFrom_ListChanged(object sender, ObservableListChangedEventArgs<State> e)
        {
            if (DataGridView == null) return;

            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    Items.Add(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    Items.Remove(e.Item);
                    transition_StateToChanged(null, null);
                    break;

                case ObservableListChangedType.Reset:
                    Items.Clear();
                    foreach (State stateItem in itemsToChooseFrom)
                    {
                        if (transition.StateFrom != stateItem)
                        {
                            Items.Add(stateItem);
                        }
                    }
                    transition_StateToChanged(null, null);
                    break;
            }
        }

        /// <inheritdoc />
        public void CellValueChanged()
        {
            if (Value != null) transition.StateTo = itemsToChooseFrom[(int)Value];
            else transition.StateTo = null;
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new DataGridViewStateTransitionStateCell(transition);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                transition.StateToChanged -= new EventHandler(transition_StateToChanged);
                itemsToChooseFrom.ListChanged -= new ObservableList<Scripting.State>.ListChangedEventHandler(itemsToChooseFrom_ListChanged);

                transition = null;
                itemsToChooseFrom = null;
            }
            base.Dispose(disposing);
        }
    }
}
