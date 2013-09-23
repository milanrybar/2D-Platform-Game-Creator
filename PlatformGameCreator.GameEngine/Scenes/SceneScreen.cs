/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using System.Diagnostics;
using PlatformGameCreator.GameEngine.Assets;
using PlatformGameCreator.GameEngine.Screens;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

namespace PlatformGameCreator.GameEngine.Scenes
{
    /// <summary>
    /// Screen representing the scene from the editor.
    /// </summary>
    public class SceneScreen : Screen
    {
        /// <summary>
        /// Gets the world of the physics simulation.
        /// </summary>
        public World World
        {
            get { return _world; }
        }
        private World _world;

        /// <summary>
        /// Gets the graphics assets used in the scene.
        /// </summary>
        public ContentManager<IGraphicsAsset> GraphicsAssets
        {
            get { return _graphicsAssets; }
        }
        /// <summary>
        /// All graphics assets used in the game.
        /// </summary>
        protected static ContentManager<IGraphicsAsset> _graphicsAssets;

        /// <summary>
        /// Gets the scene nodes.
        /// </summary>
        public SortedList<SceneNode> Nodes
        {
            get { return _nodes; }
        }
        private SortedList<SceneNode> _nodes = new SortedList<SceneNode>();

        /// <summary>
        /// Gets the actors of the scene which are defined from the editor, not created during runtime.
        /// </summary>
        public ContentManager<Actor> Actors
        {
            get { return _actors; }
        }
        private ContentManager<Actor> _actors = new ContentManager<Actor>();

        /// <summary>
        /// Gets the paths of the scene.
        /// </summary>
        public ContentManager<Path> Paths
        {
            get { return _paths; }
        }
        private ContentManager<Path> _paths = new ContentManager<Path>();

        /// <summary>
        /// Gets the camera of the scene.
        /// </summary>
        public Camera Camera
        {
            get { return _camera; }
        }
        private Camera _camera = new Camera();

        /// <summary>
        /// Gets the mouse position in the simulation units at the scene.
        /// </summary>
        public Vector2 SceneMousePosition
        {
            get { return ConvertUnits.ToSimUnits(Vector2.Transform(InputManager.MousePosition, Camera.InversWorld)); }
        }

        /// <summary>
        /// Gets the time elapsed in seconds since the last update cycle.
        /// </summary>
        public float ElapsedTime
        {
            get { return _elapsedTime; }
        }
        private float _elapsedTime;

        /// <summary>
        /// Background color of the scene.
        /// </summary>
        public Color BackgroundColor;

        /// <summary>
        /// Indicates whether the game FPS is drawn on the screen.
        /// </summary>
        public bool ShowFps = true;

        /// <summary>
        /// Position of the FPS text if the FPS is drawn on the screen. 
        /// </summary>
        public Vector2 fpsPosition = new Vector2(30, 25);

        /// <summary>
        /// Current number of the update cycle.
        /// </summary>
        private int updateCycle = 0;

        /// <summary>
        /// Called when the <see cref="SceneScreen"/> should be initialized.
        /// Initializes physics simulation and camera.
        /// </summary>
        public override void Initialize()
        {
            _world = new World(new Vector2(0f, 9.8f));

            _world.ContactManager.BeginContact += PhysicsBeginContact;
            _world.ContactManager.EndContact += PhysicsEndContact;

            Camera.Width = ScreenManager.GraphicsDevice.Viewport.Width;
            Camera.Height = ScreenManager.GraphicsDevice.Viewport.Height;
        }

        /// <summary>
        /// Fires when a contact in the physics simulation is created.
        /// Enables the feature of simple one-way platform in the game.
        /// Contact of the one-way platform should be solid only when the other fixture is moving down to the one-platform.
        /// Works only for gravity with the default direction (down).
        /// </summary>
        /// <param name="contact">Contact created in the physics simulation.</param>
        /// <returns>Value of <c>true</c> keeps the contact solid.</returns>
        private bool PhysicsBeginContact(Contact contact)
        {
            // check if one of the fixtures is the platform
            // inspired by http://www.iforce2d.net/b2dtut/one-way-walls
            if (contact.FixtureA.Body != null && contact.FixtureB.Body != null)
            {
                Fixture platformFixture = null;
                Body platformBody = null, otherBody = null;

                if (((Actor)contact.FixtureA.UserData).OneWayPlatform && !contact.FixtureB.IsSensor)
                {
                    platformFixture = contact.FixtureA;
                    platformBody = contact.FixtureA.Body;
                    otherBody = contact.FixtureB.Body;
                }
                else if (((Actor)contact.FixtureB.UserData).OneWayPlatform && !contact.FixtureA.IsSensor)
                {
                    platformFixture = contact.FixtureB;
                    platformBody = contact.FixtureB.Body;
                    otherBody = contact.FixtureA.Body;
                }

                if (platformBody != null)
                {
                    Vector2 normal;
                    FixedArray2<Vector2> points;
                    contact.GetWorldManifold(out normal, out points);

                    // check if contact points are moving downward
                    for (int i = 0; i < contact.Manifold.PointCount; ++i)
                    {
                        Vector2 pointVelPlatform = platformBody.GetLinearVelocityFromWorldPoint(points[i]);
                        Vector2 pointVelOther = otherBody.GetLinearVelocityFromWorldPoint(points[i]);
                        Vector2 relativeVel = platformBody.GetLocalVector(pointVelOther - pointVelPlatform);

                        if (platformFixture.ShapeType == FarseerPhysics.Collision.Shapes.ShapeType.Polygon)
                        {
                            PolygonShape polygon = platformFixture.Shape as PolygonShape;

                            Vector2 relativePoint = platformBody.GetLocalPoint(points[i]);
                            float platformFaceY = 0f;

                            // find the heighest vertex of the polygon
                            for (int j = 0; j < polygon.Vertices.Count; ++j)
                            {
                                if (polygon.Vertices[i].Y < platformFaceY) platformFaceY = polygon.Vertices[i].Y;
                            }

                            // check if point is at the front of the polygon
                            if (relativePoint.Y < platformFaceY + 0.05f)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // point is moving down, leave contact solid and exit
                            if (relativeVel.Y > 0)
                            {
                                return true;
                            }
                        }
                    }

                    // no points are moving downward, contact should not be solid
                    contact.Enabled = false;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Fires when a contact in the physics simulation is deleted.
        /// Enables the feature of simple one-way platform in the game.
        /// </summary>
        /// <param name="contact"></param>
        private void PhysicsEndContact(Contact contact)
        {
            // reset the default state of the contact in case it comes back for more
            if (!contact.Enabled) contact.Enabled = true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Updates the scene.
        /// Updates world in the physics simulation and all scene nodes.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            _elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // update world (physics simulation)
            World.Step(Math.Min(_elapsedTime, (1f / 30f)));

            // update camera
            Camera.Update(gameTime);

            // update scene nodes
            for (int i = 0; i < Nodes.Count; ++i)
            {
                if (!Nodes[i].Active)
                {
                    Nodes.RemoveAt(i);
                    --i;
                }
                else if (Nodes[i].UpdateCycle != updateCycle && Nodes[i].Active)
                {
                    Nodes[i].UpdateCycle = updateCycle;
                    Nodes[i].Update(gameTime);
                }
            }

            ++updateCycle;
        }

        /// <inheritdoc />
        /// <summary>
        /// Draws the scene.
        /// Draws background color and all scene nodes.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // clear screen
            ScreenManager.GraphicsDevice.Clear(BackgroundColor);

            // draw scene nodes
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.World);

            for (int i = Nodes.Count - 1; i >= 0; --i)
            {
                if (Nodes[i].Active) Nodes[i].Draw(gameTime);
            }

            ScreenManager.SpriteBatch.End();

            // draw FPS of the game
            if (ShowFps)
            {
                ScreenManager.SpriteBatch.Begin();

                string fpsText = ((PhysicsGame)ScreenManager.Game).Fps + " fps";
                ScreenManager.SpriteBatch.DrawString(ScreenManager.DefaultFont, fpsText, fpsPosition + Vector2.One, Color.Black);
                ScreenManager.SpriteBatch.DrawString(ScreenManager.DefaultFont, fpsText, fpsPosition, Color.White);

                ScreenManager.SpriteBatch.End();
            }
        }

        /// <summary>
        /// Adds the specified scene node to the scene.
        /// </summary>
        /// <param name="sceneNode">The scene node to add.</param>
        public void AddNode(SceneNode sceneNode)
        {
            Nodes.Add(sceneNode);
            sceneNode.Screen = this;
        }

        /// <summary>
        /// Adds the specified actor to the scene.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        public void AddNode(Actor actor)
        {
            Nodes.Add(actor);

            Actors.Add(actor.ActorId, actor);
        }

        /// <summary>
        /// Initializes the actors at the scene.
        /// </summary>
        protected void InitializeActors()
        {
            List<Actor> actorsWithChildren = new List<Actor>();

            // first init all actors
            foreach (Actor actor in Actors)
            {
                actor.Screen = this;

                actor.InitializeActor();

                if (actor.Children.Count != 0) actorsWithChildren.Add(actor);
            }

            // add actors children to the actors list so other actor can find them by id
            foreach (Actor actor in actorsWithChildren)
            {
                InitializeActorsAddChildren(actor);
            }

            // initialize all actors components = scripting
            foreach (Actor actor in Actors)
            {
                if (actor.Parent == null) actor.InitializeComponents();
            }

            // at last start all actors state machines after everythings else is ready.
            foreach (Actor actor in Actors)
            {
                if (actor.Parent == null) actor.InitializeStateMachines();
            }
        }

        /// <summary>
        /// Adds children of the specified actor to the <see cref="Actors"/>.
        /// </summary>
        /// <param name="actor">The actor to get chidren from.</param>
        private void InitializeActorsAddChildren(Actor actor)
        {
            foreach (Actor child in actor.Children)
            {
                Actors.Add(child.ActorId, child);
                InitializeActorsAddChildren(child);
            }
        }

        /// <summary>
        /// Called when graphics resources need to be loaded.
        /// </summary>
        public override void LoadContent()
        {

        }

        /// <summary>
        /// Called when graphics resources need to be unloaded.
        /// </summary>
        public override void UnloadContent()
        {

        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public virtual void Exit()
        {
            ScreenManager.Game.Exit();
        }
    }
}
