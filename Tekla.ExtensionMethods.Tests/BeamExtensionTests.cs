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

                // set up second beam_moved_geometrically
                Beam beam2manuallyTransformed = beamFactory(startPointFactory(), endPointFactory(), "2"); // orange class string            
                beam2manuallyTransformed.Insert();

                beam2manuallyTransformed.TransformByMutationOperation(cs1, cs2);
                beam2manuallyTransformed.Modify();
                beam2manuallyTransformed.Select(); // update memory                        
                model.CommitChanges();

                ClassicAssert.AreEqual(beam1_moved_by_operation.StartPoint, beam2manuallyTransformed.StartPoint);
                ClassicAssert.AreEqual(beam1_moved_by_operation.EndPoint, beam2manuallyTransformed.EndPoint);
                Assert.That(beam2manuallyTransformed.Position.RotationOffset, Is.EqualTo(beam1_moved_by_operation.Position.RotationOffset).Within(0.1));
            }           
        }
        
        private static Point endPointFactory()
        {
            return new Point(400, 0, 0);
        }

        private static Point startPointFactory()
        {
            return new Point(300, 0, 0);
        }

        private static Beam beamFactory(Point startPoint, Point endPoint, string classString = "4", string profileString = "PL10*140")
        {
            // move beam_moved_geometrically 3 by coordinate system.
            Beam beam = new Beam(startPoint, endPoint);
            Material material = new Material();
            material.MaterialString = "250";
            beam.Material = material;
            beam.Profile.ProfileString = profileString; // make sure profile string is here.
            beam.Class = classString;
            return beam;
        }
        public CoordinateSystem getCoordinateSystem1()
        {
            CoordinateSystem cs = new CoordinateSystem();
            cs.Origin = new Point(0, 0, 0);

            Vector xAxis = new Vector(1, 1, 0);
            cs.AxisX = xAxis;
            cs.AxisY = xAxis.getBeamCS_YVectorLength1000(); // Ideally: this should be a pure method without calculation.

            return cs;
        }

        public CoordinateSystem getCoordinateSystem2()
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

            // geometric beam_moved_geometrically CS must match
            // but the origin does not match.
            CoordinateSystem geometricCS = beam.GetGeometricCoordinateSystem();

            Assert.That(beam.GetGeometricCoordinateSystem().EqualsWithTolerance(beamCS));            

            model.CommitChanges();
        }

        [Test]
        public void RotateBeam()
        {
            Model model = new Model();

            if (model.GetConnectionStatus())
            {
                // I have a beam that I need to move geometrically
                // what would you rather be do?
                // do some mental gymnastic to work out a coordinate system transformation 
                // or would you rather say - I want to rotate this beam around the "x" axis and then be done with it?
                // I know what I would prefer!

                Beam beam_moved_by_operation = beamFactory(new Point(), endPointFactory(), "1", "UB150*14"); // we need factory methods because the same points mutate
                beam_moved_by_operation.Insert(); 
                Operation.MoveObject(beam_moved_by_operation, new CoordinateSystem(), new CoordinateSystem().WithAxisY(new Vector(0,1,1)));
                beam_moved_by_operation.Select(); // gray       


                Beam beam_moved_geometrically = beamFactory(new Point(), endPointFactory(), "4", "UB150*14"); // draw on the x axis
                beam_moved_geometrically.Insert();
                beam_moved_geometrically.Select();
             
                beam_moved_geometrically.RotateBy(Math.PI / 4, new Vector().ToXaxisWCS()); // and we rotate around the x axis - and the beam_moved_geometrically should flip!
                beam_moved_geometrically.Modify();
                beam_moved_geometrically.Select();

                Assert.That(beam_moved_by_operation.GetCoordinateSystem(), Is.EqualTo(beam_moved_geometrically.GetCoordinateSystem()).UsingPropertiesComparer());

                model.CommitChanges();

            }
        }
    }
}

