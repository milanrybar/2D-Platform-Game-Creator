/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents screen for the visual scripting scene.
    /// </summary>
    /// <remarks>
    /// Changes the position at the scene by right mouse button or arrows keys.
    /// Zooms the scene by the mouse wheel.
    /// Undoes last command at the scene by Ctrl+Z.
    /// Redoes last command at the scene by Ctrl+Y.
    /// Selects the scene nodes at the scene by left mouse button.
    /// Select or deselect scene node by left mouse button + Ctrl.
    /// Selects the scene nodes by rectangle even on hovered scene node by left mouse button + left Alt.
    /// Deletes the selected scene nodes from the scene by Delete key.
    /// Moves the selected scene nodes at the scene by left mouse button.
    /// Saves the selected scene nodes to the clipboard by Ctrl+C.
    /// Pastes the scene nodes from the clipboard by Ctrl+V.
    /// </remarks>
    partial class ScriptingScreen : Control, ISceneDrawingTools
    {
        /// <summary>
        /// Gets or sets the underlying <see cref="Scripting.State"/>.
        /// </summary>
        public State State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    if (_state != null)
                    {
                        _state.Nodes.ListChanged -= new ObservableList<BaseNode>.ListChangedEventHandler(StateNodes_ListChanged);

                        hoveredNodes.Clear();
                        SelectedNodes.Clear();
                        hoveredSocket = null;
                        selectedSocket = null;

                        Clipboard.Clear();
                        History.Clear();

                        foreach (SceneNode sceneNode in Nodes)
                        {
                            sceneNode.Dispose();
                        }
                        Nodes.Clear();

                        this.state = StateType.Default;
                        Zoom = 100;
                        Position = new PointF();

                        SelectedNodesChangedHandler();
                    }

                    _state = value;

                    if (_state != null)
                    {
                        _state.Nodes.ListChanged += new ObservableList<BaseNode>.ListChangedEventHandler(StateNodes_ListChanged);

                        Initialize();
                    }
                }
            }
        }
        private State _state;

        /// <summary>
        /// Gets or sets the position at the scripting screen.
        /// </summary>
        public PointF Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdateTransformMatrix();
            }
        }
        private PointF _position;

        /// <summary>
        /// Gets or sets the zoom (10-100) for the scripting screen.
        /// </summary>
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom > 100) _zoom = 100;
                else if (_zoom < 10) _zoom = 10;
                UpdateTransformMatrix();
            }
        }
        private int _zoom = 100;

        /// <summary>
        /// Gets scale factor defined by <see cref="Zoom"/>.
        /// </summary>
        public float ScaleFactor
        {
            get { return (float)Zoom / 100f; }
        }

        /// <summary>
        /// Gets invers scale factor defined by <see cref="Zoom"/>.
        /// </summary>
        public float ScaleInversFactor
        {
            get { return 100f / (float)Zoom; }
        }

        /// <summary>
        /// Gets the history of the scripting scene.
        /// </summary>
        public History History
        {
            get { return _history; }
        }
        private History _history = new History();

        /// <summary>
        /// Gets the clipboard for copying and pasting scene nodes at the scripting scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HashSet<SceneNode> Clipboard
        {
            get { return _clipboard; }
        }
        private HashSet<SceneNode> _clipboard = new HashSet<SceneNode>();

        /// <summary>
        /// Position at the scripting scene under mouse cursor when copying scene nodes to the clipboard.
        /// </summary>
        private PointF clipboardPosition;

        /// <summary>
        /// Gets or sets mouse position at the scripting scene.
        /// </summary>
        public PointF MouseScenePosition
        {
            get { return _mouseScenePosition; }
        }
        private PointF _mouseScenePosition;

        // transform matrix
        private Matrix transform = new Matrix();
        // invers transform matrix
        private Matrix transformInvers = new Matrix();

        /// <summary>
        /// Scene nodes of the scripting screen.
        /// </summary>
        private List<SceneNode> Nodes = new List<SceneNode>();

        /// <summary>
        /// Hovered scene nodes of the scripting screen.
        /// </summary>
        private List<SceneNode> hoveredNodes = new List<SceneNode>();

        /// <summary>
        /// Gets the selected scene nodes of the scripting screen.
        /// </summary>
        public HashSet<SceneNode> SelectedNodes
        {
            get { return _selectedNodes; }
        }
        private HashSet<SceneNode> _selectedNodes = new HashSet<SceneNode>();

        // hovered node socket
        private NodeSocketView hoveredSocket;

        // selected node socket
        private NodeSocketView selectedSocket;

        /// <summary>
        /// Occurs when the <see cref="SelectedNodes"/> changes.
        /// </summary>
        public event EventHandler SelectedNodesChanged;

        /// <summary>
        /// Gets the solid brush.
        /// </summary>
        public SolidBrush SolidBrush
        {
            get { return _solidBrush; }
        }
        private static SolidBrush _solidBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// Gets the pen for drawing a line.
        /// </summary>
        public Pen LinePen
        {
            get { return _linePen; }
        }
        private static Pen _linePen = new Pen(Color.Black, 2);
        private static AdjustableArrowCap arrowForLine = new AdjustableArrowCap(5, 10, true);

        /// <summary>
        /// Gets the pen.
        /// </summary>
        public Pen Pen
        {
            get { return _pen; }
        }
        private static Pen _pen = new Pen(Color.White);

        /// <summary>
        /// Gets the bold font.
        /// </summary>
        public Font BoldFont
        {
            get { return _boldFont; }
        }
        private Font _boldFont;

        /// <summary>
        /// Gets or sets a value indicating whether the underlying collection is changing.
        /// </summary>
        public bool UnderlyingCollectionChanging
        {
            get { return _underlyingCollectionChanging; }
            set
            {
                if (_underlyingCollectionChanging != value)
                {
                    _underlyingCollectionChanging = value;
                    underlyingCollectionChanged = false;
                }
            }
        }
        private bool _underlyingCollectionChanging;

        /// <summary>
        /// Indicating whether the underlying collection changes from the outside.
        /// </summary>
        private bool underlyingCollectionChanged;

        /// <summary>
        /// Defines a state of the <see cref="ScriptingScreen"/>.
        /// </summary>
        private enum StateType { Default, MovingScene, MovingNodes, SelectingNodes, ConnectingNodes };

        /// <summary>
        /// State of the <see cref="ScriptingScreen"/>.
        /// </summary>
        private StateType state;

        // last position for changing position of the scene and selected nodes
        private PointF lastPosition;
        private PointF wholeMovement;
        // initial point of selecting rectangle
        private PointF selectingInitPosition;
        // selecting rectangle for selecting nodes at the scene
        private RectangleF selectingRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptingScreen"/> class.
        /// </summary>
        public ScriptingScreen()
        {
            DoubleBuffered = true;

            InitializeComponent();

            MouseWheel += new MouseEventHandler(ScriptingScreen_MouseWheel);

            SceneNode.DrawingTools = this;
            _boldFont = new Font(Font, FontStyle.Bold);
        }

        /// <summary>
        /// Handles the ListChanged event of the Nodes of the underlying State.
        /// Updates the scene nodes.
        /// </summary>
        private void StateNodes_ListChanged(object sender, ObservableListChangedEventArgs<BaseNode> e)
        {
            // internal changing takes about everything
            if (UnderlyingCollectionChanging) return;

            // proccess external change
            // only supported operation is deleting (from safe deleting)
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    Debug.Assert(true, "Not supported external change.");
                    break;

                case ObservableListChangedType.ItemDeleted:
                    SceneNode deletedSceneNode = FindSceneNode(e.Item);
                    if (deletedSceneNode != null)
                    {
                        Nodes.Remove(deletedSceneNode);

                        // delete all node connections if any
                        if (deletedSceneNode.IsSocketsContainer || deletedSceneNode.CanConnect)
                        {
                            // get node connections
                            List<ConnectionView> connections;
                            if (deletedSceneNode.IsSocketsContainer) connections = deletedSceneNode.INodeSocketsContainer.GetAllConnections();
                            else connections = new List<ConnectionView>(deletedSceneNode.IConnecting.Connections);

                            // remove all connections from the scripting screen
                            foreach (ConnectionView connection in connections)
                            {
                                Nodes.Remove(connection);
                                connection.OnSceneNodeRemoved();
                            }
                        }
                    }
                    if (SelectedNodes.Contains(deletedSceneNode))
                    {
                        ClearSelectedNodes();
                        SelectedNodesChangedHandler();
                    }
                    break;

                case ObservableListChangedType.Reset:
                    Debug.Assert(true, "Not supported external change.");
                    break;
            }

            underlyingCollectionChanged = true;
        }

        /// <summary>
        /// Finds the <see cref="SceneNode"/> that represents the specified script node.
        /// </summary>
        /// <param name="baseNode">The script node to find.</param>
        /// <returns><see cref="SceneNode"/> if found; othewise <c>null</c>.</returns>
        private SceneNode FindSceneNode(BaseNode baseNode)
        {
            if (baseNode is Node) return FindSceneNode((Node)baseNode);
            else if (baseNode is Variable) return FindSceneNode((Variable)baseNode);
            else
            {
                Debug.Assert(true, "Not supported scripting node.");
                return null;
            }
        }

        /// <summary>
        /// Finds the <see cref="SceneNode"/> that represents the specified script node.
        /// </summary>
        /// <param name="node">The script node to find.</param>
        /// <returns><see cref="SceneNode"/> if found; othewise <c>null</c>.</returns>
        private SceneNode FindSceneNode(Node node)
        {
            foreach (SceneNode sceneNode in Nodes)
            {
                NodeView nodeView = sceneNode as NodeView;
                if (nodeView != null && nodeView.Node == node)
                {
                    return nodeView;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the <see cref="SceneNode"/> that represents the specified script variable.
        /// </summary>
        /// <param name="variable">The script variable to find.</param>
        /// <returns><see cref="SceneNode"/> if found; othewise <c>null</c>.</returns>
        private SceneNode FindSceneNode(Variable variable)
        {
            foreach (SceneNode sceneNode in Nodes)
            {
                VariableView variableView = sceneNode as VariableView;
                if (variableView != null && variableView.Variable == variable)
                {
                    return variableView;
                }
            }

            return null;
        }

        /// <summary>
        /// Initializes the <see cref="ScriptingScreen"/> for the current <see cref="State"/>.
        /// Creates all scene nodes and connections for the nodes of the <see cref="State"/>.
        /// </summary>
        private void Initialize()
        {
            Dictionary<BaseNode, SceneNode> createdNodes = new Dictionary<BaseNode, SceneNode>();

            // create view of all nodes
            foreach (BaseNode node in State.Nodes)
            {
                SceneNode newNode = node.CreateView();
                newNode.SceneControl = this;
                createdNodes[node] = newNode;

                Nodes.Add(newNode);
            }

            // create view of connections between nodes
            foreach (BaseNode node in State.Nodes)
            {
                // script node
                if (node is Node)
                {
                    NodeView nodeView = (NodeView)createdNodes[node];

                    // get all sockets
                    foreach (NodeSocket socket in ((Node)node).Sockets)
                    {
                        ConnectionView connectionView = null;

                        // signal out node socket
                        if (socket.Type == NodeSocketType.SignalOut)
                        {
                            // get all connections
                            foreach (SignalNodeSocket otherNodeSocket in ((SignalNodeSocket)socket).Connections)
                            {
                                NodeView otherNodeView = (NodeView)createdNodes[otherNodeSocket.Node];
                                NodeSocketView otherNodeSocketView = otherNodeView.GetSocketByName(otherNodeSocket.NodeSocketData.RealName, otherNodeSocket.Type);

                                // create connection
                                connectionView = nodeView.GetSocketByName(socket.NodeSocketData.RealName, socket.Type).MakeConnection(otherNodeSocketView);

                                if (connectionView != null)
                                {
                                    // add connection to the scripting scene
                                    Nodes.Add(connectionView);
                                    connectionView.SceneControl = this;
                                    connectionView.From.Connections.Add(connectionView);
                                    connectionView.To.Connections.Add(connectionView);
                                }
                            }
                        }

                        // variable node socket
                        else if (socket.Type == NodeSocketType.VariableIn || socket.Type == NodeSocketType.VariableOut)
                        {
                            // get all connections
                            foreach (Variable connection in ((VariableNodeSocket)socket).Connections)
                            {
                                // other end is VariableView<VarType>
                                IConnecting other = (IConnecting)createdNodes[connection];

                                // create connection
                                NodeSocketView socketView = nodeView.GetSocketByName(socket.NodeSocketData.RealName, socket.Type);

                                if (socketView.Type == NodeSocketType.VariableIn) connectionView = new ConnectionView(other, socketView);
                                else if (socketView.Type == NodeSocketType.VariableOut) connectionView = new ConnectionView(socketView, other);

                                if (connectionView != null)
                                {
                                    // add connection to the scripting scene
                                    Nodes.Add(connectionView);
                                    connectionView.SceneControl = this;
                                    connectionView.From.Connections.Add(connectionView);
                                    connectionView.To.Connections.Add(connectionView);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Invokes <see cref="SelectedNodesChanged"/> event.
        /// </summary>
        private void SelectedNodesChangedHandler()
        {
            if (SelectedNodesChanged != null) SelectedNodesChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates transform matrix.
        /// </summary>
        private void UpdateTransformMatrix()
        {
            // update transform matrix
            transform.Reset();
            transform.Scale(ScaleFactor, ScaleFactor);
            transform.Translate(-Position.X, -Position.Y);

            // update invers transform matrix
            transformInvers.Reset();
            transformInvers.Translate(Position.X, Position.Y);
            transformInvers.Scale(ScaleInversFactor, ScaleInversFactor);
        }

        /// <summary>
        /// Converts the specified point from control to screen (abstract scene) coordinates.
        /// </summary>
        /// <param name="point">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the screen coordinates.</returns>
        private PointF PointAtScene(PointF point)
        {
            return point.Transform(transformInvers);
        }

        /// <summary>
        /// Converts the specified point from control to screen (abstract scene) coordinates.
        /// </summary>
        /// <param name="point">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the screen coordinates.</returns>
        private PointF PointAtScene(Point point)
        {
            return point.Transform(transformInvers);
        }

        /// <summary>
        /// Adds the scene node to the center of the scripting screen.
        /// </summary>
        /// <param name="sceneNode">The scene node to add.</param>
        public void AddSceneNodeToCenter(SceneNode sceneNode)
        {
            sceneNode.SceneControl = this;
            sceneNode.Location = PointAtScene(new Point(Width / 2, Height / 2));

            Nodes.Add(sceneNode);
            sceneNode.OnSceneNodeAdded();

            History.Add(new AddSceneNodeCommand(this, sceneNode));
        }

        /// <summary>
        /// Called when the underlyings collections changes from the outside.
        /// Clears <see cref="History"/> and <see cref="Clipboard"/> and invalidates the scripting screen, if needed.
        /// </summary>
        public void UnderlyingCollectionChange()
        {
            History.Clear();
            Clipboard.Clear();

            if (!UnderlyingCollectionChanging && underlyingCollectionChanged)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// Zooms and centers the scripting scene to be able to see the whole scene at the <see cref="ScriptingScreen"/>, if possible.
        /// </summary>
        public void Fit()
        {
            if (Nodes.Count != 0)
            {
                RectangleF sceneRectangle = Nodes[0].Bounds;

                foreach (SceneNode sceneNode in Nodes)
                {
                    if (sceneNode.CanClone) sceneRectangle = RectangleF.Union(sceneRectangle, sceneNode.Bounds);
                }

                float scaleWidth = Width / sceneRectangle.Width;
                float scaleHeight = Height / sceneRectangle.Height;
                float scale = scaleWidth < scaleHeight ? scaleWidth : scaleHeight;

                Zoom = (int)(scale * 100f);
                scale = Zoom / 100f;

                PointF position = sceneRectangle.Location;

                if (Width > sceneRectangle.Width * scale) position.X -= (Width - sceneRectangle.Width * scale) / 2f * ScaleInversFactor;
                else position.X -= (sceneRectangle.Width * scale - Width) / 2f * ScaleInversFactor;
                if (Height > sceneRectangle.Height * scale) position.Y -= (Height - sceneRectangle.Height * scale) / 2f * ScaleInversFactor;
                else position.Y -= (sceneRectangle.Height * scale - Height) / 2f * ScaleInversFactor;

                Position = position;
            }
        }

        /// <summary>
        /// Clears hovered scene nodes at the scripting scene.
        /// </summary>
        private void ClearHoveredNodes()
        {
            if (hoveredNodes.Count != 0)
            {
                foreach (SceneNode hoveredNode in hoveredNodes)
                {
                    if (hoveredNode.SelectState != SelectState.Select) hoveredNode.SelectState = SelectState.Default;
                }
                hoveredNodes.Clear();
            }
        }

        /// <summary>
        /// Clears selected scene nodes at the scripting scene.
        /// </summary>
        private void ClearSelectedNodes()
        {
            if (SelectedNodes.Count != 0)
            {
                foreach (SceneNode selectedNode in SelectedNodes)
                {
                    selectedNode.SelectState = SelectState.Default;
                }
                SelectedNodes.Clear();
            }
        }

        /// <summary>
        /// Selects connections of the specified scene node.
        /// </summary>
        /// <param name="sceneNode">Scene node to select connections from.</param>
        private void SelectNodeConnections(SceneNode sceneNode)
        {
            if (sceneNode.IsSocketsContainer || sceneNode.CanConnect)
            {
                // get node connections
                List<ConnectionView> connections;
                if (sceneNode.IsSocketsContainer) connections = sceneNode.INodeSocketsContainer.GetAllConnections();
                else connections = sceneNode.IConnecting.Connections;

                // select connetions
                foreach (ConnectionView connection in connections)
                {
                    connection.SelectState = SelectState.Select;
                    SelectedNodes.Add(connection);
                }
            }
        }

        /// <summary>
        /// Selects all hovered scene nodes.
        /// </summary>
        private void SelectHoveredNodes()
        {
            // clear the old selected nodes
            ClearSelectedNodes();

            // select hovered nodes
            foreach (SceneNode hoveredNode in hoveredNodes)
            {
                hoveredNode.SelectState = SelectState.Select;
                SelectedNodes.Add(hoveredNode);

                SelectNodeConnections(hoveredNode);
            }
            hoveredNodes.Clear();

            SelectedNodesChangedHandler();
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the scripting screen.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            // paint background
            pe.Graphics.Clear(ColorSettings.ScriptingScreenBackground);

            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // set pen line variables
            arrowForLine.Width = 5 * ScaleFactor;
            arrowForLine.Height = 10 * ScaleFactor;
            LinePen.CustomEndCap = arrowForLine;

            // set transform matrix
            pe.Graphics.Transform = transform;

            // paint script scene nodes
            foreach (SceneNode node in Nodes)
            {
                node.Paint(pe.Graphics);
            }

            // if the action of selecting nodes is active we will paint selecting rectangle
            if (state == StateType.SelectingNodes)
            {
                SolidBrush.Color = ColorSettings.SelectingNodesRectangle;
                pe.Graphics.FillRectangle(SolidBrush, selectingRectangle);
            }
            // if the action of connections nodes is active we will paint connecting line
            else if (state == StateType.ConnectingNodes)
            {
                LinePen.Color = selectedSocket.Color;
                pe.Graphics.DrawLine(LinePen, selectedSocket.Center, MouseScenePosition);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the ScriptingScreen control.
        /// </summary>
        private void ScriptingScreen_MouseDown(object sender, MouseEventArgs e)
        {
            // focus control if not
            if (!Focused) Focus();

            // If left mouse button is down and no action is in progress
            // we will make some action.
            if (e.Button == MouseButtons.Left && state == StateType.Default)
            {
                // If left mouse button is down, no action is in progress and node socket is under mouse cursor 
                // we will start the action of connecting node sockets at the scripting scene.
                if (hoveredSocket != null)
                {
                    // activate this action
                    state = StateType.ConnectingNodes;
                    // init
                    selectedSocket = hoveredSocket;

                    Invalidate();
                }

                // If left mouse button is down, Ctrl key is down and no action is in progress
                // we will select or deselect node under the mouse cursor at the scripting screen.
                else if (Form.ModifierKeys == Keys.Control)
                {
                    Debug.Assert(hoveredNodes.Count <= 1, "Only one node can be hovered.");

                    foreach (SceneNode hoveredNode in hoveredNodes)
                    {
                        // select node under mouse cursor
                        if (!SelectedNodes.Contains(hoveredNode))
                        {
                            hoveredNode.SelectState = SelectState.Select;
                            SelectedNodes.Add(hoveredNode);

                            SelectNodeConnections(hoveredNode);

                            SelectedNodesChangedHandler();
                            Invalidate();
                        }
                        // deselect node under mouse cursor
                        else
                        {
                            hoveredNode.SelectState = SelectState.Hover;
                            SelectedNodes.Remove(hoveredNode);

                            SelectedNodesChangedHandler();
                            Invalidate();
                        }
                    }
                }

                // If left mouse button is down, no action is in progress and no node is under mouse cursor or left Alt key is down
                // we will start the action of selecting nodes at the scripting scene.
                else if (hoveredNodes.Count == 0 || Form.ModifierKeys == Keys.Alt)
                {
                    // activate this action
                    state = StateType.SelectingNodes;
                    // init 
                    selectingInitPosition = PointAtScene(e.Location);
                    selectingRectangle = new Rectangle();

                    Invalidate();
                }

                // If left mouse button is down, no action is in progress and some node is under mouse cursor 
                // we will start the action of moving selected nodes at the scripting scene.
                else
                {
                    // 1. If no nodes are selected we will select node under the mouse cursor.
                    // 2. If some nodes are selected but under mouse is not selected node 
                    // we will deselect the current selected nodes and select node under mouse cursor.
                    if (SelectedNodes.Count == 0 || !SelectedNodes.Contains(hoveredNodes[0]))
                    {
                        // select hovered nodes
                        SelectHoveredNodes();
                    }

                    // activate this action
                    state = StateType.MovingNodes;
                    // init 
                    lastPosition = e.Location;
                    wholeMovement = new Point();

                    Invalidate();
                }
            }
            // If right mouse button is down and no action is in progress
            // we will start changing the position of the scripting scene (moving scene).
            else if (e.Button == MouseButtons.Right && state == StateType.Default)
            {
                // activate this action
                state = StateType.MovingScene;
                // init
                lastPosition = e.Location;
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the ScriptingScreen control.
        /// </summary>
        private void ScriptingScreen_MouseUp(object sender, MouseEventArgs e)
        {
            // If left mouse button is up and the action of moving selected nodes at the scripting scene is active
            // we will finish it.
            if (e.Button == MouseButtons.Left && state == StateType.MovingNodes)
            {
                // add moving action to the history
                if ((wholeMovement.X != 0 || wholeMovement.Y != 0) && SelectedNodes.Count != 0)
                {
                    CompositeCommand command = new CompositeCommand();

                    foreach (SceneNode selectedNode in SelectedNodes)
                    {
                        if (selectedNode.CanMove) command.Commands.Add(new SceneNodeMoveCommand(selectedNode, wholeMovement));
                    }

                    if (command.Commands.Count != 0) History.Add(command);
                }

                // this action is over
                state = StateType.Default;
            }

            // If left mouse button is up and the action of selecting nodes at the scripting scene is active
            // we will finish it.
            else if (e.Button == MouseButtons.Left && state == StateType.SelectingNodes)
            {
                // select hovered nodes
                SelectHoveredNodes();

                // this action is over
                state = StateType.Default;

                Invalidate();
            }

            // If left mouse button is up and the action of connecting node sockets at the scripting scene is active
            // we will finish it.
            else if (e.Button == MouseButtons.Left && state == StateType.ConnectingNodes)
            {
                ConnectionView connection = null;

                if (hoveredSocket != null && hoveredSocket != selectedSocket)
                {
                    connection = selectedSocket.MakeConnection(hoveredSocket);
                }
                else if (hoveredNodes.Count == 1)
                {
                    connection = hoveredNodes[0].IConnecting.MakeConnection(selectedSocket);
                }

                if (connection != null)
                {
                    // add connection to the scene
                    Nodes.Add(connection);
                    connection.OnSceneNodeAdded();

                    // add connection to the history
                    History.Add(new AddSceneNodeCommand(this, connection));
                }

                // this action is over
                state = StateType.Default;

                Invalidate();
            }

            // If right mouse button is up and the action of changing the position of the scripting scene is active
            // we will finish it.
            else if (e.Button == MouseButtons.Right && state == StateType.MovingScene)
            {
                // this action is over
                state = StateType.Default;
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the ScriptingScreen control.
        /// </summary>
        private void ScriptingScreen_MouseMove(object sender, MouseEventArgs e)
        {
            // select control if not
            if (!Focused) Select();

            // update mouse scene position
            _mouseScenePosition = PointAtScene(e.Location);

            // If the action of selecting objects at the scripting scene is active 
            // we will update selecting rectangle and hover nodes within selecting rectangle.
            if (state == StateType.SelectingNodes)
            {
                // update selecting rectangle
                selectingRectangle = selectingInitPosition.ToRectangle(MouseScenePosition);

                // clear last hovered nodes
                ClearHoveredNodes();

                // set new hovered nodes within selecting rectangle
                foreach (SceneNode node in Nodes)
                {
                    // Is node within selecting rectangle?
                    if (node.IntersectsWith(ref selectingRectangle))
                    {
                        // set as hovered
                        node.SelectState = SelectState.Hover;
                        hoveredNodes.Add(node);
                    }
                }

                Invalidate();
            }

            // If the action of moving selected nodes at the scripting scene is active
            // we will change the location of selected nodes by distance we moved from the last changing.
            else if (state == StateType.MovingNodes)
            {
                // calculate move vector for selected nodes at the scene
                PointF moveVector = e.Location.Sub(lastPosition).Mul(ScaleInversFactor);

                // change the location of every selected nodes at the scene
                foreach (SceneNode selectedNode in SelectedNodes)
                {
                    if (selectedNode.CanMove)
                    {
                        selectedNode.Location = selectedNode.Location.Add(moveVector);
                    }
                }

                // set actual position for the next changing
                lastPosition = e.Location;
                wholeMovement = wholeMovement.Add(moveVector);

                Invalidate();
            }

            // If the action of changing the position of the scripting scene is active
            // we will change the position of the scene by distance we moved from the last changing.
            else if (state == StateType.MovingScene)
            {
                // change the position of the scene
                Position = Position.Add(lastPosition.Sub(e.Location).Mul(ScaleInversFactor));
                // set actual position for the next changing
                lastPosition = e.Location;

                Invalidate();
            }

            // If no action is in progress we will hover an node or node socket at the scene under mouse cursor.
            else if (state == StateType.Default || state == StateType.ConnectingNodes)
            {
                Debug.Assert(hoveredNodes.Count <= 1, "Selecting state assume that only one node can be hovered (if action is not in progress).");

                SceneNode node = null;
                NodeSocketView socket = null;

                // find node or node socket at the scripting scene under mouse cursor
                bool endSearching = false;
                for (int i = Nodes.Count - 1; i >= 0; --i)
                {
                    // find node
                    if (Nodes[i].Contains(MouseScenePosition))
                    {
                        node = Nodes[i];
                        endSearching = true;
                    }

                    // find node socket
                    if (Nodes[i].IsSocketsContainer && (socket = Nodes[i].INodeSocketsContainer.NodeSocketContains(MouseScenePosition)) != null)
                    {
                        endSearching = true;
                    }

                    // end searching after we found one
                    if (endSearching) break;
                }

                // change hovered node only when different node is under mouse cursor
                if (node != null && (hoveredNodes.Count == 0 || (hoveredNodes.Count == 1 && hoveredNodes[0] != node)))
                {
                    // clear previous hovered node
                    ClearHoveredNodes();

                    // set as hovered
                    if (state == StateType.Default || (state == StateType.ConnectingNodes && node.CanConnect && node.IConnecting.Connection(selectedSocket)))
                    {
                        if (node.SelectState != SelectState.Select) node.SelectState = SelectState.Hover;
                        hoveredNodes.Add(node);
                    }

                    Invalidate();
                }
                // no node is under mouse cursor but we must clear previous hovered nodes 
                else if (node == null && hoveredNodes.Count != 0)
                {
                    // clear previous hovered object
                    ClearHoveredNodes();

                    Invalidate();
                }

                // change hovered node socket only when different node socket is under mouse cursor
                if (socket != null && hoveredSocket != socket)
                {
                    // clear previous hovered node socket
                    if (hoveredSocket != null) hoveredSocket.SelectState = SelectState.Default;

                    // set as hovered
                    if (state == StateType.Default || (state == StateType.ConnectingNodes && selectedSocket.Connection(socket)))
                    {
                        socket.SelectState = SelectState.Hover;
                        hoveredSocket = socket;
                    }

                    Invalidate();
                }
                // no node socket is under mouse cursor but we must clear previous hovered node socket
                else if (socket == null && hoveredSocket != null)
                {
                    // clear previous hovered node socket
                    hoveredSocket.SelectState = SelectState.Default;
                    hoveredSocket = null;

                    Invalidate();
                }

                // If the action of connecting nodes is active
                // we must invalidate scripting scene every time when mouse moves.
                // (Because of painting connecting line.)
                if (state == StateType.ConnectingNodes)
                {
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Handles the MouseWheel event of the ScriptingScreen control.
        /// </summary>
        private void ScriptingScreen_MouseWheel(object sender, MouseEventArgs e)
        {
            // actual position under mouse cursor
            PointF actualPosition = MouseScenePosition;

            // zoom the scene
            Zoom += e.Delta / 30;

            // new positon under mouse cursor
            PointF changedPosition = PointAtScene(e.Location);

            // We want to keep the same position under mouse cursor before and after changing zoom of the scene
            // so we change the position of the scene by difference between before and after position under mouse cursor.
            Position = Position.Add(actualPosition.Sub(changedPosition));

            // update mouse scene position
            _mouseScenePosition = PointAtScene(e.Location);

            Invalidate();
        }

        /// <summary>
        /// Handles the DragEnter event of the ScriptingScreen control.
        /// Allows script node to be dragged onto the ScriptingScreen control.
        /// </summary>
        private void ScriptingScreen_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(BaseNode))) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Handles the DragDrop event of the ScriptingScreen control.
        /// If script node is dropped to the scene the script node is cloned and added to the scene.
        /// </summary>
        private void ScriptingScreen_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(BaseNode));

            if (data != null)
            {
                UnderlyingCollectionChanging = true;

                // create scene node for using at the scripting scene
                SceneNode newNode = (((BaseNode)data).Clone(State)).CreateView();

                newNode.SceneControl = this;
                newNode.Location = PointAtScene(PointToClient(new Point(e.X, e.Y)));

                // add to the scripting scene
                Nodes.Add(newNode);
                newNode.OnSceneNodeAdded();

                // add adding command to the history
                History.Add(new AddSceneNodeCommand(this, newNode));

                UnderlyingCollectionChanging = false;

                Invalidate();
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the ScriptingScreen control.
        /// </summary>
        private void ScriptingScreen_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + Z => Undo history
            if (e.Control && e.KeyCode == Keys.Z)
            {
                UnderlyingCollectionChanging = true;
                History.Undo();
                UnderlyingCollectionChanging = false;
                Invalidate();
            }

            // Ctrl + Y => Redo history
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                UnderlyingCollectionChanging = true;
                History.Redo();
                UnderlyingCollectionChanging = false;
                Invalidate();
            }

            // Ctrl + C => copy selected nodes at the scripting scene to the clipboard
            else if (e.Control && e.KeyCode == Keys.C)
            {
                // clear the current clipboard
                Clipboard.Clear();
                // rectangle of selected nodes for calculate clipboard position
                RectangleF? selectedNodesArea = null;

                // add selected nodes at the scene to the clipboard
                foreach (SceneNode selectedNode in SelectedNodes)
                {
                    Clipboard.Add(selectedNode);

                    if (selectedNode.CanClone)
                    {
                        if (selectedNodesArea == null) selectedNodesArea = selectedNode.Bounds;
                        else selectedNodesArea = RectangleF.Union(selectedNodesArea.Value, selectedNode.Bounds);
                    }
                }

                // calculate clipboard position 
                if (selectedNodesArea != null) clipboardPosition = new PointF(selectedNodesArea.Value.X + selectedNodesArea.Value.Width / 2, selectedNodesArea.Value.Y + selectedNodesArea.Value.Height / 2);
            }

            // Ctrl + V => paste nodes from the clipboard to the scripting scene
            else if (e.Control && e.KeyCode == Keys.V && Clipboard.Count != 0)
            {
                UnderlyingCollectionChanging = true;

                // calculate move vector for nodes at the clipboard
                PointF moveVector = MouseScenePosition.Sub(clipboardPosition);
                // prepare command for history
                CompositeCommand command = new CompositeCommand();

                Dictionary<SceneNode, SceneNode> clonedNodes = new Dictionary<SceneNode, SceneNode>();

                // paste all nodes except connections from the clipboard to the scripting scene
                foreach (SceneNode node in Clipboard)
                {
                    if (!(node is ConnectionView) && node.CanClone)
                    {
                        // copy node
                        SceneNode copiedNode = node.Clone();

                        copiedNode.SceneControl = this;
                        // move node to the new location
                        copiedNode.Location = copiedNode.Location.Add(moveVector);

                        // add to the scene
                        Nodes.Add(copiedNode);
                        copiedNode.OnSceneNodeAdded();

                        clonedNodes[node] = copiedNode;

                        command.Commands.Add(new AddSceneNodeCommand(this, copiedNode));
                    }
                }

                // paste all connections from to clipboard to the scripting scene
                foreach (SceneNode node in Clipboard)
                {
                    ConnectionView connection = node as ConnectionView;
                    if (connection != null)
                    {
                        // connection can by only pasted when it has both parents (connectors) in the clipboard
                        if (Clipboard.Contains(connection.From.Parent) && Clipboard.Contains(connection.To.Parent))
                        {
                            IConnecting from, to;
                            NodeSocketView nodeSocketFrom = null, nodeSocketTo = null;

                            // connection from parent is sockets container => connection from is node socket
                            if (connection.From.Parent.IsSocketsContainer)
                            {
                                nodeSocketFrom = (NodeSocketView)connection.From;
                                // get the same node socket from the cloned node
                                from = clonedNodes[(SceneNode)connection.From.Parent].INodeSocketsContainer.GetSocketByName(nodeSocketFrom.NodeSocket.NodeSocketData.RealName, nodeSocketFrom.Type);
                            }
                            else
                            {
                                from = (IConnecting)clonedNodes[(SceneNode)connection.From];
                            }

                            // connection to parent is sockets container => connection to is node socket
                            if (connection.To.Parent.IsSocketsContainer)
                            {
                                nodeSocketTo = (NodeSocketView)connection.To;
                                // get the same node socket from the cloned node
                                to = clonedNodes[(SceneNode)connection.To.Parent].INodeSocketsContainer.GetSocketByName(nodeSocketTo.NodeSocket.NodeSocketData.RealName, nodeSocketTo.Type);
                            }
                            else
                            {
                                to = (IConnecting)clonedNodes[(SceneNode)connection.To];
                            }

                            // create connection view
                            ConnectionView copiedConnection = null;
                            if (nodeSocketFrom != null)
                            {
                                copiedConnection = to.MakeConnection((NodeSocketView)from);
                            }
                            if (copiedConnection == null && nodeSocketTo != null)
                            {
                                copiedConnection = from.MakeConnection((NodeSocketView)to);
                            }

                            Debug.Assert(copiedConnection != null, "Connection must be copied because it has both parents in the clipboard");

                            // add to the scene
                            Nodes.Add(copiedConnection);
                            copiedConnection.OnSceneNodeAdded();

                            command.Commands.Add(new AddSceneNodeCommand(this, copiedConnection));
                        }
                    }
                }

                // add pasting command to the history
                if (command.Commands.Count != 0) History.Add(command);

                UnderlyingCollectionChanging = false;

                Invalidate();
            }

            // Ctrl + X => delete selected nodes at the scripting scene and copy them to the clipboard
            else if (e.Control && e.KeyCode == Keys.X && SelectedNodes.Count != 0)
            {
                UnderlyingCollectionChanging = true;

                // clear the current clipboard
                Clipboard.Clear();
                // rectangle of selected nodes for calculate clipboard position
                RectangleF? selectedNodesArea = null;
                // prepare command for history
                CompositeCommand command = new CompositeCommand();

                // remove all selected nodes from the scripting screen and add them to the clipboard
                foreach (SceneNode selectedNode in SelectedNodes)
                {
                    // add to clipboard
                    Clipboard.Add(selectedNode);

                    if (selectedNode.CanClone)
                    {
                        if (selectedNodesArea == null) selectedNodesArea = selectedNode.Bounds;
                        else selectedNodesArea = RectangleF.Union(selectedNodesArea.Value, selectedNode.Bounds);
                    }

                    // delete commmand
                    DeleteSceneNodeCommand deleteCommand = new DeleteSceneNodeCommand(this, selectedNode);
                    deleteCommand.Do();
                    command.Commands.Add(deleteCommand);

                    // delete all node connections if any
                    if (selectedNode.IsSocketsContainer || selectedNode.CanConnect)
                    {
                        // get node connections
                        List<ConnectionView> connections;
                        if (selectedNode.IsSocketsContainer) connections = selectedNode.INodeSocketsContainer.GetAllConnections();
                        else connections = new List<ConnectionView>(selectedNode.IConnecting.Connections);

                        // remove all connections from the scripting screen
                        foreach (ConnectionView connection in connections)
                        {
                            // remove only if is not selected, otherwise it will be removed as selected node
                            if (!SelectedNodes.Contains(connection))
                            {
                                // delete command
                                deleteCommand = new DeleteSceneNodeCommand(this, connection);
                                deleteCommand.Do();
                                command.Commands.Add(deleteCommand);
                            }
                        }
                    }
                }
                ClearSelectedNodes();

                // calculate clipboard position 
                if (selectedNodesArea != null) clipboardPosition = new PointF(selectedNodesArea.Value.X + selectedNodesArea.Value.Width / 2, selectedNodesArea.Value.Y + selectedNodesArea.Value.Height / 2);

                History.Add(command);

                // selected nodes changed
                SelectedNodesChangedHandler();

                UnderlyingCollectionChanging = false;

                Invalidate();
            }

            // Delete key is down => delete selected nodes at the scripting scene
            else if (e.KeyCode == Keys.Delete && state == StateType.Default && SelectedNodes.Count != 0)
            {
                UnderlyingCollectionChanging = true;

                // prepare command for history
                CompositeCommand command = new CompositeCommand();

                // remove all selected nodes from the scripting screen
                foreach (SceneNode selectedNode in SelectedNodes)
                {
                    // delete commmand
                    DeleteSceneNodeCommand deleteCommand = new DeleteSceneNodeCommand(this, selectedNode);
                    deleteCommand.Do();
                    command.Commands.Add(deleteCommand);

                    // delete all node connections if any
                    if (selectedNode.IsSocketsContainer || selectedNode.CanConnect)
                    {
                        // get node connections
                        List<ConnectionView> connections;
                        if (selectedNode.IsSocketsContainer) connections = selectedNode.INodeSocketsContainer.GetAllConnections();
                        else connections = new List<ConnectionView>(selectedNode.IConnecting.Connections);

                        // remove all connections from the scripting screen
                        foreach (ConnectionView connection in connections)
                        {
                            // remove only if is not selected, otherwise it will be removed as selected node
                            if (!SelectedNodes.Contains(connection))
                            {
                                // delete command
                                deleteCommand = new DeleteSceneNodeCommand(this, connection);
                                deleteCommand.Do();
                                command.Commands.Add(deleteCommand);
                            }
                        }
                    }
                }
                ClearSelectedNodes();

                // add deleting command to the history
                History.Add(command);

                // selected nodes changed
                SelectedNodesChangedHandler();

                UnderlyingCollectionChanging = false;

                Invalidate();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                State = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Command for removing the scene node from the scripting screen.
        /// </summary>
        private class DeleteSceneNodeCommand : Command
        {
            private ScriptingScreen scriptingScreen;
            private SceneNode scriptSceneNode;

            /// <summary>
            /// Initializes a new instance of the <see cref="DeleteSceneNodeCommand"/> class.
            /// </summary>
            /// <param name="scriptingScreen">The scripting screen.</param>
            /// <param name="scriptSceneNode">The scene node to remove.</param>
            public DeleteSceneNodeCommand(ScriptingScreen scriptingScreen, SceneNode scriptSceneNode)
            {
                this.scriptingScreen = scriptingScreen;
                this.scriptSceneNode = scriptSceneNode;
            }

            /// <summary>
            /// Removes the scene node from the scripting scene.
            /// </summary>
            public override void Do()
            {
                scriptingScreen.Nodes.Remove(scriptSceneNode);
                scriptSceneNode.OnSceneNodeRemoved();
            }

            /// <summary>
            /// Adds the scene node to the scripting scene.
            /// </summary>
            public override void Undo()
            {
                scriptingScreen.Nodes.Add(scriptSceneNode);
                scriptSceneNode.OnSceneNodeAdded();
            }
        }

        /// <summary>
        /// Command for adding the scene node to the scripting screen.
        /// </summary>
        private class AddSceneNodeCommand : Command
        {
            private ScriptingScreen scriptingScreen;
            private SceneNode scriptSceneNode;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddSceneNodeCommand"/> class.
            /// </summary>
            /// <param name="scriptingScreen">The scripting screen.</param>
            /// <param name="scriptSceneNode">The scene node to add.</param>
            public AddSceneNodeCommand(ScriptingScreen scriptingScreen, SceneNode scriptSceneNode)
            {
                this.scriptingScreen = scriptingScreen;
                this.scriptSceneNode = scriptSceneNode;
            }

            /// <summary>
            /// Adds the scene node to the scripting scene.
            /// </summary>
            public override void Do()
            {
                scriptingScreen.Nodes.Add(scriptSceneNode);
                scriptSceneNode.OnSceneNodeAdded();
            }

            /// <summary>
            /// Removes the scene node from the scripting scene.
            /// </summary>
            public override void Undo()
            {
                scriptingScreen.Nodes.Remove(scriptSceneNode);
                scriptSceneNode.OnSceneNodeRemoved();
            }
        }
    }
}
