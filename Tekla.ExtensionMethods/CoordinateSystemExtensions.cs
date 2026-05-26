using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class CoordinateSystemExtensions
    {
        /// <summary>
        /// Returns transformation matrix to get this coordinate system to a Global coordinate system.        
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <returns></returns>
        public static Matrix ToGlobalCoordinateSystem(this CoordinateSystem coordinateSystem)
        {            
            return MatrixFactory.ByCoordinateSystems(coordinateSystem, new CoordinateSystem(new Point(0,0,0), new Vector(1,0,0), new Vector(0,1,0)));
        }

        public static Matrix ToCoordinateSystem(this CoordinateSystem coordinateSystem, CoordinateSystem toCoordinateSystem)
        {           
            return MatrixFactory.ByCoordinateSystems(coordinateSystem, toCoordinateSystem);
        }

        public static bool IsGlobalCoordinateSystem(this CoordinateSystem coordinateSystem)
        {
            return coordinateSystem.Equals(new CoordinateSystem());
        }

        public static Vector AxisZ(this CoordinateSystem coordinateSystem)
        {
            Vector xVector = coordinateSystem.AxisX;
            Vector yVector = coordinateSystem.AxisY;

            return xVector.Cross(yVector);
        }

        public static CoordinateSystem Transform(this CoordinateSystem coordinateSystem, Matrix matrix)
        {
            Point origin = coordinateSystem.Origin;
            Vector xAxis = coordinateSystem.AxisX;
            Vector yAxis = coordinateSystem.AxisY;

            return new CoordinateSystem(origin.Transform(matrix), xAxis.Transform(matrix), yAxis.Transform(matrix));
        }
    }
}
