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
