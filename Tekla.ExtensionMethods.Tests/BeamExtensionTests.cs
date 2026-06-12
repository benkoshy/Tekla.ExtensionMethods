using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.ExtensionMethods.Tests
{
    public class BeamExtensionTests
    {
        [Test]
        [Ignore("Work in progress - We can transform by using MoveObject rather than implemented everything ourselves by hand")]
        public void TransformByOperation()
        {
            Beam beam = new Beam(); // beam create with the minimum parameters

            beam.StartPoint = new Point();
            beam.EndPoint = new Point();

            // Operation.MoveObject(Beam3, Beam1.GetCoordinateSystem(), Beam2.GetCoordinateSystem())

            CoordinateSystem cs = beam.GetCoordinateSystem();



        }
    }
}
