using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class ContourPlateExtensions
    {
        /// <summary>
        /// Returns a new contourPlate that is the same, but transformed
        /// WARNING: you must have inserted the contour plate for this to work
        /// because we need to obtain the contour plate's coordinate system.
        /// </summary>
        /// <param name="contourPlate"></param>
        /// <param name="matrix"></param>
        public static void TransformByMutation(this ContourPlate contourPlate, Matrix matrix)
        {
            CoordinateSystem contourCoordinateSystem = contourPlate.GetCoordinateSystem();
            Vector transformedX = contourCoordinateSystem.AxisX.Transform(matrix).GetNormal();

            Contour transformedContour = new Contour();
            ContourPoint[] transformedContourPoints = contourPlate.Contour
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

            contourPlate.Contour = transformedContour;

            contourPlate.Position.RotationOffset = BeamExtensions.getAngleInDegrees(transformedX.getReferenceVector().Transform(matrix), transformedX.getBeamCS_YVectorLength1000().GetNormal());
        }

        public static ContourPlate CloneByProperties(this ContourPlate plate)
        {
            ContourPlate newPlate = new ContourPlate();
            newPlate.AssemblyNumber = plate.AssemblyNumber;
            newPlate.CastUnitType = plate.CastUnitType;
            newPlate.Class = plate.Class;
            newPlate.Contour = plate.Contour;
            newPlate.DeformingData = plate.DeformingData;            
            newPlate.Finish = plate.Finish;
            newPlate.Material = plate.Material;
            newPlate.Name = plate.Name;
            newPlate.PartNumber = plate.PartNumber;
            newPlate.Position = plate.Position;
            newPlate.PourPhase = plate.PourPhase;
            newPlate.Profile = plate.Profile;            

            return newPlate;
        }
    }
}
