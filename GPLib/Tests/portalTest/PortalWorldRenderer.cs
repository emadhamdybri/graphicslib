/*
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
using System.Drawing;
using System.Text;

using World;

using Drawables;
using Drawables.DisplayLists;
using Drawables.Materials;

using OpenTK;
using OpenTK.Graphics;

using Math3D;

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

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);

            float edgeDistance = cell.EdgeDistance(edge);

            GL.TexCoord2(destination.Material.GetFinalUV(edgeDistance, destination.EPTop.Z - destination.EPBottom.Z));
            GL.Vertex3(destination.EPBottom);

            GL.TexCoord2(destination.Material.GetFinalUV(0, destination.SPTop.Z - destination.SPBottom.Z));
            GL.Vertex3(destination.SPBottom);

            GL.TexCoord2(destination.Material.GetFinalUV(0, 0));
            GL.Vertex3(destination.SPTop);

            GL.TexCoord2(destination.Material.GetFinalUV(edgeDistance, 0));
            GL.Vertex3(destination.EPTop);
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

    public class ExternalPortal
    {
        public VizCell cell;
        public CellEdge edge;
        public PortalDestination destination;
    }

    public class PortalVisitItem
    {
        public PortalDestination portal;
        public BoundingFrustum frustum;
    }

    public class PortalWorldRenderer
    {
        public PortalWorld World;

        List<Cell> visitedCells = new List<Cell>();

        Dictionary<CellGroup, List<ExternalPortal> > externalPortals = new Dictionary<CellGroup, List<ExternalPortal> >();

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

            CacheExternalCells();
        }

        protected void CacheExternalCells()
        {
            foreach (CellGroup group in World.CellGroups)
            {
                List<ExternalPortal> dests = new List<ExternalPortal>();
                foreach (VizCell cell in group.Cells)
                {
                    foreach (CellEdge edge in cell.Edges)
                    {
                        if (edge.EdgeType == CellEdgeType.Portal)
                        {
                            foreach (PortalDestination dest in edge.Destinations)
                            {
                                if (dest.DestinationCell.GroupName == group.Name)
                                    continue;

                                ExternalPortal externPort = new ExternalPortal();
                                externPort.cell = cell;
                                externPort.destination = dest;
                                externPort.edge = edge;

                                dests.Add(externPort);
                            }
                        }
                    }
                }

                externalPortals.Add(group, dests);
            }
        }

        protected void DrawGroup ( CellGroup group )
        {
            foreach (VizCell cell in group.Cells)
            {
                if (visitedCells.Contains(cell))
                    continue;

                cell.Add();
                visitedCells.Add(cell);
            }

            // draw any objects in the cell too
            DrawablesSystem.system.Execute();
            DrawablesSystem.system.removeAll();
        }


        void DrawPortalStackToStencil (List<PortalVisitItem> VisitStack)
        {
            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            GL.Disable(EnableCap.DepthTest);

            GL.Color4(1, 1, 1, 0f);

            for (int i = VisitStack.Count - 1; i >= 0; i-- )
            {
                PortalDestination destination = VisitStack[i].portal;
                GL.Begin(BeginMode.Quads);

                GL.Vertex3(destination.EPBottom);
                GL.Vertex3(destination.SPBottom);
                GL.Vertex3(destination.SPTop);
                GL.Vertex3(destination.EPTop);

                GL.End();
            }
            GL.Enable(EnableCap.DepthTest);
            GL.StencilFunc(StencilFunction.Equal, 1, 1);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        }

        protected void RecursiveWalkGroup ( Cell cell, ref List<PortalVisitItem> VisitStack, ViewPosition view )
        {
            if (visitedCells.Contains(cell))
                return; // this is being handled by others, so let it go

            DrawPortalStackToStencil(VisitStack);
            // ok first draw the group
            DrawGroup(cell.Group);

            GL.Disable(EnableCap.StencilTest);

            List<ExternalPortal> dests = externalPortals[cell.Group];

            foreach (ExternalPortal dest in dests)
            {
                float dot = Vector3.Dot(VectorHelper3.Subtract(dest.destination.SPBottom,view.Eye),new Vector3(dest.edge.Normal));
                if (dot > 0)
                    continue;

                if (VisitStack[0].frustum.Intersects(dest.destination.GetPolygon()))
                {
                    PortalVisitItem item = new PortalVisitItem();
                    // TODO CLIP the frustum to the portal!!!
                    item.frustum = VisitStack[0].frustum;
                    item.portal = dest.destination;

                    VisitStack.Add(item);
                    RecursiveWalkGroup(dest.destination.Cell, ref VisitStack, view);
                    VisitStack.Remove(item);
                }
            }
        }

        public void Draw( ViewPosition view, BoundingFrustum frustum )
        {
            // always draw where we are.
            visitedCells.Clear();
            DrawGroup(view.cell.Group);

            List<ExternalPortal> dests = externalPortals[view.cell.Group];

            foreach (ExternalPortal dest in dests)
            {
                List<PortalVisitItem> VisitStack = new List<PortalVisitItem>();

                if (frustum.Intersects(dest.destination.GetPolygon()))
                {
                    PortalVisitItem item = new PortalVisitItem();
                    // TODO CLIP the frustum to the portal!!!

                    item.frustum = frustum;
                    item.portal = dest.destination;

                    VisitStack.Add(item);
                    RecursiveWalkGroup(dest.destination.Cell, ref VisitStack, view);
                }
            }
        }

        public void DrawMapView ( ViewPosition view )
        {
            float scale = 20f;

            GL.PushMatrix();
            GL.Color4(Color.Red);
            IntPtr q = Glu.NewQuadric();
            Glu.Sphere(q,5, 10, 10);
            Glu.DeleteQuadric(q);

            GL.Rotate(-view.Rotation.Y + 90, 0, 0, 1);
            GL.Translate(-view.Position.X * scale, -view.Position.Y * scale, -1);

            GL.LineWidth(2);

            GL.Color4(1f, 0f, 0f, 0.5f);
            foreach (CellGroup group in World.CellGroups)
            {
                foreach (VizCell cell in group.Cells)
                {
                    if (cell == view.cell)
                        GL.Color3(Color.Olive);
                    else
                    {
                        if (visitedCells.Contains(cell))
                            GL.Color3(Color.LightSalmon);
                        else
                            GL.Color3(Color.Green);
                    }
                       

                    GL.Begin(BeginMode.Polygon);
                    foreach( CellVert vert in cell.Verts )
                       GL.Vertex2(vert.Bottom.X * scale, vert.Bottom.Y * scale);
                    GL.End();

                    GL.Color3(Color.LightGreen);
                    foreach (CellEdge edge in cell.Edges)
                    {
                        if (edge.EdgeType == CellEdgeType.Portal)
                        {
                            GL.Color3(Color.OliveDrab);
                            foreach( PortalDestination dest in edge.Destinations)
                            {
                                if (dest.Cell.Group != group)
                                    GL.Color3(Color.Yellow);
                            }
                        }
                        else
                            GL.Color3(Color.LightGreen);

                       GL.Begin(BeginMode.Lines);
                       GL.Vertex3(cell.Verts[edge.Start].Bottom.X * scale, cell.Verts[edge.Start].Bottom.Y * scale, 1);
                       GL.Vertex3(cell.Verts[edge.End].Bottom.X * scale, cell.Verts[edge.End].Bottom.Y * scale, 1);
                       GL.End();

                    }
                }
            }
            GL.LineWidth(1);
            GL.PopMatrix();
        }
    }
}
