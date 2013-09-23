/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scenes
{
    /// <summary>
    /// Represents text which is drawn at the scene for the specified duration.
    /// </summary>
    public class TextEffect : SceneNode
    {
        /// <summary>
        /// Text to draw at the scene.
        /// </summary>
        public string Text;

        /// <summary>
        /// Position of the text to draw at the scene.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Color of the text to draw at the scene.
        /// </summary>
        public Color Color;

        /// <summary>
        /// Duration in seconds for how long the text is drawn at the scene.
        /// </summary>
        public double Duration;

        /// <summary>
        /// Indicates whether the default font or large font is used for the text.
        /// </summary>
        public bool DefaultFont = true;

        /// <summary>
        /// Elapsed time from the start of drawing the text (creating this instance) at the scene.
        /// </summary>
        private double elapsedTime = 0.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEffect"/> class.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position of the text to draw.</param>
        /// <param name="color">The color of the text to draw.</param>
        public TextEffect(string text, Vector2 position, Color color)
        {
            Text = text;
            Position = position;
            Color = color;

            // front layer (like HUD)
            Layer = 0;
            Active = true;
        }

        /// <inheritdoc />
        /// <summary>
        /// When the elapsed time of drawing the text passed the <see cref="Duration"/> then the <see cref="TextEffect"/> is disabled.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > Duration)
            {
                // at least one frame drawable
                if (Duration == 0.0) Duration = -1;
                // disable
                else Active = false;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Whens the specified text at the scene.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Screen.ScreenManager.SpriteBatch.DrawString(DefaultFont ? Screen.ScreenManager.DefaultFont : Screen.ScreenManager.LargeFont, Text, ConvertUnits.ToDisplayUnits(Position), Color);
        }

        /// <summary>
        /// Called when the <see cref="TextEffect"/> should be initialized.
        /// </summary>
        public override void Initialize()
        {
        }
    }
}
