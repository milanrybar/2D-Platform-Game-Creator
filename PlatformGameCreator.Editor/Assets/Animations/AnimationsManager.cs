/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Assets.Textures;

namespace PlatformGameCreator.Editor.Assets.Animations
{
    /// <summary>
    /// Manager for <see cref="Animation">animations</see> used in the project.
    /// </summary>
    [Serializable]
    class AnimationsManager : ContentManager<Animation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationsManager"/> class.
        /// </summary>
        public AnimationsManager()
        {
        }

        /// <inheritdoc />
        private AnimationsManager(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
