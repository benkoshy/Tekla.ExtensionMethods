using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using TeklaExtensionMethods;
using static Tekla.Structures.ModelInternal.Operation;

namespace Tekla.ExtensionMethods.Tests
{
    [TestFixture]
    public class BeamExtensionTests
    {
        [Test]
        public void TransformByOperation()
        {           
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                CoordinateSystem cs1 = getCoordinateSystem1();
                CoordinateSystem cs2 = getCoordinateSystem2();

                // move by operation
                Beam beam1_moved_by_operation = beamFactory(startPointFactory(), endPointFactory(), "1"); // we need factory methods because the same points mutate
                beam1_moved_by_operation.Insert();
                Operation.MoveObject(beam1_moved_by_operation, cs1, cs2);
                beam1_moved_by_operation.Select(); // gray       

                // set up second beamGeometric
                Beam beam2manuallyTransformed = beamFactory(startPointFactory(), endPointFactory(), "2"); // orange class string            
                beam2manuallyTransformed.Insert();

                Matrix final = BeamExtensions.FromObjectToObjectTransformationMatrix(cs1, cs2);
                beam2manuallyTransformed.TransformByMutation(final);
                beam2manuallyTransformed.Modify();
                beam2manuallyTransformed.Select(); // update memory                        
                model.CommitChanges();

                ClassicAssert.AreEqual(beam1_moved_by_operation.StartPoint, beam2manuallyTransformed.StartPoint);
                ClassicAssert.AreEqual(beam1_moved_by_operation.EndPoint, beam2manuallyTransformed.EndPoint);
                Assert.That(beam2manuallyTransformed.Position.RotationOffset, Is.EqualTo(beam1_moved_by_operation.Position.RotationOffset).Within(0.1));
            }           
        }
        
        public static Point endPointFactory()
        {
            return new Point(400, 0, 0);
        }

        public static Point startPointFactory()
        {
            return new Point(300, 0, 0);
        }

        public static Beam beamFactory(Point startPoint, Point endPoint, string classString = "4", string profileString = "PL10*140")
        {
            // move beamGeometric 3 by coordinate system.
            Beam beam = new Beam(startPoint, endPoint);
            Material material = new Material();
            material.MaterialString = "250";
            beam.Material = material;
            beam.Profile.ProfileString = profileString; // make sure profile string is here.
            beam.Class = classString;
            return beam;
        }
        public static CoordinateSystem getCoordinateSystem1()
        {
            CoordinateSystem cs = new CoordinateSystem();
            cs.Origin = new Point(0, 0, 0);

            Vector xAxis = new Vector(1, 1, 0);
            cs.AxisX = xAxis;
            cs.AxisY = xAxis.getBeamCS_YVectorLength1000(); // Ideally: this should be a pure method without calculation.

            return cs;
        }

        public static CoordinateSystem getCoordinateSystem2()
        {
            CoordinateSystem cs = new CoordinateSystem();
            cs.Origin = new Point(0, 0, 0);

            Vector xAxis = new Vector(1, 0, 1);
            cs.AxisX = xAxis;            
            cs.AxisY = xAxis.getBeamCS_YVectorLength1000(); // Ideally: This should be a pure method without calculation.

            return cs;
        }

        public class xVectorInputPoints
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(2, 0, 0));
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(100, 0, 0));
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(-100, 0, 0));
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(0, 100, 0));
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(0, -100, 0));
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(0, 0, 100));
                    yield return new TestCaseData(new Point(0, 0, 0), new Point(0, 0, -100));
                }
            }
        }


        [TestCaseSource(typeof(xVectorInputPoints), nameof(xVectorInputPoints.TestCases))]
        public void TestVectorExtensionsAndBeamExtensions(Point startPoint, Point endPoint)
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                Beam beam = beamFactory(startPoint, endPoint, "1", "UB150*14");
                beam.Insert();
                beam.Select();

                CoordinateSystem beamCS = beam.GetCoordinateSystem().WithOrigin(startPoint); // TODO: Bug -  not sure why the beamCS origin is not zero? Or perhaps the coordinate system origin is set to the workplane?
                Vector csX = beamCS.AxisX;
                Vector csY = beamCS.AxisY;
                Vector csZ = csX.Cross(csY).GetNormal();

                // Calculated Beam
                Vector calculatedX = startPoint.GetVectorTo(endPoint);
                Vector calculatedY = calculatedX.getBeamCS_YVectorLength1000();
                Vector calculatedZ = calculatedX.getBeamCS_ZVectorNormalized();

                string explanation = $"With startpoint: {startPoint} and endpoint: {endPoint}";

                Assert.That(calculatedX, Is.EqualTo(csX), $"Xaxis: {explanation}");
                Assert.That(calculatedY, Is.EqualTo(csY), $"Yaxis: {explanation}");
                Assert.That(calculatedZ, Is.EqualTo(csZ), $"Zaxis: {explanation}");

                // TODO: geometric beamGeometric CS must match
                // but the origin does not match.
                CoordinateSystem geometricCS = beam.XVector().GetGeometricCoordinateSystem();

                Assert.That(beam.XVector().GetGeometricCoordinateSystem().EqualsWithTolerance(beamCS));

                model.CommitChanges(); 
            }
        }

        [Test]        
        public void TransformByMoveOperation_TestMovingFromObjectToObjectViaMatrixTransformation()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                CoordinateSystem cs1 = getCoordinateSystem1();
                CoordinateSystem cs2 = getCoordinateSystem2();

                // move by operation
                Beam beam1_moved_by_operation = beamFactory(startPointFactory(), endPointFactory(), "1"); // we need factory methods because the same points mutate
                beam1_moved_by_operation.Insert();
                Operation.MoveObject(beam1_moved_by_operation, cs1, cs2);
                beam1_moved_by_operation.Select(); // gray       

                // set up second beamGeometric
                Beam beam2manuallyTransformed = beamFactory(startPointFactory(), endPointFactory(), "2"); // orange class string            
                beam2manuallyTransformed.Insert();                
            }
        }

        [Test]
        [Ignore("Developer Test")]
        public void DeveloperTest_MoveOperation()
        {

            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                Beam Beam1 = new Beam(new Point(0, 0, 0), new Point(3000, 0, 0));
                Beam1.Profile.ProfileString = "PL10*140";
                Beam1.Insert();

                Beam Beam2 = new Beam(new Point(3000, 0, 0), new Point(6000, 0, 0));
                Beam2.Profile.ProfileString = "PL10*140";
                Beam2.Insert();

                Beam Beam3 = new Beam(new Point(100, 0, 0), new Point(100, 0, 400));
                Beam3.Profile.ProfileString = "PL10*140";
                Beam3.Insert();

                Operation.MoveObject(Beam3, Beam1.GetCoordinateSystem(), Beam2.GetCoordinateSystem());
            }

            model.CommitChanges();
        }

        /// <summary>
        /// This is to be used for a blog post!
        /// </summary>
        [Test]        
        public void DeveloperTest_RotateBeam()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                // I have a beam that I need to move geometrically
                // what would you rather be do?
                // do some mental gymnastic to work out a coordinate system transformation 
                // or would you rather say - I want to rotate this beam around the "x" axis and then be done with it?
                // I know what I would prefer!
                Beam beamOperation = beamFactory(new Point(), endPointFactory(), "1", "UB150*14"); // we need factory methods because the same points mutate
                beamOperation.Insert();

                CoordinateSystem localCoordinateSystem = new CoordinateSystem().WithAxisY(new Vector(0, 1, 1));
                
                Operation.MoveObject(beamOperation, new CoordinateSystem(), localCoordinateSystem);
                beamOperation.Select(); // gray       


                // Move the beam geometrically

                Beam beamGeometric = beamFactory(new Point(), endPointFactory(), "3", "UB150*14"); // draw on the x axis
                beamGeometric.Insert();                
                beamGeometric.RotateByMutation(Math.PI / 4, new Vector().ToXaxisWCS()); // and we rotate around the x axis - and the beamGeometric should flip!                
                beamGeometric.Modify();
                beamGeometric.Select();

                Assert.That(beamGeometric.GetCoordinateSystem(), Is.EqualTo(beamOperation.GetCoordinateSystem()).UsingPropertiesComparer());                                                                                             

                Beam beam4 = beamFactory(new Point(), endPointFactory(), "5", "UB150*14"); // draw on the x axis               

                beam4.Insert();

                beam4.MoveBy(new CoordinateSystem(), new CoordinateSystem().WithRotationBy(-Math.PI / 4, new Vector().ToXaxisWCS()));
                beam4.Select();            

                Assert.That(beam4.GetCoordinateSystem(), Is.EqualTo(beamOperation.GetCoordinateSystem()).UsingPropertiesComparer());
                
                model.CommitChanges();
            }
        }

        /// <summary>
        /// This is to be used for a blog post!
        /// </summary>
        [Test]
        public void DeveloperTest_DisplacementOfBeam()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                Beam beam4 = beamFactory(new Point(), endPointFactory(), "5", "UB150*14"); // draw on the x axis 
                beam4.Insert();

                beam4.MoveBy(new CoordinateSystem(), new CoordinateSystem().WithDisplacementBy(new Vector(0,1000,0))); // move the vector up by 100
                beam4.Select();

                Assert.That(beam4.StartPoint, Is.EqualTo(new Point(0,1000,0)));

                model.CommitChanges();
            }
        }

        #region Developer Exeriments
        [Test]
        [Ignore("This is purely a developer experiment")]
        public void DeveloperExperiment_TestTransformByOperation()
        {
            // Is moving object Operation.MoveObject(beam, cs1, cs2);
            // simply a matter of aligning coordinate systems?
            // No - it is not a matter of aligning Coordinate Systems.
            // It is more akin to moving from Object To Object.

            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                CoordinateSystem cs1 = getCoordinateSystem1();
                CoordinateSystem cs2 = getCoordinateSystem2();

                // move by operation
                Beam beam1_moved_by_operation = beamFactory(startPointFactory(), endPointFactory(), "1"); // we need factory methods because the same points mutate
                beam1_moved_by_operation.Insert();
                Operation.MoveObject(beam1_moved_by_operation, cs1, cs2);
                beam1_moved_by_operation.Select(); // gray       

                // set up second beamGeometric
                Beam beam2manuallyTransformed = beamFactory(startPointFactory(), endPointFactory(), "2"); // orange class string            
                beam2manuallyTransformed.Insert();

                Matrix matrix = MatrixFactory.ByCoordinateSystems(getCoordinateSystem1(), getCoordinateSystem2());

                beam2manuallyTransformed.StartPoint = startPointFactory().Transform(matrix);
                beam2manuallyTransformed.Modify();
                beam2manuallyTransformed.Select(); // update memory                        
                model.CommitChanges();

                ClassicAssert.AreEqual(beam1_moved_by_operation.StartPoint, beam2manuallyTransformed.StartPoint);
                ClassicAssert.AreEqual(beam1_moved_by_operation.EndPoint, beam2manuallyTransformed.EndPoint);
                Assert.That(beam2manuallyTransformed.Position.RotationOffset, Is.EqualTo(beam1_moved_by_operation.Position.RotationOffset).Within(0.1));
            }
        }

        [Test]
        [Ignore("This is purely a developer experiment")]
        public void DeveloperExperiment_TestMutationOfPointsWhenApplyingMoveOperations()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                CoordinateSystem cs1 = getCoordinateSystem1();
                CoordinateSystem cs2 = getCoordinateSystem2();

                // move by operation
                Point startPoint = new Point(300, 0, 0);
                Point endPoint = new Point(400, 0, 0);

                Beam beam = beamFactory(startPoint, endPoint, "1"); // we need factory methods because the same points mutate
                beam.Insert();
                Operation.MoveObject(beam, cs1, cs2);
                beam.Select(); // gray       

                model.CommitChanges();

                Assert.That(startPoint, Is.Not.EqualTo(new Point(300, 0, 0))); // They should be equal!
                Assert.That(endPoint, Is.Not.EqualTo(new Point(400, 0, 0)));   // They Should be equal!
            }
        }

        [Test]
        [Ignore("Developer Test")]
        public void TestBeamCoordinateSystem()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                // move by operation
                Point startPoint = new Point(0, 0, 0);
                Point endPoint = new Point(0, 1000, 0);

                Beam beam = beamFactory(startPoint, endPoint, "1", "UB150*14"); // we need factory methods because the same points mutate                                
                beam.Insert();
                beam.Select(); // gray       

                CoordinateSystem beamCS = beam.XVector().GetGeometricCoordinateSystem(); 

                // Origin is 0, 0, -75 ---> which is the width of the beam down by 75.
                // XAxis = new Vector(0, 1000, 0)
                // YAxis = new Vector(0, 0, 1000)               

                model.CommitChanges();

                Assert.That(beam.XVector().GetGeometricCoordinateSystem(), Is.EqualTo(beamCS).UsingPropertiesComparer()); // TODO: fix this. Ignore the origins because we cannot compare them.
            }
        }

        [Test]
        [Ignore("Developer Test - investigating a beam coordinate system's origin")]
        public void TestBeamCoordinateSystem_2()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                // move by operation
                Point startPoint = new Point(455, 455, 455);
                Point endPoint = new Point(600, 1000, -800);

                Beam beam = beamFactory(startPoint, endPoint, "1", "UB150*14"); // we need factory methods because the same points mutate                                
                beam.Insert();
                beam.Select(); // gray       

                CoordinateSystem beamCS = beam.GetCoordinateSystem();
                // AxisX = {(145.000, 545.000, -1255.000)}, Origin = {(437.411, 388.890, 424.259)}, AxisY = {(234.520, 881.472, 409.887)}

                Matrix matrix = beamCS.ToWorldCoordinateSystem(); // this is not exactly the start point. Looks like Tekla subtracts something based on the width of the beam

                Point origin = new Point();
                Point newPoint = origin.Transform(matrix);

                model.CommitChanges();                
            }
        }

        #endregion
    }
}

