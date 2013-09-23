/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Common;
using System.Runtime.Serialization;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents an event used at visual scripting.
    /// </summary>
    /// <remarks>
    /// Used as Event In or Event Out.
    /// </remarks>
    [Serializable]
    class Event : IName, IIndex, ISerializable
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (NameChanged != null) NameChanged(this, EventArgs.Empty);
            }
        }
        private string _name;

        /// <summary>
        /// Gets or sets the index of the event at the container where is stored.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Occurs when the <see cref="Name"/> property value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        private Event(SerializationInfo info, StreamingContext ctxt)
        {
            _name = info.GetString("Name");
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the name of the event.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
