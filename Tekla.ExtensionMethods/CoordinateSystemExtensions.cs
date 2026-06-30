using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace TeklaExtensionMethods
{
    public static class CoordinateSystemExtensions
    {
        /// <summary>
        /// Returns transformation rotation to get this coordinate system to a Global coordinate system.  
        /// All coordinate systems will be normalised.
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <returns></returns>
        public static Matrix ToWorldCoordinateSystem(this CoordinateSystem coordinateSystem)
        {            
            return MatrixFactory.ByCoordinateSystems(coordinateSystem, CoordinateSystemExtensions.WorldCoordinateSystem());
        }

        /// <summary>
        /// This is the equivalent of the MatrixFactory.ToCoordinateSystem(coordinateSystem) method
        /// Gets a rotation that allows a transformation from the world coordinate system to the receiver's coordinate system.
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <returns></returns>
        public static Matrix FromWorldCoordinateSystemToReceiverCoordinateSystem(this CoordinateSystem coordinateSystem)
        {            
            return MatrixFactory.ByCoordinateSystems(CoordinateSystemExtensions.WorldCoordinateSystem(), coordinateSystem);
        }

        /// <summary>
        /// Transformation from the receiver's coordinate system, to another coordinate system named as a parameter.
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="toCoordinateSystem"></param>
        /// <returns></returns>
        public static Matrix ToCoordinateSystem(this CoordinateSystem coordinateSystem, CoordinateSystem toCoordinateSystem)
        {           
            return MatrixFactory.ByCoordinateSystems(coordinateSystem, toCoordinateSystem);
        }
        public static Vector AxisZ(this CoordinateSystem coordinateSystem)
        {
            Vector xVector = coordinateSystem.AxisX;
            Vector yVector = coordinateSystem.AxisY;

            return xVector.Cross(yVector);
        }

        /// <summary>
        /// This method does not mutate.
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static CoordinateSystem Transform(this CoordinateSystem coordinateSystem, Matrix matrix)
        {
            Point origin = coordinateSystem.Origin;
            Vector xAxis = coordinateSystem.AxisX;
            Vector yAxis = coordinateSystem.AxisY;

            return new CoordinateSystem(origin.Transform(matrix), xAxis.Transform(matrix), yAxis.Transform(matrix));
        }
        public static CoordinateSystem WorldCoordinateSystem()
        {
            return new CoordinateSystem(new Point(0, 0, 0), VectorExtensions.XAxis, VectorExtensions.YAxis);
        }
      
        public static bool EqualsWithTolerance(this CoordinateSystem coordinateSystem, CoordinateSystem other, double tolerance = 1e-12)
        {
            return coordinateSystem.Origin.GetDistanceTo(other.Origin) < tolerance
                && coordinateSystem.AxisX.EqualsWithTolerance(other.AxisX, tolerance)
                && coordinateSystem.AxisY.EqualsWithTolerance(other.AxisY, tolerance);
        }

        /// <summary>
        /// Returns a new coordinate system with origin zero, and unit vectors in the x (1,0,0) and y (0,1,0) directions
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <returns></returns>
        public static CoordinateSystem ResetToWorldCoordinateSystem(this CoordinateSystem coordinateSystem)
        {
            return new CoordinateSystem(new Point(0, 0, 0), VectorExtensions.XAxis, VectorExtensions.YAxis);
        }

        public static CoordinateSystem Clone(this CoordinateSystem coordinateSystem)
        {
            CoordinateSystem newCoordinateSystem = new CoordinateSystem();
            newCoordinateSystem.Origin = coordinateSystem.Origin;
            newCoordinateSystem.AxisX = coordinateSystem.AxisX;
            newCoordinateSystem.AxisY = coordinateSystem.AxisY;
            return newCoordinateSystem;
        }

        public static CoordinateSystem WithNormalization(this CoordinateSystem coordinateSystem)
        {
            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();
            newCoordinateSystem.AxisX = coordinateSystem.AxisX.GetNormal();
            newCoordinateSystem.AxisY = coordinateSystem.AxisY.GetNormal();
            
            return newCoordinateSystem;            
        }

        public static CoordinateSystem WithOrigin(this CoordinateSystem coordinateSystem, Point origin)
        {
            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();
            newCoordinateSystem.Origin = origin;

            return newCoordinateSystem;
        }

        public static CoordinateSystem WithAxisX(this CoordinateSystem coordinateSystem, Vector xAxis)
        {
            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();
            newCoordinateSystem.AxisX = xAxis;

            return newCoordinateSystem;
        }

        /// <summary>
        /// Developers should be careful in directly setting the X axis. It should be orthogonal to the Y axis.
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="xAxis"></param>
        /// <returns></returns>

        public static CoordinateSystem WithXaxis(this CoordinateSystem coordinateSystem, Vector xAxis)
        {
            return coordinateSystem.WithAxisX(xAxis);
        }

        /// <summary>
        /// Developers should be careful in directly setting the X axis. It should be orthogonal to the Y axis.
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="yAxis"></param>
        /// <returns></returns>
        public static CoordinateSystem WithAxisY(this CoordinateSystem coordinateSystem, Vector yAxis)
        {
            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();
            newCoordinateSystem.AxisY = yAxis;

            return newCoordinateSystem;
        }

        /// <summary>
        /// Developers should be careful in directly setting the Y axis. It should be orthogonal to the Y axis.        
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="yAxis"></param>
        /// <returns></returns>
        public static CoordinateSystem WithYaxis(this CoordinateSystem coordinateSystem, Vector yAxis)
        {
            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();
            newCoordinateSystem.AxisY = yAxis;

            return newCoordinateSystem;
        }

        /// <summary>
        /// WARNING: If using this to align cooridnate systems. If you want to rotate positively around a vector, because you are aligning coordinate systems,
        /// you need to apply a negative angle to it instead.!
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="rotationInRadians"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static CoordinateSystem WithRotationBy(this CoordinateSystem coordinateSystem, double rotationInRadians, Vector vector )
        {   
            Matrix rotation = MatrixFactory.Rotate(rotationInRadians, vector); 

            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();

            Point newOrigin = coordinateSystem.Origin.Transform(rotation);
            Vector newX = coordinateSystem.AxisX.Transform(rotation);
            Vector newY = coordinateSystem.AxisY.Transform(rotation);

            newCoordinateSystem.AxisX = newX;
            newCoordinateSystem.AxisY = newY;            

            return newCoordinateSystem;
        }

        public static CoordinateSystem WithDisplacementBy(this CoordinateSystem coordinateSystem, Vector displacementVector)
        {
            Matrix rotation = new Matrix().ThenDisplaceBy(displacementVector); // because we are using coordinate systems - the applied must be negative.

            CoordinateSystem newCoordinateSystem = coordinateSystem.Clone();

            Point newOrigin = coordinateSystem.Origin.Transform(rotation);
            newCoordinateSystem.Origin = newOrigin;

            return newCoordinateSystem;
        }
    }
}
