/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;
using System.ComponentModel;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents <see cref="Node"/> at the scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    class NodeView : SceneNode, INodeSocketsContainer, IEditSettings
    {
        /// <summary>
        /// Gets the underlying script node.
        /// </summary>
        public Node Node
        {
            get { return _node; }
        }
        private Node _node;

        /// <summary>
        /// Gets or sets the name of the script node.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                nameSize = TextRenderer.MeasureText(_name, DrawingTools.BoldFont);
                UpdateGui();
            }
        }
        private string _name = "Title";

        /// <summary>
        /// Gets or sets the location of the node at the scene.
        /// </summary>
        public override PointF Location
        {
            get { return Node.Location; }
            set
            {
                Node.Location = value;
                UpdateGui();
            }
        }

        /// <summary>
        /// Gets the size and location of the node without all its sockets.
        /// </summary>
        public override RectangleF Bounds
        {
            get { return _bounds; }
        }
        private RectangleF _bounds;

        /// <summary>
        /// Gets the size and location of the node including all its sockets.
        /// </summary>
        public RectangleF NodeBounds
        {
            get { return _nodeBounds; }
        }
        private RectangleF _nodeBounds;

        /// <inheritdoc />
        public override bool CanMove
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanConnect
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool CanClone
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool IsSocketsContainer
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanEditSettings
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override INodeSocketsContainer INodeSocketsContainer
        {
            get { return this; }
        }

        /// <inheritdoc />
        public override IConnecting IConnecting
        {
            get { return null; }
        }

        /// <inheritdoc />
        public override IEditSettings IEditSettings
        {
            get { return this; }
        }

        // size of the name text
        private Size nameSize;
        // location of the name text
        private PointF nameLocation;
        // rectangle of the name area of the node
        private RectangleF nameRectangle;
        // location in pixels of the comment text
        private PointF commentLocation;

        // in sockets
        private List<NodeSocketView> inSockets = new List<NodeSocketView>();
        // out sockets
        private List<NodeSocketView> outSockets = new List<NodeSocketView>();
        // variable sockets
        private List<NodeSocketView> variableSockets = new List<NodeSocketView>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeView"/> class.
        /// </summary>
        /// <param name="node">The script node.</param>
        public NodeView(Node node)
        {
            _node = node;

            Name = Node.Name;

            Node.CommentChanged += new EventHandler(Node_CommentChanged);
        }


        /// <summary>
        /// Handles the CommentChanged event of the Node.
        /// Invalidates the node.
        /// </summary>
        private void Node_CommentChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Adds the specified signal in node socket.
        /// </summary>
        /// <param name="signalNodeSocket">The signal in node socket.</param>
        /// <exception cref="ArgumentException">Argument is not signal in node socket.</exception>
        public void AddInSocket(SignalNodeSocket signalNodeSocket)
        {
            if (signalNodeSocket.Type != NodeSocketType.SignalIn) throw new ArgumentException("Argument is not signal in node socket.");

            inSockets.Add(new SignalInNodeSocketView(this, signalNodeSocket));

            UpdateGui();
        }

        /// <summary>
        /// Adds the specified signal out node socket.
        /// </summary>
        /// <param name="signalNodeSocket">The signal out node socket.</param>
        /// <exception cref="ArgumentException">Argument is not signal out node socket.</exception>
        public void AddOutSocket(SignalNodeSocket signalNodeSocket)
        {
            if (signalNodeSocket.Type != NodeSocketType.SignalOut) throw new ArgumentException("Argument is not signal out node socket.");

            outSockets.Add(new SignalOutNodeSocketView(this, signalNodeSocket));

            UpdateGui();
        }

        /// <summary>
        /// Adds the specified variable node socket.
        /// </summary>
        /// <param name="variableNodeSocket">The variable node socket.</param>
        /// <exception cref="ArgumentException">Argument is not variable node socket.</exception>
        public void AddVariableSocket(VariableNodeSocket variableNodeSocket)
        {
            if (variableNodeSocket.Type != NodeSocketType.VariableIn && variableNodeSocket.Type != NodeSocketType.VariableOut) throw new ArgumentException("Argument is not variable node socket.");

            variableSockets.Add(new VariableNodeSocketView(this, variableNodeSocket));

            UpdateGui();
        }

        /// <summary>
        /// Adds the specified view of the node socket.
        /// </summary>
        /// <param name="nodeSocketView">The view of the node socket.</param>
        public void AddSocket(NodeSocketView nodeSocketView)
        {
            if (nodeSocketView.Type == NodeSocketType.SignalIn) inSockets.Add(nodeSocketView);
            else if (nodeSocketView.Type == NodeSocketType.SignalOut) outSockets.Add(nodeSocketView);
            else variableSockets.Add(nodeSocketView);

            UpdateGui();
        }

        /// <summary>
        /// Called when the scene node is added to the <see cref="ScriptingScreen"/>.
        /// The underlying node is added to the state.
        /// </summary>
        public override void OnSceneNodeAdded()
        {
            Debug.Assert(!SceneControl.State.Nodes.Contains(Node), "Scripting component already contains this node.");

            SceneControl.State.Nodes.Add(Node);
        }

        /// <summary>
        /// Called when the scene node is removed from the <see cref="ScriptingScreen"/>.
        /// The underlying node is removed from the state.
        /// </summary>
        public override void OnSceneNodeRemoved()
        {
            Debug.Assert(SceneControl.State.Nodes.Contains(Node), "Scripting component does not contain this node.");

            SceneControl.State.Nodes.Remove(Node);
        }

        /// <summary>
        /// Updates any necessary variables for GUI.
        /// </summary>
        private void UpdateGui()
        {
            Padding NamePadding = SizeSettings.NodeTitlePadding;
            Padding SocketPadding = SizeSettings.NodeSocketPadding;
            Padding BodyPadding = SizeSettings.NodeBodyPadding;
            int SocketSize = SizeSettings.NodeSocketSize;

            // point where body starts for in sockets
            PointF bodyStart = new PointF(Location.X, Location.Y + NamePadding.Top + nameSize.Height + NamePadding.Bottom + BodyPadding.Top);

            // compute in sockets size and set correct location for all in sockets
            SizeF inSocketsSize = new SizeF(0, 0);
            PointF actualPoint = new PointF(bodyStart.X - 1, bodyStart.Y);
            foreach (NodeSocketView nodeSocket in inSockets)
            {
                // set in socket location
                nodeSocket.Location = actualPoint;

                // update in sockets width if needed
                if (BodyPadding.Left + nodeSocket.NameSize.Width + SocketPadding.Right > inSocketsSize.Width)
                {
                    inSocketsSize.Width = BodyPadding.Left + nodeSocket.NameSize.Width + SocketPadding.Right;
                }
                // update in sockets height
                inSocketsSize.Height += SocketPadding.Top + nodeSocket.NameSize.Height + SocketPadding.Bottom;
                // move to the location of the next in socket
                actualPoint.Y += SocketPadding.Top + nodeSocket.NameSize.Height + SocketPadding.Bottom;
            }

            // compute out sockets size
            SizeF outSocketsSize = new SizeF(0, 0);
            foreach (NodeSocketView nodeSocket in outSockets)
            {
                // update out sockets width if needed
                if (SocketPadding.Left + nodeSocket.NameSize.Width + BodyPadding.Right > outSocketsSize.Width)
                {
                    outSocketsSize.Width = SocketPadding.Left + nodeSocket.NameSize.Width + BodyPadding.Right;
                }
                // update out sockets height
                outSocketsSize.Height += SocketPadding.Top + nodeSocket.NameSize.Height + SocketPadding.Bottom;
            }

            // compute variable sockets size
            SizeF variableSocketsSize = new SizeF(BodyPadding.Left + BodyPadding.Right, 0);
            foreach (NodeSocketView nodeSocket in variableSockets)
            {
                if (nodeSocket.Visible)
                {
                    // update variable sockets width
                    variableSocketsSize.Width += SocketPadding.Left + nodeSocket.NameSize.Width + SocketPadding.Right;
                    // update variable sockets height if needed
                    if (nodeSocket.NameSize.Height + BodyPadding.Bottom > variableSocketsSize.Height)
                    {
                        variableSocketsSize.Height = nodeSocket.NameSize.Height + BodyPadding.Bottom;
                    }
                }
            }

            // compute node size
            SizeF size = new SizeF(NamePadding.Left + nameSize.Width + NamePadding.Right,
                NamePadding.Top + nameSize.Height + NamePadding.Bottom + BodyPadding.Top + (inSocketsSize.Height > outSocketsSize.Height ? inSocketsSize.Height : outSocketsSize.Height) + variableSocketsSize.Height);
            if (inSocketsSize.Width + outSocketsSize.Width > size.Width) size.Width = inSocketsSize.Width + outSocketsSize.Width;
            if (variableSocketsSize.Width > size.Width) size.Width = variableSocketsSize.Width;

            // set node bounds (without all sockets)
            _bounds = new RectangleF(Location.X, Location.Y, size.Width, size.Height);
            // set node bounds (with all sockets)
            _nodeBounds = new RectangleF(inSockets.Count == 0 ? Bounds.X : (Bounds.X - SocketSize), Bounds.Y,
                (inSockets.Count == 0 ? 0 : SocketSize) + Bounds.Width + (outSockets.Count == 0 ? 0 : SocketSize),
                Bounds.Height + (variableSockets.Count == 0 ? 0 : SocketSize));

            // set location of the name text 
            nameLocation = new PointF(Location.X + (Bounds.Width - nameSize.Width) / 2, Location.Y + NamePadding.Top);
            // set rectangle for the name area
            nameRectangle = new RectangleF(Location.X, Location.Y, Bounds.Width, NamePadding.Top + nameSize.Height + NamePadding.Bottom);

            // set correct location for all out sockets
            actualPoint = new PointF(Bounds.X + Bounds.Width + 1, bodyStart.Y);
            foreach (NodeSocketView nodeSocket in outSockets)
            {
                // set out socket location
                nodeSocket.Location = actualPoint;
                // move to the location of the next out socket
                actualPoint.Y += SocketPadding.Top + nodeSocket.NameSize.Height + SocketPadding.Bottom;
            }

            // set correct location for all variable sockets
            actualPoint = new PointF(Bounds.X + BodyPadding.Left + SocketPadding.Left, Bounds.Y + Bounds.Height + 1);
            if (variableSocketsSize.Width < Bounds.Width) actualPoint.X += (Bounds.Width - variableSocketsSize.Width) / 2;
            foreach (NodeSocketView nodeSocket in variableSockets)
            {
                if (nodeSocket.Visible)
                {
                    // set variable socket location
                    nodeSocket.Location = actualPoint;
                    // move to the location of the next variable socket
                    actualPoint.X += SocketPadding.Left + nodeSocket.NameSize.Width + SocketPadding.Right;
                }
            }

            // set location of the comment text 
            commentLocation = new PointF(Location.X + SizeSettings.CommentPadding.X, Location.Y - DrawingTools.Font.GetHeight() - SizeSettings.CommentPadding.Y);
        }

        /// <inheritdoc />
        public override void Paint(Graphics graphics)
        {
            // paint node background
            DrawingTools.SolidBrush.Color = ColorSettings.ForNodeBackground(Node.NodeData.Type);
            graphics.FillRectangle(DrawingTools.SolidBrush, Bounds);

            // paint node border
            DrawingTools.Pen.Color = ColorSettings.ForNode(Node.NodeData.Type, SelectState);
            graphics.DrawRectangle(DrawingTools.Pen, Bounds);

            // paint name background
            DrawingTools.SolidBrush.Color = ColorSettings.ForNode(Node.NodeData.Type, SelectState);
            graphics.FillRectangle(DrawingTools.SolidBrush, nameRectangle);

            // paint name text
            DrawingTools.SolidBrush.Color = ColorSettings.NodeTitle;
            graphics.DrawString(Name, DrawingTools.BoldFont, DrawingTools.SolidBrush, nameLocation);

            // paint in sockets
            foreach (NodeSocketView socket in inSockets)
            {
                socket.Paint(graphics);
            }

            // paint out sockets
            foreach (NodeSocketView socket in outSockets)
            {
                socket.Paint(graphics);
            }

            // paint variable sockets
            foreach (NodeSocketView socket in variableSockets)
            {
                if (socket.Visible)
                {
                    socket.Paint(graphics);
                }
            }

            // paint comment text
            if (!String.IsNullOrEmpty(Node.Comment))
            {
                DrawingTools.SolidBrush.Color = ColorSettings.CommentText;
                graphics.DrawString(Node.Comment, DrawingTools.Font, DrawingTools.SolidBrush, commentLocation);
            }
        }

        /// <summary>
        /// Refreshes the node. Updates and invalidates the view of node.
        /// </summary>
        public void Refresh()
        {
            UpdateGui();
            Invalidate();
        }

        /// <inheritdoc />
        public override bool Contains(PointF point)
        {
            if (Bounds.Contains(point))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public NodeSocketView NodeSocketContains(PointF point)
        {
            if (NodeBounds.Contains(point))
            {
                foreach (NodeSocketView socket in inSockets)
                {
                    if (socket.Contains(point))
                    {
                        return socket;
                    }
                }

                foreach (NodeSocketView socket in outSockets)
                {
                    if (socket.Contains(point))
                    {
                        return socket;
                    }
                }

                foreach (NodeSocketView socket in variableSockets)
                {
                    if (socket.Contains(point))
                    {
                        return socket;
                    }
                }
            }

            return null;
        }

        /// <inheritdoc />
        public List<ConnectionView> GetAllConnections()
        {
            List<ConnectionView> allConnections = new List<ConnectionView>();

            foreach (NodeSocketView socket in inSockets)
            {
                allConnections.AddRange(socket.Connections);
            }

            foreach (NodeSocketView socket in outSockets)
            {
                allConnections.AddRange(socket.Connections);
            }

            foreach (NodeSocketView socket in variableSockets)
            {
                allConnections.AddRange(socket.Connections);
            }

            return allConnections;
        }

        /// <inheritdoc />
        public NodeSocketView GetSocketByName(string realName, NodeSocketType nodeSocketType)
        {
            if (nodeSocketType == NodeSocketType.SignalIn)
            {
                foreach (NodeSocketView nodeSocket in inSockets)
                {
                    if (nodeSocket.NodeSocket.NodeSocketData.RealName == realName) return nodeSocket;
                }
            }
            else if (nodeSocketType == NodeSocketType.SignalOut)
            {
                foreach (NodeSocketView nodeSocket in outSockets)
                {
                    if (nodeSocket.NodeSocket.NodeSocketData.RealName == realName) return nodeSocket;
                }
            }
            else
            {
                foreach (NodeSocketView nodeSocket in variableSockets)
                {
                    if (nodeSocket.NodeSocket.NodeSocketData.RealName == realName) return nodeSocket;
                }
            }

            return null;
        }

        private StringVar commentVar;

        /// <inheritdoc />
        public void ShowSettings(DataGridViewRowCollection rows)
        {
            // title
            DataGridViewRow row = new DataGridViewRow();

            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3, Title = Node.Name });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;
            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3 });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;
            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3 });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;
            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3 });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;

            rows.Add(row);

            // variable sockets
            foreach (NodeSocketView nodeSocketView in variableSockets)
            {
                nodeSocketView.IEditSettings.ShowSettings(rows);
            }

            // comment
            if (commentVar == null)
            {
                commentVar = new StringVar() { Value = Node.Comment };
                commentVar.ValueChanged += (object sender, EventArgs e) => { Node.Comment = commentVar.Value; };
            }

            row = new DataGridViewRow();

            row.Cells.Add(new DataGridViewDisableCheckBoxCell() { Enabled = false });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Comment" });
            row.Cells.Add(commentVar.GetGridCell());
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = VariableTypeHelper.FriendlyName(commentVar.VariableType) });

            rows.Add(row);
        }

        /// <summary>
        /// Gets all script node sockets.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerable<NodeSocketView> AllSockets()
        {
            foreach (NodeSocketView socket in inSockets)
            {
                yield return socket;
            }

            foreach (NodeSocketView socket in outSockets)
            {
                yield return socket;
            }

            foreach (NodeSocketView socket in variableSockets)
            {
                yield return socket;
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Node.CommentChanged -= new EventHandler(Node_CommentChanged);

                _node = null;
            }
        }

        /// <inheritdoc />
        public override SceneNode Clone()
        {
            return Node.Clone().CreateView();
        }
    }
}
