using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.Cameras;
using Drawables.Textures;
using Math3D;
using Drawables.DisplayLists;

namespace Drawables
{
    public class Billboard
    {
        public delegate void BillbordCallback ( Billboard board);

        public event BillbordCallback PreDraw;

        public static Camera Camera = null;

        public Texture Image = null;
        public float ImageScale = 1.0f;

        public Vector3 Position = Vector3.Zero;
        public object Tag = null;

        public DisplayList DrawList = null;

        public virtual void Draw ()
        {
            if (PreDraw != null)
                PreDraw(this);
        }
    }

    public class ZAxisBillboard : Billboard
    {
        public ZAxisBillboard ()
        {

        }

        protected double GetRotAngle ()
        {
            Vector3 lookVec = Position-Camera.EyePoint;

            lookVec.Z = 0;

            return MathHelper.RadiansToDegrees((float)Math.Atan2(lookVec.Y, lookVec.X));
        }

        public override void Draw()
        {
            base.Draw();

            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(GetRotAngle(), 0, 0, 1);

            if (DrawList == null)
                Image.Draw(ImageScale);
            else
                DrawList.Call();

            GL.PopMatrix();
        }
    }
}
