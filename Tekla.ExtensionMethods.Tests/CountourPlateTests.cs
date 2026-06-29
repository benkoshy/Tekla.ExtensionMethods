using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using TeklaExtensionMethods;
using static Tekla.Structures.ModelInternal.Operation;

namespace Tekla.ExtensionMethods.Tests
{
    public static class CountourPlateTests
    {
        [Test]
        public static void TestContourPlateRotation()
        {
            Model model = new Model();


            if (model.GetConnectionStatus())
            {
                CoordinateSystem cs1 = BeamExtensionTests.getCoordinateSystem1();
                CoordinateSystem cs2 = BeamExtensionTests.getCoordinateSystem2();

                // move by operation
                ContourPlate plate1 = ContourPlateFactory(); // we need factory methods because the same points mutate
                plate1.Insert();

                CoordinateSystem coordinateSystem = plate1.GetCoordinateSystem();

                Operation.MoveObject(plate1, cs1, cs2);
                plate1.Select(); // gray       

                // set up second beamGeometric
                ContourPlate plate2 = ContourPlateFactory();
                plate2.Insert();

                Matrix matrix = BeamExtensions.FromObjectToObjectTransformationMatrix(cs1, cs2);

                plate2.TransformByMutation(matrix);
                plate2.Modify();
                plate2.Select(); // update memory                        
                model.CommitChanges();
                
                ClassicAssert.AreEqual(plate1.Contour.ContourPoints, plate2.Contour.ContourPoints);
                Assert.That(plate2.Position.RotationOffset, Is.EqualTo(plate1.Position.RotationOffset).Within(0.1));

                model.CommitChanges();
            }
        }

        public static ContourPlate ContourPlateFactory()
        {
            ContourPoint point1 = new ContourPoint(new Point(0, 0, 0), null);
            ContourPoint point2 = new ContourPoint(new Point(6000, 6000, 0), null);
            ContourPoint point3 = new ContourPoint(new Point(3000, 8000, 0), null);
            ContourPoint point4 = new ContourPoint(new Point(-2000, 3000, 0), null);

            ContourPlate contour = new ContourPlate();

            contour.AddContourPoint(point1);
            contour.AddContourPoint(point2);
            contour.AddContourPoint(point3);
            contour.AddContourPoint(point4);

            contour.Finish = "FOO";
            contour.Profile.ProfileString = "PL200";
            contour.Material.MaterialString = "K30-2";

            bool Result = false;
            Result = contour.Insert();

            return contour;
        }
    }
}
