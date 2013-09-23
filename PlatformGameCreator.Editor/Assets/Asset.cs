/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Assets
{
    /// <summary>
    /// Represents type of asset.
    /// </summary>
    enum AssetType
    {
        /// <summary>
        /// Texture asset. Only used with the <see cref="Texture"/> class.
        /// </summary>
        Texture,

        /// <summary>
        /// Animation asset. Only used with the <see cref="Animation"/> class.
        /// </summary>
        Animation,

        /// <summary>
        /// Sound asset. Only used with the <see cref="Sound"/> class.
        /// </summary>
        Sound
    };

    /// <summary>
    /// Base class for an asset that can be used in game.
    /// </summary>
    /// <remarks>
    /// Every asset has unique id in the whole project.
    /// </remarks>
    [Serializable]
    abstract class Asset : IName, ISerializable
    {
        /// <summary>
        /// Gets the unique asset id in the whole project.
        /// </summary>
        public int Id
        {
            get { return _id; }
        }
        private int _id;

        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (NameChanged != null) NameChanged(this, EventArgs.Empty);
            }
        }
        private string _name;

        /// <summary>
        /// Gets the type of the asset.
        /// </summary>
        public abstract AssetType Type { get; }

        /// <summary>
        /// Occurs when the Name value changes.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        protected Asset()
        {
            _id = ContentManager.GetUniqueId();

            Debug.Assert(_id <= ContentManager.LastUniqueId, "Invalid id.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        /// <param name="id">The unique id for the asset.</param>
        protected Asset(int id)
        {
            Debug.Assert(id <= ContentManager.LastUniqueId, "Invalid id.");

            _id = id;
        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected Asset(SerializationInfo info, StreamingContext ctxt)
        {
            _id = info.GetInt32("Id");
            _name = info.GetString("Name");
        }

        /// <inheritdoc />
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
        }
    }
}
