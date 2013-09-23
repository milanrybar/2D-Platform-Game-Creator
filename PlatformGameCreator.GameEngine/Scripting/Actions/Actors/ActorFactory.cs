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

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Spawns the specified number of actors in the specified position during runtime. The spawned actor is in the initial state, not the current one during runtime.
    /// </summary>
    [FriendlyName("Actor Factory")]
    [Description("Spawns the specified number of actors in the specified position during runtime. The spawned actor is in the initial state, not the current one during runtime.")]
    [Category("Actions/Actors")]
    public class ActorFactoryAction : ActionNode
    {
        /// <summary>
        /// Fires when all actors have been successfully spawned.
        /// </summary>
        [Description("Fires when all actors have been successfully spawned.")]
        public ScriptSocketHandler Finished;

        /// <summary>
        /// Fires for every spawned actor.
        /// </summary>
        [Description("Fires for every spawned actor.")]
        public ScriptSocketHandler Spawned;

        /// <summary>
        /// Actor to spawn.
        /// </summary>
        [FriendlyName("Spawn Actor")]
        [Description("Actor to spawn.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Actor> SpawnActor;

        /// <summary>
        /// Position for the spawned actor from the specified actor position. Otherwise <see cref="SpawnPoint"/> is used.
        /// </summary>
        [FriendlyName("Spawn Point")]
        [Description("Position for the spawned actor from the specified actor position. Otherwise the SpawnPoint property is used.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Actor> SpawnPoint;

        /// <summary>
        /// Position for the spawned actor.
        /// </summary>
        [FriendlyName("Spawn Location")]
        [Description("Position for the spawned actor.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> SpawnLocation;

        /// <summary>
        /// The total number of actors to spawn.
        /// </summary>
        [FriendlyName("Spawn Count")]
        [Description("The total number of actors to spawn.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(1)]
        public Variable<int> SpawnCount;

        /// <summary>
        /// The amount of time to wait between spawning new actor.
        /// </summary>
        [FriendlyName("Spawn Delay")]
        [Description("The amount of time to wait between spawning new actor.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(0.5f)]
        public Variable<float> SpawnDelay;

        /// <summary>
        /// Outputs the spawned actor.
        /// </summary>
        [FriendlyName("Spawned")]
        [Description("Outputs the spawned actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Actor>[] SpawnedActor;

        /// <summary>
        /// Begins a one-time spawning of actors.
        /// </summary>
        [FriendlyName("Spawn Actor")]
        [Description("Begins a one-time spawning of actors.")]
        public void Spawn()
        {
            leftActors = SpawnCount.Value;

            SpawnActors();

            if (leftActors > 0)
            {
                remainingTime = SpawnDelay.Value;
                StartUpdating();
            }
            else
            {
                if (Finished != null) Finished();
            }
        }

        // remaining time for spawning other actors
        private double remainingTime;
        // number of actors to spawn left
        private int leftActors;

        /// <summary>
        /// Spawns actors.
        /// </summary>
        private void SpawnActors()
        {
            if (SpawnActor == null || SpawnActor.Value == null)
            {
                leftActors = 0;
                return;
            }

            while (leftActors > 0)
            {
                // create new actor
                Actor newActor = SpawnActor.Value.Create();
                newActor.Screen = Container.Actor.Screen;
                newActor.Initialize();
                // set the actor position
                newActor.Position = SpawnPoint != null && SpawnPoint.Value != null ? SpawnPoint.Value.Position : SpawnLocation.Value;
                // set the current update cycle so the spawned actor will be updated next update cycle                
                // if the spawned actor is updated in the current cycle update there will be possibility of inifinite loop 
                // (for example actor cloning itself when some key is pressed)
                newActor.UpdateCycle = Container.Actor.UpdateCycle;

                Container.Actor.Screen.Nodes.Add(newActor);

                SetOutputVariable(newActor, SpawnedActor);

                --leftActors;

                // fires Spawned socket output
                if (Spawned != null) Spawned();

                if (SpawnDelay.Value > 0f && leftActors > 0)
                {
                    break;
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Spawns new actor after the time of <see cref="SpawnDelay"/> elapsed.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (remainingTime <= 0f)
            {
                remainingTime += SpawnDelay.Value;

                SpawnActors();

                if (leftActors == 0)
                {
                    StopUpdating();

                    if (Finished != null) Finished();
                }
            }
        }
    }
}
