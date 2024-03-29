﻿/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using World;

namespace PortalEdit
{
    public class UndoObject
    {
        public string Descrioption
        {
            get {return descrioption;}
        }

        protected string descrioption = "Action";

        public virtual void Undo ( )
        {
        }
     }

    public delegate void UndoStateChangeEvent ( object sender, bool available );

    public class Undo
    {
        public static Undo System = new Undo();

        public event UndoStateChangeEvent UndoStateChanged;

        protected List<UndoObject> Undos = new List<UndoObject>();

        public int Count 
        {
            get { return Undos.Count; } 
        }

        public string Description
        {
            get 
            {
                if (Undos.Count == 0)
                    return string.Empty;

                return Undos[0].Descrioption;
            }
        }

        public void CullUndos ()
        {
            if (Settings.settings.UndoLevels > 0)
            {
                if (Undos.Count > Settings.settings.UndoLevels)
                    Undos.RemoveRange(Settings.settings.UndoLevels, Undos.Count - Settings.settings.UndoLevels);
            }
        }
 
        public void Add ( UndoObject undo )
        {
            Editor.SetDirty();
            Undos.Insert(0, undo);

            CullUndos();

            if (UndoStateChanged != null)
                UndoStateChanged(this, Undos.Count > 0);
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
            descrioption = "Delete Cell";

            cell = new EditorCell();
            cell.ID.GroupName = string.Copy(cell.ID.GroupName);
            cell.ID.CellName = string.Copy(cell.ID.CellName);

            foreach(CellVert v in c.Verts)
                cell.Verts.Add(new CellVert(v));

            foreach (CellEdge edge in c.Edges)
                cell.Edges.Add(new CellEdge(edge));

            cell.Group = null;
        }

        public override void Undo()
        {
            cell.Group = Editor.instance.map.FindGroup(cell.ID.GroupName);
            if (cell.Group != null)
            {
                cell.Group.Cells.Add(cell);
                Editor.instance.RebuildMap();
            }
        }
    }

    public class CellAddUndo : UndoObject
    {
        string group;
        string cell;
        Polygon polygon;

        public CellAddUndo(EditorCell c, Polygon poly)
        {
            descrioption = "Add Cell";

            polygon = poly;

            cell = string.Copy(c.ID.CellName);
            group = string.Copy(c.ID.GroupName);
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);
            if (g != null)
            {
                EditorCell c = (EditorCell)g.FindCell(cell);
                if (c != null)
                {
                    Editor.instance.mapRenderer.incompletePoly = polygon;
                    c.Dispose();
                    Editor.instance.map.RemoveCell(c);
                    Editor.instance.RebuildMap();
                }
            }
        }
    }


    public class GroupAddUndo : UndoObject
    {
        string group;

        public GroupAddUndo(CellGroup c)
        {
            descrioption = "Add Group";
            group = string.Copy(c.Name);
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

                Editor.instance.map.CellGroups.Remove(g);
                Editor.instance.RebuildMap();
            }
        }
    }

    public class VertexDataEditUndo : UndoObject
    {
        string cellGroup;
        string cellName;

        CellVert vert;
        int vertIndex;

        public VertexDataEditUndo(EditorCell c, int index)
        {
            descrioption = "Edit Vertex";

            cellGroup = string.Copy(c.ID.GroupName);
            cellName =  string.Copy(c.ID.CellName);

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

            Editor.instance.RebuildMap();
        }
    }

    public class IncrementalHeightsUndo : UndoObject
    {
        public string cellGroup;
        public string cellName;

        bool state;

        public IncrementalHeightsUndo(EditorCell c)
        {
            descrioption = "Edit Cell Data";

            cellGroup = string.Copy(c.ID.GroupName);
            cellName = string.Copy(c.ID.CellName);

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

    public class MapVertAddUndo : UndoObject
    {
        List<Vector2> polyList;

        public MapVertAddUndo(Polygon p)
        {
            descrioption = "Add Vertex";
            polyList = new List<Vector2>(p.Verts);
        }

        public override void Undo()
        {
            Editor.instance.mapRenderer.incompletePoly.Verts = new List<Vector2>(polyList);
        }
    }

    public class CellGroupChangeUndo : UndoObject
    {
        string group;
        string cell;
        string oldGroup;

        public CellGroupChangeUndo(Cell c, string newGroup)
        {
            descrioption = "Change Cell Group";
            cell = string.Copy(c.ID.CellName);
            group =string.Copy(newGroup);
            oldGroup = string.Copy(c.ID.GroupName);
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);

            if (g == null)
                return;

            Cell c = g.FindCell(cell);

            c.Group.Cells.Remove(c);

            CellGroup ng = Editor.instance.map.FindGroup(oldGroup);
            c.Group = ng;
            c.ID.GroupName = ng.Name;
            ng.Cells.Add(c);
          
            Editor.instance.RebuildMap();
        }
    }

    public class GroupRenameUndo : UndoObject
    {
        string group;
        string oldName;

        public GroupRenameUndo(string _oldName, string _newName)
        {
            descrioption = "Change Group Name";
            group = _newName;
            oldName = string.Copy(_oldName);
        }

        public override void Undo()
        {
            Editor.instance.RenameGroup(group, oldName);
        }
    }

    public class CellRenameUndo : UndoObject
    {
        string group;
        string cellName;

        string oldName;

        public CellRenameUndo(Cell cell, string newName)
        {
            descrioption = "Change Cell Name";

            group = string.Copy(cell.ID.GroupName);
            oldName = string.Copy(cell.ID.CellName);
            cellName = string.Copy(newName);
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);
            EditorCell c = (EditorCell)g.FindCell(cellName);

            Editor.instance.RenameCell(c, oldName);
        }
    }

    public class CellVertXYEditUndo : UndoObject
    {
        string group;
        string cellName;

        Vector2 oldPos;
        int     vertIndex;

        public CellVertXYEditUndo(Cell cell, int i)
        {
            descrioption = "Edit Vert";

            group = string.Copy(cell.ID.GroupName);
            cellName = string.Copy(cell.ID.CellName);

            oldPos = new Vector2(cell.Verts[i].Bottom.X,cell.Verts[i].Bottom.Y);
            vertIndex = i;
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);
            EditorCell c = (EditorCell)g.FindCell(cellName);

            Editor.instance.SetCellVertXY(oldPos,vertIndex,c);
        }
    }

    public class EdgeVisUndo : UndoObject
    {
        string group;
        string cellName;

        int edge;
        bool vis;

        public EdgeVisUndo(Cell cell, int i)
        {
            descrioption = "Edit Edge Visibility";

            group = string.Copy(cell.ID.GroupName);
            cellName = string.Copy(cell.ID.CellName);

            edge = i;
            if (edge >= 0)
                vis = cell.Edges[i].Vizable;
            else if (edge == -1)
                vis = cell.RoofVizable;
            else
                vis = cell.FloorVizable;
        }

        public override void Undo()
        {
            CellGroup g = Editor.instance.map.FindGroup(group);
            EditorCell c = (EditorCell)g.FindCell(cellName);

            if (edge >= 0)
                c.Edges[edge].Vizable = vis;
            else if (edge == -1)
                c.RoofVizable = vis;
            else
                c.FloorVizable = vis;

            c.GenerateDisplayGeometry();
        }
    }
}
