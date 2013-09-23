/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.Editor.Common
{
    /// <summary>
    /// Contains a name of this object and event when the name is changed.
    /// </summary>
    interface IName
    {
        /// <summary>
        /// Gets or sets the name of this object.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        event EventHandler NameChanged;
    }

    /// <summary>
    /// Contains a value whether this object is visible and event when the value is changed.
    /// </summary>
    interface IVisible
    {
        /// <summary>
        /// Gets or sets a value indicating whether this object is visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property value changes.
        /// </summary>
        event EventHandler VisibleChanged;
    }

    /// <summary>
    /// Contains an index of this object. Meaning is defined by the implementation.
    /// </summary>
    interface IIndex
    {
        /// <summary>
        /// Gets or sets the index of this object.
        /// </summary>
        int Index { get; set; }
    }

    /// <summary>
    /// Provides data when the value changes.
    /// </summary>
    /// <typeparam name="ValueType">The type of the value.</typeparam>
    class ValueChangedEventArgs<ValueType> : EventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public ValueType OldValue
        {
            get { return _oldValue; }
        }
        private ValueType _oldValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs&lt;ValueType&gt;"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        public ValueChangedEventArgs(ValueType oldValue)
        {
            _oldValue = oldValue;
        }
    }

    /// <summary>
    /// Occurs when the value changes.
    /// </summary>
    /// <typeparam name="ValueType">The type of the value.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="PlatformGameCreator.Editor.Common.ValueChangedEventArgs&lt;ValueType&gt;"/> instance containing the event data.</param>
    delegate void ValueChangedHandler<ValueType>(object sender, ValueChangedEventArgs<ValueType> e);
}
