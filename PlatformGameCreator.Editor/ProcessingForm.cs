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
using System.Diagnostics;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Delegate to execute the operation.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
    delegate void ProcessingDoWorkCallback(IProcessingContainer container, DoWorkEventArgs e);

    /// <summary>
    /// Delegate that is called after the operation is completed.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
    delegate void ProcessingCompletedCallback(IProcessingContainer container, RunWorkerCompletedEventArgs e);

    /// <summary>
    /// Represents container that executes some operation at the background.
    /// </summary>
    interface IProcessingContainer
    {
        /// <summary>
        /// Gets or sets the title of the container.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the current task.
        /// </summary>
        string CurrentTask { get; set; }

        /// <summary>
        /// Gets or sets the log text of the operation.
        /// </summary>
        string LogText { get; set; }

        /// <summary>
        /// Gets or sets the progress percentage of the operation.
        /// </summary>
        int ProgressPercentage { get; set; }
    }

    /// <summary>
    /// Form that executes an operation on a separate thread and shows progress and information about the operation.
    /// </summary>
    /// <remarks>
    /// Internally uses <see cref="System.ComponentModel.BackgroundWorker"/> for executing an operation on a separate thread.
    /// </remarks>
    partial class ProcessingForm : Form, IProcessingContainer
    {
        /// <summary>
        /// Represents the messages manager for the <see cref="ProcessingForm"/>.
        /// </summary>
        private class ProcessingMessagesManager : IMessagesManager
        {
            private ProcessingForm processingForm;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProcessingMessagesManager"/> class.
            /// </summary>
            /// <param name="processingForm">The <see cref="ProcessingMessagesManager"/>.</param>
            public ProcessingMessagesManager(ProcessingForm processingForm)
            {
                Debug.Assert(processingForm != null, "ProcessingForm cannot be null.");
                this.processingForm = processingForm;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Adds the information message to the log of the operation.
            /// </remarks>
            public void ShowInfo(string message)
            {
                processingForm.LogText = message + System.Environment.NewLine + processingForm.LogText;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Adds the warning message to the log of the operation.
            /// </remarks>
            public void ShowWarning(string message)
            {
                processingForm.LogText = "Warning: " + message + System.Environment.NewLine + processingForm.LogText;
            }

            /// <inheritdoc />
            /// <remarks>
            /// Adds the error message to the log of the operation and notes that the operation finished by the error.
            /// </remarks>
            public void ShowError(string message)
            {
                processingForm.finishedByError = true;
                processingForm.LogText = "Error: " + message + System.Environment.NewLine + processingForm.LogText;
                MessageBox.Show(message, "Error");
            }
        }

        /// <summary>
        /// Gets or sets the title of the form.
        /// </summary>
        public string Title
        {
            get
            {
                if (InvokeRequired)
                {
                    IAsyncResult result = BeginInvoke((Func<string>)delegate { return Title; });
                    return (string)EndInvoke(result);
                }
                else
                {
                    return Text;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { Title = value; });
                }
                else
                {
                    Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the current task.
        /// </summary>
        public string CurrentTask
        {
            get
            {
                if (currentLabel.InvokeRequired)
                {
                    IAsyncResult result = currentLabel.BeginInvoke((Func<string>)delegate { return CurrentTask; });
                    return (string)currentLabel.EndInvoke(result);
                }
                else
                {
                    return currentLabel.Text;
                }
            }
            set
            {
                if (currentLabel.InvokeRequired)
                {
                    currentLabel.BeginInvoke((MethodInvoker)delegate { CurrentTask = value; });
                }
                else
                {
                    currentLabel.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress percentage of the operation.
        /// </summary>
        public int ProgressPercentage
        {
            get
            {
                if (progressBar.InvokeRequired)
                {
                    IAsyncResult result = progressBar.BeginInvoke((Func<int>)delegate { return ProgressPercentage; });
                    return (int)progressBar.EndInvoke(result);
                }
                else
                {
                    return progressBar.Value;
                }
            }
            set
            {
                if (progressBar.InvokeRequired)
                {
                    progressBar.BeginInvoke((MethodInvoker)delegate { ProgressPercentage = value; });
                }
                else
                {
                    progressBar.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the log text of the operation.
        /// </summary>
        public string LogText
        {
            get
            {
                if (logTextBox.InvokeRequired)
                {
                    IAsyncResult result = logTextBox.BeginInvoke((Func<string>)delegate { return LogText; });
                    return (string)logTextBox.EndInvoke(result);
                }
                else
                {
                    return logTextBox.Text;
                }
            }
            set
            {
                if (logTextBox.InvokeRequired)
                {
                    logTextBox.BeginInvoke((MethodInvoker)delegate { LogText = value; });
                }
                else
                {
                    logTextBox.Text = value;
                }
            }
        }

        // messages manager for the form
        private ProcessingMessagesManager messagesManager;

        // callback to execute the operation
        private ProcessingDoWorkCallback doWork;
        // callback when the operation is finished
        private ProcessingCompletedCallback completed;

        // indicates whether the form is automatically closed when the operation is finished
        private bool closeWhenFinished;
        // indicates whether the operation finished by error
        private bool finishedByError;

        // indicates whether the form can be closed
        private bool canClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingForm"/> class.
        /// </summary>
        public ProcessingForm()
        {
            InitializeComponent();

            messagesManager = new ProcessingMessagesManager(this);
        }

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        /// <param name="argument">The argument to pass to the <paramref name="doWork"/>.</param>
        /// <param name="doWork">The operation delegate</param>
        /// <param name="completed">The completed delegate.</param>
        /// <param name="closeWhenFinished">if set to <c>true</c> the form is automatically closed when the operation is finished.</param>
        /// <exception cref="Exception">Instance is already in use.</exception>
        public void Process(object argument, ProcessingDoWorkCallback doWork, ProcessingCompletedCallback completed, bool closeWhenFinished = true)
        {
            if (backgroundWorker.IsBusy) throw new Exception("Instance is already in use.");

            canClose = false;

            logTextBox.Text = String.Empty;
            currentLabel.Text = String.Empty;
            progressBar.Value = 0;
            closeButton.Enabled = false;

            this.doWork = doWork;
            this.completed = completed;
            this.closeWhenFinished = closeWhenFinished;
            finishedByError = false;

            Messages.MessagesManager = messagesManager;

            backgroundWorker.RunWorkerAsync(argument);

            ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the closeButton control.
        /// Closes the form.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker.
        /// Executes the operation.
        /// </summary>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (doWork != null) doWork(this, e);
        }

        /// <summary>
        /// Handles the ProgressChanged event of the backgroundWorker.
        /// Updates the process percentage.
        /// </summary>
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the backgroundWorker.
        /// The operation is finished. Calls the delegate after the operation is finished.
        /// </summary>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            canClose = true;

            if (!finishedByError) progressBar.Value = 100;
            closeButton.Enabled = true;

            if (closeWhenFinished && !finishedByError) Close();

            if (completed != null) completed(this, e);
        }

        /// <summary>
        /// Handles the FormClosing event of the ProcessingForm control.
        /// </summary>
        private void ProcessingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !canClose)
            {
                e.Cancel = true;
            }
        }
    }
}
