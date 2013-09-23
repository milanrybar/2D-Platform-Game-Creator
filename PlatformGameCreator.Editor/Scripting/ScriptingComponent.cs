/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scenes;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a visual scripting.
    /// </summary>
    [Serializable]
    class ScriptingComponent : ISerializable
    {
        /// <summary>
        /// Gets the actor where the <see cref="ScriptingComponent"/> is used.
        /// </summary>
        /// <remarks>
        /// If <c>null</c> then the scripting represents the scene scripting and is used at the <see cref="Scene"/>.
        /// </remarks>
        public Actor Actor
        {
            get { return _actor; }
        }
        private Actor _actor;

        /// <summary>
        /// Gets or sets the scene where the <see cref="ScriptingComponent"/> is used, if the scripting represents the scene scripting.
        /// </summary>
        public Scene Scene { get; set; }

        /// <summary>
        /// Gets variables of the scripting.
        /// </summary>
        public ObservableList<NamedVariable> Variables
        {
            get { return _variables; }
        }
        private ObservableList<NamedVariable> _variables;

        /// <summary>
        /// Get events in (transitions) of the scripting.
        /// </summary>
        public ObservableIndexedList<Event> EventsIn
        {
            get { return _eventsIn; }
        }
        private ObservableIndexedList<Event> _eventsIn;

        /// <summary>
        /// Gets events out of the scripting.
        /// </summary>
        public ObservableIndexedList<Event> EventsOut
        {
            get { return _eventsOut; }
        }
        private ObservableIndexedList<Event> _eventsOut;

        /// <summary>
        /// Gets state machines of the scripting.
        /// </summary>
        public ObservableList<StateMachine> StateMachines
        {
            get { return _stateMachines; }
        }
        private ObservableList<StateMachine> _stateMachines;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingComponent"/> class.
        /// </summary>
        /// <param name="actor">The actor where the scripting will be used.</param>
        public ScriptingComponent(Actor actor)
        {
            _actor = actor;

            _variables = new ObservableList<NamedVariable>();
            _eventsIn = new ObservableIndexedList<Event>();
            _eventsOut = new ObservableIndexedList<Event>();
            _stateMachines = new ObservableList<StateMachine>();

            // create default state machine with default state
            StateMachines.Add(new StateMachine(this) { Name = "Default" });
            StateMachines[0].States.Add(new State(StateMachines[0]) { Name = "Default", Location = new Point(100, 100) });
            StateMachines[0].StartingState = StateMachines[0].States[0];
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private ScriptingComponent(SerializationInfo info, StreamingContext ctxt)
        {
            if (ctxt.State == StreamingContextStates.Clone)
            {
                _actor = ctxt.Context as Actor;
            }
            else if (ctxt.State == StreamingContextStates.Persistence)
            {
                _actor = (Actor)info.GetValue("Actor", typeof(Actor));
            }

            _variables = (ObservableList<NamedVariable>)info.GetValue("Variables", typeof(ObservableList<NamedVariable>));
            _eventsIn = (ObservableIndexedList<Event>)info.GetValue("EventsIn", typeof(ObservableIndexedList<Event>));
            _eventsOut = (ObservableIndexedList<Event>)info.GetValue("EventsOut", typeof(ObservableIndexedList<Event>));
            _stateMachines = (ObservableList<StateMachine>)info.GetValue("StateMachines", typeof(ObservableList<StateMachine>));
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.Clone)
            {
            }
            else if (context.State == StreamingContextStates.Persistence)
            {
                info.AddValue("Actor", Actor);
            }

            info.AddValue("Variables", Variables);
            info.AddValue("EventsIn", EventsIn);
            info.AddValue("EventsOut", EventsOut);
            info.AddValue("StateMachines", StateMachines);
        }

        /// <summary>
        /// Clones this scripting. The cloned scripting will be used at the specified actor.
        /// </summary>
        /// <param name="actor">The actor where the scripting will be used.</param>
        /// <returns>The cloned scripting.</returns>
        public ScriptingComponent Clone(Actor actor)
        {
            // clone via serialization
            BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone, actor));
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (ScriptingComponent)formatter.Deserialize(stream);
            }
        }
    }
}
