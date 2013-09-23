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
using System.ComponentModel;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.GameObjects.Actors;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a variable used at visual scripting.
    /// </summary>
    [Serializable]
    class Variable : BaseNode, ISerializable, IDeserializationCallback
    {
        /// <summary>
        /// Gets or sets the named variable of the script variable if any.
        /// </summary>
        /// <remarks>
        /// If the named variable is set variable has value of the named variable and the variable represents the named variable.
        /// </remarks>
        public NamedVariable NamedVariable
        {
            get { return _namedVariable; }
            set
            {
                if (_namedVariable != value)
                {
                    NamedVariable oldValue = _namedVariable;

                    if (value == null)
                    {
                        _value = VarFactory.Create(NamedVariable.VariableType);
                    }
                    else
                    {
                        _value = value.Value;
                    }

                    _namedVariable = value;

                    if (NamedVariableChanged != null) NamedVariableChanged(this, new ValueChangedEventArgs<NamedVariable>(oldValue));
                }
            }
        }
        private NamedVariable _namedVariable;

        /// <summary>
        /// Gets the type of the script variable.
        /// </summary>
        public VariableType VariableType
        {
            get { return Value.VariableType; }
        }

        /// <summary>
        /// Gets the value of the script variable.
        /// </summary>
        public IVariable Value
        {
            get { return _value; }
        }
        private IVariable _value;

        /// <summary>
        /// Occurs when the <see cref="NamedVariable"/> property value changes.
        /// </summary>
        public event ValueChangedHandler<NamedVariable> NamedVariableChanged;

        private int _actorIdForDeserialization = -1;
        private string _namedVariableNameForDeserialization;
        private int _namedVariableFindAtForDeserialization;

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="state">The state where the script variable will be used.</param>
        /// <param name="variableType">Type of the variable.</param>
        public Variable(State state, VariableType variableType)
            : base(state)
        {
            _value = VarFactory.Create(variableType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="state">The state where the script variable will be used.</param>
        /// <param name="value">The default value.</param>
        public Variable(State state, IVariable value)
            : base(state)
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="state">The state where the script variable will be used.</param>
        /// <param name="namedVariable">The named variable that will be used for the variable.</param>
        public Variable(State state, NamedVariable namedVariable)
            : base(state)
        {
            _namedVariable = namedVariable;
            _value = _namedVariable.Value;
        }

        /// <inheritdoc />
        protected Variable(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            if (ctxt.State == StreamingContextStates.Clone)
            {
                _actorIdForDeserialization = info.GetInt32("ActorId");

                if (_actorIdForDeserialization != -1)
                {
                    _namedVariableNameForDeserialization = info.GetString("NamedVariableName");
                    _namedVariableFindAtForDeserialization = info.GetInt32("FindAt");
                }
                else
                {
                    _value = (IVariable)info.GetValue("Value", typeof(IVariable));
                }
            }
            else if (ctxt.State == StreamingContextStates.Persistence)
            {
                _namedVariable = (NamedVariable)info.GetValue("NamedVariable", typeof(NamedVariable));

                if (_namedVariable == null) _value = (IVariable)info.GetValue("Value", typeof(IVariable));
                else _value = _namedVariable.Value;
            }
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (context.State == StreamingContextStates.Clone)
            {
                if (NamedVariable == null)
                {
                    info.AddValue("ActorId", -1);
                }
                else
                {
                    Debug.Assert(NamedVariable.ScriptingComponent.Actor != null || NamedVariable.ScriptingComponent == Project.Singleton.Scenes.SelectedScene.GlobalScript, "Variable with named variable in non actor scripting component or not global script is not supported. Serialization is not possible.");
                    Debug.Assert(NamedVariable.ScriptingComponent == Project.Singleton.Scenes.SelectedScene.GlobalScript || Project.Singleton.Scenes.SelectedScene == NamedVariable.ScriptingComponent.Actor.GetScene() || (NamedVariable.ScriptingComponent.Actor.GetScene() == null && Actor.Contains(NamedVariable.ScriptingComponent.Actor, Project.Singleton.Prototypes)), "Actor is not at the selected scene or is not prototype. Serialization is not possible.");

                    if (NamedVariable.ScriptingComponent.Actor != null)
                    {
                        info.AddValue("ActorId", NamedVariable.ScriptingComponent.Actor.Id);
                        info.AddValue("NamedVariableName", NamedVariable.Name);

                        // named variable of the actor at the scene
                        if (NamedVariable.ScriptingComponent.Actor.GetScene() != null) info.AddValue("FindAt", 0);
                        // named variable of the prototype actor
                        else info.AddValue("FindAt", 1);

                    }
                    // global script named variable
                    else
                    {
                        info.AddValue("ActorId", 0);
                        info.AddValue("NamedVariableName", NamedVariable.Name);
                        info.AddValue("FindAt", 2);
                    }
                }
            }
            else if (context.State == StreamingContextStates.Persistence)
            {
                info.AddValue("NamedVariable", NamedVariable);
            }

            if (NamedVariable == null) info.AddValue("Value", Value);
        }

        /// <inheritdoc />
        public void OnDeserialization(object sender)
        {
            if (_actorIdForDeserialization != -1)
            {
                IList<NamedVariable> namedVariablesList = null;

                // named variable of the actor at the scene
                if (_namedVariableFindAtForDeserialization == 0)
                {
                    Actor actor = (Actor)Project.Singleton.Scenes.SelectedScene.FindActorById(_actorIdForDeserialization);
                    Debug.Assert(actor != null, "Actor not found.");
                    namedVariablesList = actor.Scripting.Variables;
                }
                // named variable of the prototype actor
                else if (_namedVariableFindAtForDeserialization == 1)
                {
                    Actor actor = Actor.FindById(_actorIdForDeserialization, Project.Singleton.Prototypes);
                    Debug.Assert(actor != null, "Actor not found.");
                    namedVariablesList = actor.Scripting.Variables;
                }
                // global script named variable
                else if (_namedVariableFindAtForDeserialization == 2)
                {
                    namedVariablesList = Project.Singleton.Scenes.SelectedScene.GlobalScript.Variables;
                }

                Debug.Assert(namedVariablesList != null, "Named Variables List not found.");

                _namedVariable = NamedVariable.FindByName(_namedVariableNameForDeserialization, namedVariablesList);
                Debug.Assert(_namedVariable != null, "Named Variable not found.");
            }

            if (_namedVariable != null) _value = _namedVariable.Value;
        }

        /// <summary>
        /// Removes the script variable from the state. Also removes all its connections.
        /// </summary>
        public override void Remove()
        {
            // remove all connections
            foreach (BaseNode baseNode in State.Nodes)
            {
                Node node = baseNode as Node;
                if (node != null)
                {
                    foreach (NodeSocket nodeSocket in node.Sockets)
                    {
                        VariableNodeSocket variableNodeSocket = nodeSocket as VariableNodeSocket;
                        if (variableNodeSocket != null)
                        {
                            for (int i = 0; i < variableNodeSocket.Connections.Count; ++i)
                            {
                                if (variableNodeSocket.Connections[i] == this)
                                {
                                    variableNodeSocket.Connections.RemoveAt(i);
                                    --i;
                                }
                            }
                        }
                    }
                }
            }

            base.Remove();
        }

        /// <inheritdoc />
        public override BaseNode Clone()
        {
            return Clone(State);
        }

        /// <inheritdoc />
        public override BaseNode Clone(State state)
        {
            if (NamedVariable == null)
            {
                return new Variable(state, Value.Clone()) { Comment = Comment, Location = Location };
            }
            else
            {
                return new Variable(state, NamedVariable) { Comment = Comment, Location = Location };
            }
        }

        /// <inheritdoc />
        public override SceneNode CreateView()
        {
            return new VariableView(this);
        }
    }
}
