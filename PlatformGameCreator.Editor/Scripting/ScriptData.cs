/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.Assets.Sounds;
using PlatformGameCreator.Editor.GameObjects.Paths;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.Scenes;

namespace PlatformGameCreator.Editor.Scripting
{
    /// <summary>
    /// Represents type of a scripting node.
    /// </summary>
    enum NodeType
    {
        /// <summary>
        /// Event script node.
        /// </summary>
        Event,

        /// <summary>
        /// Action script node.
        /// </summary>
        Action
    };

    /// <summary>
    /// Represents type of a node socket.
    /// </summary>
    enum NodeSocketType
    {
        /// <summary>
        /// Signal in node socket.
        /// </summary>
        SignalIn,

        /// <summary>
        /// Signal out node socket.
        /// </summary>
        SignalOut,

        /// <summary>
        /// Variable in node socket.
        /// </summary>
        VariableIn,

        /// <summary>
        /// Variable out node socket.
        /// </summary>
        VariableOut
    };

    /// <summary>
    /// Represents type of variable that can be used in scripting.
    /// </summary>
    enum VariableType
    {
        /// <summary>
        /// Integer variable. Corresponding to <see cref="int"/> type.
        /// </summary>
        Int,

        /// <summary>
        /// Float variable. Corresponding to <see cref="float"/> type.
        /// </summary>
        Float,

        /// <summary>
        /// Boolean variable. Corresponding to <see cref="bool"/> type.
        /// </summary>
        Bool,

        /// <summary>
        /// String variable. Corresponding to <see cref="string"/> type.
        /// </summary>
        String,

        /// <summary>
        /// Actor variable. Corresponding to <see cref="Actor"/> type.
        /// </summary>
        Actor,

        /// <summary>
        /// Actor Type variable. Corresponding to <see cref="ActorType"/> type.
        /// </summary>
        ActorType,

        /// <summary>
        /// Vector variable. Corresponding to <see cref="Microsoft.Xna.Framework.Vector2"/> type.
        /// </summary>
        Vector2,

        /// <summary>
        /// Sound variable. Corresponding to <see cref="Sound"/> type.
        /// </summary>
        Sound,

        /// <summary>
        /// Song variable. Corresponding to <see cref="Sound"/> type.
        /// </summary>
        Song,

        /// <summary>
        /// Path variable. Corresponding to <see cref="Path"/> type.
        /// </summary>
        Path,

        /// <summary>
        /// Texture variable. Corresponding to <see cref="Texture"/> type.
        /// </summary>
        Texture,

        /// <summary>
        /// Animation variable. Corresponding to <see cref="Animation"/> type.
        /// </summary>
        Animation,

        /// <summary>
        /// Scene variable. Corresponding to <see cref="Scene"/> type.
        /// </summary>
        Scene,

        /// <summary>
        /// Key variable. Corresponding to <see cref="Microsoft.Xna.Framework.Input.Keys"/> type.
        /// </summary>
        Key
    };

    /// <summary>
    /// Base class for all scripting data that we get during runtime.
    /// </summary>
    class ScriptData
    {
    }

    /// <summary>
    /// Represents data of a script node.
    /// </summary>
    class NodeData : ScriptData
    {
        /// <summary>
        /// Name of the script node.
        /// </summary>
        public string Name;

        /// <summary>
        /// Realname in the assembly of the script node.
        /// </summary>
        public string RealName;

        /// <summary>
        /// Description of the script node.
        /// </summary>
        public string Description;

        /// <summary>
        /// Type of the script node.
        /// </summary>
        public NodeType Type;

        /// <summary>
        /// Sockets of the script node.
        /// </summary>
        public List<NodeSocketData> Sockets = new List<NodeSocketData>();
    }

    /// <summary>
    /// Represents data of a script node socket.
    /// </summary>
    class NodeSocketData
    {
        /// <summary>
        /// Name of the script node socket.
        /// </summary>
        public string Name;

        /// <summary>
        /// Realname in the assembly of the script node socket.
        /// </summary>
        public string RealName;

        /// <summary>
        /// Description of the script node socket.
        /// </summary>
        public string Description;

        /// <summary>
        /// Type of the script node socket.
        /// </summary>
        public NodeSocketType Type;

        /// <summary>
        /// Variable type of the script node socket.
        /// </summary>
        public VariableType VariableType;

        /// <summary>
        /// Indicates whether the the script variable node socket is an array.
        /// </summary>
        public bool IsArray;

        /// <summary>
        /// Default value of the script variable node socket.
        /// </summary>
        public object DefaultValue;

        /// <summary>
        /// Indicates whether the the script node socket is visible by default.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Indicates whether the script variable node socket can have no value.
        /// </summary>
        public bool CanBeEmpty;
    }

    /// <summary>
    /// Represents a category including scripting data.
    /// </summary>
    class CategoryData : ScriptData
    {
        /// <summary>
        /// Name of the category.
        /// </summary>
        public string Name;

        /// <summary>
        /// Items of the category.
        /// </summary>
        public List<ScriptData> Items = new List<ScriptData>();
    }

    /// <summary>
    /// Helper class for <see cref="VariableType"/> enum.
    /// </summary>
    static class VariableTypeHelper
    {
        /// <summary>
        /// Gets the friendly name of the specified variable type.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <returns>The friendly name of the specified variable type</returns>
        public static string FriendlyName(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Int:
                    return "Int";

                case VariableType.Float:
                    return "Float";

                case VariableType.Bool:
                    return "Bool";

                case VariableType.String:
                    return "String";

                case VariableType.Vector2:
                    return "Vector2";

                case VariableType.ActorType:
                    return "Actor Type";

                case VariableType.Actor:
                    return "Actor";

                case VariableType.Sound:
                    return "Sound";

                case VariableType.Song:
                    return "Song";

                case VariableType.Path:
                    return "Path";

                case VariableType.Texture:
                    return "Texture";

                case VariableType.Animation:
                    return "Animation";

                case VariableType.Scene:
                    return "Scene";

                case VariableType.Key:
                    return "Key";

                default:
                    Debug.Assert(true, "Variable type does not exist.");
                    return null;
            }
        }

        /// <summary>
        /// Gets the variable type representing the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Variable type representing the specified type.</returns>
        public static VariableType FromType(Type type)
        {
            if (type == typeof(bool)) return VariableType.Bool;
            else if (type == typeof(int)) return VariableType.Int;
            else if (type == typeof(float)) return VariableType.Float;
            else if (type == typeof(string)) return VariableType.String;
            else if (type == typeof(Microsoft.Xna.Framework.Vector2)) return VariableType.Vector2;
            else if (type == typeof(Microsoft.Xna.Framework.Audio.SoundEffect)) return VariableType.Sound;
            else if (type == typeof(Microsoft.Xna.Framework.Media.Song)) return VariableType.Song;
            else if (type == typeof(Microsoft.Xna.Framework.Input.Keys)) return VariableType.Key;
            // from Editor
            else if (type == typeof(Actor)) return VariableType.Actor;
            else if (type == typeof(ActorType)) return VariableType.ActorType;
            else if (type == typeof(Path)) return VariableType.Path;
            else if (type == typeof(Texture)) return VariableType.Texture;
            else if (type == typeof(Animation)) return VariableType.Animation;
            else if (type == typeof(Scene)) return VariableType.Scene;
            // from GameEngine
            else if (type == typeof(GameEngine.Scenes.Actor)) return VariableType.Actor;
            else if (type == typeof(uint)) return VariableType.ActorType;
            else if (type == typeof(GameEngine.Scenes.Path)) return VariableType.Path;
            else if (type == typeof(GameEngine.Assets.TextureData)) return VariableType.Texture;
            else if (type == typeof(GameEngine.Assets.AnimationData)) return VariableType.Animation;
            else if (type == typeof(GameEngine.Screens.Screen)) return VariableType.Scene;
            else
            {
                Debug.Assert(true, "Variable type does not exist.");
                throw new ArgumentException("Variable type does not exist.");
            }
        }

        /// <summary>
        /// Gets the type representing the specified variable type.
        /// </summary>
        /// <param name="variableType">Type of the variable.</param>
        /// <returns>Type representing the specified type.</returns>
        public static Type ToType(VariableType variableType)
        {
            if (variableType == VariableType.Bool) return typeof(bool);
            else if (variableType == VariableType.Int) return typeof(int);
            else if (variableType == VariableType.Float) return typeof(float);
            else if (variableType == VariableType.String) return typeof(string);
            else if (variableType == VariableType.Vector2) return typeof(Microsoft.Xna.Framework.Vector2);
            else if (variableType == VariableType.Actor) return typeof(Actor);
            else if (variableType == VariableType.ActorType) return typeof(ActorType);
            else if (variableType == VariableType.Sound) return typeof(Sound);
            else if (variableType == VariableType.Song) return typeof(Sound);
            else if (variableType == VariableType.Path) return typeof(Path);
            else if (variableType == VariableType.Texture) return typeof(Texture);
            else if (variableType == VariableType.Animation) return typeof(Animation);
            else if (variableType == VariableType.Scene) return typeof(Scene);
            else if (variableType == VariableType.Key) return typeof(Microsoft.Xna.Framework.Input.Keys);
            else
            {
                Debug.Assert(true, "Variable type does not exist.");
                throw new ArgumentException("Variable type does not exist.");
            }
        }
    }
}