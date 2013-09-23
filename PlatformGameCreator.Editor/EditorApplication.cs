/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Class that contains the main entry point for the application.
    /// </summary>
    static class EditorApplication
    {
        /// <summary>
        /// Gets the <see cref="EditorApplicationForm"/> singleton.
        /// </summary>
        public static EditorApplicationForm Editor
        {
            get { return _editor; }
        }
        private static EditorApplicationForm _editor;

        /// <summary>
        /// The main entry point for the application.
        /// Runs the <see cref="EditorApplicationForm"/>.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            _editor = new EditorApplicationForm();

            Application.Run(Editor);
        }

        /// <summary>
        /// Handles the UnhandledException event of the <see cref="AppDomain.CurrentDomain"/>.
        /// Show information to the user about the exception. Represents some fatal error.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception unhandledException = e.ExceptionObject as Exception;
            if (unhandledException != null)
            {
                MessageBox.Show("Error: " + unhandledException.Message, "Fatal Error");
            }
            else
            {
                MessageBox.Show("No error info given.", "Fatal Error");
            }
        }

        /// <summary>
        /// Handles the ThreadException event of the <see cref="Application"/>.
        /// Show information to the user about the exception. Represents some fatal error.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show("Error: " + e.Exception.Message, "Fatal Error");
        }
    }
}
