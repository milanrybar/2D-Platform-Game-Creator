/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Defines a state machine at the visual scripting.
    /// </summary>
    [Serializable]
    class StateMachine : IName, ISerializable
    {
        /// <summary>
        /// Gets the <see cref="Scripting.ScriptingComponent"/> where the state machine is used.
        /// </summary>
        public ScriptingComponent ScriptingComponent
        {
            get { return _scriptingComponent; }
        }
        private ScriptingComponent _scriptingComponent;

        /// <summary>
        /// Gets or sets the name of the state machine.
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
        /// Gets the states of the state machine.
        /// </summary>
        public ObservableIndexedList<State> States
        {
            get { return _states; }
        }
        private ObservableIndexedList<State> _states;

        /// <summary>
        /// Gets or sets the starting state of the state machine.
        /// </summary>
        public State StartingState
        {
            get { return _startingState; }
            set
            {
                _startingState = value;
                if (StartingStateChanged != null) StartingStateChanged(this, EventArgs.Empty);
            }
        }
        private State _startingState;

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Occurs when the <see cref="StartingState"/> property value changes.
        /// </summary>
        public event EventHandler StartingStateChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        /// <param name="scriptingComponent">The <see cref="Scripting.ScriptingComponent"/> where the state machine will be used.</param>
        public StateMachine(ScriptingComponent scriptingComponent)
        {
            _scriptingComponent = scriptingComponent;
            _states = new ObservableIndexedList<State>();
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private StateMachine(SerializationInfo info, StreamingContext ctxt)
        {
            _scriptingComponent = (ScriptingComponent)info.GetValue("ScriptingComponent", typeof(ScriptingComponent));
            _name = info.GetString("Name");
            _states = (ObservableIndexedList<State>)info.GetValue("States", typeof(ObservableIndexedList<State>));
            _startingState = (State)info.GetValue("StartingState", typeof(State));
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ScriptingComponent", ScriptingComponent);
            info.AddValue("Name", Name);
            info.AddValue("States", States);
            info.AddValue("StartingState", StartingState);
        }
    }

    /// <summary>
    /// Defines a state for <see cref="StateMachine"/>.
    /// </summary>
    [Serializable]
    class State : IName, IIndex, ISerializable
    {
        /// <summary>
        /// Gets the <see cref="StateMachine"/> where the state is used.
        /// </summary>
        public StateMachine StateMachine
        {
            get { return _stateMachine; }
        }
        private StateMachine _stateMachine;

        /// <summary>
        /// Gets or sets the name of the state.
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
        /// Gets or sets the location of the state.
        /// </summary>
        /// <remarks>
        /// Used as the position of the state at the <see cref="StateMachineView"/>.
        /// </remarks>
        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                if (LocationChanged != null) LocationChanged(this, EventArgs.Empty);
            }
        }
        private Point _location;

        /// <summary>
        /// Gets the scripting nodes of the state.
        /// </summary>
        public ObservableList<BaseNode> Nodes
        {
            get { return _nodes; }
        }
        private ObservableList<BaseNode> _nodes;

        /// <summary>
        /// Gets the transitions of the state.
        /// </summary>
        public ObservableList<Transition> Transitions
        {
            get { return _transitions; }
        }
        public ObservableList<Transition> _transitions;

        /// <summary>
        /// Gets or sets the index of the state in the <see cref="StateMachine"/> where is used.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Occurs when the <see cref="Location"/> property value changes.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        /// <param name="stateMachine">The <see cref="StateMachine"/> where the state will be used.</param>
        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _nodes = new ObservableList<BaseNode>();
            _transitions = new ObservableList<Transition>();
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private State(SerializationInfo info, StreamingContext ctxt)
        {
            _stateMachine = (StateMachine)info.GetValue("StateMachine", typeof(StateMachine));
            _name = info.GetString("Name");
            _location = (Point)info.GetValue("Location", typeof(Point));
            _nodes = (ObservableList<BaseNode>)info.GetValue("Nodes", typeof(ObservableList<BaseNode>));
            _transitions = (ObservableList<Transition>)info.GetValue("Transitions", typeof(ObservableList<Transition>));
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StateMachine", StateMachine);
            info.AddValue("Name", Name);
            info.AddValue("Location", Location);
            info.AddValue("Nodes", Nodes);
            info.AddValue("Transitions", Transitions);
        }

        /// <summary>
        /// Creates the <see cref="StateView"/> representing this state.
        /// </summary>
        /// <returns><see cref="StateView"/> representing this state.</returns>
        public StateView CreateView()
        {
            return new StateView(this);
        }
    }

    /// <summary>
    /// Defines a transition between <see cref="State"/> at <see cref="StateMachine"/>.
    /// </summary>
    [Serializable]
    class Transition : ISerializable
    {
        /// <summary>
        /// Gets or sets the event in of the state that represents the name of the transition.
        /// </summary>
        public Event Event
        {
            get { return _event; }
            set
            {
                _event = value;
                if (EventChanged != null) EventChanged(this, EventArgs.Empty);
            }
        }
        private Event _event;

        /// <summary>
        /// Gets the starting state of the transition.
        /// </summary>
        /// <remarks>
        /// Transition is stored at this state.
        /// </remarks>
        public State StateFrom
        {
            get { return _stateFrom; }
        }
        private State _stateFrom;

        /// <summary>
        /// Gets or sets the ending state of the transition.
        /// </summary>
        public State StateTo
        {
            get { return _stateTo; }
            set
            {
                _stateTo = value;
                if (StateToChanged != null) StateToChanged(this, EventArgs.Empty);
            }
        }
        private State _stateTo;

        /// <summary>
        /// Occurs when the <see cref="Event"/> property value changes.
        /// </summary>
        public event EventHandler EventChanged;

        /// <summary>
        /// Occurs when the <see cref="StateTo"/> property value changes.
        /// </summary>
        public event EventHandler StateToChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class.
        /// </summary>
        /// <param name="stateFrom">The state where the transition will be used.</param>
        public Transition(State stateFrom)
        {
            _stateFrom = stateFrom;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Transition(SerializationInfo info, StreamingContext ctxt)
        {
            _event = (Event)info.GetValue("Event", typeof(Event));
            _stateFrom = (State)info.GetValue("StateFrom", typeof(State));
            _stateTo = (State)info.GetValue("StateTo", typeof(State));
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Event", Event);
            info.AddValue("StateFrom", StateFrom);
            info.AddValue("StateTo", StateTo);
        }
    }
}
