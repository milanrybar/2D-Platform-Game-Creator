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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Moves the specified actor on the specified path. It is not recommended to use it for the actor with dynamic body.
    /// </summary>
    /// <remarks>
    /// Non-physics actor is moved by setting its position.
    /// State body cannot be moved.
    /// Kinematic body is moved by setting its velocity to move to the next vertex.
    /// Dynamic body is moved by creating FixedPrismaticJoint (in simulation) to move to the next vertex.
    /// </remarks>
    [FriendlyName("Move On Path")]
    [Description("Moves the specified actor on the specified path. It is not recommended to use it for the actor with dynamic body.")]
    [Category("Actions/Actors")]
    public class MoveOnPathAction : ActionNode
    {
        /// <summary>
        /// Fires when the movement has begun.
        /// </summary>
        [Description("Fires when the movement has begun.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Fires when the whole movement is finished.
        /// </summary>
        [Description("Fires when the whole movement is finished.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Actor to move on the specified path.
        /// </summary>
        [Description("Actor to move on the specified path.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Target;

        /// <summary>
        /// Path to move the specified actor on.
        /// </summary>
        [Description("Path to move the specified actor on.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Path> Path;

        /// <summary>
        /// Speed in meters per seconds to move on the specified path.
        /// </summary>
        [FriendlyName("Speed")]
        [Description("Speed in meters per seconds to move on the specified path.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Speed;

        /// <summary>
        /// Indicates whether the actor moves to the right (the next vertex from definition of the path) on the specified path. (Left is previous vertex.)
        /// </summary>
        [FriendlyName("Move Right")]
        [Description("Indicates whether the actor moves to the right (the next vertex from definition of the path) on the specified path. (Left is previous vertex.)")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(true)]
        public Variable<bool> MoveRight;

        /// <summary>
        /// Indicates whether the actor moves in the loop.
        /// </summary>
        [Description("Indicates whether the actor moves in the loop.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(false)]
        public Variable<bool> Loop;

        /// <summary>
        /// Begins movement of the actor on the specified path.
        /// </summary>
        [Description("Begins movement of the actor on the specified path.")]
        public void Move()
        {
            if (Path != null && Path.Value != null && Target != null && Target.Value != null)
            {
                // save node settings, change is not supported
                actor = Target.Value;
                path = Path.Value;
                moveRight = MoveRight.Value;

                // find nearest vertex on path
                int nearestVertex = 0;
                float minLength = float.MaxValue;
                for (int i = 0; i < Path.Value.Vertices.Length; ++i)
                {
                    float length = (Target.Value.Position - Path.Value.Vertices[i]).Length();
                    if (length < minLength)
                    {
                        minLength = length;
                        nearestVertex = i;
                    }
                }

                SetMoveToVertex(nearestVertex);

                startVertex = nearestVertex;
                actualVertex = nearestVertex;

                StartUpdating();
            }

            if (Out != null) Out();
        }

        // path to move on
        private Path path;
        // actor to move on the path
        private Actor actor;
        // indicates whether the actor moves to the right
        private bool moveRight;

        // starting and actual vertex of the path
        private int startVertex, actualVertex;
        // left time to get to the next vertex
        private float leftTime = 0;
        // move vector to get to the next vertex
        private Vector2 moveVector = Vector2.Zero;

        // actual prismatic joint for dynamic body of the actor
        private FixedPrismaticJoint fixedPrismaticJoint;

        /// <summary>
        /// Compute and set all necessary settings to move to the vertex specified by <paramref name="vertexIndex"/>.
        /// </summary>
        /// <param name="vertexIndex">Vertex to move to.</param>
        private void SetMoveToVertex(int vertexIndex)
        {
            // compute move vector and time to get to the vertex
            moveVector = path.Vertices[vertexIndex] - actor.Position;
            leftTime = moveVector.Length() / Speed.Value;
            moveVector /= leftTime;

            // destroy prismatic joint if any
            if (fixedPrismaticJoint != null)
            {
                actor.Screen.World.RemoveJoint(fixedPrismaticJoint);
                fixedPrismaticJoint = null;
            }

            // kinematic body
            if (actor.Body != null && actor.Body.BodyType == FarseerPhysics.Dynamics.BodyType.Kinematic)
            {
                actor.Body.LinearVelocity = moveVector;
            }
            // dynamic body
            else if (actor.Body != null && actor.Body.BodyType == FarseerPhysics.Dynamics.BodyType.Dynamic)
            {
                fixedPrismaticJoint = JointFactory.CreateFixedPrismaticJoint(actor.Screen.World, actor.Body, path.Vertices[vertexIndex], Vector2.Normalize(moveVector));
                fixedPrismaticJoint.MotorEnabled = true;
                fixedPrismaticJoint.MotorSpeed = Speed.Value;
                fixedPrismaticJoint.MaxMotorForce = float.MaxValue / 100f;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Keeps the actor on the path.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            leftTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // still moving to the next vertex
            if (leftTime > 0)
            {
                // no body
                if (actor.Body == null)
                {
                    actor.Position += moveVector * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            // change the vertex or finished
            else
            {
                if (moveRight)
                {
                    ++actualVertex;
                    if (actualVertex >= path.Vertices.Length && Loop.Value)
                    {
                        if (path.Loop)
                        {
                            actualVertex = 0;
                        }
                        else
                        {
                            moveRight = !moveRight;
                            actualVertex -= 2;
                        }
                    }
                }
                else
                {
                    --actualVertex;
                    if (actualVertex < 0 && Loop.Value)
                    {
                        if (path.Loop)
                        {
                            actualVertex = path.Vertices.Length - 1;
                        }
                        else
                        {
                            moveRight = !moveRight;
                            actualVertex = 1;
                        }
                    }
                }

                if (actualVertex >= path.Vertices.Length || actualVertex < 0 || (actualVertex == startVertex && !Loop.Value))
                {
                    StopUpdating();

                    if (Finished != null) Finished();
                }
                else
                {
                    SetMoveToVertex(actualVertex);
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Deactive the movement of the path for the actor.
        /// </remarks>
        protected override void OnUpdateStopped()
        {
            if (fixedPrismaticJoint != null)
            {
                actor.Screen.World.RemoveJoint(fixedPrismaticJoint);
                fixedPrismaticJoint = null;
            }

            if (actor != null && actor.Body != null && actor.Body.BodyType == FarseerPhysics.Dynamics.BodyType.Kinematic)
            {
                actor.Body.LinearVelocity = Vector2.Zero;
            }
        }
    }
}
