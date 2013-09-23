/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents a scripting node used at visual scripting.
    /// </summary>
    /// <remarks>
    /// Used for action and event scripting node.
    /// </remarks>
    [Serializable]
    class Node : BaseNode, ISerializable
    {
        /// <summary>
        /// Gets the underlying node data.
        /// </summary>
        public NodeData NodeData
        {
            get { return _nodeData; }
        }
        private NodeData _nodeData;

        /// <summary>
        /// Gets sockets of the script node.
        /// </summary>
        public NodeSocket[] Sockets
        {
            get { return _sockets; }
        }
        private NodeSocket[] _sockets;

        /// <summary>
        /// Gets the name of the script node.
        /// </summary>
        public string Name
        {
            get { return NodeData.Name; }
        }

        /// <summary>
        /// Gets the description of the script node.
        /// </summary>
        public string Description
        {
            get { return NodeData.Description; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="state">The state where the script node will be used.</param>
        /// <param name="nodeData">The node data of the script node.</param>
        public Node(State state, NodeData nodeData)
            : base(state)
        {
            _nodeData = nodeData;

            CreateSockets();
        }

        /// <inheritdoc />
        protected Node(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _nodeData = ScriptingNodes.FindNode(info.GetString("NodeDataRealName"), (NodeType)info.GetValue("NodeDataType", typeof(NodeType)));
            _sockets = (NodeSocket[])info.GetValue("Sockets", typeof(NodeSocket[]));

            Debug.Assert(_nodeData != null, "Cannot find correct node data.");
        }

        /// <summary>
        /// Creates the sockets defined at the <see cref="NodeData"/>.
        /// </summary>
        protected void CreateSockets()
        {
            _sockets = new NodeSocket[NodeData.Sockets.Count];

            for (int i = 0; i < NodeData.Sockets.Count; ++i)
            {
                if (NodeData.Sockets[i].Type == NodeSocketType.SignalIn || NodeData.Sockets[i].Type == NodeSocketType.SignalOut)
                {
                    _sockets[i] = new SignalNodeSocket(this, NodeData.Sockets[i]);
                }
                else
                {
                    _sockets[i] = new VariableNodeSocket(this, NodeData.Sockets[i]);
                }
            }
        }

        /// <summary>
        /// Clones the sockets. The cloned sockets will be used at the specified node.
        /// </summary>
        /// <param name="clonedNode">The node where to store the cloned sockets.</param>
        protected void CloneSockets(Node clonedNode)
        {
            for (int i = 0; i < Sockets.Length; ++i)
            {
                clonedNode._sockets[i] = (NodeSocket)Sockets[i].Clone(clonedNode);
            }
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("NodeDataRealName", NodeData.RealName);
            info.AddValue("NodeDataType", NodeData.Type);
            info.AddValue("Sockets", Sockets);
        }

        /// <summary>
        /// Creates the view of the sockets that will be used the specified node view.
        /// </summary>
        /// <param name="nodeView">The node view where to use view of sockets.</param>
        protected void CreateSocketsInView(NodeView nodeView)
        {
            for (int i = 0; i < Sockets.Length; ++i)
            {
                nodeView.AddSocket(Sockets[i].CreateView(nodeView));
            }
        }

        /// <summary>
        /// Removes the script node from the state. Also removes all its connections.
        /// </summary>
        public override void Remove()
        {
            // remove all connections
            foreach (BaseNode baseNode in State.Nodes)
            {
                Node node = baseNode as Node;
                if (node != null)
                {
                    foreach (NodeSocket nodeSocket in node.Sockets)
                    {
                        SignalNodeSocket signalNodeSocket = nodeSocket as SignalNodeSocket;
                        if (signalNodeSocket != null)
                        {
                            for (int i = 0; i < signalNodeSocket.Connections.Count; ++i)
                            {
                                if (signalNodeSocket.Connections[i].Node == this)
                                {
                                    signalNodeSocket.Connections.RemoveAt(i);
                                    --i;
                                }
                            }
                        }
                    }
                }
            }

            base.Remove();
        }

        /// <inheritdoc />
        public override BaseNode Clone()
        {
            return Clone(State);
        }

        /// <inheritdoc />
        public override BaseNode Clone(State state)
        {
            Node clonedNode = new Node(state, NodeData) { Comment = Comment, Location = Location };
            CloneSockets(clonedNode);
            return clonedNode;
        }

        /// <inheritdoc />
        public override SceneNode CreateView()
        {
            NodeView nodeView = new NodeView(this);

            CreateSocketsInView(nodeView);

            return nodeView;
        }
    }
}
