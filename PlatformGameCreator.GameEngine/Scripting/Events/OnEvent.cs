/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Events
{
    /// <summary>
    /// Fires when any of the specified event is invoked.
    /// </summary>
    [FriendlyName("On Event")]
    [Description("Fires when any of the specified event is invoked.")]
    [Category("Events")]
    public class OnEvent : EventNode
    {
        /// <summary>
        /// Fires when any of the specified event is invoked.
        /// </summary>
        [FriendlyName("On Event")]
        [Description("Fires when any of the specified event is invoked.")]
        public ScriptSocketHandler OnInvoke;

        /// <summary>
        /// Actors where to find events by the specified names.
        /// </summary>
        [Description("Actors where to find events by the specified names.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Names of the events in the specified actors.
        /// </summary>
        [Description("Names of the events in the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<string>[] Name;

        /// <inheritdoc />
        /// <remarks>
        /// Connects to the specified event wrappers by internal subscription.
        /// </remarks>
        public override void Connect()
        {
            if (Instance != null)
            {
                for (int i = 0; i < Instance.Length; ++i)
                {
                    for (int j = 0; j < Name.Length; ++j)
                    {
                        EventWrapper eventWrapper = Instance[i].Value.GetEvent(Name[j].Value);

                        if (eventWrapper != null)
                        {
                            Container.AddSubscription(new OnEventSubscription(this, eventWrapper, Container));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event has been invoked => fires <see cref="OnInvoke"/>.
        /// </summary>
        private void Invoking()
        {
            if (OnInvoke != null) OnInvoke();
        }

        /// <summary>
        /// Subscription for the wrapper event.
        /// </summary>
        private class OnEventSubscription : StateSubscription
        {
            private OnEvent onEvent;
            private EventWrapper eventWrapper;

            public OnEventSubscription(OnEvent onEvent, EventWrapper eventWrapper, State state)
                : base(state)
            {
                this.onEvent = onEvent;
                this.eventWrapper = eventWrapper;
            }

            protected override void OnSubscribe()
            {
                eventWrapper.Event += onEvent.Invoking;
            }

            protected override void OnUnsubscribe()
            {
                eventWrapper.Event -= onEvent.Invoking;
            }
        }
    }
}
