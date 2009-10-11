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
            String fileName = Path.Combine("Maps", "5D.PortalMap");

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
                player.Position.Z += 1.6f;
                player.cell = world.FindCell(spawns[0].cells[0]);
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

            player.Move(movement);

            checkMovement();

            return false;
        }


        void checkMovement ( )
        {
            if (player.cell == null)
                return;

            Vector2 circle = new Vector2(player.Position);
            float radius = 0.25f;

            if (!player.cell.CircleIn(circle, radius))
            {   
                foreach (CellEdge edge in player.cell.Edges)
                {
                    if (player.cell.CircleCrossEdge(edge,circle, radius))
                    {
                        if (edge.EdgeType == CellEdgeType.Wall)
                            player.CopyFrom(lastPlayerPos);
                        else
                        {
                            Cell destination = edge.Destinations[0].Cell;
                            if (destination == null)
                                return;

                            if (destination.PointIn(player.Position))
                                player.cell = destination;
                        }

                        break;
                    }
                }
            }

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
