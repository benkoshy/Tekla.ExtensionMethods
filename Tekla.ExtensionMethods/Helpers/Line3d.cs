using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
namespace TeklaExtensionMethods.Helpers
{
    public class Line3d :  IEquatable<Line3d>
    {
        private readonly Point startPoint;
        private readonly Point endPoint;

        private readonly Vector normalPositiveVector;

        public Point StartPoint { get { return startPoint; } }
        public Point EndPoint { get { return endPoint; } }

        public Line3d(Point startPoint, Point endPoint) 
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.normalPositiveVector = getNormalPositiveVector();
        }

        private Vector getNormalPositiveVector()
        {
            //double xPositive_start = Math.Abs(startPoint.X);
            //double yPositive_start = Math.Abs(startPoint.Y);
            //double zPositive_start = Math.Abs(startPoint.Z);

            //double xPositive_end = Math.Abs(endPoint.X);
            //double yPositive_end = Math.Abs(endPoint.Y);
            //double zPositive_end = Math.Abs(endPoint.Z);

            //Point newStart = new Point(xPositive_start, yPositive_start, zPositive_start);
            //Point newEnd = new Point(xPositive_end, yPositive_end, zPositive_end);

            Vector v = startPoint.GetVectorTo(endPoint).GetNormal();

            return new Vector(Math.Abs(v.X), Math.Abs(v.Y), Math.Abs(v.Z));            
        }

        public bool DoesIntersectWith(Line3d line2)
        {
            Vector p1 = (new Point()).GetVectorTo(this.startPoint); //  line1Point1;
            Vector p2 = (new Point()).GetVectorTo(this.endPoint);   //  line1Point2;
            Vector p3 = (new Point()).GetVectorTo(line2.startPoint);  // line2Point1;
            Vector p4 = (new Point()).GetVectorTo(line2.endPoint);
            Vector p13 = p1.Minus(p3); // p1 - p3;
            Vector p43 = p4.Minus(p3);  // p4 - p3;

            if (p43.GetLength()  * p43.GetLength() < double.Epsilon)
            {
                return false;
            }

            Vector p21 = p2.Minus(p1); // p2 - p1;
            if ( Math.Pow( p21.GetLength(), 2) < double.Epsilon)
            {
                return false;
            }

            double d1343 = p13.X * (double)p43.X + (double)p13.Y * p43.Y + (double)p13.Z * p43.Z;
            double d4321 = p43.X * (double)p21.X + (double)p43.Y * p21.Y + (double)p43.Z * p21.Z;
            double d1321 = p13.X * (double)p21.X + (double)p13.Y * p21.Y + (double)p13.Z * p21.Z;
            double d4343 = p43.X * (double)p43.X + (double)p43.Y * p43.Y + (double)p43.Z * p43.Z;
            double d2121 = p21.X * (double)p21.X + (double)p21.Y * p21.Y + (double)p21.Z * p21.Z;
            double denom = d2121 * d4343 - d4321 * d4321;

            if (Math.Abs(denom) < double.Epsilon)
            {
                return false;
            }
            double numer = d1343 * d4321 - d1321 * d4343;

            double mua = numer / denom;
            double mub = (d1343 + d4321 * (mua)) / d4343;

            double resultSegmentPoint1X = (float)(p1.X + mua * p21.X);
            double resultSegmentPoint1Y = (float)(p1.Y + mua * p21.Y);
            double resultSegmentPoint1Z = (float)(p1.Z + mua * p21.Z);
            double resultSegmentPoint2X = (float)(p3.X + mub * p43.X);
            double resultSegmentPoint2Y = (float)(p3.Y + mub * p43.Y);
            double resultSegmentPoint2Z = (float)(p3.Z + mub * p43.Z);

            Point resultPoint1 = new Point(resultSegmentPoint1X, resultSegmentPoint1Y, resultSegmentPoint1Z);
            Point resultPoint2 = new Point(resultSegmentPoint2X, resultSegmentPoint2Y, resultSegmentPoint2Z);

            Vector betweenResults = resultPoint1.GetVectorTo(resultPoint2) * 0.5;
            Point answer = resultPoint1.getPointTo(betweenResults);

            return true;
        }



        /// <summary>
        /// Gets intersection point and throws exception if Lines are collinear or otherwise invalid
        /// http://paulbourke.net/geometry/pointlineplane/calclineline.cs
        /// </summary>
        /// <param name="line2"></param>
        /// <returns></returns>
        public Point GetIntersectionPoint(Line3d line2)
        {
            Vector p1 = (new Point()).GetVectorTo(this.startPoint);     //  line1Point1;
            Vector p2 = (new Point()).GetVectorTo(this.endPoint);       //  line1Point2;
            Vector p3 = (new Point()).GetVectorTo(line2.startPoint);    // line2Point1;
            Vector p4 = (new Point()).GetVectorTo(line2.endPoint);
            Vector p13 = p1.Minus(p3);                                  // p1 - p3;
            Vector p43 = p4.Minus(p3);                                  // p4 - p3;

            if (p43.GetLength() * p43.GetLength() < double.Epsilon)
            {
                throw new DivideByZeroException("");                
            }

            Vector p21 = p2.Minus(p1);                                  // p2 - p1;

            if (Math.Pow(p21.GetLength(), 2) < double.Epsilon)
            {
                throw new DivideByZeroException("");                
            }

            double d1343 = p13.X * (double)p43.X + (double)p13.Y * p43.Y + (double)p13.Z * p43.Z;
            double d4321 = p43.X * (double)p21.X + (double)p43.Y * p21.Y + (double)p43.Z * p21.Z;
            double d1321 = p13.X * (double)p21.X + (double)p13.Y * p21.Y + (double)p13.Z * p21.Z;
            double d4343 = p43.X * (double)p43.X + (double)p43.Y * p43.Y + (double)p43.Z * p43.Z;
            double d2121 = p21.X * (double)p21.X + (double)p21.Y * p21.Y + (double)p21.Z * p21.Z;
            double denom = d2121 * d4343 - d4321 * d4321;

            if (Math.Abs(denom) < double.Epsilon)
            {
                throw new DivideByZeroException("Collinear Lines - there is no intersection.");
            }

            double numer = d1343 * d4321 - d1321 * d4343;

            double mua = numer / denom;
            double mub = (d1343 + d4321 * (mua)) / d4343;

            double resultSegmentPoint1X = (float)(p1.X + mua * p21.X);
            double resultSegmentPoint1Y = (float)(p1.Y + mua * p21.Y);
            double resultSegmentPoint1Z = (float)(p1.Z + mua * p21.Z);
            double resultSegmentPoint2X = (float)(p3.X + mub * p43.X);
            double resultSegmentPoint2Y = (float)(p3.Y + mub * p43.Y);
            double resultSegmentPoint2Z = (float)(p3.Z + mub * p43.Z);

            Point resultPoint1 = new Point(resultSegmentPoint1X, resultSegmentPoint1Y, resultSegmentPoint1Z);
            Point resultPoint2 = new Point(resultSegmentPoint2X, resultSegmentPoint2Y, resultSegmentPoint2Z);

            Vector betweenResults = resultPoint1.GetVectorTo(resultPoint2) * 0.5;
            Point answer = resultPoint1.getPointTo(betweenResults);

            return answer;
        }


        public Vector getVector()
        {
            return this.startPoint.GetVectorTo(this.endPoint);
        }

        public override bool Equals(object obj)
        {
            return obj is Line3d d &&
                   EqualityComparer<Vector>.Default.Equals(normalPositiveVector, d.normalPositiveVector);
        }

        public override int GetHashCode()
        {
            return -1054140154 + EqualityComparer<Vector>.Default.GetHashCode(normalPositiveVector);
        }

        public bool Equals(Line3d other)
        {
            // solution 1
            double dotProduct = this.normalPositiveVector.Dot(other.normalPositiveVector);
            return dotProduct == Math.Sqrt(this.normalPositiveVector.X * other.normalPositiveVector.X + this.normalPositiveVector.Y * other.normalPositiveVector.Y + this.normalPositiveVector.Z * other.normalPositiveVector.Z);

            // solution 2
            //Vector ab = startPoint.GetVectorTo(endPoint);
            //Vector ac = startPoint.GetVectorTo(other.startPoint);
            //Vector ad = startPoint.GetVectorTo(other.endPoint);
            //return ab.Cross(ac).Equals(new Vector()) && ab.Cross(ad).Equals(new Vector());
        }

        public override string ToString()
        {
            return $"Start: {this.startPoint.ToString()}! End: {this.endPoint.ToString()}";
        }
    }
}

