using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using TeklaExtensionMethods;

namespace Tekla.ExtensionMethods
{
    public static class ModelObjectExtensions
    {
        public static void RotateByOperation(this ModelObject beam, double angleInRadians, Vector a)
        {
            Matrix matrix = MatrixFactory.Rotate(angleInRadians, a);

            Operation.MoveObject(beam, new CoordinateSystem(), new CoordinateSystem().WithRotationBy(angleInRadians, a));
        }
    }
}
