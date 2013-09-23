/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PlatformGameCreator.GameEngine.Screens
{
    /// <summary>
    /// Represent screen in the game.
    /// </summary>
    /// <remarks>
    /// Screen can be:
    /// <list type="bullet">
    /// <item><description>one screen of the menu or even complete menu</description></item>
    /// <item><description>one level of the game or even complete game</description></item>
    /// </list>
    /// It depends on the design of the application.
    /// </remarks>
    public abstract class Screen
    {
        /// <summary>
        /// Gets or sets the screen manager where the screen is used.
        /// </summary>
        public ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Gets the content manager of the screen.
        /// </summary>
        public ContentManager Content
        {
            get { return ScreenManager.Content; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screen"/> is active. Default value is true.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Called when the <see cref="Screen"/> needs to be updated. Override this method with screen-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Called when the <see cref="Screen"/> needs to be drawn. Override this method with screen-specific drawing code.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Called when the <see cref="Screen"/> should be initialized. 
        /// Called after the <see cref="Screen"/> is created, but before LoadContent.
        /// This method can be used for setting up non-graphics resources.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any screen-specific graphics resources.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any screen-specific graphics resources.
        /// </summary>
        public abstract void UnloadContent();

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        public Screen()
        {
            Active = true;
        }
    }
}
