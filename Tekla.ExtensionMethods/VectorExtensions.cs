using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class VectorExtensions
    {
        public static Vector ProjectionOnto(this Vector a, Vector b)
        {
            return (a.Dot(b)) / (b.Dot(b)) * b;
        }

        // we need operator overloads for this.
        public static Vector Add(this Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector Subtract(this Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// a - b == a.Minus(b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector Minus(this Vector a, Vector b)
        {
            return a.Subtract(b);
        }

        public static Point ToPoint(this Vector vector)
        {
            return new Point(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Returns a vector orthogonal to this vector, namely the vector ( -y, x)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector GetPerpendicularVector(this Vector v)
        {
            return new Vector(-v.Y, v.X, 0);
        }


        public static bool IsCollinearTo(this Vector a, Vector vector, double tolerance = double.Epsilon)
        {            
            Vector crossVector = a.Cross(vector);

            return (Math.Abs(crossVector.X) < tolerance) && (Math.Abs(crossVector.Y) < tolerance) && (Math.Abs(crossVector.Z) < tolerance);
        }

        // these should be extension properties
        public static Vector AxisX => new Vector(1, 0, 0);
        
        public static Vector AxisY => new Vector(0, 1, 0);
        public static Vector AxisZ => new Vector(0, 0, 1);

        public static Vector Transform(this Vector vector, Matrix transformationMatrix)
        {
            Point startPoint = new Point(0, 0, 0); // by definition
            Point endPoint = vector.ToPoint();

            Point newStartPoint = startPoint.Transform(transformationMatrix);
            Point newEndPoint = endPoint.Transform(transformationMatrix);
            Vector newVector = newStartPoint.GetVectorTo(newEndPoint);

            return newVector;
        }

    }
}
