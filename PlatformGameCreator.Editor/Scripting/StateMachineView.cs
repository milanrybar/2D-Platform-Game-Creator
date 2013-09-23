/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents screen for editing a state machine at the visual scripting.
    /// </summary>
    /// <remarks>
    /// Changes the position at the scene by right mouse button.
    /// Selects the state at the screen by left mouse button.
    /// Deletes the selected state from the screen by Delete key.
    /// Moves the selected state at the screen by left mouse button.
    /// </remarks>
    partial class StateMachineView : Control, ISceneDrawingTools
    {
        /// <summary>
        /// Gets or sets the underlying <see cref="Scripting.StateMachine"/>.
        /// </summary>
        public StateMachine StateMachine
        {
            get { return _stateMachine; }
            set
            {
                if (_stateMachine != value)
                {
                    if (_stateMachine != null)
                    {
                        hoveredState = null;
                        SelectedState = null;
                        selectedTransition = null;

                        foreach (StateView stateView in States)
                        {
                            stateView.Dispose();
                        }
                        States.Clear();

                        state = StateType.Default;
                        Position = new PointF();
                    }

                    _stateMachine = value;

                    if (_stateMachine != null)
                    {
                        foreach (State state in StateMachine.States)
                        {
                            States.Add(state.CreateView());
                            States[States.Count - 1].ScreenControl = this;
                        }
                    }
                }
            }
        }
        private StateMachine _stateMachine;

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
        /// States at the screen.
        /// </summary>
        private List<StateView> States = new List<StateView>();

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
        /// Defines a state of the <see cref="StateMachineView"/>.
        /// </summary>
        private enum StateType { Default, MovingScene, MovingState, ConnectingStates };

        /// <summary>
        /// State of the <see cref="StateMachineView"/>.
        /// </summary>
        private StateType state;

        // transform matrix
        private Matrix transform = new Matrix();
        // invers transform matrix
        private Matrix transformInvers = new Matrix();

        // mouse position at the screen
        private Point mouseScenePosition;

        // hovered state
        private StateView hoveredState;

        /// <summary>
        /// Selected state of the <see cref="StateMachineView"/>.
        /// </summary>
        public StateView SelectedState;

        // selected transition
        private TransitionView selectedTransition;

        /// <summary>
        /// Occurs when the <see cref="SelectedState"/> changes.
        /// </summary>
        public event EventHandler SelectedStateChanged;

        /// <summary>
        /// Occurs when the user double-click at the <see cref="SelectedState"/>.
        /// </summary>
        public event EventHandler StateDoubleClicked;

        /// <summary>
        /// Occurs after the <see cref="SelectedState"/> is deleted.
        /// </summary>
        public event EventHandler AfterDeletingState;

        // last position for changing position of the screen and selected states
        private Point lastPosition;
        private Point wholeMovement;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachineView"/> class.
        /// </summary>
        public StateMachineView()
        {
            InitializeComponent();

            DoubleBuffered = true;

            StateView.DrawingTools = this;
            _boldFont = new Font(Font, FontStyle.Bold);
            _linePen.CustomEndCap = new AdjustableArrowCap(5, 8, true);
        }

        /// <summary>
        /// Updates transform matrix.
        /// </summary>
        private void UpdateTransformMatrix()
        {
            // update transform matrix
            transform.Reset();
            transform.Translate(-Position.X, -Position.Y);

            // update invers transform matrix
            transformInvers.Reset();
            transformInvers.Translate(Position.X, Position.Y);
        }

        /// <summary>
        /// Converts the specified point from control to screen (abstract scene) coordinates.
        /// </summary>
        /// <param name="point">The point in the control coordinates to convert.</param>
        /// <returns>Converted point in the screen coordinates.</returns>
        private Point PointAtScene(Point point)
        {
            return point.Transform(transformInvers).ToPoint();
        }

        /// <inheritdoc />
        /// <summary>
        /// Paints the <see cref="StateMachineView"/>.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            // paint background
            pe.Graphics.Clear(ColorSettings.ScriptingScreenBackground);

            pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // set transform matrix
            pe.Graphics.Transform = transform;

            // paint states
            foreach (StateView stateView in States)
            {
                stateView.Paint(pe.Graphics);
            }

            // paint connecting line when action of connecting states is active
            if (state == StateType.ConnectingStates)
            {
                pe.Graphics.DrawLine(LinePen, selectedTransition.Bounds.X + selectedTransition.Bounds.Width / 2, selectedTransition.Bounds.Y + selectedTransition.Bounds.Height / 2, mouseScenePosition.X, mouseScenePosition.Y);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the StateMachineView control.
        /// </summary>
        private void StateMachineView_MouseDown(object sender, MouseEventArgs e)
        {
            // focus control if is not focused
            if (!Focused) Focus();

            // If left mouse button is down, no action is in progress and some node is under mouse cursor 
            // we will start the action of moving selected nodes at the scripting scene.
            if (e.Button == MouseButtons.Left && state == StateType.Default)
            {
                if (SelectedState != null)
                {
                    SelectedState.SelectState = SelectState.Default;
                }

                if (hoveredState != null)
                {
                    SelectedState = hoveredState;
                    SelectedState.SelectState = SelectState.Select;
                    hoveredState = null;

                    if (SelectedState.TransitionContains(mouseScenePosition, out selectedTransition))
                    {
                        // activate this action
                        state = StateType.ConnectingStates;
                    }
                    else
                    {
                        // activate this action
                        state = StateType.MovingState;
                        // init 
                        lastPosition = e.Location;
                        wholeMovement = new Point();
                    }

                    if (SelectedStateChanged != null) SelectedStateChanged(this, EventArgs.Empty);

                    Invalidate();
                }
                else if (SelectedState != null)
                {
                    SelectedState = null;

                    if (SelectedStateChanged != null) SelectedStateChanged(this, EventArgs.Empty);

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
        /// Handles the MouseUp event of the StateMachineView control.
        /// </summary>
        private void StateMachineView_MouseUp(object sender, MouseEventArgs e)
        {
            // If left mouse button is up and the action of moving selected nodes at the scripting scene is active
            // we will finish it.
            if (e.Button == MouseButtons.Left && state == StateType.MovingState)
            {
                // this action is over
                state = StateType.Default;
            }

            else if (e.Button == MouseButtons.Left && state == StateType.ConnectingStates)
            {
                // set transition end
                if (hoveredState != null && hoveredState != SelectedState)
                {
                    selectedTransition.Transition.StateTo = hoveredState.State;
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
        /// Handles the MouseMove event of the StateMachineView control.
        /// </summary>
        private void StateMachineView_MouseMove(object sender, MouseEventArgs e)
        {
            // select control if is not focused
            if (!Focused) Select();

            // update mouse scene position
            mouseScenePosition = PointAtScene(e.Location);

            // If the action of moving selected nodes at the scripting scene is active
            // we will change the location of selected nodes by distance we moved from the last changing.
            if (state == StateType.MovingState)
            {
                // calculate move vector for selected nodes at the scene
                Point moveVector = e.Location.Sub(lastPosition);

                // change the location of every selected nodes at the scene
                SelectedState.Location = SelectedState.Location.Add(moveVector);

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
                Position = Position.Add(lastPosition.Sub(e.Location));
                // set actual position for the next changing
                lastPosition = e.Location;

                Invalidate();
            }

            else if (state == StateType.Default || state == StateType.ConnectingStates)
            {
                // hover state under mouse cursor
                bool found = false;
                for (int i = States.Count - 1; i >= 0; --i)
                {
                    if (States[i].Bounds.Contains(mouseScenePosition))
                    {
                        if (hoveredState != States[i])
                        {
                            if (hoveredState != null && hoveredState.SelectState != SelectState.Select) hoveredState.SelectState = SelectState.Default;

                            // set as hovered
                            hoveredState = States[i];
                            if (hoveredState.SelectState != SelectState.Select) hoveredState.SelectState = SelectState.Hover;

                            Invalidate();
                        }

                        found = true;
                        break;
                    }
                }

                if (!found && hoveredState != null)
                {
                    if (hoveredState.SelectState != SelectState.Select) hoveredState.SelectState = SelectState.Default;
                    hoveredState = null;
                    Invalidate();
                }

                if (state == StateType.ConnectingStates)
                {
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Finds the <see cref="StateView"/> representing the specified state.
        /// </summary>
        /// <param name="state">The state to find..</param>
        /// <returns><see cref="StateView"/> representing the specified state; otherwise <c>null</c>.</returns>
        public StateView FindStateViewForState(State state)
        {
            foreach (StateView stateView in States)
            {
                if (stateView.State == state) return stateView;
            }

            return null;
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the StateMachineView control.
        /// </summary>
        private void StateMachineView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (SelectedState != null)
            {
                if (StateDoubleClicked != null) StateDoubleClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the StateMachineView control.
        /// </summary>
        private void StateMachineView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && state == StateType.Default && SelectedState != null)
            {
                if (StateMachine.States.Count <= 1)
                {
                    Messages.ShowWarning("State cannot be removed. At least one state must exist.");
                    return;
                }

                if (MessageBox.Show("Are you sure?", "Delete State", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    // remove state from all transitions
                    foreach (State stateMachineState in StateMachine.States)
                    {
                        if (stateMachineState != SelectedState.State)
                        {
                            foreach (Transition transition in stateMachineState.Transitions)
                            {
                                if (transition.StateTo == SelectedState.State) transition.StateTo = null;
                            }
                        }
                    }

                    // remove from state machine
                    StateMachine.States.Remove(SelectedState.State);
                    if (StateMachine.StartingState == SelectedState.State) StateMachine.StartingState = null;

                    States.Remove(SelectedState);
                    SelectedState.Dispose();

                    SelectedState = States[0];

                    if (SelectedStateChanged != null) SelectedStateChanged(this, EventArgs.Empty);

                    Messages.ShowInfo("State deleted.");

                    if (AfterDeletingState != null) AfterDeletingState(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Creates and adds the new state to the undelying state machine.
        /// </summary>
        public void CreateNewState()
        {
            // create new state
            State newState = new State(StateMachine);
            newState.Name = GenerateUniqueStateName();
            newState.Location = PointAtScene(new Point(Width / 2, Height / 2));

            // add to the state machine
            StateMachine.States.Add(newState);
            States.Add(newState.CreateView());
            States[States.Count - 1].ScreenControl = this;

            Invalidate();
        }

        /// <summary>
        /// Generates the unique name for the state.
        /// </summary>
        /// <returns>The unique name for the state.</returns>
        private string GenerateUniqueStateName()
        {
            int i = 0;
            string stateName = "New State";
            while (ContainsStateName(stateName))
            {
                stateName = String.Format("New State({0})", ++i);
            }

            return stateName;
        }

        /// <summary>
        /// Determines whether any state has the specified name.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns><c>true</c> if any state has the specified name; otherwise <c>false</c>.</returns>
        private bool ContainsStateName(string stateName)
        {
            foreach (State state in StateMachine.States)
            {
                if (state.Name == stateName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates the new transition for the <see cref="SelectedState"/>.
        /// </summary>
        public void CreateTransition()
        {
            if (SelectedState != null)
            {
                // create new transition
                Transition newTransition = new Transition(SelectedState.State);

                // add to the selected state
                SelectedState.State.Transitions.Add(newTransition);
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
                StateMachine = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Represents <see cref="State"/> at the <see cref="StateMachineView"/> control.
    /// </summary>
    class StateView : Disposable
    {
        /// <summary>
        /// Gets the underlying state.
        /// </summary>
        public State State
        {
            get { return _state; }
        }
        private State _state;

        /// <summary>
        /// Gets or sets the <see cref="StateMachineView"/> where the <see cref="StateView"/> is used.
        /// </summary>
        public StateMachineView ScreenControl { get; set; }

        /// <summary>
        /// Gets or sets the name of the state.
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
        private string _name;

        /// <summary>
        /// Gets or sets the location of the state at the <see cref="ScreenControl"/>.
        /// </summary>
        public Point Location
        {
            get { return State.Location; }
            set
            {
                State.Location = value;
                UpdateGui();
            }
        }

        /// <summary>
        /// Gets the size and location of the state.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
        }
        private Rectangle _bounds;

        /// <summary>
        /// Gets the height of the title text.
        /// </summary>
        public float TitleHeight
        {
            get { return SizeSettings.StateTextPadding.Top + nameSize.Height + SizeSettings.StateTextPadding.Bottom; }
        }

        /// <summary>
        /// Gets or sets the select state of the state.
        /// </summary>
        public SelectState SelectState { get; set; }

        /// <summary>
        /// Gets or sets drawing tools for the state.
        /// </summary>
        public static ISceneDrawingTools DrawingTools { get; set; }

        /// <summary>
        /// Gets a value indicating whether this state is the starting state of the state machine.
        /// </summary>
        public bool StartingState
        {
            get { return State.StateMachine.StartingState == State; }
        }

        // size in pixels of the name text
        private Size nameSize;
        // location in pixels of the name text
        private Point nameLocation;
        // transitions area
        private List<TransitionView> transitions = new List<TransitionView>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StateView"/> class.
        /// </summary>
        /// <param name="state">The state to use.</param>
        public StateView(State state)
        {
            if (state == null) throw new ArgumentException("State cannot be null.");

            _state = state;

            foreach (Transition transition in State.Transitions)
            {
                transitions.Add(new TransitionView(this, transition));
            }

            Name = State.Name;

            State.NameChanged += new EventHandler(State_NameChanged);
            State.Transitions.ListChanged += new ObservableList<Transition>.ListChangedEventHandler(Transitions_ListChanged);
        }

        /// <summary>
        /// Handles the ListChanged event of the Transitions of the State.
        /// Updates transitions of the state.
        /// </summary>
        private void Transitions_ListChanged(object sender, ObservableListChangedEventArgs<Transition> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    transitions.Add(new TransitionView(this, e.Item));
                    break;

                case ObservableListChangedType.ItemDeleted:
                    transitions.RemoveAt(e.Index);
                    break;

                case ObservableListChangedType.Reset:
                    foreach (TransitionView transitionView in transitions)
                    {
                        transitionView.Dispose();
                    }

                    transitions.Clear();

                    foreach (Transition transition in State.Transitions)
                    {
                        transitions.Add(new TransitionView(this, transition));
                    }
                    break;
            }

            Refresh();
        }

        /// <summary>
        /// Handles the NameChanged event of the State.
        /// Updates the name and invalidates the ScreenControl.
        /// </summary>
        private void State_NameChanged(object sender, EventArgs e)
        {
            Name = State.Name;
            ScreenControl.Invalidate();
        }

        /// <summary>
        /// Paints the state.
        /// </summary>
        /// <param name="graphics"><see cref="Graphics"/> instance for painting.</param>
        public void Paint(Graphics graphics)
        {
            // paint background
            DrawingTools.SolidBrush.Color = ColorSettings.ForState(SelectState, StartingState);
            graphics.FillRectangle(DrawingTools.SolidBrush, Bounds);

            // paint border
            DrawingTools.Pen.Color = ColorSettings.ForState(SelectState, StartingState);
            graphics.DrawRectangle(DrawingTools.Pen, Bounds);

            // paint name text
            DrawingTools.SolidBrush.Color = ColorSettings.StateText;
            graphics.DrawString(Name, DrawingTools.BoldFont, DrawingTools.SolidBrush, nameLocation);

            // paint transitions
            for (int i = 0; i < transitions.Count; ++i)
            {
                transitions[i].Paint(graphics);
            }
        }

        /// <summary>
        /// Updates any necessary variables for GUI.
        /// </summary>
        private void UpdateGui()
        {
            // compute width and height
            int width = SizeSettings.StateTextPadding.Left + nameSize.Width + SizeSettings.StateTextPadding.Right;
            int height = SizeSettings.StateTextPadding.Top + nameSize.Height + SizeSettings.StateTextPadding.Bottom;

            for (int i = 0; i < transitions.Count; ++i)
            {
                transitions[i].Location = new Point(Location.X, Location.Y + height);

                if (width < transitions[i].Bounds.Width) width = transitions[i].Bounds.Width;
                height += transitions[i].Bounds.Height;
            }

            // set bounds
            _bounds = new Rectangle(Location.X, Location.Y, width, height);

            // set name text location
            nameLocation = new Point(Location.X + (width - nameSize.Width) / 2, Location.Y + SizeSettings.StateTextPadding.Top);

            // set transitions
            int heightPosition = Location.Y + SizeSettings.StateTextPadding.Top + nameSize.Height + SizeSettings.StateTextPadding.Bottom;
            for (int i = 0; i < transitions.Count; ++i)
            {
                // set transition location
                transitions[i].Location = new Point(Location.X, heightPosition);
                heightPosition += transitions[i].Bounds.Height;
            }
        }

        /// <summary>
        /// Determines whether any view of transition contains the specified point.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <param name="transitionView">The view of transition that contains the specified point, if any.</param>
        /// <returns><c>true</c> if any view of transition contains the specified point; otherwise <c>false</c>.</returns>
        public bool TransitionContains(Point point, out TransitionView transitionView)
        {
            transitionView = null;

            if (!Bounds.Contains(point)) return false;

            foreach (TransitionView transition in transitions)
            {
                if (transition.Bounds.Contains(point))
                {
                    transitionView = transition;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Refreshes the state. Updates and invalidates the view of state.
        /// </summary>
        public void Refresh()
        {
            UpdateGui();
            ScreenControl.Invalidate();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                State.NameChanged -= new EventHandler(State_NameChanged);
                State.Transitions.ListChanged -= new ObservableList<Transition>.ListChangedEventHandler(Transitions_ListChanged);
                _state = null;

                foreach (TransitionView transitionView in transitions)
                {
                    transitionView.Dispose();
                }

                transitions.Clear();
                transitions = null;
            }
        }
    }

    /// <summary>
    /// Represents <see cref="Transition"/> at the <see cref="StateMachineView"/> control.
    /// </summary>
    class TransitionView : Disposable
    {
        /// <summary>
        /// Gets the underlying transition.
        /// </summary>
        public Transition Transition
        {
            get { return _transition; }
        }
        private Transition _transition;

        /// <summary>
        /// Gets the view of the state where is the transition used.
        /// </summary>
        public StateView StateView
        {
            get { return _stateView; }
        }
        private StateView _stateView;

        /// <summary>
        /// Gets or sets the location of the transition.
        /// </summary>
        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                UpdateGui();
            }
        }
        private Point _location;

        /// <summary>
        /// Gets the size and location of the transition.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
        }
        private Rectangle _bounds;

        /// <summary>
        /// Gets or sets the event in of the transition.
        /// </summary>
        private Event Event
        {
            get { return _event; }
            set
            {
                if (_event != null)
                {
                    _event.NameChanged -= new EventHandler(Event_NameChanged);
                }

                _event = value;

                if (_event != null)
                {
                    _event.NameChanged += new EventHandler(Event_NameChanged);
                }
            }
        }
        private Event _event;

        private Point textLocation;
        private Size textSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionView"/> class.
        /// </summary>
        /// <param name="stateView">The state view where will be the transition used.</param>
        /// <param name="transition">The transition to use.</param>
        public TransitionView(StateView stateView, Transition transition)
        {
            if (stateView == null) throw new ArgumentException("StateView cannot be null.");
            if (transition == null) throw new ArgumentException("Transition cannot be null.");

            _stateView = stateView;
            _transition = transition;

            Transition.EventChanged += new EventHandler(Transition_EventChanged);
            Transition.StateToChanged += new EventHandler(Transition_StateToChanged);

            Event = transition.Event;
        }

        /// <summary>
        /// Handles the NameChanged event of the Event.
        /// Updates the state view where the transition is used.
        /// </summary>
        private void Event_NameChanged(object sender, EventArgs e)
        {
            StateView.Refresh();
        }

        /// <summary>
        /// Handles the StateToChanged event of the Transition.
        /// Updates the state view where the transition is used.
        /// </summary>
        private void Transition_StateToChanged(object sender, EventArgs e)
        {
            StateView.Refresh();
        }

        /// <summary>
        /// Handles the EventChanged event of the Transition.
        /// Updates the state view where the transition is used.
        /// </summary>
        private void Transition_EventChanged(object sender, EventArgs e)
        {
            Event = Transition.Event;

            StateView.Refresh();
        }

        /// <summary>
        /// Updates any necessary variables for GUI.
        /// </summary>
        private void UpdateGui()
        {
            // compute width and height
            if (Transition.Event != null)
            {
                textSize = TextRenderer.MeasureText(Transition.Event.Name, StateView.DrawingTools.Font);
            }
            else
            {
                textSize = new Size(0, (int)StateView.DrawingTools.Font.GetHeight());
            }

            // set bounds
            int width = SizeSettings.StateTextPadding.Left + textSize.Width + SizeSettings.StateTextPadding.Right;
            if (StateView.Bounds.Width > width) width = StateView.Bounds.Width;
            _bounds = new Rectangle(Location.X, Location.Y, width, SizeSettings.StateTextPadding.Top + textSize.Height + SizeSettings.StateTextPadding.Bottom);

            // set text location
            textLocation = new Point(Location.X + (Bounds.Width - textSize.Width) / 2, Location.Y + SizeSettings.StateTextPadding.Top);
        }

        /// <summary>
        /// Paints the transition.
        /// </summary>
        /// <param name="graphics"><see cref="Graphics"/> instance for painting.</param>
        public void Paint(Graphics graphics)
        {
            // paint background
            StateView.DrawingTools.SolidBrush.Color = ColorSettings.StateTransition;
            graphics.FillRectangle(StateView.DrawingTools.SolidBrush, Bounds);

            // paint border
            StateView.DrawingTools.Pen.Color = ColorSettings.ForState(StateView.SelectState, StateView.StartingState);
            graphics.DrawRectangle(StateView.DrawingTools.Pen, Bounds);

            // paint event text
            StateView.DrawingTools.SolidBrush.Color = ColorSettings.StateTransitionText;
            graphics.DrawString(Transition.Event != null ? Transition.Event.Name : String.Empty, StateView.DrawingTools.Font, StateView.DrawingTools.SolidBrush, textLocation);

            // paint transition
            if (Transition.StateTo != null)
            {
                StateView stateToView = StateView.ScreenControl.FindStateViewForState(Transition.StateTo);

                if (stateToView != null)
                {
                    PointF startingPoint, afterStartingPoint, afterStartingPointBezier;
                    PointF endingPoint, beforeEndingPointBezier;

                    float bezierWidthGap = 80;
                    float afterStartWidthGap = 40;

                    // right side of the transition
                    if ((stateToView.Location.X + stateToView.Bounds.Width / 2f) - (Location.X + Bounds.Width / 2f) >= 0)
                    {
                        startingPoint = new PointF(Location.X + Bounds.Width, Location.Y + Bounds.Height / 2f);
                        afterStartingPoint = new PointF(startingPoint.X + afterStartWidthGap, startingPoint.Y);
                        afterStartingPointBezier = new PointF(startingPoint.X + bezierWidthGap, startingPoint.Y);
                    }
                    // left side of the transition
                    else
                    {
                        startingPoint = new PointF(Location.X, Location.Y + Bounds.Height / 2f);
                        afterStartingPoint = new PointF(startingPoint.X - afterStartWidthGap, startingPoint.Y);
                        afterStartingPointBezier = new PointF(startingPoint.X - bezierWidthGap, startingPoint.Y);
                    }

                    // left side of the state
                    if (Math.Abs(stateToView.Location.X - afterStartingPoint.X) < Math.Abs(stateToView.Location.X + stateToView.Bounds.Width - afterStartingPoint.X))
                    {
                        endingPoint = new PointF(stateToView.Location.X, stateToView.Location.Y + stateToView.TitleHeight / 2f);
                        beforeEndingPointBezier = new PointF(endingPoint.X - bezierWidthGap, endingPoint.Y);
                    }
                    // right side of the state
                    else
                    {
                        endingPoint = new PointF(stateToView.Location.X + stateToView.Bounds.Width, stateToView.Location.Y + stateToView.TitleHeight / 2f);
                        beforeEndingPointBezier = new PointF(endingPoint.X + bezierWidthGap, endingPoint.Y);
                    }

                    graphics.DrawBezier(StateView.DrawingTools.LinePen, startingPoint, afterStartingPointBezier, beforeEndingPointBezier, endingPoint);
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Event != null) Event = null;
                if (Transition != null)
                {
                    Transition.EventChanged -= new EventHandler(Transition_EventChanged);
                    Transition.StateToChanged -= new EventHandler(Transition_StateToChanged);

                    _transition = null;
                }
            }
        }
    }
}
