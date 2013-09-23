/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Scene state with behaviour that all states should contain.
    /// </summary>
    /// <remarks>
    /// Changes the position at the scene by right mouse button or arrows keys.
    /// Zooms the scene by the mouse wheel.
    /// Undoes last command at the scene by Ctrl+Z.
    /// Redoes last command at the scene by Ctrl+Y.
    /// </remarks>
    abstract class GlobalBehaviourSceneState : SceneState
    {
        /// <summary>
        /// Gets or sets a value indicating whether any action is in progress (is active).
        /// </summary>
        public bool ActionInProgress { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the action of moving position of the scene is active.
        /// </summary>
        public bool MovingScene
        {
            get { return _movingScene; }
            set
            {
                if (value && !ActionInProgress)
                {
                    _movingScene = true;
                    ActionInProgress = true;
                }
                else if (!value)
                {
                    _movingScene = false;
                    if (ActionInProgress) ActionInProgress = false;
                }
            }
        }
        private bool _movingScene;

        /// <summary>
        /// Distance in pixels for all directions for changing the position at the scene.
        /// The position of the scene will change of this distance in one second 
        /// if we press any keys associated for moving scene.
        /// </summary>
        public static float MoveVectorSizeByKeyboard = 1f;

        /// <summary>
        /// Last position at the scene to calculate move vector to the new position.
        /// </summary>
        private Vector2 movingSceneLastPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalBehaviourSceneState"/> class.
        /// </summary>
        public GlobalBehaviourSceneState()
        {
            MovingScene = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// If right mouse button is down and no action is in progress 
        /// we will start changing the position of the scene.
        /// </summary>
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            // If right mouse button is down and no action is in progress
            // we will start changing the position of the scene (moving scene).
            if (e.Button == MouseButtons.Right && !ActionInProgress)
            {
                // activate this action
                MovingScene = true;
                // init 
                movingSceneLastPosition = new Vector2(e.X, e.Y);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If right mouse button is up and the action of changing the position of the scene is active 
        /// we will finish it.
        /// </summary>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            // If right mouse button is up and the action of changing the position of the scene is active
            // we will finish it.
            if (e.Button == MouseButtons.Right && MovingScene)
            {
                // this action is over
                MovingScene = false;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// If the action of changing the position of the scene is active 
        /// we will change the position of the scene by distance we moved from the last changing.
        /// </summary>
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            // If the action of changing the position of the scene is active
            // we will change the position of the scene by distance we moved from the last changing.
            if (MovingScene)
            {
                // actual position
                Vector2 actualScenePosition = new Vector2(e.X, e.Y);
                // change the position of the scene
                Screen.Position += (movingSceneLastPosition - actualScenePosition) * Screen.ScaleInversFactor;
                // set actual position for the next changing
                movingSceneLastPosition = actualScenePosition;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Zooms the scene according to the mouse wheel.
        /// </summary>
        public override void MouseWheel(object sender, MouseEventArgs e)
        {
            // actual position under mouse cursor
            Vector2 actualPosition = Screen.MouseScenePosition;

            // zoom the scene
            Screen.Zoom += e.Delta / 30;

            // new positon under mouse cursor
            Vector2 changedPosition = Screen.PointAtScene(e.Location);

            // We want to keep the same position under mouse cursor before and after changing zoom of the scene
            // so we change the position of the scene by difference between before and after position under mouse cursor.
            Screen.Position += (actualPosition - changedPosition);
        }

        /// <inheritdoc />
        /// <summary>
        /// Undoes last command at the scene by Ctrl+Z.
        /// Redoes last command at the scene by Ctrl+Y.
        /// </summary>
        public override void KeyDown(object sender, KeyEventArgs e)
        {
            // undo last command at the scene
            if (e.Control && e.KeyCode == Keys.Z)
            {
                Screen.History.Undo();
            }
            // redo last command at the scene
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                Screen.History.Redo();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Changes the position at the scene by arrows keys.
        /// </summary>
        public override void KeyboardState(GameTime gameTime, ref Microsoft.Xna.Framework.Input.KeyboardState keyboardState)
        {
            Vector2 moveVector = new Vector2();
            bool moved = false;

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                moveVector += new Vector2(0, -MoveVectorSizeByKeyboard);
                moved = true;
            }
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                moveVector += new Vector2(0, MoveVectorSizeByKeyboard);
                moved = true;
            }
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                moveVector += new Vector2(-MoveVectorSizeByKeyboard, 0);
                moved = true;
            }
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                moveVector += new Vector2(MoveVectorSizeByKeyboard, 0);
                moved = true;
            }

            // If any key for changing the position of the scene (moving scene) is down
            // we change the position based on elapsed time.
            if (moved)
            {
                Screen.Position += moveVector * (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Screen.ScaleInversFactor);
            }
        }

        /// <summary>
        /// Determines whether the specified list of scene nodes contains any parents of the specified scene node.
        /// </summary>
        /// <param name="sceneNodesList">The list of scene nodes to check for parents.</param>
        /// <param name="sceneNode">The scene node to get parents from.</param>
        /// <returns><c>true</c> if the specified list of scene nodes contains any parents of the specified scene node; otherwise <c>false</c>.</returns>
        protected bool ContainsAnyParent(IList<SceneNode> sceneNodesList, SceneNode sceneNode)
        {
            SceneNode currentSceneNode = sceneNode;

            while (currentSceneNode.Parent != null)
            {
                currentSceneNode = currentSceneNode.Parent;
                if (sceneNodesList.Contains(currentSceneNode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
