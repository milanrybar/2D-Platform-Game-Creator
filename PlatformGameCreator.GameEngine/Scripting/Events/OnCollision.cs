/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Events
{
    /// <summary>
    /// Fires when the specified actor is in collision/stays in collision/exits collision with the actor that goes throught the specified restriction.
    /// Instigator must be one of the <see cref="ActorRestriction"/> property or must have actor type one of the <see cref="ActorTypeRestriction"/> property.
    /// </summary>
    [FriendlyName("On Collision")]
    [Description("Fires when the specified actor is in collision/stays in collision/exits collision with the actor that goes throught the specified restriction. Instigator must be one of the Actor Restriction property or must have actor type one of the Actor Type Restriction property.")]
    [Category("Events")]
    public class OnCollisionEvent : EventNode
    {
        /// <summary>
        /// Fires when a collision occurs with the specified actor.
        /// </summary>
        [FriendlyName("On Collision Enter")]
        [Description("Fires when a collision occurs with the specified actor.")]
        public ScriptSocketHandler CollisionEnter;

        /// <summary>
        /// Fires when all actors exit the collisions with the specified actor.
        /// </summary>
        [FriendlyName("On Collision Exit")]
        [Description("Fires when all actors exit the collisions with the specified actor.")]
        public ScriptSocketHandler CollisionExit;

        /// <summary>
        /// Fires repeatedly while the specified actor is in collision.
        /// </summary>
        [FriendlyName("On Collision Stay")]
        [Description("Fires repeatedly while the specified actor is in collision.")]
        public ScriptSocketHandler CollisionStay;

        /// <summary>
        /// Actor to check collisions with.
        /// </summary>
        [Description("Actor to check collisions with.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor> Instance;

        /// <summary>
        /// Restrictions for the actors.
        /// </summary>
        [FriendlyName("Actor Restriction")]
        [Description("Restrictions for the actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Actor>[] ActorRestriction;

        /// <summary>
        /// Restrictions for the actor types.
        /// </summary>
        [FriendlyName("Actor Type Restriction")]
        [Description("Restrictions for the actor types.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<uint>[] ActorTypeRestriction;

        /// <summary>
        /// Outputs the actor which is/was in collision with the specified actor.
        /// </summary>
        [Description("Outputs the actor which is/was in collision with the specified actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Actor>[] Instigator;

        // Instance.Value actor, actor variable can not change
        private Actor actor;
        // last actor in collision with Instance
        private Actor lastActorInCollision;

        /// <inheritdoc />
        /// <remarks>
        /// Connects to the collision events by internal subscription.
        /// </remarks>
        public override void Connect()
        {
            if (Instance != null && Instance.Value != null && Instance.Value.Body != null)
            {
                actor = Instance.Value;

                Container.AddSubscription(new OnCollisionSubscription(this, actor, Container));
            }
        }

        /// <summary>
        /// Checks if the specified actor goes throught the specified restriction.
        /// </summary>
        /// <param name="instigator">Actor to check.</param>
        /// <returns>True if the specified actor goes throught the specified restriction otherwise false.</returns>
        private bool InstigatorAppliesRestriction(Actor instigator)
        {
            if (instigator != null)
            {
                if (ActorRestriction != null)
                {
                    for (int i = 0; i < ActorRestriction.Length; ++i)
                    {
                        if (instigator == ActorRestriction[i].Value)
                        {
                            return true;
                        }
                    }
                }

                for (int i = 0; i < ActorTypeRestriction.Length; ++i)
                {
                    if ((instigator.ActorType & ActorTypeRestriction[i].Value) != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Occurs when the collision with the specified actor (<see cref="Instance"/>) happens.
        /// Checks if the instigator goes through the specified restriction. If yes then fires <see cref="CollisionEnter"/>.
        /// </summary>
        /// <returns>Value of false cancel the collision.</returns>
        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Actor actorToTest = fixtureB.UserData as Actor;
            if (InstigatorAppliesRestriction(actorToTest))
            {
                lastActorInCollision = actorToTest;

                // set Instigator variable
                SetOutputVariable(lastActorInCollision, Instigator);

                // fires signal On Collision Enter
                if (CollisionEnter != null) CollisionEnter();
            }

            return true;
        }

        /// <summary>
        /// Occurs when the separation with the specified actor (<see cref="Instance"/>) happens.
        /// Checks if the instigator goes through the specified restriction. 
        /// If yes then checks if the there is another actor that goes through the specified restriction and if not then fires <see cref="CollisionExit"/>.
        /// </summary>
        private void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            Actor actorToTest = fixtureB.UserData as Actor;
            if (InstigatorAppliesRestriction(actorToTest) && !SetNextAvailableActorInCollision())
            {
                lastActorInCollision = null;

                // set Instigator variable
                SetOutputVariable(lastActorInCollision, Instigator);

                // fires signal On Collision Exit
                if (CollisionExit != null) CollisionExit();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if the there is any actor in collision with the specified actor. If yes then fires <see cref="CollisionStay"/>.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (lastActorInCollision != null && CollisionStay != null && actor.InCollision(lastActorInCollision))
            {
                // set Instigator variable
                SetOutputVariable(lastActorInCollision, Instigator);

                CollisionStay();
            }
        }

        /// <summary>
        /// Sets the another actor that is in collision with the specified actor and goes through the specified restriction.
        /// </summary>
        /// <returns>True if another actor is find, otherwise false.</returns>
        private bool SetNextAvailableActorInCollision()
        {
            foreach (Actor collisionActor in actor.GetActorsInCollisionWith())
            {
                if (InstigatorAppliesRestriction(collisionActor))
                {
                    lastActorInCollision = collisionActor;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Subscription for the collision events of the actor.
        /// </summary>
        private class OnCollisionSubscription : StateSubscription
        {
            private OnCollisionEvent onCollisionEvent;
            private Actor actor;

            public OnCollisionSubscription(OnCollisionEvent onCollisionEvent, Actor actor, State state)
                : base(state)
            {
                this.onCollisionEvent = onCollisionEvent;
                this.actor = actor;
            }

            protected override void OnSubscribe()
            {
                actor.OnCollision += onCollisionEvent.OnCollision;
                actor.OnSeparation += onCollisionEvent.OnSeparation;
                State.OnUpdate += onCollisionEvent.Update;

                // set actor in collision if any
                onCollisionEvent.SetNextAvailableActorInCollision();
            }

            protected override void OnUnsubscribe()
            {
                actor.OnCollision -= onCollisionEvent.OnCollision;
                actor.OnSeparation -= onCollisionEvent.OnSeparation;
                State.OnUpdate -= onCollisionEvent.Update;

                onCollisionEvent.lastActorInCollision = null;
            }
        }
    }
}
