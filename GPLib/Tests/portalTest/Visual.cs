using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using GUIGameWindow;
using Drawables.Cameras;
using OpenTK;
using OpenTK.Graphics;
using Math3D;
using Grids;

namespace portalTest
{
    public class DebugableVisibleFrustum : VisibleFrustum
    {
        public DebugableVisibleFrustum(VisibleFrustum val)
            : base(val)
        {
        }

        void drawEyePoint()
        {
            // put a sphere at the eye point and draw the axes
            GL.Color4(0.25f, 0.25f, 1f, 0.5f);
            Glu.Sphere(Glu.NewQuadric(), 0.25f, 6, 6);

            GL.Disable(EnableCap.Lighting);

            GL.LineWidth(2.0f);
            GL.Begin(BeginMode.Lines);

            // the "up" vector
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(Up);
            GL.Vertex3(Up);
            GL.Vertex3(Up * 0.75f + RightVec * 0.35f);

            // the "right" vector
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(RightVec);
            GL.Vertex3(RightVec);
            GL.Vertex3(RightVec * 0.75f + Up * 0.25f);

            // the "forward" vector
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(ViewDir * 3f);
            GL.Vertex3(ViewDir * 3f);
            GL.Vertex3(ViewDir * 2.75f + Up * 0.125f);
            GL.Vertex3(ViewDir * 3f);
            GL.Vertex3(ViewDir * 2.75f + Up * -0.125f);

            GL.End();

            GL.Enable(EnableCap.Lighting);
        }

        void drawViewFrustum()
        {
            GL.Disable(EnableCap.Lighting);

            float endViewAlpha = 0.25f;
            float startViewAlpha = 0.75f;

            //GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            float planeAlphaFactor = 0.25f;
            GL.Begin(BeginMode.Triangles);

            // top
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[2] * farClip);

            // bottom
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[0] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[0] * farClip);

            // left
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[0] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[0] * farClip);

            // right
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[2] * farClip);

            GL.End();

            GL.Begin(BeginMode.Quads);
            // far
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[0] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[0] * farClip);
            GL.End();

            GL.LineWidth(2.0f);
            // draw the edge vectors
            GL.Begin(BeginMode.Lines);
            foreach (Vector3 e in edge)
            {
                GL.Color4(1f, 1f, 1f, startViewAlpha);
                GL.Vertex3(0, 0, 0);
                GL.Color4(1f, 1f, 1f, endViewAlpha);
                GL.Vertex3(e * farClip);
            }
            GL.End();

            GL.LineWidth(3.0f);
            // compute the view plane points for normal drawing
            Vector3 leftPoint = (edge[0] + edge[3]) * 2;
            Vector3 rightPoint = (edge[1] + edge[2]) * 2;
            Vector3 topPoint = (edge[3] + edge[2]) * 2;
            Vector3 bottomPoint = (edge[0] + edge[1]) * 2;

            GL.Begin(BeginMode.Lines);

            GL.Color4(1f, 0f, 0f, 0.5f);
            GL.Vertex3(leftPoint);
            GL.Vertex3(leftPoint + Left.Normal);

            GL.Color4(0f, 1f, 0f, 0.5f);
            GL.Vertex3(rightPoint);
            GL.Vertex3(rightPoint + Right.Normal);

            GL.Color4(0f, 0f, 1f, 0.5f);
            GL.Vertex3(topPoint);
            GL.Vertex3(topPoint + Top.Normal);

            GL.Color4(1f, 0f, 1f, 0.5f);
            GL.Vertex3(bottomPoint);
            GL.Vertex3(bottomPoint + Bottom.Normal);

            GL.Color4(1f, 1f, 1f, endViewAlpha);
            GL.Vertex3(ViewDir * farClip);
            GL.Vertex3(ViewDir * farClip + Far.Normal);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Color4(0.5f, 0.5f, 0.5f, endViewAlpha);
            foreach (Vector3 e in edge)
                GL.Vertex3(e * farClip);
            GL.End();

            GL.PushMatrix();
            GL.Color4(1f, 1f, 1f, startViewAlpha);
            GL.Translate(ViewDir * nearClip);
            Glu.Sphere(Glu.NewQuadric(), 0.125f, 3, 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Color4(0.5f, 0.5f, 0.5f, endViewAlpha);
            GL.Translate(ViewDir * farClip);
            Glu.Sphere(Glu.NewQuadric(), 0.125f, 3, 2);
            GL.PopMatrix();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            GL.Enable(EnableCap.Lighting);
        }

        public void drawFrustum()
        {
            GL.PushMatrix();
            //move to the eye point
            GL.Translate(EyePoint);

            drawEyePoint();

            drawViewFrustum();

            GL.PopMatrix();

            GL.Enable(EnableCap.Lighting);
        }
    }

    public class Visual
    {
        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 16.0f);
        Font small_serif = new Font(FontFamily.GenericSansSerif, 8.0f);


        public Camera camera = new Camera();
        DebugableVisibleFrustum clipingFrustum = null;

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

            camera.set(new Vector3(1, 1, 2), 0, 0);

            clipingFrustum = new DebugableVisibleFrustum(camera.SnapshotFrusum());
        }

        public void Resized(EventArgs e)
        {
            camera.Resize(window.Width, window.Height);
        }

        public void RenderFrame ()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            camera.Execute();

            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(10, 15, 10, 1.0f));

            if (clipingFrustum != null)
                clipingFrustum.drawFrustum();

            grid.Exectute();

            renderer.Draw();

            GL.Clear(ClearBufferMask.DepthBufferBit);

            window.SwapBuffers();
        }
    }
}
