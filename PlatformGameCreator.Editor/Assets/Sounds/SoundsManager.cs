/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using PlatformGameCreator.Editor.Xna;

namespace PlatformGameCreator.Editor.Assets.Sounds
{
    /// <summary>
    /// Format of the sound.
    /// </summary>
    enum SoundFormat
    {
        /// <summary>
        /// WAV sound format.
        /// </summary>
        Wav,

        /// <summary>
        /// MP3 sound format.
        /// </summary>
        Mp3,

        /// <summary>
        /// WMA sound format.
        /// </summary>
        Wma
    };

    /// <summary>
    /// Manager for <see cref="Sound">sounds</see> used in the project.
    /// </summary>
    [Serializable]
    class SoundsManager : ContentManager<Sound>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundsManager"/> class.
        /// </summary>
        public SoundsManager()
        {
        }

        /// <inheritdoc />
        private SoundsManager(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <summary>
        /// Creates new <see cref="Sound"/> from <paramref name="filename"/>.
        /// </summary>
        /// <remarks>
        /// Sound will be copied to the project content directory.
        /// Compilation to the XNA format is postponed until the sound is needed for the game.
        /// </remarks>
        /// <param name="filename">The filename of the sound.</param>
        /// <param name="verbose">If set to true process will be verbose via standard <see cref="Messages"/> system.</param>
        /// <returns>Returns created <see cref="Sound"/> when finishes successfully otherwise null.</returns>
        public Sound Create(string filename, bool verbose = false)
        {
            try
            {
                SoundFormat soundFormat;

                // find out format of the sound by its extension
                // if its correct format will be found when building sound for XNA
                switch (Path.GetExtension(filename).ToLower())
                {
                    case ".wav":
                        soundFormat = SoundFormat.Wav;
                        break;

                    case ".wma":
                        soundFormat = SoundFormat.Wma;
                        break;

                    case ".mp3":
                        soundFormat = SoundFormat.Mp3;
                        break;

                    default:
                        Messages.ShowError("Not supported sound format.");
                        return null;
                }

                // sound in the project content directory
                string contentFileName = Path.Combine(Project.Singleton.ContentDirectory, Path.GetFileName(filename));

                // if file with same the already exists in the project content directory
                // find first available filename in the format of name(number).extension
                if (File.Exists(contentFileName))
                {
                    int i = 1;
                    string name = Path.GetFileNameWithoutExtension(filename);
                    string extensionName = Path.GetExtension(filename);

                    contentFileName = Path.Combine(Project.Singleton.ContentDirectory, String.Format("{0}({1}){2}", name, i, extensionName));
                    while (File.Exists(contentFileName))
                    {
                        ++i;
                        contentFileName = Path.Combine(Project.Singleton.ContentDirectory, String.Format("{0}({1}){2}", name, i, extensionName));
                    }
                }

                Messages.ShowInfo("Copying to the content directory.");

                // copy file to the project content directory
                // copy origin file, the sound will be build later when is needed
                File.Copy(filename, contentFileName);

                // create sound
                Sound sound = new Sound(Path.GetFileName(filename), soundFormat);

                // set sound name
                sound.Name = Path.GetFileNameWithoutExtension(filename);

                if (verbose) Messages.ShowInfo("Importing sound completed.");

                return sound;
            }
            catch (Exception e)
            {
                Messages.ShowError("Error: " + e.Message);
            }

            return null;
        }

        /// <summary>
        /// Builds the sounds that need to be build for use in the game (and have not been built yet).
        /// </summary>
        /// <param name="verbose">If set to true process will be verbose via standard <see cref="Messages"/> system.</param>
        /// <returns>Returns true if process is successfully finished, otherwise false.</returns>
        public bool BuildSounds(bool verbose = false)
        {
            foreach (Sound sound in this)
            {
                // sound is used as XNA SoundEffect and is not build yet
                if (sound.IsSoundEffect && !sound.CompiledAsSoundEffect)
                {
                    if (verbose) Messages.ShowInfo(String.Format(@"Sound ""{0}"" will be build as SoundEffect.", sound.Name));

                    // build sound as XNA SoundEffect
                    if (Build(sound.Id.ToString(), Path.Combine(Project.Singleton.ContentDirectory, sound.Filename), GetImporter(sound.SoundFormat), "SoundEffectProcessor", verbose))
                    {
                        sound.CompiledAsSoundEffect = true;
                    }
                    else
                    {
                        return false;
                    }
                }
                // sound is used as XNA Song and is not build yet
                if (sound.IsSong && !sound.CompiledAsSong)
                {
                    if (verbose) Messages.ShowInfo(String.Format(@"Sound ""{0}"" will be build as Song.", sound.Name));

                    // build sound as XNA Song
                    if (BuildSong(String.Format("{0}S", sound.Id), Path.Combine(Project.Singleton.ContentDirectory, sound.Filename), GetImporter(sound.SoundFormat), "SongProcessor", verbose))
                    {
                        sound.CompiledAsSong = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Builds the sound by XNA Pipeline as <see cref="Microsoft.Xna.Framework.Media.Song"/>.
        /// Almost the same as <see cref="ContentManager{T}.Build(string, string, string, string, bool)"/> but <see cref="Microsoft.Xna.Framework.Media.Song"/> contains of 2 files (.xnb and .wma). 
        /// </summary>
        /// <param name="uniqueId">The compiled filename will have the name of the unique id.</param>
        /// <param name="filename">The filename to compile by XNA Pipeline.</param>
        /// <param name="importer">The importer to use.</param>
        /// <param name="processor">The processor to use</param>
        /// <param name="verbose">If set to true building will be verbose via standard <see cref="Messages"/> system.</param>
        /// <returns>Returns true if building is successfully finished, otherwise false.</returns>
        private bool BuildSong(string uniqueId, string filename, string importer, string processor, bool verbose = false)
        {
            if (verbose) Messages.ShowInfo("Preparing to build.");

            if (XnaFramework.ContentBuilder == null) return false;

            // add file to builder
            XnaFramework.ContentBuilder.Add(filename, uniqueId, importer, processor);

            if (verbose) Messages.ShowInfo("Building.");

            // build asset to xna format
            string buildError = XnaFramework.ContentBuilder.Build();

            // build completed without any error
            if (string.IsNullOrEmpty(buildError))
            {
                if (verbose)
                {
                    Messages.ShowInfo("Building successfully finished.");
                    Messages.ShowInfo("Moving to the content directory.");
                }

                // filename in the project directory 
                string outputFileName = Path.Combine(Project.Singleton.ContentDirectory, uniqueId + ".xnb");
                // delete any file with the same name (we cannot delete our any other asset because we have unique name)
                if (File.Exists(outputFileName)) File.Delete(outputFileName);
                // move built file to the project content directory
                File.Move(Path.Combine(XnaFramework.ContentBuilder.OutputDirectory, uniqueId + ".xnb"), outputFileName);

                // filename in the project directory 
                outputFileName = Path.Combine(Project.Singleton.ContentDirectory, uniqueId + ".wma");
                // delete any file with the same name (we cannot delete our any other asset because we have unique name)
                if (File.Exists(outputFileName)) File.Delete(outputFileName);
                // move built file to the project content directory
                File.Move(Path.Combine(XnaFramework.ContentBuilder.OutputDirectory, uniqueId + ".wma"), outputFileName);

                // clear content builder
                XnaFramework.ContentBuilder.Clear();

                return true;
            }
            // errors in building
            else
            {
                // clear content builder
                XnaFramework.ContentBuilder.Clear();

                if (verbose) Messages.ShowInfo("Building unsuccessfully finished.");

                Messages.ShowError("Build Error: " + buildError);

                return false;
            }
        }

        /// <summary>
        /// Gets the XNA Pipeline importer for the given sound format.
        /// </summary>
        /// <param name="soundFormat">Format of the sound.</param>
        /// <returns>Returns XNA Pipeline importer.</returns>
        private string GetImporter(SoundFormat soundFormat)
        {
            switch (soundFormat)
            {
                case SoundFormat.Wav:
                    return "WavImporter";

                case SoundFormat.Wma:
                    return "WmaImporter";

                case SoundFormat.Mp3:
                    return "Mp3Importer";

                default:
                    Debug.Assert(true);
                    return null;
            }
        }

        /// <summary>
        /// Copies the sounds in the XNA format to <paramref name="outputDirectory"/>.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        public void CopyContent(string outputDirectory)
        {
            foreach (Sound sound in this)
            {
                // copy sound built as XNA SoundEffect
                if (sound.CompiledAsSoundEffect)
                {
                    string textureName = sound.Id + ".xnb";
                    string outputFilename = Path.Combine(outputDirectory, textureName);

                    if (!File.Exists(outputFilename))
                    {
                        File.Copy(Path.Combine(Project.Singleton.ContentDirectory, textureName), outputFilename);
                    }
                }
                // copy sound built as XNA Song
                if (sound.CompiledAsSong)
                {
                    string textureName = sound.Id + "S.xnb";
                    string outputFilename = Path.Combine(outputDirectory, textureName);

                    if (!File.Exists(outputFilename))
                    {
                        File.Copy(Path.Combine(Project.Singleton.ContentDirectory, textureName), outputFilename);
                    }

                    textureName = sound.Id + "S.wma";
                    outputFilename = Path.Combine(outputDirectory, textureName);

                    if (!File.Exists(outputFilename))
                    {
                        File.Copy(Path.Combine(Project.Singleton.ContentDirectory, textureName), outputFilename);
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Removes sound at the specified index.
        /// Also deletes files used by the removed sound (original sound file and compiled sound for the XNA format).
        /// </remarks>
        public override void RemoveAt(int index)
        {
            if (index >= 0 && index <= Count)
            {
                Sound sound = this[index];

                string soundFilename = Path.Combine(Project.Singleton.ContentDirectory, sound.Filename);
                if (File.Exists(soundFilename)) File.Delete(soundFilename);

                if (sound.CompiledAsSoundEffect)
                {
                    soundFilename = Path.Combine(Project.Singleton.ContentDirectory, sound.Id + ".xnb");
                    if (File.Exists(soundFilename)) File.Delete(soundFilename);
                }

                if (sound.CompiledAsSong)
                {
                    soundFilename = Path.Combine(Project.Singleton.ContentDirectory, sound.Id + "S.xnb");
                    if (File.Exists(soundFilename)) File.Delete(soundFilename);

                    soundFilename = Path.Combine(Project.Singleton.ContentDirectory, sound.Id + "S.wma");
                    if (File.Exists(soundFilename)) File.Delete(soundFilename);
                }
            }

            base.RemoveAt(index);
        }
    }
}
