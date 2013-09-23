/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Variables
{
    /// <summary>
    /// Sets the value of a variable using the value of another variable.
    /// </summary>
    /// <typeparam name="T">Type of the variable to set.</typeparam>
    [Category("Actions/Variables")]
    public abstract class SetVariableAction<T> : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Sets the value to set the targeted variable to.
        /// </summary>
        [Description("Sets the value to set the targeted variable to.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<T> Value;

        /// <summary>
        /// Sets the targeted variable to set the value of.
        /// </summary>
        [Description("Sets the targeted variable to set the value of.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<T>[] Target;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(Value.Value, Target);

            if (Out != null) Out();
        }
    }

    /// <summary>
    /// Sets the value of a bool variable using the value of another bool variable.
    /// </summary>
    [FriendlyName("Set Bool")]
    [Description("Sets the value of a bool variable using the value of another bool variable.")]
    public class SetBoolAction : SetVariableAction<bool>
    {

    }

    /// <summary>
    /// Sets the value of a int variable using the value of another int variable.
    /// </summary>
    [FriendlyName("Set Int")]
    [Description("Sets the value of a int variable using the value of another int variable.")]
    public class SetIntAction : SetVariableAction<int>
    {

    }

    /// <summary>
    /// Sets the value of a float variable using the value of another float variable.
    /// </summary>
    [FriendlyName("Set Float")]
    [Description("Sets the value of a float variable using the value of another float variable.")]
    public class SetFloatAction : SetVariableAction<float>
    {

    }

    /// <summary>
    /// Sets the value of a string variable using the value of another string variable.
    /// </summary>
    [FriendlyName("Set String")]
    [Description("Sets the value of a string variable using the value of another string variable.")]
    public class SetStringAction : SetVariableAction<string>
    {

    }

    /// <summary>
    /// Sets the value of a actor variable using the value of another actor variable.
    /// </summary>
    [FriendlyName("Set Actor")]
    [Description("Sets the value of a actor variable using the value of another actor variable.")]
    public class SetActorAction : SetVariableAction<Actor>
    {

    }

    /// <summary>
    /// Sets the value of a actor type variable using the value of another actor type variable.
    /// </summary>
    [FriendlyName("Set Actor Type")]
    [Description("Sets the value of a actor type variable using the value of another actor type variable.")]
    public class SetUintAction : SetVariableAction<uint>
    {

    }

    /// <summary>
    /// Sets the value of a vector variable using the value of another vector variable.
    /// </summary>
    [FriendlyName("Set Vector2")]
    [Description("Sets the value of a vector variable using the value of another vector variable.")]
    public class SetVector2Action : SetVariableAction<Vector2>
    {

    }
}
