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
using System.Windows.Forms;
using System.IO;

using Math3D;

using Drawables.Cameras;
using GUIGameWindow;
using World;
using OpenTK;

using Drawables.Textures;

namespace portalTest
{
    public class Game
    {
        PortalWorld world = null;
        PortalWorldRenderer renderer;

        public Visual visual;

        GUIGameWindowBase window;

        Point lastMousePos = Point.Empty;
        Point thisMousePos = new Point(0, 0);

        String dataDir = string.Empty;

        ViewPosition player = new ViewPosition();
        ViewPosition lastPlayerPos = new ViewPosition();

        public Game(GUIGameWindowBase win)
        {
            window = win;

            SetupResourceDirs();
            String fileName = Path.Combine("Maps", "Ramp.PortalMap");

            world = PortalWorld.Read(new FileInfo(Path.Combine(dataDir, fileName)));
            if (world == null)
                world = new PortalWorld();

            renderer = new PortalWorldRenderer(world);
            world = renderer.World;

            visual = new Visual(window, renderer);
            visual.view = player;
            lastPlayerPos.CopyFrom(player);
        }

        string GetSlashPath ( int num )
        {
            string ret = "..";

            for (int i = 0; i < num; i++)
                ret = Path.Combine(ret, "..");

            return ret;
        }
        protected void SetupResourceDirs ()
        {
            string mapsDir = Path.Combine("Data","Maps");

            DirectoryInfo info = new DirectoryInfo(Path.Combine(GetSlashPath(3),mapsDir));
            if (!info.Exists)
            {
                info = new DirectoryInfo(Path.Combine(GetSlashPath(2),mapsDir));
                if (!info.Exists)
                {
                    info = new DirectoryInfo(Path.Combine(GetSlashPath(1), mapsDir));

                    if (!info.Exists)
                        info = new DirectoryInfo(mapsDir);
                }
            }

            dataDir = info.Parent.FullName;

            TextureSystem.system.LocateFile = new TextureSystem.LocateFileHandler(LocateTextureFile);
        }

        FileInfo LocateTextureFile (string file)
        {
            return new FileInfo(Path.Combine(dataDir, file));
        }

        public void Init ()
        {
            List<ObjectInstance> spawns = world.FindObjects("Spawn");
            if (spawns.Count > 0)
            {
                player.Position = new Vector3(spawns[0].Postion);
                player.cell = world.FindCell(spawns[0].cells[0]);
                lastPlayerPos.CopyFrom(player);

                checkMovement();
            }
        }

        public void MouseMove ( MouseMoveEventArgs e)
        {
            if (e.Buttons != MouseButtons.Right)
                return;

            if (lastMousePos == Point.Empty)
                lastMousePos = new Point(e.X, e.Y);

            thisMousePos = new Point(thisMousePos.X + e.XDelta, thisMousePos.Y + e.YDelta);
        }

        protected bool doInput(GUIGameWindowBase.UpdateFrameArgs e)
        {
            if (window.Keyboard[Keys.Escape])
                return true;
            lastPlayerPos.CopyFrom(player);

            float turnSpeed = 40.0f;
            turnSpeed *= (float)e.TimeDelta;

            Point delta = thisMousePos;
            thisMousePos = new Point(0, 0);

            float sensitivity = 0.1f;

            player.Turn(-turnSpeed * sensitivity * delta.X, -turnSpeed * sensitivity * delta.Y);

            Vector3 movement = new Vector3();

            float speed = 5.0f;
            speed *= (float)e.TimeDelta;

            if (window.Keyboard[Keys.A])
                movement.X = speed;
            if (window.Keyboard[Keys.D])
                movement.X = -speed;
            if (window.Keyboard[Keys.W])
                movement.Y = speed;
            if (window.Keyboard[Keys.S])
                movement.Y = -speed;

            if (window.Keyboard[Keys.PageUp])
                movement.Z = speed;
            if (window.Keyboard[Keys.PageDown])
                movement.Z = -speed;

            if (window.Keyboard[Keys.A] || window.Keyboard[Keys.S] || window.Keyboard[Keys.D] || window.Keyboard[Keys.W] || window.Keyboard[Keys.PageUp] || window.Keyboard[Keys.PageDown])
            {
                player.Move(movement);
                checkMovement();
            }

            return false;
        }


        Cell FindCellAcrossEdge ( ref Vector3 position, Vector3 initalPos,  Cell starter, ref List<Cell> TestedCells )
        {
            Vector2 circle = new Vector2(position);
            float radius = 0.35f;

            float autoStepZ = 0.5f;

            if (starter.CircleIn(circle, radius))
                return starter;

            foreach (CellEdge edge in starter.Edges)
            {
                if (starter.CircleCrossEdge(edge, circle, radius))
                {
                    if (edge.EdgeType == CellEdgeType.Wall)
                    {
                        position = initalPos;
                        return starter;
                    }

                    if (starter.PointIn(player.Position)) // it is still in my cell but hasn't gone over
                        return starter;

                    foreach (PortalDestination dest in edge.Destinations)
                    {
                        if (TestedCells.Contains(dest.Cell))
                            continue;

                        // check it's Z
                        if (dest.EPBottom.Z > player.Position.Z + autoStepZ || dest.SPBottom.Z > player.Position.Z + autoStepZ)
                            continue; // it's above us, so go on

                        if (dest.Cell.PointIn2D(player.Position))
                            return dest.Cell;

                        TestedCells.Add(dest.Cell);
                        Cell possibleCell = FindCellAcrossEdge(ref position, initalPos, dest.Cell, ref TestedCells);
                        TestedCells.Remove(dest.Cell);
                        if (possibleCell != dest.Cell)
                            return possibleCell;
                    }
                }
            }

            // if we can't figure it out, don't allow the move
            position = initalPos;
            return starter;
        }

        void checkMovement ( )
        {
            if (player.cell == null)
                return;

            List<Cell> TestedCells = new List<Cell>();
            player.cell = FindCellAcrossEdge(ref player.Position, lastPlayerPos.Position, player.cell, ref TestedCells);

            // force the player to be at the right Z level
            player.Position.Z = Cell.GetZInPlane(player.cell.GetFloorPlane(), player.Position.X, player.Position.Y);

        }

        public bool Update(GUIGameWindowBase.UpdateFrameArgs e)
        {
            if (doInput(e))
                return true;
            return false;
        }
    }
}
