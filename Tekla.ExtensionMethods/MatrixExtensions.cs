using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class MatrixExtensions
    {
        public static bool EqualsWithTolerance(this Matrix matrix, Matrix other, double tolerance = 1e-12)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Math.Abs(matrix[i, j] - other[i, j]) >= tolerance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// TODO: consider getting rid of this
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector FirstColumn(this Matrix matrix)
        {
            return new Vector(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        }

        /// <summary>
        /// TODO: consider getting rid of this
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector SecondColumn(this Matrix matrix)
        {
            return new Vector(matrix[0, 1], matrix[1, 1], matrix[2, 1]);
        }

        /// <summary>
        /// TODO: consider getting rid of this
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector ThirdColumn(this Matrix matrix)
        {
            return new Vector(matrix[0, 2], matrix[1, 2], matrix[2, 2]);
        }

        public static Vector FirstRow(this Matrix matrix)
        {
            return new Vector(matrix[0, 0], matrix[0, 1], matrix[0, 2]);
        }
        public static Vector SecondRow(this Matrix matrix)
        {
            return new Vector(matrix[1, 0], matrix[1, 1], matrix[1, 2]);
        }
        public static Vector ThirdRow(this Matrix matrix)
        {
            return new Vector(matrix[2, 0], matrix[2, 1], matrix[2, 2]);
        }

        public static Vector GetTranslation(this Matrix matrix)
        {
            return new Vector(matrix[3, 0], matrix[3, 1], matrix[3, 2]);
        }

        public static Vector GetDisplacementVector(this Matrix matrix)
        {
            return GetTranslation(matrix);
        }

        public static CoordinateSystem FromWorlCoordinatSystemToLocalCoordinateSystem(this Matrix matrix)
        {
            CoordinateSystem coordinateSystem = new CoordinateSystem();
            coordinateSystem.AxisX = matrix.FirstRow();
            coordinateSystem.AxisY = matrix.SecondRow();
            coordinateSystem.Origin = matrix.GetTranslation().ToPoint();

            return coordinateSystem;
        }

        public static Matrix RotateBy(double angleInRadians, Vector a)
        {
            return MatrixFactory.Rotate(angleInRadians, a);
        }


        /// Transformation matrix methods
        public static Matrix ThenRotateBy(this Matrix matrix, double angleInRadians, Vector a)
        {   

            return new Matrix(matrix) * RotateBy(angleInRadians, a);
        }

        public static Matrix DisplaceBy(Vector displacementVector)
        {
            // when we use a displacement vector
            // we need to set it in reverse when aligning coordinate systems           

            return MatrixFactory.ByCoordinateSystems(new CoordinateSystem(), new CoordinateSystem().WithOrigin((displacementVector * -1).ToPoint()));
        }

        public static Matrix ThenDisplaceBy(this Matrix matrix, Vector displacementVector)
        {
            return new Matrix(matrix) * DisplaceBy(displacementVector);
        }
    }
}
