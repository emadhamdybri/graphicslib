using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using GUI;

namespace GUI.UI
{
    public class Pannel : Item
    {
        public Color FrameColor = Color.Transparent;
        public Color BackgroundColor = Color.Transparent;

        protected override void OnPaint()
        {
            base.OnPaint();
            GUIRenderer.Renderer.RenderPannelFrame(this);
        }
    }
}
