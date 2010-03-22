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
using System.Text;

using Math3D;
using OpenTK;

namespace World
{
    public class OctreeWorldObject : OctreeObject
    {
    }

    public class OctreeWorld : Octree
    {
        public List<OctreeWorldObject> objects = new List<OctreeWorldObject>();

        public void Add(OctreeWorldObject item)
        {
            objects.Add(item);
        }

        public void BuildTree( BoundingBox initalBounds )
        {
            foreach (OctreeWorldObject item in objects)
                ContainedObjects.Add(item as OctreeObject);

            ContainerBox = new BoundingBox(initalBounds.Min,initalBounds.Max);
            base.Distribute(0);
        }

        public void BuildTree()
        {
            foreach (OctreeWorldObject item in objects)
                ContainedObjects.Add(item as OctreeObject);

            Bounds();
            base.Distribute(0);
        }

        public void ObjectsInFrustum(List<OctreeObject> objects, BoundingFrustum boundingFrustum, bool exact)
        {
            base.ObjectsInFrustum(objects, boundingFrustum);
            if (exact)
            {
                for (int i = objects.Count-1; i >= 0; i--)
                {
                    if (!boundingFrustum.Intersects(objects[i].bounds))
                        objects.RemoveAt(i);
                }
            }
        }

        public void ObjectsInBoundingBox(List<OctreeObject> objects, BoundingBox boundingbox, bool exact)
        {
            base.ObjectsInBoundingBox(objects, boundingbox);
            if (exact)
            {
                for (int i = objects.Count - 1; i >= 0; i--)
                {
                    if (!boundingbox.Intersects(objects[i].bounds))
                        objects.RemoveAt(i);
                }
            }
        }
    }
}
