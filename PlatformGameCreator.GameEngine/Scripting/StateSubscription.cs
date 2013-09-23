/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting
{
    /// <summary>
    /// Base class for making subscription to the <see cref="State"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Scripting node can need to run more then one game cycle. 
    /// It is not recommended to simple connect to the desired events (delegates) because the connection must be active only when the state, where the node is used, is the active state of its state machine.
    /// <see cref="StateSubscription"/> is the right thing to use.
    /// </para>
    /// <para>
    /// Connection made via <see cref="StateSubscription"/> is automatically connected and disconnected when needed.
    /// Defines methods to subscribe and unsubscribe from the state.
    /// <see cref="Subscribe"/> method is called when the subscription is made for the first time or the state is becoming the active state of its state machine.
    /// <see cref="Unsubscribe"/> method is called when the state is becoming the inactive state of its state machine.
    /// It must be add to the <see cref="State"/> for automatically connecting and disconnecting by <see cref="Scripting.State.AddSubscription"/> or removed by <see cref="Scripting.State.RemoveSubscription"/>.
    /// </para>
    /// </remarks>
    public abstract class StateSubscription
    {
        /// <summary>
        /// Gets the state for which the subscription is established for.
        /// </summary>
        public State State
        {
            get { return _state; }
        }
        private State _state;

        /// <summary>
        /// Gets a value indicating whether the subscription is active.
        /// </summary>
        public bool Active
        {
            get { return _active; }
        }
        private bool _active;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateSubscription"/> class.
        /// </summary>
        /// <param name="state">The state to subscribe to.</param>
        public StateSubscription(State state)
        {
            _state = state;
            _active = false;
        }

        /// <summary>
        /// Subscribes to the <see cref="State"/>.
        /// </summary>
        public void Subscribe()
        {
            if (!Active)
            {
                _active = true;
                OnSubscribe();
            }
        }

        /// <summary>
        /// Unsubscribes from the <see cref="State"/>.
        /// </summary>
        public void Unsubscribe()
        {
            if (Active)
            {
                _active = false;
                OnUnsubscribe();
            }
        }

        /// <summary>
        /// Called when it subscribes to the <see cref="State"/>. 
        /// </summary>
        /// <remarks>
        /// Derivatived class makes the real subscribing.
        /// </remarks>
        protected abstract void OnSubscribe();

        /// <summary>
        /// Called when it unsubscribes from the <see cref="State"/>.
        /// </summary>
        /// <remarks>
        /// Derivative class makes the real unsubscribing.
        /// </remarks>
        protected abstract void OnUnsubscribe();
    }
}
