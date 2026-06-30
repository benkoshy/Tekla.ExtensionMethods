using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using TeklaExtensionMethods;

namespace Tekla.ExtensionMethods.Tests
{
    public class ReadmeTests
    {
        [Test]
        public void readmeTests()
        {
            Model model = new Model();

            ModelObjectSelector Selector = model.GetModelObjectSelector();

            // The Old Way
            foreach (ModelObject MO in Selector)
            {
                Beam B = MO as Beam;
                if (B != null)
                {
                    Solid solid = B.GetSolid();
                }
            }

            // The new way:
            List<Solid> solids = Selector.GetAllObjects().ToTeklaList<Beam>().Select(b => b.GetSolid()).ToList();
        }
    }
}
