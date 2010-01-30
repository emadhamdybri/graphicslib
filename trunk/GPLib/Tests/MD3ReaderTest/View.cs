using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MD3;

using OpenTK;
using OpenTK.Graphics;

using Drawables.DisplayLists;
using Math3D;

namespace MD3ReaderTest
{
    public partial class View : Form
    {
        Character character;
        CharacterInstance instance;

        ModelTree weapon;
        AnimatedInstance weaponInstance;


        Vector3 offset = new Vector3(0, 0, 5);
        Vector2 rotation = new Vector2(60, 30);
        float pullback = 100f;
        Point lastMouse = Point.Empty;

        int fps = 120;

        float GridSubDivisions = 1.0f;
        float GridSize = 50;
        float GridStep = 2;

        Timer frameTimer;

        string dir = string.Empty;
        public View(string[] args)
        {
           CharacterInstance.ProcessSurfacePath = new ProcessSurfacePathHandler(GetTexturePath);
           CharacterInstance.DrawTags = true;

           InitializeComponent();

           string model = "tifa";
           if (args.Length > 0)
               model = args[0];

            character = Reader.Read(new DirectoryInfo(model));
            if (character == null)
            {
                MessageBox.Show("Unable to read file " + model);
                Application.Exit();
                return;
            }
            instance = new CharacterInstance(character);
            instance.InterpolateMeshes = true;
         //   instance.InterpolateNormals = true;
            instance.InterpolateTagPosition = true;
         //   instance.InterpolateTagRotation = true;

            dir = model;
            
            if (args.Length > 1)
                instance.SetSkin(args[1]);

            instance.AnimationEnded += new AnimatedInstance.SequenceEvent(instance_AnimationEnded);
            instance.FrameChanged += new AnimatedInstance.SequenceFrameEvent(instance_FrameChanged);
            glControl1.MouseWheel += new MouseEventHandler(glControl1_MouseWheel);

            weapon = Reader.Read(new DirectoryInfo("railgun"), false, "weapon");
            if (weapon != null && instance != null)
                instance.AttatchWeapon(weapon);
        }

        void instance_FrameChanged(AnimatedInstance sender, int frame, ComponentType part)
        {
            if (part == ComponentType.Torso)
                TorsoFrame.Text = frame.ToString();
            else if (part == ComponentType.Legs)
                LegsFrame.Text = frame.ToString();
        }

        void instance_AnimationEnded(AnimatedInstance sender, string name, ComponentType part)
        {
            if (part == ComponentType.Torso && torsoForceLoop.Checked)
                instance.SetTorsoSequence(name);
            else if (part == ComponentType.Legs && legsForceLoop.Checked)
                instance.SetLegSequence(name);
        }

        string GetTexturePath ( ModelTree model, string path )
        {
            if (weapon == model)
                return Path.Combine("railgun", Path.GetFileName(path));
            return Path.Combine(dir,Path.GetFileName(path));
        }

        void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomSpeed = 5f;
            if (Control.ModifierKeys == Keys.Shift)
                zoomSpeed *= 5f;

            pullback += ((float)e.Delta / 120f) * zoomSpeed;
            if (pullback < 0)
                pullback = 0;
            Render3dView();
        }

        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            float rotSpeed = 0.1f;
            float moveSpeed = 0.002f;

            if (e.Button == MouseButtons.Right)
            {
                rotation.X -= (e.X - lastMouse.X) * rotSpeed;
                rotation.Y += (e.Y - lastMouse.Y) * rotSpeed;
            }

            if (e.Button == MouseButtons.Middle)
            {
                float deg2Rad = (float)Math.PI / 180.0f;
                float x = 0;
                float y = 0;

                float XDelta = (e.X - lastMouse.X) * moveSpeed * pullback;
                float YDelta = (e.Y - lastMouse.Y) * moveSpeed * pullback;

                x = (float)Math.Cos(rotation.X * deg2Rad) * XDelta;
                y = -(float)Math.Sin(rotation.X * deg2Rad) * XDelta;

                offset.X -= x;
                offset.Y += y;


                x = -(float)Math.Cos((rotation.X + 90) * deg2Rad) * YDelta;
                y = (float)Math.Sin((rotation.X + 90) * deg2Rad) * YDelta;

                offset.X -= x;
                offset.Y += y;

            }
            Render3dView();

            lastMouse = new Point(e.X, e.Y);
        }


        private void glControl1_Load(object sender, EventArgs e)
        {

        }

        private void View_Load(object sender, EventArgs e)
        {
            SetupGL();

            SetViewPort();
            Render3dView();

            frameTimer = new Timer();
            frameTimer.Interval = 1000 / fps;
            frameTimer.Tick += new EventHandler(frameTimer_Tick);
            frameTimer.Start();

            torsoList.Items.Clear();
            legsList.Items.Clear();

            AnimationSequence torsoSelect = null;
            AnimationSequence legSelet = null;

            if (character.Sequences.ContainsKey("torso"))
            {
                foreach (AnimationSequence seq in character.Sequences["torso"])
                {
                    torsoList.Items.Add(seq);
                    if (seq.Name == "stand")
                        torsoSelect = seq;
                }
            }

            if (character.Sequences.ContainsKey("legs"))
            {
                foreach (AnimationSequence seq in character.Sequences["legs"])
                {
                    legsList.Items.Add(seq);
                    if (seq.Name == "idle")
                        legSelet = seq;
                }
            }

            torsoList.SelectedItem = torsoSelect;
            legsList.SelectedItem = legSelet;
        }

        void frameTimer_Tick(object sender, EventArgs e)
        {
            Render3dView();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            SetViewPort();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Render3dView();
        }

        protected virtual void SetupGL()
        {
            GL.ClearColor(Color.SkyBlue);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);

            // setup light 0
            Vector4 lightInfo = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(1f, 1f, 1f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
        //    GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);
        }

        protected virtual void SetViewPort()
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height); // Use all of the glControl painting area  

            SetPerspective();
        }

        void SetPerspective()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float aspect = (float)glControl1.Width / (float)glControl1.Height;

            Glu.Perspective(45, aspect, 1f, 1000f);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        void SetOrthographic()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, glControl1.Width, 0, glControl1.Height, 0, 100);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected void SetCamera()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(0, 0, -pullback);								// pull back on allong the zoom vector
            GL.Rotate(rotation.Y, 1.0f, 0.0f, 0.0f);					// pops us to the tilt
            GL.Rotate(-rotation.X, 0.0f, 1.0f, 0.0f);					// gets us on our rot
            GL.Translate(-offset.X, -offset.Z, offset.Y);	// take us to the pos
            GL.Rotate(-90, 1.0f, 0.0f, 0.0f);							// gets us into XY
        }

        public void Setup3dView()
        {
            SetPerspective();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            SetCamera();
        }

        protected void DrawGridAxisMarker()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.LineSmooth);

            GL.LineWidth(2);
            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.Blue);
            GL.Vertex3(-1, 0, 0);
            GL.Vertex3(2, 0, 0);

            GL.Color3(Color.Red);
            GL.Vertex3(0, -1, 0);
            GL.Vertex3(0, 2, 0);

            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, -1);
            GL.Vertex3(0, 0, 2);
            GL.End();

            GL.Enable(EnableCap.LineSmooth);
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
        }

        void GridList_Generate(object sender, DisplayList list)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);
            GL.DepthMask(false);

            float gridSize = GridSize;

            GL.PushMatrix();
            GL.Translate(0, 0, 0);

            GL.Color4(Color.FromArgb(128, Color.LightSlateGray));
            GL.LineWidth(1);

            if (GridSubDivisions > 0)
            {
                GL.Begin(BeginMode.Lines);

                for (float i = 0; i < gridSize; i += GridSubDivisions)
                {
                //    if (i - (int)i < GridSubDivisions)
                 //       continue;

                    GL.Vertex2(gridSize, i);
                    GL.Vertex2(-gridSize, i);

                    GL.Vertex2(gridSize, -i);
                    GL.Vertex2(-gridSize, -i);

                    GL.Vertex2(i, gridSize);
                    GL.Vertex2(i, -gridSize);

                    GL.Vertex2(-i, gridSize);
                    GL.Vertex2(-i, -gridSize);
                }
                GL.End();
            }
            GL.LineWidth(2);
            GL.Begin(BeginMode.Lines);
            GL.Color4(Color.FromArgb(128, Color.LightGray));

            for (float i = 0; i < gridSize; i += GridStep)
            {
                GL.Vertex2(gridSize, i);
                GL.Vertex2(-gridSize, i);

                GL.Vertex2(gridSize, -i);
                GL.Vertex2(-gridSize, -i);

                GL.Vertex2(i, gridSize);
                GL.Vertex2(i, -gridSize);

                GL.Vertex2(-i, gridSize);
                GL.Vertex2(-i, -gridSize);
            }
            GL.End();

            GL.LineWidth(3);
            GL.Begin(BeginMode.Lines);
            GL.Color3(Color.Snow);
            GL.Vertex3(-gridSize, 0, 0);
            GL.Vertex3(gridSize, 0, 0);
            GL.Vertex3(0, -gridSize, 0);
            GL.Vertex3(0, gridSize, 0);
            GL.End();

            GL.Enable(EnableCap.LineSmooth);
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
        }

        public void Render3dView()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Setup3dView();
            GL.PushMatrix();

            GL.Enable(EnableCap.Light0);
            Vector4 lightPos = new Vector4(100, -200, 200, 0);
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);
            GridList_Generate(null, null);
            DrawGridAxisMarker();

            if (instance != null)
                instance.Draw();
            GL.PopMatrix();

            glControl1.SwapBuffers();

            if (instance != null)
                FrameTime.Text = (1.0/instance.FrameTime).ToString();
        }

        private void torsoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (instance == null)
                return;

            if (torsoList.SelectedItem == null)
                instance.SetTorsoSequence(string.Empty);
            else
            {
                AnimationSequence seq = (AnimationSequence)torsoList.SelectedItem;
                instance.SetTorsoSequence(seq.Name);

                TorsoStartFrame.Text = seq.StartFrame.ToString();
                TorsoEndFrame.Text = seq.EndFrame.ToString();
                TorsoLoopFrame.Text = seq.LoopPoint.ToString();
            }

            Render3dView();
        }

        private void legsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (instance == null)
                return;

            if (legsList.SelectedItem == null)
                instance.SetTorsoSequence(string.Empty);
            else
            {
                AnimationSequence seq = (AnimationSequence)legsList.SelectedItem;
                instance.SetLegSequence(seq.Name);

                LegsStartFrame.Text = seq.StartFrame.ToString();
                LegsEndFrame.Text = seq.EndFrame.ToString();
                LegsLoopFrame.Text = seq.LoopPoint.ToString();
            }

            Render3dView();
        }

        private void ComputeLegTorsoMatrix ()
        {
            instance.LegTorsoMatrix = Matrix4.CreateRotationZ(Trig.DegreeToRadian((float)LegTorsoSpin.Value)) * Matrix4.CreateRotationY(Trig.DegreeToRadian((float)LegTorsoTilt.Value));

        }

        private void LegTorsoSpin_ValueChanged(object sender, EventArgs e)
        {
            if (instance == null)
                return;

            ComputeLegTorsoMatrix();
        }

        private void LegTorsoTilt_ValueChanged(object sender, EventArgs e)
        {
            if (instance == null)
                return;

            ComputeLegTorsoMatrix();
        }

        private void SlowMo_CheckedChanged(object sender, EventArgs e)
        {
            if (SlowMo.Checked)
                CharacterInstance.FPSScale = 0.1f;
            else
                CharacterInstance.FPSScale = 1.0f;
        }
    }
}
