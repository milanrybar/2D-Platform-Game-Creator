/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Color settings for <see cref="ShapesEditingScreen"/> and <see cref="BasicControllerForShapesEditing"/>.
    /// Settings are outside classes for easier manipulation.
    /// </summary>
    static class ColorSettings
    {
        /// <summary>
        /// Color for grid line at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color GridLineColor = Color.Gray;

        /// <summary>
        /// Color for strong grid line (every fifth line) at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color GridStrongLineColor = Color.Black;

        /// <summary>
        /// Color for background of <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color BackgroundColor = Color.WhiteSmoke;

        /// <summary>
        /// Color for the origin vertex at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color OriginVertexColor = Color.White;

        /// <summary>
        /// Color for the origin vertex border at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color OriginVertexBorderColor = Color.Black;

        /// <summary>
        /// Color for the selected vertex of a shape at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color ShapeVertexSelected = Color.Yellow;

        /// <summary>
        /// Color for the last vertex of polygon or edge shape at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color ShapeVertexLast = Color.Blue;

        /// <summary>
        /// Color for line of a shape at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color ShapeLineColor = Color.Blue;

        /// <summary>
        /// Color for the vertex of polygon shape at <see cref="ShapesEditingScreen"/> which is part of the convex hull when convex hull is shown.
        /// </summary>
        public static Color PolygonVertexValid = Color.Green;

        /// <summary>
        /// Color for the vertex of polygon shape at <see cref="ShapesEditingScreen"/> which is not part of the convex hull when convex hull is shown.
        /// </summary>
        public static Color PolygonVertexInvalid = Color.Red;

        /// <summary>
        /// Color for line of the convex hull at <see cref="ShapesEditingScreen"/> when convex hull is shown. 
        /// </summary>
        public static Color PolygonConvexHullLineColor = Color.Green;

        /// <summary>
        /// Color for line of the convex decomposition at <see cref="ShapesEditingScreen"/> when convex decomposition is shown. 
        /// </summary>
        public static Color PolygonConvexDecompositionLineColor = Color.CornflowerBlue;

        /// <summary>
        /// Color for the vertex of edge shape at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color EdgeVertex = Color.Green;

        /// <summary>
        /// Color for the circle origin of circle shape at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static Color CircleOriginVertexColor = Color.Green;

        /// <summary>
        /// Colors for distinguishing shapes at <see cref="ShapesEditingScreen"/> when shape is not active and background of shape item at shapes list at <see cref="BasicControllerForShapesEditing"/>.
        /// </summary>
        public static Color[] GoodColors = { Color.Red, Color.Green, Color.CornflowerBlue, Color.Purple, Color.DeepPink, Color.Orange };
    }

    /// <summary>
    /// Size settings for <see cref="ShapesEditingScreen"/>.
    /// Settings are outside classes for easier manipulation.
    /// </summary>
    static class SizeSettings
    {
        /// <summary>
        /// Size of the grid gap at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static float GridGapSize = 20;

        /// <summary>
        /// Radius of a vertex at <see cref="ShapesEditingScreen"/>.
        /// </summary>
        public static float VertexRadius = 5;
    }
}
