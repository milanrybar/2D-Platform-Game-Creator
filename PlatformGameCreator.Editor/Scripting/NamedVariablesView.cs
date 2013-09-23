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
    /// View of <see cref="NamedVariable">named variables</see> of <see cref="ScriptingComponent">scripting</see>.
    /// Everything is done automatically by setting scripting (<see cref="Scripting.ScriptingComponent"/>) of named variable.
    /// </summary>
    partial class NamedVariablesView : UserControl
    {
        /// <summary>
        /// Called when the specified named variable want to be added to the scripting screen.
        /// </summary>
        /// <param name="variable">The variable to add.</param>
        public delegate void AddVariableToScene(NamedVariable variable);

        /// <summary>
        /// Occurs when a named variable want to be added to the scripting screen.
        /// </summary>
        public event AddVariableToScene VariableToScene;

        /// <summary>
        /// Gets or sets the <see cref="Scripting.ScriptingComponent"/> of named variables.
        /// </summary>
        public ScriptingComponent ScriptingComponent
        {
            get { return _scriptingComponent; }
            set
            {
                if (_scriptingComponent != null)
                {
                    settingsView.ClearRows();

                    _scriptingComponent.Variables.ListChanged -= new ObservableList<NamedVariable>.ListChangedEventHandler(Variables_ListChanged);
                }

                _scriptingComponent = value;

                if (_scriptingComponent != null)
                {
                    foreach (NamedVariable variable in _scriptingComponent.Variables)
                    {
                        ShowVariable(variable);
                    }

                    _scriptingComponent.Variables.ListChanged += new ObservableList<NamedVariable>.ListChangedEventHandler(Variables_ListChanged);
                }
            }
        }
        private ScriptingComponent _scriptingComponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedVariablesView"/> class.
        /// </summary>
        public NamedVariablesView()
        {
            InitializeComponent();

            settingsView.table.CellContentClick += new DataGridViewCellEventHandler(table_CellContentClick);
            settingsView.table.CellValidating += new DataGridViewCellValidatingEventHandler(table_CellValidating);

            settingsView.table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Name", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            settingsView.table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Value", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            settingsView.table.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Type", ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            settingsView.table.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            settingsView.table.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "", SortMode = DataGridViewColumnSortMode.NotSortable, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            // fill ComboBox
            foreach (VariableType variableType in Enum.GetValues(typeof(VariableType)))
            {
                variableTypeComboBox.Items.Add(new VariableTypeItem() { VariableType = variableType });
            }
            variableTypeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the ListChanged event of the Variables of <see cref="ScriptingComponent"/>.
        /// Updates view of named variables.
        /// </summary>
        private void Variables_ListChanged(object sender, ObservableListChangedEventArgs<NamedVariable> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ShowVariable(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    DataGridViewRow row = FindVariable(e.Item);
                    if (row != null) settingsView.table.Rows.Remove(row);
                    break;

                case ObservableListChangedType.Reset:
                    settingsView.ClearRows();
                    foreach (NamedVariable variable in ScriptingComponent.Variables)
                    {
                        ShowVariable(variable);
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds the named variable in the DataGridView.
        /// </summary>
        /// <param name="namedVariable">The named variable to find.</param>
        /// <returns>Item of DataGridView if found; otherwise null.</returns>
        private DataGridViewRow FindVariable(NamedVariable namedVariable)
        {
            foreach (DataGridViewRow row in settingsView.table.Rows)
            {
                if (row.Tag == namedVariable)
                {
                    return row;
                }
            }

            return null;
        }

        /// <summary>
        /// Handles the CellValidating event of the table control.
        /// Checks if the specified named variable has valid name.
        /// </summary>
        private void table_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // check correctness of the variable name
            if (e.ColumnIndex == 0)
            {
                NamedVariable variable = settingsView.table.Rows[e.RowIndex].Tag as NamedVariable;

                Debug.Assert(variable != null, "Cell does not contain variable data.");
                Debug.Assert(e.FormattedValue != null, "Cell value should not be null.");

                if (variable != null)
                {
                    if (!IsNameValid(e.FormattedValue.ToString(), variable, true))
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
        /// When clicked on the remove button then the named variable is removed.
        /// When clicked on the button for adding to the scene the named variable is added to the scene.
        /// </summary>
        private void table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                NamedVariable variable = settingsView.table.Rows[e.RowIndex].Tag as NamedVariable;

                if (variable != null)
                {
                    if (ScriptingComponent == null) throw new Exception("Scripting Component is not set.");

                    if (new ConsistentDeletionForm(new ConsistentDeletionHelper.ScriptNamedVariableForDeletion(variable)).ShowDialog() == DialogResult.OK)
                    {
                        Messages.ShowInfo("Variable deleted.");
                    }
                }
            }
            else if (e.ColumnIndex == 4)
            {
                NamedVariable variable = settingsView.table.Rows[e.RowIndex].Tag as NamedVariable;

                if (variable != null)
                {
                    if (VariableToScene != null) VariableToScene(variable);
                }
            }
        }

        /// <summary>
        /// Represents a type of a script variable.
        /// </summary>
        private class VariableTypeItem
        {
            /// <summary>
            /// Gets or sets the type of the variable.
            /// </summary>
            public VariableType VariableType { get; set; }

            /// <inheritdoc />
            /// <summary>
            /// Returns a friendly name of the variable type.
            /// </summary>
            public override string ToString()
            {
                return VariableTypeHelper.FriendlyName(VariableType);
            }
        }

        /// <summary>
        /// Handles the Click event of the variableAddButton control.
        /// Creates and adds the new named variable, if the name of named variable is valid.
        /// </summary>
        private void variableAddButton_Click(object sender, EventArgs e)
        {
            VariableTypeItem selected = variableTypeComboBox.SelectedItem as VariableTypeItem;

            if (selected != null)
            {
                Debug.Assert(ScriptingComponent != null, "Scripting Component is not set.");

                if (!IsNameValid(variableNameTextBox.Text, null, true)) return;

                // create new named variable
                NamedVariable variable = new NamedVariable(ScriptingComponent, selected.VariableType);

                // set name
                variable.Name = variableNameTextBox.Text;
                variableNameTextBox.Text = String.Empty;

                ScriptingComponent.Variables.Add(variable);

                Messages.ShowInfo("Variable added.");
            }
        }

        /// <summary>
        /// Shows the specified named variable in the DataGridView.
        /// </summary>
        /// <param name="variable">The named variable to show.</param>
        private void ShowVariable(NamedVariable variable)
        {
            Debug.Assert(variable != null, "Variable cannot be null.");

            StringVar nameVar = new StringVar() { Value = variable.Name };
            nameVar.ValueChanged += (object sender, EventArgs e) => { if (IsNameValid(nameVar.Value, variable)) variable.Name = nameVar.Value; };

            DataGridViewRow row = new DataGridViewRow() { Tag = variable };

            row.Cells.Add(nameVar.GetGridCell());
            row.Cells.Add(variable.Value.GetGridCell());
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = VariableTypeHelper.FriendlyName(variable.Value.VariableType) });
            row.Cells.Add(new DataGridViewButtonCell() { Value = "X" });
            row.Cells.Add(new DataGridViewButtonCell() { Value = ">" });

            settingsView.table.Rows.Add(row);
        }

        /// <summary>
        /// Handles the KeyPress event of the variableNameTextBox control.
        /// Enter - Creates and adds the new named variable, if the name of named variable is valid.
        /// </summary>
        private void variableNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                variableAddButton_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Determines whether the specified name is the unique name in the list of named variables.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="currentVariable">The current named variable.</param>
        /// <returns><c>true</c> if the specified name is the unique name in the list of named variables; otherwise <c>false</c>.</returns>
        private bool UniqueName(string name, NamedVariable currentVariable)
        {
            Debug.Assert(ScriptingComponent != null, "Scripting Component is not set.");

            foreach (NamedVariable variable in ScriptingComponent.Variables)
            {
                if (variable.Name == name && variable != currentVariable)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified name for the specified named variable is valid.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="currentVariable">The current named variable.</param>
        /// <param name="showMessage">If set to <c>true</c> message is shown to the user by <see cref="Messages"/> system, if the name is not valid.</param>
        /// <returns><c>true</c> if the specified name for the specified named variable is valid; otherwise <c>false</c>.</returns>
        private bool IsNameValid(string name, NamedVariable currentVariable, bool showMessage = false)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                if (showMessage) Messages.ShowWarning("No variable name set.");
                return false;
            }
            else if (!UniqueName(name, currentVariable))
            {
                if (showMessage) Messages.ShowWarning("Variable with the same name already exists.");
                return false;
            }

            return true;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ScriptingComponent = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
