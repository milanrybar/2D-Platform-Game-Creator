/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Assets
{
    /// <summary>
    /// Represent graphics asset that can be drawn on the screen and used in the game.
    /// </summary>
    public interface IGraphicsAsset
    {
        /// <summary>
        /// Creates the <see cref="ActorRenderer"/> to render the specified actor by this graphics asset.
        /// </summary>
        /// <param name="actor">The actor to render.</param>
        /// <returns>Created <see cref="ActorRenderer"/>.</returns>
        ActorRenderer CreateActorRenderer(Actor actor);
    }
}
