/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    /// <summary>
    /// Physics settings used at <see cref="Actor"/>.
    /// </summary>
    [Serializable]
    class PhysicsComponent
    {
        /// <summary>
        /// Represents type of physics body.
        /// </summary>
        public enum BodyPhysicsType
        {
            /// <summary>
            /// Object is not in the physics simulation.
            /// </summary>
            None,

            /// <summary>
            /// Static body. Corresponding to <see cref="FarseerPhysics.Dynamics.BodyType.Static"/>.
            /// </summary>
            Static,

            /// <summary>
            /// Kinematic body. Corresponding to <see cref="FarseerPhysics.Dynamics.BodyType.Kinematic"/>.
            /// </summary>
            Kinematic,

            /// <summary>
            /// Dynamic body. Corresponding to <see cref="FarseerPhysics.Dynamics.BodyType.Dynamic"/>.
            /// </summary>
            Dynamic
        };

        /// <summary>
        /// Gets or sets the type of physics body.
        /// </summary>
        public BodyPhysicsType Type { get; set; }

        /// <summary>
        /// Gets or sets the density.
        /// </summary>
        public float Density { get; set; }

        /// <summary>
        /// Gets or sets the friction.
        /// </summary>
        public float Friction { get; set; }

        /// <summary>
        /// Gets or sets the restitution.
        /// </summary>
        public float Restitution { get; set; }

        /// <summary>
        /// Gets or sets the linear damping.
        /// </summary>
        public float LinearDamping { get; set; }

        /// <summary>
        /// Gets or sets the angular damping.
        /// </summary>
        public float AngularDamping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the body has fixed rotation.
        /// Corresponding to <see cref="FarseerPhysics.Dynamics.Body.FixedRotation"/>
        /// </summary>
        public bool FixedRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the body is bullet.
        /// Corresponding to <see cref="FarseerPhysics.Dynamics.Body.IsBullet"/>
        /// </summary>
        public bool Bullet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the body is sensor.
        /// Corresponding to <see cref="FarseerPhysics.Dynamics.Body.IsSensor"/>
        /// </summary>
        public bool Sensor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the body is one-way platform.
        /// </summary>
        public bool OneWayPlatform { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicsComponent"/> class.
        /// </summary>
        public PhysicsComponent()
        {
            Type = BodyPhysicsType.None;
            Density = 1f;
            Friction = 0.2f;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Cloned <see cref="PhysicsComponent"/>.</returns>
        public PhysicsComponent Clone()
        {
            PhysicsComponent clonedPhysics = new PhysicsComponent();

            clonedPhysics.AngularDamping = AngularDamping;
            clonedPhysics.Bullet = Bullet;
            clonedPhysics.Density = Density;
            clonedPhysics.FixedRotation = FixedRotation;
            clonedPhysics.Friction = Friction;
            clonedPhysics.LinearDamping = LinearDamping;
            clonedPhysics.Restitution = Restitution;
            clonedPhysics.Sensor = Sensor;
            clonedPhysics.Type = Type;
            clonedPhysics.OneWayPlatform = OneWayPlatform;

            return clonedPhysics;
        }
    }
}
