/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.Editor.Common;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Scripting;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.GameObjects.Paths;
using PlatformGameCreator.Editor.GameObjects.Actors;
using System.Collections;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Represents scene of the game.
    /// </summary>
    [Serializable]
    class Scene : ISerializable, IName, IIndex, IDeserializationCallback
    {
        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    if (NameChanged != null) NameChanged(this, EventArgs.Empty);
                }
            }
        }
        private string _name;

        /// <summary>
        /// Gets or sets the index of the scene in the <see cref="ScenesManager"/>.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets the scene scripting.
        /// </summary>
        public ScriptingComponent GlobalScript
        {
            get { return _globalScript; }
        }
        private ScriptingComponent _globalScript;

        /// <summary>
        /// Gets layers at the scene.
        /// </summary>
        public ObservableIndexedList<Layer> Layers
        {
            get { return _layers; }
        }
        private ObservableIndexedList<Layer> _layers;

        /// <summary>
        /// Gets paths at the scene.
        /// </summary>
        public ObservableIndexedList<Path> Paths
        {
            get { return _paths; }
        }
        private ObservableIndexedList<Path> _paths;

        /// <summary>
        /// Gets or sets the selected layer at the scene.
        /// </summary>
        public Layer SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                if (_selectedLayer != value)
                {
                    _selectedLayer = value;
                    if (SelectedLayerChanged != null) SelectedLayerChanged(this, EventArgs.Empty);
                }
            }
        }
        private Layer _selectedLayer;

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Occurs when the <see cref="SelectedLayer"/> property value changes.
        /// </summary>
        public event EventHandler SelectedLayerChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// Creates the default layer for the scene.
        /// </summary>
        public Scene()
        {
            _globalScript = new Scripting.ScriptingComponent(null);
            _layers = new ObservableIndexedList<Layer>();
            _paths = new ObservableIndexedList<Path>();

            // default layer
            _selectedLayer = new Layer(this) { Name = "Default" };
            _layers.Add(_selectedLayer);
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Scene(SerializationInfo info, StreamingContext ctxt)
        {
            _name = info.GetString("Name");
            _paths = (ObservableIndexedList<Path>)info.GetValue("Paths", typeof(ObservableIndexedList<Path>));
            _layers = (ObservableIndexedList<Layer>)info.GetValue("Layers", typeof(ObservableIndexedList<Layer>));
            _selectedLayer = (Layer)info.GetValue("SelectedLayer", typeof(Layer));
            _globalScript = (ScriptingComponent)info.GetValue("GlobalScript", typeof(ScriptingComponent));
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Paths", Paths);
            info.AddValue("Layers", Layers);
            info.AddValue("SelectedLayer", SelectedLayer);
            info.AddValue("GlobalScript", GlobalScript);
        }

        /// <inheritdoc />
        public void OnDeserialization(object sender)
        {
            GlobalScript.Scene = this;
        }

        /// <summary>
        /// Finds the actor by the specified id at the scene. Also is finding at the actor children.
        /// </summary>
        /// <param name="id">The id of the actor to find.</param>
        /// <returns>Actor if found; otherwise <c>null</c>.</returns>
        public Actor FindActorById(int id)
        {
            foreach (Layer layer in Layers)
            {
                Actor actor = layer.FindById(id);
                if (actor != null)
                {
                    return actor;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the path by the specified id at the scene.
        /// </summary>
        /// <param name="id">The id of the path to find.</param>
        /// <returns>Path if found; otherwise <c>null</c>.</returns>
        public Path FindPathById(int id)
        {
            foreach (Path path in Paths)
            {
                if (path.Id == id)
                {
                    return path;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all actors without their children at the scene.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerable<Actor> AllActors()
        {
            foreach (Layer layer in Layers)
            {
                foreach (Actor actor in layer)
                {
                    yield return actor;
                }
            }
        }

        /// <summary>
        /// Gets all game objects at the scene (actors are without their children).
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerable<GameObject> AllGameObjects()
        {
            foreach (Layer layer in Layers)
            {
                foreach (Actor actor in layer)
                {
                    yield return actor;
                }
            }

            foreach (Path path in Paths)
            {
                yield return path;
            }
        }

        /// <summary>
        /// Adds the specified path to the scene.
        /// </summary>
        /// <param name="path">The path to add.</param>
        public void Add(Path path)
        {
            Paths.Add(path);
        }

        /// <summary>
        /// Adds the specified actor to the selected layer at the scene.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        public void Add(Actor actor)
        {
            Debug.Assert(SelectedLayer != null, "No selected layer.");

            SelectedLayer.Add(actor);
        }
    }
}
