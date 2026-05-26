using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods.Wrappers
{
    public interface IGrid
    {
        CoordinateSystem GetCoordinateSystem();        
        
        string CoordinateX { get;  }
        string CoordinateY { get; }
        string CoordinateZ { get; }
        Point Origin { get; }
    }
}


