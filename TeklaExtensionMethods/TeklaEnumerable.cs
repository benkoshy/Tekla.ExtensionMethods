using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltControl.ExtensionMethods
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
        public static IEnumerator<T> CastTeklaEnumerator<T>(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return (T)enumerator.Current;                 
            }
        }

        /*      
        // This method will never be needed
        /// <summary>
        /// There are no Tekla Enumerators that are generics.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static TeklaIEnumeratorToIEnumerable<T> convertBadBody<T>(this IEnumerator<T> enumerator )
        {
            return new TeklaIEnumeratorToIEnumerable<T>(enumerator);
        }
    */

        /// <summary>
        /// Use this only to make Tekla Enumerators into Enumerables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static TeklaIEnumeratorToIEnumerable<T> toTeklaEnumerable<T>(this IEnumerator enumerator)
        {
            return new TeklaIEnumeratorToIEnumerable<T>(enumerator);
        }

        public class TeklaIEnumeratorToIEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> genericEnumerator;

            public TeklaIEnumeratorToIEnumerable(IEnumerator enumerator)
            {
                genericEnumerator = enumerator.CastTeklaEnumerator<T>();                
            } 
            public IEnumerator<T> GetEnumerator() => genericEnumerator;
            IEnumerator IEnumerable.GetEnumerator() => genericEnumerator;
        }
    }
}
