/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Assets;
using PlatformGameCreator.Editor.Assets.Sounds;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Building;
using PlatformGameCreator.Editor.Common;
using PlatformGameCreator.Editor.GameObjects.Actors;
using PlatformGameCreator.Editor.GameObjects.Paths;
using PlatformGameCreator.Editor.Scenes;
using PlatformGameCreator.Editor.Scripting;
using PlatformGameCreator.Editor.Assets.Animations;

namespace PlatformGameCreator.Editor
{
    /// <summary>
    /// Main form of the application.
    /// </summary>
    partial class EditorApplicationForm : Form
    {
        /// <summary>
        /// Gets or sets the current project. Corresponding to <see cref="Editor.Project.Singleton"/>.
        /// </summary>
        public Project Project
        {
            get
            {
                Debug.Assert(_project == Project.Singleton || _project == null, "Only one project can exist.");
                return _project;
            }
            set
            {
                Debug.Assert(value == Project.Singleton || value == null, "Only one project can exist.");

                if (_project != null)
                {
                    UnloadProject();
                }

                _project = value;

                if (_project != null)
                {
                    LoadProject();
                }
            }
        }
        private Project _project;

        /// <summary>
        /// Gets the <see cref="SceneScreen"/> singleton.
        /// </summary>
        public static SceneScreen SceneSingleton
        {
            get { return _sceneSingleton; }
        }
        private static SceneScreen _sceneSingleton;

        /// <summary>
        /// Gets the open dialog for opening a texture image.
        /// </summary>
        public OpenFileDialog TextureOpenDialog
        {
            get
            {
                if (_textureOpenDialog == null)
                {
                    _textureOpenDialog = new OpenFileDialog();
                    _textureOpenDialog.Title = "Open Image Files";
                    _textureOpenDialog.Filter = "Image Files(*.bmp;*.jpg;*.jpeg,*.gif;*.png)|*.bmp;*.jpg;*.jpeg,*.gif;*.png";
                    _textureOpenDialog.FilterIndex = 1;
                    _textureOpenDialog.RestoreDirectory = true;
                    _textureOpenDialog.Multiselect = false;
                }

                return _textureOpenDialog;
            }
        }
        private OpenFileDialog _textureOpenDialog;

        /// <summary>
        /// Gets the open dialog for opening a sound.
        /// </summary>
        public OpenFileDialog SoundOpenDialog
        {
            get
            {
                if (_soundOpenDialog == null)
                {
                    _soundOpenDialog = new OpenFileDialog();
                    _soundOpenDialog.Title = "Open Sound Files";
                    _soundOpenDialog.Filter = "Sound Files(*.wav;*.wma;*.mp3)|*.wav;*.wma;*.mp3";
                    _soundOpenDialog.FilterIndex = 1;
                    _soundOpenDialog.RestoreDirectory = true;
                    _soundOpenDialog.Multiselect = false;
                }

                return _soundOpenDialog;
            }
        }
        private OpenFileDialog _soundOpenDialog;

        /// <summary>
        /// Gets the open dialog for opening a project.
        /// </summary>
        public OpenFileDialog ProjectOpenDialog
        {
            get
            {
                if (_projectOpenDialog == null)
                {
                    _projectOpenDialog = new OpenFileDialog();
                    _projectOpenDialog.Title = "Open Project File";
                    _projectOpenDialog.Filter = "Project(*.pgcproject)|*.pgcproject";
                    _projectOpenDialog.FilterIndex = 1;
                    _projectOpenDialog.RestoreDirectory = true;
                    _projectOpenDialog.Multiselect = false;
                }

                return _projectOpenDialog;
            }
        }
        private OpenFileDialog _projectOpenDialog;

        /// <summary>
        /// Gets the save dialog for saving a project.
        /// </summary>
        public SaveFileDialog ProjectSaveDialog
        {
            get
            {
                if (_projectSaveDialog == null)
                {
                    _projectSaveDialog = new SaveFileDialog();
                    _projectSaveDialog.Title = "Save Project File";
                    _projectSaveDialog.Filter = "Project(*.pgcproject)|*.pgcproject";
                    _projectSaveDialog.FilterIndex = 1;
                    _projectSaveDialog.RestoreDirectory = true;
                }

                return _projectSaveDialog;
            }
        }
        private SaveFileDialog _projectSaveDialog;

        /// <summary>
        /// Messages manager of the form.
        /// </summary>
        private IMessagesManager messagesManager;

        /// <summary>
        /// <see cref="ProcessingForm"/> for the form.
        /// </summary>
        private ProcessingForm processingForm = new ProcessingForm();

        /// <summary>
        /// List of opened <see cref="ScriptingForm"/>.
        /// </summary>
        private List<ScriptingForm> openedScriptingForms = new List<ScriptingForm>();

        /// <summary>
        /// <see cref="ActorInfo"/> for editing the <see cref="Actor"/>.
        /// </summary>
        private ActorInfo actorInfo;

        /// <summary>
        /// <see cref="PathInfo"/> for editing the <see cref="GameObjects.Paths.Path"/>.
        /// </summary>
        private PathInfo pathInfo;

        /// <summary>
        /// Possible zoom values for the ComboBox.
        /// </summary>
        private static int[] zoomValues = { 20, 50, 70, 100, 150, 200, 400 };

        /// <summary>
        /// <see cref="SelectingNodesSceneState"/> for the <see cref="SceneScreen"/>. 
        /// </summary>
        private SelectingNodesSceneState selectingNodesSceneState;

        /// <summary>
        /// <see cref="MovingNodesSceneState"/> for the <see cref="SceneScreen"/>. 
        /// </summary>
        private MovingNodesSceneState movingNodesSceneState;

        /// <summary>
        /// <see cref="RotatingNodesSceneState"/> for the <see cref="SceneScreen"/>. 
        /// </summary>
        private RotatingNodesSceneState rotatingNodesSceneState;

        /// <summary>
        /// <see cref="ScalingNodesSceneState"/> for the <see cref="SceneScreen"/>. 
        /// </summary>
        private ScalingNodesSceneState scalingNodesSceneState;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorApplicationForm"/> class.
        /// </summary>
        public EditorApplicationForm()
        {
            InitializeComponent();

            Icon = Properties.Resources._2DPGC_Logo;

            // init messages manager
            messagesManager = new DefaultMessagesManager(toolStripStatusLabel);
            Messages.MessagesManager = messagesManager;

            sceneScreenControl.ZoomChanged += new EventHandler(sceneScreenControl_ZoomChanged);

            // init zoom box values
            foreach (int zoomValue in zoomValues)
            {
                zoomBox.Items.Add(zoomValue + " %");
            }
            zoomBox.SelectedIndex = 3;

            sceneScreenControl.SelectedNodesChanged += new EventHandler(sceneScreenControl_SelectedNodesChanged);

            // set scene screen control singleton
            _sceneSingleton = sceneScreenControl;

            sceneScreenControl.ShowGrid = Properties.Settings.Default.SceneEditor_ShowGrid;
            sceneScreenControl.ShowShapes = Properties.Settings.Default.SceneEditor_ShowShapes;
            sceneScreenControl.GridGapSize = Properties.Settings.Default.SceneEditor_GridGapSize;

            showGridButton.Checked = sceneScreenControl.ShowGrid;
            showShapesButton.Checked = sceneScreenControl.ShowShapes;
            gridGapSizenumericUpDown.Value = sceneScreenControl.GridGapSize;
        }

        /// <summary>
        /// Opens the visual scripting editor (<see cref="ScriptingForm"/>) for the specified scripting.
        /// </summary>
        /// <param name="scriptingComponent">The scripting component.</param>
        public void OpenScriptingForm(ScriptingComponent scriptingComponent)
        {
            // check if the scripting form is alredy opened
            ScriptingForm scriptingForm = null;
            foreach (ScriptingForm openedScriptingForm in openedScriptingForms)
            {
                if (openedScriptingForm.ScriptingComponent == scriptingComponent)
                {
                    scriptingForm = openedScriptingForm;
                    break;
                }
            }

            // the scripting form is already opened
            if (scriptingForm != null)
            {
                scriptingForm.Select();
            }
            // create new scripting form 
            else
            {
                scriptingForm = new ScriptingForm(scriptingComponent);
                scriptingForm.FormClosed += new FormClosedEventHandler(ScriptingForm_FormClosed);
                openedScriptingForms.Add(scriptingForm);
                scriptingForm.Show();
            }
        }

        /// <summary>
        /// Begins editing the specified path at the <see cref="SceneScreen"/>, is possible.
        /// </summary>
        /// <param name="path">The path to edit.</param>
        public void BeginEditPath(PlatformGameCreator.Editor.GameObjects.Paths.Path path)
        {
            PathView pathView = sceneScreenControl.Find(path) as PathView;

            if (pathView != null)
            {
                if (sceneScreenControl.State.CanBeInterrupted)
                {
                    if (sceneScreenControl.State.CanBeInStack)
                    {
                        sceneScreenControl.State = new EditingPathSceneState(pathView, sceneScreenControl.State);
                    }
                }
                else
                {
                    sceneScreenControl.State.OnTryInterrupt();
                }
            }
        }

        /// <summary>
        /// Handles the FormClosed event of the ScriptingForm.
        /// Removes the specified <see cref="ScriptingForm"/> from the list of opened scripting forms.
        /// </summary>
        private void ScriptingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            openedScriptingForms.Remove(sender as ScriptingForm);
        }

        /// <summary>
        /// Closes the visual scripting editor (<see cref="ScriptingForm"/>) for the specified scripting.
        /// </summary>
        /// <param name="scriptingComponent">The scripting component.</param>
        public void CloseScriptingForm(ScriptingComponent scriptingComponent)
        {
            foreach (ScriptingForm openedScriptingForm in openedScriptingForms)
            {
                if (openedScriptingForm.ScriptingComponent == scriptingComponent)
                {
                    openedScriptingForm.Close();
                    break;
                }
            }
        }

        /// <summary>
        /// Underlying collection changes.
        /// Informs the <see cref="SceneScreen"/> and opened <see cref="ScriptingForm"/> about the change.
        /// </summary>
        public void UnderlyingDataChange()
        {
            sceneScreenControl.UnderlyingCollectionChange();

            foreach (ScriptingForm openedScriptingForm in openedScriptingForms)
            {
                openedScriptingForm.UnderlyingCollectionChange();
            }
        }

        /// <summary>
        /// Handles the SelectedNodesChanged event of the sceneScreenControl control.
        /// Updates the Properties tab. If one scene node is selected then we set the its editing control to the Properties tab.
        /// </summary>
        private void sceneScreenControl_SelectedNodesChanged(object sender, EventArgs e)
        {
            // actor
            if (sceneScreenControl.SelectedNodes.Count == 1 && sceneScreenControl.SelectedNodes[0] is ActorView)
            {
                if (actorInfo == null)
                {
                    actorInfo = new ActorInfo();
                    actorInfo.Dock = DockStyle.Fill;
                }

                AddControlToPropertiesTabPage(actorInfo);

                actorInfo.Actor = ((ActorView)sceneScreenControl.SelectedNodes[0]).Actor;
            }
            // path
            else if (sceneScreenControl.SelectedNodes.Count == 1 && sceneScreenControl.SelectedNodes[0] is PathView)
            {
                if (pathInfo == null)
                {
                    pathInfo = new PathInfo();
                    pathInfo.Dock = DockStyle.Fill;
                }

                AddControlToPropertiesTabPage(pathInfo);

                pathInfo.Path = ((PathView)sceneScreenControl.SelectedNodes[0]).Path;
            }
            else
            {
                AddControlToPropertiesTabPage(null);
            }
        }

        /// <summary>
        /// Updates the Properties tab.
        /// Clears the controls from the Properties tab and adds the specified control to it.
        /// </summary>
        /// <param name="selectedNodeInfo">The control to add.</param>
        private void AddControlToPropertiesTabPage(Control selectedNodeInfo)
        {
            // clear previous controls if any
            if (propertiesTabPage.Controls.Count != 0)
            {
                propertiesTabPage.Controls.Clear();
            }

            // add selected node control if any
            if (selectedNodeInfo != null)
            {
                propertiesTabPage.Controls.Add(selectedNodeInfo);
            }
        }

        /// <summary>
        /// Handles the ZoomChanged event of the sceneScreenControl control.
        /// Updates the zoom value in the zoomBox.
        /// </summary>
        private void sceneScreenControl_ZoomChanged(object sender, EventArgs e)
        {
            SetZoomValue();
        }

        /// <summary>
        /// Updates the zoom value in the zoomBox.
        /// </summary>
        private void SetZoomValue()
        {
            zoomBox.Text = String.Format("{0} %", sceneScreenControl.Zoom);
        }

        /// <summary>
        /// Shows the <see cref="IntroForm"/> and if the user do not select any project then quit the application.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (new IntroForm().ShowDialog() != DialogResult.OK)
            {
                Close();
            }
        }

        /// <summary>
        /// Called when the new project is set.
        /// Sets GUI specific settings for the project.
        /// </summary>
        private void LoadProject()
        {
            if (Project == null) return;

            texturesView.DrawableAssets = Project.Textures;
            animationsView.DrawableAssets = Project.Animations;
            soundsView.Sounds = Project.Sounds;
            prototypesView.Prototypes = Project.Prototypes;
            actorTypesView.ActorTypes = Project.ActorTypes;
            scenesView.Scenes = Project.Scenes;

            Project.Scenes.SelectedSceneChanged += new ValueChangedHandler<Scene>(Scenes_SelectedSceneChanged);

            LoadScene();

            Properties.Settings.Default.EditorApplication_LastOpenedProject = Project.ProjectFilename;
        }

        /// <summary>
        /// Called when the project is unset.
        /// Unsets GUI specific settings for the project.
        /// </summary>
        private void UnloadProject()
        {
            texturesView.DrawableAssets = null;
            animationsView.DrawableAssets = null;
            soundsView.Sounds = null;
            prototypesView.Prototypes = null;
            actorTypesView.ActorTypes = null;
            scenesView.Scenes = null;

            if (actorInfo != null)
            {
                actorInfo.Dispose();
                actorInfo = null;
            }
            if (pathInfo != null)
            {
                pathInfo.Dispose();
                pathInfo = null;
            }

            if (Project != null)
            {
                Project.Scenes.SelectedSceneChanged -= new ValueChangedHandler<Scene>(Scenes_SelectedSceneChanged);

                UnloadScene(Project.Scenes.SelectedScene);
            }
        }

        /// <summary>
        /// Closes all opened <see cref="ScriptingForm"/>.
        /// </summary>
        private void CloseAllScriptingForms()
        {
            while (openedScriptingForms.Count != 0)
            {
                Debug.Assert(!openedScriptingForms[0].IsDisposed, "ScriptingForm already disposed.");
                openedScriptingForms[0].Close();
            }
        }

        /// <summary>
        /// Called when the new scene is set.
        /// Sets GUI specific settings for the scene.
        /// </summary>
        private void LoadScene()
        {
            foreach (Layer layer in Project.Scenes.SelectedScene.Layers)
            {
                AddLayerToSelectedLayerComboBox(layer);
            }
            selectedLayerComboBox.SelectedItem = Project.Scenes.SelectedScene.SelectedLayer;

            Project.Scenes.SelectedScene.Layers.ListChanged += new ObservableList<Layer>.ListChangedEventHandler(Layers_ListChanged);
            Project.Scenes.SelectedScene.SelectedLayerChanged += new EventHandler(Scene_SelectedLayerChanged);

            sceneTreeView.Scene = Project.Scenes.SelectedScene;
            sceneScreenControl.Scene = Project.Scenes.SelectedScene;
        }

        /// <summary>
        /// Called when the scene is unset.
        /// Unsets GUI specific settings for the scene.
        /// </summary>
        private void UnloadScene(Scene scene)
        {
            CloseAllScriptingForms();

            sceneTreeView.Scene = null;
            sceneScreenControl.Scene = null;

            if (scene != null)
            {
                scene.Layers.ListChanged -= new ObservableList<Layer>.ListChangedEventHandler(Layers_ListChanged);
                scene.SelectedLayerChanged -= new EventHandler(Scene_SelectedLayerChanged);
            }
            selectedLayerComboBox.Items.Clear();
        }

        /// <summary>
        /// Handles the SelectedSceneChanged event of the ScenesManager.
        /// Changes the scene.
        /// </summary>
        private void Scenes_SelectedSceneChanged(object sender, ValueChangedEventArgs<Scene> e)
        {
            if (e.OldValue != null)
            {
                UnloadScene(e.OldValue);
            }

            if (Project.Scenes.SelectedScene != null)
            {
                LoadScene();
            }
        }

        /// <summary>
        /// Imports the texture specified by the filename into the project.
        /// </summary>
        /// <param name="filename">The filename of the texture.</param>
        private void ImportTexture(string filename)
        {
            // import texture
            processingForm.Process(filename, ImportTextureDoWork, ImportTextureCompleted);
            // set default messages manager
            Messages.MessagesManager = messagesManager;
        }

        /// <summary>
        /// Operation of importing texture into the project.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void ImportTextureDoWork(IProcessingContainer container, DoWorkEventArgs e)
        {
            container.Title = "Importing Texture";
            container.CurrentTask = "Importing texture";
            container.ProgressPercentage = 30;

            e.Result = Project.Textures.Create((string)e.Argument, true);
        }

        /// <summary>
        /// Called after the operation of importing texture into the project is completed.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void ImportTextureCompleted(IProcessingContainer container, RunWorkerCompletedEventArgs e)
        {
            Texture texture = e.Result as Texture;

            if (texture != null)
            {
                container.CurrentTask = "Loading texture completed";
                container.ProgressPercentage = 100;

                Project.Textures.Add(texture);
            }
            else
            {
                container.CurrentTask = "Error occured";
            }
        }

        /// <summary>
        /// Handles the Click event of the addTextureButton control.
        /// Imports the specified texture into the project.
        /// </summary>
        private void addTextureButton_Click(object sender, EventArgs e)
        {
            if (TextureOpenDialog.ShowDialog() == DialogResult.OK)
            {
                ImportTexture(TextureOpenDialog.FileName);
            }
        }

        /// <summary>
        /// Handles the Click event of the sceneScreenUndoButton control.
        /// Undoes the history of the <see cref="SceneScreen"/>.
        /// </summary>
        private void sceneScreenUndoButton_Click(object sender, EventArgs e)
        {
            sceneScreenControl.History.Undo();
        }

        /// <summary>
        /// Handles the Click event of the sceneScreenRedoButton control.
        /// Redoes the history of the <see cref="SceneScreen"/>.
        /// </summary>
        private void sceneScreenRedoButton_Click(object sender, EventArgs e)
        {
            sceneScreenControl.History.Redo();
        }

        /// <summary>
        /// Handles the FormClosed event of the EditorApplicationForm control.
        /// Saves the settings.
        /// </summary>
        private void EditorApplicationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // save settings
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Handles the Click event of the sceneScreenSelectingState control.
        /// Sets the <see cref="SelectingNodesSceneState"/> state to the <see cref="SceneScreen"/>.
        /// </summary>
        private void sceneScreenSelectingState_Click(object sender, EventArgs e)
        {
            if (selectingNodesSceneState == null)
            {
                selectingNodesSceneState = new SelectingNodesSceneState() { Screen = sceneScreenControl };
            }
            sceneScreenControl.State = selectingNodesSceneState;
        }

        /// <summary>
        /// Handles the Click event of the sceneScreenMovingState control.
        /// Sets the <see cref="MovingNodesSceneState"/> state to the <see cref="SceneScreen"/>.
        /// </summary>
        private void sceneScreenMovingState_Click(object sender, EventArgs e)
        {
            if (movingNodesSceneState == null)
            {
                movingNodesSceneState = new MovingNodesSceneState() { Screen = sceneScreenControl };
            }
            sceneScreenControl.State = movingNodesSceneState;
        }

        /// <summary>
        /// Handles the Click event of the sceneScreenRotatingState control.
        /// Sets the <see cref="RotatingNodesSceneState"/> state to the <see cref="SceneScreen"/>.
        /// </summary>
        private void sceneScreenRotatingState_Click(object sender, EventArgs e)
        {
            if (rotatingNodesSceneState == null)
            {
                rotatingNodesSceneState = new RotatingNodesSceneState() { Screen = sceneScreenControl };
            }
            sceneScreenControl.State = rotatingNodesSceneState;
        }

        /// <summary>
        /// Handles the Click event of the sceneScreenScalingState control.
        /// Sets the <see cref="ScalingNodesSceneState"/> state to the <see cref="SceneScreen"/>.
        /// </summary>
        private void sceneScreenScalingState_Click(object sender, EventArgs e)
        {
            if (scalingNodesSceneState == null)
            {
                scalingNodesSceneState = new ScalingNodesSceneState() { Screen = sceneScreenControl };
            }
            sceneScreenControl.State = scalingNodesSceneState;
        }

        /// <summary>
        /// Handles the Click event of the runGameButton control.
        /// Compiles and runs the game.
        /// </summary>
        private void runGameButton_Click(object sender, EventArgs e)
        {
            CompileAndRunGame();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the zoomBox control.
        /// Zooms the <see cref="SceneScreen"/> by the selected zoom value.
        /// </summary>
        private void zoomBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (zoomBox.SelectedIndex != -1 && sceneScreenControl.Zoom != zoomValues[zoomBox.SelectedIndex])
            {
                ChangeSceneZoom(zoomValues[zoomBox.SelectedIndex]);
            }
        }

        /// <summary>
        /// Handles the Validated event of the zoomBox control.
        /// Validates the zoom value and if valid then zooms the <see cref="SceneScreen"/> by the selected zoom value.
        /// </summary>
        private void zoomBox_Validated(object sender, EventArgs e)
        {
            int zoomValue;
            if (int.TryParse(zoomBox.Text, out zoomValue) && zoomValue >= 0)
            {
                ChangeSceneZoom(zoomValue);
            }
            else
            {
                SetZoomValue();
            }
        }

        /// <summary>
        /// Zooms the <see cref="SceneScreen"/> by the specified zoom value.
        /// </summary>
        /// <param name="zoom">The zoom for the <see cref="SceneScreen"/>.</param>
        private void ChangeSceneZoom(int zoom)
        {
            PointF centerScene = new PointF(sceneScreenControl.Width / 2f, sceneScreenControl.Height / 2f);

            // actual position under mouse cursor
            Microsoft.Xna.Framework.Vector2 actualPosition = sceneScreenControl.PointAtScene(centerScene);

            // zoom the scene
            sceneScreenControl.Zoom = zoom;

            // new positon under mouse cursor
            Microsoft.Xna.Framework.Vector2 changedPosition = sceneScreenControl.PointAtScene(centerScene);

            // We want to keep the same position under mouse cursor before and after changing zoom of the scene
            // so we change the position of the scene by difference between before and after position under mouse cursor.
            sceneScreenControl.Position += (actualPosition - changedPosition);
        }

        /// <summary>
        /// Handles the Activated event of the EditorApplicationForm form.
        /// Activates the loop for updating the <see cref="SceneScreen"/>.
        /// </summary>
        private void EditorApplicationForm_Activated(object sender, EventArgs e)
        {
            Messages.MessagesManager = messagesManager;
            sceneScreenControl.LoopActive = true;
        }

        /// <summary>
        /// Handles the Deactivate event of the EditorApplicationForm form.
        /// Deactivates the loop for not updating the <see cref="SceneScreen"/>.
        /// </summary>
        private void EditorApplicationForm_Deactivate(object sender, EventArgs e)
        {
            sceneScreenControl.LoopActive = false;
        }

        /// <summary>
        /// Opens scene visual scripting editor.
        /// </summary>
        private void scriptingButton_Click(object sender, EventArgs e)
        {
            OpenScriptingForm(Project.Scenes.SelectedScene.GlobalScript);
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// Saves the project.
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project != null)
            {
                Project.Save();

                Messages.ShowInfo("Project saved");
            }
        }

        /// <summary>
        /// Handles the Click event of the openToolStripMenuItem control.
        /// Opens the project.
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();

            if (Project == null) Close();
        }

        /// <summary>
        /// Handles the Click event of the newToolStripMenuItem control.
        /// Creates the new project.
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewProject();
        }

        /// <summary>
        /// Creates the new project.
        /// </summary>
        public void CreateNewProject()
        {
            if (ProjectSaveDialog.ShowDialog() == DialogResult.OK)
            {
                Project = null;

                Project.CreateProject(System.IO.Path.GetDirectoryName(ProjectSaveDialog.FileName), System.IO.Path.GetFileNameWithoutExtension(ProjectSaveDialog.FileName));

                // create default scene
                Scene defaultScene = new Scene();
                defaultScene.Name = "Default";
                Project.Singleton.Scenes.Add(defaultScene);
                Project.Singleton.Scenes.SelectedScene = defaultScene;

                Project = Project.Singleton;

                // copy needed content
                string editorContentDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Content");

                // Fonts
                CopyContent("DefaultFont.xnb", editorContentDirectory, Project.ContentDirectory);
                CopyContent("LargeFont.xnb", editorContentDirectory, Project.ContentDirectory);
                // PixelCircle
                CopyContent("PixelCircle.xnb", editorContentDirectory, Project.ContentDirectory);

                // save project
                Project.Save();
            }
        }

        /// <summary>
        /// Opens the project.
        /// </summary>
        public void OpenProject()
        {
            if (ProjectOpenDialog.ShowDialog() == DialogResult.OK)
            {
                OpenProject(ProjectOpenDialog.FileName);
            }
        }

        /// <summary>
        /// Opens the project by the specified filename of the project.
        /// </summary>
        /// <param name="projectFilename">The project filename.</param>
        public void OpenProject(string projectFilename)
        {
            Project = null;

            Project.Load(projectFilename);

            Project = Project.Singleton;
        }

        /// <summary>
        /// Handles the Click event of the addSoundButton control.
        /// Imports the specified sound into the project.
        /// </summary>
        private void addSoundButton_Click(object sender, EventArgs e)
        {
            if (SoundOpenDialog.ShowDialog() == DialogResult.OK)
            {
                ImportSound(SoundOpenDialog.FileName);
            }
        }

        /// <summary>
        /// Imports the sound specified by the filename into the project.
        /// </summary>
        /// <param name="filename">The filename of the texture.</param>
        private void ImportSound(string filename)
        {
            // import sound
            processingForm.Process(filename, ImportSoundDoWork, ImportSoundCompleted);
            // set default messages manager
            Messages.MessagesManager = messagesManager;
        }

        /// <summary>
        /// Operation of importing sound into the project.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void ImportSoundDoWork(IProcessingContainer container, DoWorkEventArgs e)
        {
            container.Title = "Importing Sound";
            container.CurrentTask = "Importing sound";
            container.ProgressPercentage = 50;

            e.Result = Project.Sounds.Create((string)e.Argument, true);
        }

        /// <summary>
        /// Called after the operation of importing sound into the project is completed.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void ImportSoundCompleted(IProcessingContainer container, RunWorkerCompletedEventArgs e)
        {
            Sound sound = e.Result as Sound;

            if (sound != null)
            {
                container.CurrentTask = "Loading sound completed";
                container.ProgressPercentage = 100;

                Project.Sounds.Add(sound);
            }
            else
            {
                container.CurrentTask = "Error occured";
            }
        }

        /// <summary>
        /// Handles the Click event of the addAnimationButton control.
        /// Creates the new animation in the project.
        /// </summary>
        private void addAnimationButton_Click(object sender, EventArgs e)
        {
            Animation anim = new Animation();
            anim.Name = "New Animation";

            new AnimationForm(anim).ShowDialog();

            if (anim.Frames.Count > 0)
            {
                Project.Animations.Add(anim);
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the gridGapSizenumericUpDown control.
        /// Updates the <see cref="SceneScreen.GridGapSize"/> property.
        /// </summary>
        private void gridGapSizenumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            sceneScreenControl.GridGapSize = (int)gridGapSizenumericUpDown.Value;
            Properties.Settings.Default.SceneEditor_GridGapSize = sceneScreenControl.GridGapSize;
        }

        /// <summary>
        /// Handles the KeyUp event of the zoomBox control.
        /// Zooms the <see cref="SceneScreen"/> by the selected zoom value.
        /// </summary>
        private void zoomBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                zoomBox_Validated(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the selected layer of the scene changes.
        /// Updates the seleted layer in the selectedLayerComboBox control. 
        /// </summary>
        private void Scene_SelectedLayerChanged(object sender, EventArgs e)
        {
            if (selectedLayerComboBox.SelectedItem != Project.Scenes.SelectedScene.SelectedLayer)
            {
                selectedLayerComboBox.SelectedItem = Project.Scenes.SelectedScene.SelectedLayer;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the selectedLayerComboBox control.
        /// Selects the specified layer as the selected layer of the scene.
        /// </summary>
        private void selectedLayerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedLayerComboBox.SelectedItem != null)
            {
                Project.Scenes.SelectedScene.SelectedLayer = selectedLayerComboBox.SelectedItem as Layer;
            }
        }

        /// <summary>
        /// Adds the specified layer to the selectedLayerComboBox control.
        /// </summary>
        /// <param name="layer">The layer to add.</param>
        /// <param name="index">The index where to add the layer. If -1 then adds the layer to the end.</param>
        private void AddLayerToSelectedLayerComboBox(Layer layer, int index = -1)
        {
            if (index == -1) selectedLayerComboBox.Items.Add(layer);
            else selectedLayerComboBox.Items.Insert(index, layer);
            layer.NameChanged += new EventHandler(Layer_NameChanged);
        }

        /// <summary>
        /// Removes the specified layer from the selectedLayerComboBox control.
        /// </summary>
        /// <param name="layer">The layer to remove.</param>
        private void RemoveLayerFromSelectedLayerComboBox(Layer layer)
        {
            selectedLayerComboBox.Items.Remove(layer);
            layer.NameChanged -= new EventHandler(Layer_NameChanged);
        }

        /// <summary>
        /// Removes all items (layers) from the selectedLayerComboBox control.
        /// </summary>
        private void ClearSelectedLayerComboBox()
        {
            foreach (Layer layer in selectedLayerComboBox.Items)
            {
                layer.NameChanged -= new EventHandler(Layer_NameChanged);
            }
            selectedLayerComboBox.Items.Clear();
        }

        /// <summary>
        /// Called when the layers of the scene changes (layer is added or removed).
        /// Updates the layers in the selectedLayerComboBox control. 
        /// </summary>
        private void Layers_ListChanged(object sender, ObservableListChangedEventArgs<Layer> e)
        {
            switch (e.ListChangedType)
            {
                case ObservableListChangedType.ItemAdded:
                    AddLayerToSelectedLayerComboBox(e.Item, e.Index);
                    break;

                case ObservableListChangedType.ItemDeleted:
                    RemoveLayerFromSelectedLayerComboBox(e.Item);
                    if (selectedLayerComboBox.SelectedItem == null && selectedLayerComboBox.Items.Count > 0)
                    {
                        selectedLayerComboBox.SelectedItem = selectedLayerComboBox.Items[0];
                    }
                    break;

                case ObservableListChangedType.ItemChanged:
                    Debug.Assert(true, "Not supported");
                    break;

                case ObservableListChangedType.Reset:
                    ClearSelectedLayerComboBox();
                    foreach (Layer layer in Project.Scenes.SelectedScene.Layers)
                    {
                        AddLayerToSelectedLayerComboBox(layer);
                    }
                    break;
            }
        }

        /// <summary>
        /// Called when the name of the layer changes.
        /// Updates the name of the specified layer in the selectedLayerComboBox control. 
        /// </summary>
        void Layer_NameChanged(object sender, EventArgs e)
        {
            Layer layer = sender as Layer;
            if (layer != null)
            {
                int index = selectedLayerComboBox.Items.IndexOf(layer);
                selectedLayerComboBox.Items[index] = layer;
            }
        }

        /// <summary>
        /// Handles the Click event of the fitSceneButton control.
        /// Zooms and centers the <see cref="SceneScreen"/>.
        /// </summary>
        private void fitSceneButton_Click(object sender, EventArgs e)
        {
            sceneScreenControl.Fit();
        }

        /// <summary>
        /// Handles the Click event of the addPathToolStripButton control.
        /// Sets the <see cref="AddPathSceneState"/> state to the <see cref="SceneScreen"/> for adding new path to the scene.
        /// </summary>
        private void addPathToolStripButton_Click(object sender, EventArgs e)
        {
            if (sceneScreenControl.State.CanBeInterrupted)
            {
                if (sceneScreenControl.State.CanBeInStack)
                {
                    sceneScreenControl.State = new AddPathSceneState(sceneScreenControl.State);
                }
            }
            else
            {
                sceneScreenControl.State.OnTryInterrupt();
            }
        }

        /// <summary>
        /// Handles the Click event of the showGridButton control.
        /// Updates the <see cref="SceneScreen.ShowGrid"/> property.
        /// </summary>
        private void showGridButton_Click(object sender, EventArgs e)
        {
            sceneScreenControl.ShowGrid = showGridButton.Checked;
            Properties.Settings.Default.SceneEditor_ShowGrid = showGridButton.Checked;
        }

        /// <summary>
        /// Compiles and runs the game defined by the project.
        /// </summary>
        private void CompileAndRunGame()
        {
            if (!sceneScreenControl.State.CanBeInterrupted)
            {
                sceneScreenControl.State.OnTryInterrupt();
            }
            else
            {
                // compile and run game
                processingForm.Process(null, CompileAndRunGameDoWork, CompileAndRunGameCompleted);
                // set default messages manager
                Messages.MessagesManager = messagesManager;
            }
        }

        /// <summary>
        /// Operation of generating the source code and compaling the game defined by the project.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void CompileAndRunGameDoWork(IProcessingContainer container, DoWorkEventArgs e)
        {
            container.Title = "Building Game";
            container.CurrentTask = "Preparing to build";
            container.ProgressPercentage = 0;

            // build sounds if needed
            if (!Project.Sounds.BuildSounds(true)) return;

            container.ProgressPercentage = 5;

            string sourceFilename = System.IO.Path.Combine(Project.ProjectDirectory, Project.Name + ".cs");

            GameBuild build = new GameBuild();

            try
            {
                container.CurrentTask = "Generating game source code";
                build.GenerateSourceCode(sourceFilename, BuildType.Debug, true);

                container.ProgressPercentage = 50;

                container.CurrentTask = "Compiling game";
                build.BuildGame(sourceFilename, BuildType.Debug, null, true);

                e.Result = build;
            }
            catch (TargetInvocationException exception)
            {
                Messages.ShowError(exception.InnerException != null ? exception.InnerException.Message : exception.Message);

                build.Dispose();
            }
            catch (Exception exception)
            {
                Messages.ShowError(exception.Message);

                build.Dispose();
            }
        }

        /// <summary>
        /// Called after the operation of compaling the game defined by the project is completed.
        /// If the compaling is successful then runs the compiled game.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void CompileAndRunGameCompleted(IProcessingContainer container, RunWorkerCompletedEventArgs e)
        {
            GameBuild build = e.Result as GameBuild;

            if (build != null)
            {
                container.CurrentTask = "Running game";
                container.ProgressPercentage = 100;

                // hide all opened scripting forms
                foreach (ScriptingForm openedScriptingForm in openedScriptingForms)
                {
                    openedScriptingForm.Hide();
                }

                // hide editor
                Hide();

                // hide processing dialog
                processingForm.Hide();

                try
                {
                    // run game
                    build.RunGame();
                }
                catch (TargetInvocationException exception)
                {
                    Messages.ShowError(exception.InnerException != null ? exception.InnerException.Message : exception.Message);
                }
                catch (Exception exception)
                {
                    Messages.ShowError(exception.Message);
                }

                // show cursor, XNA window could hide cursor
                Cursor.Show();

                // show processing dialog
                processingForm.Show();

                // show editor
                Show();

                // show all opened scripting forms
                foreach (ScriptingForm openedScriptingForm in openedScriptingForms)
                {
                    openedScriptingForm.Show();
                }

                build.Dispose();
            }
            else
            {
                container.CurrentTask = "Error occured";
            }
        }

        /// <summary>
        /// Publishes the game defined by the project
        /// </summary>
        private void PublishGame()
        {
            if (!sceneScreenControl.State.CanBeInterrupted)
            {
                sceneScreenControl.State.OnTryInterrupt();
            }
            else
            {
                // publish game game
                processingForm.Process(null, PublishGameDoWork, PublishGameCompleted);
                // set default messages manager
                Messages.MessagesManager = messagesManager;
            }
        }

        /// <summary>
        /// Operation of publishing the game defined by the project.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void PublishGameDoWork(IProcessingContainer container, DoWorkEventArgs e)
        {
            container.Title = "Publishing Game";
            container.CurrentTask = "Preparing to build";
            container.ProgressPercentage = 0;

            // build sounds if needed
            if (!Project.Sounds.BuildSounds(true)) return;

            container.ProgressPercentage = 5;

            string sourceFilename = System.IO.Path.Combine(Project.ProjectDirectory, Project.Name + ".cs");

            try
            {
                string publishDirectory = System.IO.Path.Combine(Project.ProjectDirectory, "Publish");

                if (Directory.Exists(publishDirectory))
                {
                    try
                    {
                        // delete old publish directory and all its content
                        Directory.Delete(publishDirectory, true);
                    }
                    catch (Exception)
                    {
                        Messages.ShowError("Unable to delete old publish directory. Directory: " + publishDirectory);
                        return;
                    }
                }

                try
                {
                    // create publish directory
                    Directory.CreateDirectory(publishDirectory);
                }
                catch (Exception)
                {
                    Messages.ShowError("Unable to create publish directory. Directory: " + publishDirectory);
                    return;
                }

                using (GameBuild build = new GameBuild())
                {
                    container.CurrentTask = "Generating game source code";
                    build.GenerateSourceCode(sourceFilename, BuildType.Release, true);

                    container.ProgressPercentage = 50;

                    container.CurrentTask = "Compiling game";
                    build.BuildGame(sourceFilename, BuildType.Release, publishDirectory, true);

                    container.ProgressPercentage = 90;

                    container.CurrentTask = "Copying content";

                    // copy needed library
                    // FarseerPhysics library
                    CopyLibrary(publishDirectory, typeof(FarseerPhysics.Dynamics.World));
                    // GameEngine library
                    CopyLibrary(publishDirectory, typeof(GameEngine.Scenes.Actor));

                    // create publish content directory
                    string publishContentDirectory = System.IO.Path.Combine(publishDirectory, "Content");
                    Directory.CreateDirectory(publishContentDirectory);

                    // copy content
                    Project.Textures.CopyContent(publishContentDirectory);
                    Project.Sounds.CopyContent(publishContentDirectory);

                    // copy other needed content
                    string editorContentDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Content");
                    // fonts
                    CopyContent("DefaultFont.xnb", editorContentDirectory, publishContentDirectory);
                    CopyContent("LargeFont.xnb", editorContentDirectory, publishContentDirectory);
                }

                e.Result = publishDirectory;
            }
            catch (Exception exception)
            {
                Messages.ShowError(exception.Message);
            }
        }

        /// <summary>
        /// Copies the library defined by the specified type to the specified directory.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        /// <param name="type">The type that the library contains.</param>
        private void CopyLibrary(string outputDirectory, Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            string outputLibraryPath = System.IO.Path.Combine(outputDirectory, System.IO.Path.GetFileName(assembly.Location));

            if (!File.Exists(outputLibraryPath))
            {
                File.Copy(assembly.Location, outputLibraryPath);
            }
        }

        /// <summary>
        /// Copies the file defined by the specified name and source directory to the destination directory.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        private void CopyContent(string name, string sourceDirectory, string destinationDirectory)
        {
            string destinationFilename = System.IO.Path.Combine(destinationDirectory, name);

            if (!File.Exists(destinationFilename))
            {
                File.Copy(System.IO.Path.Combine(sourceDirectory, name), destinationFilename);
            }
        }

        /// <summary>
        /// Called after the operation of publishing the game defined by the project is completed.
        /// If the publishing is successful then opens the publish directory in the explorer.
        /// Used by <see cref="ProcessingForm"/>.
        /// </summary>
        private void PublishGameCompleted(IProcessingContainer container, RunWorkerCompletedEventArgs e)
        {
            string publishDirectory = (string)e.Result;

            if (!String.IsNullOrEmpty(publishDirectory))
            {
                container.CurrentTask = "Publishing game completed";
                container.ProgressPercentage = 100;

                // open publish directory in explorer
                Process.Start(publishDirectory);
            }
            else
            {
                container.CurrentTask = "Error occured";
            }
        }

        /// <summary>
        /// Handles the Click event of the runToolStripMenuItem control.
        /// Compiles and runs the game defined by the project.
        /// </summary>
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompileAndRunGame();
        }

        /// <summary>
        /// Handles the Click event of the runToolStripMenuItem control.
        /// Publishes the game defined by the project.
        /// </summary>
        private void publishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PublishGame();
        }

        /// <summary>
        /// Handles the FormClosing event of the EditorApplicationForm control.
        /// Asks the user if the project should be saved.
        /// </summary>
        private void EditorApplicationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            if (Project == null) return;

            if (!sceneScreenControl.State.CanBeInterrupted)
            {
                e.Cancel = true;
                sceneScreenControl.State.OnTryInterrupt();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save project?", "Exit", MessageBoxButtons.YesNoCancel);

                // save project
                if (dialogResult == DialogResult.Yes)
                {
                    Project.Save();
                }
                else if (dialogResult != DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the aboutToolStripMenuItem control.
        /// Shows the <see cref="AboutBox"/> form.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Project = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Handles the Click event of the settingsToolStripMenuItem control.
        /// Opens the editor for the project settings.
        /// </summary>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GameSettingsForm() { Settings = Project.Settings }.ShowDialog();
            sceneScreenControl.UpdateSimulationUnitsMeasure();
        }

        /// <summary>
        /// Handles the Click event of the showShapesButton control.
        /// Updates the <see cref="SceneScreen.ShowShapes"/> property.
        /// </summary>
        private void showShapesButton_Click(object sender, EventArgs e)
        {
            sceneScreenControl.ShowShapes = showShapesButton.Checked;
            Properties.Settings.Default.SceneEditor_ShowShapes = showShapesButton.Checked;
        }

        /// <summary>
        /// Handles the Click event of the newSceneButton control.
        /// Creates and adds the new scene to the project.
        /// </summary>
        private void newSceneButton_Click(object sender, EventArgs e)
        {
            Project.Scenes.Add(new Scene() { Name = "New Scene" });
        }

        /// <summary>
        /// Handles the Click event of the newLayerToolStripButton control.
        /// Creates and adds the new layer to the current scene.
        /// </summary>
        private void newLayerToolStripButton_Click(object sender, EventArgs e)
        {
            Project.Scenes.SelectedScene.Layers.Insert(0, new Layer(Project.Scenes.SelectedScene) { Name = "New Layer" });
        }

        /// <summary>
        /// Handles the Click event of the newLayerParallaxToolStripButton control.
        /// Creates and adds the new parallax layer to the current scene.
        /// </summary>
        private void newLayerParallaxToolStripButton_Click(object sender, EventArgs e)
        {
            Project.Scenes.SelectedScene.Layers.Insert(0, new Layer(Project.Scenes.SelectedScene, true) { Name = "New Parallax Layer" });
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// Exits the application.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the KeyDown event of the EditorApplicationForm form.
        /// F5 - Compiles and runs the game specified by the project.
        /// Ctrl + S - Saves the project.
        /// Ctrl + N - Creates the new project.
        /// Ctrl + O - Opens the project.
        /// </summary>
        private void EditorApplicationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                CompileAndRunGame();
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                saveToolStripMenuItem_Click(null, null);
            }
            else if (e.KeyCode == Keys.N && e.Control)
            {
                newToolStripMenuItem_Click(null, null);
            }
            else if (e.KeyCode == Keys.O && e.Control)
            {
                openToolStripMenuItem_Click(null, null);
            }
        }
    }
}
