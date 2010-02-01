using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Utilities.Paths;
using Drawables.Textures;

using OpenTK;
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
            LoadImage();
            if (Texture != null)
                Size = new Vector2((float)Texture.Width, (float)Texture.Height);
        }

        public Image(string t, Vector2 pos)
            : base()
        {
            TextureFile = t;
            LoadImage();
            Position = pos;
            if (Texture != null)
                Size = new Vector2((float)Texture.Width, (float)Texture.Height);
        }

        public Image () : base()
        {
        }

        protected virtual void LoadImage ()
        {
            if (TextureFile == string.Empty)
                return;

            if (Texture == null)
                Texture = TextureSystem.system.GetTexture(ResourceManager.FindFile(TextureFile));
        }

        protected override void OnPaint()
        {
            LoadImage();
            if (Texture == null)
                return;

            float scale = 1;
            if (ImageScaleMode == ScaleMode.ToX)
                scale = Size.X/Texture.Width;
            else if (ImageScaleMode == ScaleMode.ToY)
                scale = Size.Y/Texture.Height;

            Texture.Draw(scale);
        }
    }
}
