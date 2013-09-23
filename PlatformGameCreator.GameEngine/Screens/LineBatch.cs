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
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Screens
{
    /// <summary>
    /// Enables a group of primitives to be drawn using the same settings.
    /// </summary>
    public class LineBatch : IDisposable
    {
        private const int DefaultBufferSize = 1024;

        // a basic effect which contains the shaders that we will use to draw our primitives.
        private BasicEffect _basicEffect;

        // the device that we will issue draw calls to.
        private GraphicsDevice _device;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        private bool _hasBegun;

        // indicates whether the instance is disposed
        private bool _isDisposed;

        // storage for the vertices of lines
        private VertexPositionColor[] _lineVertices;
        // number of the vertices in the storage
        private int _lineVertsCount;

        // storage for circles
        private List<Vector2> circleCenter = new List<Vector2>();
        private List<float> circleRadius = new List<float>();
        private List<Color> circleColor = new List<Color>();

        // camera to use for drawing
        private Camera camera;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use.</param>
        public LineBatch(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, DefaultBufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice"/> is <c>null</c>.</exception>
        public LineBatch(GraphicsDevice graphicsDevice, int bufferSize)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("GraphicsDevice cannot be null.");
            _device = graphicsDevice;

            // creates 
            _lineVertices = new VertexPositionColor[bufferSize - bufferSize % 2];

            // set up a new basic effect, and enable vertex colors.
            _basicEffect = new BasicEffect(graphicsDevice);
            _basicEffect.VertexColorEnabled = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Releases unmanaged  resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                if (_basicEffect != null) _basicEffect.Dispose();

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Begins the operation.
        /// </summary>
        /// <param name="camera">The camera to be used for drawing.</param>
        public void Begin(Camera camera)
        {
            if (_hasBegun)
            {
                throw new InvalidOperationException("End must be called before Begin can be called again.");
            }

            this.camera = camera;

            _device.SamplerStates[0] = SamplerState.AnisotropicClamp;

            // tell our basic effect to begin.
            _basicEffect.Projection = camera.Projection;
            _basicEffect.View = camera.World;
            _basicEffect.CurrentTechnique.Passes[0].Apply();

            // flip the error checking boolean. It's now ok to call DrawLineShape, Flush and End.
            _hasBegun = true;
        }

        /// <summary>
        /// Draws the line by two specified vertices of the specified color.
        /// </summary>
        /// <param name="v1">The first vertex.</param>
        /// <param name="v2">The second vertex.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawLine(Vector2 v1, Vector2 v2, Color color)
        {
            if (!_hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before DrawLineShape can be called.");
            }
            if (_lineVertsCount + 2 >= _lineVertices.Length)
            {
                Flush();
            }

            _lineVertices[_lineVertsCount].Position = new Vector3(v1, 0f);
            _lineVertices[_lineVertsCount + 1].Position = new Vector3(v2, 0f);
            _lineVertices[_lineVertsCount].Color = _lineVertices[_lineVertsCount + 1].Color = color;
            _lineVertsCount += 2;
        }

        /// <summary>
        /// Draws the circle by the spefied center, radius and color.
        /// </summary>
        /// <param name="center">The center of the circle to draw.</param>
        /// <param name="radius">The radius of the circle to draw.</param>
        /// <param name="color">The color of the circle.</param>
        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (!_hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before DrawCircle can be called.");
            }

            circleCenter.Add(center);
            circleRadius.Add(radius);
            circleColor.Add(color);
        }

        /// <summary>
        /// End is called once all the primitives have been added.
        /// It will call Flush to actually submit the draw call to the graphics card, and then tell the basic effect to end.
        /// </summary>
        public void End()
        {
            if (!_hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before End can be called.");
            }

            // draw whatever the user wanted us to draw
            Flush();

            // draw circles
            for (int i = 0; i < circleCenter.Count; ++i)
            {
                RenderCircle.DrawCircle(circleCenter[i], circleRadius[i], circleColor[i], camera);
            }

            circleCenter.Clear();
            circleRadius.Clear();
            circleColor.Clear();

            _hasBegun = false;
        }

        /// <summary>
        /// Submits the draw call to the graphics card for the stored lines.
        /// </summary>
        private void Flush()
        {
            if (!_hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }
            if (_lineVertsCount >= 2)
            {
                int primitiveCount = _lineVertsCount / 2;
                // submit the draw call to the graphics card
                _device.DrawUserPrimitives(PrimitiveType.LineList, _lineVertices, 0, primitiveCount);
                _lineVertsCount -= primitiveCount * 2;
            }
        }
    }

    /// <summary>
    /// Draws circle on the graphics card by pixel shader.
    /// </summary>
    static class RenderCircle
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
        public static void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            // load shader
            circleEffect = contentManager.Load<Effect>("PixelCircle");
            RenderCircle.graphicsDevice = graphicsDevice;

            // prepare vertex buffer
            vertexBuffer = new VertexBuffer(graphicsDevice, QuadVertex.VertexDeclaration, 2, BufferUsage.None);
        }

        /// <summary>
        /// Draws the circle by the specified center, radius and color.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="camera">The camera to use for drawing.</param>
        /// <exception cref="Exception">No graphics device = not initialized.</exception>
        public static void DrawCircle(Vector2 center, float radius, Color color, Camera camera)
        {
            if (graphicsDevice == null) throw new Exception("No graphics device");

            // set effect parameters
            circleEffect.Parameters["Thickness"].SetValue(2.5f * camera.InversScale / radius);
            circleEffect.Parameters["BaseColor"].SetValue(color.ToVector4());

            circleEffect.Parameters["Center"].SetValue(new Vector3(center.X, center.Y, 0f));
            circleEffect.Parameters["Radius"].SetValue(radius);

            circleEffect.Parameters["xProjection"].SetValue(camera.Projection);
            circleEffect.Parameters["xView"].SetValue(camera.World);
            circleEffect.Parameters["xWorld"].SetValue(Matrix.Identity);

            // set blend state
            BlendState originalBlendState = graphicsDevice.BlendState = BlendState.NonPremultiplied;

            // draw circle
            circleEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.DrawUserIndexedPrimitives<QuadVertex>(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);

            // restore original blend state
            graphicsDevice.BlendState = originalBlendState;
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
