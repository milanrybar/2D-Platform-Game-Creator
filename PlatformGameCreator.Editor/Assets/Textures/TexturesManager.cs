/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformGameCreator.Editor.Xna;
using System.Runtime.Serialization;

namespace PlatformGameCreator.Editor.Assets.Textures
{
    /// <summary>
    /// Manager for <see cref="Texture">textures</see> used in the project.
    /// </summary>
    [Serializable]
    class TexturesManager : ContentManager<Texture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TexturesManager"/> class.
        /// </summary>
        public TexturesManager()
        {
        }

        /// <inheritdoc />
        private TexturesManager(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <summary>
        /// Creates new <see cref="Texture"/> from <paramref name="filename"/>.
        /// </summary>
        /// <remarks>
        /// Compiles texture image to the XNA format of the texture and saves the result to the project content directory.
        /// Then load the texture into the memory.
        /// </remarks>
        /// <param name="filename">The filename of texture image.</param>
        /// <param name="verbose">If set to true process will be verbose via standard <see cref="Messages"/> system.</param>
        /// <returns>Returns created <see cref="Texture"/> when finishes successfully otherwise null.</returns>
        public Texture Create(string filename, bool verbose = false)
        {
            try
            {
                int id;
                if (Build(out id, filename, null, "TextureProcessor", verbose) && id != -1)
                {
                    if (verbose) Messages.ShowInfo("Loading into memory.");

                    // load (xna) texture
                    Texture2D textureXna = Load<Texture2D>(id.ToString());

                    // create texture
                    Texture texture = new Texture(textureXna, id);

                    // set texture name
                    texture.Name = Path.GetFileNameWithoutExtension(filename);

                    if (verbose) Messages.ShowInfo("Importing texture completed.");

                    return texture;
                }
                else
                {
                    return null;
                }
            }
            catch (IOException e)
            {
                Messages.ShowError("IO Error: " + e.Message);
            }
            catch (ContentLoadException e)
            {
                Messages.ShowError("Texture not found. Error: " + e.Message);
            }
            catch (Exception e)
            {
                Messages.ShowError("Error: " + e.Message);
            }

            return null;
        }

        /// <summary>
        /// Loads the XNA <see cref="Texture2D"/> by texture id.
        /// </summary>
        /// <param name="textureId">The texture id.</param>
        /// <returns>Loaded <see cref="Texture2D"/>.</returns>
        public Texture2D LoadXnaTextureById(int textureId)
        {
            return Load<Texture2D>(textureId.ToString());
        }

        /// <summary>
        /// Loads the XNA <see cref="Texture2D"/> by texture name.
        /// </summary>
        /// <param name="textureName">Name of the texture.</param>
        /// <returns>Loaded <see cref="Texture2D"/>.</returns>
        public Texture2D LoadXnaTexture(string textureName)
        {
            return Load<Texture2D>(textureName);
        }

        /// <summary>
        /// Copies the textures in the XNA format to <paramref name="outputDirectory"/>.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        public void CopyContent(string outputDirectory)
        {
            foreach (Texture texture in this)
            {
                string textureName = texture.Id + ".xnb";
                string outputFilename = Path.Combine(outputDirectory, textureName);

                if (!File.Exists(outputFilename))
                {
                    File.Copy(Path.Combine(Project.Singleton.ContentDirectory, textureName), outputFilename);
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Removes texture at the specified index.
        /// Also deletes files used by the removed texture (compiled texture in the XNA format).
        /// </remarks>
        public override void RemoveAt(int index)
        {
            if (index >= 0 && index <= Count)
            {
                Texture texture = this[index];

                if (texture.TextureXna != null) texture.TextureXna.Dispose();
                if (texture.TextureGdi != null) texture.TextureGdi.Dispose();

                string textureFilename = Path.Combine(Project.Singleton.ContentDirectory, texture.Id + ".xnb");
                if (File.Exists(textureFilename)) File.Delete(textureFilename);
            }

            base.RemoveAt(index);
        }
    }
}
