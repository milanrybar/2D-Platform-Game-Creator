/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scripting;
using PlatformGameCreator.GameEngine.Scripting.Actions;
using PlatformGameCreator.GameEngine.Scripting.Events;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PlatformGameCreator.GameEngine.Scenes
{
    /// <summary>
    /// Update delegate with the same header as Update method in all classes.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last call to Update.</param>
    public delegate void UpdateHandler(GameTime gameTime);

    /// <summary>
    /// Represents the actor that is used at the scene.
    /// It has the same ability and behaviour as in the editor.
    /// </summary>
    public abstract class Actor : SceneNode
    {
        /// <summary>
        /// Gets or sets the id of the actor which is the same id as in the editor.
        /// </summary>
        public int ActorId { get; set; }

        /// <summary>
        /// Gets or sets the type of the actor.
        /// </summary>
        public uint ActorType { get; set; }

        /// <summary>
        /// Gets the body of the actor in the physics simulation.
        /// </summary>
        public Body Body
        {
            get { return _body; }
        }
        /// <summary>
        /// The body of the actor in the physics simulation.
        /// </summary>
        protected Body _body;

        /// <summary>
        /// Gets or sets the position in simulations units of the actor.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // actor has a parent => internal position is relative to the parent
                if (Parent != null)
                {
                    Vector2 absolutePosition = Parent.Position;
                    float absoluteAngle = Parent.Angle;

                    return Vector2.Transform(_position + Parent.Position,
                        Matrix.CreateTranslation(-absolutePosition.X, -absolutePosition.Y, 0f) *
                        Matrix.CreateRotationZ(absoluteAngle) *
                        Matrix.CreateTranslation(absolutePosition.X, absolutePosition.Y, 0f));
                }
                // actor is at the parallax layer => position depends on the camera
                else if (IsParallax)
                {
                    return _position + Screen.Camera.Position * ParallaxCoefficient;
                }
                // returns internal position
                else
                {
                    return _position;
                }
            }
            set
            {
                // actor is in physics simulaton and has no parent => set position to body
                if (Body != null && Parent == null)
                {
                    Body.Position = value;
                    _position = value;

                    // manipulating a body's transform may cause non-physical behavior and 
                    // we must take care of current collisions because physics engine reset the body contacts
                    ClearCollisions();
                }
                // actor has parent => set position as relative to the parent
                else if (Parent != null)
                {
                    Vector2 absolutePosition = Parent.Position;
                    float absoluteAngle = Parent.Angle;

                    Vector2 newPosition = Vector2.Transform(value,
                    Matrix.CreateTranslation(-absolutePosition.X, -absolutePosition.Y, 0f) *
                    Matrix.CreateRotationZ(-absoluteAngle) *
                    Matrix.CreateTranslation(absolutePosition.X, absolutePosition.Y, 0f));

                    _position = newPosition - absolutePosition;
                }
                // actor is at the parallax layer => position depends on the camera
                else if (IsParallax)
                {
                    _position = value - Screen.Camera.Position * ParallaxCoefficient;
                }
                // set to internal position
                else
                {
                    _position = value;
                }
            }
        }
        /// <summary>
        /// Internal position of the actor. Value can have different meaning (depends on the actor settings).
        /// </summary>
        protected Vector2 _position;

        /// <summary>
        /// Gets or sets the rotation angle in radians of the actor.
        /// </summary>
        public float Angle
        {
            get
            {
                // actor has a parent => internal angle is relative to the parent
                if (Parent != null)
                {
                    return _angle + Parent.Angle;
                }
                // returns internal angle
                else
                {
                    return _angle;
                }
            }
            set
            {
                // actor is in physics simulaton and has no parent => set angle to body
                if (Body != null && Parent == null)
                {
                    Body.Rotation = value;
                    _angle = value;

                    // manipulating a body's transform may cause non-physical behavior and 
                    // we must take care of current collisions because physics engine reset the body contacts
                    ClearCollisions();
                }
                // actor has parent => set angle as relative to the parent
                else if (Parent != null)
                {
                    _angle = value - Parent.Angle;
                }
                // set to internal angle
                else
                {
                    _angle = value;
                }
            }
        }
        /// <summary>
        /// Internal rotation angle of the actor. Value can have different meaning (depends on the actor settings).
        /// </summary>
        protected float _angle;

        /// <summary>
        /// Gets the scale factor of the actor.
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
        }
        /// <summary>
        /// Internal  scale factor of the actor.
        /// </summary>
        protected Vector2 _scale;

        /// <summary>
        /// Indicates whether the actor is one-way platform. One-way platform has special physics behaviour.
        /// </summary>
        public bool OneWayPlatform;

        /// <summary>
        /// Indicates whether the actor is at a parallax layer. If true, position depends on the camera and <see cref="ParallaxCoefficient"/>.
        /// </summary>
        public bool IsParallax;

        /// <summary>
        /// Parallax coefficient is used when the actor is at a parallax layer.
        /// </summary>
        public Vector2 ParallaxCoefficient;

        /// <summary>
        /// Graphics effect for the actor is used when the actor is at a parallax layer.
        /// </summary>
        public GraphicsEffect GraphicsEffect = GraphicsEffect.None;

        /// <summary>
        /// Gets the children of the actor.
        /// </summary>
        public List<Actor> Children
        {
            get { return _children; }
        }
        private List<Actor> _children = new List<Actor>();

        /// <summary>
        /// Parent of the actor.
        /// </summary>
        public Actor Parent;

        /// <summary>
        /// Indicates whether the graphics of the actor is visible.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Occurs when the actor enters in the collision.
        /// </summary>
        public OnCollisionEventHandler OnCollision;

        /// <summary>
        /// Occurs when the actor exits the collision.
        /// </summary>
        public OnSeparationEventHandler OnSeparation;

        /// <summary>
        /// Gets the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> for drawing the actor.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return Screen.ScreenManager.SpriteBatch; }
        }

        /// <summary>
        /// Variables of the actor. Pair of (Name of the variable, Variable wrapper).
        /// </summary>
        protected Dictionary<string, VariableWrapper> variables = new Dictionary<string, VariableWrapper>();

        /// <summary>
        /// Events of the actor. Pair of (Name of the event, Event wrapper).
        /// </summary>
        protected Dictionary<string, EventWrapper> events = new Dictionary<string, EventWrapper>();

        /// <summary>
        /// State machines of the actor (active states of state machines).
        /// </summary>
        protected State[] stateMachines;

        /// <summary>
        /// Gets or sets the renderer of the actor.
        /// </summary>
        public ActorRenderer Renderer
        {
            get { return _renderer; }
            set
            {
                if (value.Actor != this) throw new ArgumentException("Actor Renderer does not belong to the current actor.");
                _renderer = value;
            }
        }
        private ActorRenderer _renderer;

        /// <summary>
        /// Collision with the actor. Pair of (object in collision with, number of collisions (contacts)).
        /// Body with more fixtures produces collision for every fixture separately.
        /// </summary>
        protected Dictionary<object, int> collisions = new Dictionary<object, int>();

        /// <summary>
        /// Stores new collisions and separations for the update cycle.
        /// </summary>
        private List<CollisionEventContainer> collisionEventsList = new List<CollisionEventContainer>();

        /// <summary>
        /// Gets the variable of the actor by the specified name and type.
        /// </summary>
        /// <typeparam name="T">Type of the variable to get.</typeparam>
        /// <param name="variableName">Name of the variable to get.</param>
        /// <returns>Variable if is found; otherwise <c>null</c></returns>
        public Variable<T> GetVariable<T>(string variableName)
        {
            VariableWrapper baseVariable;
            if (variables.TryGetValue(variableName, out baseVariable))
            {
                Variable<T> variable = baseVariable as Variable<T>;
                if (variable != null) return variable;
            }

            if (Parent != null) return Parent.GetVariable<T>(variableName);

            return null;
        }

        /// <summary>
        /// Gets the event of the actor by the specified name.
        /// </summary>
        /// <param name="eventName">Name of the event to get.</param>
        /// <returns>Event if is found; otherwise <c>null</c></returns>
        public EventWrapper GetEvent(string eventName)
        {
            EventWrapper eventWrapper;
            if (events.TryGetValue(eventName, out eventWrapper))
            {
                return eventWrapper;
            }

            return null;
        }

        /// <summary>
        /// Basic initialization of the actor. Override this method with actor-specific basic initialization code.
        /// </summary>
        /// <remarks>
        /// Sets all settings, creates variables and events and creates the body for the physics simulation. 
        /// Also calls <see cref="InitializeActor"/> on children.
        /// </remarks>
        public virtual void InitializeActor()
        {
            if (Body != null)
            {
                Body.OnCollision += OnBodyCollision;
                Body.OnSeparation += OnBodySeparation;

                Body.CollisionGroup = Parent == null ? (short)-ActorId : (Parent.Body != null && Parent.Body.FixtureList != null && Parent.Body.FixtureList.Count != 0 ? Parent.Body.FixtureList[0].CollisionGroup : (short)-ActorId);
            }

            // initialize children
            foreach (Actor child in Children)
            {
                child.Screen = Screen;
                child.Parent = this;

                child.InitializeActor();
            }
        }

        /// <summary>
        /// Components initialization of the actor. Override this method with actor-specific components initialization code.
        /// </summary>
        /// <remarks>
        /// Makes children bodies as the part of this body.
        /// Also calls <see cref="InitializeComponents"/> for its children.
        /// </remarks>
        public virtual void InitializeComponents()
        {
            foreach (Actor child in Children)
            {
                if (Body != null && child.Body != null)
                {
                    // make child body as the part of the parent body
                    // originally was used WeldJoint to make two bodies together 
                    // but the joint constraint sometimes produces strange behaviour (and also crashes)
                    // FarseerPhysics.Factories.JointFactory.CreateWeldJoint(Screen.World, Body, child.Body, new Vector2());

                    // new child angle, relative to the actor body
                    float newChildAngle = child.Body.Rotation - Body.Rotation;
                    // vector to make child shapes relative to the actor body
                    Vector2 moveToParent = Vector2.Transform(child.Body.Position,
                        Matrix.CreateTranslation(-Body.Position.X, -Body.Position.Y, 0f) *
                        Matrix.CreateRotationZ(-Body.Rotation) *
                        Matrix.CreateTranslation(Body.Position.X, Body.Position.Y, 0f))
                        - Body.Position;

                    foreach (Fixture fixture in child.Body.FixtureList)
                    {
                        Shape shape = null;

                        // move shape according to the parent body
                        // move polygon shape
                        if (fixture.ShapeType == ShapeType.Polygon)
                        {
                            PolygonShape polygon = fixture.Shape as PolygonShape;

                            polygon.Vertices.Rotate(newChildAngle);
                            polygon.Vertices.Translate(moveToParent);

                            Debug.Assert(polygon.Vertices.IsCounterClockWise());
                            Debug.Assert(!polygon.Vertices.CheckPolygon());

                            // reset polygon
                            polygon.Set(polygon.Vertices);
                            shape = polygon;
                        }
                        // move circle shape
                        else if (fixture.ShapeType == ShapeType.Circle)
                        {
                            CircleShape circle = fixture.Shape as CircleShape;

                            circle.Position += moveToParent;

                            shape = circle;
                        }
                        // move edge shape
                        else if (fixture.ShapeType == ShapeType.Edge)
                        {
                            EdgeShape edge = fixture.Shape as EdgeShape;

                            Matrix rotationMatrix = Matrix.CreateRotationZ(newChildAngle);

                            if (edge.HasVertex0) edge.Vertex0 = Vector2.Transform(edge.Vertex0, rotationMatrix) + moveToParent;
                            if (edge.HasVertex3) edge.Vertex3 = Vector2.Transform(edge.Vertex3, rotationMatrix) + moveToParent;

                            edge.Vertex1 = Vector2.Transform(edge.Vertex1, rotationMatrix) + moveToParent;
                            edge.Vertex2 = Vector2.Transform(edge.Vertex2, rotationMatrix) + moveToParent;

                            shape = edge;
                        }

                        // create new fixture
                        if (shape != null)
                        {
                            Fixture newFixture = Body.CreateFixture(shape, child);

                            // copy all settings from original fixture
                            newFixture.Restitution = fixture.Restitution;
                            newFixture.Friction = fixture.Friction;
                            newFixture.IsSensor = fixture.IsSensor;
                            newFixture.CollisionGroup = fixture.CollisionGroup;
                            newFixture.CollisionCategories = fixture.CollisionCategories;
                            newFixture.CollidesWith = fixture.CollidesWith;

                            // connect collision events to child events
                            newFixture.OnCollision += child.OnBodyCollision;
                            newFixture.OnSeparation += child.OnBodySeparation;
                        }
                    }

                    // reset child angle and position
                    child.Angle = child.Body.Rotation;
                    child.Position = child.Body.Position;

                    // remove original body
                    Screen.World.RemoveBody(child.Body);
                    // set child body as parent body
                    child._body = Body;
                }

                child.InitializeComponents();
            }
        }

        /// <summary>
        /// State machines initialization of the actor. 
        /// Activates the starting states of the state machines.
        /// Also calls <see cref="InitializeStateMachines"/> for its children.
        /// </summary>
        public void InitializeStateMachines()
        {
            for (int i = 0; i < stateMachines.Length; ++i)
            {
                stateMachines[i].Active = true;
            }

            for (int i = 0; i < Children.Count; ++i)
            {
                Children[i].InitializeStateMachines();
            }
        }

        /// <summary>
        /// Called when the <see cref="Actor"/> needs to be updated.
        /// </summary>
        /// <remarks>
        /// Updates the position and rotation angle, if the actor is in the physics simulation.
        /// Process collisions and separations from the physics simulation.
        /// Updates actual states of state machines.
        /// Updates renderer of the actor.
        /// Updates children by calling <see cref="Update"/> method on them.
        /// </remarks>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public override void Update(GameTime gameTime)
        {
            // update variables from physics simulation
            if (Body != null && Parent == null)
            {
                _position = Body.Position;
                _angle = Body.Rotation;
            }

            // process collisions events
            if (collisionEventsList.Count != 0)
            {
                for (int i = 0; i < collisionEventsList.Count; ++i)
                {
                    CollisionEventContainer collisionEventContainer = collisionEventsList[i];
                    if (collisionEventContainer.Type == CollisionEventContainer.CollisionEventType.Collision)
                    {
                        bool alreadySeparated = !((Actor)collisionEventContainer.FixtureB.UserData).Active;

                        if (OnCollision != null) OnCollision(collisionEventContainer.FixtureA, collisionEventContainer.FixtureB, collisionEventContainer.Contact);

                        if (alreadySeparated) Separate(collisionEventContainer.FixtureB);
                    }
                    else
                    {
                        if (OnSeparation != null) OnSeparation(collisionEventContainer.FixtureA, collisionEventContainer.FixtureB);
                    }
                }
                collisionEventsList.Clear();
            }

            // process actual states of states machines
            for (int i = 0; i < stateMachines.Length; ++i)
            {
                stateMachines[i].Update(gameTime);
            }

            // update actor renderer if any is available
            if (Renderer != null)
            {
                Renderer.Update(gameTime);
            }

            // update children
            for (int i = 0; i < Children.Count; ++i)
            {
                if (Children[i].UpdateCycle != UpdateCycle && Children[i].Active)
                {
                    Children[i].UpdateCycle = UpdateCycle;
                    Children[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="Actor"/> needs to be drawn.
        /// </summary>
        /// <remarks>
        /// Render actor if the actor renderer is available and the actor is visible.
        /// Draws children by calling <see cref="Draw"/> method on them.
        /// </remarks>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            // draw actor renderer if any is available
            if (Visible && Renderer != null)
            {
                Renderer.Draw(gameTime);
            }

            // draw children
            foreach (Actor child in Children)
            {
                child.Draw(gameTime);
            }
        }

        /// <summary>
        /// Destroys the actor and removes from the scene.
        /// </summary>
        public void Destroy()
        {
            if (Active)
            {
                Active = false;

                DestroyInternal(true);
            }
        }

        /// <summary>
        /// Destroys the actor and removes from the scene.
        /// </summary>
        /// <param name="enableBody">If set to <c>true</c> the actor body is enabled.</param>
        private void DestroyInternal(bool enableBody)
        {
            // destroy children
            while (Children.Count != 0)
            {
                Children[0].DestroyInternal(false);
            }

            // clear collisions
            ClearCollisionsList();

            // stop all state machines
            for (int i = 0; i < stateMachines.Length; ++i)
            {
                stateMachines[i].Active = false;
            }

            // remove from physics simulation
            if (Body != null)
            {
                Body.Enabled = false;

                // remove body
                if (Parent == null)
                {
                    Screen.World.RemoveBody(Body);
                }
                // remove fixtures
                // disable body, remove fixture and enable body
                // completely restart of body, every collision starts again
                // (not the best option for the game but the only possible way physics engine offers)
                else
                {
                    for (int i = 0; i < Body.FixtureList.Count; ++i)
                    {
                        Actor fixtureOwner = Body.FixtureList[i].UserData as Actor;
                        if (fixtureOwner != null)
                        {
                            fixtureOwner.ClearCollisionsList();

                            if (fixtureOwner == this)
                            {
                                Body.FixtureList.RemoveAt(i);
                                --i;
                            }
                        }
                    }

                    if (enableBody)
                    {
                        if (Body.FixtureList != null && Body.FixtureList.Count != 0)
                        {
                            Body.ResetMassData();
                            Body.Enabled = true;
                        }
                        else
                        {
                            Screen.World.RemoveBody(Body);
                        }
                    }
                }
            }

            // remove from parent
            if (Parent != null) Parent.Children.Remove(this);
            // remove actor from the scene
            else Screen.Nodes.Remove(this);
        }

        /// <summary>
        /// The specified actor separated from this actor. 
        /// Called on non-physics separation. (For example destroying the specified actor.)
        /// </summary>
        /// <param name="actor">The actor that separated from this actor.</param>
        private void Separate(Actor actor)
        {
            Debug.Assert(actor != null && actor.Body != null && actor.Body.FixtureList.Count != 0, "Actor cannot be in collision list.");

            int count;
            if (collisions.TryGetValue(actor, out count))
            {
                collisions.Remove(actor);

                // inform parent about separation
                if (Parent != null) Parent.Separate(actor);

                if (OnSeparation != null) OnSeparation(null, actor.Body.FixtureList[0]);
            }
        }

        /// <summary>
        /// The specified fixture separated from this actor.
        /// Called on non-physics separation. (For example destroying the specified fixture.)
        /// </summary>
        /// <param name="fixture">The fixture that separated from this actor.</param>
        private void Separate(Fixture fixture)
        {
            Debug.Assert(fixture != null && fixture.UserData is Actor);

            int count;
            if (collisions.TryGetValue(fixture.UserData, out count))
            {
                collisions.Remove(fixture.UserData);

                // inform parent about separation
                if (Parent != null) Parent.Separate(fixture);

                if (OnSeparation != null) OnSeparation(null, fixture);
            }
        }

        /// <summary>
        /// Removes all collisions of this actor. Other actors are informed about the separations.
        /// </summary>
        private void ClearCollisionsList()
        {
            foreach (Actor actorInCollision in GetActorsInCollisionWith())
            {
                if (actorInCollision.Active)
                {
                    Debug.Assert(actorInCollision != null && actorInCollision.Body != null && actorInCollision.Body.FixtureList.Count != 0, "Actor cannot be in collision list.");

                    // separate from the other actor
                    if (OnSeparation != null) OnSeparation(null, actorInCollision.Body.FixtureList[0]);
                    // inform the other actor that the actor is separating
                    actorInCollision.Separate(this);
                    // wake up the other actor body
                    actorInCollision.Body.Awake = true;
                }
            }

            collisions.Clear();
            collisionEventsList.Clear();
        }

        /// <summary>
        /// Removes all collisions of this actor and its children. Other actors are informed about the separations.
        /// </summary>
        private void ClearCollisions()
        {
            foreach (Actor child in Children)
            {
                child.ClearCollisions();
            }

            ClearCollisionsList();

            if (Body != null) Body.Awake = true;
        }

        /// <summary>
        /// Changes the states of state machines by specified transition name, if any transition is found.
        /// </summary>
        /// <param name="transitionName">Name of the transition.</param>
        public void ChangeState(string transitionName)
        {
            for (int i = 0; i < stateMachines.Length; ++i)
            {
                stateMachines[i].ChangeState(transitionName);
            }
        }

        /// <summary>
        /// Changes the state of the state machine, which <paramref name="currentState"/> is the active state, to the <paramref name="newState"/>.
        /// </summary>
        /// <param name="currentState">The active state of state machine where to change to the <paramref name="newState"/>.</param>
        /// <param name="newState">The state to change to.</param>
        /// <exception cref="ArgumentException">States are not from the same state machine. State machine from states does not exist. Current state is not current state in the state machine.</exception>
        public void ChangeState(State currentState, State newState)
        {
            if (currentState.StateMachineId != newState.StateMachineId) throw new ArgumentException("States are not from the same state machine.");
            if (currentState.StateMachineId >= stateMachines.Length) throw new ArgumentException("State machine does not exist.");
            if (stateMachines[currentState.StateMachineId] != currentState) throw new ArgumentException("Current state is not current state in the state machine.");

            currentState.Active = false;
            stateMachines[currentState.StateMachineId] = newState;
            newState.Active = true;
        }

        /// <summary>
        /// Called when this actor enters in the collision.
        /// If it is new collision with the other actor then saves collision for processing in <see cref="Update"/> and informs parents about the collision.
        /// </summary>
        /// <param name="fixtureA">The fixture of this actor.</param>
        /// <param name="fixtureB">The fixture of the other actor.</param>
        /// <param name="contact">The contact of the collision.</param>
        /// <returns>Value of <c>false</c> cancel the collision.</returns>
        private bool OnBodyCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            // contact can be disabled from one-way platform => keep disabled
            if (!contact.Enabled) return false;

            int count;
            if (!collisions.TryGetValue(fixtureB.UserData, out count)) count = 0;
            collisions[fixtureB.UserData] = count + 1;

            if (count == 0)
            {
                // inform parent about new collision
                if (Parent != null) Parent.OnBodyCollision(fixtureA, fixtureB, contact);
                // save collision event for update cycle
                if (OnCollision != null) collisionEventsList.Add(new CollisionEventContainer(CollisionEventContainer.CollisionEventType.Collision, fixtureA, fixtureB, contact));
            }
            return true;
        }

        /// <summary>
        /// Called when this actor exits in the collision.
        /// If it is last collision with the other actor then saves separation for processing in <see cref="Update"/> and informs parents about the separation.
        /// </summary>
        /// <param name="fixtureA">The fixture of this actor.</param>
        /// <param name="fixtureB">The fixture of the other actor.</param>
        private void OnBodySeparation(Fixture fixtureA, Fixture fixtureB)
        {
            int count;
            if (collisions.TryGetValue(fixtureB.UserData, out count))
            {
                --count;
                if (count == 0)
                {
                    collisions.Remove(fixtureB.UserData);

                    // inform parent about separation
                    if (Parent != null) Parent.OnBodySeparation(fixtureA, fixtureB);
                    // save separation event for update cycle
                    if (OnSeparation != null) collisionEventsList.Add(new CollisionEventContainer(CollisionEventContainer.CollisionEventType.Separation, fixtureA, fixtureB, null));
                }
                else
                {
                    collisions[fixtureB.UserData] = count;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this actor is in collision with the specified actor.
        /// </summary>
        /// <param name="actor">The actor to check collision with.</param>
        /// <returns><c>True</c> if is in collision with the specified actor; otherwise <c>false</c>.</returns>
        public bool InCollision(Actor actor)
        {
            int count;
            if (collisions.TryGetValue(actor, out count))
            {
                return count > 0;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether this actor is in collision.
        /// </summary>
        /// <returns><c>True</c> if is in collision; otherwise <c>false</c>.</returns>
        public bool InCollision()
        {
            return collisions.Count != 0;
        }

        /// <summary>
        /// Gets all actors that this actor is in collision with.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerable<Actor> GetActorsInCollisionWith()
        {
            Actor actor;
            foreach (object collisionObject in collisions.Keys)
            {
                actor = collisionObject as Actor;
                if (actor != null)
                {
                    yield return actor;
                }
            }
        }

        /// <summary>
        /// Completely initializes the actor.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="InitializeActor"/>, <see cref="InitializeComponents"/> and <see cref="InitializeStateMachines"/>.
        /// </remarks>
        public override void Initialize()
        {
            InitializeActor();
            InitializeComponents();
            InitializeStateMachines();
        }

        /// <summary>
        /// Gets the actor in this actor hierarchy.
        /// </summary>
        /// <param name="hierarchy">The hierarchy path where to find the actor.</param>
        /// <returns>Actor if found in the hierarchy path; otherwise null.</returns>
        private Actor GetActorInHierarchy(int[] hierarchy)
        {
            if (hierarchy == null || hierarchy.Length == 0) return null;

            Actor currentActor = this;
            for (int i = 0; i < hierarchy.Length; ++i)
            {
                // parent
                if (hierarchy[i] < 0)
                {
                    if (currentActor.Parent != null) currentActor = currentActor.Parent;
                    else return null;
                }
                // child
                else
                {
                    if (currentActor.Children != null && currentActor.Children.Count > hierarchy[i]) currentActor = currentActor.Children[hierarchy[i]];
                    else return null;
                }
            }

            return currentActor;
        }

        /// <summary>
        /// Gets the actor in this actor hierarchy and if not found then gets the actor by specified id.
        /// </summary>
        /// <param name="hierarchy">The hierarchy path where to find the actor.</param>
        /// <param name="otherwiseActorId">The id of the actor that is returned if the actor is not found in the hierarchy path.</param>
        /// <returns>Actor if found in the hierarchy path; otherwise actor with the specified id.</returns>
        protected Actor GetActorInHierarchy(int[] hierarchy, int otherwiseActorId)
        {
            Actor actorInHierarchy = GetActorInHierarchy(hierarchy);
            if (actorInHierarchy != null) return actorInHierarchy;

            Debug.Assert(Screen.Actors.Get(otherwiseActorId) != null);

            return Screen.Actors.Get(otherwiseActorId);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PhysicsGame"/> class.
        /// </summary>
        /// <returns>New instance of the <see cref="PhysicsGame"/> class.</returns>
        public abstract Actor Create();

        /// <summary>
        /// Container for storing collision or separation until is is processed in the <see cref="Update"/> method.
        /// </summary>
        private struct CollisionEventContainer
        {
            /// <summary>
            /// Defines event type of collision or separation.
            /// </summary>
            public enum CollisionEventType
            {
                /// <summary>
                /// Collision event.
                /// </summary>
                Collision,

                /// <summary>
                /// Separation event.
                /// </summary>
                Separation
            };

            /// <summary>
            /// The type of event.
            /// </summary>
            public CollisionEventType Type;

            /// <summary>
            /// Fixture A of the event.
            /// </summary>
            public Fixture FixtureA;

            /// <summary>
            /// Fixture B of the event.
            /// </summary>
            public Fixture FixtureB;

            /// <summary>
            /// Contact for the collision event.
            /// </summary>
            public Contact Contact;

            /// <summary>
            /// Initializes a new instance of the <see cref="CollisionEventContainer"/> struct.
            /// </summary>
            /// <param name="type">The type of event.</param>
            /// <param name="fixtureA">The fixture A.</param>
            /// <param name="fixtureB">The fixture B.</param>
            /// <param name="contact">The contact of the collision, if any.</param>
            public CollisionEventContainer(CollisionEventType type, Fixture fixtureA, Fixture fixtureB, Contact contact)
            {
                Type = type;
                FixtureA = fixtureA;
                FixtureB = fixtureB;
                Contact = contact;
            }
        }
    }
}
