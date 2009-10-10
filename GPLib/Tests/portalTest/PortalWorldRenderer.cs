using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using World;

using Drawables;
using Drawables.DisplayLists;
using Drawables.Materials;

using OpenTK;
using OpenTK.Graphics;

namespace portalTest
{
    public class CellDrawable : IDisposable
    {
        SingleListDrawableItem drawable = null;

        protected void Init (Material mat, int pass)
        {
            drawable = new SingleListDrawableItem(mat, new ListableEvent.GenerateEventHandler(Internal_Generate), pass);
        }

        public void Dispose()
        {
            if (drawable != null)
                drawable.Dispose();

            drawable = null;
        }

        void Internal_Generate ( object sender, DisplayList list )
        {
            Generate();
        }

        protected virtual void Generate ( )
        {
        }

        public void Add ()
        {
            drawable.Add();
        }

        protected Material GetMaterial ( CellMaterialInfo info )
        {
            Material mat = MaterialSystem.system.GetMaterial(info.Material);
            if (mat == null)
                mat = MaterialSystem.system.FromTextureFile(info.Material);
            if (mat == null)
                mat = MaterialSystem.system.GetMaterial("DEFAULT_WHITE");
            if (mat == null)
            {
                mat = MaterialSystem.system.NewMaterial();
                mat.name = "DEFAULT_WHITE";
                mat.baseColor = new GLColor(Color.White);
            }

            return mat;
        }
    }

    public class WallDrawable : CellDrawable
    {
         CellWallGeometry geometry;
         CellEdge edge;
         Cell cell;

         public WallDrawable (CellWallGeometry geo, CellEdge _edge, Cell _cell)
         {
             geometry = geo;
             edge = _edge;
             cell = _cell;

             Init(GetMaterial(geo.Material), DrawablesSystem.MiddlePass);
         }

        protected override void  Generate()
        {
            GL.Begin(BeginMode.Quads);

            CellVert sp = cell.Verts[edge.Start];
            CellVert ep = cell.Verts[edge.End];

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);

            float edgeDistance = cell.EdgeDistance(edge);

            GL.TexCoord2(geometry.Material.GetFinalUV(edgeDistance, geometry.UpperZ[1] - geometry.LowerZ[1]));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, geometry.LowerZ[1]);

            GL.TexCoord2(geometry.Material.GetFinalUV(0, geometry.UpperZ[0] - geometry.LowerZ[0]));
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, geometry.LowerZ[0]);

            GL.TexCoord2(geometry.Material.GetFinalUV(0, 0));
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, geometry.UpperZ[0]);

            GL.TexCoord2(geometry.Material.GetFinalUV(edgeDistance, 0));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, geometry.UpperZ[1]);
            GL.End();
        }
    }

    public class FloorDrawable : CellDrawable
    {
        Cell cell;

        public FloorDrawable(Cell _cell)
        {
            cell = _cell;
            Init(GetMaterial(cell.FloorMaterial), DrawablesSystem.MiddlePass);
        }

        protected override void Generate()
        {
            GL.Begin(BeginMode.Polygon);

            GL.Normal3(cell.FloorNormal);

            CellVert start = cell.Verts[cell.Edges[0].Start];

            foreach (CellEdge edge in cell.Edges)
            {
                CellVert vert = cell.Verts[edge.End];
                GL.TexCoord2(cell.FloorMaterial.GetFinalUV(vert.Bottom.X - start.Bottom.X, start.Bottom.Y - vert.Bottom.Y));
                GL.Vertex3(vert.Bottom);
            }
            GL.End();
        }
    }

    public class RoofDrawable : CellDrawable
    {
        Cell cell;

        public RoofDrawable(Cell _cell)
        {
            cell = _cell;
            Init(GetMaterial(cell.RoofMaterial), DrawablesSystem.MiddlePass);
        }

        protected override void Generate()
        {
            GL.Begin(BeginMode.Polygon);
            CellVert start = cell.Verts[cell.Edges[0].Start];

            GL.Normal3(cell.RoofNormal);
            for (int i = cell.Edges.Count - 1; i >= 0; i--)
            {
                CellVert vert = cell.Verts[cell.Edges[i].End];
                GL.TexCoord2(cell.FloorMaterial.GetFinalUV(vert.Bottom.X - start.Bottom.X, start.Bottom.Y - vert.Bottom.Y));
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(cell.HeightIsIncremental));
            }
            GL.End();
        }
    }

    public class PortalDrawable : CellDrawable
    {
        PortalDestination destination;
        CellEdge edge;
        Cell cell;

        public PortalDrawable(PortalDestination _destination, CellEdge _edge, Cell _cell )
        {
            destination = _destination;

            edge = _edge;
            cell = _cell;
            Init(GetMaterial(cell.RoofMaterial), DrawablesSystem.MiddlePass + 100);
        }

        protected override void Generate()
        {
            GL.Begin(BeginMode.Quads);

            CellVert sp = cell.Verts[edge.Start];
            CellVert ep = cell.Verts[edge.End];

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);

            float edgeDistance = cell.EdgeDistance(edge);

            CellVert destSP = destination.Cell.MatchingVert(sp);
            CellVert destEP = destination.Cell.MatchingVert(ep);

            float SPTop = cell.RoofZ(edge.Start);
            if (destination.Cell.RoofZ(destSP) < SPTop)
                SPTop = destination.Cell.RoofZ(destSP);

            float SPBottom = sp.Bottom.Z;
            if (destSP.Bottom.Z > SPBottom)
                SPBottom = destSP.Bottom.Z;

            float EPTop = cell.RoofZ(edge.Start);
            if (destination.Cell.RoofZ(destEP) < EPTop)
                EPTop = destination.Cell.RoofZ(destEP);

            float EPBottom = sp.Bottom.Z;
            if (destEP.Bottom.Z > EPBottom)
                EPBottom = destEP.Bottom.Z;

            GL.TexCoord2(destination.Material.GetFinalUV(edgeDistance, EPTop - EPBottom));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, EPBottom);

            GL.TexCoord2(destination.Material.GetFinalUV(0, SPTop - SPBottom));
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y,SPBottom);

            GL.TexCoord2(destination.Material.GetFinalUV(0, 0));
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, SPTop);

            GL.TexCoord2(destination.Material.GetFinalUV(edgeDistance, 0));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, EPTop);
            GL.End();
        }

    }


    public class VizCell : Cell
    {
        List<CellDrawable> geometry = new List<CellDrawable>();

        public VizCell(Cell cell) : base(cell)
        {
            geometry.Add(new FloorDrawable(this));
            geometry.Add(new RoofDrawable(this));

            foreach(CellEdge edge in Edges)
            {
                foreach (CellWallGeometry geo in edge.Geometry)
                    geometry.Add(new WallDrawable(geo,edge,this));

                if (edge.EdgeType == CellEdgeType.Portal)
                {
                    foreach (PortalDestination dest in edge.Destinations)
                    {
                        if (dest.Visable)
                            geometry.Add(new PortalDrawable(dest, edge, this));
                    }
                }
            }
        }

        public void Add()
        {
            foreach (CellDrawable drawable in geometry)
                drawable.Add();
        }
    }

    public class PortalWorldRenderer
    {
        public PortalWorld World;

        public PortalWorldRenderer (PortalWorld world)
        {
            World = new PortalWorld();
            World.MapObjects = world.MapObjects;
            World.MapAttributes = world.MapAttributes;

            foreach (CellGroup group in world.CellGroups)
            {
                CellGroup newGroup = new CellGroup();
                newGroup.GroupAttributes = group.GroupAttributes;
                newGroup.Name = group.Name;

                foreach (Cell cell in group.Cells)
                    newGroup.Cells.Add(new VizCell(cell));

                World.CellGroups.Add(newGroup);
            }

            World.RebindCells();
        }

        public void ComputeViz ()
        {
            foreach (CellGroup group in World.CellGroups)
            {
                foreach (VizCell cell in group.Cells)
                    cell.Add();
            }
        }

        public bool VizDone ()
        {
            return true;
        }

        public void Draw ()
        {
        }
    }
}
