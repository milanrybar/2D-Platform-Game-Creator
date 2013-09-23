/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.GameEngine.Assets;
using PlatformGameCreator.GameEngine.Scenes;

namespace PlatformGameCreator.GameEngine.Scripting.Actions.Actors
{
    /// <summary>
    /// Sets the specified actors graphics to the specified texture.
    /// </summary>
    [FriendlyName("Set Texture")]
    [Description("Sets the specified actors graphics to the specified texture.")]
    [Category("Actions/Actors")]
    public class SetTextureAction : ActionNode
    {
        /// <summary>
        /// Fires when the action is completed.
        /// </summary>
        [Description("Fires when the action is completed.")]
        public ScriptSocketHandler Out;

        /// <summary>
        /// Actors to set the specified texture to.
        /// </summary>
        [Description("Actors to set the specified texture to.")]
        [VariableSocket(VariableSocketType.In)]
        [DefaultValueActorOwnerInstance]
        public Variable<Actor>[] Instance;

        /// <summary>
        /// Texture to be used as graphics in the specified actors.
        /// </summary>
        [Description("Texture to be used as graphics in the specified actors.")]
        [VariableSocket(VariableSocketType.In)]
        public Variable<TextureData> Texture;

        /// <summary>
        /// Indicates whether the texture will be flipped horizontally.
        /// </summary>
        [FriendlyName("Flip Horizontally")]
        [Description("Indicates whether the animation will be flipped horizontally.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<bool> FlipHorizontally;

        /// <summary>
        /// Indicates whether the animation will be flipped vertically.
        /// </summary>
        [FriendlyName("Flip Vertically")]
        [Description("Indicates whether the animation will be flipped vertically.")]
        [VariableSocket(VariableSocketType.In, Visible = false)]
        public Variable<bool> FlipVertically;

        /// <summary>
        /// Activates the action.
        /// </summary>
        [Description("Activates the action.")]
        public void In()
        {
            if (Texture != null && Texture.Value != null && Instance != null)
            {
                for (int i = 0; i < Instance.Length; ++i)
                {
                    if (Instance[i].Value != null)
                    {
                        TextureActorRenderer textureActorRenderer;
                        if (Instance[i].Value.Renderer != null && Instance[i].Value.Renderer.GraphicsAsset == Texture.Value) textureActorRenderer = (TextureActorRenderer)Instance[i].Value.Renderer;
                        else textureActorRenderer = (TextureActorRenderer)Texture.Value.CreateActorRenderer(Instance[i].Value);

                        textureActorRenderer.FlipHorizontally = FlipHorizontally.Value;
                        textureActorRenderer.FlipVertically = FlipVertically.Value;

                        Instance[i].Value.Renderer = textureActorRenderer;
                    }
                }
            }

            if (Out != null) Out();
        }
    }
}
