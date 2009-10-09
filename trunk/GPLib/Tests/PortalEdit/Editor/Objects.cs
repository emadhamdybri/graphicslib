using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Math3D;
using OpenTK.Graphics;

using World;

namespace PortalEdit
{
    public class MapObjectHandler
    {
        public String Name
        {
            get {return name;}
        }

        protected String name = "MapObject";

        public virtual ObjectInstance New ()
        {
            ObjectInstance inst = new ObjectInstance();
            inst.ObjectType = name;
            return inst;
        }

        public virtual void EditorDraw (ObjectInstance obj)
        {
            return;
        }
    }

    public class Objects
    {
        public static Dictionary<String,MapObjectHandler> Handlers = new Dictionary<string,MapObjectHandler>();
        public static MapObjectHandler Default = new MapObjectHandler();

        public static void RegisterDefaults ()
        {
            Register(new SpawnObjectHandler());
        }

        public static void Register ( MapObjectHandler handler )
        {
            Handlers.Add(handler.Name, handler);
        }

        public static MapObjectHandler GetHandler ( string name )
        {
            if (Handlers.ContainsKey(name))
                return Handlers[name];

            return Default;
        }

        public static MapObjectHandler GetHandler ( ObjectInstance obj )
        {
            return GetHandler(obj.ObjectType);
        }

        public static void EditorDraw (ObjectInstance obj)
        {
            GetHandler(obj).EditorDraw(obj);
        }
    }

    public class SpawnObjectHandler : MapObjectHandler
    {
        public SpawnObjectHandler()
        {
            name = "Spawn"; 
        }

        public override void EditorDraw(ObjectInstance obj)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(0, 0, 1, 0.75f);
            IntPtr quadric = Glu.NewQuadric();
            GL.PushMatrix();
            GL.Translate(obj.Postion);
            GL.Translate(0,0,0.5);
            GL.Scale(1, 1, 2);
            Glu.Sphere(quadric, 0.25, 4, 2);
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }
    }
}
