/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.Common;
using System.IO;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.GameObjects;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Assets.Animations;
using PlatformGameCreator.Editor.GameObjects.Paths;

namespace PlatformGameCreator.Editor.Building
{
    /// <summary>
    /// Generates the source code in C# for the scene. Only used by <see cref="GameGenerator"/>.
    /// </summary>
    class SceneGenerator
    {
        private TextWriter writer;
        private Scene scene;
        private ActorGenerator actorGenerator;

        /// <summary>
        /// Gets a name of namespace for the specified scene.
        /// </summary>
        /// <param name="scene">The scene to get namespace for.</param>
        /// <returns>Name of namespace for the specified scene.</returns>
        public static string GetSceneNamespace(Scene scene)
        {
            return "Scene" + scene.Index;
        }

        /// <summary>
        /// Gets a fullname of class for the specified scene (namespace + class name).
        /// </summary>
        /// <param name="scene">The scene to get fullname for.</param>
        /// <returns>Fullname of class for the specified scene.</returns>
        public static string GetSceneFullClassName(Scene scene)
        {
            return GetSceneNamespace(scene) + ".SceneLevel";
        }

        /// <summary>
        /// Generates actor classes.
        /// </summary>
        private void GenerateActors()
        {
            foreach (Actor actor in scene.AllActors())
            {
                GenerateActor(actor);
            }
        }

        /// <summary>
        /// Generates an actor class for the specified actor.
        /// </summary>
        /// <param name="actor">The actor to generate class for.</param>
        private void GenerateActor(Actor actor)
        {
            actorGenerator.Generate(actor, writer);

            foreach (Actor child in actor.Children)
            {
                GenerateActor(child);
            }
        }

        /// <summary>
        /// Generates source code for inializiting the specified texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        private void GenerateTextureData(Texture texture)
        {
            writer.WriteLine("TextureData textureData = new TextureData();");

            writer.WriteLine(@"textureData.Texture = Content.Load<Texture2D>(""{0}"");", texture.Id);

            writer.WriteLine("textureData.Origin = new Vector2({0}f, {1}f);", texture.Origin.X, texture.Origin.Y);

            writer.WriteLine("GraphicsAssets.Add({0}, textureData);", texture.Id);
        }

        /// <summary>
        /// Generates source code for inializiting the specified animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        private void GenerateAnimationData(Animation animation)
        {
            if (animation.Frames.Count != 0)
            {
                writer.WriteLine("AnimationData animationData = new AnimationData();");

                writer.WriteLine("animationData.Textures = new TextureData[{0}];", animation.Frames.Count);

                for (int i = 0; i < animation.Frames.Count; ++i)
                {
                    writer.WriteLine("animationData.Textures[{0}] = (TextureData)GraphicsAssets.Get({1});", i, animation.Frames[i].Id);
                }

                writer.WriteLine("animationData.Speed = {0}f;", animation.Speed);
                writer.WriteLine("animationData.Loop = {0};", "true");

                writer.WriteLine("GraphicsAssets.Add({0}, animationData);", animation.Id);
            }
        }

        /// <summary>
        /// Generates source code for inializiting the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        private void GeneratePath(PlatformGameCreator.Editor.GameObjects.Paths.Path path)
        {
            writer.Write("Paths.Add({0}, new Path({1}, new Vector2[{2}]", path.Id,
                path.Loop ? "true" : "false", path.Vertices.Count);

            writer.Write(" { ");

            for (int i = 0; i < path.Vertices.Count; ++i)
            {
                writer.Write("new Vector2({0}f, {1}f)",
                    GameEngine.ConvertUnits.ToSimUnits(path.Vertices[i].X),
                    GameEngine.ConvertUnits.ToSimUnits(path.Vertices[i].Y));
                if (i + 1 != path.Vertices.Count) writer.Write(", ");
            }

            writer.WriteLine(" }));");
        }

        /// <summary>
        /// Generates source code for inializiting the content of the current scene (paths, actors).
        /// </summary>
        private void GenerateContent()
        {
            // paths
            foreach (PlatformGameCreator.Editor.GameObjects.Paths.Path path in scene.Paths)
            {
                writer.WriteLine("{");
                GeneratePath(path);
                writer.WriteLine("}");
            }

            // global script
            writer.WriteLine("AddNode(new GlobalSceneActor() { ActorId = 0, Layer = 0});");

            // actors
            foreach (Actor actor in scene.AllActors())
            {
                writer.Write("AddNode(new {0}()", ActorGenerator.GetActorClassName(actor));
                writer.Write(" { ");
                writer.Write("ActorId = {0}, ", actor.Id);
                writer.Write("Layer = {0}f", ActorGenerator.GetLayerIndex(actor));
                writer.WriteLine(" });");
            }

            writer.WriteLine("InitializeActors();");
        }

        /// <summary>
        /// Generates the source code in C# for the specified scene.
        /// </summary>
        /// <param name="scene">The scene to generate the source code for.</param>
        /// <param name="textWriter">The <see cref="TextWriter"/> for writing the source code of the scene.</param>
        /// <param name="buildType">Type of the build.</param>
        public void Generate(Scene scene, TextWriter textWriter, BuildType buildType)
        {
            this.scene = scene;
            writer = textWriter;

            if (actorGenerator == null) actorGenerator = new ActorGenerator();

            // scene namespace start
            writer.WriteLine("namespace {0}", GetSceneNamespace(scene));
            writer.WriteLine("{");

            // scene class
            writer.WriteLine("class SceneLevel : GlobalContentForScene");
            writer.WriteLine("{");

            writer.WriteLine("public override void LoadContent()");
            writer.WriteLine("{");
            writer.WriteLine("base.LoadContent();");

            writer.WriteLine("FarseerPhysics.Settings.ContinuousPhysics = {0};", ActorGenerator.GetBool(Project.Singleton.Settings.ContinuousCollisionDetection));
            writer.WriteLine("ConvertUnits.SetDisplayUnitToSimUnitRatio({0}f);", Project.Singleton.Settings.SimulationUnits);
            writer.WriteLine("World.Gravity = new Vector2({0}f, {1}f);", Project.Singleton.Settings.DefaultGravity.X, Project.Singleton.Settings.DefaultGravity.Y);
            writer.WriteLine("BackgroundColor = new Color({0}, {1}, {2});", Project.Singleton.Settings.BackgroundColor.R, Project.Singleton.Settings.BackgroundColor.G, Project.Singleton.Settings.BackgroundColor.B);

            GenerateContent();

            writer.WriteLine("}");

            writer.WriteLine("}");

            // generete actor global script class
            actorGenerator.GenerateGlobalSceneActor(scene.GlobalScript, writer);

            // generate actors classes
            GenerateActors();

            // scene namespace end
            writer.WriteLine("}");
        }

        /// <summary>
        /// Generates the source code in C# for the special scene which cointains all the project content (textures and animations).
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> for writing the source code of the scene.</param>
        /// <param name="buildType">Type of the build.</param>
        public void GenerateGlobalContent(TextWriter textWriter, BuildType buildType)
        {
            this.scene = null;
            writer = textWriter;

            // scene class
            writer.WriteLine("class GlobalContentForScene : {0}", buildType == BuildType.Debug ? "DebugScreen" : "SceneScreen");
            writer.WriteLine("{");

            writer.WriteLine("public override void LoadContent()");
            writer.WriteLine("{");
            writer.WriteLine("base.LoadContent();");

            writer.WriteLine("if (GraphicsAssets == null)");
            writer.WriteLine("{");
            writer.WriteLine("_graphicsAssets = new ContentManager<IGraphicsAsset>();");

            // textures
            foreach (Texture texture in Project.Singleton.Textures)
            {
                writer.WriteLine("{");
                GenerateTextureData(texture);
                writer.WriteLine("}");
            }

            // animations
            foreach (Animation animation in Project.Singleton.Animations)
            {
                writer.WriteLine("{");
                GenerateAnimationData(animation);
                writer.WriteLine("}");
            }

            writer.WriteLine("}");

            writer.WriteLine("}");

            writer.WriteLine("}");
        }
    }
}
