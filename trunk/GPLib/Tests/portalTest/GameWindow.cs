using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

#pragma warning disable 612, 618

namespace portalTest
{
    public partial class GameWindow : GUIGameWindowBase
    {
        Camera camera = new Camera();
        
        DebugableVisibleFrustum clipingFrustum = null;

        DebugablePortalWorld world = null;

        float[] viewMat = new float[16];

        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 16.0f);
        Font small_serif = new Font(FontFamily.GenericSansSerif, 8.0f);

        Point lastMousePos = Point.Empty;
        Point thisMousePos = new Point(0, 0);

        public GameWindow() : base(1024,768)
        {
            InitializeComponent();
            Mouse.Move += new MouseMoveEventHandler(Mouse_Move);
        }

        void Mouse_Move(MouseDevice sender, MouseMoveEventArgs e)
        {
            if (lastMousePos == Point.Empty)
                lastMousePos = new Point(e.X, e.Y);

            thisMousePos = new Point(thisMousePos.X + e.XDelta, thisMousePos.Y + e.YDelta);
        }

        protected override void SetupGL()
        {
            base.SetupGL();

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

        protected override void InitGame()
        {
            world = new DebugablePortalWorld();
            Cursor.Position = new Point(Location.X + (Width / 2), Location.X + (Height / 2));
        }

        protected override void Resized(EventArgs e)
        {
            camera.Resize(Width, Height);
        }

        protected bool doInput(UpdateFrameArgs e)
        {
            if (Keyboard[Keys.Escape])
                return true;

            float turnSpeed = 40.0f;
            turnSpeed *= (float)e.TimeDelta;

            Point delta = thisMousePos;
            thisMousePos = new Point(0,0);

            float sensitivity = 0.1f;

            camera.turn(-turnSpeed * sensitivity * delta.Y, -turnSpeed * sensitivity * delta.X);
           
            Vector3 forward = new Vector3(camera.Heading());
            Vector3 leftward = new Vector3(forward);
            leftward.X = -forward.Y;
            leftward.Y = forward.X;

            Vector2 movement = new Vector2();

            float speed = 15.0f;
            speed *= (float)e.TimeDelta;

            if (Keyboard[Keys.A])
                movement.X = 1;
            if (Keyboard[Keys.D])
                movement.X = -1;
            if (Keyboard[Keys.W])
                movement.Y = 1;
            if (Keyboard[Keys.S])
                movement.Y = -1;

            if (Keyboard[Keys.PageUp])
                camera.move(0, 0, speed);
            if (Keyboard[Keys.PageDown])
                camera.move(0, 0, -speed);

            Vector3 incremnt = new Vector3();
            incremnt += forward * movement.Y * speed;
            incremnt += leftward * movement.X * speed;

            camera.move(incremnt);
            return false;
        }

        public override void OnUpdateFrame(UpdateFrameArgs e)
        {
            base.OnUpdateFrame(e);
            if (doInput(e))
                Exit();
        }

        public override void OnRenderFrame(RenderFrameArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            camera.Execute();

            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(10, 15, 10, 1.0f));

            if (clipingFrustum != null)
                clipingFrustum.drawFrustum();
            
            drawWorld();

            GL.Clear(ClearBufferMask.DepthBufferBit);

            SwapBuffers();
        }

        public void drawWorld()
        {
            if (world != null)
            {
                world.drawGround();
                world.draw();
            }
        }
    }

    public class DebugablePortalWorld
    {
        Grid grid = new Grid();
        float groundSize = 100f;

        public void FinalizeWorld()
        {
        }

        public float Size
        {
            get { return groundSize; }
        }

        public void Setup(float ground, float majorSpace, float minorSpace)
        {
            groundSize = ground;

            grid.gridSize = groundSize;
            grid.majorSpacing = majorSpace;
            grid.minorSpacing = minorSpace;
            grid.alpha = 0.5f;
        }

        public void drawGround()
        {
            GL.Disable(EnableCap.Lighting);
            grid.Exectute();
            GL.Enable(EnableCap.Lighting);
        }

        public int draw()
        {
            return 0;
        }
    }

    public class DebugableVisibleFrustum : VisibleFrustum
    {
        public DebugableVisibleFrustum ( VisibleFrustum val ) : base(val)
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
}
