/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using PlatformGameCreator.GameEngine.Scenes;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PlatformGameCreator.GameEngine.Screens
{
    /// <summary>
    /// Debug screen for the scene.
    /// </summary>
    /// <remarks>
    /// Is used when running the game in the editor.
    /// Has ability to zoom the scene and to show the shapes of the scene nodes (collision shapes of actor, path).
    /// </remarks>
    public class DebugScreen : SceneScreen
    {
        /// <summary>
        /// Indicates whether the <see cref="DebugScreen"/> draws shapes of the scene nodes. Default value is <c>true</c>.
        /// </summary>
        public bool DrawShapes = true;

        /// <summary>
        /// <see cref="LineBatch"/> for drawing shapes of the scene nodes.
        /// </summary>
        private LineBatch lineBatch;

        /// <inheritdoc />
        /// <summary>
        /// Zoom the scene when the <see cref="InputManager.ScrollWheelValue"/> changes.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // zoom screen
            Camera.Scale += InputManager.ScrollWheelValue / 3000f;

            if (InputManager.IsKeyPressed(Keys.F1)) DrawShapes = !DrawShapes;

            base.Update(gameTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// If <see cref="DrawShapes"/> is set to <c>true</c> then draws shapes of the scene nodes.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // draw scene node shapes
            if (DrawShapes)
            {
                if (lineBatch == null)
                {
                    lineBatch = ScreenManager.LineBatch;
                }

                lineBatch.Begin(Camera);

                DrawActorShapes();
                DrawPaths();

                lineBatch.End();
            }
        }

        /// <summary>
        /// Draws collision shapes of all actors at the scene.
        /// </summary>
        public void DrawActorShapes()
        {
            Actor actor;
            foreach (SceneNode sceneNode in Nodes)
            {
                actor = sceneNode as Actor;
                if (actor != null) DrawShape(actor);
            }
        }

        /// <summary>
        /// Draws all paths at the scene.
        /// </summary>
        public void DrawPaths()
        {
            foreach (PlatformGameCreator.GameEngine.Scenes.Path path in Paths)
            {
                DrawShape(path);
            }
        }

        /// <summary>
        /// Draws collision shapes of the specified actor.
        /// </summary>
        /// <param name="actor">Actor to draw collision shapes of.</param>
        private void DrawShape(Actor actor)
        {
            if (actor == null || actor.Body == null || actor.Body.FixtureList == null) return;

            Vector2 positionSim = ConvertUnits.ToDisplayUnits(actor.Position);

            foreach (Fixture fixture in actor.Body.FixtureList)
            {
                Color color = ((Actor)fixture.UserData).InCollision() ? Color.Red : Color.Yellow;

                // polygon
                if (fixture.Shape is PolygonShape)
                {
                    PolygonShape polygon = fixture.Shape as PolygonShape;
                    Vertices vertices = new Vertices(polygon.Vertices);
                    vertices.Rotate(fixture.Body.Rotation);

                    for (int i = 1; i < vertices.Count; ++i)
                    {
                        ScreenManager.LineBatch.DrawLine(ConvertUnits.ToDisplayUnits(vertices[i - 1]) + positionSim, ConvertUnits.ToDisplayUnits(vertices[i]) + positionSim, color);
                    }
                    lineBatch.DrawLine(ConvertUnits.ToDisplayUnits(vertices[vertices.Count - 1]) + positionSim, ConvertUnits.ToDisplayUnits(vertices[0]) + positionSim, color);
                }
                // circle
                else if (fixture.Shape is CircleShape)
                {
                    CircleShape circle = fixture.Shape as CircleShape;
                    Vertices vertices = new Vertices(new Vector2[] { circle.Position });
                    vertices.Rotate(fixture.Body.Rotation);

                    lineBatch.DrawCircle(ConvertUnits.ToDisplayUnits(vertices[0]) + positionSim, ConvertUnits.ToDisplayUnits(circle.Radius), color);
                }
                // edge
                else if (fixture.Shape is EdgeShape)
                {
                    EdgeShape edge = fixture.Shape as EdgeShape;
                    Vertices vertices = new Vertices(new Vector2[] { edge.Vertex1, edge.Vertex2 });
                    vertices.Rotate(fixture.Body.Rotation);

                    lineBatch.DrawLine(ConvertUnits.ToDisplayUnits(vertices[0]) + positionSim, ConvertUnits.ToDisplayUnits(vertices[1]) + positionSim, color);
                }
            }
        }

        /// <summary>
        /// Draws the specified paths.
        /// </summary>
        /// <param name="path">Path to draw.</param>
        private void DrawShape(PlatformGameCreator.GameEngine.Scenes.Path path)
        {
            for (int i = 1; i < path.Vertices.Length; ++i)
            {
                lineBatch.DrawLine(ConvertUnits.ToDisplayUnits(path.Vertices[i - 1]), ConvertUnits.ToDisplayUnits(path.Vertices[i]), Color.Blue);
            }
            if (path.Loop) lineBatch.DrawLine(ConvertUnits.ToDisplayUnits(path.Vertices[path.Vertices.Length - 1]), ConvertUnits.ToDisplayUnits(path.Vertices[0]), Color.Blue);
        }

        /// <summary>
        /// Indicates whether the game is already exiting.
        /// </summary>
        private bool exiting = false;

        /// <inheritdoc />
        public override void Exit()
        {
            // solve problem with exiting XNA game in editor:
            // if we normally call ScreenManager.Game.Exit(); it quit the whole application
            // also sending message WM_CLOSE or SC_CLOSE to game window quit the whole application
            // simulation of pressing ALT + F4 solve the problem

            const byte VK_MENU = 0x12; // ALT key
            const byte VK_F4 = 0x73; // F4 key

            if (!exiting)
            {
                const int SW_SHOW = 5; // Activates the window and displays it in its current size and position.

                // try to active the game window, because we will send ALT+F4 to active window
                ShowWindow(ScreenManager.Game.Window.Handle, SW_SHOW);
                if (SetForegroundWindow(ScreenManager.Game.Window.Handle))
                {
                    SetFocus(ScreenManager.Game.Window.Handle);

                    // send ALT + F4
                    SimulateKeyDown(VK_MENU);
                    SimulateKeyDown(VK_F4);
                    SimulateKeyUp(VK_F4);
                    SimulateKeyUp(VK_MENU);
                }
                else
                {
                    Debug.Assert(true);
                }

                exiting = true;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBOARD_INPUT
        {
            public uint type;
            public ushort vk;
            public ushort scanCode;
            public uint flags;
            public uint time;
            public uint extrainfo;
            public uint padding1;
            public uint padding2;
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] KEYBOARD_INPUT[] input, int structSize);

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key DOWN.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to press</param>
        private static void SimulateKeyDown(ushort keyCode)
        {
            const uint INPUT_KEYBOARD = 0x01;

            KEYBOARD_INPUT[] input = new KEYBOARD_INPUT[1];
            input[0] = new KEYBOARD_INPUT();
            input[0].type = INPUT_KEYBOARD;
            input[0].vk = keyCode;
            input[0].scanCode = 0;
            input[0].flags = 0;
            input[0].time = 0;
            input[0].extrainfo = 0;

            uint result = SendInput(1, input, Marshal.SizeOf(input[0]));

            if (result == 0) throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key UP.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to lift up</param>
        private static void SimulateKeyUp(ushort keyCode)
        {
            const uint INPUT_KEYBOARD = 0x01;
            const uint KEYUP = 0x0002;

            KEYBOARD_INPUT[] input = new KEYBOARD_INPUT[1];
            input[0] = new KEYBOARD_INPUT();
            input[0].type = INPUT_KEYBOARD;
            input[0].vk = keyCode;
            input[0].scanCode = 0;
            input[0].flags = KEYUP;
            input[0].time = 0;
            input[0].extrainfo = 0;

            uint result = SendInput(1, input, Marshal.SizeOf(input[0]));

            if (result == 0) throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
        }
    }
}
