using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GUI
{
    public class ResizeEventArgs : EventArgs
    {
        public Vector2 Size = Vector2.Zero;
        public Vector2 Delta = Vector2.Zero;
        public ResizeEventArgs(Vector2 s, Vector2 d)
            : base()
        {
            Size = s;
            Delta = d;
        }
    }

    public class Item
    {
        public static float DepthShift = 0.1f;
        public List<Item> Children = new List<Item>();

        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = Vector2.Zero;

        protected Item Parrent = null;

        public static bool DrawDebugOutlines = false;

        public bool AnchorLeft = true;
        public bool AnchorTop = false;
        public bool AnchorRight = false;
        public bool AnchorBottom = true;

        protected bool canFocus = false;

        public bool CanFocus
        {
            get { return canFocus; }
        }

        public Vector2 GetAbsolutPosition ()
        {
            if (Parrent != null)
                return Parrent.GetAbsolutPosition() + Position;
            return Position;
        }

        public virtual void AddChild ( Item item )
        {
            item.Parrent = this;
            Children.Add(item);
            item.Adopted();
        }

        public virtual void Draw ()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.PushMatrix();
            GL.Translate(Position.X, Position.Y, DepthShift);
            OnPaint();

            if (DrawDebugOutlines)
            {
                GL.Disable(EnableCap.Texture2D);
                GL.Translate(0, 0, DepthShift);
                GL.Color4(Color.White);
                GL.Begin(BeginMode.LineLoop);
                GUIRenderer.ItemBoxVerts(this);
                GL.End();
                GL.Translate(0, 0, -DepthShift);
            }

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
            Vector2 oldSize = new Vector2(Size);

            if (Size != Vector2.Zero && Parrent != null && (AnchorBottom || AnchorTop || AnchorLeft || AnchorRight))
            {
                if (AnchorLeft && AnchorRight) // we are stretchy in X
                    Size.X += args.Delta.X;
                else
                {
                    if (AnchorRight && !AnchorLeft)
                        Position.X += args.Delta.X;
                }

                if (AnchorTop && AnchorBottom) // we are stretchy in Y
                    Size.Y += args.Delta.Y;
                else
                {
                    if (AnchorTop && !AnchorBottom)
                        Position.Y += args.Delta.Y;
                }
            }
            if (Size.X < 0)
                Size.X = 0;
            if (Size.Y < 0)
                Size.Y = 0;
            
            ResizeEventArgs childArgs = new ResizeEventArgs(Size,Size-oldSize);
            foreach (Item item in Children)
                item.Resize(childArgs);
        }

        protected virtual void OnPaint()
        {
        }
    }
}
