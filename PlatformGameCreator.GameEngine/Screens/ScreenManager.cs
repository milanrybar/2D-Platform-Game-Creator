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

namespace PlatformGameCreator.GameEngine.Screens
{
    /// <summary>
    /// Handles the management of the screens in the game.
    /// </summary>
    /// <remarks>
    /// Screens are stored in the stack. The top of the stack is the active <see cref="Screen"/>.
    /// One screen is always active. If no screen is available during <see cref="Update"/> or <see cref="Draw"/> cycle then the game is exitted.
    /// </remarks>
    public class ScreenManager : DrawableGameComponent
    {
        /// <summary>
        /// Gets the global <see cref="SpriteBatch"/> which is available for all screens.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Gets the global <see cref="LineBatch"/> which is available for all screens.
        /// </summary>
        public LineBatch LineBatch
        {
            get
            {
                if (_lineBatch == null)
                {
                    RenderCircle.Initialize(GraphicsDevice, Content);
                    _lineBatch = new LineBatch(GraphicsDevice);
                }
                return _lineBatch;
            }
        }
        private LineBatch _lineBatch;

        /// <summary>
        /// Gets the default font.
        /// </summary>
        public SpriteFont DefaultFont
        {
            get { return _defaultFont; }
        }
        private SpriteFont _defaultFont;

        /// <summary>
        /// Gets the large font.
        /// </summary>
        public SpriteFont LargeFont
        {
            get { return _largeFont; }
        }
        private SpriteFont _largeFont;

        /// <summary>
        /// Gets the <see ContentManager=""/> used for loading managed objects from the binary files produced by the content pipeline
        /// </summary>
        public ContentManager Content
        {
            get { return Game.Content; }
        }

        /// <summary>
        /// Stack of screens. The top screen on the stack is the active screen.
        /// </summary>
        private Stack<Screen> screens = new Stack<Screen>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        /// <param name="game">The <see cref="PhysicsGame"/> that the <see cref="ScreenManager"/> should be attached to.</param>
        public ScreenManager(PhysicsGame game)
            : base(game)
        {

        }

        /// <summary>
        /// Called when graphics resources need to be loaded.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _defaultFont = Content.Load<SpriteFont>("DefaultFont");
            _largeFont = Content.Load<SpriteFont>("LargeFont");

            base.LoadContent();

            InputManager.Update();
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded. 
        /// </summary>
        protected override void UnloadContent()
        {
            Clear();

            base.UnloadContent();
        }

        /// <summary>
        /// Called when the <see cref="ScreenManager"/> needs to be updated.
        /// Updates <see cref="InputManager"/> and the active <see cref="Screen"/>.
        /// If the <see cref="ScreenManager"/> is empty then exits the game.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public override void Update(GameTime gameTime)
        {
            // update input manager
            InputManager.Update();

            while (screens.Count > 0 && !screens.Peek().Active)
            {
                PopScreen();
            }

            // no screen => exit program (game)
            if (screens.Count == 0)
            {
                Game.Exit();
                return;
            }

            screens.Peek().Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the <see cref="ScreenManager"/> needs to be drawn.
        /// Draws the active <see cref="Screen"/>.
        /// If the <see cref="ScreenManager"/> is empty then exits the game.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            // no screen => exit program (game)
            if (screens.Count == 0)
            {
                Game.Exit();
                return;
            }

            screens.Peek().Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Add the specified screen to the to the top of the stack.
        /// </summary>
        /// <param name="screen">Screen to add.</param>
        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;

            screen.Initialize();
            screen.LoadContent();

            screens.Push(screen);
        }

        /// <summary>
        /// Removes the screen at the top of the stack.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="ScreenManager"/> is empty.</exception>
        public void PopScreen()
        {
            Screen screen = screens.Pop();

            screen.UnloadContent();
        }

        /// <summary>
        /// Removes all screens from the <see cref="ScreenManager"/>.
        /// </summary>
        public void Clear()
        {
            foreach (Screen screen in screens)
            {
                screen.UnloadContent();
            }

            screens.Clear();
        }
    }
}
