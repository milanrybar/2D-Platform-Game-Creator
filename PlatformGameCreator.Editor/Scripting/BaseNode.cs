/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using PlatformGameCreator.Editor.Common;
using System.Drawing;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Base class for a script node used at visual scripting.
    /// </summary>
    [Serializable]
    abstract class BaseNode : ISerializable
    {
        /// <summary>
        /// Gets the state where the script node is used.
        /// </summary>
        public State State
        {
            get { return _state; }
        }
        private State _state;

        /// <summary>
        /// Gets or sets the location of the script node.
        /// </summary>
        /// <remarks>
        /// Used as the position of the script node at the <see cref="ScriptingScreen"/>.
        /// </remarks>
        public PointF Location
        {
            get { return _location; }
            set
            {
                _location = value;
                if (LocationChanged != null) LocationChanged(this, EventArgs.Empty);
            }
        }
        private PointF _location;

        /// <summary>
        /// Gets or sets the comment for the script node.
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                if (CommentChanged != null) CommentChanged(this, EventArgs.Empty);
            }
        }
        private string _comment = String.Empty;

        /// <summary>
        /// Occurs when the <see cref="Location"/> property value changes.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Occurs when the <see cref="Comment"/> property value changes.
        /// </summary>
        public event EventHandler CommentChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNode"/> class.
        /// </summary>
        /// <param name="state">The state where the script node will be used.</param>
        protected BaseNode(State state)
        {
            _state = state;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected BaseNode(SerializationInfo info, StreamingContext ctxt)
        {
            _state = (State)info.GetValue("State", typeof(State));
            _location = (PointF)info.GetValue("Location", typeof(PointF));
            _comment = info.GetString("Comment");
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("State", State);
            info.AddValue("Location", Location);
            info.AddValue("Comment", Comment);
        }

        /// <summary>
        /// Removes the script node from the state.
        /// </summary>
        public virtual void Remove()
        {
            State.Nodes.Remove(this);
            _state = null;
        }

        /// <summary>
        /// Clones this script node.
        /// </summary>
        /// <returns>Cloned script node.</returns>
        public abstract BaseNode Clone();

        /// <summary>
        /// Clones this script node. The cloned script node will be used at the specified state.
        /// </summary>
        /// <param name="state">The state where the script node will be used.</param>
        /// <returns>Cloned script node.</returns>
        public abstract BaseNode Clone(State state);

        /// <summary>
        /// Creates the <see cref="SceneNode"/> representing this script node.
        /// </summary>
        /// <returns><see cref="SceneNode"/> representing this script node.</returns>
        public abstract SceneNode CreateView();
    }
}
