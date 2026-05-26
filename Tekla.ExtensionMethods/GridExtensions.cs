using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

using System.Text.RegularExpressions;

namespace TeklaExtensionMethods
{
    public static class GridExtensions
    {
        /// <summary>
        /// In the local grid coordinate system        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static IEnumerable<Point> XAxisLocalCoordinates(this TSM.Grid grid)
        {
            return grid.runningCoordinateX()
                .Select(x => new Point(x, 0, 0));
        }

        public static IEnumerable<Point> YAxisLocalCoordinates(this TSM.Grid grid)
        {
            return grid.runningCoordinateY()
                .Select(y => new Point(0, y, 0));
        }

        public static IEnumerable<Point> ZAxisLocalCoordinates(this TSM.Grid grid)
        {
            return grid.runningCoordinateZ()
                .Select(z => new Point(0, 0, z));
        }

        public static IEnumerable<double> runningCoordinateX(this TSM.Grid grid)
        {
            string xCoordinates = grid.CoordinateX;

            return ParseToRunningCoordinates(xCoordinates);
        }

        public static IEnumerable<double> runningCoordinateY(this TSM.Grid grid)
        {
            string yCoordinates = grid.CoordinateY;

            return ParseToRunningCoordinates(yCoordinates);
        }

        public static IEnumerable<double> runningCoordinateZ(this TSM.Grid grid)
        {
            string zCoordinates = grid.CoordinateZ;

            return ParseToRunningCoordinates(zCoordinates);
        }

        public static Point GetClosestGridPoint(this TSM.Grid grid, Point selectedPoint)
        {
            // throw exception if the grid doesn't have any points.
            return grid.GetGridIntersectionPoints().OrderBy(p => p.GetDistanceTo(selectedPoint)).First();
        }

        public static List<Point> GetGridIntersectionPoints(this TSM.Grid grid)
        {
            IEnumerable<double> xCoordinates = grid.runningCoordinateX();
            IEnumerable<double> yCoordinates = grid.runningCoordinateY();
            IEnumerable<double> zCoordinates = grid.runningCoordinateZ();

            List<Point> gridIntersectionPoints = new List<Point>();

            foreach (double x in xCoordinates)
            {
                foreach (double y in yCoordinates)
                {
                    foreach (double z in zCoordinates)
                    {
                        Point point = new Point(x, y, z);
                        gridIntersectionPoints.Add(point);
                    }
                }
            }

            return gridIntersectionPoints.ToList();
        }

        public static IEnumerable<double> ParseToRunningCoordinates(string coordinates)
        {
            double sum = 0;

            return coordinates.Split(' ')
                .SelectMany(d => parseSubstringNumber(d))
                .Select((x) =>
                {
                    double runningCoordinate = sum + x;
                    sum += x;
                    return  runningCoordinate;
                });
        }

        // we need to parse for these values.
        // 0.00 4*3000.00 
        public static List<double> parseSubstringNumber(string number)
        {
            try
            {
                double d = Double.Parse(number);
                return new List<double>() { d };
            }
            catch (FormatException ex)
            {
                string regexMatcher = @"^([0-9]*)\*(\d*\.?\d*)";

                Regex regex = new Regex(regexMatcher);

                Match match = regex.Match(number);

                if (match.Success)
                {
                    int multiplier = int.Parse(match.Groups[1].Value);
                    double distance = double.Parse(match.Groups[2].Value);

                    return Enumerable.Repeat(distance, multiplier).ToList();
                }
                else
                {
                    return new List<double>() { };
                }
            }
        }

        public static List<Point> GetIntersectionPoints(this TSM.Grid grid, Point point, Vector vector)
        {
            Line line = new Line(point, point.getPointTo(vector));

            List<GeometricPlane> planes = grid.GetAllGridPlanes().Distinct().ToList();

            var points = planes
                .Where(pl => pl.DoesIntersectWith(line))
                .Select(pl => pl.IntersectsWith(line))
                .Distinct()
                .ToList();

            return points;
        }

        /// <summary>
        /// Global grid line coordinates
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static IEnumerable<GeometricPlane> GetAllGridPlanes(this TSM.Grid grid)
        {
            List<GeometricPlane> allGridLines = new List<GeometricPlane>();

            allGridLines.AddRange(grid.GetXGridPlanes());
            allGridLines.AddRange(grid.GetYGridPlanes());
            allGridLines.AddRange(grid.GetZGridPlanes());

            return allGridLines;
        }

        public static IEnumerable<GeometricPlane> GetXGridPlanes(this TSM.Grid grid)
        {
            Tekla.Structures.Geometry3d.CoordinateSystem gridCs = grid.GetCoordinateSystem();
            Matrix transformation = grid.GetCoordinateSystem().ToGlobalCoordinateSystem();
            Vector normalVector = gridCs.AxisX;

            var xgridplanes = grid.XAxisLocalCoordinates()
                .Select(p => p.Transform(transformation))
                .Select(p => new GeometricPlane(p, normalVector)).ToList();

            return xgridplanes;
        }

        public static IEnumerable<GeometricPlane> GetYGridPlanes(this TSM.Grid grid)
        {
            Tekla.Structures.Geometry3d.CoordinateSystem gridCs = grid.GetCoordinateSystem();
            Matrix transformation = grid.GetCoordinateSystem().ToGlobalCoordinateSystem();
            Vector normalVector = gridCs.AxisY;

            var yGridPlanes = grid.YAxisLocalCoordinates()
                .Select(p => p.Transform(transformation))
                .Select(p => new GeometricPlane(p, normalVector)).ToList();

            return yGridPlanes;
        }


        public static IEnumerable<GeometricPlane> GetZGridPlanes(this TSM.Grid grid)
        {
            Tekla.Structures.Geometry3d.CoordinateSystem gridCs = grid.GetCoordinateSystem();
            Matrix transformation = grid.GetCoordinateSystem().ToGlobalCoordinateSystem();
            Vector normalVector = gridCs.AxisZ();

            var zGridPlanes = grid.ZAxisLocalCoordinates()
                .Select(p => p.Transform(transformation))
                .Select(p => new GeometricPlane(p, normalVector))
                .ToList();


            return zGridPlanes;
        }

        public static Vector getTranslactionVector(this TSM.Grid grid)
        {
            Point origin = new Point();
            Point gridOrigin = grid.Origin;
            Vector translationVector = origin.GetVectorTo(gridOrigin);
            return translationVector;
        }

        public static Matrix ToGlobalCoordinateSystem(this TSM.Grid grid)
        {
            return grid.GetCoordinateSystem().ToGlobalCoordinateSystem();
        }

        public static Matrix ToLocalFromGlobalCoordinateSystem(this TSM.Grid grid)
        {
            CoordinateSystem cs = grid.GetCoordinateSystem();

            return MatrixFactory.ByCoordinateSystems(new CoordinateSystem(), cs);
        }
    }
}