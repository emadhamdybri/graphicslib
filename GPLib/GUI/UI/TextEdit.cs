using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace GUI.UI
{
    public class TextEdit : Label
    {
        public bool Focus = false;

        public bool CarretUp;
        public static float CarretBlinkTime = 1f;

        float carratPos = 0;
        int carratCharacter = 0;

        public float CarratPos
        {
            get { return carratPos; }
        }

        public int CarratCharacter
        {
            get { return carratCharacter; }
        }

        protected Timer CarretTimer;
        public TextEdit()
        {
            canFocus = true;

            FrameColor = Color.Black;
            BackgroundColor = Color.Gray;

            CarretTimer = new Timer(new TimerCallback(CarretTick));
            CarretTimer.Change(0, (int)(CarretBlinkTime * 1000));
        }

        protected void CarretTick ( object state)
        {
            if (Focus)
                CarretUp = !CarretUp;
            else
                CarretUp = false;
        }

        public virtual void SetText ( string text )
        {
            Text = text;

            Screen.Printer.Begin();
            carratPos = Screen.Printer.Measure(Text, Typeface).BoundingBox.Width+2;
            carratCharacter = text.Length;
        }

        public virtual void AddCharacters ( string text )
        {
            Text += text;
            SetText(Text);
        }

        protected override void OnPaint()
        {
            GUIRenderer.Renderer.RenderTEFrame(this);

            if (bounds == RectangleF.Empty)
                ComputeBounds();

            GL.Enable(EnableCap.Texture2D);
            Screen.Printer.Begin();
            Screen.Printer.Print(Text, Typeface, TypeColor, bounds, OpenTK.Graphics.TextPrinterOptions.Default, alignment);
            Screen.Printer.End();

            if (!CarretUp)
                return;

            GL.PushMatrix();
            GL.Translate(carratPos, 0, DepthShift);
            GL.Color4(TypeColor);
            GUIRenderer.Renderer.RenderTECarret(bounds.Height);
            GL.PopMatrix();
        }
    }
}
