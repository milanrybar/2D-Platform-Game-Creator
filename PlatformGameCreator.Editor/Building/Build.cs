/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using PlatformGameCreator.Editor.Common;

namespace PlatformGameCreator.Editor.Building
{
    /// <summary>
    /// Defines type of build of the game.
    /// </summary>
    enum BuildType
    {
        /// <summary>
        /// Debug build. For playing the game in the editor.
        /// </summary>
        Debug,

        /// <summary>
        /// Release build. For publishing the game.
        /// </summary>
        Release
    };

    /// <summary>
    /// Compile and run the compiled game defined by the current project.
    /// </summary>
    class GameBuild : Disposable
    {
        /// <summary>
        /// Application domain where is the game compiled and run.
        /// </summary>
        private AppDomain processingDomain;

        /// <summary>
        /// Game compiler for compiling and running the game in the <see cref="processingDomain"/>.
        /// </summary>
        private GameCompiler gameCompiler;

        /// <summary>
        /// Generates the source code in C# for the game.
        /// </summary>
        /// <param name="sourceFilename">The filename where to save the source code.</param>
        /// <param name="buildType">Type of the build.</param>
        /// <param name="verbose">If set to true generating will be verbose via standard <see cref="Messages"/> system.</param>
        public void GenerateSourceCode(string sourceFilename, BuildType buildType, bool verbose = false)
        {
            using (StreamWriter textWriter = new StreamWriter(sourceFilename))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                if (verbose) Messages.ShowInfo("Generating game source code.");

                GameGenerator gameGenerator = new GameGenerator();
                gameGenerator.Generate(textWriter, buildType);
            }

            if (verbose) Messages.ShowInfo("Generating game source code completed.");
        }

        /// <summary>
        /// Compiles the game from the specified source code in C#.
        /// </summary>
        /// <remarks>
        /// Debug build: the game is compiled and stored in the memory. The game can be later run by <see cref="RunGame"/> method.
        /// Release build: the game is compiled and is saved to the output directory.
        /// </remarks>
        /// <param name="sourceFilename">The filename where is to source code of the game.</param>
        /// <param name="buildType">Type of the build.</param>
        /// <param name="outputDirectory">The output directory where to save the game, if Release build.</param>
        /// <param name="verbose">If set to true building will be verbose via standard <see cref="Messages"/> system.</param>
        /// <exception cref="TargetInvocationException">Compile error.</exception>
        public void BuildGame(string sourceFilename, BuildType buildType, string outputDirectory = null, bool verbose = false)
        {
            if (processingDomain == null)
            {
                processingDomain = AppDomain.CreateDomain("CompilingGame:" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);

                Type gameCompilerType = typeof(GameCompiler);
                gameCompiler = (GameCompiler)processingDomain.CreateInstanceAndUnwrap(gameCompilerType.Assembly.FullName, gameCompilerType.FullName);
            }

            if (verbose) Messages.ShowInfo("Compiling game.");

            gameCompiler.Compile(sourceFilename, buildType, outputDirectory);

            if (verbose) Messages.ShowInfo("Compiling game completed.");
        }

        /// <summary>
        /// Runs the previously compiled game by Debug build.
        /// </summary>
        /// <exception cref="Exception">No previously compiled game.</exception>
        public void RunGame()
        {
            if (gameCompiler == null) throw new Exception("Nothing to run.");

            gameCompiler.Run();
        }

        /// <summary>
        /// Unloads all data used by this instance. (For example: compiled game)
        /// </summary>
        public void Unload()
        {
            AppDomain.Unload(processingDomain);

            processingDomain = null;
            gameCompiler = null;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unload();
            }
        }

        /// <summary>
        /// Compile and run the compiled game.
        /// Instance of this class will run in the different application domain.
        /// </summary>
        private class GameCompiler : MarshalByRefObject
        {
            private CompilerResults compilerResults;

            /// <summary>
            /// Initializes parameters for the compiler which are the same for every build type.
            /// </summary>
            /// <returns>Returns parameters for the compiler.</returns>
            private CompilerParameters InitCompilerParameters()
            {
                CompilerParameters CompilerParams = new CompilerParameters();

                CompilerParams.TreatWarningsAsErrors = true;
                CompilerParams.GenerateExecutable = true;

                CompilerParams.CompilerOptions = "/platform:x86 /target:winexe /optimize";

                // basic .net libraries
                string[] references = { "System.dll", "System.Xml.dll", "mscorlib.dll", "System.Core.dll", "System.Net.dll" };
                CompilerParams.ReferencedAssemblies.AddRange(references);

                // XNA libraries
                // Microsoft.Xna.Framework.dll
                CompilerParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Microsoft.Xna.Framework.Color)).Location);
                // Microsoft.Xna.Framework.Game.dll
                CompilerParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Microsoft.Xna.Framework.Game)).Location);
                // Microsoft.Xna.Framework.Graphics.dll
                CompilerParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch)).Location);

                // FarseerPhysics library
                CompilerParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(FarseerPhysics.Dynamics.World)).Location);
                // GameEngine library
                CompilerParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(GameEngine.Scenes.Actor)).Location);

                return CompilerParams;
            }

            /// <summary>
            /// Compiles the game from the specified source code in C#.
            /// </summary>
            /// <param name="sourceFilename">The filename of the source code.</param>
            /// <param name="buildType">Type of the build.</param>
            /// <param name="outputDirectory">The output directory where to save the game, if Release build.</param>
            /// <exception cref="Exception">Compile error.</exception>
            public void Compile(string sourceFilename, BuildType buildType, string outputDirectory = null)
            {
                CompilerParameters compilerParameters = InitCompilerParameters();

                if (buildType == BuildType.Debug)
                {
                    compilerParameters.GenerateInMemory = true;
                }
                else
                {
                    compilerParameters.GenerateInMemory = false;
                    compilerParameters.OutputAssembly = Path.Combine(outputDirectory != null ? outputDirectory : Path.GetDirectoryName(sourceFilename), "Game.exe");
                }

                CSharpCodeProvider provider = new CSharpCodeProvider();

                // compile
                compilerResults = provider.CompileAssemblyFromFile(compilerParameters, sourceFilename);

                if (compilerResults.Errors.HasErrors)
                {
                    string text = "Compile error: ";
                    foreach (CompilerError ce in compilerResults.Errors)
                    {
                        text += "\n" + ce.ToString();
                    }
                    throw new Exception(text);
                }
            }

            /// <summary>
            /// Runs the previously compiled game.
            /// </summary>
            /// <exception cref="Exception">No previously compiled game.</exception>
            public void Run()
            {
                if (compilerResults == null) throw new Exception("Nothing to run.");

                // run compiled assembly
                compilerResults.CompiledAssembly.EntryPoint.Invoke(null, new Object[] { null });
            }
        }
    }
}
