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

namespace Tekla.ExtensionMethods.Tests
{
    public class PolyBeamTests
    {
        [Test]
        [Ignore("This test re: polybeams fails. It is unclear where the fault is.")]
        public static void PolybeamRotation()
        {
            Model model = new Model();


            if (model.GetConnectionStatus())
            {
                CoordinateSystem cs1 = BeamExtensionTests.getCoordinateSystem1();
                CoordinateSystem cs2 = BeamExtensionTests.getCoordinateSystem2();

                // move by operation
                PolyBeam polyBeam1 = PolybeamFactory("3"); // we need factory methods because the same points mutate                
                polyBeam1.Insert();

                Operation.MoveObject(polyBeam1, cs1, cs2);
                polyBeam1.Select(); 
                
                PolyBeam polyBeam2 = PolybeamFactory("5");
                polyBeam2.Insert();

                Matrix matrix = BeamExtensions.FromObjectToObjectTransformationMatrix(cs1, cs2);

                polyBeam2.TransformByMutation(matrix);
                polyBeam2.Modify();
                polyBeam2.Select(); 
                model.CommitChanges();

                ClassicAssert.AreEqual(polyBeam1.Contour.ContourPoints, polyBeam2.Contour.ContourPoints);
                Assert.That(polyBeam2.Position.RotationOffset, Is.EqualTo(polyBeam1.Position.RotationOffset).Within(0.1));                
            }
        }

        public static PolyBeam PolybeamFactory(string classString)
        {
            ContourPoint point = new ContourPoint(new Point(0, 2000, 0), null);
            ContourPoint point2 = new ContourPoint(new Point(2000, 2000, 0), null);
            ContourPoint point3 = new ContourPoint(new Point(0, 4000, 0), null);

            PolyBeam polyBeam = new PolyBeam();

            polyBeam.AddContourPoint(point);
            polyBeam.AddContourPoint(point2);
            polyBeam.AddContourPoint(point3);

            polyBeam.Profile.ProfileString = "HEA400";
            polyBeam.Finish = "PAINT";
            polyBeam.Class = classString;
            bool Result = false;
            return polyBeam;
        }

        [Test]
        public void readmeTests()
        {
            Model model = new Model();

            ModelObjectSelector Selector = model.GetModelObjectSelector();

            foreach (ModelObject MO in Selector)
            {
                Beam B = MO as Beam;
                if (B != null)
                {
                    Solid solid = B.GetSolid();
                }
            }            
        }
    }
}
