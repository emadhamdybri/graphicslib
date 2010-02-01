using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GUI
{
    public class ResizeEventArgs : EventArgs
    {
        public Vector2 Size = Vector2.Zero;
        public ResizeEventArgs (Vector2 s) : base()
        {
            Size = s;
        }
    }

    public class Item
    {
        public static float DepthShift = 0.01f;
        public List<Item> Children = new List<Item>();

        public Vector3 Position = Vector3.Zero;
        public Vector2 Size = Vector2.Zero;

        protected Item Parrent = null;

        public virtual void AddChild ( Item item )
        {
            item.Parrent = this;
            Children.Add(item);
            item.Adopted();
        }

        public virtual void Draw ()
        {
            OnPaint();
            GL.PushMatrix();
            GL.Translate(0,0,DepthShift);
            foreach (Item item in Children)
                item.Draw();
            GL.PopMatrix();
        }

        public virtual void Update ()
        {
        }

        protected virtual void Adopted()
        {
        }

        public virtual void Resize (ResizeEventArgs args)
        {
            foreach (Item item in Children)
                item.Resize(args);
        }

        protected virtual void OnPaint()
        {
        }
    }
}
