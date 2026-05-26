using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Matrix = Tekla.Structures.Geometry3d.Matrix;

namespace BoltControl.ExtensionMethods
{
    public static class PlaneExtensions
    {
        public static Plane Transform(this Plane plane, Matrix matrix)
        {
            Plane newPlane = new Plane();

            newPlane.Origin = plane.Origin.Transform(matrix);
            newPlane.AxisX = plane.AxisX.Transform(matrix);
            newPlane.AxisY = plane.AxisY.Transform(matrix);

            return newPlane;  
        }

    }
}
