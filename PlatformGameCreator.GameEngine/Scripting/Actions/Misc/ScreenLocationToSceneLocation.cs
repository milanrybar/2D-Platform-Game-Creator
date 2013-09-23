/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Misc
{
    /// <summary>
    /// Converts the location in the screen units (pixels) to the scene units (meters) and returns the result.
    /// </summary>
    [FriendlyName("Screen Location to Scene Location")]
    [Description("Converts the location in the screen units (pixels) to the scene units (meters) and returns the result.")]
    [Category("Actions/Misc")]
    public class ScreenLocationToSceneLocationAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Location in the screen units (pixel) to convert.
        /// </summary>
        [FriendlyName("Screen Location")]
        [Description("Location in the screen units (pixel) to convert.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> ScreenLocation;

        /// <summary>
        /// Outputs the converted location in the scene units (meters).
        /// </summary>
        [FriendlyName("Scene Location")]
        [Description("Outputs the converted location in the scene units (meters).")]
        [VariableSocket(VariableSocketType.Out)]
        public Variable<Vector2>[] SceneLocation;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            SetOutputVariable(ConvertUnits.ToSimUnits(Vector2.Transform(ScreenLocation.Value, Container.Actor.Screen.Camera.InversWorld)), SceneLocation);

            if (Out != null) Out();
        }
    }
}
