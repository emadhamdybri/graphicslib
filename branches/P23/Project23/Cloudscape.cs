using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.Textures;
using Math3D;

using Utilities.Paths;

namespace Project23
{
    class Cloud
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Velocity = Vector3.Zero; // XY speed + rotation speed
        public float Scale = 1.0f;
        public float Rotation = 0;

        public Texture texture = null;

        protected Vector2 size = Vector2.Zero;

        public void Update ( double time, Vector3 movement )
        {
            float z= Position.Z;
            Position += (Velocity * (float)time) + (movement*(float)time);
            Position.Z = z;
            Rotation += Velocity.Z * (float)time;
        }

        public void Draw ()
        {
            if (texture == null)
                return;

            if (size == Vector2.Zero)
                size = new Vector2(texture.Width / Cloudscape.CloudPixelScale, texture.Height / Cloudscape.CloudPixelScale);

            texture.Bind();
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation, 0, 0, 1);
            GL.Color4(1,1,1,0.95f);
            GL.Begin(BeginMode.Quads);
                GL.Normal3(0, 0, 1);
                GL.TexCoord2(0, -1);
                GL.Vertex3(-size.X * Scale, size.Y * Scale, 0);
               
                GL.TexCoord2(0, 0);
                GL.Vertex3(-size.X * Scale, -size.Y * Scale, 0);

                GL.TexCoord2(1, 0);
                GL.Vertex3(size.X * Scale, -size.Y * Scale, 0);

                GL.TexCoord2(1, -1);
                GL.Vertex3(size.X * Scale, size.Y * Scale, 0);

            GL.End();

            GL.PopMatrix();
        }
    }

    class Cloudscape
    {
        public static float CloudPixelScale = 50f;
        protected static float WindSpeed = 0.5f;
        protected static float RotationSpeed = 4f;
        protected static float CloudScale = 1.5f;

        public static int CloudSeedFactor = 25;

        List<List<Cloud>> CloudLayers = new List<List<Cloud>>();

        Vector2 bounds;

        List<Texture> CloudTextures = new List<Texture>();

        Vector2 Wind = Vector2.Zero;
        Vector2 RootWind = Vector2.Zero;

        public Cloudscape(Vector2 b)
        {
            bounds = b;

            Random rand = new Random();

            WindSpeed = FloatHelper.Random(WindSpeed,WindSpeed*2f);

            RootWind = new Vector2(((float)rand.NextDouble() * 2f - 1f)*WindSpeed,((float)rand.NextDouble() * 2f - 1f)*WindSpeed);

            CloudTextures.Add(TextureSystem.system.GetTexture(ResourceManager.FindFile("background/Cloud1.png")));
            CloudTextures.Add(TextureSystem.system.GetTexture(ResourceManager.FindFile("background/Cloud4.png")));
            CloudTextures.Add(TextureSystem.system.GetTexture(ResourceManager.FindFile("background/Cloud2.png")));
            CloudTextures.Add(TextureSystem.system.GetTexture(ResourceManager.FindFile("background/Cloud3.png")));
            CloudTextures.Add(TextureSystem.system.GetTexture(ResourceManager.FindFile("background/Cloud4.png")));

            for (int layer = 0; layer < 3; layer++)
            {
                int seed = rand.Next(CloudSeedFactor) + ((CloudSeedFactor/2) * layer);

                List<Cloud> layerList = new List<Cloud>();
                CloudLayers.Add(layerList);

                float layerZ = -10+(layer*1f);

                for ( int i = 0; i < seed; i++ )
                {
                    Cloud cloud = new Cloud();
                    if (CloudTextures.Count == 1)
                        cloud.texture = CloudTextures[0];
                    else
                        cloud.texture = CloudTextures[rand.Next(CloudTextures.Count-1)];
                    cloud.Position = new Vector3(FloatHelper.Random(-bounds.X, bounds.X), FloatHelper.Random(-bounds.Y, bounds.Y), FloatHelper.Random(layerZ, layerZ + 1f));

                    SetCloudRandomElements(cloud, rand);
                    layerList.Add(cloud);
                }
            }
        }

        protected void SetCloudRandomElements ( Cloud cloud, Random rand )
        {
            Vector2 windVariance = new Vector2((float)rand.NextDouble() * 2f - 1f, (float)rand.NextDouble() * 2f - 1f);

            cloud.Velocity = new Vector3(RootWind.X + windVariance.X * WindSpeed * 0.25f, RootWind.Y + windVariance.Y * WindSpeed * 0.25f, ((float)rand.NextDouble() * RotationSpeed * 2) - RotationSpeed);
            cloud.Scale = FloatHelper.Random(1, CloudScale);
        }

        protected void WrapCloud ( Cloud cloud )
        {
            bool reset = false;

            if (cloud.Position.X > bounds.X)
            {
                cloud.Position.X = -bounds.X;
                reset = true;
            }
            else if (cloud.Position.X < -bounds.X)
            {
                cloud.Position.X = bounds.X;
                reset = true;
            }

            if (cloud.Position.Y > bounds.Y)
            {
                cloud.Position.Y = -bounds.Y;
                reset = true;
            }
            else if (cloud.Position.X < -bounds.Y)
            {
                cloud.Position.Y = bounds.Y;
                reset = true;
            }

            if (reset)
                SetCloudRandomElements(cloud, new Random());
        }

        protected void Update(double time, Vector3 movement)
        {
            int layer = 0;
            foreach(List<Cloud> layerList in CloudLayers)
            {
                Vector3 vec = movement * (layer + 1) * 0.25f;
                foreach(Cloud cloud in layerList)
                {
                    cloud.Update(time, vec);
                    WrapCloud(cloud);
                }
                layer++;
            }
        }

        public void Render ( double time, Vector3 movement )
        {
            Update(time, movement);

            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);

            GL.Disable(EnableCap.DepthTest);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(0, 0, bounds.X * -0.5f);
            foreach (List<Cloud> layerList in CloudLayers)
                foreach (Cloud cloud in layerList)
                    cloud.Draw();

            GL.Enable(EnableCap.DepthTest);

            GL.PopMatrix();
        }
    }
}
