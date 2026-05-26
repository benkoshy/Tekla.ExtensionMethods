using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class ContourExtensions
    {
        public static Contour Transform(this Contour contour, Matrix matrix)
        {
            Contour newContour = new Contour();            

            ContourPoint[] transformedContourPoints = contour
                                                              .ContourPoints
                                                              .Cast<ContourPoint>()
                                                              .Select(cp =>
                                                              {
                                                                  Point transformedPoint = matrix.Transform(cp);
                                                                  return new ContourPoint(transformedPoint, cp.Chamfer);
                                                              }).ToArray();

            foreach (ContourPoint contourPoint in transformedContourPoints)
            {
                newContour.AddContourPoint(contourPoint);
            }

            return newContour;
        }
    }
}
