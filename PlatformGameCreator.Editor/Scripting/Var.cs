/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Assets.Sounds;
using PlatformGameCreator.Editor.GameObjects.Paths;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.Common;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Key = Microsoft.Xna.Framework.Input.Keys;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a wrapper for a variable.
    /// </summary>
    interface IVariable
    {
        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        VariableType VariableType { get; }

        /// <summary>
        /// Occurs when the value of the variable changes.
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        /// <returns>Value of the variable.</returns>
        object GetValue();

        /// <summary>
        /// Sets the value of the variable.
        /// </summary>
        /// <param name="value">The value to set.</param>
        void SetValue(object value);

        /// <summary>
        /// Gets the <see cref="DataGridViewCell"/> representing this variable.
        /// </summary>
        /// <remarks>
        /// Used for editing the value of the variable at GUI.
        /// </remarks>
        /// <returns><see cref="DataGridViewCell"/> representing this variable</returns>
        DataGridViewCell GetGridCell();

        /// <summary>
        /// Clones this variable.
        /// </summary>
        /// <returns>Cloned variable.</returns>
        IVariable Clone();
    }

    /// <summary>
    /// Represents a wrapper for a variable of the specified type.
    /// </summary>
    /// <typeparam name="VarType">The type of the variable.</typeparam>
    [Serializable]
    abstract class Var<VarType> : ISerializable, IVariable
    {
        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        public virtual VarType Value
        {
            get { return _value; }
            set
            {
                _value = value;
                InvokeValueChanged();
            }
        }
        /// <summary>
        /// Value of the variable.
        /// </summary>
        protected VarType _value;

        /// <inheritdoc />
        public abstract VariableType VariableType { get; }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property value changes.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <inheritdoc />
        public abstract IVariable Clone();

        /// <inheritdoc />
        public abstract DataGridViewCell GetGridCell();

        /// <inheritdoc />
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        /// <inheritdoc />
        public object GetValue()
        {
            return Value;
        }

        /// <inheritdoc />
        public virtual void SetValue(object value)
        {
            Value = (VarType)value;
        }

        /// <summary>
        /// Invokes the <see cref="ValueChanged"/> event.
        /// </summary>
        protected void InvokeValueChanged()
        {
            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the value of the variable as a human-readable string.
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// <see cref="DataGridViewComboBoxCell"/> for the variable of the specified type.
        /// </summary>
        /// <typeparam name="VariableType">The type of the variable.</typeparam>
        protected abstract class DataGridViewVarComboBoxCell<VariableType> : DataGridViewComboBoxCell, ICellValueChanged
        {
            /// <summary>
            /// Gets the underlying variable.
            /// </summary>
            public Var<VariableType> Variable
            {
                get { return _variable; }
            }
            private Var<VariableType> _variable;

            /// <summary>
            /// Initializes a new instance of the <see cref="Var&lt;VarType&gt;.DataGridViewVarComboBoxCell&lt;VariableType&gt;"/> class.
            /// </summary>
            /// <param name="variable">The variable to use.</param>
            public DataGridViewVarComboBoxCell(Var<VariableType> variable)
            {
                _variable = variable;
                _variable.ValueChanged += new EventHandler(Variable_ValueChanged);
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                if (disposing && Variable != null)
                {
                    Variable.ValueChanged -= new EventHandler(Variable_ValueChanged);
                }
                base.Dispose(disposing);
            }

            public abstract void CellValueChanged();

            /// <summary>
            /// Handles the ValueChanged event of the Variable.
            /// Updates the value in this <see cref="DataGridViewCell"/>.
            /// </summary>
            protected abstract void Variable_ValueChanged(object sender, EventArgs e);
        }

        /// <summary>
        /// <see cref="DataGridViewComboBoxCell"/> for the variable of the specified type.
        /// Items for <see cref="DataGridViewComboBoxCell"/> are stored at the <see cref="ObservableList{VariableType}"/>.
        /// </summary>
        /// <typeparam name="VariableType">The type of the variable.</typeparam>
        protected abstract class DataGridViewVarComboBoxCellOnObservableList<VariableType> : DataGridViewVarComboBoxCell<VariableType>
        {
            /// <summary>
            /// Items for <see cref="DataGridViewComboBoxCell"/>.
            /// </summary>
            protected ObservableList<VariableType> items;

            /// <summary>
            /// Initializes a new instance of the <see cref="Var&lt;VarType&gt;.DataGridViewVarComboBoxCellOnObservableList&lt;VariableType&gt;"/> class.
            /// </summary>
            /// <param name="variable">The variable to use.</param>
            /// <param name="items">The items to choose from.</param>
            public DataGridViewVarComboBoxCellOnObservableList(Var<VariableType> variable, ObservableList<VariableType> items)
                : base(variable)
            {
                this.items = items;
                this.items.ListChanged += new ObservableList<VariableType>.ListChangedEventHandler(items_ListChanged);

                foreach (VariableType item in items)
                {
                    Items.Add(item);
                }
            }

            /// <summary>
            /// Handles the ListChanged event of the items.
            /// Updates the items in the <see cref="DataGridViewComboBoxCell"/>.
            /// </summary>
            private void items_ListChanged(object sender, ObservableListChangedEventArgs<VariableType> e)
            {
                switch (e.ListChangedType)
                {
                    case ObservableListChangedType.ItemAdded:
                        Items.Add(e.Item);
                        break;

                    case ObservableListChangedType.ItemDeleted:
                        Items.Remove(e.Item);
                        break;

                    case ObservableListChangedType.ItemChanged:
                        Debug.Assert(true, "Not supported operation.");
                        break;

                    case ObservableListChangedType.Reset:
                        Items.Clear();
                        foreach (VariableType item in items)
                        {
                            Items.Add(item);
                        }
                        break;
                }
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    items.ListChanged -= new ObservableList<VariableType>.ListChangedEventHandler(items_ListChanged);
                    items = null;
                }
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// <see cref="DataGridViewTextBoxCell"/> for the variable of the specified type.
        /// </summary>
        /// <typeparam name="VariableType">The type of the variable.</typeparam>
        protected abstract class DataGridViewVarTextBoxCell<VariableType> : DataGridViewTextBoxCell, ICellValueChanged
        {
            /// <summary>
            /// Gets the underlying variable.
            /// </summary>
            public Var<VariableType> Variable
            {
                get { return _variable; }
            }
            private Var<VariableType> _variable;

            /// <summary>
            /// Initializes a new instance of the <see cref="Var&lt;VarType&gt;.DataGridViewVarTextBoxCell&lt;VariableType&gt;"/> class.
            /// </summary>
            /// <param name="variable">The variable to use.</param>
            public DataGridViewVarTextBoxCell(Var<VariableType> variable)
            {
                _variable = variable;
                _variable.ValueChanged += new EventHandler(Variable_ValueChanged);
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                if (disposing && Variable != null)
                {
                    Variable.ValueChanged -= new EventHandler(Variable_ValueChanged);
                }
                base.Dispose(disposing);
            }

            /// <inheritdoc />
            public abstract void CellValueChanged();

            /// <summary>
            /// Handles the ValueChanged event of the Variable.
            /// Updates the value in this <see cref="DataGridViewCell"/>.
            /// </summary>
            protected abstract void Variable_ValueChanged(object sender, EventArgs e);
        }
    }

    /// <summary>
    /// Variable wrapper for the specified system variable type.
    /// </summary>
    /// <typeparam name="VarType">The type of the variable.</typeparam>
    [Serializable]
    abstract class SystemVar<VarType> : Var<VarType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemVar&lt;VarType&gt;"/> class.
        /// </summary>
        public SystemVar()
        {
            _value = default(VarType);
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected SystemVar(SerializationInfo info, StreamingContext ctxt)
        {
            _value = (VarType)info.GetValue("Value", typeof(VarType));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value);
        }
    }

    /// <summary>
    /// Variable wrapper for the specified variable type that implements <see cref="IName"/> interface.
    /// </summary>
    /// <typeparam name="VarType">The type of the variable that implements <see cref="IName"/> interface.</typeparam>
    [Serializable]
    abstract class VarOnIName<VarType> : Var<VarType>, IDeserializationCallback where VarType : IName
    {
        /// <inheritdoc />
        public override VarType Value
        {
            set
            {
                if (_value != null)
                {
                    _value.NameChanged -= new EventHandler(Value_NameChanged);
                }

                _value = value;

                if (_value != null)
                {
                    _value.NameChanged += new EventHandler(Value_NameChanged);
                }

                InvokeValueChanged();
            }
        }

        /// <summary>
        /// Handles the NameChanged event of the Value.
        /// Invokes ValueChanged event.
        /// </summary>
        private void Value_NameChanged(object sender, EventArgs e)
        {
            InvokeValueChanged();
        }

        /// <inheritdoc />
        public virtual void OnDeserialization(object sender)
        {
            // to set NameChanged event
            Value = Value;
        }
    }

    /// <summary>
    /// Represents a wrapper for a bool variable.
    /// </summary>
    [Serializable]
    class BoolVar : SystemVar<bool>
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Bool; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolVar"/> class.
        /// </summary>
        public BoolVar()
        {
        }

        /// <inheritdoc />
        private BoolVar(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewBoolVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new BoolVar() { Value = Value };
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a bool variable.
        /// </summary>
        private class DataGridViewBoolVarCell : DataGridViewVarComboBoxCell<bool>
        {
            public DataGridViewBoolVarCell(BoolVar value)
                : base(value)
            {
                Value = value.Value ? "True" : "False";

                Items.Add("True");
                Items.Add("False");
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value ? "True" : "False";
            }

            public override void CellValueChanged()
            {
                Variable.Value = Value.ToString() == "True" ? true : false;
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for an int variable.
    /// </summary>
    [Serializable]
    class IntVar : SystemVar<int>
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Int; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntVar"/> class.
        /// </summary>
        public IntVar()
        {
        }

        /// <inheritdoc />
        private IntVar(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewIntVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new IntVar() { Value = Value };
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing an int variable.
        /// </summary>
        private class DataGridViewIntVarCell : DataGridViewVarTextBoxCell<int>
        {
            public DataGridViewIntVarCell(IntVar value)
                : base(value)
            {
                Value = value.Value.ToString();
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value.ToString();
            }

            public override void CellValueChanged()
            {
                int newValue;
                if (Value != null && int.TryParse(Value.ToString(), out newValue))
                {
                    Variable.Value = newValue;
                }
                else
                {
                    Variable.Value = 0;
                }
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a float variable.
    /// </summary>
    [Serializable]
    class FloatVar : SystemVar<float>
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Float; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatVar"/> class.
        /// </summary>
        public FloatVar()
        {
        }

        /// <inheritdoc />
        private FloatVar(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewFloatVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new FloatVar() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a float variable.
        /// </summary>
        private class DataGridViewFloatVarCell : DataGridViewVarTextBoxCell<float>
        {
            public DataGridViewFloatVarCell(FloatVar value)
                : base(value)
            {
                Value = value.ToString();
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.ToString();
            }

            public override void CellValueChanged()
            {
                float value = 0f;
                if (Value != null)
                {
                    if (!float.TryParse(Value.ToString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value))
                    {
                        float.TryParse(Value.ToString(), out value);
                    }
                }

                if (float.IsNaN(value) || float.IsInfinity(value)) value = 0f;

                Variable.Value = value;
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a string variable.
    /// </summary>
    [Serializable]
    class StringVar : Var<string>
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.String; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringVar"/> class.
        /// </summary>
        public StringVar()
        {
            _value = String.Empty;
        }

        /// <inheritdoc />
        private StringVar(SerializationInfo info, StreamingContext ctxt)
        {
            _value = info.GetString("Value");
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value);
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewStringVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new StringVar() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Value)) return " ";
            else return Value;
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a string variable.
        /// </summary>
        private class DataGridViewStringVarCell : DataGridViewVarTextBoxCell<string>
        {
            public DataGridViewStringVarCell(StringVar value)
                : base(value)
            {
                Value = value.Value;
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value;
            }

            public override void CellValueChanged()
            {
                if (Value == null) Variable.Value = String.Empty;
                else
                {
                    string str = Value.ToString();
                    for (int i = 0; i < str.Length; ++i)
                    {
                        if (str[i] < 32 || str[i] > 126)
                        {
                            str = str.Remove(i, 1);
                            --i;
                        }
                    }
                    Variable.Value = str;
                }
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a actor type variable.
    /// </summary>
    [Serializable]
    class ActorTypeVar : VarOnIName<ActorType>, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override ActorType Value
        {
            set
            {
                if (value == null) value = Project.Singleton.ActorTypes.Root;

                base.Value = value;
            }
        }

        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.ActorType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTypeVar"/> class.
        /// </summary>
        public ActorTypeVar()
        {
            _value = Project.Singleton.ActorTypes.Root;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private ActorTypeVar(SerializationInfo info, StreamingContext ctxt)
        {
            _valueNameForDeserialization = info.GetString("ActorTypeName");
        }

        private string _valueNameForDeserialization;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueNameForDeserialization != null)
            {
                _value = Project.Singleton.ActorTypes.Find(_valueNameForDeserialization);
                Debug.Assert(_value != null, "ActorType not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ActorTypeName", Value.Name);
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewActorTypeVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new ActorTypeVar() { Value = Value };
        }

        /// <inheritdoc />
        public override void SetValue(object value)
        {
            if (value == null) Value = Project.Singleton.ActorTypes.Root;
            else base.SetValue(value);
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a actor type variable.
        /// </summary>
        private class DataGridViewActorTypeVarCell : DataGridViewVarComboBoxCell<ActorType>
        {
            private ActorTypesManager actorTypes;

            public DataGridViewActorTypeVarCell(ActorTypeVar value)
                : base(value)
            {
                this.DisplayMember = "NameWithLevelInfo";
                this.ValueMember = "Value";

                Value = value.Value.Value;

                actorTypes = Project.Singleton.ActorTypes;
                actorTypes.ItemsChanged += new EventHandler(actorTypes_ItemsChanged);

                foreach (ActorType actorType in actorTypes.Items)
                {
                    Items.Add(actorType);
                }
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value.Value;
            }

            public override void CellValueChanged()
            {
                Variable.Value = Project.Singleton.ActorTypes.Find((uint)Value);
            }

            /// <summary>
            /// Handles the ItemsChanged event of the actorTypes.
            /// Updates items at the <see cref="DataGridViewComboBoxCell"/>.
            /// </summary>
            private void actorTypes_ItemsChanged(object sender, EventArgs e)
            {
                Items.Clear();

                foreach (ActorType actorType in actorTypes.Items)
                {
                    Items.Add(actorType);
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    actorTypes.ItemsChanged -= new EventHandler(actorTypes_ItemsChanged);
                    actorTypes = null;
                }
                base.Dispose(disposing);
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for an actor variable.
    /// </summary>
    [Serializable]
    class ActorVar : VarOnIName<Actor>, IDeserializationCallback
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Actor; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ActorVar"/> has a value according to its owner.
        /// </summary>
        public bool Owner
        {
            get { return _owner; }
            set
            {
                if (_owner != value)
                {
                    if (value) _value = null;
                    _owner = value;
                }
            }
        }
        private bool _owner = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorVar"/> class.
        /// </summary>
        public ActorVar()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private ActorVar(SerializationInfo info, StreamingContext ctxt)
        {
            if (ctxt.State == StreamingContextStates.Clone)
            {
                _valueIdForDeserialization = info.GetInt32("ActorId");
                if (_valueIdForDeserialization != -1) _findAtSceneForDeserialization = info.GetBoolean("FindAtScene");
            }
            else if (ctxt.State == StreamingContextStates.Persistence) _value = (Actor)info.GetValue("Actor", typeof(Actor));
            else Debug.Assert(true, "Not supported serialization state.");

            _owner = info.GetBoolean("Owner");
        }

        private int _valueIdForDeserialization = -1;
        private bool _findAtSceneForDeserialization;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueIdForDeserialization != -1)
            {
                if (_findAtSceneForDeserialization)
                {
                    _value = (Actor)Project.Singleton.Scenes.SelectedScene.FindActorById(_valueIdForDeserialization);
                }
                else
                {
                    _value = Actor.FindById(_valueIdForDeserialization, Project.Singleton.Prototypes);
                }
                Debug.Assert(_value != null, "Actor not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.Clone)
            {
                if (Value != null)
                {
                    Debug.Assert(Project.Singleton.Scenes.SelectedScene == Value.GetScene() || (Value.GetScene() == null && Actor.Contains(Value, Project.Singleton.Prototypes)), "Actor is not at the selected scene or is not prototype. Serialization is not possible.");

                    info.AddValue("ActorId", Value.Id);
                    info.AddValue("FindAtScene", Value.GetScene() != null);
                }
                else
                {
                    info.AddValue("ActorId", -1);
                }
            }
            else if (context.State == StreamingContextStates.Persistence) info.AddValue("Actor", Value);
            else Debug.Assert(true, "Not supported serialization state.");

            info.AddValue("Owner", Owner);
        }

        /// <inheritdoc />
        public override void SetValue(object value)
        {
            if (value == null)
            {
                _owner = false;
                Value = null;
            }
            else if (value.GetType() == typeof(string) && value.ToString() == PlatformGameCreator.GameEngine.Scripting.DefaultValueActorOwnerInstanceAttribute.ActorOwnerInstance)
            {
                _owner = true;
            }
            else
            {
                base.SetValue(value);
            }
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewActorVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new ActorVar() { Value = Value, _owner = Owner };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Value == null) return Owner ? "Owner" : "None";
            else return Value.ToString();
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing an actor variable.
        /// </summary>
        private class DataGridViewActorVarCell : DataGridViewVarTextBoxCell<Actor>
        {
            public override Type EditType
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the bounding rectangle of the import button.
            /// </summary>
            protected Rectangle ButtonImportBounds
            {
                get
                {
                    return new Rectangle(Size.Width - Size.Height * 3, 0, Size.Height, Size.Height);
                }
            }

            /// <summary>
            /// Gets the bounding rectangle of the remove button.
            /// </summary>
            protected Rectangle ButtonRemoveBounds
            {
                get
                {
                    return new Rectangle(Size.Width - Size.Height * 2, 0, Size.Height, Size.Height);
                }
            }

            /// <summary>
            /// Gets the bounding rectangle of the show button.
            /// </summary>
            protected Rectangle ButtonShowBounds
            {
                get
                {
                    return new Rectangle(Size.Width - Size.Height, 0, Size.Height, Size.Height);
                }
            }

            /// <summary>
            /// Gets the state of the import button.
            /// </summary>
            public PushButtonState ButtonImportState
            {
                get { return _buttonImportState; }
                set
                {
                    if (_buttonImportState != value)
                    {
                        _buttonImportState = value;
                        InvalidateCell();
                    }
                }
            }
            private PushButtonState _buttonImportState = PushButtonState.Normal;

            /// <summary>
            /// Gets the state of the remove button.
            /// </summary>
            public PushButtonState ButtonRemoveState
            {
                get { return _buttonRemoveState; }
                set
                {
                    if (_buttonRemoveState != value)
                    {
                        _buttonRemoveState = value;
                        InvalidateCell();
                    }
                }
            }
            private PushButtonState _buttonRemoveState = PushButtonState.Normal;

            /// <summary>
            /// Gets the state of the show button.
            /// </summary>
            public PushButtonState ButtonShowState
            {
                get { return _buttonShowState; }
                set
                {
                    if (_buttonShowState != value)
                    {
                        _buttonShowState = value;
                        InvalidateCell();
                    }
                }
            }
            private PushButtonState _buttonShowState = PushButtonState.Normal;

            // image for the show button
            private static Image showIcon;

            public DataGridViewActorVarCell(ActorVar value)
                : base(value)
            {
                Value = value;

                if (showIcon == null) showIcon = Properties.Resources.ZoomHS;
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable;
                InvalidateCell();
            }

            public override void CellValueChanged()
            {
                if (Value != null && Value is Actor)
                {
                    Variable.Value = (Actor)Value;
                }
            }

            protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
            {
                if (this.DataGridView != null && e.Button == MouseButtons.Left)
                {
                    if (ButtonImportBounds.Contains(e.Location))
                    {
                        ButtonImportState = PushButtonState.Pressed;
                    }
                    else if (ButtonRemoveBounds.Contains(e.Location))
                    {
                        ButtonRemoveState = PushButtonState.Pressed;
                    }
                    else if (ButtonShowBounds.Contains(e.Location))
                    {
                        ButtonShowState = PushButtonState.Pressed;
                    }
                }
            }

            protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
            {
                if (ButtonImportState == PushButtonState.Pressed)
                {
                    ButtonImportState = PushButtonState.Normal;

                    if (EditorApplicationForm.SceneSingleton.SelectedNodes.Count == 1)
                    {
                        Actor newActor = (EditorApplicationForm.SceneSingleton.SelectedNodes[0] as ActorView).Actor;

                        if (newActor != null)
                        {
                            Variable.Value = newActor;
                        }
                    }
                }
                else if (ButtonRemoveState == PushButtonState.Pressed)
                {
                    ButtonRemoveState = PushButtonState.Normal;

                    ((ActorVar)Variable).Owner = false;
                    Variable.Value = null;
                }
                else if (ButtonShowState == PushButtonState.Pressed)
                {
                    ButtonShowState = PushButtonState.Normal;

                    if (Variable.Value != null)
                    {
                        EditorApplicationForm.SceneSingleton.Center(Variable.Value);
                        EditorApplication.Editor.Select();
                    }
                }
            }

            protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
            {
                if (this.DataGridView != null)
                {
                    if (ButtonImportBounds.Contains(e.Location))
                    {
                        if (ButtonImportState != PushButtonState.Pressed)
                        {
                            ButtonImportState = PushButtonState.Hot;
                        }
                    }
                    else if (ButtonImportState != PushButtonState.Normal)
                    {
                        ButtonImportState = PushButtonState.Normal;
                    }

                    if (ButtonRemoveBounds.Contains(e.Location))
                    {
                        if (ButtonRemoveState != PushButtonState.Pressed)
                        {
                            ButtonRemoveState = PushButtonState.Hot;
                        }
                    }
                    else if (ButtonRemoveState != PushButtonState.Normal)
                    {
                        ButtonRemoveState = PushButtonState.Normal;
                    }

                    if (ButtonShowBounds.Contains(e.Location))
                    {
                        if (ButtonShowState != PushButtonState.Pressed)
                        {
                            ButtonShowState = PushButtonState.Hot;
                        }
                    }
                    else if (ButtonShowState != PushButtonState.Normal)
                    {
                        ButtonShowState = PushButtonState.Normal;
                    }
                }
            }

            protected override void OnMouseLeave(int rowIndex)
            {
                base.OnMouseLeave(rowIndex);

                if (rowIndex == RowIndex)
                {
                    if (ButtonImportState != PushButtonState.Normal)
                    {
                        ButtonImportState = PushButtonState.Normal;
                    }
                    if (ButtonRemoveState != PushButtonState.Normal)
                    {
                        ButtonRemoveState = PushButtonState.Normal;
                    }
                    if (ButtonShowState != PushButtonState.Normal)
                    {
                        ButtonShowState = PushButtonState.Normal;
                    }
                }
            }

            private void InvalidateCell()
            {
                if (DataGridView != null)
                {
                    DataGridView.Invalidate(DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, false));
                }
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

                // import button
                Rectangle buttonRect = ButtonImportBounds;
                buttonRect.Offset(cellBounds.X, cellBounds.Y);

                ButtonRenderer.DrawButton(graphics, buttonRect, "<", cellStyle.Font, ButtonImportState == PushButtonState.Pressed, ButtonImportState);

                // remove button
                buttonRect = ButtonRemoveBounds;
                buttonRect.Offset(cellBounds.X, cellBounds.Y);

                ButtonRenderer.DrawButton(graphics, buttonRect, "X", cellStyle.Font, ButtonRemoveState == PushButtonState.Pressed, ButtonRemoveState);

                // show button
                buttonRect = ButtonShowBounds;
                buttonRect.Offset(cellBounds.X, cellBounds.Y);
                Rectangle iconRect = new Rectangle(buttonRect.X + (buttonRect.Width - showIcon.Width) / 2, buttonRect.Y + (buttonRect.Height - showIcon.Height) / 2, showIcon.Width, showIcon.Height);

                ButtonRenderer.DrawButton(graphics, buttonRect, showIcon, iconRect, ButtonShowState == PushButtonState.Pressed, ButtonShowState);
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a vector variable.
    /// </summary>
    [Serializable]
    class Vector2Var : SystemVar<Vector2>
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Vector2; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2Var"/> class.
        /// </summary>
        public Vector2Var()
        {
        }

        /// <inheritdoc />
        private Vector2Var(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewVector2VarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new Vector2Var() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return String.Format("{0};{1}", Value.X.ToString(System.Globalization.CultureInfo.InvariantCulture), Value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a vector variable.
        /// </summary>
        private class DataGridViewVector2VarCell : DataGridViewVarTextBoxCell<Vector2>
        {
            public DataGridViewVector2VarCell(Vector2Var value)
                : base(value)
            {
                Value = value.ToString();
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.ToString();
            }

            public override void CellValueChanged()
            {
                if (Value != null)
                {
                    string[] parts = Value.ToString().Split(';');
                    float x = 0f, y = 0f;
                    if (parts.Length == 2)
                    {
                        if (!float.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out x))
                        {
                            float.TryParse(parts[0], out x);
                        }
                        if (!float.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out y))
                        {
                            float.TryParse(parts[1], out y);
                        }
                    }

                    if (float.IsNaN(x) || float.IsInfinity(x)) x = 0f;
                    if (float.IsNaN(y) || float.IsInfinity(y)) y = 0f;

                    Variable.Value = new Vector2(x, y);
                }
                else
                {
                    Variable.Value = new Vector2();
                }
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a sound or song variable.
    /// </summary>
    [Serializable]
    class SoundVar : VarOnIName<Sound>, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return isSound ? Scripting.VariableType.Sound : Scripting.VariableType.Song; }
        }

        /// <summary>
        /// Indicates whether this instance is sound variable or song variable.
        /// </summary>
        private bool isSound;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundVar"/> class.
        /// </summary>
        /// <param name="isSound">If set to <c>true</c> variable is sound variable; otherwise song variable.</param>
        public SoundVar(bool isSound)
        {
            this.isSound = isSound;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private SoundVar(SerializationInfo info, StreamingContext ctxt)
        {
            _valueIdForDeserialization = info.GetInt32("SoundId");
            isSound = info.GetBoolean("IsSound");
        }

        private int _valueIdForDeserialization;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueIdForDeserialization != -1)
            {
                _value = Project.Singleton.Sounds.FindById(_valueIdForDeserialization);
                Debug.Assert(_value != null, "Sound not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SoundId", Value != null ? Value.Id : -1);
            info.AddValue("IsSound", isSound);
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewSoundVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new SoundVar(isSound) { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Value == null) return "None";
            else return Value.Name;
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a soung or song variable.
        /// </summary>
        private class DataGridViewSoundVarCell : DataGridViewVarComboBoxCellOnObservableList<Sound>
        {
            public DataGridViewSoundVarCell(SoundVar value)
                : base(value, Project.Singleton.Sounds)
            {
                this.DisplayMember = "Name";
                this.ValueMember = "Id";

                if (value.Value != null)
                {
                    Value = value.Value.Id;
                }
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value.Id;
            }

            public override void CellValueChanged()
            {
                if (((SoundVar)Variable).isSound)
                {
                    if (Variable.Value != null) --Variable.Value.SoundEffectUsed;
                }
                else
                {
                    if (Variable.Value != null) --Variable.Value.SongUsed;
                }

                Variable.Value = Project.Singleton.Sounds.FindById((int)Value);

                if (((SoundVar)Variable).isSound)
                {
                    if (Variable.Value != null) ++Variable.Value.SoundEffectUsed;
                }
                else
                {
                    if (Variable.Value != null) ++Variable.Value.SongUsed;
                }
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a path variable.
    /// </summary>
    [Serializable]
    class PathVar : VarOnIName<Path>, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Path; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathVar"/> class.
        /// </summary>
        public PathVar()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private PathVar(SerializationInfo info, StreamingContext ctxt)
        {
            if (ctxt.State == StreamingContextStates.Clone) _valueIdForDeserialization = info.GetInt32("PathId");
            else if (ctxt.State == StreamingContextStates.Persistence) _value = (Path)info.GetValue("Path", typeof(Path));
            else Debug.Assert(true, "Not supported serialization state.");
        }

        private int _valueIdForDeserialization = -1;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueIdForDeserialization != -1)
            {
                Value = Project.Singleton.Scenes.SelectedScene.FindPathById(_valueIdForDeserialization);
                Debug.Assert(Value != null, "Path not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.Clone) info.AddValue("PathId", Value != null ? Value.Id : -1);
            else if (context.State == StreamingContextStates.Persistence) info.AddValue("Path", Value);
            else Debug.Assert(true, "Not supported serialization state.");
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewPathVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new PathVar() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Value == null) return "None";
            else return Value.Name;
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a path variable.
        /// </summary>
        private class DataGridViewPathVarCell : DataGridViewVarComboBoxCellOnObservableList<Path>
        {
            public DataGridViewPathVarCell(PathVar value)
                : base(value, Project.Singleton.Scenes.SelectedScene.Paths)
            {
                this.DisplayMember = "Name";
                this.ValueMember = "Id";

                if (value.Value != null)
                {
                    Value = value.Value.Id;
                }
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value.Id;
            }

            public override void CellValueChanged()
            {
                Variable.Value = Project.Singleton.Scenes.SelectedScene.FindPathById((int)Value);
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a texture variable.
    /// </summary>
    [Serializable]
    class TextureVar : VarOnIName<Texture>, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Texture; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureVar"/> class.
        /// </summary>
        public TextureVar()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private TextureVar(SerializationInfo info, StreamingContext ctxt)
        {
            _valueIdForDeserialization = info.GetInt32("TextureId");
        }

        private int _valueIdForDeserialization;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueIdForDeserialization != -1)
            {
                Value = Project.Singleton.Textures.FindById(_valueIdForDeserialization);
                Debug.Assert(_value != null, "Texture not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TextureId", Value != null ? Value.Id : -1);
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewTextureVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new TextureVar() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Value == null) return "None";
            else return Value.Name;
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a texture variable.
        /// </summary>
        private class DataGridViewTextureVarCell : DataGridViewVarComboBoxCellOnObservableList<Texture>
        {
            public DataGridViewTextureVarCell(TextureVar value)
                : base(value, Project.Singleton.Textures)
            {
                this.DisplayMember = "Name";
                this.ValueMember = "Id";

                if (value.Value != null)
                {
                    Value = value.Value.Id;
                }
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                if (Variable.Value != null) Value = Variable.Value.Id;
                else Value = null;
            }

            public override void CellValueChanged()
            {
                if (Value != null) Variable.Value = Project.Singleton.Textures.FindById((int)Value);
                else Variable.Value = null;
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for an animation variable.
    /// </summary>
    [Serializable]
    class AnimationVar : VarOnIName<Animation>, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Animation; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationVar"/> class.
        /// </summary>
        public AnimationVar()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private AnimationVar(SerializationInfo info, StreamingContext ctxt)
        {
            _valueIdForDeserialization = info.GetInt32("AnimationId");
        }

        private int _valueIdForDeserialization;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueIdForDeserialization != -1)
            {
                Value = Project.Singleton.Animations.FindById(_valueIdForDeserialization);
                Debug.Assert(_value != null, "Animation not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AnimationId", Value != null ? Value.Id : -1);
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewAnimationVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new AnimationVar() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Value == null) return "None";
            else return Value.Name;
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing an animation variable.
        /// </summary>
        private class DataGridViewAnimationVarCell : DataGridViewVarComboBoxCellOnObservableList<Animation>
        {
            public DataGridViewAnimationVarCell(AnimationVar value)
                : base(value, Project.Singleton.Animations)
            {
                this.DisplayMember = "Name";
                this.ValueMember = "Id";

                if (value.Value != null)
                {
                    Value = value.Value.Id;
                }
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value.Id;
            }

            public override void CellValueChanged()
            {
                Variable.Value = Project.Singleton.Animations.FindById((int)Value);
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a scene variable.
    /// </summary>
    [Serializable]
    class SceneVar : VarOnIName<Scene>, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Scene; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneVar"/> class.
        /// </summary>
        public SceneVar()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        private SceneVar(SerializationInfo info, StreamingContext ctxt)
        {
            _valueIdForDeserialization = info.GetInt32("SceneIndex");
        }

        private int _valueIdForDeserialization;

        /// <inheritdoc />
        public override void OnDeserialization(object sender)
        {
            if (_valueIdForDeserialization != -1)
            {
                Value = Project.Singleton.Scenes[_valueIdForDeserialization];
                Debug.Assert(_value != null, "Scene not found.");
            }

            base.OnDeserialization(sender);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SceneIndex", Value != null ? Value.Index : -1);
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewSceneVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new SceneVar() { Value = Value };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Value == null) return "None";
            else return Value.Name;
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a scene variable.
        /// </summary>
        private class DataGridViewSceneVarCell : DataGridViewVarComboBoxCellOnObservableList<Scene>
        {
            public DataGridViewSceneVarCell(SceneVar value)
                : base(value, Project.Singleton.Scenes)
            {
                this.DisplayMember = "Name";
                this.ValueMember = "Index";

                if (value.Value != null)
                {
                    Value = value.Value.Index;
                }
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                if (Variable.Value != null) Value = Variable.Value.Index;
                else Value = null;
            }

            public override void CellValueChanged()
            {
                if (Value != null) Variable.Value = Project.Singleton.Scenes[(int)Value];
                else Variable.Value = null;
            }
        }
    }

    /// <summary>
    /// Represents a wrapper for a key variable.
    /// </summary>
    [Serializable]
    class KeyVar : SystemVar<Key>
    {
        /// <inheritdoc />
        public override VariableType VariableType
        {
            get { return Scripting.VariableType.Key; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVar"/> class.
        /// </summary>
        public KeyVar()
        {
            Value = Key.None;
        }

        /// <inheritdoc />
        private KeyVar(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <inheritdoc />
        public override DataGridViewCell GetGridCell()
        {
            return new DataGridViewKeyVarCell(this);
        }

        /// <inheritdoc />
        public override IVariable Clone()
        {
            return new KeyVar() { Value = Value };
        }

        /// <summary>
        /// <see cref="DataGridViewCell"/> representing a key variable.
        /// </summary>
        private class DataGridViewKeyVarCell : DataGridViewVarComboBoxCell<Key>
        {
            public DataGridViewKeyVarCell(KeyVar value)
                : base(value)
            {
                ValueType = typeof(Key);

                foreach (Key key in Enum.GetValues(typeof(Key)))
                {
                    Items.Add(key);
                }

                Value = value.Value;
            }

            protected override void Variable_ValueChanged(object sender, EventArgs e)
            {
                Value = Variable.Value;
            }

            public override void CellValueChanged()
            {
                Variable.Value = (Key)Value;
            }
        }
    }

    /// <summary>
    /// Factory for creating a <see cref="IVariable"/>.
    /// </summary>
    class VarFactory
    {
        /// <summary>
        /// Creates the variable by the specified variable type.
        /// </summary>
        /// <param name="variableType">Type of the variable to create.</param>
        /// <returns>Created variable.</returns>
        public static IVariable Create(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Int:
                    return new IntVar();

                case VariableType.Float:
                    return new FloatVar();

                case VariableType.Bool:
                    return new BoolVar();

                case VariableType.String:
                    return new StringVar();

                case VariableType.Vector2:
                    return new Vector2Var();

                case VariableType.ActorType:
                    return new ActorTypeVar();

                case VariableType.Actor:
                    return new ActorVar();

                case VariableType.Sound:
                    return new SoundVar(true);

                case VariableType.Song:
                    return new SoundVar(false);

                case VariableType.Path:
                    return new PathVar();

                case VariableType.Texture:
                    return new TextureVar();

                case VariableType.Animation:
                    return new AnimationVar();

                case VariableType.Scene:
                    return new SceneVar();

                case VariableType.Key:
                    return new KeyVar();

                default:
                    Debug.Assert(true, "Variable does not exist.");
                    return null;
            }
        }
    }
}
