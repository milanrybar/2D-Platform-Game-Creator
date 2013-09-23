/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.Assets.Sounds;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.Xna;
using System.Diagnostics;
using PlatformGameCreator.Editor.Assets;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Represents a project. Project contains the whole game.
    /// </summary>
    [Serializable]
    class Project : Disposable, ISerializable
    {
        /// <summary>
        /// Gets the singleton of the <see cref="Project"/>. Only one project can be opened.
        /// </summary>
        public static Project Singleton
        {
            get { return _singleton; }
        }
        private static Project _singleton;

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <remarks>
        /// The name of the project is used for the filename of the project.
        /// </remarks>
        public string Name
        {
            get { return _name; }
        }
        private string _name;

        /// <summary>
        /// Gets the directory of the project.
        /// </summary>
        /// <remarks>
        /// Directory of the project contains:
        /// <list type="bullet">
        /// <item><description>file that represents the project</description></item>
        /// <item><description>generated source code of the game</description></item>
        /// <item><description>content directory</description></item>
        /// <item><description>publish directory; after the game is published</description></item>
        /// </list>
        /// </remarks>
        public string ProjectDirectory
        {
            get
            {
                return _projectDirectory;
            }
            private set
            {
                _projectDirectory = value;
                ContentDirectory = Path.Combine(_projectDirectory, "Content");
            }
        }
        private string _projectDirectory;

        /// <summary>
        /// Gets the content directory of the project.
        /// </summary>
        /// <remarks>
        /// Content directory contains all content that are used in the project.
        /// </remarks>
        public string ContentDirectory
        {
            get
            {
                return _contentDirectory;
            }
            private set
            {
                _contentDirectory = value;
                if (XnaFramework.ContentManager.RootDirectory != _contentDirectory)
                {
                    XnaFramework.ContentManager.RootDirectory = _contentDirectory;
                    if (!Directory.Exists(_contentDirectory)) Directory.CreateDirectory(_contentDirectory);
                }
            }
        }
        private string _contentDirectory;

        /// <summary>
        /// Gets the filename of the project (full path).
        /// </summary>
        public string ProjectFilename
        {
            get { return Path.Combine(ProjectDirectory, String.Format("{0}.pgcproject", Name)); }
        }

        /// <summary>
        /// Gets the textures of the project.
        /// </summary>
        public TexturesManager Textures
        {
            get { return _textures; }
        }
        private TexturesManager _textures;

        /// <summary>
        /// Gets the animations of the project.
        /// </summary>
        public AnimationsManager Animations
        {
            get { return _animations; }
        }
        private AnimationsManager _animations;

        /// <summary>
        /// Gets the sounds of the project.
        /// </summary>
        public SoundsManager Sounds
        {
            get { return _sounds; }
        }
        private SoundsManager _sounds;

        /// <summary>
        /// Gets the scenes of the project.
        /// </summary>
        public ScenesManager Scenes
        {
            get { return _scenes; }
        }
        private ScenesManager _scenes;

        /// <summary>
        /// Gets the actor types of the project.
        /// </summary>
        public ActorTypesManager ActorTypes
        {
            get { return _actorTypes; }
        }
        private ActorTypesManager _actorTypes;

        /// <summary>
        /// Gets the prototypes (<see cref="Actor"/>) of the project.
        /// </summary>
        public ObservableList<Actor> Prototypes
        {
            get { return _prototypes; }
        }
        private ObservableList<Actor> _prototypes;

        /// <summary>
        /// Gets the settings of the project.
        /// </summary>
        public ProjectSettings Settings
        {
            get { return _settings; }
        }
        private ProjectSettings _settings;

        /// <summary>
        /// Prevents a default instance of the <see cref="Project"/> class from being created.
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        private Project(string projectDirectory)
        {
            _singleton = this;
            _name = "NewProject";
            ProjectDirectory = projectDirectory;
            _textures = new TexturesManager();
            _animations = new AnimationsManager();
            _sounds = new SoundsManager();
            _scenes = new ScenesManager();
            _actorTypes = new ActorTypesManager();
            _prototypes = new ObservableList<Actor>();
            _settings = new ProjectSettings();
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Project(SerializationInfo info, StreamingContext ctxt)
        {
            Debug.Assert(ctxt.Context != null && ctxt.Context is string, "No project directory given.");

            _singleton = this;
            _name = info.GetString("Name");
            ProjectDirectory = ctxt.Context.ToString();
            _actorTypes = (ActorTypesManager)info.GetValue("ActorTypes", typeof(ActorTypesManager));
            info.GetValue("ContentManager", typeof(ContentManager));
            _textures = (TexturesManager)info.GetValue("Textures", typeof(TexturesManager));
            _animations = (AnimationsManager)info.GetValue("Animations", typeof(AnimationsManager));
            _sounds = (SoundsManager)info.GetValue("Sounds", typeof(SoundsManager));
            _scenes = (ScenesManager)info.GetValue("Scenes", typeof(ScenesManager));
            _prototypes = (ObservableList<Actor>)info.GetValue("Prototypes", typeof(ObservableList<Actor>));
            _settings = (ProjectSettings)info.GetValue("Settings", typeof(ProjectSettings));
        }

        /// <summary>
        /// Creates the project at the specified directory with the specified name.
        /// </summary>
        /// <remarks>
        /// Closes the current project, creates the new project and sets it as the current project.
        /// </remarks>
        /// <param name="projectDirectory">The project directory.</param>
        /// <param name="projectName">Name of the project.</param>
        public static void CreateProject(string projectDirectory, string projectName)
        {
            // close already opened project
            if (Singleton != null)
            {
                Singleton.Dispose();
                _singleton = null;
            }

            _singleton = new Project(projectDirectory);
            _singleton._name = projectName;
        }

        /// <summary>
        /// Disposes all data that was loaded by XNA ContentManager.
        /// </summary>
        /// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise <c>false</c>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                XnaFramework.ContentManager.Unload();
            }
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("ActorTypes", ActorTypes);
            info.AddValue("ContentManager", new ContentManager());
            info.AddValue("Textures", Textures);
            info.AddValue("Animations", Animations);
            info.AddValue("Sounds", Sounds);
            info.AddValue("Scenes", Scenes);
            info.AddValue("Prototypes", Prototypes);
            info.AddValue("Settings", Settings);
        }

        /// <summary>
        /// Saves the project.
        /// </summary>
        /// <returns><c>true</c> if the project is successful saved; otherwise <c>false</c>.</returns>
        public bool Save()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Persistence));
                using (Stream stream = new FileStream(ProjectFilename, FileMode.Create))
                {
                    formatter.Serialize(stream, this);
                }

                return true;
            }
            catch (SerializationException)
            {
                Messages.ShowError("Unable to save project.");
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                if (e.InnerException != null && e.InnerException is SerializationException)
                {
                    Messages.ShowError("Unable to save project.");
                }
                else
                {
                    Messages.ShowError("Unable to save project. Error: " + e.Message);
                }
            }
            catch (System.Security.SecurityException)
            {
                Messages.ShowError("Unable to save project due to security problem.");
            }
            catch (Exception e)
            {
                Messages.ShowError("Unable to save project. Error: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Loades the project by the specified filename.
        /// </summary>
        /// <remarks>
        /// Closes the current project, loades the specified project and sets it as the current project.
        /// </remarks>
        /// <returns><c>true</c> if the project is successful loaded; otherwise <c>false</c>.</returns>
        public static bool Load(string filename)
        {
            // close already opened project
            if (Singleton != null)
            {
                Singleton.Dispose();
                _singleton = null;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Persistence, Path.GetDirectoryName(filename)));
                using (Stream stream = new FileStream(filename, FileMode.Open))
                {
                    formatter.Deserialize(stream, null);
                }
                return true;
            }
            catch (SerializationException)
            {
                Messages.ShowError("Unable to load project. File is not correct project file.");
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                if (e.InnerException != null && e.InnerException is SerializationException)
                {
                    Messages.ShowError("Unable to load project. File is not correct project file.");
                }
                else
                {
                    Messages.ShowError("Unable to load project. Error: " + e.InnerException != null ? e.InnerException.Message : e.Message);
                }
            }
            catch (System.Security.SecurityException)
            {
                Messages.ShowError("Unable to load project due to security problem.");
            }
            catch (Exception e)
            {
                Messages.ShowError("Unable to load project. Error: " + e.Message);
            }

            _singleton = null;
            return false;
        }
    }
}
