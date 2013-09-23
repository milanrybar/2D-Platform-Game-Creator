/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scripting;
using System.ComponentModel;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.Assets;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    /// <summary>
    /// Actor that can be used in the game.
    /// </summary>
    [Serializable]
    class Actor : GameObject, ISerializable
    {
        /// <summary>
        /// Gets or sets the layer where the actor is stored.
        /// </summary>
        /// <remarks>
        /// When the <see cref="Layer"/> is <c>null</c> then this actor is child of the <see cref="Parent"/> actor.
        /// When the <see cref="Layer"/> is <c>null</c> and the <see cref="Parent"/> is <c>null</c> then this actor is prototype or is stored nowhere.
        /// </remarks>
        public Layer Layer { get; set; }

        /// <summary>
        /// Gets or sets the position of the actor at the scene.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // returns internal position
                if (Parent == null) return _position;
                // actor has a parent => internal position is relative to the parent
                else return Vector2.Transform(_position + Parent.Position, Parent.Transform);
            }
            set
            {
                // set to internal position
                if (Parent == null) _position = value;
                // actor has parent => set position as relative to the parent
                else
                {
                    Vector2 absolutePosition = Parent.Position;
                    float absoluteAngle = Parent.Angle;
                    Vector2 absoluteScaleFactor = Parent.ScaleFactor;

                    Vector2 newPosition = Vector2.Transform(value,
                    Matrix.CreateTranslation(-absolutePosition.X, -absolutePosition.Y, 0f) *
                    Matrix.CreateRotationZ(-absoluteAngle) *
                    Matrix.CreateScale(1f / absoluteScaleFactor.X, 1f / absoluteScaleFactor.Y, 0f) *
                    Matrix.CreateTranslation(absolutePosition.X, absolutePosition.Y, 0f));

                    _position = newPosition - absolutePosition;
                }

                InvokeAppearanceChanged();
            }
        }
        private Vector2 _position;

        /// <summary>
        /// Gets or sets the rotation angle in radians of the actor.
        /// </summary>
        public float Angle
        {
            get
            {
                // returns internal angle
                if (Parent == null) return _angle;
                // actor has a parent => internal angle is relative to the parent
                else return _angle + Parent.Angle;
            }
            set
            {
                Debug.Assert(!float.IsNaN(value) && !float.IsInfinity(value), "Invalid value.");

                // set to internal angle
                if (Parent == null) _angle = value;
                // actor has parent => set angle as relative to the parent
                else _angle = value - Parent.Angle;

                InvokeAppearanceChanged();
            }
        }
        private float _angle = 0f;

        /// <summary>
        /// Gets or sets the scale factor of the actor.
        /// </summary>
        public Vector2 ScaleFactor
        {
            get
            {
                // returns internal scale factor
                if (Parent == null) return _scale;
                // actor has a parent => internal scale factor is relative to the parent
                else return _scale * Parent.ScaleFactor;
            }
            set
            {
                Debug.Assert(!float.IsNaN(value.X) && !float.IsInfinity(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.Y), "Invalid value.");

                // set to internal scale factor
                if (Parent == null) _scale = value;
                // actor has parent => set scale factor as relative to the parent
                else _scale = value / Parent.ScaleFactor;

                InvokeAppearanceChanged();
            }
        }
        private Vector2 _scale = Vector2.One;

        /// <summary>
        /// Gets the transform matrix of the actor.
        /// </summary>
        public Matrix Transform
        {
            get
            {
                Vector2 position = Position;
                float angle = Angle;
                Vector2 scaleFactor = ScaleFactor;

                return Matrix.CreateTranslation(-position.X, -position.Y, 0f) *
                    Matrix.CreateScale(scaleFactor.X, scaleFactor.Y, 0f) *
                    Matrix.CreateRotationZ(angle) *
                    Matrix.CreateTranslation(position.X, position.Y, 0f);
            }
        }

        /// <summary>
        /// Gets or sets the type of the actor.
        /// </summary>
        public ActorType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (ActorTypeChanged != null) ActorTypeChanged(this, EventArgs.Empty);
            }
        }
        private ActorType _type;

        /// <summary>
        /// Gets or sets the drawable asset of the actor. Represents the graphics of the actor.
        /// </summary>
        public DrawableAsset DrawableAsset
        {
            get { return _drawableAsset; }
            set
            {
                if (_drawableAsset != value)
                {
                    DrawableAsset oldValue = _drawableAsset;

                    _drawableAsset = value;

                    if (DrawableAssetChanged != null) DrawableAssetChanged(this, new ValueChangedEventArgs<DrawableAsset>(oldValue));
                }
            }
        }
        private DrawableAsset _drawableAsset;

        /// <summary>
        /// Gets or sets a value indicating whether the drawable asset (graphics) of the actor is visible.
        /// </summary>
        public bool DrawableAssetVisible { get; set; }

        /// <summary>
        /// Gets the physics settings of the actor.
        /// </summary>
        public PhysicsComponent Physics
        {
            get { return _physics; }
        }
        private PhysicsComponent _physics;

        /// <summary>
        /// Gets the scripting of the actor.
        /// </summary>
        public ScriptingComponent Scripting
        {
            get { return _scripting; }
        }
        private ScriptingComponent _scripting;

        /// <summary>
        /// Gets the collision shapes of the actor.
        /// When the <see cref="DrawableAsset"/> is not <c>null</c> then returns collision shapes of the <see cref="DrawableAsset"/>; otherwise returns internal collision shapes. 
        /// </summary>
        public ObservableList<Shape> Shapes
        {
            get
            {
                if (_shapes != null && _shapes.Count != 0) return _shapes;
                else if (DrawableAsset != null) return DrawableAsset.Shapes;
                else return _shapes;
            }
        }
        private ObservableList<Shape> _shapes;

        /// <summary>
        /// Gets the parent actor of the actor, if any.
        /// </summary>
        public Actor Parent
        {
            get { return _parent; }
        }
        private Actor _parent;

        /// <summary>
        /// Gets the children of the actor.
        /// </summary>
        public ChildrenList Children
        {
            get { return _children; }
        }
        private ChildrenList _children;

        /// <summary>
        /// Occurs when some important value for appearance of the actor changes.
        /// Occurs when this or any child position, rotation angle or scale factor changes.
        /// </summary>
        public event EventHandler AppearanceChanged;

        /// <summary>
        /// Occurs when the <see cref="DrawableAsset"/> property value changes.
        /// </summary>
        public event ValueChangedHandler<DrawableAsset> DrawableAssetChanged;

        /// <summary>
        /// Occurs when the <see cref="Type"/> property value changes.
        /// </summary>
        public event EventHandler ActorTypeChanged;

        /// <summary>
        /// Occurs when the <see cref="Parent"/> property value changes.
        /// </summary>
        public event ValueChangedHandler<Actor> ParentChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class.
        /// </summary>
        /// <param name="drawableAsset">The drawable asset for the actor.</param>
        /// <param name="position">The position of the actor.</param>
        public Actor(DrawableAsset drawableAsset, Vector2 position)
        {
            _name = String.Format("Actor {0}", Id);
            _drawableAsset = drawableAsset;
            _position = position;
            _physics = new PhysicsComponent();
            _scripting = new ScriptingComponent(this);
            _type = Project.Singleton.ActorTypes.Root;
            _children = new ChildrenList(this);
            DrawableAssetVisible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class from the specified <see cref="Actor"/> instance.
        /// </summary>
        /// <param name="actor">The actor.</param>
        private Actor(Actor actor)
        {
            _drawableAsset = actor.DrawableAsset;
            _position = actor.Position;
            _name = actor.Name;
            _angle = actor.Angle;
            _scale = actor.ScaleFactor;
            _physics = actor.Physics.Clone();
            _scripting = actor.Scripting.Clone(this);
            _type = actor.Type;
            _children = new ChildrenList(this);
            DrawableAssetVisible = actor.DrawableAssetVisible;
        }

        /// <inheritdoc />
        protected Actor(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            Layer = (Layer)info.GetValue("Layer", typeof(Layer));
            _parent = (Actor)info.GetValue("Parent", typeof(Actor));
            _children = (ChildrenList)info.GetValue("Children", typeof(ChildrenList));
            _position = (Vector2)info.GetValue("Position", typeof(Vector2));
            _angle = info.GetSingle("Angle");
            _scale = (Vector2)info.GetValue("ScaleFactor", typeof(Vector2));
            _type = (ActorType)info.GetValue("ActorType", typeof(ActorType));
            _drawableAsset = (DrawableAsset)info.GetValue("DrawableAsset", typeof(DrawableAsset));
            _physics = (PhysicsComponent)info.GetValue("Physics", typeof(PhysicsComponent));
            _scripting = (ScriptingComponent)info.GetValue("Scripting", typeof(ScriptingComponent));
            _shapes = (ObservableList<Shape>)info.GetValue("Shapes", typeof(ObservableList<Shape>));
            DrawableAssetVisible = info.GetBoolean("DrawableAssetVisible");
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Layer", Layer);
            info.AddValue("Parent", Parent);
            info.AddValue("Children", Children);
            info.AddValue("Position", _position);
            info.AddValue("Angle", _angle);
            info.AddValue("ScaleFactor", _scale);
            info.AddValue("ActorType", Type);
            info.AddValue("DrawableAsset", DrawableAsset);
            info.AddValue("Physics", Physics);
            info.AddValue("Scripting", Scripting);
            info.AddValue("Shapes", _shapes);
            info.AddValue("DrawableAssetVisible", DrawableAssetVisible);
        }

        /// <summary>
        /// Clones this actor. Cloned actor has new unique id.
        /// If <paramref name="addToContainer"/> is <c>true</c> cloned actor is added to the layer of this actor.
        /// </summary>
        /// <param name="addToContainer">If set to <c>true</c> cloned object is added to the layer of this actor.</param>
        /// <returns>Cloned actor.</returns>
        public override GameObject Clone(bool addToContainer = false)
        {
            return Clone(Layer, addToContainer);
        }

        /// <summary>
        /// Clones this actor. Cloned actor has new unique id.
        /// If <paramref name="addToContainer"/> is <c>true</c> cloned actor is added to the specified layer.
        /// </summary>
        /// <param name="layer">The layer where to add the cloned actor, if <paramref name="addToContainer"/> is <c>true</c>.</param>
        /// <param name="addToContainer">If set to <c>true</c> cloned object is added to the specified layer.</param>
        /// <returns>Cloned actor.</returns>
        public GameObject Clone(Layer layer, bool addToContainer = false)
        {
            Actor clonedActor = new Actor(this);

            foreach (Actor child in Children)
            {
                clonedActor.Children.Add((Actor)child.Clone());
            }

            if (addToContainer && layer != null)
            {
                layer.Add(clonedActor);
            }

            return clonedActor;
        }

        /// <summary>
        /// Creates the <see cref="ActorView"/> representing this actor for using at the scene.
        /// </summary>
        /// <returns>Created <see cref="ActorView"/> of this actor.</returns>
        public override PlatformGameCreator.Editor.Scenes.SceneNode CreateSceneView()
        {
            return new ActorView(this);
        }

        /// <summary>
        /// Removes this actor from the container where is stored (layer, parent children or prototypes list).
        /// </summary>
        public override void Remove()
        {
            // actor is child
            if (Parent != null)
            {
                Parent.Children.Remove(this);
                _parent = null;
            }
            // actor is at the layer
            else if (Layer != null)
            {
                Layer.Remove(this);
                Layer = null;
            }
            // actor is not used
            else
            {
                // actor is prototype
                if (Project.Singleton.Prototypes.Contains(this))
                {
                    Project.Singleton.Prototypes.Remove(this);
                }
            }
        }

        /// <summary>
        /// Gets the layer where is the actor used.
        /// </summary>
        /// <returns>Layer where is the actor used; otherwise <c>null</c>.</returns>
        public Layer GetLayer()
        {
            Actor currentActor = this;
            while (currentActor.Parent != null)
            {
                currentActor = currentActor.Parent;
            }

            return currentActor.Layer;
        }

        /// <summary>
        /// Gets the scene where is the actor used.
        /// </summary>
        /// <returns>Scene where is the actor used; otherwise <c>null</c>.</returns>
        public Scene GetScene()
        {
            Layer layer = GetLayer();

            if (layer != null) return layer.Scene;
            else return null;
        }

        /// <inheritdoc />
        public override Command CreateDeleteCommand()
        {
            if (Parent != null)
            {
                return new DeleteGameObjectCommand<Actor>(this, Parent.Children, Index);
            }
            else if (Layer != null)
            {
                return new DeleteGameObjectCommand<Actor>(this, Layer, Index);
            }

            return null;
        }

        /// <inheritdoc />
        public override Command CreateAddCommand()
        {
            if (Parent != null)
            {
                return new AddGameObjectCommand<Actor>(this, Parent.Children, Index);
            }
            else if (Layer != null)
            {
                return new AddGameObjectCommand<Actor>(this, Layer, Index);
            }

            return null;
        }

        /// <summary>
        /// Invokes the <see cref="AppearanceChanged"/> event of this actor and calls the same method on children.
        /// </summary>
        /// <param name="startInvoking">The actor that starts invoking.</param>
        private void InvokeAppearanceChanged(Actor startInvoking = null)
        {
            if (startInvoking == null) startInvoking = this;
            if (AppearanceChanged != null) AppearanceChanged(startInvoking, EventArgs.Empty);

            foreach (Actor child in Children)
            {
                child.InvokeAppearanceChanged(startInvoking);
            }
        }

        /// <summary>
        /// Invokes the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="oldParent">The old value of <see cref="Parent"/> property.</param>
        private void InvokeParentChanged(Actor oldParent)
        {
            if (ParentChanged != null) ParentChanged(this, new ValueChangedEventArgs<Actor>(oldParent));
        }

        /// <summary>
        /// Finds the actor by the specified id in the specified collection of actors.
        /// </summary>
        /// <param name="id">The unique id of the actor to find.</param>
        /// <param name="collectionOfActors">The collection of actors.</param>
        /// <param name="recursive">If set to <c>true</c> also children of actors are searched for the actor.</param>
        /// <returns>Actor if found; otherwise <c>null</c>.</returns>
        public static Actor FindById(int id, IEnumerable<Actor> collectionOfActors, bool recursive = true)
        {
            Actor result = null;

            foreach (Actor actor in collectionOfActors)
            {
                if (actor.Id == id) return actor;

                if (recursive)
                {
                    result = actor.Children.FindById(id);
                    if (result != null) return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified collection contains the specified actor.
        /// </summary>
        /// <param name="actor">The actor to find in the specified collection.</param>
        /// <param name="collectionOfActors">The collection of actors.</param>
        /// <param name="recursive">If set to <c>true</c> also children of actors are searched for the actor.</param>
        /// <returns><c>true</c> if the specified collection contains the specified actor; otherwise <c>false</c>.</returns>
        public static bool Contains(Actor actor, IEnumerable<Actor> collectionOfActors, bool recursive = true)
        {
            return FindById(actor.Id, collectionOfActors, recursive) != null;
        }

        /// <summary>
        /// List for storing an actor children.
        /// </summary>
        [Serializable]
        public class ChildrenList : ObservableIndexedList<Actor>
        {
            /// <summary>
            /// Gets the actor that represents the parent for the children.
            /// </summary>
            public Actor Actor
            {
                get { return _actor; }
            }
            private Actor _actor;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChildrenList"/> class.
            /// </summary>
            /// <param name="actor">The actor that represents the parent for the children.</param>
            public ChildrenList(Actor actor)
            {
                _actor = actor;
            }

            /// <inheritdoc />
            protected ChildrenList(SerializationInfo info, StreamingContext ctxt)
                : base(info, ctxt)
            {
                _actor = (Actor)info.GetValue("Actor", typeof(Actor));
            }

            /// <inheritdoc />
            /// <remarks>
            /// Makes the relative position, rotation angle and scale factor to the parent of the added actor.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Actor cannot be null.</exception>
            public override void Add(Actor item)
            {
                if (item == null) throw new ArgumentNullException("Actor cannot be null.");
                Actor oldItemParent = item.Parent;

                // set parent
                item._parent = Actor;
                // make relative
                MakeRelative(item);

                base.Add(item);

                SetCorrectPhysicsType(item);
                item.InvokeParentChanged(oldItemParent);
            }

            /// <inheritdoc />
            /// <remarks>
            /// Makes the relative position, rotation angle and scale factor to the parent of the inserted actor.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Actor cannot be null.</exception>
            public override void Insert(int index, Actor item)
            {
                if (item == null) throw new ArgumentNullException("Actor cannot be null.");
                Actor oldItemParent = item.Parent;

                // set parent
                item._parent = Actor;
                // make relative
                MakeRelative(item);

                base.Insert(index, item);

                SetCorrectPhysicsType(item);
                item.InvokeParentChanged(oldItemParent);
            }

            /// <inheritdoc />
            /// <remarks>
            /// Makes the absolute position, rotation angle and scale factor from the parent of the removed actor.
            /// </remarks>
            public override void RemoveAt(int index)
            {
                Debug.Assert(this[index].Parent == Actor, "Actor does not belong to this collection.");

                Actor item = this[index];

                // make absolute
                MakeAbsolute(item);
                // unset parent
                item._parent = null;

                base.RemoveAt(index);

                item.InvokeParentChanged(Actor);
            }

            /// <summary>
            /// Applies the restriction of the physics type to the specified child.
            /// </summary>
            /// <param name="item">The actor to set correct physics type.</param>
            private void SetCorrectPhysicsType(Actor item)
            {
                if (item.Physics.Type != PhysicsComponent.BodyPhysicsType.None && item.Physics.Type != Actor.Physics.Type)
                {
                    item.Physics.Type = Actor.Physics.Type;
                }
            }

            /// <summary>
            /// Makes the relative position, angle and scale to the parent.
            /// </summary>
            /// <param name="actor">The actor.</param>
            private void MakeRelative(Actor actor)
            {
                actor.Position = actor._position;
                actor.Angle = actor._angle;
                actor.ScaleFactor = actor._scale;
            }

            /// <summary>
            /// Makes the absolute position, angle and scale from the parent.
            /// </summary>
            /// <param name="actor">The actor.</param>
            private void MakeAbsolute(Actor actor)
            {
                actor._position = actor.Position;
                actor._angle = actor.Angle;
                actor._scale = actor.ScaleFactor;
            }

            /// <inheritdoc />
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);

                info.AddValue("Actor", Actor);
            }

            /// <summary>
            /// Finds the actor by the specified id.
            /// </summary>
            /// <param name="id">The unique id of the actor to find.</param>
            /// <param name="recursive">If set to <c>true</c> also children of actors are searched for the actor.</param>
            /// <returns>Actor if found; otherwise <c>null</c>.</returns>
            public Actor FindById(int id, bool recursive = true)
            {
                Actor result = null;

                foreach (Actor child in this)
                {
                    if (child.Id == id)
                    {
                        return child;
                    }

                    if (recursive)
                    {
                        result = child.Children.FindById(id);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }

                return null;
            }
        }
    }
}
