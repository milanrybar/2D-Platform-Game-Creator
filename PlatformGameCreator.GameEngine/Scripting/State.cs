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

namespace PlatformGameCreator.GameEngine.Scripting
{
    /// <summary>
    /// State of the state machine used in the <see cref="Actor"/>.
    /// </summary>
    public class State
    {
        /// <summary>
        /// Gets the actor where its state machine is used.
        /// </summary>
        public Actor Actor
        {
            get { return _actor; }
        }
        private Actor _actor;

        /// <summary>
        /// Gets the id of the state machine where the <see cref="State"/> is used.
        /// </summary>
        public int StateMachineId
        {
            get { return _stateMachineId; }
        }
        private int _stateMachineId;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="State"/> is the active of its state machine.
        /// </summary>
        public bool Active
        {
            get { return _active; }
            set
            {
                if (value && !_active)
                {
                    _active = value;
                    Starting();
                }
                else if (!value && _active)
                {
                    Ending();
                    _active = value;
                }
            }
        }
        private bool _active;

        /// <summary>
        /// Occurs when the state is starting (becoming active).
        /// </summary>
        public ScriptSocketHandler OnStart;

        /// <summary>
        /// Occurs when the state is end (becoming inactive).
        /// </summary>
        public ScriptSocketHandler OnEnd;

        /// <summary>
        /// Occurs when the state is updated.
        /// </summary>
        public UpdateHandler OnUpdate;

        /// <summary>
        /// Transitions from the state. Pair of (Transition name (event in), State).
        /// </summary>
        private Dictionary<string, State> transitions = new Dictionary<string, State>();

        /// <summary>
        /// Subscriptions for the state.
        /// </summary>
        private List<StateSubscription> subscriptions = new List<StateSubscription>();

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        /// <param name="actor">The actor of its state machine</param>
        /// <param name="stateMachineId">The state machine id.</param>
        public State(Actor actor, int stateMachineId)
        {
            _actor = actor;
            _stateMachineId = stateMachineId;
        }

        /// <summary>
        /// Update the state. Only invokes <see cref="OnUpdate"/>.
        /// </summary>
        /// <remarks>
        /// Scripting nodes are updated from invoking <see cref="OnUpdate"/> or they are activated from different events but their subscription is saved in this state.
        /// </remarks>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            if (OnUpdate != null) OnUpdate(gameTime);
        }

        /// <summary>
        /// Adds the transition to the state.
        /// </summary>
        /// <param name="transitionName">Transition name.</param>
        /// <param name="state">The state to move to by transition.</param>
        public void AddTransition(string transitionName, State state)
        {
            transitions[transitionName] = state;
        }

        /// <summary>
        /// Changes the state of its state machine by the specified transition name.
        /// </summary>
        /// <param name="transitionName">Transition name to move to.</param>
        public void ChangeState(string transitionName)
        {
            State newState;
            if (transitions.TryGetValue(transitionName, out newState))
            {
                Actor.ChangeState(this, newState);
            }
        }

        /// <summary>
        /// The state is starting (becoming active).
        /// Subscribes all its subscriptions and call <see cref="OnStart"/>.
        /// </summary>
        private void Starting()
        {
            foreach (StateSubscription subscription in subscriptions)
            {
                subscription.Subscribe();
            }

            if (OnStart != null) OnStart();
        }

        /// <summary>
        /// The state is ending (becoming inactive).
        /// Unsubscribes all its subscriptions and call <see cref="OnEnd"/>.
        /// </summary>
        private void Ending()
        {
            for (int i = 0; i < subscriptions.Count; ++i)
            {
                subscriptions[i].Unsubscribe();

                // subscription remove itself from subscriptions
                if (i < subscriptions.Count && subscriptions[i].Active) --i;
            }

            if (OnEnd != null) OnEnd();
        }

        /// <summary>
        /// Adds the subscription to the state.
        /// </summary>
        /// <param name="subscription">The subscription to add.</param>
        /// <param name="subscribe">If set to <c>true</c> subscription is automatically subscribed.</param>
        /// <seealso cref="StateSubscription"/>
        public void AddSubscription(StateSubscription subscription, bool subscribe = false)
        {
            subscriptions.Add(subscription);

            if (subscribe) subscription.Subscribe();
        }

        /// <summary>
        /// Removes the subscription from the state.
        /// </summary>
        /// <param name="subscription">The subscription to remove</param>
        /// <seealso cref="StateSubscription"/>
        public void RemoveSubscription(StateSubscription subscription)
        {
            subscriptions.Remove(subscription);

            if (subscription.Active) subscription.Unsubscribe();
        }
    }
}
