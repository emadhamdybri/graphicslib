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
