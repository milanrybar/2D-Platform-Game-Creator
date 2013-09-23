/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Scenes
{
    /// <summary>
    /// Form for editing parallax <see cref="Scenes.Layer"/> settings.
    /// </summary>
    partial class LayerParallaxSettings : Form
    {
        private Layer layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerParallaxSettings"/> class.
        /// </summary>
        /// <param name="layer">The layer to edit.</param>
        public LayerParallaxSettings(Layer layer)
        {
            InitializeComponent();

            this.layer = layer;

            parallaxCoefficientXFloatBox.Value = layer.ParallaxCoefficient.X;
            parallaxCoefficientYFloatBox.Value = layer.ParallaxCoefficient.Y;
            graphicsEffectRepeatHorizontallyCheckBox.Checked = (layer.GraphicsEffect & SceneElementEffect.RepeatHorizontally) != 0;
            graphicsEffectRepeatVerticallyCheckBox.Checked = (layer.GraphicsEffect & SceneElementEffect.RepeatVertically) != 0;
            graphicsEffectFillCheckBox.Checked = (layer.GraphicsEffect & SceneElementEffect.Fill) != 0;

            Text = "Layer Parallax Settings - " + layer.Name;
        }

        /// <summary>
        /// Handles the Click event of the okButton control.
        /// Close this form.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the ValueChanged event of the parallaxCoefficientXFloatBox control.
        /// Updates value of the <see cref="Scenes.Layer.ParallaxCoefficient"/> of X coordinate.
        /// </summary>
        private void parallaxCoefficientXFloatBox_ValueChanged(float value)
        {
            layer.ParallaxCoefficient.X = parallaxCoefficientXFloatBox.Value;
        }

        /// <summary>
        /// Handles the ValueChanged event of the parallaxCoefficientYFloatBox control.
        /// Updates value of the <see cref="Scenes.Layer.ParallaxCoefficient"/> of Y coordinate.
        /// </summary>
        private void parallaxCoefficientYFloatBox_ValueChanged(float value)
        {
            layer.ParallaxCoefficient.Y = parallaxCoefficientYFloatBox.Value;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the graphicsEffectRepeatHorizontallyCheckBox control.
        /// Updates value of the <see cref="Scenes.Layer.GraphicsEffect"/> for <see cref="SceneElementEffect.RepeatHorizontally"/> settings.
        /// </summary>
        private void graphicsEffectRepeatHorizontallyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (graphicsEffectRepeatHorizontallyCheckBox.Checked) layer.GraphicsEffect |= SceneElementEffect.RepeatHorizontally;
            else layer.GraphicsEffect &= ~SceneElementEffect.RepeatHorizontally;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the graphicsEffectRepeatVerticallyCheckBox control.
        /// Updates value of the <see cref="Scenes.Layer.GraphicsEffect"/> for <see cref="SceneElementEffect.RepeatVertically"/> settings.
        /// </summary>
        private void graphicsEffectRepeatVerticallyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (graphicsEffectRepeatVerticallyCheckBox.Checked) layer.GraphicsEffect |= SceneElementEffect.RepeatVertically;
            else layer.GraphicsEffect &= ~SceneElementEffect.RepeatVertically;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the graphicsEffectFillCheckBox control.
        /// Updates value of the <see cref="Scenes.Layer.GraphicsEffect"/> for <see cref="SceneElementEffect.Fill"/> settings.
        /// </summary>
        private void graphicsEffectFillCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (graphicsEffectFillCheckBox.Checked) layer.GraphicsEffect |= SceneElementEffect.Fill;
            else layer.GraphicsEffect &= ~SceneElementEffect.Fill;
        }
    }
}
