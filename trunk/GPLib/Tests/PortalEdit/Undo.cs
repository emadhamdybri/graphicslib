using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortalEdit
{
    public class UndoObject
    {
        public virtual void Undo ( )
        {
        }
     }

    public delegate void UndoStateChangeEvent ( object sender, bool available );

    public class Undo
    {
        public static Undo System = new Undo();

        public event UndoStateChangeEvent UndoStateChanged;

        public int UndoLevels = 25;

        protected List<UndoObject> Undos = new List<UndoObject>();
 
        public void Add ( UndoObject undo )
        {
            Undos.Insert(0, undo);
            if (UndoStateChanged != null)
                UndoStateChanged(this,true);
        }

        public bool UndoAvail ( )
        {
            return Undos.Count > 0;
        }

        public void Apply ( )
        {
            if (Undos.Count == 0)
                return;

            Undos[0].Undo();
            Undos.Remove(Undos[0]);
            
            if (Undos.Count == 0 && UndoStateChanged != null)
                UndoStateChanged(this,false);
        }
    }

    public class CellDeleteUndo : UndoObject
    {
        EditorCell cell;

        public CellDeleteUndo(EditorCell c )
        {
            cell = new EditorCell();
            cell.GroupName = String.Copy(cell.GroupName);
            cell.tag = c.tag;
            cell.Name = String.Copy(cell.Name);

            foreach(CellVert v in c.Verts)
                cell.Verts.Add(new CellVert(v));

            foreach (CellEdge edge in c.Edges)
                cell.Edges.Add(new CellEdge(edge));

            cell.Group = null;
        }

        public override void Undo()
        {
            cell.Group = Editor.instance.map.FindGroup(cell.GroupName);
            if (cell.Group != null)
            {
                cell.Group.Cells.Add(cell);
                Editor.instance.RebuildMap();
            }
        }
    }

    public class CellAddUndo : UndoObject
    {
        String group;
        String cell;

        public CellAddUndo(EditorCell c)
        {
            cell = String.Copy(c.Name);
            group = String.Copy(c.GroupName);
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);
            if (g != null)
            {
                EditorCell c = (EditorCell)g.FindCell(cell);
                if (c != null)
                {
                    c.Dispose();
                    Editor.instance.map.RemoveCell(c);
                    Editor.instance.RebuildMap();
                }
            }
        }
    }


    public class GroupAddUndo : UndoObject
    {
        String group;

        public GroupAddUndo(CellGroup c)
        {
            group = String.Copy(c.Name);
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);
            if (g != null)
            {
                foreach (EditorCell c in g.Cells)
                {
                    c.Dispose();
                    c.Edges.Clear();
                }

                g.Cells.Clear();

                Editor.instance.map.cellGroups.Remove(g);
                Editor.instance.RebuildMap();
            }
        }
    }

    public class VertexDataEditUndo : UndoObject
    {
        String cellGroup;
        String cellName;

        CellVert vert;
        int vertIndex;

        public VertexDataEditUndo(EditorCell c, int index)
        {
            cellGroup = String.Copy(c.GroupName);
            cellName =  String.Copy(c.Name);

            vert = new CellVert(c.Verts[index]);
            vertIndex = index;
        }

        public override void Undo()
        {
            CellGroup group = Editor.instance.map.FindGroup(cellGroup);
            if (group != null)
            {
                Cell cell = group.FindCell(cellName);
                if (cell != null)
                    cell.Verts[vertIndex] = vert;
            }
        }
    }

    public class IncrementalHeightsUndo : UndoObject
    {
        public String cellGroup;
        public String cellName;

        bool state;

        public IncrementalHeightsUndo(EditorCell c)
        {
            cellGroup = String.Copy(c.GroupName);
            cellName = String.Copy(c.Name);

            state = c.HeightIsIncremental;
        }

        public override void Undo()
        {
            CellGroup group = Editor.instance.map.FindGroup(cellGroup);
            if (group != null)
            {
                Cell cell = group.FindCell(cellName);
                if (cell != null)
                    cell.HeightIsIncremental = state;
            }
        }
    }
}
