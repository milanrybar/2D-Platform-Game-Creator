/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// State for changing the origin of the <see cref="Texture"/>.
    /// </summary>
    /// <remarks>
    /// Origin editing: Left Mouse - Choose origin position.
    /// </remarks>
    class ChangeOriginState : GlobalScreenState
    {
        /// <summary>
        /// Previous state of the <see cref="ShapesEditingState"/>.
        /// </summary>
        private ShapesEditingState previousState;

        /// <summary>
        /// <see cref="TextureScreen"/> where the state is used.
        /// </summary>
        private TextureScreen textureScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeOriginState"/> class.
        /// </summary>
        /// <param name="previousState">Previous state of the <see cref="ShapesEditingState"/>.</param>
        /// <param name="textureScreen"><see cref="TextureScreen"/> where the state is used.</param>
        public ChangeOriginState(ShapesEditingState previousState, TextureScreen textureScreen)
        {
            this.previousState = previousState;
            Parent = textureScreen;
            this.textureScreen = textureScreen;
        }

        /// <summary>
        /// Shows information about setting the origin of the texture.
        /// </summary>
        public override void OnSet()
        {
            Messages.ShowInfo("Click to choose origin. Left Mouse - Choose origin position.");
        }

        /// <inheritdoc/>
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

            if (e.Button == MouseButtons.Left && !ActionInProgress)
            {
                // new origin
                Microsoft.Xna.Framework.Vector2 move = Parent.MouseScreenPosition.ToVector2();
                textureScreen.Texture.Origin += move;

                // update data
                foreach (ShapeState shapeState in Parent.Shapes)
                {
                    shapeState.MoveShape(-move);
                }

                Parent.Invalidate();

                // return to previous state
                Parent.State = previousState;
            }
        }
    }
}
