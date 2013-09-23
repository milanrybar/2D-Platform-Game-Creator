/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PlatformGameCreator.Editor.Xna
{
    /// <summary>
    /// Stores important XNA variables.
    /// </summary>
    abstract class XnaFramework
    {
        /// <summary>
        /// <see cref="XnaFramework"/> singleton.
        /// </summary>
        protected static XnaFramework Singleton = null;

        /// <summary>
        /// Gets the content manager for the XNA.
        /// </summary>
        /// <returns>Content manager for the XNA, if any.</returns>
        protected abstract ContentManager GetContentManager();

        /// <summary>
        /// Gets the content manager for the XNA.
        /// </summary>
        public static ContentManager ContentManager
        {
            get
            {
                if (Singleton == null) return null;
                else return Singleton.GetContentManager();
            }
        }

        /// <summary>
        /// Gets the builder for building XNA content.
        /// </summary>
        public static ContentBuilder ContentBuilder
        {
            get
            {
                if (_contentBuilder == null)
                {
                    try
                    {
                        _contentBuilder = new ContentBuilder();
                    }
                    catch (Microsoft.Build.Exceptions.InvalidProjectFileException)
                    {
                        Messages.ShowError("Unable to find XNA libraries for building content. Make sure you have installed Microsoft XNA Game Studio 4.0.");
                    }
                }
                return _contentBuilder;
            }
        }
        private static ContentBuilder _contentBuilder;
    }
}
