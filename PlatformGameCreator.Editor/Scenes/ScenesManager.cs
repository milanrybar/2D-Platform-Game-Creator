/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Manager for <see cref="Scene">scenes</see> used in the project.
    /// </summary>
    [Serializable]
    class ScenesManager : ObservableIndexedList<Scene>, ISerializable
    {
        /// <summary>
        /// Gets the last used unique id for the game object.
        /// </summary>
        public static int LastUniqueGameObjectId
        {
            get { return _lastUniqueGameObjectId; }
        }
        private static int _lastUniqueGameObjectId = 0;

        /// <summary>
        /// Gets the unique id for the new game object.
        /// </summary>
        /// <returns>Returns the unique id for the game object.</returns>
        public static int GetUniqueGameObjectId()
        {
            return ++_lastUniqueGameObjectId;
        }

        /// <summary>
        /// Gets or sets the selected scene of the project.
        /// </summary>
        public Scene SelectedScene
        {
            get { return _selectedScene; }
            set
            {
                if (_selectedScene != value)
                {
                    Scene oldValue = _selectedScene;
                    _selectedScene = value;
                    if (SelectedSceneChanged != null) SelectedSceneChanged(this, new ValueChangedEventArgs<Scene>(oldValue));
                }
            }
        }
        private Scene _selectedScene;

        /// <summary>
        /// Occurs when the <see cref="SelectedScene"/> property value changes.
        /// </summary>
        public event ValueChangedHandler<Scene> SelectedSceneChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenesManager"/> class.
        /// </summary>
        public ScenesManager()
        {

        }

        /// <inheritdoc />
        private ScenesManager(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _lastUniqueGameObjectId = info.GetInt32("LastUniqueId");
            _selectedScene = (Scene)info.GetValue("SelectedScene", typeof(Scene));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LastUniqueId", _lastUniqueGameObjectId);
            info.AddValue("SelectedScene", SelectedScene);

            base.GetObjectData(info, context);
        }
    }
}
