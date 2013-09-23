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

namespace PlatformGameCreator.Editor.Common
{
    /// <summary>
    /// Defines useful extension methods.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Adds two specified points together and returns the result.
        /// </summary>
        /// <param name="first">The first to add.</param>
        /// <param name="second">The second to add.</param>
        /// <returns>Result of addition.</returns>
        public static PointF Add(this PointF first, PointF second)
        {
            return new PointF(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Adds two specified points together and returns the result.
        /// </summary>
        /// <param name="first">The first to add.</param>
        /// <param name="second">The second to add.</param>
        /// <returns>Result of addition.</returns>
        public static Point Add(this Point first, Point second)
        {
            return new Point(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Subs the second from the first specified point and returns the result.
        /// </summary>
        /// <param name="first">Minuend of subtraction.</param>
        /// <param name="second">subtrahend  of subtraction.</param>
        /// <returns>Result of subtraction.</returns>
        public static PointF Sub(this PointF first, PointF second)
        {
            return new PointF(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Subs the second from the first specified point and returns the result.
        /// </summary>
        /// <param name="first">Minuend of subtraction.</param>
        /// <param name="second">subtrahend  of subtraction.</param>
        /// <returns>Result of subtraction.</returns>
        public static Point Sub(this Point first, Point second)
        {
            return new Point(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Subs the second from the first specified point and returns the result.
        /// </summary>
        /// <param name="first">Minuend of subtraction.</param>
        /// <param name="second">subtrahend  of subtraction.</param>
        /// <returns>Result of subtraction.</returns>
        public static PointF Sub(this Point first, PointF second)
        {
            return new PointF(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Subs the second from the first specified point and returns the result.
        /// </summary>
        /// <param name="first">Minuend of subtraction.</param>
        /// <param name="second">subtrahend  of subtraction.</param>
        /// <returns>Result of subtraction.</returns>
        public static PointF Sub(this Point first, Microsoft.Xna.Framework.Vector2 second)
        {
            return new PointF(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Array of points used for transformation.
        /// </summary>
        private static PointF[] pointToTransform = new PointF[1];

        /// <summary>
        /// Applies the geometric transform represented by the specified matrix to the specified point.
        /// </summary>
        /// <param name="point">Point to transform.</param>
        /// <param name="transformMatrix">Matrix to transform the point by.</param>
        /// <returns>Transformed point.</returns>
        public static PointF Transform(this PointF point, Matrix transformMatrix)
        {
            pointToTransform[0] = point;
            transformMatrix.TransformPoints(pointToTransform);
            return pointToTransform[0];
        }

        /// <summary>
        /// Applies the geometric transform represented by the specified matrix to the specified point.
        /// </summary>
        /// <param name="point">Point to transform.</param>
        /// <param name="transformMatrix">Matrix to transform the point by.</param>
        /// <returns>Transformed point.</returns>
        public static PointF Transform(this Point point, Matrix transformMatrix)
        {
            pointToTransform[0] = point;
            transformMatrix.TransformPoints(pointToTransform);
            return pointToTransform[0];
        }

        /// <summary>
        /// Multiplies the specified point by scalar value and returns the result.
        /// </summary>
        /// <param name="point">The point to multiply.</param>
        /// <param name="mul">The scalar to multiply the point by.</param>
        /// <returns>Result of multiplication.</returns>
        public static PointF Mul(this PointF point, float mul)
        {
            return new PointF(point.X * mul, point.Y * mul);
        }

        /// <summary>
        /// Multiplies the specified point by scalar value and returns the result.
        /// </summary>
        /// <param name="point">The point to multiply.</param>
        /// <param name="mul">The scalar to multiply the point by.</param>
        /// <returns>Result of multiplication.</returns>
        public static PointF Mul(this Point point, float mul)
        {
            return new PointF(point.X * mul, point.Y * mul);
        }

        /// <summary>
        /// Converts the specified point in <see cref="PointF"/> to <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The point in <see cref="PointF"/> to convert.</param>
        /// <returns>Converted point in <see cref="Point"/>.</returns>
        public static Point ToPoint(this PointF point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// Creates a rectangle from two specified points.
        /// </summary>
        /// <param name="first">The first point of the rectangle.</param>
        /// <param name="second">The second point of the rectangle.</param>
        /// <returns>Created rectangle.</returns>
        public static RectangleF ToRectangleF(this PointF first, PointF second)
        {
            return new RectangleF(Math.Min(first.X, second.X), Math.Min(first.Y, second.Y),
                Math.Abs(first.X - second.X), Math.Abs(first.Y - second.Y));
        }

        /// <summary>
        /// Creates a rectangle from two specified points.
        /// </summary>
        /// <param name="first">The first point of the rectangle.</param>
        /// <param name="second">The second point of the rectangle.</param>
        /// <returns>Created rectangle.</returns>
        public static Rectangle ToRectangle(this PointF first, PointF second)
        {
            return new Rectangle((int)Math.Min(first.X, second.X), (int)Math.Min(first.Y, second.Y),
                (int)Math.Abs(first.X - second.X), (int)Math.Abs(first.Y - second.Y));
        }

        /// <summary>
        /// Draws the specified rectangle.
        /// </summary>
        /// <param name="graphics"><see cref="Graphics"/> instance for drawing the rectangle.</param>
        /// <param name="pen">The pen to draw the rectangle by.</param>
        /// <param name="rectangle">The rectangle to draw.</param>
        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF rectangle)
        {
            graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Creates the rounded rectangle from the specified rectangle and corner radius.
        /// </summary>
        /// <param name="rect">The rectangle to create the rounded rectangle from.</param>
        /// <param name="cornerRadius">The radius of the corners for the rounded rectangle.</param>
        /// <returns><see cref="GraphicsPath"/> from the specified rectangle and corner radius.</returns>
        public static GraphicsPath CreateRoundedRectanglePath(this RectangleF rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();

            roundedRect.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius, rect.Y + rect.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            roundedRect.AddArc(rect.X, rect.Y + rect.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            roundedRect.AddLine(rect.X, rect.Y + rect.Height - cornerRadius, rect.X, rect.Y + cornerRadius / 2);

            return roundedRect;
        }

        /// <summary>
        /// Converts the specified point in <see cref="Point"/> to <see cref="Microsoft.Xna.Framework.Vector2"/>.
        /// </summary>
        /// <param name="point">The point in <see cref="Point"/> to convert.</param>
        /// <returns>Converted point in <see cref="Microsoft.Xna.Framework.Vector2"/>.</returns>
        public static Microsoft.Xna.Framework.Vector2 ToVector2(this Point point)
        {
            return new Microsoft.Xna.Framework.Vector2(point.X, point.Y);
        }

        /// <summary>
        /// Converts the specified point in <see cref="PointF"/> to <see cref="Microsoft.Xna.Framework.Vector2"/>.
        /// </summary>
        /// <param name="point">The point in <see cref="PointF"/> to convert.</param>
        /// <returns>Converted point in <see cref="Microsoft.Xna.Framework.Vector2"/>.</returns>
        public static Microsoft.Xna.Framework.Vector2 ToVector2(this PointF point)
        {
            return new Microsoft.Xna.Framework.Vector2(point.X, point.Y);
        }

        /// <summary>
        /// Creates the thumbnail for the specified image.
        /// </summary>
        /// <param name="image">The image to create the thumbnail from.</param>
        /// <param name="width">The width of the thumbnail.</param>
        /// <param name="height">The height of the thumbnail.</param>
        /// <returns>Thumbnail of the specified image.</returns>
        public static Image CreateThumbnail(this Image image, int width, int height)
        {
            int thumbWidth = width;
            int thumbHeight = height;
            Bitmap thumbnail = new Bitmap(thumbWidth, thumbHeight, image.PixelFormat);

            using (Graphics g = Graphics.FromImage(thumbnail))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                RectangleF thumbRectangle;
                if (image.Width <= thumbWidth && image.Height <= thumbHeight)
                {
                    thumbRectangle = new RectangleF((thumbWidth - image.Width) / 2f, (thumbHeight - image.Height) / 2f, image.Width, image.Height);
                }
                else if (image.Width >= image.Height)
                {
                    float newHeight = image.Height * (thumbHeight / (float)image.Width);
                    thumbRectangle = new RectangleF(0, (thumbHeight - newHeight) / 2f, thumbWidth, newHeight);
                }
                else
                {
                    float newWidth = image.Width * (thumbWidth / (float)image.Height);
                    thumbRectangle = new RectangleF((thumbWidth - newWidth) / 2f, 0, newWidth, thumbHeight);
                }

                g.DrawImage(image, thumbRectangle);
            }

            return thumbnail;
        }
    }
}
