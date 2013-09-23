/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Misc
{
    /// <summary>
    /// Draws text on the specified position for the specified duration.
    /// </summary>
    [FriendlyName("Draw Text")]
    [Description("Draws text on the specified position for the specified duration.")]
    [Category("Actions/Misc")]
    public class DrawTextAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Text to draw on the specified position.
        /// </summary>
        [Description("Text to draw on the specified position.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValue("text")]
        public Variable<string> Text;

        /// <summary>
        /// Position to draw the specified text on.
        /// </summary>
        [FriendlyName("Position")]
        [Description("Position to draw the specified text on.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<Vector2> Position;

        /// <summary>
        /// Red part of a color (in RGB) for the drawn text.
        /// </summary>
        [FriendlyName("Color Red")]
        [Description("Red part of a color (in RGB) for the drawn text.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<int> ColorR;

        /// <summary>
        /// Green part of a color (in RGB) for the drawn text.
        /// </summary>
        [FriendlyName("Color Green")]
        [Description("Green part of a color (in RGB) for the drawn text.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<int> ColorG;

        /// <summary>
        /// Blue part of a color (in RGB) for the drawn text.
        /// </summary>
        [FriendlyName("Color Blue")]
        [Description("Blue part of a color (in RGB) for the drawn text.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<int> ColorB;

        /// <summary>
        /// Amount of time in seconds for how long the specified text is drawn during runtime.
        /// </summary>
        [Description("Amount of time in seconds for how long the specified text is drawn during runtime.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<float> Duration;

        /// <summary>
        /// Indicated whether the drawn text uses a large font.
        /// </summary>
        [FriendlyName("Large Font")]
        [Description("Indicated whether the drawn text uses a large font.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(false)]
        public Variable<bool> LargeFont;

        /// <summary>
        /// Draws text on the specified position for the specified duration.
        /// </summary>
        [Description("Draws text on the specified position for the specified duration.")]
        public void In()
        {
            if (Text != null && Text.Value != null)
            {
                TextEffect textEffect = new TextEffect(Text.Value, Position.Value,
                    new Color(MathHelper.Max(0, ColorR.Value % 256), MathHelper.Max(0, ColorG.Value % 256), MathHelper.Max(0, ColorB.Value % 256)));
                textEffect.Duration = Duration.Value;
                textEffect.DefaultFont = !LargeFont.Value;

                Container.Actor.Screen.AddNode(textEffect);
            }

            if (Out != null) Out();
        }
    }
}
