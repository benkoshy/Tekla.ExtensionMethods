using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using ModelObject = Tekla.Structures.Drawing.ModelObject;

namespace TeklaExtensionMethods
{
    public static class ViewExtensions
    {
        public static IEnumerable<M> GetModelObjects<D, M>(this View view, Model model)
            where D : ModelObject
            where M : Tekla.Structures.Model.ModelObject
        {
            return view.GetObjects()
                .ToTeklaList<DrawingObject>()
                .OfType<D>()
                .Select(g => model.SelectModelObject(g.ModelIdentifier))
                .Cast<M>();
        }
    }
}