using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class PolyBeamExtensions
    {
        /// <summary>
        /// Returns a new polyBeam that is the same, but transformed.
        /// WARNING: this method does not seem to actually update the contours.
        /// It is unclear whether this is a bug in our code or in Tekla's.
        /// </summary>
        /// <param name="polyBeam"></param>
        /// <param name="matrix"></param>
        [Obsolete("This method may not work. The tests fail on my instance but it may work on yours. Use with caution because it is not clear where the problem lies")]
        public static void TransformByMutation(this PolyBeam polyBeam, Matrix matrix)
        {
            CoordinateSystem contourCoordinateSystem = polyBeam.GetCoordinateSystem();
            Vector transformedX = contourCoordinateSystem.AxisX.Transform(matrix).GetNormal();

            Contour transformedContour = new Contour();
            ContourPoint[] transformedContourPoints = polyBeam.Contour
                                                              .ContourPoints
                                                              .Cast<ContourPoint>()
                                                              .Select(cp =>
                                                              {
                                                                  Point transformedPoint = matrix.Transform(cp);
                                                                  return new ContourPoint(transformedPoint, cp.Chamfer);
                                                              }).ToArray();

            foreach (ContourPoint contourPoint in transformedContourPoints)
            {
                transformedContour.AddContourPoint(contourPoint);
            }            

            polyBeam.Contour = transformedContour;

            polyBeam.Position.RotationOffset = BeamExtensions.getAngleInDegrees(transformedX.getReferenceVector().Transform(matrix), transformedX.getBeamCS_YVectorLength1000().GetNormal());
        }

        public static PolyBeam CloneByProperties(this PolyBeam polyBeam)
        {
            PolyBeam newPolyBeam = new PolyBeam();
            newPolyBeam.AssemblyNumber = polyBeam.AssemblyNumber;
            newPolyBeam.CastUnitType = polyBeam.CastUnitType;
            newPolyBeam.Class = polyBeam.Class;
            newPolyBeam.Contour = polyBeam.Contour;
            newPolyBeam.DeformingData = polyBeam.DeformingData;
            newPolyBeam.Finish = polyBeam.Finish;
            newPolyBeam.Material = polyBeam.Material;
            newPolyBeam.Name = polyBeam.Name;
            newPolyBeam.PartNumber = polyBeam.PartNumber;
            newPolyBeam.Position = polyBeam.Position;
            newPolyBeam.PourPhase = polyBeam.PourPhase;
            newPolyBeam.Profile = polyBeam.Profile;

            return newPolyBeam;
        }
    }
}
