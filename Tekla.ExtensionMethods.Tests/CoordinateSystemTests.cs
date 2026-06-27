using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;

namespace Tekla.ExtensionMethods.Tests
{
    public class CoordinateSystemTests
    {
        [Test]
        public void WorldCoordinateSystem_True()
        {
            CoordinateSystem cs = CoordinateSystemExtensions.WorldCoordinateSystem();

            // TODO: change all of these to Equals Assert:
            ClassicAssert.IsTrue(cs.Origin.Equals(new Point(0, 0, 0)));
            ClassicAssert.IsTrue(cs.AxisX.Equals(VectorExtensions.XAxis));
            ClassicAssert.IsTrue(cs.AxisY.Equals(VectorExtensions.YAxis));
        }

        [Test]
        public void EqualsWithTolerance_WhenEqual_True()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            ClassicAssert.IsTrue(cs1.EqualsWithTolerance(cs2));
        }

        [Test]
        public void EqualsWithTolerance_WhenOriginDiffers_False()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            cs2.Origin = new Point(1, 0, 0);

            ClassicAssert.IsFalse(cs1.EqualsWithTolerance(cs2));
        }

        [Test]
        public void EqualsWithTolerance_WhenXAxisDiffers_False()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            cs2.AxisX = new Vector(123, 0, 0);

            ClassicAssert.IsFalse(cs1.EqualsWithTolerance(cs2));
        }

        [Test]
        public void EqualsWithTolerance_WhenYAxisDiffers_False()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            cs2.AxisY = new Vector(0, 123, 0);

            ClassicAssert.IsFalse(cs1.EqualsWithTolerance(cs2));
        }

        // Custom Tolerances
        [Test]
        public void EqualsWithTolerance_WhenEqualWithCustomTolerance_True()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            ClassicAssert.IsTrue(cs1.EqualsWithTolerance(cs2, 1e-14));
        }

        [Test]
        public void EqualsWithTolerance_WhenOriginDiffersWithCustomTolerance_False()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            cs2.Origin = new Point(1, 0, 0);

            ClassicAssert.IsFalse(cs1.EqualsWithTolerance(cs2, 1));
        }

        [Test]
        public void EqualsWithTolerance_WhenXAxisDiffersWithCustomTolerance_False()
        {
            CoordinateSystem cs1 = new CoordinateSystem().ResetToWorldCoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem()
                                   .ResetToWorldCoordinateSystem()
                                   .WithAxisX(new Vector(10, 0, 0));            

            ClassicAssert.IsFalse(cs1.EqualsWithTolerance(cs2, 9));
        }

        [Test]
        public void EqualsWithTolerance_WhenYAxisDiffersWithCustomTolerance_False()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            cs2.AxisY = new Vector(123, 0, 0);

            ClassicAssert.IsFalse(cs1.EqualsWithTolerance(cs2));
        }

        [Test]
        public void ResetToWorldCoordinateSystem_ReturnsWorldCoordinateSystem()
        {
            CoordinateSystem cs1 = new CoordinateSystem().ResetToWorldCoordinateSystem();
            CoordinateSystem cs2 = new CoordinateSystem();
            cs2.AxisX = new Vector(1, 0, 0);
            cs2.AxisY = new Vector(0, 1, 0);

            ClassicAssert.IsTrue(cs1.EqualsWithTolerance(cs2));
        }

        [Test]
        public void Clone_NotEqual()
        {
            CoordinateSystem cs1 = new CoordinateSystem();
            CoordinateSystem cs2 = cs1.Clone();

            ClassicAssert.AreNotEqual(cs1, cs2);
            ClassicAssert.IsTrue(cs1.EqualsWithTolerance(cs2));
        }

        [Test]
        public void Orthogonalize_EqualsWCS()
        {
            CoordinateSystem cs = new CoordinateSystem().WithNormalization();

            ClassicAssert.IsTrue(cs.EqualsWithTolerance(new CoordinateSystem().ResetToWorldCoordinateSystem()));
        }

        [Test]
        public void ResetToWorldCoordinateSystem_ArbitraryCoordinateSystem1_ReturnsWorldCoordinateSystem()
        {
            CoordinateSystem cs = new CoordinateSystem().ResetToWorldCoordinateSystem();

            ClassicAssert.IsTrue(cs.EqualsWithTolerance(CoordinateSystemExtensions.WorldCoordinateSystem()));
        }

        [Test]
        public void ResetToWorldCoordinateSystem_ArbitraryCoordinateSystem2_ReturnsWorldCoordinateSystem()
        {
            CoordinateSystem cs = new CoordinateSystem()
                                      .WithOrigin(new Point(1, 2, 3))
                                      .WithXaxis(new Vector(4, 5, 6))
                                      .WithYaxis(new Vector(6, 7, 8))
                                      .WithNormalization();

            CoordinateSystem cs2 = cs.ResetToWorldCoordinateSystem();

            ClassicAssert.IsTrue(cs2.EqualsWithTolerance(CoordinateSystemExtensions.WorldCoordinateSystem()));
        }

        [Test]
        public void ToWorldCoordinateSystem_ArbitraryCoordinateSystem_ReturnsWorldCoordinateSystem()
        {
            CoordinateSystem cs = new CoordinateSystem().ResetToWorldCoordinateSystem();

            ClassicAssert.IsTrue(cs.EqualsWithTolerance(new CoordinateSystem().ResetToWorldCoordinateSystem()));
        }

        [Test]
        public void WithOrigin_ArbitraryOrigin_CoordinateSystemWithOrigin()
        {
            Point origin = new Point(1, 2, 3);
            CoordinateSystem cs = new CoordinateSystem().WithOrigin(origin);

            ClassicAssert.AreEqual(cs.Origin, origin);
        }

        [Test]
        public void WithAxisX_ArbitraryVector_CoordinateSystemWithAxisX()
        {
            Vector xAsix = new Vector(1, 2, 3);
            CoordinateSystem cs = new CoordinateSystem().WithAxisX(xAsix);

            ClassicAssert.AreEqual(cs.AxisX, xAsix);
        }

        [Test]
        public void WithXaxis_ArbitraryVector_CoordinateSystemWithXaxis()
        {
            Vector xAsix = new Vector(1, 2, 3);
            CoordinateSystem cs = new CoordinateSystem().WithAxisX(xAsix);

            ClassicAssert.AreEqual(cs.AxisX, xAsix);
        }

        [Test]
        public void WithAxisY_ArbitraryVector_CoordinateSystemWithAxisY()
        {
            Vector yAsix = new Vector(1, 2, 3);
            CoordinateSystem cs = new CoordinateSystem().WithAxisY(yAsix);

            ClassicAssert.AreEqual(cs.AxisY, yAsix);
        }

        [Test]
        public void WithYaxis_ArbitraryVector_CoordinateSystemWithAxisY()
        {
            Vector yAsix = new Vector(1, 2, 3);
            CoordinateSystem cs = new CoordinateSystem().WithYaxis(yAsix);

            ClassicAssert.AreEqual(cs.AxisY, yAsix);
        }

        [Test]
        public void FromWorldCoordinateSystemToReceiverCoordinateSystem_ArbitraryCS_CorrectMatrix()
        {
            Vector x = new Vector(1, 2, 3).GetNormal();
            Vector y = new Vector(4, 5, 6).GetNormal();
            Vector z = x.Cross(y).GetNormal();

            CoordinateSystem cs = new CoordinateSystem().WithXaxis(x)
                                                        .WithYaxis(y);

            Matrix csMatrix = cs.FromWorldCoordinateSystemToReceiverCoordinateSystem();            

            ClassicAssert.IsTrue(MatrixFactory.ByCoordinateSystems(CoordinateSystemExtensions.WorldCoordinateSystem(), cs).EqualsWithTolerance(csMatrix));
            ClassicAssert.AreEqual(cs.AxisX.GetNormal(), csMatrix.FirstColumn());
        }

        [Test]
        public void Xaxis_ArbitraryMatrix_XBasis()
        {
            Vector x = new Vector(1, 2, 3).GetNormal();
            Vector y = new Vector(4, 5, 6).GetNormal();
            Vector z = x.Cross(y).GetNormal();

            CoordinateSystem cs = new CoordinateSystem().WithXaxis(x)
                                                        .WithYaxis(y);

            Matrix csMatrix = cs.FromWorldCoordinateSystemToReceiverCoordinateSystem();            
            ClassicAssert.AreEqual(cs.AxisX.GetNormal(), csMatrix.FirstColumn());
        }

        [Test]
        public void Yaxis_ArbitraryMatrix_YBasis()
        {
            Vector x = new Vector(1, 2, 3).GetNormal();
            Vector y = new Vector(4, 5, 6).GetNormal();
            Vector z = x.Cross(y).GetNormal();

            CoordinateSystem cs = new CoordinateSystem().WithXaxis(x)
                                                        .WithYaxis(y);

            Matrix csMatrix = cs.FromWorldCoordinateSystemToReceiverCoordinateSystem();
            ClassicAssert.AreEqual(cs.AxisY.GetNormal(), csMatrix.SecondColumn());
        }

        [Test]
        public void Zaxis_ArbitraryMatrix_ZBasis()
        {
            Vector x = new Vector(1, 2, 3).GetNormal();
            Vector y = new Vector(4, 5, 6).GetNormal();
            Vector z = x.Cross(y).GetNormal();

            CoordinateSystem cs = new CoordinateSystem().WithXaxis(x)
                                                        .WithYaxis(y);

            Matrix csMatrix = cs.FromWorldCoordinateSystemToReceiverCoordinateSystem();
            ClassicAssert.AreEqual(z, csMatrix.ThirdColumn());
        }


        [Test]
        public void WithRotationBy_RotateZAxis90Degrees()
        {
            CoordinateSystem expected = new CoordinateSystem().WithAxisX(new Vector(0, -1000, 0)).WithYaxis(new Vector(1000, 0, 0));                              
            CoordinateSystem rotatedCS = new CoordinateSystem().WithRotationBy(Math.PI / 2, new Vector().ToZaxisWCS());

            Assert.That(rotatedCS, Is.EqualTo(expected).UsingPropertiesComparer());
        }
    }
}
