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
        /// TODO: this will not handle complex rotations.
        /// We can delegate this to Tekla's move method
        /// and it should handle everything automatically.
        /// </summary>
        /// <param name="contourPlate"></param>
        /// <param name="matrix"></param>
        public static void TransformByMutation(this ContourPlate contourPlate, Matrix matrix)
        {
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
