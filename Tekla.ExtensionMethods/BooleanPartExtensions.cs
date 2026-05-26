using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaExtensionMethods
{
    public static class BooleanPartExtensions
    {

        /// <summary>
        /// We cannot clone the operative part
        /// </summary>
        /// <param name="booleanPart"></param>
        /// <returns></returns>
       public static BooleanPart CloneProperties(this BooleanPart booleanPart) 
       { 
            BooleanPart newBooleanPart = new BooleanPart();            
            newBooleanPart.Father = booleanPart.Father;                        
            newBooleanPart.Type = booleanPart.Type;

            return newBooleanPart;       
        }
    }
}
