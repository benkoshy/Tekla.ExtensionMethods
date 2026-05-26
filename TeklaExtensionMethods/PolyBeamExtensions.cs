using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BoltControl.ExtensionMethods
{
    public static class PolyBeamExtensions
    {
        /// <summary>
        /// Returns a new polybeam that is the same, but transformed
        /// </summary>
        /// <param name="polybeam"></param>
        /// <param name="matrix"></param>
        public static void TransformByMutation(this PolyBeam polybeam, Matrix matrix)
        {
            Contour transformedContour = new Contour();
            ContourPoint[] transformedContourPoints = polybeam.Contour
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

            polybeam.Contour = transformedContour;            
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
