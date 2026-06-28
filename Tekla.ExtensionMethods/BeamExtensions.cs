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
        /// Replicates Operation.MoveObject method - basically a copy object to object method - except we are moving the object.
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="fromCoordinateSystem"></param>
        /// <param name="toCoordinateSystem"></param>
        public static void TransformByMutationOperation(this Beam beam, CoordinateSystem fromCoordinateSystem, CoordinateSystem toCoordinateSystem)
        {
            Matrix worldToCS1 = MatrixFactory.ByCoordinateSystems(new CoordinateSystem(), fromCoordinateSystem);
            Matrix CS2toCS1 = MatrixFactory.ByCoordinateSystems(toCoordinateSystem, fromCoordinateSystem);
            Matrix cs1ToWorld = MatrixFactory.ByCoordinateSystems(fromCoordinateSystem, new CoordinateSystem());
            Matrix final = worldToCS1 * CS2toCS1 * cs1ToWorld;

            beam.TransformByMutation(final);
        }

        private static double getAngleInDegrees(Vector beamVectorX, Vector zVector)
        /// <summary>
        /// TODO: (Vector zVector, Vector beamYVector) should probably be the parameter names
        /// </summary>
        /// <param name="zVector"></param>
        /// <param name="beamYVector"></param>
        /// <returns></returns>
        {
            double angle = beamVectorX.GetAngleBetween(zVector) * (180 / Math.PI);
            return angle;
        }

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

        /// <summary>
        /// We have found instances where the geometric coordinate system method does not match with the beam's GetCoordinateSystem method 
        /// because the origin may differ.
        /// TODO: We cannot trust this method. Because the origin does not match with
        /// Tekla's actual calculated beam coordinates.
        /// </summary>
        /// <param name="beam"></param>
        /// <returns></returns>
        public static CoordinateSystem GetGeometricCoordinateSystem(this Beam beam)
        {
            return beam.StartPoint.GetVectorTo(beam.EndPoint).GetGeometricCoordinateSystem()
                .WithOrigin(beam.StartPoint);            
        }


        public static Vector GetReferenceVector(this Beam beam)
        {
            return beam.XVector().getReferenceVector();
        }

        public static void RotateBy(this Beam beam, double angleInRadians, Vector a)
        {
            Matrix matrix = MatrixFactory.Rotate(angleInRadians, a);
            beam.TransformByMutation(matrix);
        }
    }
}