using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Drawables.StaticModels;
using Drawables.StaticModels.OBJ;

using Simulation;

namespace Project23
{
    public class PlayerModelInfo
    {
        public string Name = string.Empty;
        public Vector3 RightEngineOffset = Vector3.Zero;
        public Vector3 LeftEngineOffset = Vector3.Zero;
        public Vector3 RightEngineFlameOffset = Vector3.Zero;
        public Vector3 LeftEngineFlameOffset = Vector3.Zero;
    }

    class PlayerModel
    {
        PlayerModelInfo info;

        StaticModel body;
        StaticModel leftEngine;
        StaticModel rightEngine;
        StaticModel flame;

        public PlayerModel(string dir)
        {
            FileInfo defFile = new FileInfo(ResourceManager.FindFile(Path.Combine(dir, "modeldef.xml")));
            if (!defFile.Exists)
                return;

            FileStream fs = defFile.OpenRead();
            StreamReader reader = new StreamReader(fs);
            info = (PlayerModelInfo)new XmlSerializer(typeof(PlayerModelInfo)).Deserialize(reader);
            fs.Close();

            body = OBJFile.Read(ResourceManager.FindFile(Path.Combine(dir, "body.obj")));
            leftEngine = OBJFile.Read(ResourceManager.FindFile(Path.Combine(dir, "leftEngine.obj")));
            rightEngine = OBJFile.Read(ResourceManager.FindFile(Path.Combine(dir, "rightEngine.obj")));
            flame = OBJFile.Read(ResourceManager.FindFile(Path.Combine(dir, "flame.obj")));
        }

        public void Draw ( double time, float leftRot, float rightRot )
        {
            body.drawAll();
           
            GL.PushMatrix();
                GL.Translate(info.RightEngineOffset);
                GL.Rotate(rightRot, 0, 1f, 0);
                rightEngine.drawAll();
                GL.Translate(info.RightEngineFlameOffset);
                flame.drawAll();
            GL.PopMatrix();

            GL.PushMatrix();
                GL.Translate(info.LeftEngineOffset);
                GL.Rotate(leftRot, 0, 1f, 0);
                leftEngine.drawAll();
                GL.Translate(info.LeftEngineFlameOffset);
                flame.drawAll();
            GL.PopMatrix();
        }
    }

    class PlayerModelInstance
    {
        PlayerModel model;

        public PlayerModelInstance ( PlayerModel m )
        {
            model = m;
        }

        public void Draw ( double time, Player player, Sim sim )
        {
            GL.PushMatrix();
            GL.Translate(player.CurrentState.Position);
            GL.Rotate(player.CurrentState.Rotation, 0, 0, 1f);

            float mainRot = 90f;
            if (player.MovementSpeed > 0.1f)
            {
                mainRot = player.MovementSpeed/ player.ForwardSpeed;
                if (mainRot > 0.85f)
                    mainRot = 1f;
                mainRot *= 90f;
                mainRot = 90f - mainRot;
            }
            else if (player.MovementSpeed < -0.1f)
            {
                mainRot = Math.Abs(player.MovementSpeed)/ player.BackwardSpeed ;
                if (mainRot > 0.85f)
                    mainRot = 1f;
                mainRot *= 45f;
                mainRot += 90f;
            }

            float turnFactor = 0;
            if (player.CurrentState.Spin != 0)
            {
                turnFactor = player.CurrentState.Spin / player.TurningSpeed;
                turnFactor *= 5;
            }
            model.Draw(time, -mainRot + turnFactor, -mainRot - turnFactor);

            GL.PopMatrix();
        }
    }
}
