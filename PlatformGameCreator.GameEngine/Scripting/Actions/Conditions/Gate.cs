/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Conditions
{
    /// <summary>
    /// This action allows you to pass through a signal to the Out signal out socket depending on the state of the gate. To change the state of the gate, send a signal to the Toggle input. To open or close the gate, send a signal to the Open or Close input, respectively. The Out output will be active when there is an 'In' signal AND the gate is opened. This is useful to combine inputs, e.g.: If two conditions must occur for an action to be triggered. This can be achieved by opening the gate when Condition 1 is met and firing 'In' signal when Condition 2 is met.
    /// </summary>
    [FriendlyName("Gate")]
    [Description("This action allows you to pass through a signal to the Out signal out socket depending on the state of the gate. To change the state of the gate, send a signal to the Toggle input. To open or close the gate, send a signal to the Open or Close input, respectively. The Out output will be active when there is an 'In' signal AND the gate is opened. This is useful to combine inputs, e.g.: If two conditions must occur for an action to be triggered. This can be achieved by opening the gate when Condition 1 is met and firing 'In' signal when Condition 2 is met.")]
    [Category("Actions/Conditions")]
    public class GateAction : ActionNode
    {
        /// <summary>
        /// Fires when the signal went through the gate.
        /// </summary>
        [Description("Fires when the signal went through the gate.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Indicates whether the gate will initially be open.
        /// </summary>
        [FriendlyName("Start Open")]
        [Description("Indicates whether the gate will initially be open.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        [DefaultValue(true)]
        public Variable<bool> StartOpen;

        /// <summary>
        /// Amount of times the gate can be activated before it closes automatically. A value of 0 disables this functionality.
        /// </summary>
        [FriendlyName("Auto Close Count")]
        [Description("Amount of times the gate can be activated before it closes automatically. A value of 0 disables this functionality.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<int> AutoCloseCount;

        // indicates whether the node must be initialized
        private bool init = true;
        // indicates whether the gate is opened
        private bool gateOpen;
        // indicates whether the gate counts activated signals
        private bool useSignalCount;
        // number of times the gate was activated
        private int signalCount;

        /// <summary>
        /// Passes signal through to the Out signal out socket if the gate is currently in the open state.
        /// </summary>
        [Description("Passes signal through to the Out signal out socket if the gate is currently in the open state.")]
        public void In()
        {
            if (init)
            {
                InitSignalCount();
                gateOpen = StartOpen.Value;
                init = false;
            }

            if (gateOpen)
            {
                if (useSignalCount)
                {
                    if (signalCount > 0)
                    {
                        --signalCount;
                        if (signalCount <= 0) gateOpen = false;
                        if (Out != null) Out();
                    }
                    else gateOpen = false;
                }
                else
                {
                    if (Out != null) Out();
                }
            }
        }

        /// <summary>
        /// Sets the gate to the open state.
        /// </summary>
        [Description("Sets the gate to the open state.")]
        public void Open()
        {
            InitSignalCount();
            gateOpen = true;
        }

        /// <summary>
        /// Sets the gate to the closed state.
        /// </summary>
        [Description("Sets the gate to the closed state.")]
        public void Close()
        {
            gateOpen = false;
        }

        /// <summary>
        /// Toggles the state of the gate.
        /// </summary>
        [Description("Toggles the state of the gate.")]
        public void Toggle()
        {
            InitSignalCount();
            gateOpen = !gateOpen;
        }

        /// <summary>
        /// Initializes counting for activating the gate.
        /// </summary>
        private void InitSignalCount()
        {
            signalCount = AutoCloseCount.Value;
            useSignalCount = signalCount > 0;
        }
    }
}
