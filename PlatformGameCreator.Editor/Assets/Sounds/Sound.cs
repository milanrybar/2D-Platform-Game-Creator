/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Common;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Assets.Sounds
{
    /// <summary>
    /// Sound that can be used in game.
    /// </summary>
    [Serializable]
    class Sound : Asset, ISerializable, IDeserializationCallback
    {
        /// <inheritdoc />
        public override AssetType Type
        {
            get { return AssetType.Sound; }
        }

        /// <summary>
        /// Gets the filename of the sound. Sound is located in the project content directory.
        /// </summary>
        public string Filename
        {
            get { return _filename; }
        }
        private string _filename;

        /// <summary>
        /// Gets the format of the sound.
        /// </summary>
        public SoundFormat SoundFormat
        {
            get { return _soundFormat; }
        }
        private SoundFormat _soundFormat;

        /// <summary>
        /// Gets or sets the number of how many times is the sound used as XNA <see cref="Microsoft.Xna.Framework.Audio.SoundEffect"/>.
        /// </summary>
        public int SoundEffectUsed { get; set; }

        /// <summary>
        /// Gets or sets the number of how many times is the sound used as XNA <see cref="Microsoft.Xna.Framework.Media.Song"/>.
        /// </summary>
        public int SongUsed { get; set; }

        /// <summary>
        /// Gets a value indicating whether the sound is used as XNA <see cref="Microsoft.Xna.Framework.Audio.SoundEffect"/>.
        /// </summary>
        public bool IsSoundEffect
        {
            get { return SoundEffectUsed > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the sound is used as XNA <see cref="Microsoft.Xna.Framework.Media.Song"/>.
        /// </summary>
        public bool IsSong
        {
            get { return SongUsed > 0; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sound is already compiled as XNA <see cref="Microsoft.Xna.Framework.Audio.SoundEffect"/>. 
        /// </summary>
        internal bool CompiledAsSoundEffect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sound is already compiled as XNA <see cref="Microsoft.Xna.Framework.Media.Song"/>.
        /// </summary>
        internal bool CompiledAsSong { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="filename">The filename of the sound</param>
        /// <param name="soundFormat">The sound format.</param>
        public Sound(string filename, SoundFormat soundFormat)
            : base(ContentManager.GetUniqueId())
        {
            _filename = filename;
            _soundFormat = soundFormat;
        }

        /// <inheritdoc />
        protected Sound(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            _filename = info.GetString("Filename");
            _soundFormat = (SoundFormat)info.GetValue("SoundFormat", typeof(SoundFormat));
            SoundEffectUsed = info.GetInt32("SoundEffectUsed");
            SongUsed = info.GetInt32("SongUsed");
            CompiledAsSoundEffect = info.GetBoolean("CompiledAsSoundEffect");
            CompiledAsSong = info.GetBoolean("CompiledAsSong");
        }

        /// <inheritdoc />
        /// <remarks>
        /// Checks if the sound file exists, if not then the sound is removed from the project.
        /// </remarks>
        public void OnDeserialization(object sender)
        {
            // check if sound file exists
            if (!File.Exists(Path.Combine(Project.Singleton.ContentDirectory, Filename)))
            {
                Messages.ShowError(String.Format(@"Unable to load sound ""{0}"". Sound will be removed from project.", Name));

                if (new ConsistentDeletionForm(new ConsistentDeletionHelper.SoundForDeletion(this)) { AllowCancel = false, ProcessWhenEmptyList = true }.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Messages.ShowInfo("Sound deleted.");
                }
                else
                {
                    Debug.Assert(true);
                }
            }
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Filename", Filename);
            info.AddValue("SoundFormat", SoundFormat);
            info.AddValue("SoundEffectUsed", SoundEffectUsed);
            info.AddValue("SongUsed", SongUsed);
            info.AddValue("CompiledAsSoundEffect", CompiledAsSoundEffect);
            info.AddValue("CompiledAsSong", CompiledAsSong);
        }

        /// <summary>
        /// Returns the name of the sound.
        /// </summary>
        /// <returns>Returns the name of the sound.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
