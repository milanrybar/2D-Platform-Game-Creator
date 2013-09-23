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
    /// View of list of <see cref="Event">events</see>.
    /// Everything is done automatically by setting list (<see cref="EventsView.Events"/>) of events.
    /// </summary>
    partial class EventsView : UserControl
    {
        /// <summary>
        /// Gets or sets the list of events.
        /// </summary>
        public new ObservableList<Event> Events
        {
            get { return _events; }
            set
            {
                if (_events != null)
                {
                    settingsView.ClearRows();

                    _events.ListChanged -= new ObservableList<Event>.ListChangedEventHandler(Events_ListChanged);
                }

                _events = value;

                if (_events != null)
                {
                    _events.ListChanged += new ObservableList<Event>.ListChangedEventHandler(Events_ListChanged);

                    foreach (Event scriptEvent in Events)
                    {
                        ShowEvent(scriptEvent);
                    }
                }
            }
        }
        private ObservableList<Event> _events;

        /// <summary>
        /// Called when the specified event is removing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="scriptEvent">The script event that is removing.</param>
        public delegate void EventRemovingHandler(object sender, Event scriptEvent);

        /// <summary>
        /// Occurs when an event is removing.
        /// </summary>
        public event EventRemovingHandler OnEventRemoving;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsView"/> class.
        /// </summary>
        public EventsView()
        {
            InitializeComponent();

            settingsView.table.CellContentClick += new DataGridViewCellEventHandler(table_CellContentClick);
            settingsView.table.CellValidating += new DataGridViewCellValidatingEventHandler(table_CellValidating);

            settingsView.table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Event Name", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            settingsView.table.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        /// <summary>
        /// Handles the ListChanged event of the <see cref="Events"/>.
        /// Updates the view of events.
        /// </summary>
        private void Events_ListChanged(object sender, ObservableListChangedEventArgs<Event> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowEvent(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    DataGridViewRow deletedRow = FindEvent(e.Item);
                    if (deletedRow != null) settingsView.table.Rows.Remove(deletedRow);
                    break;

                case ObservableListChangedType.Reset:
                    settingsView.ClearRows();
                    foreach (Event scriptEvent in Events)
                    {
                        ShowEvent(scriptEvent);
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds the event in the DataGridView.
        /// </summary>
        /// <param name="scriptEvent">The event to find.</param>
        /// <returns>Item of DataGridView if found; otherwise null.</returns>
        private DataGridViewRow FindEvent(Event scriptEvent)
        {
            foreach (DataGridViewRow row in settingsView.table.Rows)
            {
                if (row.Tag == scriptEvent)
                {
                    return row;
                }
            }

            return null;
        }

        /// <summary>
        /// Handles the CellValidating event of the table control.
        /// Checks if the specified event has valid name.
        /// </summary>
        private void table_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // check correctness of the event name
            if (e.ColumnIndex == 0)
            {
                Event scriptEvent = settingsView.table.Rows[e.RowIndex].Tag as Event;

                Debug.Assert(scriptEvent != null, "Cell does not contain event data.");
                Debug.Assert(e.FormattedValue != null, "Cell value should not be null.");

                if (scriptEvent != null)
                {
                    if (!IsNameValid(e.FormattedValue.ToString(), scriptEvent, true))
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        Messages.ShowInfo("Ready");
                    }
                }
            }
        }

        /// <summary>
        /// Handles the CellContentClick event of the table control.
        /// When clicked on the remove button then the event is removed.
        /// </summary>
        private void table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Event scriptEvent = settingsView.table.Rows[e.RowIndex].Tag as Event;

                if (scriptEvent != null)
                {
                    Debug.Assert(Events != null, "Events are not set.");

                    if (OnEventRemoving != null) OnEventRemoving(this, scriptEvent);

                    Events.Remove(scriptEvent);

                    Messages.ShowInfo("Event deleted.");
                }
            }
        }

        /// <summary>
        /// Shows the specified event in the DataGridView.
        /// </summary>
        /// <param name="scriptEvent">The event to show.</param>
        private void ShowEvent(Event scriptEvent)
        {
            Debug.Assert(scriptEvent != null, "Event cannot be null.");

            StringVar nameVar = new StringVar() { Value = scriptEvent.Name };
            nameVar.ValueChanged += (object sender, EventArgs e) => { if (IsNameValid(nameVar.Value, scriptEvent)) scriptEvent.Name = nameVar.Value; };

            DataGridViewRow row = new DataGridViewRow() { Tag = scriptEvent };

            row.Cells.Add(nameVar.GetGridCell());
            row.Cells.Add(new DataGridViewButtonCell() { Value = "X" });

            settingsView.table.Rows.Add(row);
        }

        /// <summary>
        /// Handles the Click event of the addEventButton control.
        /// Creates and adds the new event to the <see cref="Events"/>, if the name of event is valid.
        /// </summary>
        private void addEventButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(Events != null, "Events are not set.");

            if (!IsNameValid(eventNameTextBox.Text, null, true)) return;

            // create new event
            Event scriptEvent = new Event();

            // set name
            scriptEvent.Name = eventNameTextBox.Text;
            eventNameTextBox.Text = String.Empty;

            Events.Add(scriptEvent);

            Messages.ShowInfo("Event added.");
        }

        /// <summary>
        /// Handles the KeyPress event of the eventNameTextBox control.
        /// Enter - Creates and adds the new event to the <see cref="Events"/>, if the name of event is valid.
        /// </summary>
        private void eventNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                addEventButton_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Determines whether the specified name for the specified event is valid.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="currentEvent">The current event.</param>
        /// <param name="showMessage">If set to <c>true</c> message is shown to the user by <see cref="Messages"/> system, if the name is not valid.</param>
        /// <returns><c>true</c> if the specified name for the specified event is valid; otherwise <c>false</c>.</returns>
        private bool IsNameValid(string name, Event currentEvent, bool showMessage = false)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                if (showMessage) Messages.ShowWarning("No event name set.");
                return false;
            }
            else if (!UniqueName(name, currentEvent))
            {
                if (showMessage) Messages.ShowWarning("Event with the same name already exists.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified name is the unique name in the list of events.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="currentEvent">The current event.</param>
        /// <returns><c>true</c> if the specified name is the unique name in the list of events; otherwise <c>false</c>.</returns>
        private bool UniqueName(string name, Event currentEvent)
        {
            Debug.Assert(Events != null, "Events are not set.");

            foreach (Event scriptEvent in Events)
            {
                if (scriptEvent.Name == name && scriptEvent != currentEvent)
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
            if (disposing && Events != null)
            {
                Events = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
