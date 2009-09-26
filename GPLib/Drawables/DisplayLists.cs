using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics.OpenGL;

namespace Drawables.DisplayLists
{
    public class DisplayList
    {
        int listID = -1;

        bool generating = false;

        public bool Valid ()
        {
            return listID != -1;
        }

        public void Invalidate()
        {
            if (generating)
                End();

            if (listID != -1)
                GL.DeleteLists(listID,1);

            listID = -1;
        }

        public void Start ()
        {
            Start(false);
        }

        public void Start( bool execute )
        {
            Invalidate();
            listID = GL.GenLists(1);
            if (execute)
                GL.NewList(listID, ListMode.CompileAndExecute);
            else
                GL.NewList(listID, ListMode.Compile);

            generating = true;
        }

        public void End( )
        {
            if (generating)
                GL.EndList();

            generating = false;
        }

        public bool Call()
        {
            if (generating || !Valid())
                return false;

            GL.CallList(listID);
            return true;
        }
    }

    public class DisplayListSystem
    {
        public static DisplayListSystem system = new DisplayListSystem();

        List<DisplayList> displayLists = new List<DisplayList>();

        public void Invalidate()
        {
            foreach (DisplayList d in displayLists)
                d.Invalidate();
        }

        public void Flush()
        {
            foreach (DisplayList d in displayLists)
                d.Invalidate();

            displayLists.Clear();
        }

        public DisplayList newList ()
        {
            DisplayList d = new DisplayList();
            displayLists.Add(d);
            return d;
        }

        public void deleteList( DisplayList d )
        {
            displayLists.Remove(d);
        }
    }

    public class ListableItem : IDisposable
    {
        protected DisplayList list = new DisplayList();

        public void Dispose()
        {
            if (list != null)
            {
                DisplayListSystem.system.deleteList(list);
                list = null;
            }
        }

        public void Invalidate()
        {
            list.Invalidate();
        }

        protected void Rebuild()
        {
            list.Start();
            GenerateList();
            list.End();
        }

        protected virtual void GenerateList()
        {

        }

        public virtual void Execute()
        {
            if (!list.Valid())
                Rebuild();

            list.Call();
        }
    }

    public class ListableEvent : IDisposable
    {
        public delegate void GenerateEventHandler(object sender, DisplayList list);
        public event GenerateEventHandler Generate;

        public object tag = null;

        protected DisplayList list = DisplayListSystem.system.newList();

        public void Dispose()
        {
            Invalidate();

            if (list != null)
            {
                DisplayListSystem.system.deleteList(list);
                list = null;
            }

            if (tag != null)
                tag = null;
        }

        public void Invalidate ()
        {
            if (list != null)
                list.Invalidate();
        }

        public void Call ()
        {
            if (Generate == null)
                return;

            if (!list.Valid())
            {
                list.Start(true);
                Generate(this, list);
                list.End();
            }
            else
                list.Call();
        }
    }
}
