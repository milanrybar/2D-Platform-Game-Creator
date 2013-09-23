/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.Xna;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Enables a group of sprites or primitives to be drawn using the same settings.
    /// Used at <see cref="SceneScreen"/> for drawing the scene.
    /// </summary>
    class SceneBatch : SpriteBatch
    {
        /// <summary>
        /// Gets the projection matrix for the drawing.
        /// </summary>
        public Matrix Projection
        {
            get { return _projection; }
        }
        private Matrix _projection;

        /// <summary>
        /// Gets the view matrix for the drawing.
        /// </summary>
        public Matrix View
        {
            get { return _view; }
        }
        private Matrix _view;

        /// <summary>
        /// Gets the world matrix for the drawing.
        /// </summary>
        public Matrix World
        {
            get { return _world; }
        }
        private Matrix _world;

        /// <summary>
        /// Gets the scale factor for the drawing. Corresponding to <see cref="SceneScreen.ScaleFactor"/>.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
        }
        private float _scale;

        /// <summary>
        /// Gets the invers scale factor defined by <see cref="Scale"/>. Corresponding to <see cref="SceneScreen.ScaleInversFactor"/>.
        /// </summary>
        public float InversScale
        {
            get { return _inversScale; }
        }
        private float _inversScale;

        /// <summary>
        /// Size of the scene viewport at the <see cref="SceneScreen"/>.
        /// </summary>
        public Vector2 SceneSize;

        /// <summary>
        /// Position at the scene. Corresponding to <see cref="SceneScreen.Position"/>.
        /// </summary>
        public Vector2 ScenePosition;

        /// <summary>
        /// Gets the time passed since the last call to Draw at the <see cref="SceneScreen"/>.
        /// </summary>
        public GameTime GameTime
        {
            get { return _gameTime; }
        }
        private GameTime _gameTime;

        /// <summary>
        /// Basic effect for drawing the primitives.
        /// </summary>
        private BasicEffect basicEffect;

        // storage for the vertices of lines
        private VertexPositionColor[] lineVertices;
        // number of the vertices in the storage
        private int lineVerticesCount;

        // indicates whether we are drawing in the 2D mode for drawing sprites
        private bool drawing2D;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public SceneBatch(GraphicsDevice graphicsDevice, int bufferSize = 50)
            : base(graphicsDevice)
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;

            bufferSize = bufferSize < 10 ? 10 : bufferSize;
            lineVertices = new VertexPositionColor[bufferSize - bufferSize % 2];
        }

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                if (basicEffect != null) basicEffect.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Extends the buffer for drawing.
        /// </summary>
        private void ExtendBuffer()
        {
            VertexPositionColor[] temp = lineVertices;
            lineVertices = new VertexPositionColor[lineVertices.Length * 2];

            for (int i = 0; i < temp.Length; ++i)
            {
                lineVertices[i] = temp[i];
            }
        }

        /// <summary>
        /// Begins the operation with the specified settings.
        /// </summary>
        /// <param name="projection">The projection matrix to use.</param>
        /// <param name="view">The view matrix to use.</param>
        /// <param name="world">The world matrix to use.</param>
        /// <param name="scale">The scale factor to use.</param>
        /// <param name="inversScale">The invers scale factor to use.</param>
        /// <param name="gameTime">The time passed since the last call to Draw at the <see cref="SceneScreen"/>.</param>
        public void Begin(ref Matrix projection, ref Matrix view, ref Matrix world, float scale, float inversScale, ref GameTime gameTime)
        {
            _projection = projection;
            _view = view;
            _world = world;
            _scale = scale;
            _inversScale = inversScale;

            basicEffect.Projection = projection;
            basicEffect.View = view;
            basicEffect.World = world;

            _gameTime = gameTime;

            drawing2D = false;
        }

        /// <summary>
        /// End is called once all the sprites or primitives have been added.
        /// It will call Flush to actually submit the draw call to the graphics card, if any.
        /// </summary>
        public new void End()
        {
            if (lineVerticesCount != 0)
            {
                DrawLines();
            }

            if (drawing2D)
            {
                base.End();
            }
        }

        /// <summary>
        /// Begins the operation for drawing the group of sprites using the same settings.
        /// </summary>
        public void Apply2D()
        {
            if (!drawing2D)
            {
                End();
                base.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, _world);
                drawing2D = true;
            }
        }

        /// <summary>
        /// Begins the operation for drawing the group of primitives using the same settings.
        /// </summary>
        public void Apply3D()
        {
            if (drawing2D)
            {
                base.End();
                drawing2D = false;
            }
        }

        /// <summary>
        /// Draws the specified shape by the specified color.
        /// </summary>
        /// <param name="verticesShape">The shape to draw.</param>
        /// <param name="color">The color of the shape.</param>
        /// <param name="z">The z coordinate of the shape.</param>
        public void Draw(ShapeBasedOnVertices verticesShape, Color color, float z)
        {
            if (verticesShape.Vertices.Count < 2) return;
            while (verticesShape.Vertices.Count > lineVertices.Length) ExtendBuffer();

            if (lineVerticesCount >= 2) DrawLines();

            for (int i = 0; i < verticesShape.Vertices.Count; ++i)
            {
                lineVertices[i].Position = new Vector3(verticesShape.Vertices[i], z);
                lineVertices[i].Color = color;
            }
            if (verticesShape.Type == Shape.ShapeType.Polygon) lineVertices[verticesShape.Vertices.Count] = lineVertices[0];
            lineVerticesCount = verticesShape.Type == Shape.ShapeType.Polygon ? verticesShape.Vertices.Count : verticesShape.Vertices.Count - 1;

            ApplyBasicEffect();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, lineVertices, 0, lineVerticesCount);

            lineVerticesCount = 0;
        }

        /// <summary>
        /// Draws the specified circle by the specified color.
        /// </summary>
        /// <param name="circle">The circle to draw.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="z">The z coordinate of the circle.</param>
        public void Draw(Circle circle, Color color, float z)
        {
            Apply3D();
            RenderCircle.DrawCircle(circle.Origin, circle.Radius, color, this);
        }

        /// <summary>
        /// Draws the specified shape by the specified color.
        /// </summary>
        /// <param name="shape">The shape to draw.</param>
        /// <param name="color">The color of the shape.</param>
        /// <param name="z">The z coordinate of the shape.</param>
        public void Draw(Shape shape, Color color, float z)
        {
            if (shape.Type == Shape.ShapeType.Circle) Draw((Circle)shape, color, z);
            else Draw((ShapeBasedOnVertices)shape, color, z);
        }

        /// <summary>
        /// Draws the specified rectangle by the specified color.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color of the rectangle.</param>
        /// <param name="z">The z coordinate of the rectangle.</param>
        public void Draw(FarseerPhysics.Collision.AABB rectangle, Color color, float z)
        {
            while (5 > lineVertices.Length) ExtendBuffer();

            if (lineVerticesCount >= 2) DrawLines();

            lineVertices[0].Position = new Vector3(rectangle.LowerBound, z);
            lineVertices[1].Position = new Vector3(rectangle.UpperBound.X, rectangle.LowerBound.Y, z);
            lineVertices[2].Position = new Vector3(rectangle.UpperBound, z);
            lineVertices[3].Position = new Vector3(rectangle.LowerBound.X, rectangle.UpperBound.Y, z);
            lineVertices[4].Position = lineVertices[0].Position;

            lineVertices[0].Color = lineVertices[1].Color = lineVertices[2].Color = lineVertices[3].Color = lineVertices[4].Color = color;

            ApplyBasicEffect();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, lineVertices, 0, 4);

            lineVerticesCount = 0;
        }

        /// <summary>
        /// Applies the basic effect that is used for drawing primitives.
        /// </summary>
        public void ApplyBasicEffect()
        {
            Apply3D();
            basicEffect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Draws the line by two specified points.
        /// </summary>
        /// <param name="startPoint">The start point of the line.</param>
        /// <param name="endPoint">The end point of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="z">The z coordinate of the line.</param>
        public void DrawLine(Vector2 startPoint, Vector2 endPoint, Color color, float z)
        {
            while (lineVerticesCount + 2 >= lineVertices.Length) ExtendBuffer();

            lineVertices[lineVerticesCount] = new VertexPositionColor(new Vector3(startPoint, z), color);
            lineVertices[lineVerticesCount + 1] = new VertexPositionColor(new Vector3(endPoint, z), color);

            lineVerticesCount += 2;
        }

        /// <summary>
        /// Draws the line by two specified points.
        /// </summary>
        /// <param name="startPoint">The start point of the line.</param>
        /// <param name="endPoint">The end point of the line.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawLine(Vector3 startPoint, Vector3 endPoint, Color color)
        {
            while (lineVerticesCount + 2 >= lineVertices.Length) ExtendBuffer();

            lineVertices[lineVerticesCount] = new VertexPositionColor(startPoint, color);
            lineVertices[lineVerticesCount + 1] = new VertexPositionColor(endPoint, color);

            lineVerticesCount += 2;
        }

        /// <summary>
        /// Submits the draw call to the graphics card for the stored lines.
        /// </summary>
        private void DrawLines()
        {
            if (lineVerticesCount >= 2)
            {
                ApplyBasicEffect();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, lineVertices, 0, lineVerticesCount / 2);
                lineVerticesCount = 0;
            }
        }

        /// <summary>
        /// Draws the specified texture with the specified settings.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position of the texture.</param>
        /// <param name="rotation">The rotation of the texture.</param>
        /// <param name="scale">The scale of the texture.</param>
        /// <param name="origin">The origin of the texture.</param>
        /// <param name="effect">Effect to apply to the texture.</param>
        public void DrawTexture(Texture2D texture, ref Vector2 position, float rotation, ref Vector2 scale, Vector2 origin, SceneElementEffect effect)
        {
            Apply2D();

            if (effect == SceneElementEffect.None)
            {
                Draw(texture, position, null, Microsoft.Xna.Framework.Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
            }
            else
            {
                Vector2 textureSize = new Vector2(texture.Width * scale.X, texture.Height * scale.Y);

                if (rotation != 0f)
                {
                    Vector2[] rectangle = { new Vector2(), new Vector2(0, textureSize.Y), new Vector2(textureSize.X, 0), textureSize };
                    Matrix rotateTransform = Matrix.CreateRotationZ(rotation);
                    Vector2.Transform(rectangle, ref rotateTransform, rectangle);

                    Vector2 lowerBound = Vector2.Min(rectangle[0], Vector2.Min(rectangle[1], Vector2.Min(rectangle[2], rectangle[3])));
                    Vector2 upperBound = Vector2.Max(rectangle[0], Vector2.Max(rectangle[1], Vector2.Max(rectangle[2], rectangle[3])));

                    textureSize = new Vector2(Math.Abs(lowerBound.X - upperBound.X), Math.Abs(lowerBound.Y - upperBound.Y));
                }

                Vector2 start = new Vector2(ScenePosition.X - textureSize.X + ((position.X - ScenePosition.X) % textureSize.X), ScenePosition.Y - textureSize.Y + ((position.Y - ScenePosition.Y) % textureSize.Y));
                Vector2 end = new Vector2(ScenePosition.X + SceneSize.X + textureSize.X, ScenePosition.Y + SceneSize.Y + textureSize.Y);

                // Fill
                if ((effect & SceneElementEffect.Fill) != 0)
                {
                    for (float x = start.X; x < end.X; x += textureSize.X)
                    {
                        for (float y = start.Y; y < end.Y; y += textureSize.Y)
                        {
                            Draw(texture, new Vector2(x, y), null, Microsoft.Xna.Framework.Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
                        }
                    }
                }
                else
                {
                    // RepeatHorizontally
                    if ((effect & SceneElementEffect.RepeatHorizontally) != 0)
                    {
                        if (position.X - textureSize.X > start.X || position.X + textureSize.X < end.X)
                        {
                            for (float x = start.X; x < end.X; x += textureSize.X)
                            {
                                Draw(texture, new Vector2(x, position.Y), null, Microsoft.Xna.Framework.Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
                            }
                        }
                    }

                    // RepeatVertically
                    if ((effect & SceneElementEffect.RepeatVertically) != 0)
                    {
                        if (position.Y - textureSize.Y > start.Y || position.Y + textureSize.Y < end.Y)
                        {
                            for (float y = start.Y; y < end.Y; y += textureSize.Y)
                            {
                                Draw(texture, new Vector2(position.X, y), null, Microsoft.Xna.Framework.Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
            }
        }
    }
}
