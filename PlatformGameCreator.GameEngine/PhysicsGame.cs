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
using Microsoft.Xna.Framework.Graphics;
using PlatformGameCreator.GameEngine.Screens;

namespace PlatformGameCreator.GameEngine
{
    /// <summary>
    /// Base class for the game created by the editor.
    /// </summary>
    public class PhysicsGame : Game
    {
        /// <summary>
        /// Gets the width of the game window.
        /// </summary>
        public int WindowWidth
        {
            get { return graphics.PreferredBackBufferWidth; }
        }

        /// <summary>
        /// Gets the height of the game window.
        /// </summary>
        public int WindowHeight
        {
            get { return graphics.PreferredBackBufferHeight; }
        }

        /// <summary>
        /// Handles the configuration and management of the graphics device.
        /// </summary>
        protected GraphicsDeviceManager graphics;

        /// <summary>
        /// Handles the management of the screens in the game.
        /// </summary>
        protected ScreenManager screenManager;

        /// <summary>
        /// Gets the FPS of the game.
        /// </summary>
        public int Fps
        {
            get { return frameRate; }
        }

        private double elapsedTime;
        private int frameCounter;
        private int frameRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicsGame"/> class.
        /// </summary>
        public PhysicsGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            graphics.GraphicsProfile = GraphicsProfile.Reach;
        }

        /// <summary>
        /// Called when the <see cref="PhysicsGame"/> needs to be updated.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime > 1000.0)
            {
                elapsedTime -= 1000.0;
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        /// <summary>
        /// Called when the <see cref="PhysicsGame"/> needs to be drawn.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ++frameCounter;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}