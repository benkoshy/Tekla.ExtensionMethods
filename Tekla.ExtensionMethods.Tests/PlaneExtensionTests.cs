using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using TeklaExtensionMethods;

namespace Tekla.ExtensionMethods.Tests
{
    public class PlaneExtensionTests
    {

        [Test]
        public void TestPlaneTransformation1()
        {
            CoordinateSystem world = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
            CoordinateSystem shiftRight = new CoordinateSystem(new Point(1, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));

            Plane worldPlane = new Plane();
            worldPlane.Origin = world.Origin;
            worldPlane.AxisX = world.AxisX;
            worldPlane.AxisY = world.AxisY;

            Matrix transformation = MatrixFactory.ByCoordinateSystems(world, shiftRight);
            Plane newPlane = worldPlane.Transform(transformation);

            ClassicAssert.AreEqual(newPlane.AxisX.GetLength(), worldPlane.AxisX.GetLength());
            ClassicAssert.AreEqual(newPlane.AxisY.GetLength(), worldPlane.AxisY.GetLength());
        }


        [Test]
        public void TestPlaneTransformation2()
        {
            CoordinateSystem world = new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));

            int dx = 1200;
            CoordinateSystem move = new CoordinateSystem(new Point(dx, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));

            Plane worldPlane = new Plane();
            worldPlane.Origin = world.Origin;
            worldPlane.AxisX = world.AxisX;
            worldPlane.AxisY = world.AxisY;

            Matrix transformation = MatrixFactory.ByCoordinateSystems(world, move);

            Plane transformedPlane =  worldPlane.Transform(transformation);
            
            ClassicAssert.AreEqual(new Point(-dx, 0, 0 ), transformedPlane.Origin);
        }
    }
}
