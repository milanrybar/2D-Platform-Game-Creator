/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Events
{
    /// <summary>
    /// Base class for event scripting node.
    /// </summary>
    public abstract class EventNode : ScriptNode
    {
        /// <summary>
        /// Create connection so event node is called automatically whenever it wants to.
        /// </summary>
        /// <remarks>
        /// After event node sets its all needed settings then calls <see cref="Connect"/> to create the connection for automatically calling event node functions whenever it wants to.
        /// </remarks>
        public abstract void Connect();
    }
}
