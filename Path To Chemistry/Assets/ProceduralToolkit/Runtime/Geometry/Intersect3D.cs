using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    ///     Collection of intersection algorithms
    /// </summary>
    public static partial class Intersect
    {
        #region Point-Line

        /// <summary>
        ///     Tests if the point lies on the line
        /// </summary>
        public static bool PointLine(Vector3 point, Line3 line)
        {
            return PointLine(point, line.origin, line.direction);
        }

        /// <summary>
        ///     Tests if the point lies on the line
        /// </summary>
        public static bool PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection)
        {
            return Distance.PointLine(point, lineOrigin, lineDirection) < Geometry.Epsilon;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        ///     Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector3 point, Ray ray)
        {
            return PointRay(point, ray.origin, ray.direction);
        }

        /// <summary>
        ///     Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection)
        {
            return Distance.PointRay(point, rayOrigin, rayDirection) < Geometry.Epsilon;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        ///     Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector3 point, Segment3 segment)
        {
            return PointSegment(point, segment.a, segment.b);
        }

        /// <summary>
        ///     Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Distance.PointSegment(point, segmentA, segmentB) < Geometry.Epsilon;
        }

        #endregion Point-Segment

        #region Point-Sphere

        /// <summary>
        ///     Tests if the point is inside the sphere
        /// </summary>
        public static bool PointSphere(Vector3 point, Sphere sphere)
        {
            return PointSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        ///     Tests if the point is inside the sphere
        /// </summary>
        public static bool PointSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
        {
            // For points on the sphere's surface magnitude is more stable than sqrMagnitude
            return (point - sphereCenter).magnitude < sphereRadius + Geometry.Epsilon;
        }

        #endregion Point-Sphere

        #region Line-Line

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line3 lineA, Line3 lineB)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line3 lineA, Line3 lineB, out Vector3 intersection)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector3 originA, Vector3 directionA, Vector3 originB, Vector3 directionB)
        {
            return LineLine(originA, directionA, originB, directionB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector3 originA, Vector3 directionA, Vector3 originB, Vector3 directionB,
            out Vector3 intersection)
        {
            var sqrMagnitudeA = directionA.sqrMagnitude;
            var sqrMagnitudeB = directionB.sqrMagnitude;
            var dotAB = Vector3.Dot(directionA, directionB);

            var denominator = sqrMagnitudeA * sqrMagnitudeB - dotAB * dotAB;
            var originBToA = originA - originB;
            var a = Vector3.Dot(directionA, originBToA);
            var b = Vector3.Dot(directionB, originBToA);

            Vector3 closestPointA;
            Vector3 closestPointB;
            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                var distanceB = dotAB > sqrMagnitudeB ? a / dotAB : b / sqrMagnitudeB;

                closestPointA = originA;
                closestPointB = originB + directionB * distanceB;
            }
            else
            {
                // Not parallel
                var distanceA = (sqrMagnitudeA * b - dotAB * a) / denominator;
                var distanceB = (dotAB * b - sqrMagnitudeB * a) / denominator;

                closestPointA = originA + directionA * distanceA;
                closestPointB = originB + directionB * distanceB;
            }

            if ((closestPointB - closestPointA).sqrMagnitude < Geometry.Epsilon)
            {
                intersection = closestPointA;
                return true;
            }

            intersection = Vector3.zero;
            return false;
        }

        #endregion Line-Line

        #region Line-Sphere

        /// <summary>
        ///     Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Line3 line, Sphere sphere)
        {
            return LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Line3 line, Sphere sphere, out IntersectionLineSphere intersection)
        {
            return LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter,
            float sphereRadius)
        {
            return LineSphere(lineOrigin, lineDirection, sphereCenter, sphereRadius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter,
            float sphereRadius,
            out IntersectionLineSphere intersection)
        {
            var originToCenter = sphereCenter - lineOrigin;
            var centerProjection = Vector3.Dot(lineDirection, originToCenter);
            var sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection * centerProjection;

            var sqrDistanceToIntersection = sphereRadius * sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionLineSphere.None();
                return false;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                intersection = IntersectionLineSphere.Point(lineOrigin + lineDirection * centerProjection);
                return true;
            }

            var distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            var distanceA = centerProjection - distanceToIntersection;
            var distanceB = centerProjection + distanceToIntersection;

            var pointA = lineOrigin + lineDirection * distanceA;
            var pointB = lineOrigin + lineDirection * distanceB;
            intersection = IntersectionLineSphere.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Line-Sphere

        #region Ray-Sphere

        /// <summary>
        ///     Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Ray ray, Sphere sphere)
        {
            return RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Ray ray, Sphere sphere, out IntersectionRaySphere intersection)
        {
            return RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius)
        {
            return RaySphere(rayOrigin, rayDirection, sphereCenter, sphereRadius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius,
            out IntersectionRaySphere intersection)
        {
            var originToCenter = sphereCenter - rayOrigin;
            var centerProjection = Vector3.Dot(rayDirection, originToCenter);
            if (centerProjection + sphereRadius < -Geometry.Epsilon)
            {
                intersection = IntersectionRaySphere.None();
                return false;
            }

            var sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection * centerProjection;
            var sqrDistanceToIntersection = sphereRadius * sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionRaySphere.None();
                return false;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    intersection = IntersectionRaySphere.None();
                    return false;
                }

                intersection = IntersectionRaySphere.Point(rayOrigin + rayDirection * centerProjection);
                return true;
            }

            // Line intersection
            var distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            var distanceA = centerProjection - distanceToIntersection;
            var distanceB = centerProjection + distanceToIntersection;

            if (distanceA < -Geometry.Epsilon)
            {
                if (distanceB < -Geometry.Epsilon)
                {
                    intersection = IntersectionRaySphere.None();
                    return false;
                }

                intersection = IntersectionRaySphere.Point(rayOrigin + rayDirection * distanceB);
                return true;
            }

            var pointA = rayOrigin + rayDirection * distanceA;
            var pointB = rayOrigin + rayDirection * distanceB;
            intersection = IntersectionRaySphere.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Ray-Sphere

        #region Segment-Sphere

        /// <summary>
        ///     Computes an intersection of the segment and the sphere
        /// </summary>
        public static bool SegmentSphere(Segment3 segment, Sphere sphere)
        {
            return SegmentSphere(segment.a, segment.b, sphere.center, sphere.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segment and the sphere
        /// </summary>
        public static bool SegmentSphere(Segment3 segment, Sphere sphere, out IntersectionSegmentSphere intersection)
        {
            return SegmentSphere(segment.a, segment.b, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segment and the sphere
        /// </summary>
        public static bool SegmentSphere(Vector3 segmentA, Vector3 segmentB, Vector3 sphereCenter, float sphereRadius)
        {
            return SegmentSphere(segmentA, segmentB, sphereCenter, sphereRadius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segment and the sphere
        /// </summary>
        public static bool SegmentSphere(Vector3 segmentA, Vector3 segmentB, Vector3 sphereCenter, float sphereRadius,
            out IntersectionSegmentSphere intersection)
        {
            var segmentAToCenter = sphereCenter - segmentA;
            var fromAtoB = segmentB - segmentA;
            var segmentLength = fromAtoB.magnitude;
            if (segmentLength < Geometry.Epsilon)
            {
                var distanceToPoint = segmentAToCenter.magnitude;
                if (distanceToPoint < sphereRadius + Geometry.Epsilon)
                {
                    if (distanceToPoint > sphereRadius - Geometry.Epsilon)
                    {
                        intersection = IntersectionSegmentSphere.Point(segmentA);
                        return true;
                    }

                    intersection = IntersectionSegmentSphere.None();
                    return true;
                }

                intersection = IntersectionSegmentSphere.None();
                return false;
            }

            var segmentDirection = fromAtoB.normalized;
            var centerProjection = Vector3.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + sphereRadius < -Geometry.Epsilon ||
                centerProjection - sphereRadius > segmentLength + Geometry.Epsilon)
            {
                intersection = IntersectionSegmentSphere.None();
                return false;
            }

            var sqrDistanceToLine = segmentAToCenter.sqrMagnitude - centerProjection * centerProjection;
            var sqrDistanceToIntersection = sphereRadius * sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionSegmentSphere.None();
                return false;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon ||
                    centerProjection > segmentLength + Geometry.Epsilon)
                {
                    intersection = IntersectionSegmentSphere.None();
                    return false;
                }

                intersection = IntersectionSegmentSphere.Point(segmentA + segmentDirection * centerProjection);
                return true;
            }

            // Line intersection
            var distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            var distanceA = centerProjection - distanceToIntersection;
            var distanceB = centerProjection + distanceToIntersection;

            var pointAIsAfterSegmentA = distanceA > -Geometry.Epsilon;
            var pointBIsBeforeSegmentB = distanceB < segmentLength + Geometry.Epsilon;

            if (pointAIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                var pointA = segmentA + segmentDirection * distanceA;
                var pointB = segmentA + segmentDirection * distanceB;
                intersection = IntersectionSegmentSphere.TwoPoints(pointA, pointB);
                return true;
            }

            if (!pointAIsAfterSegmentA && !pointBIsBeforeSegmentB)
            {
                // The segment is inside, but no intersection
                intersection = IntersectionSegmentSphere.None();
                return true;
            }

            var pointAIsBeforeSegmentB = distanceA < segmentLength + Geometry.Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
            {
                // Point A intersection
                intersection = IntersectionSegmentSphere.Point(segmentA + segmentDirection * distanceA);
                return true;
            }

            var pointBIsAfterSegmentA = distanceB > -Geometry.Epsilon;
            if (pointBIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                // Point B intersection
                intersection = IntersectionSegmentSphere.Point(segmentA + segmentDirection * distanceB);
                return true;
            }

            intersection = IntersectionSegmentSphere.None();
            return false;
        }

        #endregion Segment-Sphere

        #region Sphere-Sphere

        /// <summary>
        ///     Computes an intersection of the spheres
        /// </summary>
        /// <returns>True if the spheres intersect or one sphere is contained within the other</returns>
        public static bool SphereSphere(Sphere sphereA, Sphere sphereB)
        {
            return SphereSphere(sphereA.center, sphereA.radius, sphereB.center, sphereB.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the spheres
        /// </summary>
        /// <returns>True if the spheres intersect or one sphere is contained within the other</returns>
        public static bool SphereSphere(Sphere sphereA, Sphere sphereB, out IntersectionSphereSphere intersection)
        {
            return SphereSphere(sphereA.center, sphereA.radius, sphereB.center, sphereB.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the spheres
        /// </summary>
        /// <returns>True if the spheres intersect or one sphere is contained within the other</returns>
        public static bool SphereSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB)
        {
            return SphereSphere(centerA, radiusA, centerB, radiusB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the spheres
        /// </summary>
        /// <returns>True if the spheres intersect or one sphere is contained within the other</returns>
        public static bool SphereSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB,
            out IntersectionSphereSphere intersection)
        {
            var fromBtoA = centerA - centerB;
            var distanceFromBtoASqr = fromBtoA.sqrMagnitude;
            if (distanceFromBtoASqr < Geometry.Epsilon)
            {
                if (Mathf.Abs(radiusA - radiusB) < Geometry.Epsilon)
                {
                    // Spheres are coincident
                    intersection = IntersectionSphereSphere.Sphere(centerA, radiusA);
                    return true;
                }

                // One sphere is inside the other
                intersection = IntersectionSphereSphere.None();
                return true;
            }

            // For intersections on the sphere's edge magnitude is more stable than sqrMagnitude
            var distanceFromBtoA = Mathf.Sqrt(distanceFromBtoASqr);

            var sumOfRadii = radiusA + radiusB;
            if (Mathf.Abs(distanceFromBtoA - sumOfRadii) < Geometry.Epsilon)
            {
                // One intersection outside
                intersection = IntersectionSphereSphere.Point(centerB + fromBtoA * (radiusB / sumOfRadii));
                return true;
            }

            if (distanceFromBtoA > sumOfRadii)
            {
                // No intersections, spheres are separate
                intersection = IntersectionSphereSphere.None();
                return false;
            }

            var differenceOfRadii = radiusA - radiusB;
            var differenceOfRadiiAbs = Mathf.Abs(differenceOfRadii);
            if (Mathf.Abs(distanceFromBtoA - differenceOfRadiiAbs) < Geometry.Epsilon)
            {
                // One intersection inside
                intersection = IntersectionSphereSphere.Point(centerB - fromBtoA * (radiusB / differenceOfRadii));
                return true;
            }

            if (distanceFromBtoA < differenceOfRadiiAbs)
            {
                // One sphere is contained within the other
                intersection = IntersectionSphereSphere.None();
                return true;
            }

            // Circle intersection
            var radiusASqr = radiusA * radiusA;
            var distanceToMiddle = 0.5f * (radiusASqr - radiusB * radiusB) / distanceFromBtoASqr + 0.5f;
            var middle = centerA - fromBtoA * distanceToMiddle;

            var discriminant = radiusASqr / distanceFromBtoASqr - distanceToMiddle * distanceToMiddle;
            var radius = distanceFromBtoA * Mathf.Sqrt(discriminant);

            intersection = IntersectionSphereSphere.Circle(middle, -fromBtoA.normalized, radius);
            return true;
        }

        #endregion Sphere-Sphere
    }
}