/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Settings of a project.
    /// </summary>
    [Serializable]
    class ProjectSettings : IDeserializationCallback
    {
        /// <summary>
        /// Width of the game window. 
        /// </summary>
        /// <remarks>
        /// Default value is 1280.
        /// </remarks>
        public int GameWindowWidth;

        /// <summary>
        /// Height of the game window. 
        /// </summary>
        /// <remarks>
        /// Default value is 720.
        /// </remarks>
        public int GameWindowHeight;

        /// <summary>
        /// Indicates whether the game window will run in the full-screen mode.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>.
        /// </remarks>
        public bool GameIsFullScreen;

        /// <summary>
        /// Gets or sets the ratio for converting display to simulation units.
        /// </summary>
        /// <remarks>
        /// Represents how many pixels is one meter in the simulation units.
        /// Default value is 100.
        /// </remarks>
        public float SimulationUnits
        {
            get { return _simulationUnits; }
            set
            {
                _simulationUnits = value;
                GameEngine.ConvertUnits.SetDisplayUnitToSimUnitRatio(_simulationUnits);
            }
        }
        private float _simulationUnits;

        /// <summary>
        /// Default gravity for the game
        /// </summary>
        /// <remarks>
        /// Default value is (0, 9.8f).
        /// </remarks>
        public Vector2 DefaultGravity;

        /// <summary>
        /// Color for the background of the game.
        /// </summary>
        /// <remarks>
        /// Default value is color with the value R:100 G:149 B:237 A:255.
        /// </remarks>
        public Color BackgroundColor;

        /// <summary>
        /// Indicates whether the physics simulation has enabled this settings.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c>.
        /// </remarks>
        public bool ContinuousCollisionDetection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSettings"/> class.
        /// </summary>
        public ProjectSettings()
        {
            GameWindowWidth = 1280;
            GameWindowHeight = 720;
            GameIsFullScreen = false;

            SimulationUnits = 100f;
            DefaultGravity = new Vector2(0, 9.8f);
            BackgroundColor = Color.CornflowerBlue;
            ContinuousCollisionDetection = true;
        }

        /// <inheritdoc />
        public void OnDeserialization(object sender)
        {
            SimulationUnits = _simulationUnits;
        }
    }
}
