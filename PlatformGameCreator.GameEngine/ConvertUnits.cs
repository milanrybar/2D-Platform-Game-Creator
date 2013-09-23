/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine
{
    /// <summary>
    /// Converts units between display (screen) and simulation units.
    /// </summary>
    public static class ConvertUnits
    {
        /// <summary>
        /// Ratio for converting the simulation units to the display units
        /// </summary>
        private static float _displayUnitsToSimUnitsRatio = 100f;

        /// <summary>
        /// Ratio for converting the display units to the simulation units
        /// </summary>
        private static float _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

        /// <summary>
        /// Sets the ratio of the display units to the simulation units (pixels to meters, how long is one meter in pixels).
        /// </summary>
        /// <param name="displayUnitsPerSimUnit">The ratio of the display units to the simulation.</param>
        public static void SetDisplayUnitToSimUnitRatio(float displayUnitsPerSimUnit)
        {
            _displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
            _simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="simUnits">Value in the simulation units to convert to the display units.</param>
        /// <returns>Converted value in the display units.</returns>
        public static float ToDisplayUnits(float simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="simUnits">Value in the simulation units to convert to the display units.</param>
        /// <returns>Converted value in the display units.</returns>
        public static float ToDisplayUnits(int simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="simUnits">Value in the simulation units to convert to the display units.</param>
        /// <returns>Converted value in the display units.</returns>
        public static Vector2 ToDisplayUnits(Vector2 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="simUnits">Value in the simulation units to convert to the display units.</param>
        /// <param name="displayUnits">Converted value in the display units.</param>
        public static void ToDisplayUnits(ref Vector2 simUnits, out Vector2 displayUnits)
        {
            Vector2.Multiply(ref simUnits, _displayUnitsToSimUnitsRatio, out displayUnits);
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="simUnits">Value in the simulation units to convert to the display units.</param>
        /// <returns>Converted value in the display units.</returns>
        public static Vector3 ToDisplayUnits(Vector3 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="x">Coordinate X in the simulation units to convert to the display units.</param>
        /// <param name="y">Coordinate Y in the simulation units to convert to the display units.</param>
        /// <returns>Converted vector in the display units.</returns>
        public static Vector2 ToDisplayUnits(float x, float y)
        {
            return new Vector2(x, y) * _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the simulation units to the display units.
        /// </summary>
        /// <param name="x">Coordinate X in the simulation units to convert to the display units.</param>
        /// <param name="y">Coordinate Y in the simulation units to convert to the display units.</param>
        /// <param name="displayUnits">Converted value in the display units.</param>
        public static void ToDisplayUnits(float x, float y, out Vector2 displayUnits)
        {
            displayUnits = Vector2.Zero;
            displayUnits.X = x * _displayUnitsToSimUnitsRatio;
            displayUnits.Y = y * _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="displayUnits">Value in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static float ToSimUnits(float displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="displayUnits">Value in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static float ToSimUnits(double displayUnits)
        {
            return (float)displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="displayUnits">Value in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static float ToSimUnits(int displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="displayUnits">Value in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static Vector2 ToSimUnits(Vector2 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="displayUnits">Value in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static Vector3 ToSimUnits(Vector3 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="displayUnits">Value in the display units to convert to the simulation units.</param>
        /// <param name="simUnits">Converted value in the simulation units.</param>
        public static void ToSimUnits(ref Vector2 displayUnits, out Vector2 simUnits)
        {
            Vector2.Multiply(ref displayUnits, _simUnitsToDisplayUnitsRatio, out simUnits);
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="x">Coordinate X in the display units to convert to the simulation units.</param>
        /// <param name="y">Coordinate Y in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static Vector2 ToSimUnits(float x, float y)
        {
            return new Vector2(x, y) * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="x">Coordinate X in the display units to convert to the simulation units.</param>
        /// <param name="y">Coordinate Y in the display units to convert to the simulation units.</param>
        /// <returns>Converted value in the simulation units.</returns>
        public static Vector2 ToSimUnits(double x, double y)
        {
            return new Vector2((float)x, (float)y) * _simUnitsToDisplayUnitsRatio;
        }

        /// <summary>
        /// Converts the specified value in the display units to the simulation units.
        /// </summary>
        /// <param name="x">Coordinate X in the display units to convert to the simulation units.</param>
        /// <param name="y">Coordinate Y in the display units to convert to the simulation units.</param>
        /// <param name="simUnits">Converted value in the simulation units.</param>
        public static void ToSimUnits(float x, float y, out Vector2 simUnits)
        {
            simUnits = Vector2.Zero;
            simUnits.X = x * _simUnitsToDisplayUnitsRatio;
            simUnits.Y = y * _simUnitsToDisplayUnitsRatio;
        }
    }
}
