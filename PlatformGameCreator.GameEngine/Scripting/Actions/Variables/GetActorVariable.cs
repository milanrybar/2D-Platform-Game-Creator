/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Variables
{
    /// <summary>
    /// Gets the variable from the specified actor by the specified variable name.
    /// </summary>
    /// <typeparam name="T">Type of the variable.</typeparam>
    [Category("Actions/Variables/Actor Variables")]
    public abstract class GetActorVariableAction<T> : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actor to get the variable from.
        /// </summary>
        [Description("Actor to get the variable from.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Actor> Actor;

        /// <summary>
        /// Name of the variable from the specified actor.
        /// </summary>
        [FriendlyName("Variable Name")]
        [Description("Name of the variable from the specified actor.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string> Name;

        /// <summary>
        /// Outputs the variable specified by the name and actor, if any.
        /// </summary>
        [Description("Outputs the variable specified by the name and actor, if any.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<T>[] Variable;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Actor != null && Actor.Value != null)
            {
                Variable<T> variable = Actor.Value.GetVariable<T>(Name.Value);

                if (variable != null && Variable != null)
                {
                    for (int i = 0; i < Variable.Length; ++i)
                    {
                        Variable[i].Parent = variable;
                    }
                }
            }

            if (Out != null) Out();
        }
    }

    /// <summary>
    /// Gets the bool variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get Bool Variable")]
    [Description("Gets the bool variable from the specified actor by specified variable name.")]
    public class GetBoolVariableAction : GetActorVariableAction<bool>
    {
    }

    /// <summary>
    /// Gets the int variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get Int Variable")]
    [Description("Gets the int variable from the specified actor by specified variable name.")]
    public class GetIntVariableAction : GetActorVariableAction<int>
    {
    }

    /// <summary>
    /// Gets the float variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get Float Variable")]
    [Description("Gets the float variable from the specified actor by specified variable name.")]
    public class GetFloatVariableAction : GetActorVariableAction<float>
    {
    }

    /// <summary>
    /// Gets the string variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get String Variable")]
    [Description("Gets the string variable from the specified actor by specified variable name.")]
    public class GetStringVariableAction : GetActorVariableAction<string>
    {
    }

    /// <summary>
    /// Gets the vector variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get Vector Variable")]
    [Description("Gets the vector variable from the specified actor by specified variable name.")]
    public class GetVectorVariableAction : GetActorVariableAction<Vector2>
    {
    }

    /// <summary>
    /// Gets the actor variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get Actor Variable")]
    [Description("Gets the actor variable from the specified actor by specified variable name.")]
    public class GetActorVariableAction : GetActorVariableAction<Actor>
    {
    }

    /// <summary>
    /// Gets the actor type variable from the specified actor by specified variable name.
    /// </summary>
    [FriendlyName("Get Actor Type Variable")]
    [Description("Gets the actor type variable from the specified actor by specified variable name.")]
    public class GetActorTypeVariableAction : GetActorVariableAction<uint>
    {
    }
}
