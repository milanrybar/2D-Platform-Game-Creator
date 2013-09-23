/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformGameCreator.Editor.Assets.Textures;
using PlatformGameCreator.Editor.Common;
using System.Diagnostics;

namespace PlatformGameCreator.Editor.Assets.Animations
{
    /// <summary>
    /// Form for completely editing the <see cref="Animation"/>
    /// Editing collision shapes, name and frames of the <see cref="Animation"/>.
    /// </summary>
    partial class AnimationForm : Form
    {
        /// <summary>
        /// <see cref="TextureScreen"/> for editing collision shapes of the <see cref="Animation"/>. 
        /// </summary>
        private TextureScreen animationScreen;

        /// <summary>
        /// Controller for the <see cref="ShapesEditingScreen"/>.
        /// </summary>
        private BasicControllerForShapesEditing shapesController;

        /// <summary>
        /// Animation that is edited.
        /// </summary>
        private Animation animation;

        /// <summary>
        /// View of textures to choose frame from.
        /// </summary>
        private TexturesView textures;

        /// <summary>
        /// Current index of the animation frame when playing the animation.
        /// </summary>
        private int animationFrame;

        /// <summary>
        /// Messages manager for this form.
        /// </summary>
        private IMessagesManager messagesManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationForm"/> class.
        /// </summary>
        /// <param name="animation">The animation to edit.</param>
        public AnimationForm(Animation animation)
        {
            InitializeComponent();

            Icon = Properties.Resources._2DPGC_Logo;

            // messages manager
            messagesManager = new DefaultMessagesManager(toolStripStatusLabel);
            Messages.MessagesManager = messagesManager;

            // animation
            this.animation = animation;

            // textures view
            textures = new TexturesView();
            textures.DrawableAssets = Project.Singleton.Textures;
            textures.Size = new System.Drawing.Size(200, 300);
            textures.Margin = new Padding(1, 1, 0, 1);
            textures.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(textures, 0, 0);

            // frames view
            framesView.BeginUpdate();

            framesView.LargeImageList = new ImageList();
            framesView.LargeImageList.ImageSize = new System.Drawing.Size(framesView.Size.Height - 40, framesView.Size.Height - 40);
            framesView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;

            // init frames
            foreach (Texture texture in animation.Frames)
            {
                AddTexture(texture);
            }

            framesView.EndUpdate();

            // animation screen
            animationScreen = new TextureScreen();
            animationScreen.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(animationScreen, 1, 0);
            animationScreen.Zoom = 100;
            animationScreen.Position = new PointF(-animationScreen.Width / 2f, -animationScreen.Height / 2f);

            // shapes controller 
            shapesController = new BasicControllerForShapesEditing(animationScreen);
            shapesController.Location = new Point(3, 145);
            animationSettingsPanel.Controls.Add(shapesController);

            // init controls settings
            nameTextBox.Text = animation.Name;
            speedBox.Value = animation.Speed;

            // title (window text)
            UpdateTitle();

            // init shapes from animation
            foreach (Shape shape in animation.Shapes)
            {
                ShapeState newItem = animationScreen.AddShape(shape);
                shapesController.ShapesList.Items.Add(newItem);

            }
            shapesController.ShapesList.SelectedIndex = shapesController.ShapesList.Items.Count - 1;
        }

        /// <summary>
        /// Updates title of form.
        /// </summary>
        private void UpdateTitle()
        {
            Text = "Animation Editor - " + animation.Name;
        }

        /// <summary>
        /// Adds the specified texture to the animation frames to the specified index.
        /// </summary>
        /// <param name="texture">The texture to add to the frames.</param>
        /// <param name="index">The index where to add frame. If -1 texture is added to the end.</param>
        private void AddTexture(Texture texture, int index = -1)
        {
            if (!framesView.LargeImageList.Images.ContainsKey(texture.Id.ToString()))
            {
                framesView.LargeImageList.Images.Add(texture.Id.ToString(), texture.TextureGdi.CreateThumbnail(framesView.LargeImageList.ImageSize.Width, framesView.LargeImageList.ImageSize.Height));
            }

            if (index < 0)
            {
                framesView.Items.Add(new ListViewItem(texture.Name, texture.Id.ToString()) { Tag = texture });
            }
            else
            {
                framesView.Items.Insert(index, new ListViewItem(texture.Name, texture.Id.ToString()) { Tag = texture });
            }
        }

        /// <summary>
        /// Handles the DragEnter event of the framesView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void framesView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(typeof(DrawableAsset)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Handles the DragDrop event of the framesView control.
        /// When texture is dropped to the <see cref="framesView"/> then texture is added to the frames at the specified location.
        /// When frame from the frames is dropped to the <see cref="framesView"/> then frame is moved to the specified location.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void framesView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DrawableAsset)))
            {
                Texture texture = (Texture)e.Data.GetData(typeof(DrawableAsset));
                ListViewItem item = framesView.FindNearestItem(SearchDirectionHint.Left, framesView.PointToClient(new Point(e.X, e.Y)));

                Debug.Assert(texture != null, "Only texture is possible as drawable asset.");

                AddTexture(texture, item != null ? item.Index + 1 : 0);

                UpdateFrameViewItems();
            }
            else if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                int newIndex = 0;
                ListViewItem dropItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                ListViewHitTestInfo hitTest = framesView.HitTest(framesView.PointToClient(new Point(e.X, e.Y)));
                if (hitTest != null && hitTest.Item != null)
                {
                    newIndex = hitTest.Item.Index;
                }
                else
                {
                    ListViewItem item = framesView.FindNearestItem(SearchDirectionHint.Left, framesView.PointToClient(new Point(e.X, e.Y)));

                    newIndex = item == null ? 0 : dropItem.Index < item.Index || item.Index + 1 == framesView.Items.Count ? item.Index : item.Index + 1;
                }

                framesView.BeginUpdate();

                framesView.Items.RemoveAt(dropItem.Index);
                framesView.Items.Insert(newIndex, dropItem);

                UpdateFrameViewItems();

                framesView.EndUpdate();
            }
        }

        /// <summary>
        /// Refreshs the frames control.
        /// Solving ListView bug when using LargeIcons.
        /// ( http://stackoverflow.com/questions/5591269/net-inserting-with-index-in-a-listview-in-largeicon-mode-dont-display-inserte )
        /// </summary>
        private void UpdateFrameViewItems()
        {
            framesView.Alignment = ListViewAlignment.Default;
            framesView.Alignment = ListViewAlignment.Left;
        }

        /// <summary>
        /// Handles the ItemDrag event of the framesView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemDragEventArgs"/> instance containing the event data.</param>
        private void framesView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item != null)
            {
                framesView.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// Called when the name of the animation changes. Updates title of form.
        /// </summary>
        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (nameTextBox.Text != animation.Name)
            {
                animation.Name = nameTextBox.Text;
                UpdateTitle();
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the speedBox control.
        /// Updates the speed time of the animation.
        /// </summary>
        private void speedBox_ValueChanged(object sender, EventArgs e)
        {
            if (speedBox.Value != animation.Speed)
            {
                animation.Speed = (uint)speedBox.Value;

                if (timer.Enabled && animation.Speed > 0f)
                {
                    timer.Interval = (int)animation.Speed;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the playButton control.
        /// Begins playing of the animation.
        /// </summary>
        private void playButton_Click(object sender, EventArgs e)
        {
            if (!timer.Enabled && framesView.Items.Count > 0 && animation.Speed > 0f)
            {
                timer.Interval = (int)animation.Speed;
                animationFrame = 0;
                animationScreen.Texture = (Texture)framesView.Items[animationFrame].Tag;

                playButton.Enabled = false;
                stopButton.Enabled = true;

                timer.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the stopButton control.
        /// Stops playing of the animation.
        /// </summary>
        private void stopButton_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                playButton.Enabled = true;
                stopButton.Enabled = false;

                timer.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// Called when it is time to change to the next frame of the animation when playing the animation.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            ++animationFrame;

            // end of the animation
            if (animationFrame >= framesView.Items.Count)
            {
                // loop
                if (loopCheckBox.Checked && framesView.Items.Count > 0)
                {
                    animationFrame = 0;
                }
                // stop animation
                else
                {
                    stopButton_Click(null, null);
                    return;
                }
            }

            // change to next frame
            animationScreen.Texture = (Texture)framesView.Items[animationFrame].Tag;
        }

        /// <summary>
        /// Handles the KeyDown event of the framesView control.
        /// Delete - Removes selected frame.
        /// </summary>
        private void framesView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && framesView.SelectedItems != null)
            {
                foreach (ListViewItem selectedItem in framesView.SelectedItems)
                {
                    if (selectedItem.Index != -1) framesView.Items.RemoveAt(selectedItem.Index);
                }

                UpdateFrameViewItems();
            }
        }

        /// <summary>
        /// Handles the Activated event of the AnimationForm control.
        /// </summary>
        private void AnimationForm_Activated(object sender, EventArgs e)
        {
            Messages.MessagesManager = messagesManager;
            shapesController.SetSettings();
            ShapesEditingState.DrawingTools = animationScreen;
        }

        /// <summary>
        /// Handles the FormClosing event of the AnimationForm control.
        /// Checks if collision shapes are valid and animation has at least one frame. If not form is not closed until all shapes and animation are valid.
        /// If yes, saves data to the animation and invokes its <see cref="DrawableAsset.DrawableAssetChanged"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void AnimationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // check if every shape is valid
            for (int i = 0; i < shapesController.ShapesList.Items.Count; ++i)
            {
                ShapeState shapeState = shapesController.ShapesList.Items[i] as ShapeState;
                if (shapeState != null)
                {
                    // shape is not valid
                    if (!shapeState.IsShapeValid())
                    {
                        // select invalid shape
                        shapesController.ShapesList.SelectedIndex = i;
                        // show message
                        shapeState.OnInvalidShape();
                        // form will not be closed until all shapes are valid
                        e.Cancel = true;
                        return;
                    }
                }
            }

            // check if animation is valid
            if (framesView.Items.Count == 0)
            {
                // show message
                Messages.ShowWarning("Invalid animation. No frame is set.");
                // form will not be closed until all shapes are valid
                e.Cancel = true;
                return;
            }

            // animation is valid => update animation
            UpdateAnimation();

            // animation changed
            animation.InvokeDrawableAssetChanged();
        }

        /// <summary>
        /// Updates underlying animation. Saves all data to animation.
        /// </summary>
        private void UpdateAnimation()
        {
            // save shapes to the animation
            animation.Shapes.Clear();

            foreach (ShapeState shape in animationScreen.Shapes)
            {
                animation.Shapes.Add(shape.Shape);
            }

            // save frames to the animation
            animation.Frames.Clear();

            foreach (ListViewItem frame in framesView.Items)
            {
                animation.Frames.Add((Texture)frame.Tag);
            }
        }
    }
}
