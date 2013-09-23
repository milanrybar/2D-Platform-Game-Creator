/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents <see cref="NodeSocket"/> at the scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    abstract class NodeSocketView : SceneNode, IConnecting
    {
        /// <summary>
        /// Gets the underlying script node socket.
        /// </summary>
        public NodeSocket NodeSocket
        {
            get { return _nodeSocket; }
        }
        private NodeSocket _nodeSocket;

        /// <summary>
        /// Gets the view of the script node where is the node socket used.
        /// </summary>
        public NodeView Node
        {
            get { return _node; }
        }
        private NodeView _node;

        /// <summary>
        /// Gets the view of the script node where is the node socket used.
        /// </summary>
        public SceneNode Parent
        {
            get { return Node; }
        }

        /// <summary>
        /// Gets or sets the name of the node socket.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _nameSize = TextRenderer.MeasureText(_name, DrawingTools.Font);
                UpdateGui();
            }
        }
        private string _name;

        /// <summary>
        /// Gets a size in pixels of the name text of the node socket.
        /// </summary>
        public Size NameSize
        {
            get { return _nameSize; }
        }
        private Size _nameSize;

        /// <summary>
        /// Gets or sets the location of the node socket at the scene.
        /// </summary>
        public override PointF Location
        {
            get { return _location; }
            set
            {
                _location = value;
                UpdateGui();
            }
        }
        private PointF _location;

        /// <summary>
        /// Gets the size and location of the node socket.
        /// </summary>
        public override RectangleF Bounds
        {
            get { return _bounds; }
        }
        protected RectangleF _bounds;

        /// <summary>
        /// Gets the type of the node socket.
        /// </summary>
        public NodeSocketType Type
        {
            get { return NodeSocket.Type; }
        }

        /// <summary>
        /// Gets the center of the node socket for the connection line. Used by <see cref="ConnectionView"/>.
        /// </summary>
        public PointF Center
        {
            get { return _center; }
        }
        protected PointF _center;

        /// <summary>
        /// Gets a value indicating whether this <see cref="NodeSocketView"/> is visible.
        /// </summary>
        public abstract bool Visible { get; }

        /// <inheritdoc />
        public override bool CanMove
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool CanConnect
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanClone
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanEditSettings
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override bool IsSocketsContainer
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override IConnecting IConnecting
        {
            get { return this; }
        }

        /// <inheritdoc />
        public override INodeSocketsContainer INodeSocketsContainer
        {
            get { return null; }
        }

        /// <inheritdoc />
        public override IEditSettings IEditSettings
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the connections of the node socket.
        /// </summary>
        public List<ConnectionView> Connections
        {
            get { return _connections; }
        }
        private List<ConnectionView> _connections = new List<ConnectionView>();

        /// <summary>
        /// Gets the color of the node socket.
        /// </summary>
        public abstract Color Color { get; }

        /// <summary>
        /// Location of the name text.
        /// </summary>
        protected PointF textLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeSocketView"/> class.
        /// </summary>
        /// <param name="scriptNode">The view of the script node where the view of the node socket will be used.</param>
        /// <param name="nodeSocket">The node socket.</param>
        public NodeSocketView(NodeView scriptNode, NodeSocket nodeSocket)
        {
            _node = scriptNode;
            _nodeSocket = nodeSocket;
            Name = nodeSocket.Name;
        }

        /// <inheritdoc />
        public override void OnSceneNodeAdded()
        {
        }

        /// <inheritdoc />
        public override void OnSceneNodeRemoved()
        {
        }

        /// <inheritdoc />
        public override bool Contains(PointF point)
        {
            return Bounds.Contains(point);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
        }

        /// <inheritdoc />
        /// <remarks>
        /// <see cref="NodeSocketView"/> can be cloned only via <see cref="NodeView"/>.
        /// </remarks>
        /// <exception cref="NotImplementedException"><see cref="NodeSocketView"/> can be cloned only via <see cref="NodeView"/>.</exception>
        public override SceneNode Clone()
        {
            throw new NotImplementedException("NodeSocketView can be cloned only via NodeView.");
        }

        /// <inheritdoc />
        public abstract bool Connection(NodeSocketView socket);

        /// <inheritdoc />
        public abstract ConnectionView MakeConnection(NodeSocketView socket);

        /// <inheritdoc />
        public abstract void OnConnectionAdded(IConnecting connector);

        /// <inheritdoc />
        public abstract void OnConnectionRemoved(IConnecting connector);

        /// <summary>
        /// Updates any necessary variables for GUI.
        /// </summary>
        protected abstract void UpdateGui();
    }

    /// <summary>
    /// Represents script signal in node socket (<see cref="SignalNodeSocket"/>) at the scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    class SignalInNodeSocketView : NodeSocketView
    {
        /// <summary>
        /// Gets the underlying script signal node socket.
        /// </summary>
        public SignalNodeSocket SignalNodeSocket
        {
            get { return _signalNodeSocket; }
        }
        private SignalNodeSocket _signalNodeSocket;

        /// <inheritdoc />
        public override Color Color
        {
            get { return ColorSettings.SignalNodeSocket; }
        }

        /// <inheritdoc />
        public override bool Visible
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalInNodeSocketView"/> class.
        /// </summary>
        /// <param name="scriptNode">The view of the script node where the view of the node socket will be used.</param>
        /// <param name="signalNodeSocket">The signal node socket.</param>
        public SignalInNodeSocketView(NodeView scriptNode, SignalNodeSocket signalNodeSocket)
            : base(scriptNode, signalNodeSocket)
        {
            _signalNodeSocket = signalNodeSocket;
        }

        /// <inheritdoc />
        public override void Paint(Graphics graphics)
        {
            // paint in socket
            if (this.SelectState == Scripting.SelectState.Default) DrawingTools.SolidBrush.Color = ColorSettings.SignalNodeSocket;
            else if (this.SelectState == Scripting.SelectState.Hover) DrawingTools.SolidBrush.Color = ColorSettings.Hover;
            graphics.FillRectangle(DrawingTools.SolidBrush, Bounds);

            // paint name text
            DrawingTools.SolidBrush.Color = ColorSettings.NodeSocketText;
            graphics.DrawString(Name, DrawingTools.Font, DrawingTools.SolidBrush, textLocation);
        }

        /// <inheritdoc />
        protected override void UpdateGui()
        {
            _bounds = new RectangleF(Location.X - SizeSettings.NodeSocketSize, Location.Y, SizeSettings.NodeSocketSize, SizeSettings.NodeSocketSize);
            _center = new PointF(_bounds.X + _bounds.Width / 2, _bounds.Y + _bounds.Height / 2);
            textLocation = new PointF(Location.X + SizeSettings.NodeBodyPadding.Left, Location.Y + (SizeSettings.NodeSocketSize - NameSize.Height) / 2);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Signal in node socket is able to connect to signal out node socket.
        /// </remarks>
        public override bool Connection(NodeSocketView socket)
        {
            if (socket.Type == NodeSocketType.SignalOut) return true;
            else return false;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Signal in node socket is able to connect to signal out node socket.
        /// </remarks>
        public override ConnectionView MakeConnection(NodeSocketView socket)
        {
            if (socket.Type == NodeSocketType.SignalOut && !this.ContainsConnection(socket, this))
            {
                return new ConnectionView(socket, this);
            }

            return null;
        }

        /// <inheritdoc />
        public override void OnConnectionAdded(IConnecting connector)
        {
        }

        /// <inheritdoc />
        public override void OnConnectionRemoved(IConnecting connector)
        {
        }
    }

    /// <summary>
    /// Represents script signal out node socket (<see cref="SignalNodeSocket"/>) at the scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    class SignalOutNodeSocketView : NodeSocketView
    {
        /// <summary>
        /// Gets the underlying script signal node socket.
        /// </summary>
        public SignalNodeSocket SignalNodeSocket
        {
            get { return _signalNodeSocket; }
        }
        private SignalNodeSocket _signalNodeSocket;

        /// <inheritdoc />
        public override Color Color
        {
            get { return ColorSettings.SignalNodeSocket; }
        }

        /// <inheritdoc />
        public override bool Visible
        {
            get { return true; }
        }

        private static StringFormat stringRightFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalOutNodeSocketView"/> class.
        /// </summary>
        /// <param name="scriptNode">The view of the script node where the view of the node socket will be used.</param>
        /// <param name="signalNodeSocket">The signal node socket.</param>
        public SignalOutNodeSocketView(NodeView scriptNode, SignalNodeSocket signalNodeSocket)
            : base(scriptNode, signalNodeSocket)
        {
            _signalNodeSocket = signalNodeSocket;
        }

        /// <inheritdoc />
        public override void Paint(Graphics graphics)
        {
            if (stringRightFormat == null)
            {
                stringRightFormat = new StringFormat(StringFormat.GenericDefault);
                stringRightFormat.Alignment = StringAlignment.Far;
            }

            // paint out socket
            if (this.SelectState == Scripting.SelectState.Default) DrawingTools.SolidBrush.Color = ColorSettings.SignalNodeSocket;
            else if (this.SelectState == Scripting.SelectState.Hover) DrawingTools.SolidBrush.Color = ColorSettings.Hover;
            graphics.FillRectangle(DrawingTools.SolidBrush, Bounds);

            // paint name text
            DrawingTools.SolidBrush.Color = ColorSettings.NodeSocketText;
            graphics.DrawString(Name, DrawingTools.Font, DrawingTools.SolidBrush, textLocation, stringRightFormat);
        }

        /// <inheritdoc />
        protected override void UpdateGui()
        {
            _bounds = new RectangleF(Location.X, Location.Y, SizeSettings.NodeSocketSize, SizeSettings.NodeSocketSize);
            _center = new PointF(_bounds.X + _bounds.Width / 2, _bounds.Y + _bounds.Height / 2);
            textLocation = new PointF(Location.X - SizeSettings.NodeBodyPadding.Right, Location.Y + (SizeSettings.NodeSocketSize - NameSize.Height) / 2);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Signal out node socket is able to connect to signal in node socket.
        /// </remarks>
        public override bool Connection(NodeSocketView socket)
        {
            if (socket.Type == NodeSocketType.SignalIn) return true;
            else return false;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Signal out node socket is able to connect to signal in node socket.
        /// </remarks>
        public override ConnectionView MakeConnection(NodeSocketView socket)
        {
            if (socket.Type == NodeSocketType.SignalIn && !this.ContainsConnection(this, socket))
            {
                return new ConnectionView(this, socket);
            }

            return null;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Connection is added to the connections of this <see cref="SignalNodeSocket"/>.
        /// </remarks>
        public override void OnConnectionAdded(IConnecting connector)
        {
            Debug.Assert(!SignalNodeSocket.Connections.Contains(((SignalInNodeSocketView)connector).SignalNodeSocket), "Scripting component already contains this connection.");

            SignalNodeSocket.Connections.Add(((SignalInNodeSocketView)connector).SignalNodeSocket);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Connection is removed from the connections of this <see cref="SignalNodeSocket"/>.
        /// </remarks>
        public override void OnConnectionRemoved(IConnecting connector)
        {
            Debug.Assert(SignalNodeSocket.Connections.Contains(((SignalInNodeSocketView)connector).SignalNodeSocket), "Scripting component already contains this connection.");

            SignalNodeSocket.Connections.Remove(((SignalInNodeSocketView)connector).SignalNodeSocket);
        }
    }

    /// <summary>
    /// Represents script variable node socket (<see cref="VariableNodeSocket"/>) at the scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    class VariableNodeSocketView : NodeSocketView, IEditSettings
    {
        /// <summary>
        /// Gets the underlying script variable node socket.
        /// </summary>
        public VariableNodeSocket VariableNodeSocket
        {
            get { return _variableNodeSocket; }
        }
        private VariableNodeSocket _variableNodeSocket;

        /// <inheritdoc />
        public override Color Color
        {
            get { return ColorSettings.ForVariable(VariableNodeSocket.VariableType); }
        }

        /// <inheritdoc />
        public override IEditSettings IEditSettings
        {
            get { return this; }
        }

        /// <inheritdoc />
        public override bool Visible
        {
            get { return VariableNodeSocket.Visible; }
        }

        private RectangleF valueTextRectangle;
        private static StringFormat valueTextFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNodeSocketView"/> class.
        /// </summary>
        /// <param name="scriptNode">The view of the script node where the view of the node socket will be used.</param>
        /// <param name="variableNodeSocket">The variable node socket.</param>
        public VariableNodeSocketView(NodeView scriptNode, VariableNodeSocket variableNodeSocket)
            : base(scriptNode, variableNodeSocket)
        {
            _variableNodeSocket = variableNodeSocket;

            if (VariableNodeSocket.Type == NodeSocketType.VariableIn)
            {
                VariableNodeSocket.Value.ValueChanged += new EventHandler(Value_ValueChanged);
            }

            if (valueTextFormat == null)
            {
                valueTextFormat = new StringFormat();
                valueTextFormat.Trimming = StringTrimming.EllipsisCharacter;
                valueTextFormat.Alignment = StringAlignment.Center;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the Value of the underlying VariableNodeSocket.
        /// Invalidates this scene node.
        /// </summary>
        private void Value_ValueChanged(object sender, EventArgs e)
        {
            // is painted default value text?
            if (VariableNodeSocket.Type == NodeSocketType.VariableIn && VariableNodeSocket.Connections.Count == 0 && !VariableNodeSocket.NodeSocketData.CanBeEmpty)
            {
                Node.SceneControl.Invalidate();
            }
        }

        /// <inheritdoc />
        public override void Paint(Graphics graphics)
        {
            // paint variable socket
            if (this.SelectState == Scripting.SelectState.Default) DrawingTools.SolidBrush.Color = ColorSettings.ForVariable(VariableNodeSocket.VariableType);
            else if (this.SelectState == Scripting.SelectState.Hover) DrawingTools.SolidBrush.Color = ColorSettings.Hover;

            if (Type == NodeSocketType.VariableIn)
            {
                // variable in = rectangle
                graphics.FillRectangle(DrawingTools.SolidBrush, Bounds);
            }
            else
            {
                // variable out = triangle
                PointF[] triangle = new PointF[3];
                triangle[0] = new PointF(Bounds.X, Bounds.Y);
                triangle[1] = new PointF(Bounds.X + Bounds.Width, Bounds.Y);
                triangle[2] = new PointF(Bounds.X + Bounds.Width / 2f, Bounds.Y + Bounds.Height);
                graphics.FillPolygon(DrawingTools.SolidBrush, triangle);
            }

            // paint name text
            DrawingTools.SolidBrush.Color = ColorSettings.NodeSocketText;
            graphics.DrawString(Name, DrawingTools.Font, DrawingTools.SolidBrush, textLocation);

            // paint default value text
            if (VariableNodeSocket.Type == NodeSocketType.VariableIn && VariableNodeSocket.Connections.Count == 0 && !VariableNodeSocket.NodeSocketData.CanBeEmpty)
            {
                DrawingTools.SolidBrush.Color = ColorSettings.NodeVariableSocketValueText;
                graphics.DrawString(VariableNodeSocket.Value.ToString(), DrawingTools.Font, DrawingTools.SolidBrush, valueTextRectangle, valueTextFormat);
            }
        }

        /// <inheritdoc />
        protected override void UpdateGui()
        {
            _bounds = new RectangleF(Location.X + (NameSize.Width - SizeSettings.NodeSocketSize) / 2, Location.Y, SizeSettings.NodeSocketSize, SizeSettings.NodeSocketSize);
            _center = new PointF(_bounds.X + _bounds.Width / 2, _bounds.Y + _bounds.Height / 2);
            textLocation = new PointF(Location.X, Location.Y - SizeSettings.NodeBodyPadding.Bottom - NameSize.Height);
            valueTextRectangle = new RectangleF(Location.X, Location.Y + SizeSettings.NodeSocketSize + SizeSettings.NodeSocketPadding.Bottom, NameSize.Width, DrawingTools.Font.GetHeight());
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (VariableNodeSocket.Type == NodeSocketType.VariableIn)
                {
                    VariableNodeSocket.Value.ValueChanged -= new EventHandler(Value_ValueChanged);
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Variable node socket is not able to connect to other node socket.
        /// </remarks>
        public override bool Connection(NodeSocketView socket)
        {
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Variable node socket is not able to connect to other node socket.
        /// </remarks>
        public override ConnectionView MakeConnection(NodeSocketView socket)
        {
            return null;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Connection is added to the connections of this <see cref="VariableNodeSocket"/>.
        /// </remarks>
        public override void OnConnectionAdded(IConnecting connector)
        {
            VariableNodeSocket.Connections.Add(((VariableView)connector).Variable);

            UpdateRow();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Connection is removed from the connections of this <see cref="VariableNodeSocket"/>.
        /// </remarks>
        public override void OnConnectionRemoved(IConnecting connector)
        {
            VariableNodeSocket.Connections.Remove(((VariableView)connector).Variable);

            UpdateRow();
        }

        private DataGridViewRow row;
        private enum VariableNodeSocketCells { Visible = 0, Name = 1, Value = 2, Type = 3 };

        /// <inheritdoc />
        public void ShowSettings(DataGridViewRowCollection rows)
        {
            row = new DataGridViewRow();

            DataGridViewDisableCheckBoxCell visibleCell = new DataGridViewDisableCheckBoxCell() { Value = VariableNodeSocket.Visible, Enabled = VariableNodeSocket.Connections.Count == 0 };
            visibleCell.ValueChanged += (object sender, EventArgs e) =>
            {
                VariableNodeSocket.Visible = (bool)visibleCell.Value;
                if (!VariableNodeSocket.Visible && VariableNodeSocket.Connections.Count != 0) VariableNodeSocket.Visible = true;
                else Node.Refresh();
            };
            row.Cells.Add(visibleCell);

            row.Cells.Add(new DataGridViewTextBoxCell() { Value = VariableNodeSocket.Name });

            // variable in
            if (VariableNodeSocket.Type == NodeSocketType.VariableIn)
            {
                if (VariableNodeSocket.Connections.Count == 0)
                {
                    // edit value
                    if (!VariableNodeSocket.NodeSocketData.CanBeEmpty)
                    {
                        row.Cells.Add(VariableNodeSocket.Value.GetGridCell());
                    }
                    // only connection is possible
                    else
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = "(connection-only)" });
                        row.Cells[row.Cells.Count - 1].ReadOnly = true;
                    }
                }
                // socket used
                else
                {
                    row.Cells.Add(new DataGridViewTextBoxCell() { Value = "(socket used)" });
                    row.Cells[row.Cells.Count - 1].ReadOnly = true;
                }
            }
            // variable out
            else
            {
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = "(read-only)" });
                row.Cells[row.Cells.Count - 1].ReadOnly = true;
            }

            row.Cells.Add(new DataGridViewTextBoxCell() { Value = VariableTypeHelper.FriendlyName(VariableNodeSocket.VariableType) });

            rows.Add(row);
        }

        /// <summary>
        /// Called when the connection is added or removed.
        /// Updates the row of the settings to edit this variable node socket.
        /// </summary>
        private void UpdateRow()
        {
            if (row != null && row.DataGridView != null && row.Index != -1)
            {
                ((DataGridViewDisableCheckBoxCell)row.Cells[(int)VariableNodeSocketCells.Visible]).Enabled = VariableNodeSocket.Connections.Count == 0;
                row.DataGridView.InvalidateCell(row.Cells[(int)VariableNodeSocketCells.Visible]);

                if (VariableNodeSocket.Type == NodeSocketType.VariableIn && VariableNodeSocket.Connections.Count == 1 && !row.Cells[(int)VariableNodeSocketCells.Value].ReadOnly)
                {
                    row.Cells[(int)VariableNodeSocketCells.Value] = new DataGridViewTextBoxCell() { Value = "(socket used)" };
                    row.Cells[(int)VariableNodeSocketCells.Value].ReadOnly = true;
                }
            }
        }
    }
}
