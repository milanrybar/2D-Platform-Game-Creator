/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Defines type of message for the messages system (<see cref="Messages"/>).
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Represents information message.
        /// </summary>
        Info,

        /// <summary>
        /// Represents warning message.
        /// </summary>
        Warning,

        /// <summary>
        /// Represents error message.
        /// </summary>
        Error
    };

    /// <summary>
    /// Interface for implementing messages manager to be used at <see cref="Messages"/>.
    /// </summary>
    /// <remarks>
    /// Implements how the messages are shown to the user.
    /// </remarks>
    /// <seealso cref="Messages"/>
    interface IMessagesManager
    {
        /// <summary>
        /// Shows the specified information message.
        /// </summary>
        /// <param name="message">The message to show.</param>
        void ShowInfo(string message);

        /// <summary>
        /// Shows the specified warning message.
        /// </summary>
        /// <param name="message">The message to show.</param>
        void ShowWarning(string message);

        /// <summary>
        /// Shows the specified error message.
        /// </summary>
        /// <param name="message">The message to show.</param>
        void ShowError(string message);
    }

    /// <summary>
    /// Represents a system that shows messages to the user.
    /// </summary>
    /// <remarks>
    /// Classes that needs messages system do not know anything about the current implementation of the messages manager.
    /// They do not know how will the message is shown to the user. 
    /// Every Form sets their messages manager that will show messages how the Form wants.
    /// </remarks>
    static class Messages
    {
        /// <summary>
        /// Gets or sets the messages manager that is currently used.
        /// </summary>
        public static IMessagesManager MessagesManager { get; set; }

        /// <summary>
        /// Shows the specified message to the user.
        /// </summary>
        /// <param name="message">The message to show to the user.</param>
        /// <param name="type">Type of the message.</param>
        public static void Show(string message, MessageType type)
        {
            if (MessagesManager == null) throw new Exception("Messages manager not set.");

            switch (type)
            {
                case MessageType.Info:
                    MessagesManager.ShowInfo(message);
                    break;

                case MessageType.Warning:
                    MessagesManager.ShowWarning(message);
                    break;

                case MessageType.Error:
                    MessagesManager.ShowError(message);
                    break;

                default:
                    Debug.Assert(true, "Not supported message type.");
                    break;
            }
        }

        /// <summary>
        /// Shows the specified information message to the user.
        /// </summary>
        /// <param name="message">The message to show to the user.</param>
        public static void ShowInfo(string message)
        {
            Show(message, MessageType.Info);
        }

        /// <summary>
        /// Shows the specified warning message to the user.
        /// </summary>
        /// <param name="message">The message to show to the user.</param>
        public static void ShowWarning(string message)
        {
            Show(message, MessageType.Warning);
        }

        /// <summary>
        /// Shows the specified error message to the user.
        /// </summary>
        /// <param name="message">The message to show to the user.</param>
        public static void ShowError(string message)
        {
            Show(message, MessageType.Error);
        }
    }

    /// <summary>
    /// Represents the messages manager that is used by the the most Forms.
    /// </summary>
    /// <remarks>
    /// Info and warning message is shown at the <see cref="ToolStripStatusLabel"/> of the <see cref="ToolStrip"/>.
    /// Error message is shown by the <see cref="MessageBox"/>.
    /// </remarks>
    class DefaultMessagesManager : IMessagesManager
    {
        /// <summary>
        /// Gets the <see cref="ToolStripStatusLabel"/> where to show information and warning message.
        /// </summary>
        public ToolStripStatusLabel StatusLabel
        {
            get { return _statusLabel; }
        }
        private ToolStripStatusLabel _statusLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMessagesManager"/> class.
        /// </summary>
        /// <param name="statusLabel">The <see cref="ToolStripStatusLabel"/> to use for messages.</param>
        public DefaultMessagesManager(ToolStripStatusLabel statusLabel)
        {
            if (statusLabel == null) throw new ArgumentNullException("Status label cannot be null value.");

            _statusLabel = statusLabel;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Show the information message at the <see cref="StatusLabel"/>.
        /// </remarks>
        public void ShowInfo(string message)
        {
            StatusLabel.BackColor = SystemColors.Control;
            StatusLabel.Text = message;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Show the warning message at the <see cref="StatusLabel"/> and is highlighted by the its background.
        /// </remarks>
        public void ShowWarning(string message)
        {
            StatusLabel.BackColor = Color.Yellow;
            StatusLabel.Text = message;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Show the error message by the <see cref="MessageBox"/>.
        /// </remarks>
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error");
        }
    }
}
