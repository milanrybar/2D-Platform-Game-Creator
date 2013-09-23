/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Base class for a script node socket used at <see cref="Node"/>.
    /// </summary>
    [Serializable]
    abstract class NodeSocket : ISerializable
    {
        /// <summary>
        /// Gets the node where the script node socket is used.
        /// </summary>
        public Node Node
        {
            get { return _node; }
        }
        private Node _node;

        /// <summary>
        /// Gets the underlying node socket data. 
        /// </summary>
        public NodeSocketData NodeSocketData
        {
            get { return _nodeSocketData; }
        }
        private NodeSocketData _nodeSocketData;

        /// <summary>
        /// Gets the type of the script node socket.
        /// </summary>
        public NodeSocketType Type
        {
            get { return NodeSocketData.Type; }
        }

        /// <summary>
        /// Gets the name of the script node socket.
        /// </summary>
        public string Name
        {
            get { return NodeSocketData.Name; }
        }

        /// <summary>
        /// Gets the description of the script node socket.
        /// </summary>
        public string Description
        {
            get { return NodeSocketData.Description; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeSocket"/> class.
        /// </summary>
        /// <param name="node">The node where the script node socket will be used.</param>
        /// <param name="nodeSocketData">The node socket data of the script node socket.</param>
        protected NodeSocket(Node node, NodeSocketData nodeSocketData)
        {
            _nodeSocketData = nodeSocketData;
            _node = node;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected NodeSocket(SerializationInfo info, StreamingContext ctxt)
        {
            _node = (Node)info.GetValue("Node", typeof(Node));

            string nodeSocketDataRealName = info.GetString("NodeSocketDataRealName");
            NodeSocketType nodeSocketDataType = (NodeSocketType)info.GetValue("NodeSocketDataType", typeof(NodeSocketType));

            foreach (NodeSocketData nodeSocketData in Node.NodeData.Sockets)
            {
                if (nodeSocketData.RealName == nodeSocketDataRealName && nodeSocketData.Type == nodeSocketDataType)
                {
                    _nodeSocketData = nodeSocketData;
                    break;
                }
            }

            Debug.Assert(_nodeSocketData != null, "Cannot find correct node socket data.");
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Node", Node);
            info.AddValue("NodeSocketDataRealName", NodeSocketData.RealName);
            info.AddValue("NodeSocketDataType", NodeSocketData.Type);
        }

        /// <summary>
        /// Clones this script node socket. The cloned script node socket will be used at the specified node.
        /// </summary>
        /// <param name="node">The node where the script node will be used.</param>
        /// <returns>Cloned script node socket.</returns>
        public abstract NodeSocket Clone(Node node);

        /// <summary>
        /// Creates the <see cref="NodeSocketView"/> representing this script node socket that will be used the specified node view.
        /// </summary>
        /// <param name="nodeView">The node view where to use view of script node socket.</param>
        /// <returns><see cref="NodeSocketView"/> representing this script node socket.</returns>
        public abstract NodeSocketView CreateView(NodeView nodeView);
    }

    /// <summary>
    /// Represents a script signal node socket.
    /// </summary>
    [Serializable]
    class SignalNodeSocket : NodeSocket, ISerializable
    {
        /// <summary>
        /// Gets connections of the script signal node socket.
        /// </summary>
        public List<SignalNodeSocket> Connections
        {
            get { return _connections; }
        }
        private List<SignalNodeSocket> _connections;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalNodeSocket"/> class.
        /// </summary>
        /// <param name="node">The node where the script node socket will be used.</param>
        /// <param name="nodeSocketData">The node socket data of the script node socket.</param>
        public SignalNodeSocket(Node node, NodeSocketData nodeSocketData)
            : base(node, nodeSocketData)
        {
            _connections = new List<SignalNodeSocket>();
        }

        /// <inheritdoc />
        protected SignalNodeSocket(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _connections = (List<SignalNodeSocket>)info.GetValue("Connections", typeof(List<SignalNodeSocket>));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Connections", Connections);
        }

        /// <inheritdoc />
        public override NodeSocket Clone(Node node)
        {
            return new SignalNodeSocket(node, NodeSocketData);
        }

        /// <inheritdoc />
        public override NodeSocketView CreateView(NodeView nodeView)
        {
            if (Type == NodeSocketType.SignalIn) return new SignalInNodeSocketView(nodeView, this);
            else return new SignalOutNodeSocketView(nodeView, this);
        }
    }

    /// <summary>
    /// Represents a script variable node socket.
    /// </summary>
    [Serializable]
    class VariableNodeSocket : NodeSocket, IVisible, ISerializable
    {
        /// <summary>
        /// Gets the type of the script variable node socket.
        /// </summary>
        public VariableType VariableType
        {
            get { return NodeSocketData.VariableType; }
        }

        /// <summary>
        /// Gets connections of the script variable node socket.
        /// </summary>
        public List<Variable> Connections
        {
            get { return _connections; }
        }
        private List<Variable> _connections;

        /// <summary>
        /// Gets or sets the value of the script variable node socket.
        /// It will be used when no connection is made.
        /// </summary>
        public IVariable Value
        {
            get { return _value; }
        }
        protected IVariable _value;

        /// <summary>
        /// Gets or sets a value indicating whether the script variable node socket is visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (VisibleChanged != null) VisibleChanged(this, EventArgs.Empty);
            }
        }
        private bool _visible = true;

        /// <summary>
        /// Occurs when the <see cref="Visible"/> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNodeSocket"/> class.
        /// </summary>
        /// <param name="node">The node where the script node socket will be used.</param>
        /// <param name="nodeSocketData">The node socket data of the script node socket.</param>
        public VariableNodeSocket(Node node, NodeSocketData nodeSocketData)
            : base(node, nodeSocketData)
        {
            _value = VarFactory.Create(nodeSocketData.VariableType);
            _visible = nodeSocketData.Visible;
            _connections = new List<Variable>();

            // set default value
            if (NodeSocketData.DefaultValue != null) Value.SetValue(NodeSocketData.DefaultValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNodeSocket"/> class.
        /// </summary>
        /// <param name="node">The node where the script node socket will be used.</param>
        /// <param name="nodeSocketData">The node socket data of the script node socket.</param>
        /// <param name="value">Default value of the script variable node socket.</param>
        public VariableNodeSocket(Node node, NodeSocketData nodeSocketData, IVariable value)
            : base(node, nodeSocketData)
        {
            _value = value;
            _visible = nodeSocketData.Visible;
            _connections = new List<Variable>();
        }

        /// <inheritdoc />
        protected VariableNodeSocket(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _value = (IVariable)info.GetValue("Value", typeof(IVariable));
            _connections = (List<Variable>)info.GetValue("Connections", typeof(List<Variable>));
            _visible = info.GetBoolean("Visible");
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Value", Value);
            info.AddValue("Connections", Connections);
            info.AddValue("Visible", Visible);
        }

        /// <inheritdoc />
        public override NodeSocket Clone(Node node)
        {
            return new VariableNodeSocket(node, NodeSocketData, Value.Clone()) { Visible = Visible };
        }

        /// <inheritdoc />
        public override NodeSocketView CreateView(NodeView nodeView)
        {
            return new VariableNodeSocketView(nodeView, this);
        }
    }
}
