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

using GUI.UI;

namespace GUI
{
    public class GUIRenderer
    {
        public static GUIRenderer Renderer = new GUIRenderer();

        public static void ItemBoxVerts ( Item item )
        {
            GL.Vertex2(0, 0);
            GL.Vertex2(item.Size.X, 0);
            GL.Vertex2(item.Size.X, item.Size.Y);
            GL.Vertex2(0, item.Size.Y);
        }

        public virtual void RenderPannelFrame( Pannel pannel )
        {
            if (pannel.BackgroundColor != Color.Transparent)
            {
                GL.Color4(pannel.BackgroundColor);
                GL.Begin(BeginMode.Quads);

                GL.Normal3(0, 0, 1);
                ItemBoxVerts(pannel);
                GL.End();

                GL.Translate(0, 0, Item.DepthShift);
            }

            if (pannel.FrameColor != Color.Transparent)
            {
                GL.Color4(pannel.FrameColor);
                GL.Begin(BeginMode.LineLoop);
                ItemBoxVerts(pannel);
                GL.End();
            }
        }

        public virtual void RenderTEFrame( TextEdit item )
        {
            RenderPannelFrame(item);
        }

        public virtual void RenderTECarret ( float height )
        {
            GL.Disable(EnableCap.Texture2D);
            
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(0, 0);
            GL.Vertex2(0, height);
            GL.End();
        }
    }
}
