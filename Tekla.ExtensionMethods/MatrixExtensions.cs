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

        public static Vector FirstColumn(this Matrix matrix)
        {
            return new Vector(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        }

        public static Vector SecondColumn(this Matrix matrix)
        {
            return new Vector(matrix[0, 1], matrix[1, 1], matrix[2, 1]);
        }

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

        public static CoordinateSystem FromWorlCoordinatSystemToLocalCoordinateSystem(this Matrix matrix)
        {
            CoordinateSystem coordinateSystem = new CoordinateSystem();
            coordinateSystem.AxisX = matrix.FirstRow();
            coordinateSystem.AxisY = matrix.SecondRow();
            coordinateSystem.Origin = matrix.GetTranslation().ToPoint();

            return coordinateSystem;
        }
    }
}
