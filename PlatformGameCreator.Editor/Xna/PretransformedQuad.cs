/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PlatformGameCreator.Editor.Scenes;

namespace PlatformGameCreator.Editor.Xna
{
    /// <summary>
    /// Draws circle on the graphics card by pixel shader.
    /// </summary>
    class RenderCircle
    {
        private static GraphicsDevice graphicsDevice;
        private static VertexBuffer vertexBuffer;

        private static short[] indices = { 0, 1, 2, 2, 1, 3 };

        private static QuadVertex[] vertices = new QuadVertex[]{
            new QuadVertex( new Vector3(-1, -1, 0) , new Vector2(0, 0)), // top left
            new QuadVertex( new Vector3( 1, -1, 0), new Vector2(1, 0)), // top right
            new QuadVertex( new Vector3(-1, 1, 0), new Vector2(0, 1) ), // bottom left
            new QuadVertex( new Vector3( 1, 1, 0), new Vector2(1, 1)), // bottom right
        };

        private static Effect circleEffect;

        /// <summary>
        /// Initializes settings for drawing the circles.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to used.</param>
        /// <param name="contentManager">The content manager to load shader.</param>
        public static void Init(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            circleEffect = contentManager.Load<Effect>("PixelCircle");
            RenderCircle.graphicsDevice = graphicsDevice;

            vertexBuffer = new VertexBuffer(graphicsDevice, QuadVertex.VertexDeclaration, 2, BufferUsage.None);
        }

        /// <summary>
        /// Draws the circle by the specified center, radius and color.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="sceneBatch">The scene batch for drawing the circle.</param>
        public static void DrawCircle(Vector2 center, float radius, Color color, SceneBatch sceneBatch)
        {
            if (graphicsDevice == null) throw new Exception("No graphics device");

            // set effect parameters
            circleEffect.Parameters["Thickness"].SetValue(2.5f / radius * sceneBatch.InversScale);
            circleEffect.Parameters["BaseColor"].SetValue(color.ToVector4());

            circleEffect.Parameters["Center"].SetValue(new Vector3(center.X, center.Y, 0f));
            circleEffect.Parameters["Radius"].SetValue(radius);

            circleEffect.Parameters["xProjection"].SetValue(sceneBatch.Projection);
            circleEffect.Parameters["xView"].SetValue(sceneBatch.View);
            circleEffect.Parameters["xWorld"].SetValue(sceneBatch.World);

            // set blend state
            BlendState temp = graphicsDevice.BlendState = BlendState.NonPremultiplied;

            // draw circle
            circleEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.DrawUserIndexedPrimitives<QuadVertex>(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);

            // set previous blend state
            graphicsDevice.BlendState = temp;
        }

        private struct QuadVertex : IVertexType
        {
            public Vector3 Position;
            public Vector2 Tex0;

            public QuadVertex(Vector3 p, Vector2 t)
            {
                this.Position = p;
                this.Tex0 = t;
            }

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
              new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
              new VertexElement(3 * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            );

            public static int Stride = 5 * 4;

            VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
        }
    }
}