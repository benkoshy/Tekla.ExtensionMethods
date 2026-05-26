using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;
using Matrix = Tekla.Structures.Geometry3d.Matrix;

namespace Tekla.ExtensionMethods.Tests
{
    public class MatrixExtensionTests
    {
        [Test]
        public void ToCoordinateSystem_GivenOrigin_ReturnsReversedOriginWhenTransformed()
        {
            var origin = new Point();

            var fortyFive = new Vector(1, 0, 0);
            var cs = new CoordinateSystem(new Point(45, 45, 0), fortyFive, fortyFive.GetPerpendicularVector());

            var fromGlobal = new CoordinateSystem();

            ClassicAssert.AreEqual(new Point(-45, -45, 0), fromGlobal.ToCoordinateSystem(cs).Transform(origin));
        }

        [Test]
        public void TestCoordinateSystem()
        {
            var baseGridPoints = getBaseGridPoints();
            var global = new CoordinateSystem();

            var xVector = new Vector(1, 1, 0);
            var yVector = new Vector(-1, 1, 0);
            var origin = new Point(1, 1, 0);

            var gridCoordinateSystem = new CoordinateSystem(origin, xVector, yVector);
            var matrix =
                gridCoordinateSystem.ToGlobalCoordinateSystem(); // Matrix matrix = MatrixFactory.ByCoordinateSystems(gridCS, global);

            var transformedPoints = baseGridPoints.Select(p => matrix.Transform(p)).OrderBy(p => p.X).ToList();

            var answerPoints = getAnswerPoints(xVector, yVector, origin);

            CollectionAssert.AreEquivalent(transformedPoints, answerPoints.OrderBy(p => p.X));
        }

        private static List<Point> getBaseGridPoints()
        {
            var points = new List<Point>();

            for (var x = 0; x < 2; x += 1)
            for (var y = 0; y < 2; y += 1)
            {
                var p = new Point(x, y, 0);
                points.Add(p);
            }

            return points;
        }

        private static List<Point> getAnswerPoints(Vector xVector, Vector yVector, Point origin2)
        {
            var answerPoints = new List<Point>();

            for (var x = 0; x < 2; x += 1)
            for (var y = 0; y < 2; y += 1)
            {
                var p = new Point(0, 0, 0);
                var v = new Point().GetVectorTo(origin2);

                var xVectorised = xVector.GetNormal() * x;
                var yVectorized = yVector.GetNormal() * y;

                var answer = p.getPointTo(v).getPointTo(xVectorised).getPointTo(yVectorized);

                answerPoints.Add(answer);
            }

            return answerPoints;
        }

        [Test]
        public void TestMatrixEquality()
        {
            var m = new Matrix();
            m[0, 1] = 1;
            var m2 = new Matrix();
            m2[0, 1] = 1;
            ClassicAssert.IsTrue(m.IsEqualTo(m2));
        }
    }
}