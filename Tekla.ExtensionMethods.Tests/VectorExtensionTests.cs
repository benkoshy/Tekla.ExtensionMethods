using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Runtime.CompilerServices;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;

namespace Tests
{
    public class TestVectorExtensions
    {
        [Test]
        public void VectorEquality()
        {
            Vector v1 = new Vector(1, 0, 0);
            Vector v2 = new Vector(1, 0, 0);
            ClassicAssert.True(v1.Equals(v2));
            ClassicAssert.True(v1 == v2);
        }

        [Test]
        public void VectorEqualityZeroVector()
        {
            Vector v1 = new Vector(0, 0, 0);
            Vector v2 = new Vector(0, 0, 0);
            ClassicAssert.True(v1.Equals(v2));
            ClassicAssert.True(v1 == v2);
        }


        [Test]
        public void TestCollinearVectors()
        {
            Vector v1 = new Vector(1, 0, 0);
            Vector v2 = new Vector(10, 0, 0);
            ClassicAssert.True(v1.IsCollinearTo(v2));
        }

        [Test]
        public void TestZeroVectorLength()
        {
            Vector v1 = new Vector(0, 0, 0);

            ClassicAssert.True(v1.GetLength() == 0);
            ClassicAssert.True(v1.GetLength().Equals(0));
            ClassicAssert.True(v1.GetNormal().GetLength() == 0);

        }

        [Test]
        public void IsCollinearTo_OutsideTolerance_ReturnsFalse()
        {
            Vector v1 = new Vector(1, 0, 0);
            Point topRight = new Point(3192.500, 16194.000, 0.000);
            Point middleBolt = new Point(3000.000, 16194.255, 0.000); // middle bolt

            ClassicAssert.IsFalse(topRight.GetVectorTo(middleBolt).IsCollinearTo(v1));
        }

        [Test]
        public void IsCollinearTo_WithinTolerance_ReturnsTrue()
        {
            Vector v1 = new Vector(1, 0, 0);
            Point topRight = new Point(3192.500, 16194.000, 0.000);
            Point middleBolt = new Point(3000.000, 16194.255, 0.000); // middle bolt

            ClassicAssert.IsTrue(topRight.GetVectorTo(middleBolt).IsCollinearTo(v1, 0.255));
        }

        [Test]
        public void ProjectectionTest()
        {
            Vector diagonal = new Vector(1, 1, 0);

            Vector xVector = new Vector(1, 0, 0);

            Vector projectionVector = diagonal.ProjectOnto(xVector);

            ClassicAssert.AreEqual(projectionVector, xVector);
        }

        [Test]
        public void TestGetVectorTo()
        {
            Point origin = new Point(0, 0, 0);
            Point p1 = new Point(1, 1, 1);

            Vector vector = origin.GetVectorTo(p1);

            ClassicAssert.AreEqual(new Vector(1, 1, 1), vector);
        }

        [Test]
        public void TestVectorTransformation()
        {
            CoordinateSystem world = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            CoordinateSystem shiftRight = new CoordinateSystem(new Point(1, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));

            Matrix transformationMatrix = MatrixFactory.ByCoordinateSystems(world, shiftRight);
            Vector answerVector = world.AxisX.Transform(transformationMatrix);


            Vector xVector = world.AxisX;

            Point startPoint = new Point(0, 0, 0); // by definition
            Point endPoint = xVector.ToPoint();

            Point newStartPoint = startPoint.Transform(transformationMatrix);
            Point newEndPoint = endPoint.Transform(transformationMatrix);
            Vector newVector = newStartPoint.GetVectorTo(newEndPoint);

            newVector.Transform(transformationMatrix);
            ClassicAssert.AreEqual(answerVector.GetLength(), newVector.GetLength());
            ClassicAssert.AreEqual(answerVector, newVector);
        }

        [Test]
        public void XAxis_ReturnsXAxis()
        {
            ClassicAssert.AreEqual(new Vector(1, 0, 0), new Vector().ToXaxisWCS());
            ClassicAssert.AreEqual(new Vector(1, 0, 0), VectorExtensions.XAxis);
            ClassicAssert.AreEqual(new Vector(1, 0, 0), VectorExtensions.AxisX);
        }

        [Test]
        public void YAxis_ReturnsYAxis()
        {
            ClassicAssert.AreEqual(new Vector(0, 1, 0), new Vector().ToYaxisWCS());
            ClassicAssert.AreEqual(new Vector(0, 1, 0), VectorExtensions.AxisY);
            ClassicAssert.AreEqual(new Vector(0, 1, 0), VectorExtensions.YAxis);
        }

        [Test]
        public void ZAxis_ReturnsZAxis()
        {
            ClassicAssert.AreEqual(new Vector(0, 0, 1), new Vector().ToZaxisWCS());
            ClassicAssert.AreEqual(new Vector(0, 0, 1), VectorExtensions.AxisZ);
            ClassicAssert.AreEqual(new Vector(0, 0, 1), VectorExtensions.ZAxis);
        }

        [Test]
        public void EqualsWithTolerance_CustomTolerance_True()
        {
            ClassicAssert.IsTrue(new Vector(0, 0, 1).EqualsWithTolerance(new Vector(0, 0, 0.9), 1.0));
            ClassicAssert.IsTrue(new Vector(0, 1, 0).EqualsWithTolerance(new Vector(0, 0.9, 0), 1.0));
            ClassicAssert.IsTrue(new Vector(1, 0, 0).EqualsWithTolerance(new Vector(0.9, 0, 0), 1.0));
        }


        [Test]
        public void EqualsWithTolerance_CustomTolerance_False()
        {
            ClassicAssert.IsFalse(new Vector(0, 0, 1).EqualsWithTolerance(new Vector(0, 0, 0.9), 0.09));
            ClassicAssert.IsFalse(new Vector(0, 1, 0).EqualsWithTolerance(new Vector(0, 0.9, 0), 0.09));
            ClassicAssert.IsFalse(new Vector(1, 0, 0).EqualsWithTolerance(new Vector(0.9, 0, 0), 0.09));
        }

        [Test]
        public void EqualsWithTolerance_DefaultTolerance_True()
        {
            double unitPlusDelta = 1.0 + 1e-13;
            ClassicAssert.IsTrue(new Vector(0, 0, 1).EqualsWithTolerance(new Vector(0, 0, unitPlusDelta)));
            ClassicAssert.IsTrue(new Vector(0, 1, 0).EqualsWithTolerance(new Vector(0, unitPlusDelta, 0)));
            ClassicAssert.IsTrue(new Vector(1, 0, 0).EqualsWithTolerance(new Vector(unitPlusDelta, 0, 0)));
        }

        [Test]
        public void EqualsWithTolerance_DefaultTolerance_False()
        {
            double unitPlusDelta = 1.0 + 1e-11;
            ClassicAssert.IsFalse(new Vector(0, 0, 1).EqualsWithTolerance(new Vector(0, 0, unitPlusDelta)));
            ClassicAssert.IsFalse(new Vector(0, 1, 0).EqualsWithTolerance(new Vector(0, unitPlusDelta, 0)));
            ClassicAssert.IsFalse(new Vector(1, 0, 0).EqualsWithTolerance(new Vector(unitPlusDelta, 0, 0)));
        }

        [Test]
        public void EqualsWithTolerance_WhenBothVectorAreNull_True()
        {
            Vector v = null;
            Vector w = null;
            ClassicAssert.IsTrue(w.EqualsWithTolerance(v));
        }


        [Test]
        public void EqualsWithTolerance_WhenOneVectorIsNull_False()
        {
            Vector v = null;
            Vector w = new Vector();
            ClassicAssert.IsFalse(w.EqualsWithTolerance(v));
        }

        [Test]
        public void EqualsWithTolerance_False()
        {
            ClassicAssert.IsFalse(new Vector(0, 0, 1).EqualsWithTolerance(new Vector(0, 0, 0.9), (0.09)));
            ClassicAssert.IsFalse(new Vector(0, 1, 0).EqualsWithTolerance(new Vector(0, 0.9, 0), (0.09)));
            ClassicAssert.IsFalse(new Vector(1, 0, 0).EqualsWithTolerance(new Vector(0.9, 0, 0), (0.09)));
        }

        #region Cross Product

        [Test]
        public void XCrossY_ReturnsZ()
        {
            Assert.That(VectorExtensions.XAxis.Cross(VectorExtensions.YAxis), Is.EqualTo(VectorExtensions.ZAxis));
        }


        [Test]
        public void YCrossZ_ReturnsX()
        {
            Assert.That(VectorExtensions.YAxis.Cross(VectorExtensions.ZAxis), Is.EqualTo(VectorExtensions.XAxis));
        }

        [Test]
        public void ZCrossX_ReturnsY()
        {
            Assert.That(VectorExtensions.ZAxis.Cross(VectorExtensions.XAxis), Is.EqualTo(VectorExtensions.YAxis));
        }

        [Test]
        public void TestYCrossX_ReturnsNegativeZ()
        {
            // negative z
            Assert.That(VectorExtensions.YAxis.Cross(VectorExtensions.XAxis), Is.EqualTo(-1 * VectorExtensions.ZAxis));
        }

        #endregion       

        [Test]
        public void ProjectOnto_Returns_3()
        {
            Vector a = new Vector(3,4,5);
            Vector b = new Vector().ToXaxisWCS();
        }


    }
}