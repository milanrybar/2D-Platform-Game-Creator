/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlatformGameCreator.GameEngine
{
    /// <summary>
    /// Specifies a mouse button.
    /// </summary>
    public enum MouseButtons
    {
        /// <summary>
        /// Left mouse button.
        /// </summary>
        Left,

        /// <summary>
        /// Right mouse button.
        /// </summary>
        Right
    };

    /// <summary>
    /// Input manager for the game.
    /// </summary>
    public static class InputManager
    {
        // last state of the keyboard
        private static KeyboardState lastKeyboardState;
        // current state of the keyboard
        private static KeyboardState currentKeyboardState;

        // last state of the mouse
        private static MouseState lastMouseState;
        // current state of the mouse
        private static MouseState currentMouseState;

        /// <summary>
        /// Gets the mouse position in the display units.
        /// </summary>
        public static Vector2 MousePosition
        {
            get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
        }

        /// <summary>
        /// Gets the difference between the current and the last value of the mouse scroll wheel.
        /// </summary>
        public static int ScrollWheelValue
        {
            get { return currentMouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue; }
        }

        /// <summary>
        /// Initializes the <see cref="InputManager"/> class.
        /// </summary>
        static InputManager()
        {
            lastKeyboardState = currentKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Updates input manager. Updates keyboard and mouse states.
        /// </summary>
        public static void Update()
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Determines whether the specified key is down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns whether the specified key is down.</returns>
        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines whether the specified key is up.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns whether the specified key is up.</returns>
        public static bool IsKeyUp(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Determines whether the specified key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns whether the specified key is pressed.</returns>
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Determines whether the specified mouse button is down.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Returns whether the specified mouse button is down.</returns>
        public static bool IsMouseButtonDown(MouseButtons button)
        {
            if (button == MouseButtons.Left) return currentMouseState.LeftButton == ButtonState.Pressed;
            else return currentMouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Determines whether the specified mouse button is up.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Returns whether the specified mouse button is up.</returns>
        public static bool IsMouseButtonUp(MouseButtons button)
        {
            if (button == MouseButtons.Left) return currentMouseState.LeftButton == ButtonState.Released;
            else return currentMouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Determines whether the specified mouse button is pressed.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Returns whether the specified mouse button is pressed.</returns>
        public static bool IsMouseButtonPressed(MouseButtons button)
        {
            if (button == MouseButtons.Left) return currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
            else return currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;
        }
    }
}
