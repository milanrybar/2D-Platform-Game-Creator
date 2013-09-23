/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting
{
    /// <summary>
    /// Callback for calling the scripting nodes methods.
    /// </summary>
    /// <remarks>
    /// Scripting nodes function that can be called from the outside (and are visible in editor) have this header. No parameters and returns void.
    /// </remarks>
    public delegate void ScriptSocketHandler();

    /// <summary>
    /// Base class for scripting node.
    /// </summary>
    /// <remarks>
    /// Scripting node can be used only in a <see cref="State"/> of a state machine of an <see cref="Scenes.Actor"/>.
    /// Scripting node is processed only when the <see cref="State"/> (its <see cref="Container"/>) is the active state of its state machine.
    /// </remarks>
    public abstract class ScriptNode
    {
        /// <summary>
        /// Gets or sets the container of the scripting node.
        /// </summary>
        public State Container { get; set; }

        /// <summary>
        /// Sets <paramref name="value"/> to all variables of <paramref name="outputVariable"/>.
        /// </summary>
        /// <remarks>
        /// Helper function for settings output variables in the action scripting nodes.
        /// </remarks>
        /// <typeparam name="T">Type of the variable to set.</typeparam>
        /// <param name="value">Value to set to output variable.</param>
        /// <param name="outputVariable">Output variable to set.</param>
        protected static void SetOutputVariable<T>(T value, Variable<T>[] outputVariable)
        {
            if (outputVariable != null)
            {
                for (int i = 0; i < outputVariable.Length; ++i)
                {
                    outputVariable[i].Value = value;
                }
            }
        }

        /// <summary>
        /// Scripting node can need to run more then one game cycle.
        /// The derivatived class uses this function for updating scripting nodes as long as it needs.
        /// </summary>
        /// <example>
        /// Scripting node can be updated by connecting to <see cref="State.OnUpdate"/> of <see cref="Container"/>:
        /// <code>
        /// Container.OnUpdate += Update;
        /// </code>
        /// Now the scripting node is updated whenever <see cref="Container"/> is the active state of its state machine.
        /// </example>
        /// <param name="gameTime">Time passed since the last game update cycle.</param>
        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
