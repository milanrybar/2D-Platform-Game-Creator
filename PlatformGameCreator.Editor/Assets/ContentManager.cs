/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Xna;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Assets
{
    /// <summary>
    /// Content manager that assigns asset the unique id in the whole project. 
    /// </summary>
    [Serializable]
    class ContentManager : ISerializable
    {
        /// <summary>
        /// Gets the last used unique id for the asset.
        /// </summary>
        public static int LastUniqueId
        {
            get { return _lastUniqueId; }
        }
        private static int _lastUniqueId = 0;

        /// <summary>
        /// Gets the unique id for the new asset.
        /// </summary>
        /// <returns>Unique id for asset.</returns>
        public static int GetUniqueId()
        {
            return ++_lastUniqueId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class.
        /// </summary>
        public ContentManager()
        {

        }

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to retrieve data.</param>
        /// <param name="ctxt">The source (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this deserialization.</param>
        protected ContentManager(SerializationInfo info, StreamingContext ctxt)
        {
            _lastUniqueId = info.GetInt32("LastUniqueId");
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LastUniqueId", _lastUniqueId);
        }
    }

    /// <summary>
    /// Content manager contains <see cref="Asset">Assets</see> of the given type.
    /// </summary>
    /// <typeparam name="AssetType">The type of the <see cref="Asset"/> to contain.</typeparam>
    [Serializable]
    abstract class ContentManager<AssetType> : ObservableList<AssetType>, ISerializable where AssetType : Asset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager&lt;AssetType&gt;"/> class.
        /// </summary>
        public ContentManager()
        {
        }

        /// <inheritdoc />
        protected ContentManager(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        /// <summary>
        /// Builds the <paramref name="filename"/> by XNA Pipeline and moves result to the project content directory to use in the project.
        /// </summary>
        /// <param name="uniqueId">The unique id for asset using this <paramref name="filename"/>. The compiled filename will have the name of the unique id. If building is unsuccessfully finished then is set to -1.</param>
        /// <param name="filename">The filename to compile by XNA Pipeline.</param>
        /// <param name="importer">The importer to use.</param>
        /// <param name="processor">The processor to use</param>
        /// <param name="verbose">If set to true building will be verbose via standard <see cref="Messages"/> system.</param>
        /// <returns>Returns true if building is successfully finished, otherwise false.</returns>
        protected bool Build(out int uniqueId, string filename, string importer, string processor, bool verbose = false)
        {
            int uniqueIdForAsset = ContentManager.GetUniqueId();

            if (Build(uniqueIdForAsset.ToString(), filename, importer, processor, verbose))
            {
                uniqueId = uniqueIdForAsset;
                return true;
            }
            else
            {
                uniqueId = -1;
                return false;
            }
        }

        /// <summary>
        /// Builds the <paramref name="filename"/> by XNA Pipeline and moves result to the project content directory to use in the project.
        /// </summary>
        /// <param name="uniqueId">The compiled filename will have the name of the unique id.</param>
        /// <param name="filename">The filename to compile by XNA Pipeline.</param>
        /// <param name="importer">The importer to use.</param>
        /// <param name="processor">The processor to use</param>
        /// <param name="verbose">If set to true building will be verbose via standard <see cref="Messages"/> system.</param>
        /// <returns>Returns true if building is successfully finished, otherwise false.</returns>
        protected bool Build(string uniqueId, string filename, string importer, string processor, bool verbose = false)
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
        /// Loads the specified asset name by XNA <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.
        /// </summary>
        /// <typeparam name="T">Type of loading XNA asset.</typeparam>
        /// <param name="assetName">Name of the asset to load.</param>
        /// <returns>Loaded asset.</returns>
        protected T Load<T>(string assetName) where T : class
        {
            // load asset file via XNA pipeline
            return XnaFramework.ContentManager.Load<T>(assetName);
        }

        /// <summary>
        /// Finds the asset by given the id.
        /// </summary>
        /// <param name="id">The unique id of the asset to find.</param>
        /// <returns>Returns found asset if exists otherwise null.</returns>
        public AssetType FindById(int id)
        {
            foreach (AssetType asset in this)
            {
                if (asset.Id == id)
                {
                    return asset;
                }
            }

            return null;
        }
    }
}
