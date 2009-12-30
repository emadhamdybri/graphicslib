using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.Textures;

namespace Project24
{
    class Cloud
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Velocity = Vector3.Zero; // XY speed + rotation speed
        public float Scale = 1.0f;
        public float Rotation = 0;

        public Texture texture = null;

        protected Vector2 size = Vector2.Zero;

        public void Update ( double time )
        {
            float z= Position.Z;
            Position += Velocity * (float)time;
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
            GL.Begin(BeginMode.Quads);
                GL.Normal3(0, 0, 1);
                GL.TexCoord2(0, 0);
                GL.Vertex2(-size.X, -size.Y);
                GL.TexCoord2(0, -1);
                GL.Vertex2(-size.X, size.Y);
                GL.TexCoord2(1, -1);
                GL.Vertex2(size.X, size.Y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(size.X, -size.Y);
            GL.End();
        }
    }

    class Cloudscape
    {
        public static float CloudPixelScale = 100f;

        List<List<Cloud>> CloudLayers = new List<List<Cloud>>();

        Vector2 bounds;

        List<Texture> CloudTextures = new List<Texture>();

        public Cloudscape(Vector2 b)
        {
            bounds = b;

            CloudTextures.Add(TextureSystem.system.GetTexture(ResourceManager.FindFile("background/Cloud1.png")));

            for (int layer = 0; layer < 3; layer++)
            {
                int seed = new Random().Next(10)+5*layer;

                List<Cloud> layerList = new List<Cloud>();
                CloudLayers.Add(layerList);

                for ( int i = 0; i < seed; i++ )
                {
                    Cloud cloud = new Cloud();
                    cloud.texture = CloudTextures[new Random().Next(CloudTextures.Count)];
                }
            }
        }

        public void Render ( double time, Vector3 movement )
        {

        }
    }
}
