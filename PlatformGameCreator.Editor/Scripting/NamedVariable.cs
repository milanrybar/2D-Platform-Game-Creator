/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a named variable used at visual scripting.
    /// </summary>
    [Serializable]
    class NamedVariable : IName, ISerializable
    {
        /// <summary>
        /// Gets the <see cref="Scripting.ScriptingComponent"/> where the named variable is used.
        /// </summary>
        public ScriptingComponent ScriptingComponent
        {
            get { return _scriptingComponent; }
        }
        private ScriptingComponent _scriptingComponent;

        /// <summary>
        /// Gets or sets the name of the named variable.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (NameChanged != null) NameChanged(this, EventArgs.Empty);
            }
        }
        private string _name;

        /// <summary>
        /// Gets the type of the named variable.
        /// </summary>
        public VariableType VariableType
        {
            get { return Value.VariableType; }
        }

        /// <summary>
        /// Gets the value of the named variable.
        /// </summary>
        public IVariable Value
        {
            get { return _value; }
        }
        private IVariable _value;

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedVariable"/> class.
        /// </summary>
        /// <param name="scriptingComponent"><see cref="Scripting.ScriptingComponent"/> where the named variable will be used.</param>
        /// <param name="variableType">Type of the variable.</param>
        public NamedVariable(ScriptingComponent scriptingComponent, VariableType variableType)
        {
            _scriptingComponent = scriptingComponent;
            _value = VarFactory.Create(variableType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedVariable"/> class.
        /// </summary>
        /// <param name="scriptingComponent"><see cref="Scripting.ScriptingComponent"/> where the named variable will be used.</param>
        /// <param name="value">The default value.</param>
        public NamedVariable(ScriptingComponent scriptingComponent, IVariable value)
        {
            _scriptingComponent = scriptingComponent;
            _value = value;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private NamedVariable(SerializationInfo info, StreamingContext ctxt)
        {
            _scriptingComponent = (ScriptingComponent)info.GetValue("ScriptingComponent", typeof(ScriptingComponent));
            _value = (IVariable)info.GetValue("Value", typeof(IVariable));
            _name = info.GetString("Name");
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ScriptingComponent", ScriptingComponent);
            info.AddValue("Value", Value);
            info.AddValue("Name", Name);
        }

        /// <summary>
        /// Clones this named variable.
        /// </summary>
        /// <returns>Cloned named variable.</returns>
        public NamedVariable Clone()
        {
            return Clone(ScriptingComponent);
        }

        /// <summary>
        /// Clones this named variable. The cloned named variable will be used at the specified <see cref="Scripting.ScriptingComponent"/>.
        /// </summary>
        /// <param name="scriptingComponent"><see cref="Scripting.ScriptingComponent"/> where the named variable will be used.</param>
        /// <returns>Cloned named variable.</returns>
        public NamedVariable Clone(ScriptingComponent scriptingComponent)
        {
            return new NamedVariable(scriptingComponent, Value.Clone()) { Name = Name };
        }

        /// <summary>
        /// Finds the named variable by the specified name at the specified collection.
        /// </summary>
        /// <param name="namedVariableName">Name of the named variable.</param>
        /// <param name="collectionOfNamedVariables">The collection of named variables.</param>
        /// <returns><see cref="NamedVariable"/> if found; otherwise <c>null</c>.</returns>
        public static NamedVariable FindByName(string namedVariableName, IEnumerable<NamedVariable> collectionOfNamedVariables)
        {
            foreach (NamedVariable namedVariable in collectionOfNamedVariables)
            {
                if (namedVariable.Name == namedVariableName)
                {
                    return namedVariable;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified collection contains the named variable defined by the specified name.
        /// </summary>
        /// <param name="namedVariableName">Name of the named variable.</param>
        /// <param name="collectionOfNamedVariables">The collection of named variables.</param>
        /// <returns><c>true</c> if the specified collection contains the named variable; otherwise <c>false</c>.</returns>
        public static bool ContainsByName(string namedVariableName, IEnumerable<NamedVariable> collectionOfNamedVariables)
        {
            return FindByName(namedVariableName, collectionOfNamedVariables) != null;
        }
    }
}
