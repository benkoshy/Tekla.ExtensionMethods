using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace BoltControl.ExtensionMethods
{
    public static class ViewExtensions
    {
        public static IEnumerable<M> GetModelObjects<D, M>(this View view, Model model) 
                where D : Tekla.Structures.Drawing.ModelObject
                where M : Tekla.Structures.Model.ModelObject
        {
            return view.GetObjects()
                                .toTeklaEnumerable<DrawingObject>() // cache it for performance.
                                .OfType<D>()
                                .Select(g => model.SelectModelObject(g.ModelIdentifier))
                                .Cast<M>();
        }

    }
}
