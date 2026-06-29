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
        /// <summary>
        /// Simply delegates any model object to Tekla's internal MoveObject.
        /// 
        /// WARNING: First call beam.Select() BEFORE calling
        /// beam.Modify() - otherwise you will lose all your changes.
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="cs1"></param>
        /// <param name="cs2"></param>
        public static void MoveBy(this ModelObject beam, CoordinateSystem cs1, CoordinateSystem cs2 )
        {
            CoordinateSystem cs1copied = cs1.Clone();
            CoordinateSystem cs2copied = cs2.Clone();
            Operation.MoveObject(beam, cs1copied, cs2copied);
        }
    }
}
