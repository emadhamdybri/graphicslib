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
