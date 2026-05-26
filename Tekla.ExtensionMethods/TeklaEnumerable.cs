using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Catalogs;

namespace TeklaExtensionMethods
{
    public static class TeklaEnumerable
    {
        /// <summary>
        /// Should only be used on Tekla APIs which return Enumerator objects.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static IEnumerator<T> CastTeklaIEnumerator<T>(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return (T)enumerator.Current;                 
            }
        }
        
        public static List<T> ToTeklaList<T>(this IEnumerator enumerator)
        {
            CatalogHandler catalogHandler = new CatalogHandler();
            
            var list = new List<T>();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is T)
                {
                    list.Add((T)enumerator.Current);
                }
                
            }

            return list;
        }
    }
}
