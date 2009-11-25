﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Math3D;

using Drawables.Cameras;

namespace SkinedModel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new Visual().Run();
        }
    }

    class Bone
    {
        public Matrix4 matrix = Matrix4.Identity;

        public Vector3 translation = Vector3.Zero;
        public Vector3 rotation = Vector3.Zero;

        public List<Vector3> FrameTranslations = new List<Vector3>();
        public List<Vector3> FrameRotations = new List<Vector3>();

        public Matrix4 CachedWorldMatrix
        {
            get
            {
                if (worldMatrixCache == null)
                    CacheWorldMatrix(); 
                return worldMatrixCache;
            }
        }
        Matrix4 worldMatrixCache;


        public Matrix4 CachedWorldMatrixInv
        {
            get
            {
                if (worldMatrixCacheInv == null)
                    CacheWorldMatrix();
                return worldMatrixCacheInv;
            }
        }
        Matrix4 worldMatrixCacheInv;

        Bone Parent = null;
        public List<Bone> Children = new List<Bone>();

        public List<Vector3> Verts = new List<Vector3>();

        public Bone(){}

        public Bone (Matrix4 m):this()
        {
            matrix = m;
        }

        public bool Oprhan()
        {
            return Parent == null;
        }

        public Bone Add(Bone child)
        {
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public int Add(Vector3 vert)
        {
            for(int i = 0; i < Verts.Count; i++)
            {
                if (vert == Verts[i])
                    return i;
            }
            Verts.Add(vert);
            return Verts.Count - 1;
        }

        public Matrix4 WorldMatrix ( )
        {
            if (Parent == null)
                return matrix;

            return matrix * Parent.WorldMatrix();
        }

        public Matrix4 WorldMatrixInv()
        {
            Matrix4 mat = WorldMatrix();
            mat.Invert();
            return mat;
        }

        public void CacheWorldMatrix()
        {
            worldMatrixCache = WorldMatrix();
            worldMatrixCacheInv = WorldMatrixInv();

            foreach (Bone child in Children)
                child.CacheWorldMatrix();
        }

        public Matrix4 GetFrameMatrix ( int frame )
        {
            Matrix4 parrentMatrix = Matrix4.Identity;
            if (Parent != null)
                parrentMatrix = Parent.GetFrameMatrix(frame);

            Matrix4 rotMat = Matrix4.Identity;
            if (frame < FrameRotations.Count)
                rotMat = Matrix4.CreateRotationX(FrameRotations[frame].X) * Matrix4.CreateRotationY(FrameRotations[frame].Y) * Matrix4.CreateRotationZ(FrameRotations[frame].Z);

            Matrix4 transMat = Matrix4.Identity;
            if (frame < FrameTranslations.Count)
                transMat = Matrix4.CreateTranslation(FrameTranslations[frame]);

            return parrentMatrix * matrix * rotMat * transMat;
        }

        public Vector3 GetVert(int index)
        {
            return Verts[index];
        }

        public Vector4 GetVertInvMatrix ( int index )
        {
            return Vector3.Transform(Verts[index], worldMatrixCacheInv);
        }

        public Vector3 GetNormal(Vector3 normal)
        {
            return normal;
        }

        public Vector3 GetNormalInvMatrix(Vector3 normal)
        {
            return Vector3.TransformNormal(normal, worldMatrixCacheInv);
        }
    }

    class Polygon
    {
        public List<int> Verts = new List<int>();
        public List<Vector3> Normals = new List<Vector3>();
        public List<Vector2> UVs = new List<Vector2>();

        public void Add ( int v, Vector3 n, Vector2 t )
        {
            Verts.Add(v);
            Normals.Add(n);
            UVs.Add(t);
        }
    }

    class BoneMesh
    {
        public Color Material = Color.White;

        public List<KeyValuePair<Bone,int> > Verts = new List<KeyValuePair<Bone,int> >();
        public List<Polygon> Faces = new List<Polygon>();
        public bool Show = true;

        public int AddVert (Bone bone, int index )
        {
            Verts.Add(new KeyValuePair<Bone, int>(bone, index));
            return Verts.Count - 1;
        }

        public void Draw( int frame)
        {
            if (!Show)
                return;

            GL.Color3(Material);
            foreach (Polygon face in Faces)
            {
                GL.Begin(BeginMode.Polygon);
                for ( int i = 0; i < face.Verts.Count; i++)
                {
                    Bone bone = Verts[face.Verts[i]].Key;
                    int boneVert = Verts[face.Verts[i]].Value;

                    if (frame < 0)
                    {
                        GL.Normal3(bone.GetNormal(face.Normals[i]));
                        GL.Vertex3(bone.GetVert(boneVert));
                    }
                    else
                    {
                        // transform each vert BACK into the bone
                        Vector4 vec = bone.GetVertInvMatrix(boneVert);
                        Vector3 norm = bone.GetNormalInvMatrix(face.Normals[i]);

                        // get the frame matrix
                        Matrix4 frameMatrix = bone.GetFrameMatrix(frame);

                        // transform from the bone to the frame 
                        vec = Vector3.Transform(new Vector3(vec), frameMatrix);
                        norm = Vector3.TransformNormal(norm, frameMatrix);
                       
                        GL.Normal3(norm.X,norm.Y,norm.Z);
                        GL.Vertex3(vec.X, vec.Y, vec.Z);
                    }
                }
                GL.End();
            }
        }
    }

    class BoneModel
    {
        public Bone Root = new Bone();
        public List<BoneMesh> Meshes = new List<BoneMesh>();

        public int frame = -1;

        public void Draw ()
        {
            Root.CacheWorldMatrix();
            foreach(BoneMesh mesh in Meshes)
                mesh.Draw(frame);
        }

        void DrawBone ( Bone bone )
        {
            GL.PushMatrix();
            GL.MultMatrix(ref bone.matrix);
            GL.Color4(Color.Blue);
            GL.LineWidth(2);

            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0.05f, 0, 0);
            GL.Vertex3(-0.05f, 0, 0);

            GL.Vertex3(0,0.05f, 0);
            GL.Vertex3(0,-0.05f, 0);
          
            GL.Vertex3(0, 0, 0.05f);
            GL.Vertex3(0, 0, -0.05f);
            GL.End();
            GL.LineWidth(1);

            foreach (Bone child in bone.Children)
                DrawBone(child);
            GL.PopMatrix();
        }

        public void DrawSkeliton()
        {
            GL.DepthMask(false);
            GL.Disable(EnableCap.DepthTest);

            DrawBone(Root);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }
    }

    class Visual : GameWindow
    {
        bool loaded = false;

        BoneModel model;

        Camera camera;
        Bone child;

        float bendAngle = 0;
        float bendDir = 1;

        float viewRot = 0;

        void BuildModel()
        {
            model = new BoneModel();
            model.Root.matrix = Matrix4.CreateTranslation(0, 0, 2);

            child = model.Root.Add(new Bone());

            model.Root.Add(new Vector3(1, 0, 1));
            model.Root.Add(new Vector3(0, 0, 0));
            model.Root.Add(new Vector3(1, 0, -1));

            BoneMesh mesh = new BoneMesh();
            mesh.Material = Color.Brown;

            mesh.AddVert(model.Root, 0);
            mesh.AddVert(model.Root, 1);
            mesh.AddVert(model.Root, 2);

            Polygon polygon = new Polygon();
            polygon.Add(0, new Vector3(0, 1, 0), new Vector2(1, 1));
            polygon.Add(1, new Vector3(0, 1, 0), new Vector2(1, 0));
            polygon.Add(2, new Vector3(0, 1, 0), new Vector2(0, 0.5f));

            mesh.Faces.Add(polygon);

            child.Verts.Add(new Vector3(-1, 0, 1));
            child.Verts.Add(new Vector3(-1, 0, -1));

            mesh.AddVert(child, 0);
            mesh.AddVert(child, 1);

            polygon = new Polygon();
            polygon.Add(1, new Vector3(0, 1, 0), new Vector2(0, 0.5f));
            polygon.Add(3, new Vector3(0, 1, 0), new Vector2(1, 1));
            polygon.Add(4, new Vector3(0, 1, 0), new Vector2(1, 0));

            mesh.Faces.Add(polygon);

            model.Meshes.Add(mesh);

            mesh = new BoneMesh();
            mesh.Material = Color.Red;

            mesh.AddVert(model.Root, 0);
            mesh.AddVert(model.Root, 1);
            mesh.AddVert(child, 0);
            mesh.AddVert(model.Root, 2);
            mesh.AddVert(child, 1);

            polygon = new Polygon();
            polygon.Add(0, new Vector3(0, 1, 0), new Vector2(1, 1));
            polygon.Add(2, new Vector3(0, 1, 0), new Vector2(0, 0.5f));
            polygon.Add(1, new Vector3(0, 1, 0), new Vector2(1, 0));
            mesh.Faces.Add(polygon);

            polygon = new Polygon();
            polygon.Add(1, new Vector3(0, 1, 0), new Vector2(0, 0.5f));
            polygon.Add(4, new Vector3(0, 1, 0), new Vector2(1, 1));
            polygon.Add(3, new Vector3(0, 1, 0), new Vector2(1, 0));
            mesh.Faces.Add(polygon);

            model.Meshes.Add(mesh);
        }

        void BuidMilkshapeModel ( string filename )
        {
            MilkshapeModel msModel = new MilkshapeModel();
            if (!msModel.Read(new FileInfo(filename)))
                return;

            model = new BoneModel();

            Dictionary<String, Bone> milkbones = new Dictionary<string, Bone>();

            List<Bone> boneIndexes = new List<Bone>();

            foreach(MilkshapeJoint joint in msModel.Joints)
            {
                Bone bone = new Bone();
                bone.matrix = Matrix4.CreateRotationX(joint.Rotation.X) * Matrix4.CreateRotationY(joint.Rotation.Y) * Matrix4.CreateRotationZ(joint.Rotation.Z) * Matrix4.CreateTranslation(joint.Translation);
                bone.translation = joint.Translation;
                bone.rotation = joint.Rotation;

                foreach(MilkshapeKeyframe keyframe in joint.RotationFrames)
                    bone.FrameRotations.Add(new Vector3(keyframe.Paramater));

                foreach (MilkshapeKeyframe keyframe in joint.TranslationFrames)
                    bone.FrameTranslations.Add(new Vector3(keyframe.Paramater));

                boneIndexes.Add(bone);
                milkbones.Add(joint.Name, bone);
            }

            foreach (MilkshapeJoint joint in msModel.Joints)
            {
                if (milkbones.ContainsKey(joint.ParentName))
                    milkbones[joint.ParentName].Add(milkbones[joint.Name]);
            }

            foreach (KeyValuePair<String,Bone> bone in milkbones)
            {
                if (bone.Value.Oprhan())
                    model.Root.Add(bone.Value);
            }

            milkbones.Clear();

            foreach (MilkshapeGroup group in msModel.Groups)
            {
                BoneMesh mesh = new BoneMesh();
                if (group.MaterialIndex > msModel.Materials.Count)
                    mesh.Material = msModel.Materials[group.MaterialIndex].Diffuse;

                foreach (int index in group.Triangles)
                {
                    MilkshapeTriangle tri = msModel.Triangles[index];
                    Polygon poly = new Polygon();
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 vertex = msModel.Verts[tri.Verts[i]].Location;
                        // find bone
                        Bone bone = model.Root;
                        if (msModel.Verts[tri.Verts[i]].BoneID < boneIndexes.Count)
                            bone = boneIndexes[msModel.Verts[tri.Verts[i]].BoneID];

                        int vert = mesh.AddVert(bone, bone.Add(vertex));
                        poly.Add(vert, tri.Normals[i], tri.UVs[i]);
                    }

                    mesh.Faces.Add(poly);
                }

                model.Meshes.Add(mesh);
           }
           boneIndexes.Clear();
           model.Meshes[3].Show = false;

           model.frame = 0;
        }

        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetupGL();

           // BuildModel();
            BuidMilkshapeModel("../../jill.ms3d");
        }

        void SetupGL()
        {
            GL.ClearColor(System.Drawing.Color.LightBlue);
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

            GL.Enable(EnableCap.Light0);
            // setup light 0
            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            loaded = true;

            camera = new Camera();
            camera.set(new Vector3(0, -2, 0.0f), 0, 90);

            OnResize(EventArgs.Empty);
        }

        protected virtual void SetViewPort()
        {
        }
       
        bool DoInput(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                return true;

            return false;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (DoInput(e))
                Exit();

            bendAngle += bendDir * 30 * (float)e.Time * 2;
            if (Math.Abs(bendAngle) > 30)
            {
                bendAngle = bendDir * 30;
                bendDir *= -1;
            }

           // viewRot += (float)e.Time * 15;

          //  model.frame += (int)e.Time*2;

         //   child.matrix = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(bendAngle));

           // model.Root.matrix = model.Root.matrix * Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)e.Time * 15));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!loaded)
                return;

            GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area  
            camera.Resize(Width, Height);
        }

        protected void Grid()
        {
            GL.Color4(Color.DarkBlue);
            GL.Begin(BeginMode.Lines);

            for (int i = -10; i <= 10; i++)
            {
                GL.Vertex3(i, 10, 0);
                GL.Vertex3(i, -10, 0);
                GL.Vertex3(10, i, 0);
                GL.Vertex3(-10, i, 0);
            }

            GL.End();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            if (!loaded)
                return;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            camera.Execute();

            Grid();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(10, -15, 10, 1.0f));

            DrawModel();

            SwapBuffers();
        }

        private void DrawModel()
        {
            if (model == null)
                return;

            GL.PushMatrix();

            GL.Rotate(90, 1, 0, 0);

            GL.Rotate(viewRot, 0, 1, 0);
            model.Draw();


            GL.Disable(EnableCap.Lighting);

            model.DrawSkeliton();
            GL.PopMatrix();
        }
    }
}