/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Represents state of the <see cref="SceneScreen"/> control.
    /// </summary>
    abstract class SceneState
    {
        /// <summary>
        /// Gets or sets the <see cref="SceneScreen"/> where the state is used.
        /// </summary>
        public SceneScreen Screen { get; set; }

        /// <summary>
        /// Gets a value indicating whether the state can be interrupted.
        /// </summary>
        /// <remarks>
        /// When the state cannot be interrupted then the state at <see cref="SceneScreen"/> cannot be changed.
        /// </remarks>
        public abstract bool CanBeInterrupted { get; }

        /// <summary>
        /// Gets a value indicating whether the state can be stacked in an another state.
        /// </summary>
        /// <remarks>
        /// Some state can used the current state at <see cref="SceneScreen"/> as previous state and 
        /// when the action of the state is done then sets back the previous state. 
        /// When the state cannot be stacked then the state cannot be used as previous state.
        /// </remarks>
        public abstract bool CanBeInStack { get; }

        /// <summary>
        /// Handles the MouseDown event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseDown(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the MouseUp event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseUp(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the MouseMove event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseMove(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the MouseWheel event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public virtual void MouseWheel(object sender, MouseEventArgs e) { }

        /// <summary>
        /// Handles the KeyDown event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        public virtual void KeyDown(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Handles the KeyUp event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        public virtual void KeyUp(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Handles the KeyPress event of the <see cref="SceneScreen"/> control. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        public virtual void KeyPress(object sender, KeyPressEventArgs e) { }

        /// <summary>
        /// Called when the <see cref="SceneScreen"/> is updating. Override this method with state-specific behaviour.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        /// <param name="keyboardState">State of the keyboard.</param>
        public virtual void KeyboardState(GameTime gameTime, ref KeyboardState keyboardState) { }

        /// <summary>
        /// Called when the <see cref="SceneScreen"/> is drawing. Override this method with state-specific drawing code.
        /// </summary>
        /// <param name="sceneBatch">The scene batch for drawing the state.</param>
        public virtual void Draw(SceneBatch sceneBatch) { }

        /// <summary>
        /// Called when the state is set as the <see cref="SceneScreen.State"/> of the <see cref="SceneScreen"/>. Override this method with state-specific behaviour.
        /// </summary>
        public virtual void OnSet() { }

        /// <summary>
        /// Called when the state cannot be interrupted and someone tries to interrupt it. Override this method with state-specific behaviour.
        /// </summary>
        /// <remarks>
        /// Usually shows some information to the user.
        /// </remarks>
        public virtual void OnTryInterrupt() { }
    }
}
