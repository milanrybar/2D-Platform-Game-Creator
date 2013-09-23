/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Events
{
    /// <summary>
    /// Fires when the specified mouse button is down/pressed/up.
    /// </summary>
    [FriendlyName("Mouse Events")]
    [Description("Fires when the specified mouse button is down/pressed/up.")]
    [Category("Events")]
    public class MouseEvents : EventNode
    {
        /// <summary>
        /// Fires when the specified mouse button is down.
        /// </summary>
        [FriendlyName("On Mouse Down")]
        [Description("Fires when the specified mouse button is down.")]
        public ScriptSocketHandler Down;

        /// <summary>
        /// Fires when the specified mouse button is pressed.
        /// </summary>
        [FriendlyName("On Mouse Pressed")]
        [Description("Fires when the specified mouse button is pressed.")]
        public ScriptSocketHandler Pressed;

        /// <summary>
        /// Fires when the specified mouse button is up.
        /// </summary>
        [FriendlyName("On Mouse Up")]
        [Description("Fires when the specified mouse button is up.")]
        public ScriptSocketHandler Up;

        /// <summary>
        /// Indicates whether left or right mouse button is checked.
        /// </summary>
        [FriendlyName("Left Button")]
        [Description("Indicates whether left or right mouse button is checked.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(true)]
        public Variable<bool> LeftButton;

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
        /// Checks if the specified mouse button is down/pressed/up.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            MouseButtons mouseButton = LeftButton.Value ? MouseButtons.Left : MouseButtons.Right;

            if (InputManager.IsMouseButtonDown(mouseButton))
            {
                if (Down != null) Down();
            }

            if (InputManager.IsMouseButtonPressed(mouseButton))
            {
                if (Pressed != null) Pressed();
            }

            if (InputManager.IsMouseButtonUp(mouseButton))
            {
                if (Up != null) Up();
            }
        }
    }
}
