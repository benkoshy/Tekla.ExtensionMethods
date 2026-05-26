using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;

namespace Tests
{
    public class TestVectorExtensions
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void VectorEquality()
        {
            Vector v1 = new Vector(1, 0, 0);
            Vector v2 = new Vector(1, 0, 0);
            ClassicAssert.True(v1.Equals( v2));
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
            Vector v1 = new Vector(1,0,0);
            Vector v2 = new Vector(10, 0, 0);
            ClassicAssert.True(v1.IsCollinearTo(v2));
        }

        [Test]
        public void TestZeroVectorLength()
        {
            Vector v1 = new Vector(0, 0, 0);
            
            ClassicAssert.True(v1.GetLength() == 0 );
            ClassicAssert.True(v1.GetLength().Equals(0)); // v1.GetNormal();
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

            Vector projectionVector = diagonal.ProjectionOnto(xVector);

            ClassicAssert.AreEqual(projectionVector, xVector);
        }

        [Test]
        public void TestGetVectorTo()
        {
            Point origin = new Point(0, 0, 0);
            Point p1 = new Point(1, 1, 1);

            Vector vector = origin.GetVectorTo(p1);

            ClassicAssert.AreEqual(new Vector(1,1,1), vector);
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
    }
}