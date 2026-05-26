using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Point = Tekla.Structures.Geometry3d.Point;
using Tekla.Structures.Geometry3d;


namespace TeklaExtensionMethods
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point fromPoint, Point point)
        {
            return Math.Sqrt(
                 Math.Pow((point.X - fromPoint.X), 2) +
                 Math.Pow(point.Y - fromPoint.Y, 2) +
                 Math.Pow(point.Z - fromPoint.Z, 2)
                 );
        }

        public static Vector GetVectorTo(this Point boltPosition, Point point)
        {
            Vector originToPoint = new Vector(point);
            Vector originToBoltPosition = new Vector(boltPosition);

            return originToPoint.Minus(originToBoltPosition);
        }

        public static Point getPointTo(this Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        public static bool IsCollinearTo(this Point a, Point b, Vector vector, double tolerance = double.Epsilon)
        {
            Vector ab = a.GetVectorTo(b);
            Vector crossVector =  ab.Cross(vector);

            return ( Math.Abs(crossVector.X) < tolerance) && (Math.Abs(crossVector.Y) < tolerance) && (Math.Abs(crossVector.Z) < tolerance);
        }

        public static Point Flatten(this Point point)
        {
            return new Point(point.X, point.Y, 0);
        }

        public static Point TranslateNew(this Point point, double x = 0, double y = 0, double z = 0)
        {
            return new Point(point.X + x, point.Y + y, point.Z + z);
        }


        public static Vector ToVector(this Point point)
        {
            return new Vector(point.X, point.Y, point.Z);
        }

        public static Point ToPointX(this Point point, double x)
        {
            return new Point(x, point.Y, point.Z);
        }

        public static Point ToPointY(this Point point, double y)
        {
            return new Point(point.X, y, point.Z);
        }

        public static Point ToPointZ(this Point point, double z)
        {
            return new Point(point.X, point.Y, z);
        }

        public static Point Transform(this Point point, Matrix matrix)
        {
            return matrix.Transform(point);
        }
    }
}

            