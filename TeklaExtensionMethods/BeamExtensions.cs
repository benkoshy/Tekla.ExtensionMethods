using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BoltControl.ExtensionMethods
{
    public static class BeamExtensions
    {
        public static void TransformByMutation(this Beam beam, Matrix matrix)
        { 
            beam.StartPoint = beam.StartPoint.Transform(matrix);
            beam.EndPoint = beam.EndPoint.Transform(matrix);
        }

        public static Beam CloneByProperties(this Beam beam)
        {
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
