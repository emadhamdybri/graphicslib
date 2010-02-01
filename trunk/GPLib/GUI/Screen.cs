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

        public static Font DefaultFont = new Font(FontFamily.GenericSerif, 10);
        public static OpenTK.Graphics.TextPrinter Printer = new OpenTK.Graphics.TextPrinter();

        public override void Resize(ResizeEventArgs args)
        {
            base.Resize(args);
            Size = new Vector2(args.Size);
            ScreenY = args.Size.Y;
        }
    }
}
