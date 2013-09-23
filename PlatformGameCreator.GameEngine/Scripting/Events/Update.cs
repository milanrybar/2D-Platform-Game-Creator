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
    /// Fires when the state is updating (fires every game update cycle).
    /// </summary>
    [FriendlyName("Update")]
    [Description("Fires when the state is updating (fires every game update cycle).")]
    [Category("Events")]
    public class UpdateEvent : EventNode
    {
        /// <summary>
        /// Fires when the state is updating (fires every game update cycle).
        /// </summary>
        [FriendlyName("On Update")]
        [Description("Fires when the state is updating (fires every game update cycle).")]
        public ScriptSocketHandler OnUpdate;

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
        /// Fires <see cref="OnUpdate"/>.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (OnUpdate != null) OnUpdate();
        }
    }
}
