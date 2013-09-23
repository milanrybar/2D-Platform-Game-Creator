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
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Defines state for <see cref="SceneNode"/> when selecting objects at the <see cref="ScriptingScreen"/>.
    /// </summary>
    enum SelectState
    {
        /// <summary>
        /// Default state.
        /// </summary>
        Default,

        /// <summary>
        /// <see cref="SceneNode"/> is hovered.
        /// </summary>
        Hover,

        /// <summary>
        /// <see cref="SceneNode"/> is selected.
        /// </summary>
        Select
    };

    /// <summary>
    /// Represents container of script node sockets.
    /// </summary>
    interface INodeSocketsContainer
    {
        /// <summary>
        /// Finds the script node socket at the specified point.
        /// </summary>
        /// <param name="point">The point where to find.</param>
        /// <returns><see cref="NodeSocketView"/> if found; otherwise <c>null</c>.</returns>
        NodeSocketView NodeSocketContains(PointF point);

        /// <summary>
        /// Gets all connections from all script node sockets.
        /// </summary>
        /// <returns>List of <see cref="ConnectionView"/> from all script node sockets.</returns>
        List<ConnectionView> GetAllConnections();

        /// <summary>
        /// Gets the <see cref="NodeSocketView"/> by the specified realname and type of the script node socket.
        /// </summary>
        /// <param name="realName">Realname in the assembly of the script node socket.</param>
        /// <param name="nodeSocketType">Type of the script node socket.</param>
        /// <returns><see cref="NodeSocketView"/> if found; otherwise <c>null</c>.</returns>
        NodeSocketView GetSocketByName(string realName, NodeSocketType nodeSocketType);
    }

    /// <summary>
    /// Interface containing drawing tools for <see cref="SceneNode"/>.
    /// </summary>
    interface ISceneDrawingTools
    {
        /// <summary>
        /// Gets the solid brush.
        /// </summary>
        SolidBrush SolidBrush { get; }

        /// <summary>
        /// Gets the pen.
        /// </summary>
        Pen Pen { get; }

        /// <summary>
        /// Gets the pen for drawing a line.
        /// </summary>
        Pen LinePen { get; }

        /// <summary>
        /// Gets the font.
        /// </summary>
        Font Font { get; }

        /// <summary>
        /// Gets the bold font.
        /// </summary>
        Font BoldFont { get; }
    }

    /// <summary>
    /// Represents a script node that is able to connect to the other script node socket.
    /// </summary>
    interface IConnecting
    {
        /// <summary>
        /// Determines whether the connection between this script node and the specified script node socket can be made.
        /// </summary>
        /// <param name="socket">The script node socket to connect.</param>
        /// <returns><c>true</c> if the connection can be made; otherwise <c>false</c>.</returns>
        bool Connection(NodeSocketView socket);

        /// <summary>
        /// Makes the connection between this script node and the specified script node socket if possible.
        /// </summary>
        /// <param name="socket">The script node socket to connect.</param>
        /// <returns><see cref="ConnectionView"/> of the connection if the connection is made; otherwise <c>null</c>.</returns>
        ConnectionView MakeConnection(NodeSocketView socket);

        /// <summary>
        /// Gets the center of the script node for the connection line. Used by <see cref="ConnectionView"/>.
        /// </summary>
        PointF Center { get; }

        /// <summary>
        /// Gets the color for the connection line. Used by <see cref="ConnectionView"/>.
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// Gets the connections of the script node.
        /// </summary>
        List<ConnectionView> Connections { get; }

        /// <summary>
        /// Gets the parent of this script node.
        /// </summary>
        SceneNode Parent { get; }

        /// <summary>
        /// Called when the connection is made with the specified <see cref="IConnecting"/> instance.
        /// </summary>
        /// <param name="connector">The other connector.</param>
        void OnConnectionAdded(IConnecting connector);

        /// <summary>
        /// Called when the connection is removed with the specified <see cref="IConnecting"/> instance.
        /// </summary>
        /// <param name="connector">The other connector.</param>
        void OnConnectionRemoved(IConnecting connector);
    }

    /// <summary>
    /// Extension method for <see cref="IConnecting"/>.
    /// </summary>
    static class IConnectingExtension
    {
        /// <summary>
        /// Determines whether the specified <see cref="IConnecting"/> instance contains a connection between two specified <see cref="IConnecting"/> instances.
        /// </summary>
        /// <param name="connecting">The connecting where to look for connection.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns><c>true</c> if the specified connection exists; otherwise <c>false</c>.</returns>
        public static bool ContainsConnection(this IConnecting connecting, IConnecting from, IConnecting to)
        {
            foreach (ConnectionView connection in connecting.Connections)
            {
                if (connection.From == from && connection.To == to)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Represents the script node that its settings can be edited at GUI.
    /// </summary>
    interface IEditSettings
    {
        /// <summary>
        /// Shows the settings to edit of this script node.
        /// </summary>
        /// <param name="rows">The rows where to add the fields.</param>
        void ShowSettings(DataGridViewRowCollection rows);
    }

    /// <summary>
    /// Base class for element that can be used and shown at the scripting scene (at <see cref="ScriptingScreen"/> control).
    /// </summary>
    abstract class SceneNode : Disposable
    {
        /// <summary>
        /// Gets or sets the underlying <see cref="ScriptingScreen"/> where the scene node is used.
        /// </summary>
        public ScriptingScreen SceneControl { get; set; }

        /// <summary>
        /// Gets or sets the state of the scene node when selecting objects at the scene.
        /// </summary>
        public SelectState SelectState { get; set; }

        /// <summary>
        /// Gets or sets the location of the scene node at the scene.
        /// </summary>
        public abstract PointF Location { get; set; }

        /// <summary>
        /// Gets the size and location of the scene node.
        /// </summary>
        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// Gets a value indicating whether this scene node can be moved.
        /// </summary>
        public abstract bool CanMove { get; }

        /// <summary>
        /// Gets a value indicating whether this scene node can be cloned.
        /// </summary>
        public abstract bool CanClone { get; }

        /// <summary>
        /// Gets a value indicating whether this scene node can be connected to the script node socket.
        /// </summary>
        public abstract bool CanConnect { get; }

        /// <summary>
        /// Gets a value indicating whether this settings of the scene node can edited.
        /// </summary>
        public abstract bool CanEditSettings { get; }

        /// <summary>
        /// Gets a value indicating whether this scene node is node container of script node socket.
        /// </summary>
        public abstract bool IsSocketsContainer { get; }

        /// <summary>
        /// Gets <see cref="IConnecting"/> interface or <c>null</c> when property <see cref="CanConnect"/> is <c>false</c>.
        /// </summary>
        public abstract IConnecting IConnecting { get; }

        /// <summary>
        /// Gets <see cref="INodeSocketsContainer"/> interface or <c>null</c> when property <see cref="IsSocketsContainer"/> is <c>false</c>.
        /// </summary>
        public abstract INodeSocketsContainer INodeSocketsContainer { get; }

        /// <summary>
        /// Gets <see cref="IEditSettings"/> interface or <c>null</c> when property <see cref="CanEditSettings"/> is <c>false</c>.
        /// </summary>
        public abstract IEditSettings IEditSettings { get; }

        /// <summary>
        /// Gets or sets drawing tools for the scene node.
        /// </summary>
        public static ISceneDrawingTools DrawingTools { get; set; }

        /// <summary>
        /// Determines whether the scene node contains the specified point.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <returns>Returns <c>true</c> if the scene node contains the specified point, otherwise <c>false</c>.</returns>
        public abstract bool Contains(PointF point);

        /// <summary>
        /// Paints the scene node.
        /// </summary>
        /// <param name="graphics"><see cref="Graphics"/> instance for painting.</param>
        public abstract void Paint(Graphics graphics);

        /// <summary>
        /// Clones this scene node.
        /// </summary>
        /// <returns>Cloned <see cref="SceneNode"/> or <c>null</c> when property <see cref="CanClone"/> is <c>false</c>.</returns>
        public abstract SceneNode Clone();

        /// <summary>
        /// Called when the scene node is added to the <see cref="SceneControl"/>.
        /// </summary>
        public abstract void OnSceneNodeAdded();

        /// <summary>
        /// Called when the scene node is removed from the <see cref="SceneControl"/>.
        /// </summary>
        public abstract void OnSceneNodeRemoved();

        /// <summary>
        /// Determines whether the scene node intersects with the specified rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check.</param>
        /// <returns>Returns <c>true</c> if the scene node intersects with the specified rectangle, otherwise <c>false</c>.</returns>
        public virtual bool IntersectsWith(ref RectangleF rectangle)
        {
            return Bounds.IntersectsWith(rectangle);
        }

        /// <summary>
        /// Invalidates this scene node.
        /// </summary>
        public void Invalidate()
        {
            SceneControl.Invalidate();
        }
    }

    /// <summary>
    /// Command for moving the <see cref="SceneNode"/> at the <see cref="ScriptingScreen"/>.
    /// </summary>
    class SceneNodeMoveCommand : Command
    {
        /// <summary>
        /// The scene node to move.
        /// </summary>
        private SceneNode scriptSceneNode;

        /// <summary>
        /// Move vector to move the scene node by.
        /// </summary>
        private PointF move;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodeMoveCommand"/> class.
        /// </summary>
        /// <param name="sceneNode">The scene node to move.</param>
        /// <param name="move">The move vector to move by.</param>
        public SceneNodeMoveCommand(SceneNode sceneNode, PointF move)
        {
            this.scriptSceneNode = sceneNode;
            this.move = move;
        }

        /// <summary>
        /// Moves the <see cref="SceneNode"/> by the move vector.
        /// </summary>
        public override void Do()
        {
            scriptSceneNode.Location = scriptSceneNode.Location.Add(move);
        }

        /// <summary>
        /// Moves the <see cref="SceneNode"/> back by the move vector.
        /// </summary>
        public override void Undo()
        {
            scriptSceneNode.Location = scriptSceneNode.Location.Sub(move);
        }
    }
}
