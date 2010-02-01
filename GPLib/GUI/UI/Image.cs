using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities.Paths;
using Drawables.Textures;

using OpenTK.Graphics.OpenGL;

namespace GUI.UI
{
    public class Image : Item
    {
        public enum ScaleMode
        {
            None,
            ToX,
            ToY
        }

        public ScaleMode ImageScaleMode = ScaleMode.None;

        protected Texture Texture = null;
        public string ImageFile
        {
            get { return TextureFile; }
        }

        protected string TextureFile = string.Empty;

        public Image ( string t ) : base()
        {
            TextureFile = t;
        }

        public Image () : base()
        {
        }

        protected override void OnPaint()
        {
            if (TextureFile == string.Empty)
                return;

            if (Texture == null)
                Texture = TextureSystem.system.GetTexture(ResourceManager.FindFile(TextureFile));

            if (Texture == null)
                return;

            float scale = 1;
            if (ImageScaleMode == ScaleMode.ToX)
                scale = Size.X/Texture.Width;
            else if (ImageScaleMode == ScaleMode.ToY)
                scale = Size.Y/Texture.Height;

            GL.PushMatrix();
            GL.Translate(Position.X, Position.Y, 0);
            Texture.Draw(scale);
            GL.PopMatrix();
        }
    }
}
