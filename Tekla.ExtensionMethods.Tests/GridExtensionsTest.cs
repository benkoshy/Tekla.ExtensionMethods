using TeklaExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TSM = Tekla.Structures.Model;
using NSubstitute;
using Tekla.Structures;

using Tekla.Structures.Geometry3d;
using NUnit.Framework.Legacy;
using TeklaExtensionMethods.Helpers;
using TeklaExtensionMethods.Wrappers;

namespace Tekla.ExtensionMethods.Tests
{
    class GridExtensionsTest
    {
        // input string
        // get a result.

        // mock a grid object's XCoordinates
        // return list of doubles.

        [Test]
        public void ParseCoordinates_whenZero_Returns0()
        {
            string zero = "0";

            CollectionAssert.AreEquivalent(new List<double>() { 0 }, GridExtensions.parseSubstringNumber(zero));
        }

        [Test]
        public void ParseCoordinates_whenZeroDotZero_Returns0()
        {
            string zero = "0.0";

            CollectionAssert.AreEquivalent(new List<double>() { 0 }, GridExtensions.parseSubstringNumber(zero));
        }

        [Test]
        public void ParseCoordinates_whenMultiplier_Returns300Multiple()
        {
            string gridMultiplier = "3*300";

            CollectionAssert.AreEquivalent(new List<double>() { 300, 300,  300 }, GridExtensions.parseSubstringNumber(gridMultiplier));
        }

        [Test]        
        public void ParseToRunningCoordinates()
        {
            string coordinateAxis = "3*1.0";
            ClassicAssert.AreEqual(new List<double> {1, 2, 3 }, GridExtensions.ParseToRunningCoordinates(coordinateAxis));
        }

        public struct GridWrapper_ParseAxis_Case 
        {        
            public List<double> runningCoordinates_Output { get; set; }
            public string CoordinateAxis_Input { get; set; }
        }

        public static GridWrapper_ParseAxis_Case[] ParseAxisCases =
        {
            new GridWrapper_ParseAxis_Case { CoordinateAxis_Input =  "4*300.00", runningCoordinates_Output = new List<double>() { 300.00, 600.00, 900.00, 1200 }  },
            new GridWrapper_ParseAxis_Case { CoordinateAxis_Input =  "300 300 300 300", runningCoordinates_Output = new List<double>() { 300.00, 600.00, 900.00, 1200.00 }  },
            new GridWrapper_ParseAxis_Case { CoordinateAxis_Input =  "0 1 2 3 4", runningCoordinates_Output = new List<double>() {0, 1, 3, 6, 10}  },
            new GridWrapper_ParseAxis_Case { CoordinateAxis_Input =  "0 1 2 3 4 5*5", runningCoordinates_Output = new List<double>() {0, 1, 3, 6, 10, 15, 20, 25, 30, 35}  },
            new GridWrapper_ParseAxis_Case { CoordinateAxis_Input =  "1 2 3 4 5*5", runningCoordinates_Output = new List<double>() {1, 3, 6, 10, 15, 20, 25, 30, 35}  }
        };


        [Test]
        [TestCaseSource(nameof(ParseAxisCases))]
        public void GridWrapper_ParseXAxisSubstring_when_4_times_300_ReturnsListof300(GridWrapper_ParseAxis_Case testCase)
        {            
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateX.Returns(testCase.CoordinateAxis_Input);

            GridWrapper grid = new GridWrapper(gridStub);           

            CollectionAssert.AreEquivalent(testCase.runningCoordinates_Output, grid.ParseToRunningCoordinates(testCase.CoordinateAxis_Input));            
        }


        [Test]
        [TestCaseSource(nameof(ParseAxisCases))]
        public void GridWrapper_ParseYAxisSubstring_when_4_times_300_ReturnsListof300(GridWrapper_ParseAxis_Case testCase)
        {
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateY.Returns(testCase.CoordinateAxis_Input);

            GridWrapper grid = new GridWrapper(gridStub);

            CollectionAssert.AreEquivalent(testCase.runningCoordinates_Output, grid.ParseToRunningCoordinates(testCase.CoordinateAxis_Input));
        }

        [Test]
        [TestCaseSource(nameof(ParseAxisCases))]
        public void GridWrapper_ParseZAxisSubstring_when_4_times_300_ReturnsListof300(GridWrapper_ParseAxis_Case testCase)
        {
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateZ.Returns(testCase.CoordinateAxis_Input);

            GridWrapper grid = new GridWrapper(gridStub);

            CollectionAssert.AreEquivalent(testCase.runningCoordinates_Output, grid.ParseToRunningCoordinates(testCase.CoordinateAxis_Input));
        }


        [Test]
        public void GridWrapper_ParseYAxisSubstring_when_4_times_300_ReturnsListof300()
        {
            string coordinateY = "4*300.00";
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateZ.Returns(coordinateY);

            GridWrapper grid = new GridWrapper(gridStub);

            CollectionAssert.AreEquivalent(new List<double>() { 300.00, 300.00, 300.00, 300.00 }, grid.parseSubstringNumber(coordinateY));
        }

        [Test]
        public void ParseZAxisSubstring_when_4_times_300_ReturnsListof300()
        {
            string coordinateZ = "4*300.00";
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateZ.Returns(coordinateZ);

            GridWrapper grid = new GridWrapper(gridStub);

            
            CollectionAssert.AreEquivalent (new List<double>() { 300.00, 300.00, 300.00, 300.00 }, grid.parseSubstringNumber(coordinateZ)) ;
        }

        [Test]
        public void GetXAxisPlanes_when2x300()
        {
            string coordinateX = "2*300.00";

            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateX.Returns(coordinateX);
            gridStub.CoordinateZ.Returns("0");
            gridStub.Origin.Returns(new Point());
            gridStub.GetCoordinateSystem().Returns(new CoordinateSystem());

            GridWrapper grid = new GridWrapper(gridStub);

            GeometricPlane p1 = new GeometricPlane(new Point(300, 0, 0), gridStub.GetCoordinateSystem().AxisX);
            GeometricPlane p2 = new GeometricPlane(new Point(600, 0, 0), gridStub.GetCoordinateSystem().AxisX);

            Assert.That(new List<GeometricPlane>() { p1, p2}, Is.EqualTo(  grid.GetXGridPlanes()).Using(new PlaneComparer()));
        }

        [Test]
        public void GetXPlanes_when2x300_andAngledCoordinateSystem()
        {
            string coordinateX = "2*300.00";
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateX.Returns(coordinateX);
            gridStub.CoordinateZ.Returns("0");
            gridStub.Origin.Returns(new Point());
            Vector xVector = new Vector(1, 1, 0);
            Vector yVector = xVector.GetPerpendicularVector();
            gridStub.GetCoordinateSystem().Returns(new CoordinateSystem(new Point(1, 1, 0), xVector, yVector));

            GridWrapper grid = new GridWrapper(gridStub);

            double a = Math.Sqrt(45000);
            Point a1 = new Point(a + 1, a + 1, 0);
            Point a2 = new Point(2 * a + 1, 2 * a + 1, 0);

            GeometricPlane p1 = new GeometricPlane(a1, xVector);
            GeometricPlane p2 = new GeometricPlane(a2, xVector);

            ClassicAssert.AreEqual(2, grid.GetXGridPlanes().Count());
            ClassicAssert.That(new List<GeometricPlane>() { p1, p2 }, Is.EqualTo( grid.GetXGridPlanes()).Using(new PlaneComparer()));
        }       
        
        [Test]
        public void GetXPlanes_when0_2x300_andOriginIs100_100_0()
        {
            string coordinateX = "0 2*300.00";
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateX.Returns(coordinateX);
            gridStub.CoordinateZ.Returns("0");
            Point origin = new Point(100, 100, 0);
            gridStub.Origin.Returns(origin);

            Vector xVector = new Vector(1000, 0, 0);
            gridStub.GetCoordinateSystem().Returns(new CoordinateSystem(origin, xVector, xVector.GetPerpendicularVector()));
            

            GridWrapper grid = new GridWrapper(gridStub);
            GeometricPlane pl0 = new GeometricPlane(new Point(100, 100, 0), xVector);
            GeometricPlane pl1 = new GeometricPlane(new Point(100 + 300, 100, 0), xVector);
            GeometricPlane pl2 = new GeometricPlane(new Point(100 + 600, 100, 0), xVector);

            Assert.That(new List<GeometricPlane>() { pl0, pl1, pl2 }, Is.EqualTo(grid.GetXGridPlanes()).Using(new PlaneComparer()));
        }

        [Test]
        public void GetYPlanes_when2x300_400()
        {
            string coordinateY = "2*300.00 400";
            var gridStub = Substitute.For<IGrid>();
            gridStub.CoordinateY.Returns(coordinateY);
            gridStub.CoordinateZ.Returns("0");
            gridStub.Origin.Returns(new Point(0,0,0));

            gridStub.GetCoordinateSystem().Returns(new CoordinateSystem());
            Vector yVector = new Vector(0, 1000, 0);

            GridWrapper grid = new GridWrapper(gridStub);            
            GeometricPlane pl0 = new GeometricPlane(new Point(0, 300, 0), yVector);
            GeometricPlane pl1 = new GeometricPlane(new Point(0, 600, 0), yVector);
            GeometricPlane pl2 = new GeometricPlane(new Point(0, 1000, 0), yVector);

            Assert.That(new List<GeometricPlane>() { pl0, pl1, pl2 }, Is.EqualTo(grid.GetYGridPlanes()).Using(new PlaneComparer()));            
        }

        [Test]
        public void GetZPlanes_when2x300_400()
        {            
                string coordinateZ = "2*300.00 400";
                var gridStub = Substitute.For<IGrid>();
                gridStub.CoordinateX.Returns("0");
                gridStub.CoordinateY.Returns("0");
                gridStub.CoordinateZ.Returns(coordinateZ);
                
                gridStub.Origin.Returns(new Point(0,0,0));
                gridStub.GetCoordinateSystem().Returns(new CoordinateSystem(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0)));

                GridWrapper grid = new GridWrapper(gridStub);

                Vector zVector = new Vector(0, 0, 1); // 1,000,000

                GeometricPlane pl0 = new GeometricPlane(new Point(0, 0, 300), zVector);
                GeometricPlane pl1 = new GeometricPlane(new Point(0, 0, 600), zVector);
                GeometricPlane pl2 = new GeometricPlane(new Point(0, 0, 1000), zVector);
                var zGridPlanes = grid.GetZGridPlanes();                
            
                Assert.That(new List<GeometricPlane>() { pl0, pl1, pl2 }, Is.EqualTo(grid.GetZGridPlanes()).Using(new PlaneComparer()));                
        }

        [Test]
        public void TestLineCollectionEquality()
        {
            Line3d line = new Line3d(new Point(0, 300, 0), new Point(1000, 300, 0));
            Line3d line2 = new Line3d(new Point(0, 300, 0), new Point(10000, 300, 0));

            CollectionAssert.AreEquivalent(new List<Line3d>() { line}, new List<Line3d>() { line2 });
        }

        [Test]
        public void GetGridIntersectionPoints()
        {            
            Point origin = new Point(1, 1, 0);
            
            Vector xVector = new Vector(1, 1, 0);
            Vector yVector = xVector.GetPerpendicularVector();
            CoordinateSystem cs = new CoordinateSystem(origin, xVector, yVector);

            IGrid grid = Substitute.For<IGrid>();
            grid.CoordinateX.Returns("0 1");
            grid.CoordinateY.Returns("0");
            grid.Origin.Returns(origin);
            grid.GetCoordinateSystem().Returns(cs);             

            GridWrapper wrapper = new GridWrapper(grid);

            Point selectedPoint = new Point(0, 1, 0);
            Vector across = new Vector(1, 0, 0);
            
            CollectionAssert.AreEqual(new List<Point> { new Point(1, 1, 0), new Point(1 + 2 * Math.Sqrt(0.5), 1, 0)}, 
                                      wrapper.GetIntersectionPoints(selectedPoint, across));
            
        }


        private class PlaneComparer : IEqualityComparer<GeometricPlane>
        {
            public bool Equals(GeometricPlane plane1, GeometricPlane plane2)
            {
                if (plane1 == null && plane2 == null) { return true; }                    

                return plane1.Origin.Equals(plane2.Origin) && (plane1.Normal.Equals(plane2.Normal));
            }

            public int GetHashCode(GeometricPlane obj)
            {
                throw new NotImplementedException();
            }
        }


    }
}

