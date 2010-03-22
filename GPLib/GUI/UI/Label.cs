/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics;

using GUI;

namespace GUI.UI
{
    public class Label : Pannel
    {
        public enum TextAlignment
        {
            Near,
            Far,
            Center,
        }

        public TextAlignment Alignment
        {
            get
            {
                switch(alignment)
                {
                    case (OpenTK.Graphics.TextAlignment.Near):
                        return TextAlignment.Near;
                    case (OpenTK.Graphics.TextAlignment.Far):
                        return TextAlignment.Far;
                }
                return TextAlignment.Center;
            }

            set
            {
                switch(value)
                {
                     case TextAlignment.Near:
                        alignment = OpenTK.Graphics.TextAlignment.Near;
                        break;
                     case TextAlignment.Far:
                        alignment = OpenTK.Graphics.TextAlignment.Far;
                        break;
                     case TextAlignment.Center:
                         alignment = OpenTK.Graphics.TextAlignment.Center;
                         break;
                }
               
            }
        }

        protected OpenTK.Graphics.TextAlignment alignment = OpenTK.Graphics.TextAlignment.Near;

        public string Text = string.Empty;

        public Font Typeface = Screen.DefaultFont;
        public Color TypeColor = Color.White;

        protected RectangleF bounds = RectangleF.Empty;

        public Label( string t) : base()
        {
            Text = t;
        }

        public Label() : base()
        {
        }

        public override void Resize(ResizeEventArgs args)
        {
            base.Resize(args);
            bounds = RectangleF.Empty;
        }

        protected virtual void ComputeBounds ()
        {
            if (Size == Vector2.Zero)
            {
                Screen.Printer.Begin();
                TextExtents extents = Screen.Printer.Measure(Text, Typeface);
                Screen.Printer.End();

                Size = new Vector2(extents.BoundingBox.Width + 2, extents.BoundingBox.Height + 2);
            }
            Vector2 pos = GetAbsolutPosition();
            bounds = new RectangleF(pos.X + 1, Screen.ScreenY - pos.Y - Size.Y, Size.X, Size.Y);
        }

        protected override void OnPaint()
        {
            base.OnPaint();

            GUIRenderer.Renderer.RenderPannelFrame(this);

            if (bounds == RectangleF.Empty)
                ComputeBounds();

            GL.Enable(EnableCap.Texture2D);
            Screen.Printer.Begin();
            Screen.Printer.Print(Text, Typeface, TypeColor, bounds, OpenTK.Graphics.TextPrinterOptions.Default, alignment);
            Screen.Printer.End();
        }
    }
}
