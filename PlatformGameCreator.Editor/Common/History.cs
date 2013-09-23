/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.Editor.Common
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    abstract class Command
    {
        /// <summary>
        /// Does the command represented by this instance.
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// Undoes the command represented by this instance.
        /// </summary>
        public abstract void Undo();
    }

    /// <summary>
    /// Command for storing more commands as a single command.
    /// </summary>
    class CompositeCommand : Command
    {
        /// <summary>
        /// List of commands.
        /// </summary>
        public List<Command> Commands = new List<Command>();

        /// <summary>
        /// Does all stored commands.
        /// </summary>
        public override void Do()
        {
            foreach (Command command in Commands)
            {
                command.Do();
            }
        }

        /// <summary>
        /// Undoes all stored commands.
        /// </summary>
        public override void Undo()
        {
            foreach (Command command in Commands)
            {
                command.Undo();
            }
        }
    }

    /// <summary>
    /// Represents a history of commands. History can be undone and redone.
    /// </summary>
    class History
    {
        /// <summary>
        /// Stack of undo commands.
        /// </summary>
        private Stack<Command> undo = new Stack<Command>();

        /// <summary>
        /// Stack of redo commands.
        /// </summary>
        private Stack<Command> redo = new Stack<Command>();

        /// <summary>
        /// Adds the specified command to the history.
        /// </summary>
        /// <remarks>
        /// The specified command is pushed to the undo stack and all redo commands are removed from the history.
        /// </remarks>
        /// <param name="command">The command to add.</param>
        public void Add(Command command)
        {
            undo.Push(command);
            if (redo.Count != 0) redo.Clear();
        }

        /// <summary>
        /// Undoes the last command, if any.
        /// </summary>
        /// <remarks>
        /// The last command of undo stack is undone and push to the redo stack.
        /// </remarks>
        public void Undo()
        {
            if (undo.Count != 0)
            {
                // get last command
                Command lastCommand = undo.Pop();
                // undo command
                lastCommand.Undo();
                // move to redo
                redo.Push(lastCommand);
            }
        }

        /// <summary>
        /// Redoes the last command, if any.
        /// </summary>
        /// <remarks>
        /// The last command of redo stack is redone and push to the undo stack.
        /// </remarks>
        public void Redo()
        {
            if (redo.Count != 0)
            {
                // get last command
                Command lastCommand = redo.Pop();
                // do command
                lastCommand.Do();
                // move to undo
                undo.Push(lastCommand);
            }
        }

        /// <summary>
        /// Removed all commands from the history.
        /// </summary>
        public void Clear()
        {
            undo.Clear();
            redo.Clear();
        }
    }
}
