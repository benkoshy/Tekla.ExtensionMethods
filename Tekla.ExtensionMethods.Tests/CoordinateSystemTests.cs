using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;

namespace Tekla.ExtensionMethods.Tests
{
    public class CoordinateSystemTests
    {
        [Test]
        public void ZAxis_WhenXYVectors_returnsUp()
        {
            CoordinateSystem cs = new CoordinateSystem();
            ClassicAssert.AreEqual(new Vector(0, 0, 1), cs.AxisZ().GetNormal());
        }
    }
}
