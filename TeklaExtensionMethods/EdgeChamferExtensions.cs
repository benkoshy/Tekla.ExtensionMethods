using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace BoltControl.ExtensionMethods
{
    public static class EdgeChamferExtensions
    {
        public static EdgeChamfer CloneProperties(this EdgeChamfer edgeChamfer)
        {
            EdgeChamfer newEdgeChamfer = new EdgeChamfer(edgeChamfer.FirstEnd, edgeChamfer.SecondEnd);
            newEdgeChamfer.Chamfer = edgeChamfer.Chamfer;
            newEdgeChamfer.Father = edgeChamfer.Father;
            newEdgeChamfer.FirstBevelDimension = edgeChamfer.FirstBevelDimension;
            newEdgeChamfer.Name = edgeChamfer.Name;
            newEdgeChamfer.SecondBevelDimension = edgeChamfer.SecondBevelDimension;
            newEdgeChamfer.SecondChamferEndType = edgeChamfer.SecondChamferEndType;
            return newEdgeChamfer;
        }

        public static void TransformByMutation(this EdgeChamfer edgeChamfer, Matrix matrix)
        {
            edgeChamfer.FirstEnd = edgeChamfer.FirstEnd.Transform(matrix);
            edgeChamfer.SecondEnd = edgeChamfer.SecondEnd.Transform(matrix);
        }
    }
}
