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

namespace PlatformGameCreator.GameEngine.Scripting.Events
{
    /// <summary>
    /// Fires when the specified key is down/pressed/up.
    /// </summary>
    [FriendlyName("Input Events")]
    [Description("Fires when the specified key is down/pressed/up.")]
    [Category("Events")]
    public class InputEvents : EventNode
    {
        /// <summary>
        /// Fires when the specified key is down.
        /// </summary>
        [FriendlyName("On Input Down")]
        [Description("Fires when the specified key is down.")]
        public ScriptSocketHandler Down;

        /// <summary>
        /// Fires when the specified key is pressed.
        /// </summary>
        [FriendlyName("On Input Pressed")]
        [Description("Fires when the specified key is pressed.")]
        public ScriptSocketHandler Pressed;

        /// <summary>
        /// Fires when the specified key is up.
        /// </summary>
        [FriendlyName("On Input Up")]
        [Description("Fires when the specified key is up.")]
        public ScriptSocketHandler Up;

        /// <summary>
        /// Key to check.
        /// </summary>
        [Description("Key to check.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Keys> Key;

        /// <inheritdoc />
        /// <remarks>
        /// Connects to the <see cref="State"/> <see cref="State.OnUpdate"/> event.
        /// </remarks>
        public override void Connect()
        {
            Container.OnUpdate += Update;
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if the specified key is down/pressed/up.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Key.Value))
            {
                if (Down != null) Down();
            }

            if (InputManager.IsKeyPressed(Key.Value))
            {
                if (Pressed != null) Pressed();
            }

            if (InputManager.IsKeyUp(Key.Value))
            {
                if (Up != null) Up();
            }
        }
    }
}
