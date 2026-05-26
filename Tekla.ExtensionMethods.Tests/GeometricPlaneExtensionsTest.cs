using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;

namespace Tekla.ExtensionMethods.Tests
{
    class GeometricPlaneExtensionsTest
    {
        [Test]
        public void PlaneIntersectsWithLine_WhenParallel_nullReturned()
        {
            GeometricPlane plane = new GeometricPlane(new Point(0, 0, 0), new Vector(0, 0, 1));
            Line line = new Line(new Point(1, 0, 1), new Point(10, 0, 1));

            ClassicAssert.AreEqual(null, plane.IntersectsWith(line));
        }

        [Test]
        public void DoesIntersectWithLine_WhenParallel_falseReturned()
        {
            GeometricPlane plane = new GeometricPlane(new Point(0, 0, 0), new Vector(0, 0, 1));
            Line line = new Line(new Point(1, 0, 1), new Point(10, 0, 1));

            ClassicAssert.IsFalse(plane.DoesIntersectWith(line));
        }
    }
}
