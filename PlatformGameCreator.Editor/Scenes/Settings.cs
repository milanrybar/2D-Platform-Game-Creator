/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Color settings for <see cref="SceneScreen"/>, <see cref="SceneState"/> and <see cref="SceneNode"/>.
    /// Settings are outside classes for easier manipulation.
    /// </summary>
    static class ColorSettings
    {
        /// <summary>
        /// Color for background of <see cref="SceneScreen"/>.
        /// </summary>
        public static Color BackgroundColor = new Color(163, 163, 163);

        /// <summary>
        /// Color for grid line at <see cref="SceneScreen"/>.
        /// </summary>
        public static Color GridLineColor = new Color(145, 145, 145);

        /// <summary>
        /// Color for background of information box at <see cref="SceneScreen"/>.
        /// </summary>
        public static Color InfoBoxBackgroundColor = new Color(71, 71, 71, 200);

        /// <summary>
        /// Color for border of information box at <see cref="SceneScreen"/>.
        /// </summary>
        public static Color InfoBoxBorderColor = new Color(0, 0, 0, 200);

        /// <summary>
        /// Color for text at information box at <see cref="SceneScreen"/>.
        /// </summary>
        public static Color InfoBoxTextColor = new Color(255, 255, 255);

        /// <summary>
        /// Color for selecting rectangle when selecting scene nodes at the scene (at <see cref="SceneScreen"/>).
        /// </summary>
        public static readonly Color SelectingNodesRectangle = new Color(255, 0, 0, 100);

        /// <summary>
        /// Color for simulation units at <see cref="SceneScreen"/>.
        /// </summary>
        public static Color SimulationUnitsColor = Color.White;

        /// <summary>
        /// Gets a color for bounding rectangle of the scene node.
        /// </summary>
        /// <param name="selectState">State of the scene node.</param>
        /// <returns>Color for bounding rectangle of the scene node.</returns>
        public static Color ForBounds(SelectState selectState)
        {
            switch (selectState)
            {
                case SelectState.Default:
                    return Color.Red;

                case SelectState.Hover:
                    return Color.Green;

                case SelectState.Select:
                    return Color.Blue;

                default:
                    Debug.Assert(true, "Not supported select type.");
                    throw new Exception("Not supported select type.");
            }
        }

        /// <summary>
        /// Gets a color for a shape of the scene node.
        /// </summary>
        /// <param name="selectState">State of the scene node.</param>
        /// <returns>Color for a shape of the scene node.</returns>
        public static Color ForShape(SelectState selectState)
        {
            switch (selectState)
            {
                case SelectState.Default:
                    return Color.Yellow;

                case SelectState.Hover:
                    return Color.Green;

                case SelectState.Select:
                    return Color.Blue;

                default:
                    Debug.Assert(true, "Not supported select type.");
                    throw new Exception("Not supported select type.");
            }
        }
    }

    /// <summary>
    /// Size settings for <see cref="SceneScreen"/>.
    /// Settings are outside classes for easier manipulation.
    /// </summary>
    static class SizeSettings
    {
        /// <summary>
        /// Size of information box at <see cref="SceneScreen"/>.
        /// </summary>
        public static Vector2 InfoBoxSize = new Vector2(120, 40);

        /// <summary>
        /// Width of arrow at simulation units at <see cref="SceneScreen"/>.
        /// </summary>
        public static float SimulationUnitsArrowWidth = 3f;

        /// <summary>
        /// Half of height of arrow at simulation units at <see cref="SceneScreen"/>.
        /// </summary>
        public static float SimulationUnitsArrowHalfHeight = 3f;
    }
}
