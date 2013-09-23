/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PlatformGameCreator.Editor.Winforms
{
    /// <summary>
    /// Represents <see cref="NumericUpDown"/> (spin box) that can be used at the <see cref="ToolStrip"/>.
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    class ToolStripNumericUpDown : ToolStripControlHost
    {
        /// <summary>
        /// Gets or sets the value assigned to the spin box.
        /// </summary>
        public decimal Value
        {
            get { return numericUpDownControl.Value; }
            set { numericUpDownControl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the spin box.
        /// </summary>
        public decimal Minimum
        {
            get { return numericUpDownControl.Minimum; }
            set { numericUpDownControl.Minimum = value; }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the spin box.
        /// </summary>
        public decimal Maximum
        {
            get { return numericUpDownControl.Maximum; }
            set { numericUpDownControl.Maximum = value; }
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property value changes.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Internal <see cref="NumericUpDown"/> control.
        /// </summary>
        private NumericUpDown numericUpDownControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStripNumericUpDown"/> class.
        /// </summary>
        public ToolStripNumericUpDown()
            : base(new NumericUpDown())
        {
            numericUpDownControl = Control as NumericUpDown;
        }

        /// <inheritdoc />
        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);

            NumericUpDown mumControl = (NumericUpDown)c;
            mumControl.ValueChanged += new EventHandler(OnValueChanged);
        }

        /// <inheritdoc />
        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);

            NumericUpDown mumControl = (NumericUpDown)c;
            mumControl.ValueChanged -= new EventHandler(OnValueChanged);
        }

        /// <summary>
        /// Called when the value of the internal spin box changes.
        /// Invokes <see cref="ValueChanged"/> event.
        /// </summary>
        private void OnValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }
    }
}
