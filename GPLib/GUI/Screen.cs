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
            Resize(args);
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
