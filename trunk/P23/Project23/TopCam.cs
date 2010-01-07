using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;

namespace Project23
{
    class TopCam
    {
        float width;
        float height;

        float FOV = 45;
        float hither = 0.1f;
        float yon = 1000f;
        float orthoDepth = 100f;

        Vector3 position = new Vector3(0,0,0);

        bool ortho = false;

        public Vector3 Position
        {
            get {return position;}
        }

        public TopCam()
        {

        }

        public void Zoom ( float zIncrement )
        {
            position.Z += zIncrement;
        }

        public void SetXY ( Vector3 pos )
        {
            position.X = pos.X;
            position.Y = pos.Y;
        }

        public void Resize( float w, float h )
        {
            width = w;
            height = h;
            Perspective();
        }

        public void Perspective()
        {
            ortho = false;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float aspect = (width / height);

            Glu.Perspective(FOV, aspect, hither, yon);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        public void Orthographic()
        {
            ortho = true;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, 0, height, 0, yon);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        public void Execute ()
        {
            if (ortho)
                Perspective();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(-position.X, -position.Y, -position.Z);	// take us to the pos

        }
    }
}
