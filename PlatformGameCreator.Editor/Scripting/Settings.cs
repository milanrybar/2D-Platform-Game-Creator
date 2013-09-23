/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Color settings for <see cref="ScriptingScreen"/>, <see cref="SceneNode"/> and <see cref="StateMachineView"/>.
    /// Settings are outside classes for easier manipulation.
    /// </summary>
    static class ColorSettings
    {
        /// <summary>
        /// Color for a hovered <see cref="SceneNode"/>.
        /// </summary>
        public static readonly Color Hover = Color.FromArgb(255, 255, 240);

        /// <summary>
        /// Color for a selected <see cref="SceneNode"/>.
        /// </summary>
        public static readonly Color Select = Color.FromArgb(255, 255, 196);

        /// <summary>
        /// Color for the text of the script node socket.
        /// </summary>
        public static readonly Color NodeSocketText = Color.FromArgb(167, 174, 189);

        /// <summary>
        /// Color for the title of the script node.
        /// </summary>
        public static readonly Color NodeTitle = Color.FromArgb(18, 19, 21);

        /// <summary>
        /// Color for the text of the script variable node socket.
        /// </summary>
        public static readonly Color NodeVariableSocketValueText = Color.White;

        /// <summary>
        /// Color for the background of the script signal node socket.
        /// </summary>
        public static readonly Color SignalNodeSocket = Color.Black;

        /// <summary>
        /// Color for the text representing a comment.
        /// </summary>
        public static readonly Color CommentText = Color.White;

        /// <summary>
        /// Color for the text of the script variable.
        /// </summary>
        public static readonly Color VariableText = Color.White;

        /// <summary>
        /// Color of selecting rectangle for selecting scene nodes at the <see cref="ScriptingScreen"/>.
        /// </summary>
        public static readonly Color SelectingNodesRectangle = Color.FromArgb(100, 255, 0, 0);

        /// <summary>
        /// Color of the text of the state at the state machine.
        /// </summary>
        public static readonly Color StateText = Color.FromArgb(18, 19, 21);

        /// <summary>
        /// Color of the transition at the state machine.
        /// </summary>
        public static readonly Color StateTransition = Color.FromArgb(61, 69, 82);

        /// <summary>
        /// Color of the text of the transition at the state machine.
        /// </summary>
        public static readonly Color StateTransitionText = Color.FromArgb(167, 174, 189);

        /// <summary>
        /// Color for background of <see cref="ScriptingScreen"/>.
        /// </summary>
        public static readonly Color ScriptingScreenBackground = Color.FromArgb(102, 111, 126);

        /// <summary>
        /// Gets the color for border of the node of the specified type.
        /// </summary>
        /// <param name="nodeType">Type of the node.</param>
        /// <returns>Color for border of the specified node.</returns>
        public static Color ForNode(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Event:
                    return Color.FromArgb(255, 180, 74);

                case NodeType.Action:
                    return Color.FromArgb(167, 174, 189);

                default:
                    Debug.Assert(true, "Not supported node type.");
                    throw new Exception("Not supported node type.");
            }
        }

        /// <summary>
        /// Gets the color for border of the node of the specified type.
        /// </summary>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="selectState">State of the scene node.</param>
        /// <returns>Color for border of the specified node.</returns>
        public static Color ForNode(NodeType nodeType, SelectState selectState)
        {
            if (selectState == SelectState.Select) return Select;
            else if (selectState == SelectState.Hover) return Hover;
            else return ForNode(nodeType);
        }

        /// <summary>
        /// Gets the color for background of the node of the specified type.
        /// </summary>
        /// <param name="nodeType">Type of the node.</param>
        /// <returns>Color for background of the specified node.</returns>
        public static Color ForNodeBackground(NodeType nodeType)
        {
            return Color.FromArgb(61, 69, 82);
        }

        /// <summary>
        /// Gets the color for border of the variable of the specified type.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <returns>Color for border of the specified variable.</returns>
        public static Color ForVariable(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Bool:
                    return Color.FromArgb(255, 58, 58);

                case VariableType.Int:
                    return Color.FromArgb(0, 222, 255);

                case VariableType.Float:
                    return Color.FromArgb(72, 115, 255);

                case VariableType.String:
                    return Color.FromArgb(109, 224, 120);

                case VariableType.Actor:
                    return Color.FromArgb(200, 100, 255);

                case VariableType.ActorType:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Vector2:
                    return Color.FromArgb(243, 204, 110);

                case VariableType.Sound:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Song:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Path:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Texture:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Animation:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Scene:
                    return Color.FromArgb(255, 255, 255);

                case VariableType.Key:
                    return Color.FromArgb(255, 255, 255);

                default:
                    Debug.Assert(true, "Not supported variable type.");
                    throw new Exception("Not supported variable type.");
            }
        }

        /// <summary>
        /// Gets the color for border of the variable of the specified type.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="selectState">State of the scene node.</param>
        /// <returns>Color for border of the specified variable.</returns>
        public static Color ForVariable(VariableType variableType, SelectState selectState)
        {
            if (selectState == SelectState.Select) return Select;
            else if (selectState == SelectState.Hover) return Hover;
            else return ForVariable(variableType);
        }

        /// <summary>
        /// Gets the color for background of the variable of the specified type.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <returns>Color for background of the specified variable.</returns>
        public static Color ForVariableBackground(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Bool:
                    return Color.FromArgb(85, 61, 72);

                case VariableType.Int:
                    return Color.FromArgb(54, 92, 101);

                case VariableType.Float:
                    return Color.FromArgb(54, 72, 103);

                case VariableType.Actor:
                    return Color.FromArgb(83, 61, 103);

                case VariableType.String:
                    return Color.FromArgb(58, 92, 72);

                case VariableType.ActorType:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Vector2:
                    return Color.FromArgb(85, 86, 78);

                case VariableType.Sound:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Song:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Path:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Texture:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Animation:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Scene:
                    return Color.FromArgb(85, 92, 103);

                case VariableType.Key:
                    return Color.FromArgb(85, 92, 103);

                default:
                    Debug.Assert(true, "Not supported variable type.");
                    throw new Exception("Not supported variable type.");
            }
        }

        /// <summary>
        /// Gets the color for background of the variable of the specified type.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="selectState">State of the scene node.</param>
        /// <returns>Color for background of the specified variable.</returns>
        public static Color ForVariableBackground(VariableType variableType, SelectState selectState)
        {
            if (selectState == SelectState.Select) return Color.FromArgb(85, 90, 78);
            else return ForVariableBackground(variableType);
        }

        /// <summary>
        /// Gets the color for border of the specified state at the state machine.
        /// </summary>
        /// <param name="selectState">Select state of the state.</param>
        /// <param name="startingState">If set to <c>true</c> the state is the starting state.</param>
        /// <returns>Color for border of the specified state.</returns>
        public static Color ForState(SelectState selectState, bool startingState)
        {
            if (selectState == SelectState.Select) return Select;
            else if (selectState == SelectState.Hover) return Hover;
            else if (startingState) return Color.FromArgb(255, 180, 74);
            else return Color.FromArgb(167, 174, 189);
        }
    }

    /// <summary>
    /// Size settings for <see cref="ScriptingScreen"/>, <see cref="SceneNode"/> and <see cref="StateMachineView"/>.
    /// Settings are outside classes for easier manipulation.
    /// </summary>
    static class SizeSettings
    {
        /// <summary>
        /// Padding for the title area of the script node.
        /// </summary>
        public static Padding NodeTitlePadding = new Padding(5, 3, 5, 3);

        /// <summary>
        /// Padding for the body area of the script node.
        /// </summary>
        public static Padding NodeBodyPadding = new Padding(8);

        /// <summary>
        /// Padding for the node sockets of the script node.
        /// </summary>
        public static Padding NodeSocketPadding = new Padding(10, 5, 10, 5);

        /// <summary>
        /// Padding for the text of the script variable node.
        /// </summary>
        public static Padding VariableNodePadding = new Padding(15);

        /// <summary>
        /// Corner radius of the script variable node.
        /// </summary>
        public static int VariableCornerRadius = 25;

        /// <summary>
        /// Padding for the text comment of the script node.
        /// </summary>
        public static Point CommentPadding = new Point(2, 5);

        /// <summary>
        /// Size of the script node socket.
        /// </summary>
        public static int NodeSocketSize = 10;

        /// <summary>
        /// Padding for the text of script state.
        /// </summary>
        public static Padding StateTextPadding = new Padding(10, 5, 10, 5);
    }
}
