using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using System.Text.RegularExpressions;

namespace TeklaExtensionMethods.Wrappers
{
    public class GridWrapper : IGrid
    {
        private readonly IGrid grid;

        public string CoordinateX => grid.CoordinateX;

        public string CoordinateY => grid.CoordinateY;

        public string CoordinateZ => grid.CoordinateZ;

        public Point Origin => grid.Origin;

        public GridWrapper(IGrid grid)
        {
            this.grid = grid;
        }

        public IEnumerable<GeometricPlane> GetXGridPlanes()
        {
            Tekla.Structures.Geometry3d.CoordinateSystem gridCs = grid.GetCoordinateSystem();
            Matrix transformation = grid.GetCoordinateSystem().ToWorldCoordinateSystem();
            Vector normalVector = gridCs.AxisX;

            var xgridplanes = this.XAxisLocalCoordinates()
                .Select(p => p.Transform(transformation))
                .Select(p => new GeometricPlane(p, normalVector)).ToList();

            return xgridplanes;
        }


        public IEnumerable<GeometricPlane> GetYGridPlanes()
        {
            Tekla.Structures.Geometry3d.CoordinateSystem gridCs = grid.GetCoordinateSystem();
            Matrix transformation = grid.GetCoordinateSystem().ToWorldCoordinateSystem();
            Vector normalVector = gridCs.AxisY;

            var yGridPlanes = this.YAxisLocalCoordinates()
                .Select(p => p.Transform(transformation))
                .Select(p => new GeometricPlane(p, normalVector)).ToList();

            return yGridPlanes;
        }

        public IEnumerable<GeometricPlane> GetZGridPlanes()
        {
            Tekla.Structures.Geometry3d.CoordinateSystem gridCs = grid.GetCoordinateSystem();
            Matrix transformation = grid.GetCoordinateSystem().ToWorldCoordinateSystem();
            Vector normalVector = gridCs.AxisZ();

            var zGridPlanes = this.ZAxisLocalCoordinates()
                                .Select(p => p.Transform(transformation))
                                .Select(p => new GeometricPlane(p, normalVector))
                                .ToList();


            return zGridPlanes;
        }

        public List<Point> GetIntersectionPoints(Point point, Vector vector)
        {
            Line line = new Line(point, point.getPointTo(vector));

            List<GeometricPlane> planes =  GetAllGridPlanes().Distinct().ToList();

            var points =  planes
                        .Where(pl => pl.DoesIntersectWith(line))
                        .Select(pl => pl.IntersectsWith(line))
                        .Distinct()
                        .ToList();

            return points;
        }

        /// <summary>
        /// consider getting rid of this - it's meaningless
        /// you need the plane.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GeometricPlane> GetAllGridPlanes()
        {
            List<GeometricPlane> allGridLines = new List<GeometricPlane>();

            allGridLines.AddRange(this.GetXGridPlanes());
            allGridLines.AddRange(this.GetYGridPlanes());
            allGridLines.AddRange(this.GetZGridPlanes());

            return allGridLines;
        }

        public CoordinateSystem GetCoordinateSystem()
        {
            return grid.GetCoordinateSystem();
        }
        public IEnumerable<double> runningCoordinateX()
        {
            string xCoordinates = grid.CoordinateX;

            return ParseToRunningCoordinates(xCoordinates);
        }

        public IEnumerable<double> runningCoordinateY()
        {
            string yCoordinates = grid.CoordinateY;

            return ParseToRunningCoordinates(yCoordinates);
        }

        public IEnumerable<double> runningCoordinateZ()
        {
            string zCoordinates = grid.CoordinateZ;

            return ParseToRunningCoordinates(zCoordinates);

        }

        /// <summary>
        /// "0 3000 3000" becomes "0 3000 6000"
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public IEnumerable<double> ParseToRunningCoordinates(string coordinates)
        {
            double sum = 0;
            return coordinates.Split(' ')
                        .SelectMany(d => parseSubstringNumber(d))
                        .Select((a) =>
                        {
                            double runningCoordinate = sum + a;
                            sum += a;
                            return runningCoordinate;
                        });
        }

        // we need to parse for these values.
        // 0.00 4*3000.00 
        public List<double> parseSubstringNumber(string number)
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

        public IEnumerable<Point> XAxisLocalCoordinates()
        {
            return this.runningCoordinateX()
                       .Select(x => new Point(x, 0, 0));
        }

        public IEnumerable<Point> YAxisLocalCoordinates()
        {
            return this.runningCoordinateY()
                       .Select(y => new Point(0, y, 0));
        }

        public IEnumerable<Point> ZAxisLocalCoordinates()
        {
            return this.runningCoordinateZ()
                       .Select(z => new Point(0, 0, z));
        }
    }
}
