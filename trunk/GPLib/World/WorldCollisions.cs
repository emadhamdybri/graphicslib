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
using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using Math3D;

namespace World
{
    public enum CollisionBoundryType
    {
        None,
        AxisBox,
        RotatedBox,
        Sphere,
        Cylinder,
    }

    public class CollisionBoundry
    {
        public CollisionBoundryType type = CollisionBoundryType.AxisBox;
        public Vector3 center = new Vector3();
        public Vector3 bounds = new Vector3();
        public Vector3 Orientation = new Vector3();

        // cache for prims
        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingBox box = BoundingBox.Empty;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingSphere sphere = BoundingSphere.Empty;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingCylinderXY cylinder = BoundingCylinderXY.Empty;

        public BoundingBox Bounds ()
        {
            switch(type)
            {
                case CollisionBoundryType.Sphere:
                    return BoundingBox.CreateFromSphere(getSphere());

                case CollisionBoundryType.Cylinder:
                    return BoundingBox.CreateFromCylinderXY(getCylinder());
            }

            return getBox();
        }

        BoundingBox getBox ()
        {
            if (box == BoundingBox.Empty)
                box = new BoundingBox(center + -bounds, center + bounds);
            return box;
        }

        BoundingSphere getSphere()
        {
            if (sphere == BoundingSphere.Empty)
                sphere = new BoundingSphere(center,bounds.X);
            return sphere;
        }

        BoundingCylinderXY getCylinder()
        {
            if (cylinder == BoundingCylinderXY.Empty)
                cylinder = new BoundingCylinderXY(center, bounds.X, bounds.Z);

            return cylinder;
        }
    }
}
