/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlatformGameCreator.Editor.Winforms
{
    /// <summary>
    /// Represents text box control for editing float value.
    /// </summary>
    /// <remarks>
    /// Float value can be represented by the current culture or the InvariantCulture.
    /// After editing the value is shown at the InvariantCulture.
    /// </remarks>
    class FloatBox : TextBox
    {
        /// <summary>
        /// Gets or sets the value of the control.
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Text = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        private float _value;

        /// <summary>
        /// Called when the value changes and passes the new value.
        /// </summary>
        /// <param name="value">The new value.</param>
        public delegate void ValueChangedHandler(float value);

        /// <summary>
        /// Occurs when the <see cref="Value"/> property value changes.
        /// </summary>
        public event ValueChangedHandler ValueChanged;

        /// <inheritdoc />
        /// <remarks>
        /// Updates the value.
        /// </remarks>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            PossibleValueChanged();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Updates the value.
        /// </remarks>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);

            PossibleValueChanged();
        }

        /// <summary>
        /// Called when that value possible changes.
        /// Updates the value if needed.
        /// </summary>
        private void PossibleValueChanged()
        {
            float value = 0f;
            if (Text != null)
            {
                if (!float.TryParse(Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value))
                {
                    float.TryParse(Text, out value);
                    Text = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

                if (float.IsNaN(value) || float.IsInfinity(value))
                {
                    value = 0f;
                    Text = value.ToString();
                }
            }

            if (value != _value)
            {
                _value = value;
                if (ValueChanged != null) ValueChanged(value);
            }
        }
    }
}
