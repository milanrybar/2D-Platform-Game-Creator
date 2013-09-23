/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformGameCreator.Editor.Common
{
    /// <summary>
    /// Defines a method to release allocated resources.
    /// </summary>
    /// <remarks>
    /// Class for easy implementing <see cref="IDisposable"/>.
    /// </remarks>
    abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the instance has been disposed of.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }
        private bool _isDisposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise <c>false</c>.</param>
        protected abstract void Dispose(bool disposing);
    }
}
