using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;


namespace TeklaExtensionMethods
{
    public static class ViewExtensions
    {
        public static IEnumerable<M> GetModelObjects<D, M>(this View view, Model model)
            where D : Tekla.Structures.Drawing.ModelObject
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