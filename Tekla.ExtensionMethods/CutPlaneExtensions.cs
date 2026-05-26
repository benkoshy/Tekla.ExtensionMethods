using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace TeklaExtensionMethods
{
    public static class CutPlaneExtensions
    {       
        /// <summary>
        /// Transforms the cut plane by mutation.
        /// The caller must call Modify().
        /// </summary>
        /// <param name="cutPlane"></param>
        /// <returns></returns>
        public static void TransformByMutation(this CutPlane cutPlane, Matrix matrix)
        {           
            cutPlane.Plane = cutPlane.Plane.Transform(matrix);
        }
    }
}
