using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tekla.Structures.Geometry3d;
using TeklaExtensionMethods;
using Matrix = Tekla.Structures.Geometry3d.Matrix;

namespace Tekla.ExtensionMethods.Tests
{
    public class MatrixExtensionTests
    {
        [Test]
        public void EqualsWithTolerance_SameNumbersWithDefaultTolerance_True()
        {
            Matrix m = new Matrix();
            m[0, 1] = 1;
            
            Matrix m2 = new Matrix();
            m2[0, 1] = 1;

            ClassicAssert.IsTrue(m.EqualsWithTolerance(m2));
        }

        [Test]
        public void EqualsWithTolerance_SameNumbersWithCustomTolerance_True()
        {
            Matrix m = new Matrix();
            m[0, 1] = 1;

            Matrix m2 = new Matrix();
            m2[0, 1] = 1.9;
            
            ClassicAssert.IsTrue(m.EqualsWithTolerance(m2, 1));
        }

        [Test]
        public void EqualsWithTolerance_DifferentNumbersWithDefaultTolerance_False()
        {
            Matrix m = new Matrix();
            m[0, 1] = 1;

            Matrix m2 = new Matrix();
            m2[0, 1] = 1 + 1e-8;

            ClassicAssert.IsFalse(m.EqualsWithTolerance(m2));
        }

        [Test]
        public void EqualsWithTolerance_DifferentNumbersWithCustomTolerance_False()
        {
            Matrix m = new Matrix();
            m[0, 1] = 1;

            Matrix m2 = new Matrix();
            m2[0, 1] = 2.1;

            ClassicAssert.IsFalse(m.EqualsWithTolerance(m2, 1));
        }
    }
}