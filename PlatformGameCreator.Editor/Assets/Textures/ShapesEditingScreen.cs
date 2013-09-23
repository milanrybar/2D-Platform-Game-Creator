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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Common;
using System.Drawing.Drawing2D;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Screen for editing collision shapes and origin of <see cref="DrawableAsset"/> (<see cref="Texture"/> and <see cref="Animations.Animation"/>).
    /// </summary>
    /// <remarks>
    /// Used at <see cref="TextureForm"/> and <see cref="Animations.AnimationForm"/>.
    /// </remarks>
    abstract partial class ShapesEditingScreen : Control, IScreenDrawingTools
    {
        /// <summary>
        /// Gets or sets the position at the screen.
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
        /// Gets or sets the zoom (10-500) for the screen.
        /// </summary>
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom > 500) _zoom = 500;
                else if (_zoom < 10) _zoom = 10;
                UpdateTransformMatrix();
            }
        }
        private int _zoom = 100;

        /// <summary>
        /// Gets the scale factor defined by the <see cref="Zoom"/>.
        /// </summary>
        public float ScaleFactor
        {
            get { return (float)Zoom / 100f; }
        }

        /// <summary>
        /// Gets the invers scale factor defined by the <see cref="Zoom"/>.
        /// </summary>
        public float ScaleInversFactor
        {
            get { return 100f / (float)Zoom; }
        }

        /// <summary>
        /// Gets the mouse position at the screen.
        /// </summary>
        public PointF MouseScreenPosition
        {
            get { return _mouseScreenPosition; }
        }
        private PointF _mouseScreenPosition;

        /// <summary>
        /// Gets or sets the state of the screen.
        /// </summary>
        /// <remarks>
        /// After the new state is set fires <see cref="StateChanged"/> event and calls <see cref="ShapesEditingState.OnSet"/> method on the state.
        /// </remarks>
        public ShapesEditingState State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (StateChanged != null) StateChanged(this, EventArgs.Empty);
                _state.OnSet();
                Invalidate();
            }
        }
        private ShapesEditingState _state;

        /// <summary>
        /// Indicates whether the grid is visible on the background. Default value is <c>true</c>.
        /// </summary>
        public bool ShowGrid = true;

        /// <summary>
        /// Indicates whether all shapes are visible or just the active shape. Default value is <c>true</c>.
        /// </summary>
        public bool ShowAllShapes = true;

        /// <summary>
        /// Indicates whether the origin is visible. Default value is <c>true</c>.
        /// </summary>
        public bool ShowOrigin = true;

        /// <summary>
        /// Gets the solid brush.
        /// </summary>
        public SolidBrush SolidBrush
        {
            get { return _solidBrush; }
        }
        private SolidBrush _solidBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// Gets the pen for drawing line.
        /// </summary>
        public Pen LinePen
        {
            get { return _linePen; }
        }
        private Pen _linePen = new Pen(Color.Black);

        /// <summary>
        /// Gets the pen for drawing line of a shape.
        /// </summary>
        public Pen ShapeLinePen
        {
            get { return _shapeLinePen; }
        }
        private Pen _shapeLinePen = new Pen(Color.Blue, 2);

        /// <summary>
        /// Gets the default state for the screen.
        /// </summary>
        public ShapesEditingState DefaultState
        {
            get
            {
                if (_defaultState == null) _defaultState = new GlobalScreenState() { Parent = this };
                return _defaultState;
            }
        }
        private ShapesEditingState _defaultState;

        /// <summary>
        /// Gets the shapes. (Actually <see cref="ShapeState"/> of shapes for faster editing.)
        /// </summary>
        public List<ShapeState> Shapes
        {
            get { return _shapes; }
        }
        private List<ShapeState> _shapes = new List<ShapeState>();

        /// <summary>
        /// Gets the radius of the vertex. (Value depends on the current <see cref="Zoom"/> value.)
        /// </summary>
        public float VertexRadius
        {
            get { return _vertexRadius; }
        }
        private float _vertexRadius;

        /// <summary>
        /// Occurs when the <see cref="State"/> value changes.
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Next color for new shape.
        /// </summary>
        private int goodColorsIndex = 0;

        /// <summary>
        /// Screen transform matrix.
        /// </summary>
        private Matrix transform = new Matrix();

        /// <summary>
        /// Invers matrix of the screen transform matrix.
        /// </summary>
        private Matrix transformInvers = new Matrix();

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapesEditingScreen"/> class.
        /// Starts with the default state.
        /// </summary>
        public ShapesEditingScreen()
        {
            DoubleBuffered = true;
            InitializeComponent();

            Resize += new EventHandler(ShapesEditingScreen_Resize);
            KeyDown += new KeyEventHandler(ShapesEditingScreen_KeyDown);
            MouseMove += new MouseEventHandler(ShapesEditingScreen_MouseMove);
            MouseWheel += new MouseEventHandler(ShapesEditingScreen_MouseWheel);
            MouseDown += new MouseEventHandler(ShapesEditingScreen_MouseDown);
            MouseUp += new MouseEventHandler(ShapesEditingScreen_MouseUp);

            // set default state
            State = DefaultState;

            // set drawing tools to screen state
            ShapesEditingState.DrawingTools = this;
        }

        /// <summary>
        /// Handles the KeyDown event of the ShapesEditingScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="ShapesEditingState.KeyDown"/> method) of the ShapesEditingScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void ShapesEditingScreen_KeyDown(object sender, KeyEventArgs e)
        {
            State.KeyDown(sender, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the ShapesEditingScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="ShapesEditingState.MouseUp"/> method) of the ShapesEditingScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ShapesEditingScreen_MouseUp(object sender, MouseEventArgs e)
        {
            State.MouseUp(sender, e);
        }

        /// <summary>
        /// Handles the MouseDown event of the ShapesEditingScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="ShapesEditingState.MouseDown"/> method) of the ShapesEditingScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ShapesEditingScreen_MouseDown(object sender, MouseEventArgs e)
        {
            // focus control if not
            if (!Focused) Focus();

            State.MouseDown(sender, e);
        }

        /// <summary>
        /// Handles the MouseWheel event of the ShapesEditingScreen control.
        /// Changes the <see cref="Zoom"/> according to the mouse wheel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ShapesEditingScreen_MouseWheel(object sender, MouseEventArgs e)
        {
            // actual position under mouse cursor
            PointF actualPosition = MouseScreenPosition;

            // zoom the scene
            Zoom += e.Delta / 30;

            // new positon under mouse cursor
            PointF changedPosition = PointAtScreen(e.Location);

            // We want to keep the same position under mouse cursor before and after changing zoom of the screen
            // so we change the position of the screen by difference between before and after position under mouse cursor.
            Position = Position.Add(actualPosition.Sub(changedPosition));

            // update mouse screen position
            _mouseScreenPosition = PointAtScreen(e.Location);

            Invalidate();
        }

        /// <summary>
        /// Converts the specified point from control to screen (abstract scene) coordinates.
        /// </summary>
        /// <param name="point">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the screen coordinates.</returns>
        public PointF PointAtScreen(PointF point)
        {
            return point.Transform(transformInvers);
        }

        /// <summary>
        /// Converts the specified point from control to screen (abstract scene) coordinates.
        /// </summary>
        /// <param name="point">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the screen coordinates.</returns>
        public PointF PointAtScreen(Point point)
        {
            return point.Transform(transformInvers);
        }

        /// <summary>
        /// Updates transform matrix of the screen.
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

            _vertexRadius = SizeSettings.VertexRadius * ScaleInversFactor;
        }

        /// <summary>
        /// Creates and adds new shape based on the specified <paramref name="shapeType"/> to the screen.
        /// </summary>
        /// <param name="shapeType">Type of the shape to create.</param>
        /// <returns>Returns <see cref="ShapeState"/> of the added shape.</returns>
        public ShapeState AddShape(Shape.ShapeType shapeType)
        {
            ShapeState newState = null;

            switch (shapeType)
            {
                case Shape.ShapeType.Polygon:
                    newState = new PolygonEditState(new Polygon(), this) { Color = ColorSettings.GoodColors[goodColorsIndex] };
                    State = newState;
                    break;

                case Shape.ShapeType.Circle:
                    newState = new CircleEditState(new Circle(), this) { Color = ColorSettings.GoodColors[goodColorsIndex] };
                    State = newState;
                    break;

                case Shape.ShapeType.Edge:
                    newState = new EdgeEditState(new Edge(), this) { Color = ColorSettings.GoodColors[goodColorsIndex] };
                    State = newState;
                    break;
            }

            if (newState != null)
            {
                if (++goodColorsIndex >= ColorSettings.GoodColors.Length) goodColorsIndex = 0;
                Shapes.Add(newState);
            }

            return newState;
        }

        /// <summary>
        /// Adds the specified shape to the screen.
        /// </summary>
        /// <param name="shape">Shape to add</param>
        /// <returns>Returns <see cref="ShapeState"/> of the added shape.</returns>
        public ShapeState AddShape(Shape shape)
        {
            ShapeState newState = null;

            switch (shape.Type)
            {
                case Shape.ShapeType.Polygon:
                    newState = new PolygonEditState((Polygon)shape, this) { Color = ColorSettings.GoodColors[goodColorsIndex] };
                    State = newState;
                    break;

                case Shape.ShapeType.Circle:
                    newState = new CircleEditState((Circle)shape, this) { Color = ColorSettings.GoodColors[goodColorsIndex] };
                    State = newState;
                    break;

                case Shape.ShapeType.Edge:
                    newState = new EdgeEditState((Edge)shape, this) { Color = ColorSettings.GoodColors[goodColorsIndex] };
                    State = newState;
                    break;
            }

            if (newState != null)
            {
                if (++goodColorsIndex >= ColorSettings.GoodColors.Length) goodColorsIndex = 0;
                Shapes.Add(newState);
            }

            return newState;
        }

        /// <summary>
        /// Removes the specified shape from the screen.
        /// </summary>
        /// <param name="shape">Shape to remove.</param>
        public void RemoveShape(ShapeState shape)
        {
            if (State == shape) State = DefaultState;

            Shapes.Remove(shape);
        }

        /// <summary>
        /// Handles the MouseMove event of the ShapesEditingScreen control.
        /// Hands the event over to the <see cref="State"/> (<see cref="ShapesEditingState.MouseMove"/> method) of the ShapesEditingScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void ShapesEditingScreen_MouseMove(object sender, MouseEventArgs e)
        {
            // select control if not
            if (!Focused) Select();

            // update mouse scene position
            _mouseScreenPosition = PointAtScreen(e.Location);

            State.MouseMove(sender, e);
        }

        /// <summary>
        /// Handles the Resize event of the ShapesEditingScreen control.
        /// Invalidates the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ShapesEditingScreen_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Paints the control.
        /// </summary>
        /// <remarks>
        /// Paints in this order:
        /// <list type="bullet">
        /// <item><description>background</description></item>
        /// <item><description>grid, if visible</description></item>
        /// <item><description>image of the asset (by <see cref="PaintImage"/>)</description></item>
        /// <item><description>not active shapes, if visible</description></item>
        /// <item><description>origin, if visible</description></item>
        /// </list>
        /// </remarks>
        /// Then hands the event over to the <see cref="State"/> (<see cref="ShapesEditingState.OnPaint"/> method) of the ShapesEditingScreen control.
        /// <param name="pe">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // draw background
            pe.Graphics.Clear(ColorSettings.BackgroundColor);

            // set transform matrix
            pe.Graphics.Transform = transform;

            ShapeLinePen.Width = 2f * ScaleInversFactor;
            LinePen.Width = 1f * ScaleInversFactor;

            // draw grid
            if (ShowGrid) DrawGrid(pe.Graphics);

            // draw image
            PaintImage(pe);

            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // draw all not selected shapes
            if (ShowAllShapes)
            {
                foreach (ShapeState shape in Shapes)
                {
                    if (shape != State)
                    {
                        shape.PaintShape(pe.Graphics);
                    }
                }
            }

            // draw origin
            if (ShowOrigin)
            {
                SolidBrush.Color = ColorSettings.OriginVertexColor;
                pe.Graphics.FillEllipse(SolidBrush, -VertexRadius, -VertexRadius, VertexRadius * 2, VertexRadius * 2);
                LinePen.Color = ColorSettings.OriginVertexBorderColor;
                pe.Graphics.DrawEllipse(LinePen, -VertexRadius, -VertexRadius, VertexRadius * 2, VertexRadius * 2);
            }

            // state paint
            State.OnPaint(pe);
        }

        /// <summary>
        /// Paints the image of asset at the control.
        /// </summary>
        /// <param name="pe">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        protected abstract void PaintImage(PaintEventArgs pe);

        /// <summary>
        /// Draws grid.
        /// </summary>
        /// <param name="graphics"><see cref="Graphics"/> for drawing.</param>
        private void DrawGrid(Graphics graphics)
        {
            float gridGapSize = SizeSettings.GridGapSize * ScaleInversFactor;
            float screenWidth = Width * ScaleInversFactor;
            float screenHeight = Height * ScaleInversFactor;

            // draw vertical lines (only normal)
            int count = (int)(Position.X / gridGapSize);
            LinePen.Color = ColorSettings.GridLineColor;
            for (float i = Position.X - (Position.X % gridGapSize); i <= Position.X + screenWidth; i += gridGapSize)
            {
                if (count % 5 != 0) graphics.DrawLine(LinePen, i, Position.Y, i, Position.Y + screenHeight);
                ++count;
            }

            // draw horizontal lines (normal & strong)
            count = (int)(Position.Y / gridGapSize);
            for (float i = Position.Y - (Position.Y % gridGapSize); i <= Position.Y + screenHeight; i += gridGapSize)
            {
                LinePen.Color = (count % 5 == 0) ? ColorSettings.GridStrongLineColor : ColorSettings.GridLineColor;
                graphics.DrawLine(LinePen, Position.X, i, Position.X + screenWidth, i);
                ++count;
            }

            // draw vertical lines (only strong)
            LinePen.Color = ColorSettings.GridStrongLineColor;
            for (float i = Position.X - (Position.X % (gridGapSize * 5f)); i <= Position.X + screenWidth; i += gridGapSize * 5f)
            {
                graphics.DrawLine(LinePen, i, Position.Y, i, Position.Y + screenHeight);
            }
        }
    }
}
