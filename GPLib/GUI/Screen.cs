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
using OpenTK.Graphics.OpenGL;

namespace GUI
{
    public class Screen : Item
    {
        public static float ScreenY = 0;
        public static float ZStart = -1;

        public static Font DefaultFont = new Font(FontFamily.GenericSerif, 10);
        public static OpenTK.Graphics.TextPrinter Printer = new OpenTK.Graphics.TextPrinter();

        public Screen() : base()
        {
            AnchorLeft = false;
            AnchorTop = false;
            AnchorRight = false;
            AnchorBottom = false;
        }

        public void Resize(int X, int Y)
        {
            ScreenY = Y;
            Vector2 size = new Vector2(X, Y);
            ResizeEventArgs args = new ResizeEventArgs(size, size-Size);
            Size = size;
            foreach (Item item in Children)
                item.Resize(args);
        }

        public override void Draw ()
        {
            GL.PushMatrix();
            GL.Translate(0, 0, ZStart);
            base.Draw();
            GL.PopMatrix();
        }
    }
}
