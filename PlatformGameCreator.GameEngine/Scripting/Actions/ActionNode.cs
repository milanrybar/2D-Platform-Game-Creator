/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions
{
    /// <summary>
    /// Base class for action scripting node.
    /// </summary>
    /// <remarks>
    /// Action node provides easy API for updating itself: <see cref="StartUpdating"/> and <see cref="StopUpdating"/>.
    /// </remarks>
    public abstract class ActionNode : ScriptNode
    {
        /// <summary>
        /// Starts updating of the action node. <see cref="State"/> will call <see cref="ScriptNode.Update"/> every game update cycle.
        /// </summary>
        /// <remarks>
        /// Action is automatically updating until <see cref="StopUpdating"/> is called or until <see cref="State"/> (its <see cref="ScriptNode.Container"/>) is the active state of its state machine.
        /// </remarks>
        protected void StartUpdating()
        {
            if (updateSubscription == null)
            {
                updateSubscription = new UpdateSubscription(this, Container);
            }

            if (Container.Active && !updateSubscription.Active)
            {
                Container.AddSubscription(updateSubscription, true);
            }
        }

        /// <summary>
        /// Stops updating of the action node.
        /// </summary>
        protected void StopUpdating()
        {
            if (updateSubscription != null && updateSubscription.Active)
            {
                updateSubscription.Unsubscribe();
            }
        }

        /// <summary>
        /// Occurs when updating of the action node is stopped.
        /// </summary>
        protected virtual void OnUpdateStopped()
        {
        }

        // subscription for node updating
        private UpdateSubscription updateSubscription;

        /// <summary>
        /// Subscription for updating the action node.
        /// </summary>
        private class UpdateSubscription : StateSubscription
        {
            // action node to update
            private ActionNode actionNode;

            public UpdateSubscription(ActionNode actionNode, State state)
                : base(state)
            {
                this.actionNode = actionNode;
            }

            protected override void OnSubscribe()
            {
                State.OnUpdate += actionNode.Update;
            }

            protected override void OnUnsubscribe()
            {
                State.OnUpdate -= actionNode.Update;
                State.RemoveSubscription(this);
                actionNode.OnUpdateStopped();
            }
        }
    }
}
