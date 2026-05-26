using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class LineExtensions
    {
        public static Point IntersectionPoint(this Line line, GeometricPlane plane)
        {
            return Intersection.LineToPlane(line, plane);
        }
    }
}
