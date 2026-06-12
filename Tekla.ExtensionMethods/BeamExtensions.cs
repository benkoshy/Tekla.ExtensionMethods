using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace TeklaExtensionMethods
{
    public static class BeamExtensions
    {
        public static void TransformByMutation(this Beam beam, Matrix matrix)
        {
            beam.StartPoint = beam.StartPoint.Transform(matrix);
            beam.EndPoint = beam.EndPoint.Transform(matrix);
        }


        public static void TransformByMutationOperation(this Beam beam, Matrix matrix)
        {
            // work in progress
            // Reference the equivalent test in BeamExtensionTests.cs
            // beam.StartPoint = beam.StartPoint.Transform(matrix);
            // beam.EndPoint = beam.EndPoint.Transform(matrix);
        }

        public static Beam CloneByProperties(this Beam beam)
        {
            // Note: the identifier is not cloned.
            Beam newBeam = new Beam();
            newBeam.AssemblyNumber = beam.AssemblyNumber;
            newBeam.CastUnitType = beam.CastUnitType;
            newBeam.Class = beam.Class;
            newBeam.DeformingData = beam.DeformingData;
            newBeam.EndPoint = beam.EndPoint;
            newBeam.EndPointOffset = beam.EndPointOffset;
            newBeam.Finish = beam.Finish;
            newBeam.Material = beam.Material;
            newBeam.Name = beam.Name;
            newBeam.PartNumber = beam.PartNumber;
            newBeam.Position = beam.Position;
            newBeam.PourPhase = beam.PourPhase;
            newBeam.Profile = beam.Profile;
            newBeam.StartPoint = beam.StartPoint;
            newBeam.StartPointOffset = beam.StartPointOffset;
            return newBeam;
        }
    }
}