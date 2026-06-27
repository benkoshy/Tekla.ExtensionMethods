using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Tekla.Structures.Geometry3d;
using NUnit.Framework.Legacy;
using TeklaExtensionMethods;

namespace Tests
{
    class PointExtensionsTest
    {

        [Test]
        public void TestCollinearVectorsFromPoints()
        {
            Point a = new Point();
            Point b = new Point(1, 0, 0);
            Vector v = new Vector(1, 0, 0);

            ClassicAssert.True(a.IsCollinearTo(b, v));
        }

        [Test]
        public void NotANumberTest()
        {
            Point point = new Point(float.NaN, float.NaN);
        }

        [Test]
        public void Rotate_1Clockwise_45Degrees()
        {
            Point x = VectorExtensions.AxisX.ToPoint();
            double angle = Math.PI / 4;

            Point rotatedVector = x.RotateBy(angle, new Vector().ToZaxisWCS());

            Assert.That(rotatedVector, Is.EqualTo(new Point(Math.Sqrt(0.5),-Math.Sqrt(0.5), 0)));
        }
    }
}
