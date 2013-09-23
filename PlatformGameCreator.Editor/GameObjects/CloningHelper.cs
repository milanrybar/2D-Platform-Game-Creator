/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.GameObjects.Paths;
using System.Diagnostics;
using PlatformGameCreator.Editor.Scripting;

namespace PlatformGameCreator.Editor.GameObjects
{
    /// <summary>
    /// Helper for cloning group of <see cref="GameObject"/>.
    /// </summary>
    static class CloningHelper
    {
        /// <summary>
        /// Clones the specified game object.
        /// </summary>
        /// <param name="gameObject">The game object to clone.</param>
        /// <param name="layer">The layer where to add the cloned actor, if <paramref name="addToContainer"/> is <c>true</c>.</param>
        /// <param name="addToContainer">If set to <c>true</c> cloned object is added to the container.</param>
        /// <param name="updateNameWithCloneStatus">If set to <c>true</c> name of the game object will contains information that was cloned.</param>
        /// <returns>Cloned game object.</returns>
        public static GameObject Clone(GameObject gameObject, Layer layer = null, bool addToContainer = false, bool updateNameWithCloneStatus = false)
        {
            List<GameObject> cloned = Clone(new GameObject[1] { gameObject }, layer, addToContainer, updateNameWithCloneStatus);

            if (cloned.Count == 1) return cloned[0];
            else return null;
        }

        /// <summary>
        /// Clones the specified game objects.
        /// </summary>
        /// <param name="gameObjectsToCloned">The game objects to clone.</param>
        /// <param name="layer">The layer where to add the cloned actor, if <paramref name="addToContainer"/> is <c>true</c>.</param>
        /// <param name="addToContainer">If set to <c>true</c> cloned object is added to the container.</param>
        /// <param name="updateNameWithCloneStatus">If set to <c>true</c> name of the game object will contains information that was cloned.</param>
        /// <returns>Cloned game objects.</returns>
        public static List<GameObject> Clone(IEnumerable<GameObject> gameObjectsToCloned, Layer layer = null, bool addToContainer = false, bool updateNameWithCloneStatus = false)
        {
            List<GameObject> clonedObjects = new List<GameObject>();
            Dictionary<Actor, Actor> clonedActors = new Dictionary<Actor, Actor>();
            Dictionary<Path, Path> clonedPaths = new Dictionary<Path, Path>();

            // clone all game objects
            foreach (GameObject gameObject in gameObjectsToCloned)
            {
                // clone actor
                if (gameObject is Actor)
                {
                    Debug.Assert(!clonedActors.ContainsKey((Actor)gameObject), "Game object cannot be clonned twice.");

                    Actor clonedActor;
                    if (layer == null) clonedActor = (Actor)((Actor)gameObject).Clone(addToContainer);
                    else clonedActor = (Actor)((Actor)gameObject).Clone(layer, addToContainer);

                    clonedObjects.Add(clonedActor);
                    AddAllClonedActors((Actor)gameObject, clonedActor, clonedActors);

                    if (updateNameWithCloneStatus) clonedActor.Name = clonedActor.Name + "(Copy)";
                }
                // clone path
                else if (gameObject is Path)
                {
                    Debug.Assert(!clonedPaths.ContainsKey((Path)gameObject), "Game object cannot be clonned twice.");

                    Path clonedPath = (Path)gameObject.Clone(addToContainer);
                    clonedPaths.Add((Path)gameObject, clonedPath);
                    clonedObjects.Add(clonedPath);

                    if (updateNameWithCloneStatus) clonedPath.Name = clonedPath.Name + "(Copy)";
                }
                // clone other game object
                else
                {
                    clonedObjects.Add(gameObject.Clone(addToContainer));
                }
            }

            // check all cloned actors
            foreach (KeyValuePair<Actor, Actor> clonedGameObject in clonedActors)
            {
                Actor originalActor = clonedGameObject.Key;
                Actor clonedActor = clonedGameObject.Value;

                // all variables
                foreach (NamedVariable variable in clonedActor.Scripting.Variables)
                {
                    UpdateVariable(variable.Value, originalActor, clonedActors, clonedPaths);
                }

                // all state machines
                foreach (StateMachine stateMachine in clonedActor.Scripting.StateMachines)
                {
                    // all states
                    foreach (State state in stateMachine.States)
                    {
                        foreach (BaseNode baseNode in state.Nodes)
                        {
                            // all script nodes
                            if (baseNode is Node)
                            {
                                Node node = baseNode as Node;

                                foreach (NodeSocket nodeSocket in node.Sockets)
                                {
                                    // all variable node sockets
                                    if (nodeSocket is VariableNodeSocket)
                                    {
                                        VariableNodeSocket variableNodeSocket = nodeSocket as VariableNodeSocket;

                                        UpdateVariable(variableNodeSocket.Value, originalActor, clonedActors, clonedPaths);
                                    }
                                }
                            }
                            // all script variables
                            else if (baseNode is Variable)
                            {
                                Variable variable = baseNode as Variable;

                                if (variable.NamedVariable == null)
                                {
                                    UpdateVariable(variable.Value, originalActor, clonedActors, clonedPaths);
                                }
                                else
                                {
                                    Actor clonedNamedVariableOwner;
                                    if (variable.NamedVariable.ScriptingComponent.Actor != null && clonedActors.TryGetValue(variable.NamedVariable.ScriptingComponent.Actor, out clonedNamedVariableOwner))
                                    {
                                        variable.NamedVariable = FindNamedVariable(clonedNamedVariableOwner, variable.NamedVariable.Name);
                                        Debug.Assert(variable.NamedVariable != null, "Named variable not found");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return clonedObjects;
        }

        /// <summary>
        /// Adds the specified original and cloned actors and their children to the specified <paramref name="actorsToAdd"/>.
        /// </summary>
        /// <param name="originalActor">The original actor.</param>
        /// <param name="clonedActor">The cloned actor.</param>
        /// <param name="actorsToAdd">Container for storing original and cloned actor.</param>
        private static void AddAllClonedActors(Actor originalActor, Actor clonedActor, Dictionary<Actor, Actor> actorsToAdd)
        {
            Debug.Assert(!actorsToAdd.ContainsKey(originalActor), "Actor is already cloned.");

            actorsToAdd.Add(originalActor, clonedActor);

            for (int i = 0; i < originalActor.Children.Count; ++i)
            {
                AddAllClonedActors(originalActor.Children[i], clonedActor.Children[i], actorsToAdd);
            }
        }

        /// <summary>
        /// Updates the specified variable.
        /// When the variable is actor or path variable: checks if the value is at original/cloned container; 
        /// if yes then updates value to the cloned value.
        /// When the variable is actor variable: checks if the value is <paramref name="actorOwner"/>; if yes then sets value to Owner.
        /// </summary>
        /// <param name="variable">The variable to update.</param>
        /// <param name="actorOwner">The owner of the variable.</param>
        /// <param name="clonedActors">The original/cloned actors container.</param>
        /// <param name="clonedPaths">The original/cloned paths container.</param>
        private static void UpdateVariable(IVariable variable, Actor actorOwner, Dictionary<Actor, Actor> clonedActors, Dictionary<Path, Path> clonedPaths)
        {
            // actor variable
            if (variable.VariableType == VariableType.Actor && variable.GetValue() != null)
            {
                // set as owner
                if (variable.GetValue() == actorOwner)
                {
                    ((ActorVar)variable).Owner = true;
                }
                // set to cloned actor
                else
                {
                    SetValue(variable, clonedActors);
                }
            }
            // path variable
            else if (variable.VariableType == VariableType.Path && variable.GetValue() != null)
            {
                // set to cloned path
                SetValue(variable, clonedPaths);
            }
        }

        /// <summary>
        /// Updates the specified variable.
        /// If the value is at original/cloned container then updates value to the cloned value.
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="variable">The variable to update.</param>
        /// <param name="clonedObjects">The original/cloned container.</param>
        private static void SetValue<T>(IVariable variable, IDictionary<T, T> clonedObjects) where T : class
        {
            T newValue;
            if (clonedObjects.TryGetValue(variable.GetValue() as T, out newValue))
            {
                variable.SetValue(newValue);
            }
        }

        /// <summary>
        /// Finds the named variable by the specified name in the specified actor.
        /// </summary>
        /// <param name="owner">Actor where to find.</param>
        /// <param name="namedVariableName">Name of the named variable.</param>
        /// <returns>Named variable if found; otherwise <c>null</c>.</returns>
        private static NamedVariable FindNamedVariable(Actor owner, string namedVariableName)
        {
            foreach (NamedVariable namedVariable in owner.Scripting.Variables)
            {
                if (namedVariable.Name == namedVariableName)
                {
                    return namedVariable;
                }
            }

            return null;
        }
    }
}
