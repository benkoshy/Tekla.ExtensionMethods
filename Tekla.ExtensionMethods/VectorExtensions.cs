using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace TeklaExtensionMethods
{
    public static class VectorExtensions
    {
        public static Vector ProjectOnto(this Vector a, Vector b)
        {
            return (a.Dot(b)) / (b.Dot(b)) * b;
        }

        public static Vector RejectFrom(this Vector a, Vector b)
        {
            return a.Subtract(a.ProjectOnto(b));
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

        public static Vector XAxis => new Vector(1, 0, 0);

        public static Vector AxisY => new Vector(0, 1, 0);
        
        public static Vector YAxis => new Vector(0, 1, 0);
        public static Vector AxisZ => new Vector(0, 0, 1);

        public static Vector ZAxis => new Vector(0, 0, 1);

        public static Vector Transform(this Vector vector, Matrix transformationMatrix)
        {
            Point startPoint = new Point(0, 0, 0); // by definition
            Point endPoint = vector.ToPoint();

            Point newStartPoint = startPoint.Transform(transformationMatrix);
            Point newEndPoint = endPoint.Transform(transformationMatrix);
            Vector newVector = newStartPoint.GetVectorTo(newEndPoint);

            return newVector;
        }
        /// <summary>
        /// What ever the vector was, it is now a WCS (World Coordinate System) x axix vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Vector</returns>

        public static Vector ToXaxisWCS(this Vector vector)
        {
            return XAxis;
        }

        /// <summary>
        /// /// What ever the vector was, it is now a WCS (World Coordinate System) y axix vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Vector</returns>
        public static Vector ToYaxisWCS(this Vector vector)
        {
            return YAxis;
        }

        /// <summary>
        /// /// What ever the vector was, it is now a WCS (World Coordinate System) z axix vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Vector</returns>
        public static Vector ToZaxisWCS(this Vector vector)
        {
            return ZAxis;
        }

        public static bool EqualsWithTolerance(this Vector current, Vector other, double tolerance = 1e-12)
        {
            if (current == null && other == null)
            {
                return true;
            }

            if (current == null && other != null || current != null && other == null)
            {
                return false;
            }

            return Math.Abs(current.X - other.X) < tolerance &&
                   Math.Abs(current.Y - other.Y) < tolerance &&
                   Math.Abs(current.Z - other.Z) < tolerance;
        }

        public static CoordinateSystem GetGeometricCoordinateSystem(this Vector beam)
        {
            CoordinateSystem coordinateSystem = new CoordinateSystem()                                                
                                                .WithAxisX(beam)
                                                .WithYaxis(getBeamCS_YVectorLength1000(beam));

            return coordinateSystem;
        }

        public static Vector getBeamCS_ZVectorNormalized(this Vector inputBeamXVector)
        {
            if (isXVectorEqualToGlobalZVector(inputBeamXVector))
            {
                return getReferenceVector(inputBeamXVector);
            }
            else
            {
                Vector globalZ = getReferenceVector(inputBeamXVector); // which is beamY
                return inputBeamXVector.Cross(globalZ).GetNormal();// x cross Y is z
            }
        }

        /// <summary>
        /// Reflect's the a beam CS's actual y axis
        /// </summary>
        /// <param name="inputBeamXVector"></param>
        /// <returns></returns>
        public static Vector getBeamCS_YVectorLength1000(this Vector inputBeamXVector)
        {
            // x cross y returns z
            Vector zVector = getBeamCS_ZVectorNormalized(inputBeamXVector);
            return zVector.Cross(inputBeamXVector).GetNormal() * 1000;
        }


        /// <summary>        
        /// This effectively gives the reference vector for Tekla.
        //  But it is based on the input vector. If the input vector
        //  The input vector is always the xVector of the beawm.
        //  If the x vector is NOT parallel to the global z vector, then
        //  the reference vector is the global Z vector.
        //  This is the temporary "Y" vector. We use this temporary 'y' vector
        //  to calculate the beam CS's actual Z vector. And then use the actual z vector
        //  to calculate the beam CS's y vector.
        /// </summary>
        /// <param name="inputBeamXVector"></param>
        /// <returns></returns>
        public static Vector getReferenceVector(this Vector inputBeamXVector)
        {
            if (isXVectorEqualToGlobalZVector(inputBeamXVector))
            {
                return -1 * VectorExtensions.YAxis;
            }
            else
            {
                // standard xaxis comes through here.
                return VectorExtensions.ZAxis;
            }
        }

        private static bool isXVectorEqualToGlobalZVector(this Vector inputBeamXVector)
        {
            return inputBeamXVector.IsCollinearTo(VectorExtensions.ZAxis);
        }

        /// <summary>
        /// Delegation method
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double GetAngleTo(this Vector a, Vector b)
        {
            return a.GetAngleBetween(b);
        }

        public static Vector RotateBy(this Vector a, double angleInRadians, Vector b)
        {
            // delegate to Point and return a vector
            return a.ToPoint().RotateBy(angleInRadians, b).ToVector();
        }
    }
}
