using System;
using System.Net;
using System.Runtime.CompilerServices;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

namespace TeklaExtensionMethods
{
    public static class BeamExtensions
    {
        public static void TransformByMutation(this Beam beam, Matrix matrix)
        {
            Vector transformedX = beam.XVector().Transform(matrix).GetNormal();

            beam.StartPoint = beam.StartPoint.Transform(matrix);
            beam.EndPoint = beam.EndPoint.Transform(matrix);
            
            beam.Position.RotationOffset = getAngleInDegrees(transformedX.getReferenceVector().Transform(matrix), transformedX.getBeamCS_YVectorLength1000().GetNormal());
        }

        /// <summary>
        /// Replicates Operation.MoveObject method - basically a copy object to object method - except we are moving the object using a matrix transformation
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="fromCoordinateSystem"></param>
        /// <param name="toCoordinateSystem"></param>
        public static void TransformByMutationOperation(this Beam beam, CoordinateSystem fromCoordinateSystem, CoordinateSystem toCoordinateSystem)
        {
            Matrix final = FromObjectToObjectTransformationMatrix(fromCoordinateSystem, toCoordinateSystem);

            beam.TransformByMutation(final);
        }

        public static Matrix FromObjectToObjectTransformationMatrix(CoordinateSystem cs1, CoordinateSystem cs2)
        {
            Matrix worldToCS1 = MatrixFactory.ByCoordinateSystems(new CoordinateSystem(), cs1);
            Matrix CS2toCS1 = MatrixFactory.ByCoordinateSystems(cs2, cs1);
            Matrix cs1ToWorld = MatrixFactory.ByCoordinateSystems(cs1, new CoordinateSystem());
            Matrix final = worldToCS1 * CS2toCS1 * cs1ToWorld;
            return final;
        }

        public static double getAngleInDegrees(Vector referenceVector, Vector beamYVector)
        /// <summary>
        ///  This is the angle between a beam's "Y" Axis and the reference vector associated with that beam
        /// </summary>
        /// <param name="zVector"></param>
        /// <param name="beamYVector"></param>
        /// <returns></returns>
        {
            double angle = referenceVector.GetAngleBetween(beamYVector) * (180 / Math.PI);
            return angle;
        }

        /// <summary>
        /// The Minimum parameters required in order to create a beam.
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="materialString"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static Beam BeamFactory(this Beam beam, Point startPoint, Point endPoint, string materialString, string profile)
        {
            beam.StartPoint = startPoint;
            beam.EndPoint = endPoint;

            Material material = new Material();
            material.MaterialString = materialString;
            beam.Material = material;

            beam.Profile.ProfileString = profile;

            return beam;
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

        public static Vector XVector(this Beam beam)
        {
            return beam.StartPoint.GetVectorTo(beam.EndPoint);
        }

        public static Vector XAxis(this Beam beam)
        {
            return beam.XVector();
        }
        public static Vector GetReferenceVector(this Beam beam)
        {
            return beam.XVector().getReferenceVector();
        }

        public static void RotateByMutation(this Beam beam, double angleInRadians, Vector a)
        {
            Matrix matrix = MatrixFactory.Rotate(angleInRadians, a);
            beam.TransformByMutation(matrix);
        }
    }
}