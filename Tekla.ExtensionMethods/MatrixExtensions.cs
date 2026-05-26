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
        public static bool IsEqualTo(this Matrix matrix,  Matrix other)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {                    
                    if (Math.Abs(matrix[i, j] - other[i, j]) >= Double.Epsilon)
                    {
                        return false;
                    }
                }
            }            

            return true;            
        }
    }
}
