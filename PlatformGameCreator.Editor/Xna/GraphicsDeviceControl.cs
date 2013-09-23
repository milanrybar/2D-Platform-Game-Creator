/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 * 
 * -----------------------------------------------------------------------------
 * 
 * GraphicsDeviceControl based on http://create.msdn.com/en-US/education/catalog/sample/winforms_series_2
 * Loop tick based on http://blogs.msdn.com/b/shawnhar/archive/2010/12/06/when-winforms-met-game-loop.aspx
 */

//-----------------------------------------------------------------------------
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
//-----------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGameCreator.Editor.Xna
{
    // System.Drawing and the XNA Framework both define Color and Rectangle
    // types. To avoid conflicts, we specify exactly which ones to use.
    using Color = System.Drawing.Color;
    using Rectangle = Microsoft.Xna.Framework.Rectangle;


    /// <summary>
    /// Custom control uses the XNA Framework GraphicsDevice to render onto
    /// a Windows Form. Derived classes can override the Initialize and Draw
    /// methods to add their own drawing code.
    /// </summary>
    abstract class GraphicsDeviceControl : Control
    {
        // However many GraphicsDeviceControl instances you have, they all share
        // the same underlying GraphicsDevice, managed by this helper service.
        private GraphicsDeviceService graphicsDeviceService;

        /// <summary>
        /// Content manager for this control.
        /// Every instance of GraphicsDeviceControl have different one.
        /// </summary>
        protected ContentManager Content
        {
            get { return _contentManager; }
        }
        private ContentManager _contentManager;

        // xna global variables
        private XnaGlobals xnaGlobals;

        /// <summary>
        /// Gets a GraphicsDevice that can be used to draw onto this control.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected GraphicsDevice GraphicsDevice
        {
            get { return graphicsDeviceService.GraphicsDevice; }
        }

        /// <summary>
        /// Gets an IServiceProvider containing our IGraphicsDeviceService.
        /// This can be used with components such as the ContentManager,
        /// which use this service to look up the GraphicsDevice.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ServiceContainer Services
        {
            get { return services; }
        }
        private ServiceContainer services = new ServiceContainer();


        #region Initialization


        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void OnCreateControl()
        {
            // Don't initialize the graphics device if we are running in the designer.
            if (!DesignMode)
            {
                graphicsDeviceService = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height);

                // Register the service, so components like ContentManager can find it.
                services.AddService<IGraphicsDeviceService>(graphicsDeviceService);

                // create xna global variables
                xnaGlobals = new XnaGlobals(services);

                // create ContentManager for this control
                _contentManager = new ContentManager(services, "Content");

                // Give derived classes a chance to initialize themselves.
                Initialize();
            }

            base.OnCreateControl();
        }


        /// <summary>
        /// Disposes the control.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (graphicsDeviceService != null)
            {
                graphicsDeviceService.Release(disposing);
                graphicsDeviceService = null;
            }

            base.Dispose(disposing);
        }


        #endregion

        #region Paint


        /// <summary>
        /// Redraws the control in response to a WinForms paint message.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            string beginDrawError = BeginDraw();

            if (string.IsNullOrEmpty(beginDrawError))
            {
                // Draw the control using the GraphicsDevice.
                Draw(gameTime);
                EndDraw();
            }
            else if (e != null)
            {
                // If BeginDraw failed, show an error message using System.Drawing.
                PaintUsingSystemDrawing(e.Graphics, beginDrawError);
            }
        }


        /// <summary>
        /// Attempts to begin drawing the control. Returns an error message string
        /// if this was not possible, which can happen if the graphics device is
        /// lost, or if we are running inside the Form designer.
        /// </summary>
        string BeginDraw()
        {
            // If we have no graphics device, we must be running in the designer.
            if (graphicsDeviceService == null)
            {
                return Text + "\n\n" + GetType();
            }

            // Make sure the graphics device is big enough, and is not lost.
            string deviceResetError = HandleDeviceReset();

            if (!string.IsNullOrEmpty(deviceResetError))
            {
                return deviceResetError;
            }

            // Many GraphicsDeviceControl instances can be sharing the same
            // GraphicsDevice. The device backbuffer will be resized to fit the
            // largest of these controls. But what if we are currently drawing
            // a smaller control? To avoid unwanted stretching, we set the
            // viewport to only use the top left portion of the full backbuffer.
            Viewport viewport = new Viewport();

            viewport.X = 0;
            viewport.Y = 0;

            viewport.Width = ClientSize.Width;
            viewport.Height = ClientSize.Height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            GraphicsDevice.Viewport = viewport;

            return null;
        }


        /// <summary>
        /// Ends drawing the control. This is called after derived classes
        /// have finished their Draw method, and is responsible for presenting
        /// the finished image onto the screen, using the appropriate WinForms
        /// control handle to make sure it shows up in the right place.
        /// </summary>
        void EndDraw()
        {
            try
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

                GraphicsDevice.Present(sourceRectangle, null, this.Handle);
            }
            catch
            {
                // Present might throw if the device became lost while we were
                // drawing. The lost device will be handled by the next BeginDraw,
                // so we just swallow the exception.
            }
        }


        /// <summary>
        /// Helper used by BeginDraw. This checks the graphics device status,
        /// making sure it is big enough for drawing the current control, and
        /// that the device is not lost. Returns an error string if the device
        /// could not be reset.
        /// </summary>
        string HandleDeviceReset()
        {
            bool deviceNeedsReset = false;

            switch (GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    // If the graphics device is lost, we cannot use it at all.
                    return "Graphics device lost";

                case GraphicsDeviceStatus.NotReset:
                    // If device is in the not-reset state, we should try to reset it.
                    deviceNeedsReset = true;
                    break;

                default:
                    // If the device state is ok, check whether it is big enough.
                    PresentationParameters pp = GraphicsDevice.PresentationParameters;

                    deviceNeedsReset = (ClientSize.Width > pp.BackBufferWidth) || (ClientSize.Height > pp.BackBufferHeight);
                    break;
            }

            // Do we need to reset the device?
            if (deviceNeedsReset)
            {
                try
                {
                    graphicsDeviceService.ResetDevice(ClientSize.Width, ClientSize.Height);
                }
                catch (Exception e)
                {
                    return "Graphics device reset failed\n\n" + e;
                }
            }

            return null;
        }


        /// <summary>
        /// If we do not have a valid graphics device (for instance if the device
        /// is lost, or if we are running inside the Form designer), we must use
        /// regular System.Drawing method to display a status message.
        /// </summary>
        protected virtual void PaintUsingSystemDrawing(Graphics graphics, string text)
        {
            graphics.Clear(Color.CornflowerBlue);

            using (Brush brush = new SolidBrush(Color.Black))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    graphics.DrawString(text, Font, brush, ClientRectangle, format);
                }
            }
        }


        /// <summary>
        /// Ignores WinForms paint-background messages. The default implementation
        /// would clear the control to the current background color, causing
        /// flickering when our OnPaint implementation then immediately draws some
        /// other color over the top using the XNA Framework GraphicsDevice.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }


        #endregion

        #region Game Loop

        /*
         * Based on http://blogs.msdn.com/b/shawnhar/archive/2010/12/06/when-winforms-met-game-loop.aspx
         */

        static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Message
            {
                public IntPtr hWnd;
                public uint Msg;
                public IntPtr wParam;
                public IntPtr lParam;
                public uint Time;
                public System.Drawing.Point Point;
            }

            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PeekMessage(out Message message, IntPtr hWnd, uint filterMin, uint filterMax, uint flags);
        }

        private Stopwatch stopWatch = Stopwatch.StartNew();
        private TimeSpan accumulatedTime = TimeSpan.Zero;
        private TimeSpan oneSecond = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Total runnning time from the last tick.
        /// </summary>
        protected TimeSpan lastTime = TimeSpan.Zero;

        private GameTime gameTime;

        /// <summary>
        /// Gets the FPS.
        /// </summary>
        public int Fps
        {
            get { return fps; }
        }
        private int fps = 0;
        private int frameCounter = 0;

        /// <summary>
        /// Called when the application is idle.
        /// Updates and draws the control until the application needs to process anything.
        /// </summary>
        private void TickWhileIdle(object sender, EventArgs e)
        {
            NativeMethods.Message message;

            TimeSpan currentTime;
            TimeSpan elapsedTime;

            while (!NativeMethods.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                // update time
                currentTime = stopWatch.Elapsed;
                elapsedTime = currentTime - lastTime;
                lastTime = currentTime;

                // create game time
                gameTime = new GameTime(currentTime, elapsedTime, false);

                // update fps counter
                accumulatedTime += elapsedTime;
                if (accumulatedTime >= oneSecond)
                {
                    fps = frameCounter;
                    frameCounter = 0;
                    accumulatedTime -= oneSecond;
                }
                frameCounter++;

                // update control
                Update(gameTime);

                // draw control 
                //Invalidate();
                OnPaint(null);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the loop is activated.
        /// </summary>
        public bool LoopActive
        {
            get { return _loopActive; }
            set
            {
                if (_loopActive != value)
                {
                    if (value) ActiveLoop();
                    else DeactiveLoop();
                }
                _loopActive = value;
            }
        }
        private bool _loopActive = false;

        /// <summary>
        /// Active game loop.
        /// </summary>
        private void ActiveLoop()
        {
            // define loop when application is idle
            Application.Idle += TickWhileIdle;
        }

        /// <summary>
        /// Deactive game loop.
        /// </summary>
        private void DeactiveLoop()
        {
            // define loop when application is idle
            Application.Idle -= TickWhileIdle;
        }

        #endregion

        #region Abstract Methods


        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        protected abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Derived classes override this to update themselves.
        /// </summary>
        protected abstract void Update(GameTime gameTime);


        #endregion

        /// <summary>
        /// Provides using of XNA <see cref="ContentManager"/>.
        /// </summary>
        private class XnaGlobals : XnaFramework
        {
            private ContentManager contentManager = null;
            private ServiceContainer services;

            /// <summary>
            /// Initializes a new instance of the <see cref="XnaGlobals"/> class.
            /// </summary>
            /// <param name="serviceContainer">The service container for the content manager.</param>
            public XnaGlobals(ServiceContainer serviceContainer)
            {
                services = serviceContainer;

                Singleton = this;
            }

            /// <inheritdoc />
            protected override ContentManager GetContentManager()
            {
                if (contentManager == null)
                {
                    contentManager = new ContentManager(services);
                }

                return contentManager;
            }
        }
    }
}
