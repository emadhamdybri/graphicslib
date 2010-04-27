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
using System.Linq;
using System.Text;
using System.Drawing;

using Drawables.DisplayLists;
using Drawables.Materials;
using Drawables;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using World;

namespace GraphicWorlds
{
    class GroundRenderer
    {
        DisplayList groundList = new DisplayList();
        DisplayList wallList = new DisplayList();

        Material groundMaterial = null;
        Material wallMaterial = null;

        Vector3 size;

        float wallHeight = 0;

        float uvScale = 1.0f;

        public void Setup(ObjectWorld world )
        {
            groundList.Invalidate();
            wallList.Invalidate();

            size = world.size;
            
            if (groundMaterial != null)
                groundMaterial.Invalidate();

            if (world.groundMaterialName != string.Empty)
                groundMaterial = MaterialSystem.system.GetMaterial(world.groundMaterialName);

            if (groundMaterial == null)
                groundMaterial = MaterialSystem.system.GetMaterial(new Material(Color.ForestGreen));

            if (world.groundUVSize > 0)
                uvScale = 1f/world.groundUVSize;

            DrawablesSystem.system.addItem(groundMaterial, new ExecuteCallback(DrawGround),DrawablesSystem.FirstPass, groundList);

            wallHeight = world.wallHeight;
            if (wallHeight > 0)
            {
                if (world.wallMaterialName != string.Empty)
                    wallMaterial = MaterialSystem.system.GetMaterial(world.wallMaterialName);
                if (wallMaterial == null)
                    wallMaterial = MaterialSystem.system.GetMaterial(new Material(Color.Brown));

                DrawablesSystem.system.addItem(wallMaterial, new ExecuteCallback(DrawGround), DrawablesSystem.FirstPass, wallList);
            }
        }

        void BuildGroundList()
        {
            groundList.Start();
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1f);

            GL.TexCoord2(size.X * uvScale, -size.Y * uvScale);
            GL.Vertex3(size.X, size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, -size.Y * uvScale);
            GL.Vertex3(-size.X, size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, size.Y * uvScale);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.TexCoord2(size.X * uvScale,size.Y * uvScale);
            GL.Vertex3(size.X, -size.Y, 0);
            GL.End();
            groundList.End();
        }

        void BuildWallList()
        {
            wallList.Start();

            GL.Begin(BeginMode.Quads);

            // y+
            GL.Normal3(0, -1f, 0);
            GL.TexCoord2(size.X * uvScale, 0);
            GL.Vertex3(size.X, size.Y, 0);

            GL.TexCoord2(size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, size.Y, wallHeight);
          
            GL.TexCoord2(-size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, size.Y, wallHeight);

            GL.TexCoord2(-size.X * uvScale, 0);
            GL.Vertex3(-size.X, size.Y, 0);

            // y-
            GL.Normal3(0, 1f, 0);
            GL.TexCoord2(-size.X * uvScale, 0);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, -size.Y, wallHeight);

            GL.TexCoord2(size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, -size.Y, wallHeight);
 
            GL.TexCoord2(size.X * uvScale, 0);
            GL.Vertex3(size.X, -size.Y, 0);


            // X+
            GL.Normal3(-1f, 0, 0);
            GL.TexCoord2(-size.Y * uvScale, 0);
            GL.Vertex3(size.X, -size.Y, 0);

            GL.TexCoord2(-size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, -size.Y, wallHeight);

            GL.TexCoord2(size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, size.Y, wallHeight);
          
            GL.TexCoord2(size.Y * uvScale, 0);
            GL.Vertex3(size.X, size.Y, 0);

            // X-
            GL.Normal3(1f, 0, 0);
            GL.TexCoord2(size.Y * uvScale, 0);
            GL.Vertex3(-size.X, size.Y, 0);

            GL.TexCoord2(size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, size.Y, wallHeight);

            GL.TexCoord2(-size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, -size.Y, wallHeight);

            GL.TexCoord2(-size.Y * uvScale, 0);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.End();

            wallList.End();
        }

        protected bool DrawGround(Material mat, object tag)
        {
            DisplayList list = tag as DisplayList;
            if (list == null)
                return false;

            if (list == groundList && !groundList.Valid())
                BuildGroundList();
            else if (list == wallList && !wallList.Valid())
                BuildWallList();

            list.Call();

            return true;
        }
    }
}
