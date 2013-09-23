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
using System.Drawing.Drawing2D;
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;
using System.ComponentModel;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents <see cref="Variable"/> at the scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    class VariableView : SceneNode, IConnecting, IEditSettings
    {
        /// <summary>
        /// Gets the underlying script variable.
        /// </summary>
        public Variable Variable
        {
            get { return _variable; }
        }
        private Variable _variable;

        /// <summary>
        /// Gets or sets the name of the variable node.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                nameSize = TextRenderer.MeasureText(_name, DrawingTools.Font);
                UpdateGui();
            }
        }
        private string _name = "Variable";

        /// <summary>
        /// Gets or sets the location of the variable node at the scene.
        /// </summary>
        public override PointF Location
        {
            get { return Variable.Location; }
            set
            {
                Variable.Location = value;
                UpdateGui();
            }
        }

        /// <summary>
        /// Gets the size and location of the variable node.
        /// </summary>
        public override RectangleF Bounds
        {
            get { return _bounds; }
        }
        private RectangleF _bounds;

        /// <inheritdoc />
        public override bool CanMove
        {
            get { return true; }
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
            get { return true; }
        }

        /// <inheritdoc />
        public override bool IsSocketsContainer
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override INodeSocketsContainer INodeSocketsContainer
        {
            get { return null; }
        }

        /// <inheritdoc />
        public override IConnecting IConnecting
        {
            get { return this; }
        }

        /// <inheritdoc />
        public override IEditSettings IEditSettings
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the connections of the script variable node.
        /// </summary>
        public List<ConnectionView> Connections
        {
            get { return _connections; }
        }
        private List<ConnectionView> _connections = new List<ConnectionView>();

        /// <summary>
        /// Gets this script variable node.
        /// </summary>
        public SceneNode Parent
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the center of the script variable node for the connection line. Used by <see cref="ConnectionView"/>.
        /// </summary>
        public PointF Center
        {
            get { return _center; }
        }
        protected PointF _center;

        /// <summary>
        /// Gets the color for the script variable node.
        /// </summary>
        public Color Color
        {
            get { return ColorSettings.ForVariable(Variable.VariableType, Scripting.SelectState.Default); }
        }

        // size in pixels of the name text
        private Size nameSize = new Size(100, 15);
        // location in pixels of the name text
        private PointF nameLocation;
        // location in pixels of the comment text
        private PointF commentLocation;

        // path of rounded rectangle of the node
        private GraphicsPath roundedRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableView"/> class.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public VariableView(Variable variable)
        {
            _variable = variable;

            SetName();

            Variable.CommentChanged += new EventHandler(Variable_CommentChanged);
            Variable.Value.ValueChanged += new EventHandler(Value_ValueChanged);

            if (Variable.NamedVariable != null)
            {
                Variable.NamedVariable.NameChanged += new EventHandler(Value_ValueChanged);
                Variable.NamedVariableChanged += new ValueChangedHandler<NamedVariable>(Variable_NamedVariableChanged);
            }
        }

        /// <summary>
        /// Handles the NamedVariableChanged event of the Variable.
        /// Updates and invalidates the variable node.
        /// </summary>
        private void Variable_NamedVariableChanged(object sender, ValueChangedEventArgs<NamedVariable> e)
        {
            Debug.Assert(Variable.NamedVariable == null, "Not supported named variable change.");

            if (e.OldValue != null)
            {
                e.OldValue.NameChanged -= new EventHandler(Value_ValueChanged);
                e.OldValue.Value.ValueChanged -= new EventHandler(Value_ValueChanged);
                Variable.NamedVariableChanged -= new ValueChangedHandler<NamedVariable>(Variable_NamedVariableChanged);
            }

            Variable.Value.ValueChanged += new EventHandler(Value_ValueChanged);

            SetName();
            Invalidate();
        }

        /// <summary>
        /// Handles the ValueChanged event of the Variable.
        /// Updates and invalidates the variable node.
        /// </summary>
        private void Value_ValueChanged(object sender, EventArgs e)
        {
            SetName();
            Invalidate();
        }

        /// <summary>
        /// Handles the CommentChanged event of the Variable.
        /// Invalidates the variable node.
        /// </summary>
        private void Variable_CommentChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Sets the name for the variable node.
        /// </summary>
        private void SetName()
        {
            if (Variable.NamedVariable != null)
            {
                Name = String.Format("{0} :: {1}", Variable.NamedVariable.Name, Variable.Value.ToString());
            }
            else
            {
                Name = Variable.Value.ToString();
            }
        }

        /// <inheritdoc />
        public override bool Contains(PointF point)
        {
            return Bounds.Contains(point);
        }

        /// <summary>
        /// Called when the scene node is added to the <see cref="ScriptingScreen"/>.
        /// The underlying variable is added to the state.
        /// </summary>
        public override void OnSceneNodeAdded()
        {
            Debug.Assert(!SceneControl.State.Nodes.Contains(Variable), "Scripting component already contains this variable.");

            SceneControl.State.Nodes.Add(Variable);
        }

        /// <summary>
        /// Called when the scene node is removed from the <see cref="ScriptingScreen"/>.
        /// The underlying variable is removed from the state.
        /// </summary>
        public override void OnSceneNodeRemoved()
        {
            Debug.Assert(SceneControl.State.Nodes.Contains(Variable), "Scripting component does not contain this variable.");

            SceneControl.State.Nodes.Remove(Variable);
        }

        /// <summary>
        /// Updates any necessary variables for GUI.
        /// </summary>
        private void UpdateGui()
        {
            // set node bounds 
            _bounds = new RectangleF(Location.X, Location.Y, SizeSettings.VariableNodePadding.Left + nameSize.Width + SizeSettings.VariableNodePadding.Right,
                SizeSettings.VariableNodePadding.Top + nameSize.Height + SizeSettings.VariableNodePadding.Bottom);

            _center = new PointF(_bounds.X + _bounds.Width / 2f, _bounds.Y + _bounds.Height / 2f);

            // create rounded path of the node
            roundedRectangle = _bounds.CreateRoundedRectanglePath(SizeSettings.VariableCornerRadius);
            // set location of the name text 
            nameLocation = new PointF(Location.X + SizeSettings.VariableNodePadding.Left, Location.Y + SizeSettings.VariableNodePadding.Top);
            // set location of the comment text 
            commentLocation = new PointF(Location.X + SizeSettings.CommentPadding.X, Location.Y - DrawingTools.Font.GetHeight() - SizeSettings.CommentPadding.Y);
        }

        /// <inheritdoc />
        public override void Paint(Graphics graphics)
        {
            // background
            DrawingTools.SolidBrush.Color = ColorSettings.ForVariableBackground(Variable.VariableType, SelectState);
            graphics.FillPath(DrawingTools.SolidBrush, roundedRectangle);

            // border
            float originalPenWidth = DrawingTools.Pen.Width;
            DrawingTools.Pen.Color = ColorSettings.ForVariable(Variable.VariableType, SelectState);
            DrawingTools.Pen.Width = 2f;
            graphics.DrawPath(DrawingTools.Pen, roundedRectangle);
            DrawingTools.Pen.Width = originalPenWidth;

            // name (value)
            DrawingTools.SolidBrush.Color = ColorSettings.VariableText;
            graphics.DrawString(Name, DrawingTools.Font, DrawingTools.SolidBrush, nameLocation);

            // paint comment text
            if (!String.IsNullOrEmpty(Variable.Comment))
            {
                DrawingTools.SolidBrush.Color = ColorSettings.CommentText;
                graphics.DrawString(Variable.Comment, DrawingTools.Font, DrawingTools.SolidBrush, commentLocation);
            }
        }

        /// <inheritdoc />
        public override SceneNode Clone()
        {
            return new VariableView((Variable)Variable.Clone());
        }

        /// <inheritdoc />
        /// <remarks>
        /// Variable node is able to connect to variable node socket of the same type if possible.
        /// </remarks>
        public bool Connection(NodeSocketView socket)
        {
            return socket.NodeSocket.NodeSocketData.VariableType == Variable.VariableType &&
                (socket.Type == NodeSocketType.VariableIn || socket.Type == NodeSocketType.VariableOut) &&
                (socket.NodeSocket.NodeSocketData.IsArray ||
                (!socket.NodeSocket.NodeSocketData.IsArray && ((VariableNodeSocket)socket.NodeSocket).Connections.Count == 0));
        }

        /// <inheritdoc />
        /// <remarks>
        /// Variable node is able to connect to variable node socket of the same type if possible.
        /// </remarks>
        public ConnectionView MakeConnection(NodeSocketView socket)
        {
            if (Connection(socket))
            {
                if (socket.Type == NodeSocketType.VariableIn)
                {
                    if (!this.ContainsConnection(this, socket)) return new ConnectionView(this, socket);
                }
                else if (socket.Type == NodeSocketType.VariableOut)
                {
                    if (!this.ContainsConnection(socket, this)) return new ConnectionView(socket, this);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public void OnConnectionAdded(IConnecting connector)
        {
        }

        /// <inheritdoc />
        public void OnConnectionRemoved(IConnecting connector)
        {
        }

        private StringVar commentVar;
        private StringVar nameVar;

        /// <inheritdoc />
        public void ShowSettings(DataGridViewRowCollection rows)
        {
            // title
            DataGridViewRow row = new DataGridViewRow();

            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3, Title = String.Format("Varible of {0}", VariableTypeHelper.FriendlyName(Variable.VariableType)) });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;
            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3 });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;
            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3 });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;
            row.Cells.Add(new DataGridViewTitleCell() { LeftColumn = 0, RightColumn = 3 });
            row.Cells[row.Cells.Count - 1].ReadOnly = true;

            rows.Add(row);

            // name
            if (Variable.NamedVariable != null)
            {
                if (nameVar == null)
                {
                    nameVar = new StringVar() { Value = Variable.NamedVariable.Name };
                }

                row = new DataGridViewRow();

                row.Cells.Add(new DataGridViewDisableCheckBoxCell() { Enabled = false });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Name" });
                row.Cells.Add(nameVar.GetGridCell());
                row.Cells[row.Cells.Count - 1].ReadOnly = true;
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" });

                rows.Add(row);
            }

            // value
            row = new DataGridViewRow();

            row.Cells.Add(new DataGridViewDisableCheckBoxCell() { Enabled = false });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Value" });
            row.Cells.Add(Variable.Value.GetGridCell());
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = VariableTypeHelper.FriendlyName(Variable.VariableType) });

            rows.Add(row);

            // comment
            if (commentVar == null)
            {
                commentVar = new StringVar() { Value = Variable.Comment };
                commentVar.ValueChanged += (object sender, EventArgs e) => { Variable.Comment = commentVar.Value; };
            }

            row = new DataGridViewRow();

            row.Cells.Add(new DataGridViewDisableCheckBoxCell() { Enabled = false });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Comment" });
            row.Cells.Add(commentVar.GetGridCell());
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = VariableTypeHelper.FriendlyName(commentVar.VariableType) });

            rows.Add(row);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Variable.CommentChanged -= new EventHandler(Variable_CommentChanged);
                Variable.Value.ValueChanged -= new EventHandler(Value_ValueChanged);
                if (Variable.NamedVariable != null)
                {
                    Variable.NamedVariable.NameChanged -= new EventHandler(Value_ValueChanged);
                    Variable.NamedVariableChanged -= new ValueChangedHandler<NamedVariable>(Variable_NamedVariableChanged);
                }

                _variable = null;
            }
        }
    }
}
