/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.Scripting;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.Assets;
using PlatformGameCreator.Editor.GameObjects.Paths;
using PlatformGameCreator.Editor.Assets.Sounds;
using System.Diagnostics;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Defines type of deletion for <see cref="ItemForDeletion"/>.
    /// </summary>
    enum DeletionType
    {
        /// <summary>
        /// Item will be removed. Meaning is defined by the specified item.
        /// </summary>
        Remove,

        /// <summary>
        /// Item will be cleared. Meaning is defined by the specified item.
        /// </summary>
        Clear
    };

    /// <summary>
    /// Represents safe deleting of an item that want to be deleted from the project.
    /// </summary>
    abstract class ItemForDeletion
    {
        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the type of deletion of the item.
        /// </summary>
        public DeletionType Type { get; set; }

        /// <summary>
        /// Gets the underlying object to delete.
        /// </summary>
        public abstract object Tag { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemForDeletion"/> class.
        /// </summary>
        protected ItemForDeletion()
        {
            Type = DeletionType.Clear;
        }

        /// <summary>
        /// Deletes this item according to <see cref="Type"/> value.
        /// </summary>
        public void Delete()
        {
            if (Type == DeletionType.Remove) DeleteRemove();
            else DeleteClear();
        }

        /// <summary>
        /// Clears this item according to <see cref="DeletionType.Clear"/> deletion type.
        /// </summary>
        protected abstract void DeleteClear();

        /// <summary>
        /// Removes this item according to <see cref="DeletionType.Remove"/> deletion type.
        /// </summary>
        protected abstract void DeleteRemove();

        /// <summary>
        /// Gets the items that need to be also deleted according to <see cref="Type"/> value. Provides safe deleting.
        /// </summary>
        /// <param name="items">Container to store items to delete.</param>
        public virtual void ItemsToDelete(HashSet<ItemForDeletion> items)
        {
        }

        /// <summary>
        /// Gets the path of the item. Items are stored from this item to its root (that is project).
        /// </summary>
        /// <param name="path">Container to store items on the path.</param>
        public abstract void GetPath(List<ItemOnPath> path);

        /// <summary>
        /// Creates the item on path representing the specified actor.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Item on the path representing the specified actor.</returns>
        protected ItemOnPath CreateItemOnPath(Actor actor)
        {
            return new ItemOnPath(actor.Name, "Actor", actor);
        }

        /// <summary>
        /// Creates the item on path representing the specified scene.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns>Item on the path representing the specified scene.</returns>
        protected ItemOnPath CreateItemOnPath(Scene scene)
        {
            return new ItemOnPath(scene.Name, "Scene", scene);
        }

        /// <summary>
        /// Creates the item on path representing the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Item on the path representing the specified node.</returns>
        protected ItemOnPath CreateItemOnPath(Node node)
        {
            return new ItemOnPath(node.Name, "Node", node);
        }

        /// <summary>
        /// Creates the item on path representing the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>Item on the path representing the specified state.</returns>
        protected ItemOnPath CreateItemOnPath(State state)
        {
            return new ItemOnPath(state.Name, "State", state);
        }

        /// <summary>
        /// Creates the item on path representing the specified state machine.
        /// </summary>
        /// <param name="stateMachine">The state machine.</param>
        /// <returns>Item on the path representing the specified state machine.</returns>
        protected ItemOnPath CreateItemOnPath(StateMachine stateMachine)
        {
            return new ItemOnPath(stateMachine.Name, "State Machine", stateMachine);
        }

        /// <summary>
        /// Gets the path from the specified actor.
        /// </summary>
        /// <param name="actor">The actor where to start.</param>
        /// <param name="path">Container to store items on the path.</param>
        protected void GetPathFromActor(Actor actor, List<ItemOnPath> path)
        {
            path.Add(CreateItemOnPath(actor));

            Actor currentActor = actor;
            while (currentActor.Parent != null)
            {
                currentActor = currentActor.Parent;
                path.Add(CreateItemOnPath(currentActor));
            }

            if (currentActor.Layer != null) path.Add(CreateItemOnPath(currentActor.Layer.Scene));
            else path.Add(new ItemOnPath("Prototypes", null, Project.Singleton.Prototypes));
        }

        /// <summary>
        /// Gets the path from the specified scripting.
        /// </summary>
        /// <param name="scripting">The scripting where to start.</param>
        /// <param name="path">Container to store items on the path.</param>
        protected void GetPathFromScripting(ScriptingComponent scripting, List<ItemOnPath> path)
        {
            if (scripting.Actor != null && scripting.Scene == null)
            {
                GetPathFromActor(scripting.Actor, path);
            }
            else if (scripting.Actor == null && scripting.Scene != null)
            {
                path.Add(new ItemOnPath("Scene Scripting", null, scripting));
                path.Add(CreateItemOnPath(scripting.Scene));
            }
            else Debug.Assert(true, "Not possible combination");
        }
    }

    /// <summary>
    /// Helper for creating <see cref="ItemForDeletion"/> and safe deleting.
    /// </summary>
    static class ConsistentDeletionHelper
    {
        /// <summary>
        /// Represents an <see cref="Actor"/> that want to be deleted from the project.
        /// </summary>
        public class ActorForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return actor; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Actor - {0}", actor.Name); }
            }

            /// <summary>
            /// The underlying actor.
            /// </summary>
            protected Actor actor;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActorForDeletion"/> class.
            /// </summary>
            /// <param name="actor">The actor to delete.</param>
            public ActorForDeletion(Actor actor)
            {
                this.actor = actor;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not suported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Actor do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            /// <remarks>
            /// Also closes the scripting form for this actor, if any is opened.
            /// </remarks>
            protected override void DeleteRemove()
            {
                EditorApplication.Editor.CloseScriptingForm(actor.Scripting);
                actor.Remove();
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // actor named variables can be used at other actors
                    foreach (NamedVariable variable in actor.Scripting.Variables)
                    {
                        (new ConsistentDeletionHelper.ScriptNamedVariableForDeletion(variable) { Type = DeletionType.Remove }).ItemsToDelete(items);
                    }

                    // actor can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Actor, actor, items);

                    // the same checking for the children
                    foreach (Actor child in actor.Children)
                    {
                        (new ConsistentDeletionHelper.ActorForDeletion(child) { Type = DeletionType.Remove }).ItemsToDelete(items);
                    }
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                GetPathFromActor(actor, path);
            }
        }

        /// <summary>
        /// Represents a <see cref="Path"/> that want to be deleted from the project.
        /// </summary>
        public class PathForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return path; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Path - {0}", path.Name); }
            }

            /// <summary>
            /// The underlying path.
            /// </summary>
            private Path path;

            /// <summary>
            /// Initializes a new instance of the <see cref="PathForDeletion"/> class.
            /// </summary>
            /// <param name="path">The path to delete.</param>
            public PathForDeletion(Path path)
            {
                this.path = path;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not supported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Path do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                path.Remove();
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // path can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Path, path, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Paths", String.Empty, Project.Singleton.Scenes.SelectedScene.Paths));
                path.Add(CreateItemOnPath(Project.Singleton.Scenes.SelectedScene));
            }
        }

        /// <summary>
        /// Represents a <see cref="Texture"/> that want to be deleted from the project.
        /// </summary>
        public class TextureForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return texture; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Texture - {0}", texture.Name); }
            }

            /// <summary>
            /// The underlying texture.
            /// </summary>
            private Texture texture;

            /// <summary>
            /// Initializes a new instance of the <see cref="TextureForDeletion"/> class.
            /// </summary>
            /// <param name="texture">The texture to delete.</param>
            public TextureForDeletion(Texture texture)
            {
                this.texture = texture;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not supported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Texture do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                Project.Singleton.Textures.Remove(texture);
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // texture can be used at animations
                    ConsistentDeletionHelper.DeleteFromAnimations(texture, items);
                    // texture can be used at actors
                    ConsistentDeletionHelper.DeleteFromActors(texture, items);
                    // texture can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Texture, texture, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Textures", String.Empty, Project.Singleton.Textures));
            }
        }

        /// <summary>
        /// Represents an <see cref="Animation"/> that want to be deleted from the project.
        /// </summary>
        public class AnimationForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return animation; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Animation - {0}", animation.Name); }
            }

            /// <summary>
            /// The underlying animation.
            /// </summary>
            protected Animation animation;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnimationForDeletion"/> class.
            /// </summary>
            /// <param name="animation">The animation to delete.</param>
            public AnimationForDeletion(Animation animation)
            {
                this.animation = animation;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not supported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Animation do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                Project.Singleton.Animations.Remove(animation);
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // animation can be used at actors
                    ConsistentDeletionHelper.DeleteFromActors(animation, items);
                    // animation can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Animation, animation, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Animations", String.Empty, Project.Singleton.Animations));
            }
        }

        /// <summary>
        /// Represents a <see cref="Sound"/> that want to be deleted from the project.
        /// </summary>
        public class SoundForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return sound; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Sound - {0}", sound.Name); }
            }

            /// <summary>
            /// The underlying sound.
            /// </summary>
            private Sound sound;

            /// <summary>
            /// Initializes a new instance of the <see cref="SoundForDeletion"/> class.
            /// </summary>
            /// <param name="sound">The sound to delete.</param>
            public SoundForDeletion(Sound sound)
            {
                this.sound = sound;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not supported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Sound do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                Project.Singleton.Sounds.Remove(sound);
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // sound can be used at the scripting as Sound
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Sound, sound, items);
                    // sound can be used at the scripting as Song
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Song, sound, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Sounds", String.Empty, Project.Singleton.Animations));
            }
        }

        /// <summary>
        /// Represents a <see cref="Scene"/> that want to be deleted from the project.
        /// </summary>
        public class SceneForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return scene; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Scene - {0}", scene.Name); }
            }

            /// <summary>
            /// The underlying scene.
            /// </summary>
            private Scene scene;

            /// <summary>
            /// Initializes a new instance of the <see cref="SceneForDeletion"/> class.
            /// </summary>
            /// <param name="scene">The scene to delete.</param>
            public SceneForDeletion(Scene scene)
            {
                this.scene = scene;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not supported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Scene do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                Project.Singleton.Scenes.Remove(scene);
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // scene can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.Scene, scene, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Scenes", String.Empty, Project.Singleton.Scenes));
            }
        }

        /// <summary>
        /// Represents an <see cref="ActorType"/> that want to be deleted from the project.
        /// </summary>
        public class ActorTypeForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return actorType; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Actor Type - {0}", actorType.Name); }
            }

            /// <summary>
            /// The underlying actor type.
            /// </summary>
            private ActorType actorType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActorTypeForDeletion"/> class.
            /// </summary>
            /// <param name="actorType">The actor type to delete.</param>
            public ActorTypeForDeletion(ActorType actorType)
            {
                this.actorType = actorType;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Not supported.
            /// </remarks>
            protected override void DeleteClear()
            {
                Debug.Assert(true, "Actor Type do not support clear deletion type");
                DeleteRemove();
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                actorType.Parent.Children.Remove(actorType);
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // actor type can be used at actors
                    ConsistentDeletionHelper.DeleteFromActors(actorType, items);
                    // actor type can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(Scripting.VariableType.ActorType, actorType, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Actor Types", String.Empty, Project.Singleton.ActorTypes));
            }
        }

        /// <summary>
        /// Represents a <see cref="NamedVariable"/> that want to be deleted from the project.
        /// </summary>
        public class ScriptNamedVariableForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return namedVariable; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Variable - {0}", namedVariable.Name); }
            }

            /// <summary>
            /// The underlying named variable.
            /// </summary>
            private NamedVariable namedVariable;

            /// <summary>
            /// Initializes a new instance of the <see cref="ScriptNamedVariableForDeletion"/> class.
            /// </summary>
            /// <param name="namedVariable">The named variable to delete.</param>
            public ScriptNamedVariableForDeletion(NamedVariable namedVariable)
            {
                this.namedVariable = namedVariable;
            }

            /// <inheritdoc />
            protected override void DeleteClear()
            {
                namedVariable.Value.SetValue(null);
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                namedVariable.ScriptingComponent.Variables.Remove(namedVariable);
            }

            /// <inheritdoc />
            public override void ItemsToDelete(HashSet<ItemForDeletion> items)
            {
                if (Type == DeletionType.Remove)
                {
                    // named variable can be used at the scripting
                    ConsistentDeletionHelper.DeleteFromScripting(namedVariable, items);
                }
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(new ItemOnPath("Variables", String.Empty, namedVariable.ScriptingComponent));
                GetPathFromScripting(namedVariable.ScriptingComponent, path);
            }
        }

        /// <summary>
        /// Represents a <see cref="VariableNodeSocket"/> that want to be deleted from the project.
        /// </summary>
        public class ScriptVariableNodeSocketForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return variableNodeSocket; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Variable - {0}", variableNodeSocket.Name); }
            }

            /// <summary>
            /// The underlying variable node socket.
            /// </summary>
            private VariableNodeSocket variableNodeSocket;

            /// <summary>
            /// Initializes a new instance of the <see cref="ScriptVariableNodeSocketForDeletion"/> class.
            /// </summary>
            /// <param name="variableNodeSocket">The variable node socket to delete.</param>
            public ScriptVariableNodeSocketForDeletion(VariableNodeSocket variableNodeSocket)
            {
                this.variableNodeSocket = variableNodeSocket;
            }

            /// <inheritdoc />
            protected override void DeleteClear()
            {
                variableNodeSocket.Value.SetValue(null);
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                variableNodeSocket.Value.SetValue(null);
                if (variableNodeSocket.Node.State != null) variableNodeSocket.Node.Remove();
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(CreateItemOnPath(variableNodeSocket.Node));
                path.Add(CreateItemOnPath(variableNodeSocket.Node.State));
                path.Add(CreateItemOnPath(variableNodeSocket.Node.State.StateMachine));
                GetPathFromScripting(variableNodeSocket.Node.State.StateMachine.ScriptingComponent, path);
            }
        }

        /// <summary>
        /// Represents a <see cref="Variable"/> that want to be deleted from the project.
        /// </summary>
        public class ScriptVariableForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return variable; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Variable of value {0}", variable.Value.ToString()); }
            }

            /// <summary>
            /// The underlying variable.
            /// </summary>
            private Variable variable;

            /// <summary>
            /// Initializes a new instance of the <see cref="ScriptVariableForDeletion"/> class.
            /// </summary>
            /// <param name="variable">The variable to delete.</param>
            public ScriptVariableForDeletion(Variable variable)
            {
                this.variable = variable;
            }

            /// <inheritdoc />
            protected override void DeleteClear()
            {
                variable.Value.SetValue(null);
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                variable.Remove();
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(CreateItemOnPath(variable.State));
                path.Add(CreateItemOnPath(variable.State.StateMachine));
                GetPathFromScripting(variable.State.StateMachine.ScriptingComponent, path);
            }
        }

        /// <summary>
        /// Represents a <see cref="Variable"/> of the <see cref="NamedVariable"/> that want to be deleted from the project.
        /// </summary>
        public class ScriptVariableOfNamedVariableForDeletion : ItemForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return variable; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Variable of named variable {0}", variable.NamedVariable.Name); }
            }

            /// <summary>
            /// The underlying variable.
            /// </summary>
            private Variable variable;

            /// <summary>
            /// Initializes a new instance of the <see cref="ScriptVariableOfNamedVariableForDeletion"/> class.
            /// </summary>
            /// <param name="variable">The variable to delete.</param>
            public ScriptVariableOfNamedVariableForDeletion(Variable variable)
            {
                this.variable = variable;
                Type = DeletionType.Remove;
            }

            /// <inheritdoc />
            protected override void DeleteClear()
            {
                variable.NamedVariable = null;
            }

            /// <inheritdoc />
            protected override void DeleteRemove()
            {
                variable.Remove();
            }

            /// <inheritdoc />
            public override void GetPath(List<ItemOnPath> path)
            {
                path.Add(CreateItemOnPath(variable.State));
                path.Add(CreateItemOnPath(variable.State.StateMachine));
                GetPathFromScripting(variable.State.StateMachine.ScriptingComponent, path);
            }
        }

        /// <summary>
        /// Represents a frame(s) (<see cref="Texture"/>) of the <see cref="Animation"/> that want to be deleted from the project.
        /// </summary>
        private class AnimationFrameForDeletion : AnimationForDeletion
        {
            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Animation - {0}", animation.Name); }
            }

            /// <summary>
            /// The underlying texture (frame(s) of the animation).
            /// </summary>
            private Texture texture;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnimationFrameForDeletion"/> class.
            /// </summary>
            /// <param name="animation">The animation.</param>
            /// <param name="texture">The texture that is used as frame(s) in the animation.</param>
            public AnimationFrameForDeletion(Animation animation, Texture texture)
                : base(animation)
            {
                this.texture = texture;
            }

            /// <inheritdoc />
            protected override void DeleteClear()
            {
                for (int i = 0; i < animation.Frames.Count; ++i)
                {
                    if (animation.Frames[i] == texture)
                    {
                        animation.Frames.RemoveAt(i);
                        --i;
                    }
                }

                animation.InvokeDrawableAssetChanged();
            }
        }

        /// <summary>
        /// Represents a <see cref="DrawableAsset"/> (graphics) of the <see cref="Actor"/> that want to be deleted from the project.
        /// </summary>
        private class ActorDrawableAssetForDeletion : ActorForDeletion
        {
            /// <inheritdoc />
            public override string Name
            {
                get { return "Graphics"; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ActorDrawableAssetForDeletion"/> class.
            /// </summary>
            /// <param name="actor">The actor where to delete drawable asset.</param>
            public ActorDrawableAssetForDeletion(Actor actor)
                : base(actor)
            {
            }

            /// <inheritdoc />
            protected override void DeleteClear()
            {
                actor.DrawableAsset = null;
            }
        }

        /// <summary>
        /// Represents a <see cref="ActorType"/> of the <see cref="Actor"/> that want to be deleted from the project.
        /// </summary>
        private class ActorActorTypeForDeletion : ActorForDeletion
        {
            /// <inheritdoc />
            public override object Tag
            {
                get { return actor; }
            }

            /// <inheritdoc />
            public override string Name
            {
                get { return String.Format("Actor Type - {0}", actor.Type.Name); }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ActorActorTypeForDeletion"/> class.
            /// </summary>
            /// <param name="actor">The actor where to delete actor type.</param>
            public ActorActorTypeForDeletion(Actor actor)
                : base(actor)
            {
            }

            protected override void DeleteClear()
            {
                actor.Type = Project.Singleton.ActorTypes.Root;
            }
        }

        /// <summary>
        /// Deletes the specified value of the specified variable type from all scriptings.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="value">The value.</param>
        /// <returns>Items to delete.</returns>
        public static HashSet<ItemForDeletion> DeleteFromScripting(VariableType variableType, object value)
        {
            HashSet<ItemForDeletion> items = new HashSet<ItemForDeletion>();
            DeleteFromScripting(variableType, value, items);
            return items;
        }

        /// <summary>
        /// Deletes the specified value of the specified variable type from all scriptings.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="value">The value.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(VariableType variableType, object value, HashSet<ItemForDeletion> items)
        {
            foreach (Scene scene in Project.Singleton.Scenes)
            {
                DeleteFromScripting(variableType, value, scene, items);
                DeleteFromScripting(variableType, value, scene.GlobalScript, items);
            }

            foreach (Actor prototype in Project.Singleton.Prototypes)
            {
                DeleteFromScripting(variableType, value, prototype, items);
            }
        }

        /// <summary>
        /// Deletes the specified value of the specified variable type from actors scriptings from the specified scene.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="value">The value.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(VariableType variableType, object value, Scene scene, HashSet<ItemForDeletion> items)
        {
            foreach (Actor actor in scene.AllActors())
            {
                DeleteFromScripting(variableType, value, actor, items);
            }
        }

        /// <summary>
        /// Deletes the specified value of the specified variable type from the specified actor and its children scriptings.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="value">The value.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(VariableType variableType, object value, Actor actor, HashSet<ItemForDeletion> items)
        {
            DeleteFromScripting(variableType, value, actor.Scripting, items);

            foreach (Actor child in actor.Children)
            {
                DeleteFromScripting(variableType, value, child, items);
            }
        }

        /// <summary>
        /// Deletes the specified value of the specified variable type from the specified scripting.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <param name="value">The value.</param>
        /// <param name="scripting">The scripting.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(VariableType variableType, object value, ScriptingComponent scripting, HashSet<ItemForDeletion> items)
        {
            // all variables
            foreach (NamedVariable variable in scripting.Variables)
            {
                // used in variable
                if (variable.VariableType == variableType && variable.Value.GetValue() == value)
                {
                    items.Add(new ScriptNamedVariableForDeletion(variable));
                }
            }

            // all state machines
            foreach (StateMachine stateMachine in scripting.StateMachines)
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

                                    // used in variable node socket
                                    if (variableNodeSocket.VariableType == variableType && variableNodeSocket.Value.GetValue() == value)
                                    {
                                        items.Add(new ScriptVariableNodeSocketForDeletion(variableNodeSocket));
                                    }
                                }
                            }
                        }
                        // all script variables
                        else if (baseNode is Variable)
                        {
                            Variable variable = baseNode as Variable;

                            // used in script variable
                            if (variable.NamedVariable == null && variable.VariableType == variableType && variable.Value.GetValue() == value)
                            {
                                items.Add(new ScriptVariableForDeletion(variable));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the specified texture from all animations of the project.
        /// </summary>
        /// <param name="texture">The texture to delete.</param>
        /// <returns>Items to delete.</returns>
        public static HashSet<ItemForDeletion> DeleteFromAnimations(Texture texture)
        {
            HashSet<ItemForDeletion> items = new HashSet<ItemForDeletion>();
            DeleteFromAnimations(texture, items);
            return items;
        }

        /// <summary>
        /// Deletes the specified texture from all animations of the project.
        /// </summary>
        /// <param name="texture">The texture to delete.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromAnimations(Texture texture, HashSet<ItemForDeletion> items)
        {
            foreach (Animation animation in Project.Singleton.Animations)
            {
                foreach (Texture frame in animation.Frames)
                {
                    if (frame == texture)
                    {
                        items.Add(new AnimationFrameForDeletion(animation, texture));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the specified drawable asset from all actors of the project.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to delete.</param>
        /// <returns>Items to delete.</returns>
        public static HashSet<ItemForDeletion> DeleteFromActors(DrawableAsset drawableAsset)
        {
            HashSet<ItemForDeletion> items = new HashSet<ItemForDeletion>();
            DeleteFromActors(drawableAsset, items);
            return items;
        }

        /// <summary>
        /// Deletes the specified drawable asset from all actors of the project.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to delete.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromActors(DrawableAsset drawableAsset, HashSet<ItemForDeletion> items)
        {
            foreach (Scene scene in Project.Singleton.Scenes)
            {
                DeleteFromActors(drawableAsset, scene, items);
            }

            foreach (Actor prototype in Project.Singleton.Prototypes)
            {
                DeleteFromActors(drawableAsset, prototype, items);
            }
        }

        /// <summary>
        /// Deletes the specified drawable asset from all actors of the specified scene.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to delete.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromActors(DrawableAsset drawableAsset, Scene scene, HashSet<ItemForDeletion> items)
        {
            foreach (Actor actor in scene.AllActors())
            {
                DeleteFromActors(drawableAsset, actor, items);
            }
        }

        /// <summary>
        /// Deletes the specified drawable asset from the specified actor and its children.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset to delete.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromActors(DrawableAsset drawableAsset, Actor actor, HashSet<ItemForDeletion> items)
        {
            if (actor.DrawableAsset == drawableAsset)
            {
                items.Add(new ActorDrawableAssetForDeletion(actor) { Type = DeletionType.Remove });
            }

            foreach (Actor child in actor.Children)
            {
                DeleteFromActors(drawableAsset, child, items);
            }
        }

        /// <summary>
        /// Deletes the specified named variable from all scriptings.
        /// </summary>
        /// <param name="namedVariable">The named variable to delete.</param>
        /// <returns>Items to delete.</returns>
        public static HashSet<ItemForDeletion> DeleteFromScripting(NamedVariable namedVariable)
        {
            HashSet<ItemForDeletion> items = new HashSet<ItemForDeletion>();
            DeleteFromScripting(namedVariable, items);
            return items;
        }

        /// <summary>
        /// Deletes the specified named variable from all scriptings.
        /// </summary>
        /// <param name="namedVariable">The named variable to delete.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(NamedVariable namedVariable, HashSet<ItemForDeletion> items)
        {
            foreach (Scene scene in Project.Singleton.Scenes)
            {
                DeleteFromScripting(namedVariable, scene, items);
                DeleteFromScripting(namedVariable, scene.GlobalScript, items);
            }

            foreach (Actor prototype in Project.Singleton.Prototypes)
            {
                DeleteFromScripting(namedVariable, prototype, items);
            }
        }

        /// <summary>
        /// Deletes the specified named variable from all actors scriptings of the specified scene.
        /// </summary>
        /// <param name="namedVariable">The named variable to delete.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(NamedVariable namedVariable, Scene scene, HashSet<ItemForDeletion> items)
        {
            foreach (Actor actor in scene.AllActors())
            {
                DeleteFromScripting(namedVariable, actor, items);
            }
        }

        /// <summary>
        /// Deletes the specified named variable from the specified actor and its children scriptings.
        /// </summary>
        /// <param name="namedVariable">The named variable to delete.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(NamedVariable namedVariable, Actor actor, HashSet<ItemForDeletion> items)
        {
            DeleteFromScripting(namedVariable, actor.Scripting, items);

            foreach (Actor child in actor.Children)
            {
                DeleteFromScripting(namedVariable, child, items);
            }
        }

        /// <summary>
        /// Deletes the specified named variable from the specified scripting.
        /// </summary>
        /// <param name="namedVariable">The named variable to delete.</param>
        /// <param name="scripting">The scripting.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromScripting(NamedVariable namedVariable, ScriptingComponent scripting, HashSet<ItemForDeletion> items)
        {
            // all state machines
            foreach (StateMachine stateMachine in scripting.StateMachines)
            {
                // all states
                foreach (State state in stateMachine.States)
                {
                    foreach (BaseNode baseNode in state.Nodes)
                    {
                        Variable variable = baseNode as Variable;
                        if (variable != null && variable.NamedVariable == namedVariable)
                        {
                            items.Add(new ScriptVariableOfNamedVariableForDeletion(variable));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the specified actor type from all actors.
        /// </summary>
        /// <param name="actorType">The actor type to delete.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromActors(ActorType actorType, HashSet<ItemForDeletion> items)
        {
            foreach (Scene scene in Project.Singleton.Scenes)
            {
                DeleteFromActors(actorType, scene, items);
            }

            foreach (Actor prototype in Project.Singleton.Prototypes)
            {
                DeleteFromActors(actorType, prototype, items);
            }
        }

        /// <summary>
        /// Deletes the specified actor type from all actors of the specified scene.
        /// </summary>
        /// <param name="actorType">The actor type to delete.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromActors(ActorType actorType, Scene scene, HashSet<ItemForDeletion> items)
        {
            foreach (Actor actor in scene.AllActors())
            {
                DeleteFromActors(actorType, actor, items);
            }
        }

        /// <summary>
        /// Deletes the specified actor type from the specified actor and its children.
        /// </summary>
        /// <param name="actorType">The actor type to delete.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="items">Stores items to delete.</param>
        public static void DeleteFromActors(ActorType actorType, Actor actor, HashSet<ItemForDeletion> items)
        {
            if (actor.Type == actorType)
            {
                items.Add(new ActorActorTypeForDeletion(actor));
            }

            foreach (Actor child in actor.Children)
            {
                DeleteFromActors(actorType, child, items);
            }
        }
    }
}
