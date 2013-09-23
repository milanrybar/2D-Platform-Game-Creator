/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.Editor.Assets;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Scenes;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGameCreator.Editor.GameObjects.Actors
{
    /// <summary>
    /// Represents <see cref="Actors.Actor"/> at the scene (at <see cref="SceneScreen"/> control).
    /// </summary>
    class ActorView : ShapesSceneNode
    {
        /// <summary>
        /// Gets the underlying <see cref="Actors.Actor"/>.
        /// </summary>
        public Actor Actor
        {
            get { return _actor; }
        }
        private Actor _actor;

        /// <summary>
        /// Gets the underlying <see cref="Actors.Actor"/>.
        /// </summary>
        public override GameObject Tag
        {
            get { return _actor; }
        }

        /// <summary>
        /// Gets or sets the position of the actor at the scene.
        /// </summary>
        /// <remarks>
        /// Position of the actor is cached until any change happens.
        /// </remarks>
        public override Vector2 Position
        {
            get
            {
                // actor is at the parallax layer
                if (Actor.Layer != null && Actor.Layer.IsParallax && SceneControl != null)
                {
                    return _position + SceneControl.Position * Actor.Layer.ParallaxCoefficient;
                }
                else
                {
                    return _position;
                }
            }
            set
            {
                // actor is at the parallax layer
                if (Actor.Layer != null && Actor.Layer.IsParallax)
                {
                    Actor.Position = value - SceneControl.Position * Actor.Layer.ParallaxCoefficient;
                }
                else
                {
                    Actor.Position = value;
                }
            }
        }
        private Vector2 _position;

        /// <summary>
        /// Gets or sets the rotation angle in radians of the actor.
        /// </summary>
        /// <remarks>
        /// Rotation angle of the actor is cached until any change happens.
        /// </remarks>
        public override float Angle
        {
            get { return _angle; }
            set { Actor.Angle = value; }
        }
        private float _angle;

        /// <summary>
        /// Gets or sets the scale factor of the actor.
        /// </summary>
        /// <remarks>
        /// Scale factor of the actor is cached until any change happens.
        /// </remarks>
        public override Vector2 ScaleFactor
        {
            get { return _scaleFactor; }
            set { Actor.ScaleFactor = value; }
        }
        private Vector2 _scaleFactor;

        /// <summary>
        /// Gets or sets the bounding rectangle of the actor.
        /// </summary>
        public override FarseerPhysics.Collision.AABB Rectangle
        {
            get
            {
                // actor is at the parallax layer
                if (Actor.Layer != null && Actor.Layer.IsParallax)
                {
                    UpdateShapes();
                }

                return base.Rectangle;
            }
        }

        /// <inheritdoc />
        public override bool CanMove
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanRotate
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool CanScale
        {
            get { return true; }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Actor is visible when its parent if any is visible and layer where the actor is used is visible.
        /// </remarks>
        public override bool Visible
        {
            get
            {
                if (Parent == null) return Actor.Visible && Actor.Layer.Visible;
                else return Actor.Visible && Parent.Visible;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Index contains information about layer index and the actor index.
        /// </remarks>
        public override int Index
        {
            get
            {
                int index = 0;
                if (Actor.Parent == null) index = (Actor.Layer.Index + 1) << 16;
                index += Actor.Index;

                return index;
            }
        }

        /// <summary>
        /// Scene drawable element for the <see cref="ActorView"/>.
        /// </summary>
        private ISceneDrawable sceneDrawable;

        /// <summary>
        /// Indicates whether the <see cref="ActorView"/> needs to update its appearance settings.
        /// </summary>
        private bool updateAppearance;

        /// <summary>
        /// Indicates whether the actor contains any collision shapes.
        /// </summary>
        private bool containsShapes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorView"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public ActorView(Actor actor)
        {
            _actor = actor;

            if (Actor.DrawableAsset != null)
            {
                sceneDrawable = Actor.DrawableAsset.CreateView();

                Actor.DrawableAsset.DrawableAssetChanged += new EventHandler(DrawableAsset_DrawableAssetChanged);
            }

            Actor.AppearanceChanged += new EventHandler(Actor_AppearanceChanged);
            Actor.DrawableAssetChanged += new ValueChangedHandler<DrawableAsset>(Actor_DrawableAssetChanged);

            Actor.Children.ListChanged += new ObservableList<Actor>.ListChangedEventHandler(Children_ListChanged);

            UpdateAppearance();
        }

        /// <summary>
        /// Handles the <see cref="DrawableAsset.DrawableAssetChanged"/> event of the <see cref="DrawableAsset"/> of the actor.
        /// Updates collision shapes of the actor which are shown at the scene.
        /// </summary>
        private void DrawableAsset_DrawableAssetChanged(object sender, EventArgs e)
        {
            UpdateShapes();
        }

        /// <summary>
        /// Handles the <see cref="Actors.Actor.DrawableAssetChanged"/> event of the <see cref="Actor"/>.
        /// Updates the scene drawable element for the <see cref="ActorView"/> and collision shapes which are shown at the scene.
        /// </summary>
        private void Actor_DrawableAssetChanged(object sender, ValueChangedEventArgs<DrawableAsset> e)
        {
            if (e.OldValue != null)
            {
                e.OldValue.DrawableAssetChanged -= new EventHandler(DrawableAsset_DrawableAssetChanged);
            }

            if (Actor.DrawableAsset != null)
            {
                sceneDrawable = Actor.DrawableAsset.CreateView();

                Actor.DrawableAsset.DrawableAssetChanged += new EventHandler(DrawableAsset_DrawableAssetChanged);
            }
            else
            {
                sceneDrawable = null;
            }

            UpdateShapes();
        }

        /// <summary>
        /// Handles the <see cref="Actors.Actor.AppearanceChanged"/> event of the <see cref="Actor"/>.
        /// Notes that appearance needs to be updated.
        /// </summary>
        private void Actor_AppearanceChanged(object sender, EventArgs e)
        {
            updateAppearance = true;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Creates <see cref="ActorView">scene nodes</see> for the actor children.
        /// </remarks>
        public override void Initialize()
        {
            base.Initialize();

            foreach (Actor child in Actor.Children)
            {
                ActorView newChildView = (ActorView)Children.Add(child);
                newChildView.Parent = this;
            }
        }

        /// <summary>
        /// Handles the ListChanged event of the children of the actor.
        /// Updates the actor children (child is added or removed).
        /// </summary>
        private void Children_ListChanged(object sender, ObservableListChangedEventArgs<Actor> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    ActorView addedActorView = (ActorView)Children.Add(e.Item);
                    addedActorView.Parent = this;
                    break;

                case ObservableListChangedType.ItemDeleted:
                    Children.Remove(e.Item);
                    break;
            }
        }

        /// <summary>
        /// Updates the collision shapes which are shown at the scene (at <see cref="SceneScreen"/>).
        /// </summary>
        protected void UpdateShapes()
        {
            // clear old shapes
            Shapes.Clear();

            // update all shapes
            if (Actor.Shapes != null)
            {
                foreach (Shape shape in Actor.Shapes)
                {
                    Shape newShape = shape.Clone();

                    newShape.Move(Position);
                    newShape.Scale(ScaleFactor, Position);
                    newShape.Rotate(Angle, Position);

                    Shapes.Add(newShape);
                }
            }

            containsShapes = Shapes.Count != 0;

            // no shape
            if (Shapes.Count == 0)
            {
                // set rectangle to border texture
                if (sceneDrawable != null)
                {
                    Polygon textureRectangle = sceneDrawable.GetBounds(Position);
                    if (textureRectangle != null)
                    {
                        textureRectangle.Scale(ScaleFactor, Position);
                        textureRectangle.Rotate(Angle, Position);

                        Shapes.Add(textureRectangle);
                    }
                }

                // set some shape otherwise the actor cannot be selected
                if (Shapes.Count == 0)
                {
                    float defaultShapeSize = 50f;
                    Polygon defaultRectangle = Polygon.CreateAsRectangle(Position - new Vector2(defaultShapeSize / 2f, defaultShapeSize / 2f), defaultShapeSize, defaultShapeSize);
                    defaultRectangle.Scale(ScaleFactor, Position);
                    defaultRectangle.Rotate(Angle, Position);

                    Shapes.Add(defaultRectangle);
                }
            }

            // update AABB rectangle
            UpdateRectangle();
        }

        /// <inheritdoc />
        public override void Move(Vector2 move)
        {
            if (!CanMove) return;

            // new position
            Position += move;

            // no need to update shapes because 
            // it will be done at the response for the event AppearanceChanged from the actor
        }

        /// <inheritdoc />
        public override void Rotate(float angle)
        {
            if (!CanRotate) return;

            // new angle
            Angle += angle;

            // no need to update shapes because 
            // it will be done at the response for the event AppearanceChanged from the actor
        }

        /// <inheritdoc />
        public override void Scale(Vector2 scale)
        {
            if (!CanScale) return;

            // new scale factor
            ScaleFactor *= scale;

            // no need to update shapes because 
            // it will be done at the response for the event AppearanceChanged from the actor
        }

        /// <inheritdoc />
        /// <remarks>
        /// Before drawing the appearance of the actor is updated, if needed.
        /// Draws the actor and its bounding rectangle.
        /// </remarks>
        public override void Draw(SceneBatch sceneBatch)
        {
            if (updateAppearance)
            {
                UpdateAppearance();
                updateAppearance = false;
            }

            if (Actor.DrawableAssetVisible && sceneDrawable != null)
            {
                sceneDrawable.Draw(sceneBatch, Position, Angle, ScaleFactor, Actor.Layer != null ? Actor.Layer.GraphicsEffect : SceneElementEffect.None);
            }

            sceneBatch.Draw(Rectangle, ColorSettings.ForBounds(SceneSelect), 0);
        }

        /// <inheritdoc />
        public override void DrawShapes(SceneBatch sceneBatch)
        {
            if (containsShapes && Actor.Physics.Type != PhysicsComponent.BodyPhysicsType.None)
            {
                Color color = ColorSettings.ForShape(SceneSelect);

                foreach (Shape shape in Shapes)
                {
                    sceneBatch.Draw(shape, color, 0f);
                }
            }
        }

        /// <summary>
        /// Updates the appearance of the actor:
        /// Updates the position, angle and scale factor of the actor.
        /// Updates the collision shapes which are shown at the scene.
        /// </summary>
        private void UpdateAppearance()
        {
            _position = Actor.Position;
            _angle = Actor.Angle;
            _scaleFactor = Actor.ScaleFactor;

            UpdateShapes();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Actor.AppearanceChanged -= new EventHandler(Actor_AppearanceChanged);
                Actor.DrawableAssetChanged -= new ValueChangedHandler<DrawableAsset>(Actor_DrawableAssetChanged);
                Actor.Children.ListChanged -= new ObservableList<Actor>.ListChangedEventHandler(Children_ListChanged);

                if (Actor.DrawableAsset != null)
                {
                    Actor.DrawableAsset.DrawableAssetChanged -= new EventHandler(DrawableAsset_DrawableAssetChanged);
                }

                _actor = null;
                Parent = null;
            }

            base.Dispose(disposing);
        }
    }
}
