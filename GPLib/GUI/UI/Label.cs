using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics;


namespace GUI.UI
{
    public class Label : Item
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
        public Color FrameColor = Color.Transparent;
        public Color BackgroundColor = Color.Transparent;

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
            bounds = new RectangleF(Position.X, Screen.ScreenY - Position.Y, Size.X, Size.Y);
        }

        protected override void OnPaint()
        {
            base.OnPaint();

            GL.PushMatrix();
            if (BackgroundColor != Color.Transparent)
            {
                GL.Color4(BackgroundColor);
                GL.Begin(BeginMode.Quads);

                GL.Normal3(0, 0, 1);
                GL.Vertex2(Position.X, Position.Y);
                GL.Vertex2(Position.X+Size.X, Position.Y);
                GL.Vertex2(Position.X + Size.X, Position.Y + Size.Y);
                GL.Vertex2(Position.X, Position.Y + Size.Y);
                GL.End();

                GL.Translate(0, 0, Item.DepthShift);
            }

            if (FrameColor != Color.Transparent)
            {
                GL.Color4(FrameColor);
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex2(Position.X, Position.Y);
                GL.Vertex2(Position.X + Size.X, Position.Y);
                GL.Vertex2(Position.X + Size.X, Position.Y + Size.Y);
                GL.Vertex2(Position.X, Position.Y + Size.Y);
                GL.End();
            }

            if (bounds == RectangleF.Empty)
                ComputeBounds();

            Screen.Printer.Begin();
            Screen.Printer.Print(Text, Typeface, TypeColor, bounds, OpenTK.Graphics.TextPrinterOptions.Default, alignment);
            Screen.Printer.End();
            GL.PopMatrix();
        }
    }
}
