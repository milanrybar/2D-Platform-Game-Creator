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
    /// Moves the specified actor to the specified location. It is not recommended to use it for the actor with dynamic body.
    /// </summary>
    /// <remarks>
    /// Non-physics actor is moved by setting its position.
    /// State body cannot be moved.
    /// Kinematic body is moved by setting its velocity to move to the location.
    /// Dynamic body is moved by creating FixedPrismaticJoint (in simulation) to move to the location.
    /// </remarks>
    [Description("Moves the specified actor to the specified location. It is not recommended to use it for the actor with dynamic body.")]
    [Category("Actions/Actors")]
    public abstract class BaseMoveToLocationAction : ActionNode
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
        /// Actor to move to the specified location.
        /// </summary>
        [FriendlyName("Target")]
        [Description("Actor to move to the specified location.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Target;

        /// <summary>
        /// Location to move the specified actor to.
        /// </summary>
        [FriendlyName("End Location")]
        [Description("Location to move the specified actor to.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> EndLocation;

        /// <summary>
        /// Actor to move to the location.
        /// </summary>
        protected Actor actor;

        /// <summary>
        /// Left time in seconds to get to the location.
        /// </summary>
        protected float leftTime;

        /// <summary>
        /// Direction to get to the location.
        /// </summary>
        protected Vector2 moveVector = Vector2.Zero;

        /// <summary>
        /// Speed of the movement.
        /// </summary>
        protected float speed;

        // prismatic joint for dynamic body of the actor
        private FixedPrismaticJoint fixedPrismaticJoint;

        /// <summary>
        /// Begins movement of the actor to the specified location.
        /// </summary>
        [Description("Begins movement of the actor to the specified location.")]
        public void In()
        {
            if (Target != null && Target.Value != null)
            {
                actor = Target.Value;

                InitMovement();
                StartUpdating();
            }

            if (Out != null) Out();
        }

        /// <summary>
        /// Compute and set all necessary settings to move to the location.
        /// </summary>
        protected virtual void InitMovement()
        {
            if (actor != null && actor.Body != null)
            {
                // kinematic body
                if (actor.Body.BodyType == FarseerPhysics.Dynamics.BodyType.Kinematic)
                {
                    actor.Body.LinearVelocity = moveVector;
                }
                // dynamic body
                else if (actor.Body.BodyType == FarseerPhysics.Dynamics.BodyType.Dynamic)
                {
                    fixedPrismaticJoint = JointFactory.CreateFixedPrismaticJoint(actor.Screen.World, actor.Body, EndLocation.Value, Vector2.Normalize(moveVector));
                    fixedPrismaticJoint.MotorEnabled = true;
                    fixedPrismaticJoint.MotorSpeed = speed;
                    fixedPrismaticJoint.MaxMotorForce = float.MaxValue / 100f;
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Keeps the actor on the path to the specified location.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            leftTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (leftTime > 0)
            {
                // no body
                if (actor.Body == null)
                {
                    actor.Position += moveVector * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                StopUpdating();

                if (Finished != null) Finished();
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Deactive the movement to the location for the actor.
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

    /// <inheritdoc />
    /// <summary>
    /// Moves the specified actor to the specified location in specified time. It is not recommended to use it for the actor with dynamic body.
    /// </summary>
    [FriendlyName("Move To Location")]
    [Description("Moves the specified actor to the specified location in specified time. It is not recommended to use it for the actor with dynamic body.")]
    public class MoveToLocationAction : BaseMoveToLocationAction
    {
        /// <summary>
        /// Total time in seconds to move to the specified location.
        /// </summary>
        [FriendlyName("Transition Time")]
        [Description("Total time in seconds to move to the specified location.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Time;

        /// <inheritdoc />
        protected override void InitMovement()
        {
            leftTime = Time.Value;
            if (leftTime > 0)
            {
                moveVector = (EndLocation.Value - Target.Value.Position) / leftTime;
                speed = moveVector.Length();

                base.InitMovement();
            }
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Moves the specified actor to the specified location by specified speed. It is not recommended to use it for the actor with dynamic body.
    /// </summary>
    [FriendlyName("Move To Location Fixed")]
    [Description("Moves the specified actor to the specified location by specified speed. It is not recommended to use it for the actor with dynamic body.")]
    public class MoveToLocationFixedAction : BaseMoveToLocationAction
    {
        /// <summary>
        /// Speed in meters per seconds to move to the specified location.
        /// </summary>
        [FriendlyName("Speed")]
        [Description("Speed in meters per seconds to move to the specified location.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue(1f)]
        public Variable<float> Speed;

        /// <inheritdoc />
        protected override void InitMovement()
        {
            moveVector = EndLocation.Value - Target.Value.Position;
            leftTime = moveVector.Length() / Speed.Value;
            if (leftTime > 0)
            {
                moveVector /= leftTime;
                speed = Speed.Value;

                base.InitMovement();
            }
        }
    }
}
