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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GUIGameWindow;
using Drawables.Cameras;
using Drawables;
using OpenTK;
using OpenTK.Graphics;
using Math3D;
using Grids;

using World;

namespace portalTest
{
    public class ViewPosition
    {
        public Vector3 Position = new Vector3();
        public Vector2 Rotation = new Vector2(0, 0);

        public float EyeHeight = 1.6f;
        public Vector3 Eye = new Vector3();

        public Cell cell = null;

        public void Move(Vector3 increment)
        {
            Move(increment.Y, increment.X, increment.Z);
        }

        public void CopyFrom (ViewPosition pos)
        {
            Position.X = pos.Position.X;
            Position.Y = pos.Position.Y;
            Position.Z = pos.Position.Z;

            Position.X = pos.Position.X;
            Position.Y = pos.Position.Y;

            cell = pos.cell;
        }

        public void Move(float forward, float sideways, float up)
        {
            Vector3 forwardVec = new Vector3(Heading());
            Vector3 leftwardVec = new Vector3(forwardVec);
            leftwardVec.X = -forwardVec.Y;
            leftwardVec.Y = forwardVec.X;

            Vector3 incremnt = new Vector3();
            incremnt += forwardVec * forward;
            incremnt += leftwardVec * sideways;
            incremnt.Z += up;

            Position += incremnt;
        }

        public void Turn(float spin, float tilt)
        {
            Rotation.X += tilt;
            Rotation.Y += spin;
        }

        public void SetCamera(Camera cam)
        {
            Eye = new Vector3(Position);
            Eye.Z += EyeHeight;

            cam.set(Eye, Rotation.X, Rotation.Y);
        }

        public float HeadingAngle()
        {
            return Rotation.Y;
        }

        public Vector2 Heading()
        {
            return new Vector2((float)Math.Cos(Trig.DegreeToRadian(Rotation.Y)), (float)Math.Sin(Trig.DegreeToRadian(Rotation.Y)));
        }

        public Vector3 Forward()
        {
            Vector3 forward = new Vector3(Heading());
            forward.Z = (float)Math.Tan(Trig.DegreeToRadian(Rotation.X));
            forward.Normalize();
            return forward;
        }
    }


    public class Visual
    {
        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 16.0f);
        Font small_serif = new Font(FontFamily.GenericSansSerif, 8.0f);

        public ViewPosition view = null;

        Camera camera = new Camera();

        GUIGameWindowBase window;

        PortalWorldRenderer renderer;

        Grid grid;

        public Visual ( GUIGameWindowBase win, PortalWorldRenderer rend )
        {
            renderer = rend;
            window = win;

            grid = new Grid();
        }

        public void SetupGL()
        {
            GL.ClearColor(System.Drawing.Color.Black);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.LightModel(LightModelParameter.LightModelColorControl, 1);

            // setup light 0
            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            camera.FOV = 30f;
            camera.set(new Vector3(1, 1, 2), 0, 0);
        }

        public void Resized(EventArgs e)
        {
            camera.Resize(window.Width, window.Height);
        }

        void DrawOverlay()
        {
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);
            float mapSize = 300;
            // draw map frame
            GL.Color4(1, 1, 1, 0.0f);
            GL.Begin(BeginMode.Quads);

            GL.Vertex3(mapSize, window.Height - mapSize, -100f);
            GL.Vertex3(window.Width, window.Height - mapSize, -100f);
            GL.Vertex3(window.Width, window.Height, -100f);
            GL.Vertex3(mapSize, window.Height, -100f);

            GL.Vertex3(0, 0, -100f);
            GL.Vertex3(window.Width,0, -100f);
            GL.Vertex3(window.Width, window.Height - mapSize, -100f);
            GL.Vertex3(0, window.Height - mapSize, -100f);

            GL.End();

            GL.PushMatrix();
            GL.Translate(mapSize/2f,window.Height - mapSize / 2f,-150);
            renderer.DrawMapView(view);

            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.Wheat);
            GL.Vertex3(0, 0, 10);
            GL.Vertex3(0, 100, 10);

            GL.Color3(Color.LightGray);
            GL.Vertex3(0, 0, 10);
            GL.Vertex3(Math.Cos(Trig.DegreeToRadian(camera.FOVX + 90)) * 200, Math.Sin(Trig.DegreeToRadian(camera.FOVX + 90)) * 200, 10);

            GL.Vertex3(0, 0, 10);
            GL.Vertex3(Math.Cos(Trig.DegreeToRadian(-camera.FOVX + 90)) * 200, Math.Sin(Trig.DegreeToRadian(-camera.FOVX + 90)) * 200, 10);

            GL.End();
            GL.PopMatrix();
        }

        public void RenderFrame ()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            if (view != null)
                view.SetCamera(camera);

            camera.Execute();

            if (view != null)
            {
                GL.Color3(Color.Yellow);
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(view.Eye);
                GL.Vertex3(view.Position);
                GL.End();

                IntPtr quadric = Glu.NewQuadric();
                GL.PushMatrix();
                GL.Translate(view.Position);
                Glu.Sphere(quadric, 0.25f, 10, 10);
                Glu.DeleteQuadric(quadric);
                GL.PopMatrix();
            }
             DrawablesSystem.system.removeAll();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(10, 15, 10, 1.0f));

          //  grid.Exectute();

            renderer.Draw(view, camera.ViewFrustum);

            DrawablesSystem.system.Execute();
            DrawablesSystem.system.removeAll();

            camera.SetOrthographic();
            GL.Clear(ClearBufferMask.DepthBufferBit);

            DrawOverlay();

            camera.SetPersective();

            GL.Clear(ClearBufferMask.DepthBufferBit);

            window.SwapBuffers();
        }
    }
}
