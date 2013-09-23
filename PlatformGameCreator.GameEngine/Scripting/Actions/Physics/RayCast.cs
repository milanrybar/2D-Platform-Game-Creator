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
using FarseerPhysics.Dynamics;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Physics
{
    /// <summary>
    /// Performs a ray-cast from the starting point to the end point, determines if anything was hit along the way, and fires the associated output socket. 
    /// The first hit actor is returned as well as the distance to the hit object and the location of the hit.
    /// The ray-cast ignores the actor that contains the starting point.
    /// </summary>
    [FriendlyName("RayCast")]
    [Description("Performs a ray-cast from the starting point to the end point, determines if anything was hit along the way, and fires the associated output socket. The first hit actor is returned as well as the distance to the hit object and the location of the hit. The ray-cast ignores the actor that contains the starting point.")]
    [Category("Actions/Physics")]
    public class RayCastAction : ActionNode
    {
        /// <summary>
        /// Fires if the ray-cast hit nothing.
        /// </summary>
        [FriendlyName("Not Obstructed")]
        [Description("Fires if the ray-cast hit nothing.")]
        public ScriptSocketHandler NotObstructed;

        /// <summary>
        /// Fires if the ray-cast hit something.
        /// </summary>
        [FriendlyName("Obstructed")]
        [Description("Fires if the ray-cast hit something.")]
        public ScriptSocketHandler Obstructed;

        /// <summary>
        /// The starting point for the ray-cast.
        /// </summary>
        [Description("The starting point for the ray-cast.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Start;

        /// <summary>
        /// The end point for the ray-cast.
        /// </summary>
        [Description("The end point for the ray-cast.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> End;

        /// <summary>
        /// Outputs the actor hit by the ray-cast, if any.
        /// </summary>
        [FriendlyName("Hit Actor")]
        [Description("Outputs the actor hit by the ray-cast, if any.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Actor>[] HitActor;

        /// <summary>
        /// Outputs the distance from the starting point to the Hit Actor.
        /// </summary>
        [FriendlyName("Hit Distance")]
        [Description("Outputs the distance from the starting point to the Hit Actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<float>[] HitDistance;

        /// <summary>
        /// Outputs the point of intersection of the hit actor.
        /// </summary>
        [FriendlyName("Hit Location")]
        [Description("Outputs the point of intersection of the hit actor.")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] HitPoint;

        /// <summary>
        /// Outputs the unit normal vector of the hit actor.
        /// </summary>
        [FriendlyName("Hit Normal")]
        [Description("Outputs the unit normal vector of the hit actor.")]
        [VariableSocket(VariableSocketType.Out, Visible = false)]
        public Variable<Vector2>[] HitNormal;

        // indicates whether the current ray-cast hit something
        private bool obstructed;
        // actor hit by the ray-cast
        private Actor hitActor;
        // point and normal of actor hit by the ray-cast
        private Vector2 hitPoint, hitNormal;

        /// <summary>
        /// Performs a ray-cast from the starting point to the end point.
        /// </summary>
        [Description("Performs a ray-cast from the starting point to the end point.")]
        public void In()
        {
            obstructed = false;

            Container.Actor.Screen.World.RayCast(RayCastCallback, Start.Value, End.Value);

            if (obstructed)
            {
                SetOutputVariable(hitActor, HitActor);
                SetOutputVariable((Start.Value - hitPoint).Length(), HitDistance);
                SetOutputVariable(hitPoint, HitPoint);
                SetOutputVariable(hitNormal, HitNormal);

                if (Obstructed != null) Obstructed();
            }
            else if (NotObstructed != null) NotObstructed();
        }

        /// <summary>
        /// The ray-cast callback. Called for each fixture found in the query.
        /// Gets the first actor and terminates the ray-cast.
        /// </summary>
        /// <param name="fixture">Fixture hit by the ray-cast.</param>
        /// <param name="point">The point of intersection.</param>
        /// <param name="normal">The unit normal vector.</param>
        /// <param name="fraction">The fractional distance along the ray.</param>
        /// <returns>-1 to filter, 0 to terminate, fraction to clip the ray for closest hit, 1 to continue</returns>
        private float RayCastCallback(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            Actor actor = fixture.UserData as Actor;

            if (actor != null)
            {
                obstructed = true;

                hitActor = actor;
                hitPoint = point;
                hitNormal = normal;

                return 0;
            }

            return -1;
        }
    }
}
