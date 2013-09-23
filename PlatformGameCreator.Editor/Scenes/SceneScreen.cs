/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Xna;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.Assets;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.GameObjects.Paths;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Screen for editing the <see cref="Scenes.Scene"/>.
    /// </summary>
    class SceneScreen : GraphicsDeviceControl
    {
        /// <summary>
        /// Gets or sets the underlying <see cref="Scenes.Scene"/>.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Scene Scene
        {
            get { return _scene; }
            set
            {
                if (_scene != value)
                {
                    Nodes.Clear();
                    Clipboard.Clear();
                    History.Clear();
                    SelectedNodes.Clear();
                    HoveredNodes.Clear();

                    InvokeSelectedNodesChanged();

                    if (_scene != null)
                    {
                        _scene.Layers.ListChanged -= new ObservableList<Layer>.ListChangedEventHandler(Layers_ListChanged);
                        _scene.Paths.ListChanged -= new ObservableList<Path>.ListChangedEventHandler(Paths_ListChanged);

                        foreach (Layer layer in _scene.Layers)
                        {
                            layer.ListChanged -= new ObservableList<Actor>.ListChangedEventHandler(Layer_ListChanged);
                        }
                    }

                    _scene = value;

                    if (_scene != null)
                    {
                        Zoom = 100;
                        Position = new Vector2();

                        _scene.Layers.ListChanged += new ObservableList<Layer>.ListChangedEventHandler(Layers_ListChanged);

                        foreach (Layer layer in _scene.Layers)
                        {
                            layer.ListChanged += new ObservableList<Actor>.ListChangedEventHandler(Layer_ListChanged);

                            foreach (Actor actor in layer)
                            {
                                Nodes.Add(actor);
                            }
                        }

                        _scene.Paths.ListChanged += new ObservableList<Path>.ListChangedEventHandler(Paths_ListChanged);

                        foreach (Path path in _scene.Paths)
                        {
                            Nodes.Add(path);
                        }

                        Fit();
                    }
                }
            }
        }
        private Scene _scene;

        /// <summary>
        /// Gets the scene nodes at the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SceneNodesList Nodes
        {
            get { return _nodes; }
        }
        private SceneNodesList _nodes;

        /// <summary>
        /// Gets or sets the position at the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdateTransformMatrix();
            }
        }
        private Vector2 _position = new Vector2();

        /// <summary>
        /// Gets or sets the zoom (10-1000) for the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    if (_zoom > 1000f) _zoom = 1000f;
                    else if (_zoom < 10f) _zoom = 10f;
                    UpdateTransformMatrix();
                    if (ZoomChanged != null) ZoomChanged(this, EventArgs.Empty);
                    UpdateSimulationUnitsMeasure();
                }
            }
        }
        private float _zoom = 100f;

        /// <summary>
        /// Gets the scale factor defined by the <see cref="Zoom"/>.
        /// </summary>
        public float ScaleFactor
        {
            get { return Zoom / 100f; }
        }

        /// <summary>
        /// Gets the invers scale factor defined by the <see cref="Zoom"/>.
        /// </summary>
        public float ScaleInversFactor
        {
            get { return 100f / Zoom; }
        }

        /// <summary>
        /// Gets the mouse position at the scene.
        /// </summary>
        public Vector2 MouseScenePosition
        {
            get { return _mouseScenePosition; }
        }
        private Vector2 _mouseScenePosition;

        /// <summary>
        /// Gets the selected scene nodes at the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SceneNode> SelectedNodes
        {
            get { return _selectedNodes; }
        }
        private List<SceneNode> _selectedNodes = new List<SceneNode>();

        /// <summary>
        /// Gets the hovered scene nodes at the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SceneNode> HoveredNodes
        {
            get { return _hoveredNodes; }
        }
        private List<SceneNode> _hoveredNodes = new List<SceneNode>();

        /// <summary>
        /// Gets the clipboard for copying and pasting scene nodes at the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SceneNode> Clipboard
        {
            get { return _clipboard; }
        }
        private List<SceneNode> _clipboard = new List<SceneNode>();

        /// <summary>
        /// Gets or sets the position at the scene under mouse cursor when copying scene nodes to the clipboard.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector2 ClipboardPosition { get; set; }

        /// <summary>
        /// Gets or sets the state of the scene.
        /// </summary>
        /// <remarks>
        /// Setting a new state:
        /// First checks if the current state can be interrupted, if not then 
        /// the new state cannot be set and calls <see cref="SceneState.OnTryInterrupt"/> on the current state.
        /// After the new state is set calls <see cref="SceneState.OnSet"/> method on the new state.
        /// </remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SceneState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    if (_state != null && !_state.CanBeInterrupted)
                    {
                        _state.OnTryInterrupt();
                        return;
                    }

                    _state = value;

                    if (_state != null)
                    {
                        _state.Screen = this;
                        _state.OnSet();
                    }
                }
            }
        }
        private SceneState _state;

        /// <summary>
        /// Gets the history of the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public History History
        {
            get { return _history; }
        }
        private History _history = new History();

        /// <summary>
        /// Gets or sets the size in pixels of the gap at grid. Default value is 40.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int GridGapSize
        {
            get { return _gridGapSize; }
            set
            {
                if (_gridGapSize != value)
                {
                    _gridGapSize = value;
                    if (_gridGapSize < 3) _gridGapSize = 3;
                }
            }
        }
        private int _gridGapSize = 40;

        /// <summary>
        /// Gets or sets a value indicating whether the grid is visible at the scene.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the shapes of the scene nodes are visible.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowShapes { get; set; }

        /// <summary>
        /// Occurs when the <see cref="Zoom"/> property value changes.
        /// </summary>
        public event EventHandler ZoomChanged;

        /// <summary>
        /// Occurs when the selected scene nodes at the scene changes.
        /// </summary>
        public event EventHandler SelectedNodesChanged;

        /// <summary>
        /// <see cref="SceneBatch"/> for drawing the scene.
        /// </summary>
        private SceneBatch sceneBatch;

        /// <summary>
        /// Image shown at the information box.
        /// </summary>
        private Texture2D updateIcon;

        /// <summary>
        /// Font for the scene.
        /// </summary>
        private SpriteFont font;

        // matrices for the scene
        private Matrix projection;
        private Matrix world;
        private Matrix worldInvers;
        private Matrix identity = Matrix.Identity;

        // information box drawing data
        private VertexPositionColor[] infoBoxRectangle = new VertexPositionColor[4];
        private VertexPositionColor[] infoBoxBorder = new VertexPositionColor[3];

        // simulation units arrow drawing datat
        private VertexPositionColor[] simulationUnitsLine = new VertexPositionColor[2];
        private VertexPositionColor[] simulationUnitsArrows = new VertexPositionColor[6];

        /// <summary>
        /// Initializes <see cref="SceneScreen"/>.
        /// </summary>
        protected override void Initialize()
        {
            _nodes = new SceneNodesList(this);

            AllowDrop = true;

            font = Content.Load<SpriteFont>("EditorFont");
            updateIcon = Content.Load<Texture2D>("UpdateIcon");

            MouseMove += new MouseEventHandler(SceneScreenControl_MouseMove);
            MouseUp += new MouseEventHandler(SceneScreenControl_MouseUp);
            MouseDown += new MouseEventHandler(SceneScreenControl_MouseDown);
            MouseWheel += new MouseEventHandler(SceneScreenControl_MouseWheel);

            KeyDown += new KeyEventHandler(SceneScreenControl_KeyDown);
            KeyUp += new KeyEventHandler(SceneScreenControl_KeyUp);
            KeyPress += new KeyPressEventHandler(SceneScreenControl_KeyPress);

            DragEnter += new DragEventHandler(SceneScreen_DragEnter);
            DragDrop += new DragEventHandler(SceneScreenControl_DragDrop);

            Resize += new EventHandler(SceneScreen_Resize);

            // set moving state as default state
            State = new MovingNodesSceneState() { Screen = this };

            sceneBatch = new SceneBatch(GraphicsDevice);

            RenderCircle.Init(GraphicsDevice, Content);

            SceneScreen_Resize(null, null);

            Position = new Vector2();
            Zoom = 100;
        }

        /// <summary>
        /// Handles the Resize event of the SceneScreen control.
        /// Updates drawing data for information box and simulation units arrow.
        /// </summary>
        private void SceneScreen_Resize(object sender, EventArgs e)
        {
            // update projection matrix
            projection = Matrix.CreateOrthographicOffCenter(0, Width, Height, 0, 0, 1);

            // update info box rectangle
            infoBoxRectangle[0] = new VertexPositionColor(new Vector3(Width - SizeSettings.InfoBoxSize.X, 0, 0), ColorSettings.InfoBoxBackgroundColor);
            infoBoxRectangle[1] = new VertexPositionColor(new Vector3(Width, 0, 0), ColorSettings.InfoBoxBackgroundColor);
            infoBoxRectangle[2] = new VertexPositionColor(new Vector3(Width - SizeSettings.InfoBoxSize.X, SizeSettings.InfoBoxSize.Y, 0), ColorSettings.InfoBoxBackgroundColor);
            infoBoxRectangle[3] = new VertexPositionColor(new Vector3(Width, SizeSettings.InfoBoxSize.Y, 0), ColorSettings.InfoBoxBackgroundColor);

            // update info box border
            infoBoxBorder[0] = new VertexPositionColor(new Vector3(Width - SizeSettings.InfoBoxSize.X, 0, 0), ColorSettings.InfoBoxBorderColor);
            infoBoxBorder[1] = new VertexPositionColor(new Vector3(Width - SizeSettings.InfoBoxSize.X, SizeSettings.InfoBoxSize.Y, 0), ColorSettings.InfoBoxBorderColor);
            infoBoxBorder[2] = new VertexPositionColor(new Vector3(Width, SizeSettings.InfoBoxSize.Y, 0), ColorSettings.InfoBoxBorderColor);

            UpdateSimulationUnitsMeasure();
        }

        /// <summary>
        /// Updates drawing data for the simulation units measure.
        /// </summary>
        public void UpdateSimulationUnitsMeasure()
        {
            Color simulationUnitsColor = ColorSettings.SimulationUnitsColor;
            float simulationUnitsArrowWidth = SizeSettings.SimulationUnitsArrowWidth;
            float simulationUnitsArrowHalfHeight = SizeSettings.SimulationUnitsArrowHalfHeight;

            Vector3 start = new Vector3(10, Height - 10, 0f);
            Vector3 end = new Vector3(10 + (Project.Singleton != null ? Project.Singleton.Settings.SimulationUnits : 100f) * ScaleFactor, Height - 10, 0f);

            // line
            simulationUnitsLine[0] = new VertexPositionColor(start, simulationUnitsColor);
            simulationUnitsLine[1] = new VertexPositionColor(end, simulationUnitsColor);

            // left arrow
            simulationUnitsArrows[0] = new VertexPositionColor(new Vector3(start.X + simulationUnitsArrowWidth, start.Y - simulationUnitsArrowHalfHeight, 0f), simulationUnitsColor);
            simulationUnitsArrows[1] = new VertexPositionColor(new Vector3(start.X + simulationUnitsArrowWidth, start.Y + simulationUnitsArrowHalfHeight, 0f), simulationUnitsColor);
            simulationUnitsArrows[2] = simulationUnitsLine[0];

            // right arrow
            simulationUnitsArrows[3] = new VertexPositionColor(new Vector3(end.X - simulationUnitsArrowWidth, end.Y - simulationUnitsArrowHalfHeight, 0f), simulationUnitsColor);
            simulationUnitsArrows[4] = simulationUnitsLine[1];
            simulationUnitsArrows[5] = new VertexPositionColor(new Vector3(end.X - simulationUnitsArrowWidth, end.Y + simulationUnitsArrowHalfHeight, 0f), simulationUnitsColor);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Catchs arrow keys by the control.
        /// </remarks>
        protected override bool IsInputKey(Keys keyData)
        {
            // we want arrows to be catch by control
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.KeyDown"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_KeyDown(object sender, KeyEventArgs e)
        {
            // process by current state
            State.KeyDown(sender, e);
        }

        /// <summary>
        /// Handles the KeyUp event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.KeyUp"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_KeyUp(object sender, KeyEventArgs e)
        {
            // process by current state
            State.KeyUp(sender, e);
        }

        /// <summary>
        /// Handles the KeyPress event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.KeyPress"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            // process by current state
            State.KeyPress(sender, e);
        }

        /// <summary>
        /// Handles the DragEnter event of the SceneScreen control.
        /// Allows drawable asset or actor to be dragged onto the SceneScreen control.
        /// </summary>
        private void SceneScreen_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DrawableAsset)) || e.Data.GetDataPresent(typeof(Actor))) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Handles the DragDrop event of the SceneScreenControl control.
        /// If drawable asset is dropped to the scene new actor with the specified drawable asset is created and added to the scene.
        /// If prototype (actor) is dropped to the scene the prototype is cloned and added to the scene.
        /// </summary>
        private void SceneScreenControl_DragDrop(object sender, DragEventArgs e)
        {
            Vector2 position = PointAtScene(PointToClient(new System.Drawing.Point(e.X, e.Y)));

            if (e.Data.GetDataPresent(typeof(DrawableAsset)))
            {
                DrawableAsset drawableAsset = (DrawableAsset)e.Data.GetData(typeof(DrawableAsset));

                if (drawableAsset != null)
                {
                    if (Scene.SelectedLayer == null)
                    {
                        Messages.ShowWarning("No layer selected.");
                        return;
                    }

                    // create new actor
                    Actor newActor = new Actor(drawableAsset, position);

                    // add to the scene selected layer
                    Scene.SelectedLayer.Add(newActor);

                    // add dropped objects to the history
                    History.Add(newActor.CreateAddCommand());
                }
            }
            else if (e.Data.GetDataPresent(typeof(Actor)))
            {
                Actor actor = (Actor)e.Data.GetData(typeof(Actor));

                if (actor != null)
                {
                    if (Scene.SelectedLayer == null)
                    {
                        Messages.ShowWarning("No layer selected.");
                        return;
                    }

                    // create new actor
                    Actor newActor = (Actor)CloningHelper.Clone(actor);
                    newActor.Position = position;

                    // add to the scene selected layer
                    Scene.SelectedLayer.Add(newActor);

                    // add dropped objects to the history
                    History.Add(newActor.CreateAddCommand());
                }
            }
        }

        /// <summary>
        /// Invokes the <see cref="SelectedNodesChanged"/> event.
        /// </summary>
        public void InvokeSelectedNodesChanged()
        {
            if (SelectedNodesChanged != null) SelectedNodesChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the transform matrices for the scene.
        /// </summary>
        private void UpdateTransformMatrix()
        {
            world = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) * Matrix.CreateScale(Zoom / 100f, Zoom / 100f, 0f);

            worldInvers = Matrix.CreateScale(100f / Zoom, 100f / Zoom, 0f) * Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }

        /// <summary>
        /// Converts the specified point from control to scene coordinates.
        /// </summary>
        /// <param name="mousePoint">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the scene coordinates.</returns>
        public Vector2 PointAtScene(System.Drawing.Point mousePoint)
        {
            return Vector2.Transform(new Vector2(mousePoint.X, mousePoint.Y), worldInvers);
        }

        /// <summary>
        /// Converts the specified point from control to scene coordinates.
        /// </summary>
        /// <param name="mousePoint">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the scene coordinates.</returns>
        public Vector2 PointAtScene(System.Drawing.PointF mousePoint)
        {
            return Vector2.Transform(new Vector2(mousePoint.X, mousePoint.Y), worldInvers);
        }

        /// <summary>
        /// Handles the MouseWheel event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.MouseWheel"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_MouseWheel(object sender, MouseEventArgs e)
        {
            // process by current state
            State.MouseWheel(sender, e);
        }

        /// <summary>
        /// Handles the MouseDown event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.MouseDown"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_MouseDown(object sender, MouseEventArgs e)
        {
            // process by current state
            State.MouseDown(sender, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.MouseUp"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_MouseUp(object sender, MouseEventArgs e)
        {
            // process by current state
            State.MouseUp(sender, e);
        }

        /// <summary>
        /// Handles the MouseMove event of the SceneScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="SceneState.MouseMove"/> method) of the SceneScreen control.
        /// </summary>
        private void SceneScreenControl_MouseMove(object sender, MouseEventArgs e)
        {
            // select control if not
            if (!Focused) Select();
            // update position at the scene under mouse cursor
            _mouseScenePosition = PointAtScene(e.Location);

            // process by current state
            State.MouseMove(sender, e);
        }

        /// <summary>
        /// Handles the ListChanged event of the <see cref="Scenes.Scene.Layers"/> at the scene.
        /// Called when the list of layers changes (the layer is added or removed). Adds or removes the actor from the layer.
        /// </summary>
        private void Layers_ListChanged(object sender, ObservableListChangedEventArgs<Layer> e)
        {
            IList<Layer> layers = sender as IList<Layer>;
            if (layers != null)
            {
                switch (e.ListChangedType)
                {
                    case ObservableListChangedType.ItemAdded:
                        e.Item.ListChanged += new ObservableList<Actor>.ListChangedEventHandler(Layer_ListChanged);

                        foreach (Actor actor in e.Item)
                        {
                            Nodes.Add(actor);
                        }

                        break;

                    case ObservableListChangedType.ItemDeleted:
                        e.Item.ListChanged -= new ObservableList<Actor>.ListChangedEventHandler(Layer_ListChanged);

                        foreach (Actor actor in e.Item)
                        {
                            Nodes.Remove(actor);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Handles the ListChanged event of the <see cref="Scenes.Scene.Paths"/> at the scene.
        /// Called when the list of paths changes (the path is added or removed).
        /// </summary>
        private void Paths_ListChanged(object sender, ObservableListChangedEventArgs<Path> e)
        {
            IList<Path> paths = sender as IList<Path>;
            if (paths != null)
            {
                BasicListChanged(paths, e);
            }
        }

        /// <summary>
        /// Handles the ListChanged event of the <see cref="Layer"/>.
        /// Called when the layer changes (the actor is added or removed).
        /// </summary>
        private void Layer_ListChanged(object sender, ObservableListChangedEventArgs<Actor> e)
        {
            Layer layer = sender as Layer;
            if (layer != null)
            {
                BasicListChanged(layer, e);
            }
        }

        /// <summary>
        /// Updates the specified <see cref="GameObject"/> when the container changes (<see cref="GameObject"/> is added or removed).
        /// Added or removed <see cref="SceneNode"/> for the specified <see cref="GameObject"/>.
        /// </summary>
        /// <typeparam name="GameObjectType">The type of <see cref="GameObject"/> stored in the container.</typeparam>
        private void BasicListChanged<GameObjectType>(IList<GameObjectType> container, ObservableListChangedEventArgs<GameObjectType> e) where GameObjectType : GameObject
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    Nodes.Add(e.Item);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    Nodes.Remove(e.Item);
                    break;

                case ObservableListChangedType.Reset:
                    break;
            }
        }

        /// <summary>
        /// Called when the underlyings collections changes from the outside.
        /// Clears <see cref="History"/> and <see cref="Clipboard"/>.
        /// </summary>
        public void UnderlyingCollectionChange()
        {
            History.Clear();
            Clipboard.Clear();
        }

        /// <summary>
        /// Gets all scene nodes including their children from the scene.
        /// </summary>
        /// <param name="onlyVisible">If set to <c>true</c> only visible scene nodes are returned.</param>
        private IEnumerable<SceneNode> GetAllSceneNodes(bool onlyVisible = false)
        {
            return GetAllSceneNodes(Nodes, onlyVisible);
        }

        /// <summary>
        /// Gets all scene nodes including their children from the specified <paramref name="sceneNodesList"/>.
        /// </summary>
        /// <param name="sceneNodesList">List of scene nodes to get values from.</param>
        /// <param name="onlyVisible">If set to <c>true</c> only visible scene nodes are returned.</param>
        private IEnumerable<SceneNode> GetAllSceneNodes(IList<SceneNode> sceneNodesList, bool onlyVisible = false)
        {
            for (int i = 0; i < sceneNodesList.Count; ++i)
            {
                // only visible scene node
                if (onlyVisible && !sceneNodesList[i].Visible) continue;

                foreach (SceneNode sceneNodeChild in GetAllSceneNodes(sceneNodesList[i].Children, onlyVisible))
                {
                    yield return sceneNodeChild;
                }

                yield return sceneNodesList[i];
            }
        }

        /// <summary>
        /// Gets all scene nodes including their children in reverse order from the scene.
        /// </summary>
        /// <param name="onlyVisible">If set to <c>true</c> only visible scene nodes are returned.</param>
        private IEnumerable<SceneNode> GetAllSceneNodesReverse(bool onlyVisible = false)
        {
            return GetAllSceneNodesReverse(Nodes, onlyVisible);
        }

        /// <summary>
        /// Gets all scene nodes including their children in reverse order from the specified <paramref name="sceneNodesList"/>.
        /// </summary>
        /// <param name="sceneNodesList">List of scene nodes to get values from.</param>
        /// <param name="onlyVisible">If set to <c>true</c> only visible scene nodes are returned.</param>
        private IEnumerable<SceneNode> GetAllSceneNodesReverse(IList<SceneNode> sceneNodesList, bool onlyVisible = false)
        {
            for (int i = sceneNodesList.Count - 1; i >= 0; --i)
            {
                // only visible scene node
                if (onlyVisible && !sceneNodesList[i].Visible) continue;

                yield return sceneNodesList[i];

                foreach (SceneNode sceneNodeChild in GetAllSceneNodesReverse(sceneNodesList[i].Children, onlyVisible))
                {
                    yield return sceneNodeChild;
                }
            }
        }

        /// <summary>
        /// Finds the <see cref="SceneNode"/> for the specified <see cref="GameObject"/> at the scene.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to find.</param>
        /// <returns><see cref="SceneNode"/> of the <see cref="GameObject"/> if found; otherwise <c>null</c>.</returns>
        public SceneNode Find(GameObject gameObject)
        {
            foreach (SceneNode sceneNode in GetAllSceneNodes())
            {
                if (sceneNode.Tag == gameObject)
                {
                    return sceneNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the first <see cref="SceneNode"/> at the specified position.
        /// </summary>
        /// <param name="point">The point where to find.</param>
        /// <returns><see cref="SceneNode"/> at the position if found; otherwise <c>null</c>.</returns>
        public SceneNode FindAt(Vector2 point)
        {
            foreach (SceneNode node in GetAllSceneNodes())
            {
                if (node.Visible && node.Rectangle.Contains(ref point) && node.Contains(point))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds all <see cref="SceneNode"/> at the specified rectangle defined by two points.
        /// </summary>
        /// <param name="firstPoint">The first point of the rectangle.</param>
        /// <param name="secondPoint">The second point of the retangle.</param>
        /// <returns>List of found scene nodes.</returns>
        public List<SceneNode> FindAt(Vector2 firstPoint, Vector2 secondPoint)
        {
            List<SceneNode> foundNodes = new List<SceneNode>();

            Vector2 min = new Vector2(Math.Min(firstPoint.X, secondPoint.X), Math.Min(firstPoint.Y, secondPoint.Y));
            Vector2 max = new Vector2(Math.Max(firstPoint.X, secondPoint.X), Math.Max(firstPoint.Y, secondPoint.Y));

            FarseerPhysics.Collision.AABB rectangle = new FarseerPhysics.Collision.AABB(min, max);

            foreach (SceneNode node in GetAllSceneNodes())
            {
                if (node.Visible && FarseerPhysics.Collision.AABB.TestOverlap(node.Rectangle, rectangle))
                {
                    foundNodes.Add(node);
                }
            }

            return foundNodes;
        }

        /// <summary>
        /// Called when the <see cref="SceneScreen"/> needs to be drawn.
        /// </summary>
        /// <remarks>
        /// Draws in this order:
        /// <list type="bullet">
        /// <item><description>background</description></item>
        /// <item><description>grid, if visible</description></item>
        /// <item><description>visible scene nodes</description></item>
        /// <item><description>shapes of visible scene nodes, if visible</description></item>
        /// <item><description>hands the drawing over to the <see cref="State"/> (<see cref="SceneState.Draw"/> method) of the SceneScreen control</description></item>
        /// <item><description>information box at the upper right corner, information about the position under the mouse at the bottom right corner and 
        /// simulation units measure information at the bottom left corner</description></item>
        /// </list>
        /// </remarks>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            // clear with background color
            GraphicsDevice.Clear(ColorSettings.BackgroundColor);

            sceneBatch.SceneSize = new Vector2(Width * ScaleInversFactor, Height * ScaleInversFactor);
            sceneBatch.ScenePosition = Position;

            // draw grid
            if (ShowGrid)
            {
                sceneBatch.Begin(ref projection, ref identity, ref world, ScaleFactor, ScaleInversFactor, ref gameTime);

                float gridGapSize = GridGapSize * ScaleInversFactor;
                float sceneWidth = Width * ScaleInversFactor;
                float sceneHeight = Height * ScaleInversFactor;

                for (float i = -Position.Y % gridGapSize; i <= sceneHeight; i += gridGapSize)
                {
                    sceneBatch.DrawLine(new Vector3(Position.X, Position.Y + i, 0f), new Vector3(Position.X + sceneWidth, Position.Y + i, 0f), ColorSettings.GridLineColor);
                }

                for (float i = -Position.X % gridGapSize; i <= sceneWidth; i += gridGapSize)
                {
                    sceneBatch.DrawLine(new Vector3(Position.X + i, Position.Y, 0f), new Vector3(Position.X + i, Position.Y + sceneHeight, 0f), ColorSettings.GridLineColor);
                }

                sceneBatch.End();
            }

            // draw scene (game objects)
            sceneBatch.Begin(ref projection, ref identity, ref world, ScaleFactor, ScaleInversFactor, ref gameTime);

            foreach (SceneNode node in GetAllSceneNodesReverse(true))
            {
                node.Draw(sceneBatch);
            }

            sceneBatch.End();

            // draw scene (game objects) shapes
            if (ShowShapes)
            {
                sceneBatch.Begin(ref projection, ref identity, ref world, ScaleFactor, ScaleInversFactor, ref gameTime);

                foreach (SceneNode node in GetAllSceneNodesReverse(true))
                {
                    node.DrawShapes(sceneBatch);
                }

                sceneBatch.End();
            }

            State.Draw(sceneBatch);

            // draw front (like hud)
            sceneBatch.Begin(ref projection, ref identity, ref identity, ScaleFactor, ScaleInversFactor, ref gameTime);

            // draw info box background and border
            sceneBatch.ApplyBasicEffect();
            sceneBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, infoBoxRectangle, 0, 2);
            sceneBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, infoBoxBorder, 0, 2);

            // draw simulation units measure
            sceneBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, simulationUnitsLine, 0, 1);
            sceneBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, simulationUnitsArrows, 0, 2);

            // draw rotating update icon
            sceneBatch.Apply2D();
            sceneBatch.Draw(updateIcon, new Vector2(Width - 90, 18), null, Color.White, (float)lastTime.TotalSeconds * 2f, new Vector2(updateIcon.Width / 2f, updateIcon.Height / 2f), 1f, SpriteEffects.None, 0f);

            // draw fps text
            sceneBatch.DrawString(font, String.Format("{0,9} FPS", Fps), new Vector2(Width - 80, 10), ColorSettings.InfoBoxTextColor);

            // draw mouse scene position in simulation units
            sceneBatch.DrawString(font, String.Format("X: {0,-6:0.###}", GameEngine.ConvertUnits.ToSimUnits(MouseScenePosition.X)), new Vector2(Width - 70, Height - 40), ColorSettings.InfoBoxTextColor);
            sceneBatch.DrawString(font, String.Format("Y: {0,-6:0.###}", GameEngine.ConvertUnits.ToSimUnits(MouseScenePosition.Y)), new Vector2(Width - 70, Height - 20), ColorSettings.InfoBoxTextColor);

            // draw simulation units text
            sceneBatch.DrawString(font, "1 meter", new Vector2(10, Height - 30), ColorSettings.InfoBoxTextColor);

            sceneBatch.End();
        }

        /// <summary>
        /// Called when the <see cref="SceneScreen"/> needs to be updated.
        /// Hands keyboard state over to the <see cref="State"/> (<see cref="SceneState.KeyboardState"/> method) of the SceneScreen control.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Focused)
            {
                Microsoft.Xna.Framework.Input.KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

                State.KeyboardState(gameTime, ref keyboardState);
            }
        }

        /// <summary>
        /// Zooms and centers the scene to be able to see the whole scene at the <see cref="SceneScreen"/>, if possible.
        /// </summary>
        /// <remarks>
        /// Does not work for the actors at the parallax layer.
        /// </remarks>
        public void Fit()
        {
            if (Nodes.Count != 0)
            {
                bool starting = true;
                FarseerPhysics.Collision.AABB sceneRectangle = new FarseerPhysics.Collision.AABB();

                foreach (SceneNode sceneNode in GetAllSceneNodes(true))
                {
                    if (starting)
                    {
                        sceneRectangle = sceneNode.Rectangle;
                        starting = false;
                    }
                    else
                    {
                        FarseerPhysics.Collision.AABB tempSceneNodeRectangle = sceneNode.Rectangle;
                        sceneRectangle.Combine(ref tempSceneNodeRectangle);
                    }
                }

                // no visible scene node
                if (starting) return;

                float sceneRectangleWidth = sceneRectangle.UpperBound.X - sceneRectangle.LowerBound.X;
                float sceneRectangleHeight = sceneRectangle.UpperBound.Y - sceneRectangle.LowerBound.Y;

                float scaleWidth = Width / sceneRectangleWidth;
                float scaleHeight = Height / sceneRectangleHeight;
                float scale = scaleWidth < scaleHeight ? scaleWidth : scaleHeight;

                Zoom = scale * 100f;

                Vector2 position = sceneRectangle.LowerBound;

                if (Width > sceneRectangleWidth * scale) position.X -= (Width - sceneRectangleWidth * scale) / 2f * ScaleInversFactor;
                else position.X -= (sceneRectangleWidth * scale - Width) / 2f * ScaleInversFactor;
                if (Height > sceneRectangleHeight * scale) position.Y -= (Height - sceneRectangleHeight * scale) / 2f * ScaleInversFactor;
                else position.Y -= (sceneRectangleHeight * scale - Height) / 2f * ScaleInversFactor;

                Position = position;
            }
        }

        /// <summary>
        /// Centers the specified game object at the <see cref="SceneScreen"/>.
        /// </summary>
        /// <param name="gameObject">The game object to center.</param>
        public void Center(GameObject gameObject)
        {
            SceneNode sceneNode = Find(gameObject);
            if (sceneNode != null)
            {
                bool onParallaxLayer = false;
                if (gameObject is Actor && ((Actor)gameObject).Layer != null && ((Actor)gameObject).Layer.IsParallax)
                {
                    onParallaxLayer = true;
                    Position = new Vector2();
                    Zoom = 100;
                }

                float sceneNodeWidth = sceneNode.Rectangle.UpperBound.X - sceneNode.Rectangle.LowerBound.X;
                float sceneNodeHeight = sceneNode.Rectangle.UpperBound.Y - sceneNode.Rectangle.LowerBound.Y;

                float scaleWidth = Width / sceneNodeWidth;
                float scaleHeight = Height / sceneNodeHeight;
                float scale = scaleWidth < scaleHeight ? scaleWidth : scaleHeight;
                if (scale >= 1f) scale = 1f;

                Zoom = scale * 100f;

                Vector2 position = sceneNode.Rectangle.LowerBound;

                if (onParallaxLayer)
                {
                    Vector2 antiParallaxMove = Vector2.One - ((Actor)gameObject).Layer.ParallaxCoefficient;
                    if (antiParallaxMove.X != 0f && antiParallaxMove.Y != 0f) position /= antiParallaxMove;
                }

                if (Width > sceneNodeWidth * scale) position.X -= (Width - sceneNodeWidth * scale) / 2f * ScaleInversFactor;
                else position.X -= (sceneNodeWidth * scale - Width) / 2f * ScaleInversFactor;
                if (Height > sceneNodeHeight * scale) position.Y -= (Height - sceneNodeHeight * scale) / 2f * ScaleInversFactor;
                else position.Y -= (sceneNodeHeight * scale - Height) / 2f * ScaleInversFactor;

                Position = position;
            }
        }
    }

    /// <summary>
    /// List for storing <see cref="SceneNode">scene nodes</see>. 
    /// Scene nodes are automatically sorted according to their <see cref="SceneNode.Index"/>.
    /// </summary>
    class SceneNodesList : GameEngine.SortedList<SceneNode>
    {
        /// <summary>
        /// Gets the <see cref="SceneScreen"/> where are the scene nodes used.
        /// </summary>
        public SceneScreen SceneScreen
        {
            get { return _sceneScreen; }
        }
        private SceneScreen _sceneScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodesList"/> class.
        /// </summary>
        /// <param name="sceneScreen">The <see cref="SceneScreen"/> where will be the scene node used.</param>
        public SceneNodesList(SceneScreen sceneScreen)
        {
            _sceneScreen = sceneScreen;
        }

        /// <inheritdoc />
        public override void Add(SceneNode item)
        {
            item.SceneControl = SceneScreen;
            base.Add(item);
            item.Initialize();
        }

        /// <summary>
        /// Adds the specified <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add.</param>
        /// <returns><see cref="SceneNode"/> representing the added <see cref="GameObject"/>.</returns>
        public SceneNode Add(GameObject gameObject)
        {
            SceneNode newSceneNode = gameObject.CreateSceneView();
            Add(newSceneNode);
            return newSceneNode;
        }

        /// <inheritdoc />
        public override void RemoveAt(int index)
        {
            Debug.Assert(this[index].SceneControl == SceneScreen, "SceneNode does not belong to this collection.");

            SceneNode item = this[index];

            base.RemoveAt(index);

            if (SceneScreen.SelectedNodes.Contains(item))
            {
                SceneScreen.SelectedNodes.Remove(item);
                SceneScreen.InvokeSelectedNodesChanged();
            }

            item.SceneControl = null;
            item.Dispose();
        }

        /// <summary>
        /// Removed the specified <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to remove.</param>
        public void Remove(GameObject gameObject)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (this[i].Tag == gameObject)
                {
                    RemoveAt(i);
                    break;
                }
            }
        }

        /// <inheritdoc />
        public override void Clear()
        {
            for (int i = 0; i < Count; ++i)
            {
                this[i].SceneControl = null;
                this[i].Dispose();
            }

            base.Clear();
        }
    }
}