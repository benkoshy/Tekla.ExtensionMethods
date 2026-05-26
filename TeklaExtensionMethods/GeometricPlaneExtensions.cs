using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace BoltControl.ExtensionMethods
{
    public static class GeometricPlaneExtensions
    {
        public static Point IntersectsWith(this GeometricPlane plane, Line line)
        {
            return Intersection.LineToPlane(line, plane);
        }

        public static bool DoesIntersectWith(this GeometricPlane plane, Line line)
        {
            return null != Intersection.LineToPlane(line, plane);
        }
    }
}
