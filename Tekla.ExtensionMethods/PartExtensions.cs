using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class PartExtensions
    {
        public static Part CloneByProperties(this Part part)
        {
            if (part is Beam)
            {
                Beam derivativePart = (Beam)part;

                return derivativePart.CloneByProperties();
            }

            if (part is ContourPlate)
            {
                ContourPlate derivativePart = (ContourPlate)part;

                return derivativePart.CloneByProperties();
            }

            if (part is PolyBeam)
            {
                PolyBeam derivativePart = (PolyBeam)part;

                return derivativePart.CloneByProperties(); ;
            }

            throw new NotImplementedException();
        }


        public static void TransformByMutation(this Part part, Matrix matrix)
        {
            if (part is Beam)
            {
                Beam derivativePart = (Beam)part;
                derivativePart.TransformByMutation(matrix);
            }

            if (part is ContourPlate)
            {
                ContourPlate derivativePart = (ContourPlate)part;
                derivativePart.TransformByMutation(matrix);
            }

            if (part is PolyBeam)
            {
                PolyBeam derivativePart = (PolyBeam)part;
                derivativePart.TransformByMutation(matrix);
            }
        }
    }
}
