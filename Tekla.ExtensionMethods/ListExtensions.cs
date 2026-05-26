using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;

using System.Linq;

namespace TeklaExtensionMethods
{
    public static class ListExtensions
    {
        public static PointList ToPointList<T>(this IEnumerable<T> points) where T : Point
        {
            PointList pl = new PointList();

            foreach (Point point in points)
            {
                pl.Add(point);
            }

            return pl;
        }
    }
}
