/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting
{
    /// <summary>
    /// Base wrapper class for storing a variable value.
    /// </summary>
    public abstract class VariableWrapper
    {
        /// <summary>
        /// Occurs when the value of the variable changes.
        /// </summary>
        public ScriptSocketHandler VariableChanged;
    }

    /// <summary>
    /// Wrapper class for storing a variable value.
    /// </summary>
    /// <remarks>
    /// Wrapper class is used for storing variables in the scripting nodes and named variables in the <see cref="Scenes.Actor"/>.
    /// </remarks>
    /// <typeparam name="T">Type of the variable.</typeparam>
    public class Variable<T> : VariableWrapper
    {
        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        public T Value
        {
            get
            {
                if (Parent == null) return _value;
                else return Parent.Value;
            }
            set
            {
                _value = value;
                if (Parent != null) Parent.Value = value;
                if (VariableChanged != null) VariableChanged();
            }
        }
        private T _value;

        /// <summary>
        /// Gets or sets the parent variable. If value is not null Parent represents current variable.
        /// </summary>
        public Variable<T> Parent { get; set; }

        /// <summary>
        /// Parses the specified value to this variable type and creates variable with value of the specified value.
        /// </summary>
        /// <param name="value">The value to use in created variable.</param>
        /// <returns>Variable with specified value or null when specified value is not valid variable type.</returns>
        public static Variable<T> Parse(object value)
        {
            try
            {
                T parse = (T)value;
                return new Variable<T>() { Value = parse };
            }
            catch (InvalidCastException)
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable&lt;T&gt;"/> class.
        /// </summary>
        public Variable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">Value of the variable.</param>
        public Variable(T value)
        {
            _value = value;
        }
    }

    /// <summary>
    /// Wrapper class for storing an event.
    /// </summary>
    /// <remarks>
    /// Wrapper class is used for storing defined events in the <see cref="Scenes.Actor"/>.
    /// </remarks>
    public class EventWrapper
    {
        /// <summary>
        /// Event stored in the wrapper class.
        /// </summary>
        public ScriptSocketHandler Event;

        /// <summary>
        /// Invokes the event.
        /// </summary>
        public void Invoke()
        {
            if (Event != null) Event();
        }
    }
}
