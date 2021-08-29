using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    ///     Collection of distance calculation algorithms
    /// </summary>
    public static partial class Distance
    {
        #region Point-Line

        /// <summary>
        ///     Returns a distance to the closest point on the line
        /// </summary>
        public static float PointLine(Vector2 point, Line2 line)
        {
            return Vector2.Distance(point, Closest.PointLine(point, line));
        }

        /// <summary>
        ///     Returns a distance to the closest point on the line
        /// </summary>
        public static float PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection)
        {
            return Vector2.Distance(point, Closest.PointLine(point, lineOrigin, lineDirection));
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        ///     Returns a distance to the closest point on the ray
        /// </summary>
        public static float PointRay(Vector2 point, Ray2D ray)
        {
            return Vector2.Distance(point, Closest.PointRay(point, ray));
        }

        /// <summary>
        ///     Returns a distance to the closest point on the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        public static float PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
        {
            return Vector2.Distance(point, Closest.PointRay(point, rayOrigin, rayDirection));
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        ///     Returns a distance to the closest point on the segment
        /// </summary>
        public static float PointSegment(Vector2 point, Segment2 segment)
        {
            return Vector2.Distance(point, Closest.PointSegment(point, segment));
        }

        /// <summary>
        ///     Returns a distance to the closest point on the segment
        /// </summary>
        public static float PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, Closest.PointSegment(point, segmentA, segmentB));
        }

        private static float PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, Vector2 segmentDirection,
            float segmentLength)
        {
            var pointProjection = Vector2.Dot(segmentDirection, point - segmentA);
            if (pointProjection < -Geometry.Epsilon) return Vector2.Distance(point, segmentA);
            if (pointProjection > segmentLength + Geometry.Epsilon) return Vector2.Distance(point, segmentB);
            return Vector2.Distance(point, segmentA + segmentDirection * pointProjection);
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        ///     Returns a distance to the closest point on the circle
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float PointCircle(Vector2 point, Circle2 circle)
        {
            return PointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        ///     Returns a distance to the closest point on the circle
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float PointCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            return (circleCenter - point).magnitude - circleRadius;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        ///     Returns the distance between the closest points on the lines
        /// </summary>
        public static float LineLine(Line2 lineA, Line2 lineB)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the lines
        /// </summary>
        public static float LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            if (Mathf.Abs(VectorE.PerpDot(directionA, directionB)) < Geometry.Epsilon)
            {
                // Parallel
                var originBToA = originA - originB;
                if (Mathf.Abs(VectorE.PerpDot(directionA, originBToA)) > Geometry.Epsilon ||
                    Mathf.Abs(VectorE.PerpDot(directionB, originBToA)) > Geometry.Epsilon)
                {
                    // Not collinear
                    var originBProjection = Vector2.Dot(directionA, originBToA);
                    var distanceSqr = originBToA.sqrMagnitude - originBProjection * originBProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }

                // Collinear
                return 0;
            }

            // Not parallel
            return 0;
        }

        #endregion Line-Line

        #region Line-Ray

        /// <summary>
        ///     Returns the distance between the closest points on the line and the ray
        /// </summary>
        public static float LineRay(Line2 line, Ray2D ray)
        {
            return LineRay(line.origin, line.direction, ray.origin, ray.direction);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the line and the ray
        /// </summary>
        public static float LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection)
        {
            var rayOriginToLineOrigin = lineOrigin - rayOrigin;
            var denominator = VectorE.PerpDot(lineDirection, rayDirection);
            var perpDotA = VectorE.PerpDot(lineDirection, rayOriginToLineOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                var perpDotB = VectorE.PerpDot(rayDirection, rayOriginToLineOrigin);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    var rayOriginProjection = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                    var distanceSqr = rayOriginToLineOrigin.sqrMagnitude - rayOriginProjection * rayOriginProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }

                // Collinear
                return 0;
            }

            // Not parallel
            var rayDistance = perpDotA / denominator;
            if (rayDistance < -Geometry.Epsilon)
            {
                // No intersection
                var rayOriginProjection = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                var linePoint = lineOrigin - lineDirection * rayOriginProjection;
                return Vector2.Distance(linePoint, rayOrigin);
            }

            // Point intersection
            return 0;
        }

        #endregion Line-Ray

        #region Line-Segment

        /// <summary>
        ///     Returns the distance between the closest points on the line and the segment
        /// </summary>
        public static float LineSegment(Line2 line, Segment2 segment)
        {
            return LineSegment(line.origin, line.direction, segment.a, segment.b);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the line and the segment
        /// </summary>
        public static float LineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB)
        {
            var segmentAToOrigin = lineOrigin - segmentA;
            var segmentDirection = segmentB - segmentA;
            var denominator = VectorE.PerpDot(lineDirection, segmentDirection);
            var perpDotA = VectorE.PerpDot(lineDirection, segmentAToOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                // Normalized direction gives more stable results 
                var perpDotB = VectorE.PerpDot(segmentDirection.normalized, segmentAToOrigin);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    var segmentAProjection = Vector2.Dot(lineDirection, segmentAToOrigin);
                    var distanceSqr = segmentAToOrigin.sqrMagnitude - segmentAProjection * segmentAProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }

                // Collinear
                return 0;
            }

            // Not parallel
            var segmentDistance = perpDotA / denominator;
            if (segmentDistance < -Geometry.Epsilon || segmentDistance > 1 + Geometry.Epsilon)
            {
                // No intersection
                var segmentPoint = segmentA + segmentDirection * Mathf.Clamp01(segmentDistance);
                var segmentPointProjection = Vector2.Dot(lineDirection, segmentPoint - lineOrigin);
                var linePoint = lineOrigin + lineDirection * segmentPointProjection;
                return Vector2.Distance(linePoint, segmentPoint);
            }

            // Point intersection
            return 0;
        }

        #endregion Line-Segment

        #region Line-Circle

        /// <summary>
        ///     Returns the distance between the closest points on the line and the circle
        /// </summary>
        public static float LineCircle(Line2 line, Circle2 circle)
        {
            return LineCircle(line.origin, line.direction, circle.center, circle.radius);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the line and the circle
        /// </summary>
        public static float LineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter,
            float circleRadius)
        {
            var originToCenter = circleCenter - lineOrigin;
            var centerProjection = Vector2.Dot(lineDirection, originToCenter);
            var sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection * centerProjection;
            var sqrDistanceToIntersection = circleRadius * circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
                // No intersection
                return Mathf.Sqrt(sqrDistanceToLine) - circleRadius;
            return 0;
        }

        #endregion Line-Circle

        #region Ray-Ray

        /// <summary>
        ///     Returns the distance between the closest points on the rays
        /// </summary>
        public static float RayRay(Ray2D rayA, Ray2D rayB)
        {
            return RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the rays
        /// </summary>
        public static float RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            var originBToA = originA - originB;
            var denominator = VectorE.PerpDot(directionA, directionB);
            var perpDotA = VectorE.PerpDot(directionA, originBToA);
            var perpDotB = VectorE.PerpDot(directionB, originBToA);

            var codirected = Vector2.Dot(directionA, directionB) > 0;
            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                var originBProjection = -Vector2.Dot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    if (!codirected && originBProjection < Geometry.Epsilon) return Vector2.Distance(originA, originB);
                    var distanceSqr = originBToA.sqrMagnitude - originBProjection * originBProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }
                // Collinear

                if (codirected)
                    // Ray intersection
                    return 0;

                if (originBProjection < Geometry.Epsilon)
                    // No intersection
                    return Vector2.Distance(originA, originB);
                return 0;
            }

            // Not parallel
            var distanceA = perpDotB / denominator;
            var distanceB = perpDotA / denominator;
            if (distanceA < -Geometry.Epsilon || distanceB < -Geometry.Epsilon)
            {
                // No intersection
                if (codirected)
                {
                    var originAProjection = Vector2.Dot(directionB, originBToA);
                    if (originAProjection > -Geometry.Epsilon)
                    {
                        var rayPointA = originA;
                        var rayPointB = originB + directionB * originAProjection;
                        return Vector2.Distance(rayPointA, rayPointB);
                    }

                    var originBProjection = -Vector2.Dot(directionA, originBToA);
                    if (originBProjection > -Geometry.Epsilon)
                    {
                        var rayPointA = originA + directionA * originBProjection;
                        var rayPointB = originB;
                        return Vector2.Distance(rayPointA, rayPointB);
                    }

                    return Vector2.Distance(originA, originB);
                }

                if (distanceA > -Geometry.Epsilon)
                {
                    var originBProjection = -Vector2.Dot(directionA, originBToA);
                    if (originBProjection > -Geometry.Epsilon)
                    {
                        var rayPointA = originA + directionA * originBProjection;
                        var rayPointB = originB;
                        return Vector2.Distance(rayPointA, rayPointB);
                    }
                }
                else if (distanceB > -Geometry.Epsilon)
                {
                    var originAProjection = Vector2.Dot(directionB, originBToA);
                    if (originAProjection > -Geometry.Epsilon)
                    {
                        var rayPointA = originA;
                        var rayPointB = originB + directionB * originAProjection;
                        return Vector2.Distance(rayPointA, rayPointB);
                    }
                }

                return Vector2.Distance(originA, originB);
            }

            // Point intersection
            return 0;
        }

        #endregion Ray-Ray

        #region Ray-Segment

        /// <summary>
        ///     Returns the distance between the closest points on the ray and the segment
        /// </summary>
        public static float RaySegment(Ray2D ray, Segment2 segment)
        {
            return RaySegment(ray.origin, ray.direction, segment.a, segment.b);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the ray and the segment
        /// </summary>
        public static float RaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB)
        {
            var segmentAToOrigin = rayOrigin - segmentA;
            var segmentDirection = segmentB - segmentA;
            var denominator = VectorE.PerpDot(rayDirection, segmentDirection);
            var perpDotA = VectorE.PerpDot(rayDirection, segmentAToOrigin);
            // Normalized direction gives more stable results 
            var perpDotB = VectorE.PerpDot(segmentDirection.normalized, segmentAToOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                var segmentAProjection = -Vector2.Dot(rayDirection, segmentAToOrigin);
                var originToSegmentB = segmentB - rayOrigin;
                var segmentBProjection = Vector2.Dot(rayDirection, originToSegmentB);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    if (segmentAProjection > -Geometry.Epsilon)
                    {
                        var distanceSqr = segmentAToOrigin.sqrMagnitude - segmentAProjection * segmentAProjection;
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                    }

                    if (segmentBProjection > -Geometry.Epsilon)
                    {
                        var distanceSqr = originToSegmentB.sqrMagnitude - segmentBProjection * segmentBProjection;
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                    }

                    if (segmentAProjection > segmentBProjection) return Vector2.Distance(rayOrigin, segmentA);
                    return Vector2.Distance(rayOrigin, segmentB);
                }

                // Collinear
                if (segmentAProjection > -Geometry.Epsilon || segmentBProjection > -Geometry.Epsilon)
                    // Point or segment intersection
                    return 0;
                // No intersection
                return segmentAProjection > segmentBProjection ? -segmentAProjection : -segmentBProjection;
            }

            // Not parallel
            var rayDistance = perpDotB / denominator;
            var segmentDistance = perpDotA / denominator;
            if (rayDistance < -Geometry.Epsilon ||
                segmentDistance < -Geometry.Epsilon || segmentDistance > 1 + Geometry.Epsilon)
            {
                // No intersection
                var codirected = Vector2.Dot(rayDirection, segmentDirection) > 0;
                Vector2 segmentBToOrigin;
                if (!codirected)
                {
                    PTUtils.Swap(ref segmentA, ref segmentB);
                    segmentDirection = -segmentDirection;
                    segmentBToOrigin = segmentAToOrigin;
                    segmentAToOrigin = rayOrigin - segmentA;
                    segmentDistance = 1 - segmentDistance;
                }
                else
                {
                    segmentBToOrigin = rayOrigin - segmentB;
                }

                var segmentAProjection = -Vector2.Dot(rayDirection, segmentAToOrigin);
                var segmentBProjection = -Vector2.Dot(rayDirection, segmentBToOrigin);
                var segmentAOnRay = segmentAProjection > -Geometry.Epsilon;
                var segmentBOnRay = segmentBProjection > -Geometry.Epsilon;
                if (segmentAOnRay && segmentBOnRay)
                {
                    if (segmentDistance < 0)
                    {
                        var rayPoint = rayOrigin + rayDirection * segmentAProjection;
                        var segmentPoint = segmentA;
                        return Vector2.Distance(rayPoint, segmentPoint);
                    }
                    else
                    {
                        var rayPoint = rayOrigin + rayDirection * segmentBProjection;
                        var segmentPoint = segmentB;
                        return Vector2.Distance(rayPoint, segmentPoint);
                    }
                }

                if (!segmentAOnRay && segmentBOnRay)
                {
                    if (segmentDistance < 0)
                    {
                        var rayPoint = rayOrigin;
                        var segmentPoint = segmentA;
                        return Vector2.Distance(rayPoint, segmentPoint);
                    }

                    if (segmentDistance > 1 + Geometry.Epsilon)
                    {
                        var rayPoint = rayOrigin + rayDirection * segmentBProjection;
                        var segmentPoint = segmentB;
                        return Vector2.Distance(rayPoint, segmentPoint);
                    }
                    else
                    {
                        var rayPoint = rayOrigin;
                        var originProjection = Vector2.Dot(segmentDirection, segmentAToOrigin);
                        var segmentPoint =
                            segmentA + segmentDirection * originProjection / segmentDirection.sqrMagnitude;
                        return Vector2.Distance(rayPoint, segmentPoint);
                    }
                }

                {
                    // Not on ray
                    var rayPoint = rayOrigin;
                    var originProjection = Vector2.Dot(segmentDirection, segmentAToOrigin);
                    var sqrSegmentLength = segmentDirection.sqrMagnitude;
                    if (originProjection < 0) return Vector2.Distance(rayPoint, segmentA);

                    if (originProjection > sqrSegmentLength)
                    {
                        return Vector2.Distance(rayPoint, segmentB);
                    }

                    var segmentPoint = segmentA + segmentDirection * originProjection / sqrSegmentLength;
                    return Vector2.Distance(rayPoint, segmentPoint);
                }
            }

            // Point intersection
            return 0;
        }

        #endregion Ray-Segment

        #region Ray-Circle

        /// <summary>
        ///     Returns the distance between the closest points on the ray and the circle
        /// </summary>
        public static float RayCircle(Ray2D ray, Circle2 circle)
        {
            return RayCircle(ray.origin, ray.direction, circle.center, circle.radius);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the ray and the circle
        /// </summary>
        public static float RayCircle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleCenter, float circleRadius)
        {
            var originToCenter = circleCenter - rayOrigin;
            var centerProjection = Vector2.Dot(rayDirection, originToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon)
                // No intersection
                return Mathf.Sqrt(originToCenter.sqrMagnitude) - circleRadius;

            var sqrDistanceToOrigin = originToCenter.sqrMagnitude;
            var sqrDistanceToLine = sqrDistanceToOrigin - centerProjection * centerProjection;
            var sqrDistanceToIntersection = circleRadius * circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon) return Mathf.Sqrt(sqrDistanceToOrigin) - circleRadius;
                return Mathf.Sqrt(sqrDistanceToLine) - circleRadius;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                    // No intersection
                    return Mathf.Sqrt(sqrDistanceToOrigin) - circleRadius;
                // Point intersection
                return 0;
            }

            // Line intersection
            var distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            var distanceA = centerProjection - distanceToIntersection;
            var distanceB = centerProjection + distanceToIntersection;

            if (distanceA < -Geometry.Epsilon)
            {
                if (distanceB < -Geometry.Epsilon)
                    // No intersection
                    return Mathf.Sqrt(sqrDistanceToOrigin) - circleRadius;

                // Point intersection;
                return 0;
            }

            // Two points intersection;
            return 0;
        }

        #endregion Ray-Circle

        #region Segment-Segment

        /// <summary>
        ///     Returns the distance between the closest points on the segments
        /// </summary>
        public static float SegmentSegment(Segment2 segment1, Segment2 segment2)
        {
            return SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the segments
        /// </summary>
        public static float SegmentSegment(Vector2 segment1A, Vector2 segment1B, Vector2 segment2A, Vector2 segment2B)
        {
            var from2ATo1A = segment1A - segment2A;
            var direction1 = segment1B - segment1A;
            var direction2 = segment2B - segment2A;
            var segment1Length = direction1.magnitude;
            var segment2Length = direction2.magnitude;

            var segment1IsAPoint = segment1Length < Geometry.Epsilon;
            var segment2IsAPoint = segment2Length < Geometry.Epsilon;
            if (segment1IsAPoint && segment2IsAPoint) return Vector2.Distance(segment1A, segment2A);
            if (segment1IsAPoint)
            {
                direction2.Normalize();
                return PointSegment(segment1A, segment2A, segment2B, direction2, segment2Length);
            }

            if (segment2IsAPoint)
            {
                direction1.Normalize();
                return PointSegment(segment2A, segment1A, segment1B, direction1, segment1Length);
            }

            direction1.Normalize();
            direction2.Normalize();
            var denominator = VectorE.PerpDot(direction1, direction2);
            var perpDot1 = VectorE.PerpDot(direction1, from2ATo1A);
            var perpDot2 = VectorE.PerpDot(direction2, from2ATo1A);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDot1) > Geometry.Epsilon || Mathf.Abs(perpDot2) > Geometry.Epsilon)
                {
                    // Not collinear
                    var segment2AProjection = -Vector2.Dot(direction1, from2ATo1A);
                    if (segment2AProjection > -Geometry.Epsilon &&
                        segment2AProjection < segment1Length + Geometry.Epsilon)
                    {
                        var distanceSqr = from2ATo1A.sqrMagnitude - segment2AProjection * segment2AProjection;
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                    }

                    var from1ATo2B = segment2B - segment1A;
                    var segment2BProjection = Vector2.Dot(direction1, from1ATo2B);
                    if (segment2BProjection > -Geometry.Epsilon &&
                        segment2BProjection < segment1Length + Geometry.Epsilon)
                    {
                        var distanceSqr = from1ATo2B.sqrMagnitude - segment2BProjection * segment2BProjection;
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                    }

                    if (segment2AProjection < 0 && segment2BProjection < 0)
                    {
                        if (segment2AProjection > segment2BProjection) return Vector2.Distance(segment1A, segment2A);
                        return Vector2.Distance(segment1A, segment2B);
                    }

                    if (segment2AProjection > 0 && segment2BProjection > 0)
                    {
                        if (segment2AProjection < segment2BProjection) return Vector2.Distance(segment1B, segment2A);
                        return Vector2.Distance(segment1B, segment2B);
                    }

                    var segment1AProjection = Vector2.Dot(direction2, from2ATo1A);
                    var segment2Point = segment2A + direction2 * segment1AProjection;
                    return Vector2.Distance(segment1A, segment2Point);
                }
                // Collinear

                var codirected = Vector2.Dot(direction1, direction2) > 0;
                if (codirected)
                {
                    // Codirected
                    var segment2AProjection = -Vector2.Dot(direction1, from2ATo1A);
                    if (segment2AProjection > -Geometry.Epsilon)
                        // 1A------1B
                        //     2A------2B
                        return SegmentSegmentCollinear(segment1A, segment1B, segment2A);
                    return SegmentSegmentCollinear(segment2A, segment2B, segment1A);
                }

                {
                    // Contradirected
                    var segment2BProjection = Vector2.Dot(direction1, segment2B - segment1A);
                    if (segment2BProjection > -Geometry.Epsilon)
                        // 1A------1B
                        //     2B------2A
                        return SegmentSegmentCollinear(segment1A, segment1B, segment2B);
                    return SegmentSegmentCollinear(segment2B, segment2A, segment1A);
                }
            }

            // Not parallel
            var distance1 = perpDot2 / denominator;
            var distance2 = perpDot1 / denominator;
            if (distance1 < -Geometry.Epsilon || distance1 > segment1Length + Geometry.Epsilon ||
                distance2 < -Geometry.Epsilon || distance2 > segment2Length + Geometry.Epsilon)
            {
                // No intersection
                var codirected = Vector2.Dot(direction1, direction2) > 0;
                Vector2 from1ATo2B;
                if (!codirected)
                {
                    PTUtils.Swap(ref segment2A, ref segment2B);
                    direction2 = -direction2;
                    from1ATo2B = -from2ATo1A;
                    from2ATo1A = segment1A - segment2A;
                    distance2 = segment2Length - distance2;
                }
                else
                {
                    from1ATo2B = segment2B - segment1A;
                }

                Vector2 segment1Point;
                Vector2 segment2Point;

                var segment2AProjection = -Vector2.Dot(direction1, from2ATo1A);
                var segment2BProjection = Vector2.Dot(direction1, from1ATo2B);

                var segment2AIsAfter1A = segment2AProjection > -Geometry.Epsilon;
                var segment2BIsBefore1B = segment2BProjection < segment1Length + Geometry.Epsilon;
                var segment2AOnSegment1 = segment2AIsAfter1A && segment2AProjection < segment1Length + Geometry.Epsilon;
                var segment2BOnSegment1 = segment2BProjection > -Geometry.Epsilon && segment2BIsBefore1B;
                if (segment2AOnSegment1 && segment2BOnSegment1)
                {
                    if (distance2 < -Geometry.Epsilon)
                    {
                        segment1Point = segment1A + direction1 * segment2AProjection;
                        segment2Point = segment2A;
                    }
                    else
                    {
                        segment1Point = segment1A + direction1 * segment2BProjection;
                        segment2Point = segment2B;
                    }
                }
                else if (!segment2AOnSegment1 && !segment2BOnSegment1)
                {
                    if (!segment2AIsAfter1A && !segment2BIsBefore1B)
                        segment1Point = distance1 < -Geometry.Epsilon ? segment1A : segment1B;
                    else
                        // Not on segment
                        segment1Point = segment2AIsAfter1A ? segment1B : segment1A;
                    var segment1PointProjection = Vector2.Dot(direction2, segment1Point - segment2A);
                    segment1PointProjection = Mathf.Clamp(segment1PointProjection, 0, segment2Length);
                    segment2Point = segment2A + direction2 * segment1PointProjection;
                }
                else if (segment2AOnSegment1)
                {
                    if (distance2 < -Geometry.Epsilon)
                    {
                        segment1Point = segment1A + direction1 * segment2AProjection;
                        segment2Point = segment2A;
                    }
                    else
                    {
                        segment1Point = segment1B;
                        var segment1PointProjection = Vector2.Dot(direction2, segment1Point - segment2A);
                        segment1PointProjection = Mathf.Clamp(segment1PointProjection, 0, segment2Length);
                        segment2Point = segment2A + direction2 * segment1PointProjection;
                    }
                }
                else
                {
                    if (distance2 > segment2Length + Geometry.Epsilon)
                    {
                        segment1Point = segment1A + direction1 * segment2BProjection;
                        segment2Point = segment2B;
                    }
                    else
                    {
                        segment1Point = segment1A;
                        var segment1PointProjection = Vector2.Dot(direction2, segment1Point - segment2A);
                        segment1PointProjection = Mathf.Clamp(segment1PointProjection, 0, segment2Length);
                        segment2Point = segment2A + direction2 * segment1PointProjection;
                    }
                }

                return Vector2.Distance(segment1Point, segment2Point);
            }

            // Point intersection
            return 0;
        }

        private static float SegmentSegmentCollinear(Vector2 leftA, Vector2 leftB, Vector2 rightA)
        {
            var leftDirection = leftB - leftA;
            var rightAProjection = Vector2.Dot(leftDirection.normalized, rightA - leftB);
            if (Mathf.Abs(rightAProjection) < Geometry.Epsilon)
                // LB == RA
                // LA------LB
                //         RA------RB

                // Point intersection
                return 0;
            if (rightAProjection < 0)
                // LB > RA
                // LA------LB
                //     RARB
                //     RA--RB
                //     RA------RB

                // Segment intersection
                return 0;
            // LB < RA
            // LA------LB
            //             RA------RB

            // No intersection
            return rightAProjection;
        }

        #endregion Segment-Segment

        #region Segment-Circle

        /// <summary>
        ///     Returns the distance between the closest points on the segment and the circle
        /// </summary>
        public static float SegmentCircle(Segment2 segment, Circle2 circle)
        {
            return SegmentCircle(segment.a, segment.b, circle.center, circle.radius);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the segment and the circle
        /// </summary>
        public static float SegmentCircle(Vector2 segmentA, Vector2 segmentB, Vector2 circleCenter, float circleRadius)
        {
            var segmentAToCenter = circleCenter - segmentA;
            var fromAtoB = segmentB - segmentA;
            var segmentLength = fromAtoB.magnitude;
            if (segmentLength < Geometry.Epsilon) return segmentAToCenter.magnitude - circleRadius;

            var segmentDirection = fromAtoB.normalized;
            var centerProjection = Vector2.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon ||
                centerProjection - circleRadius > segmentLength + Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < 0) return segmentAToCenter.magnitude - circleRadius;
                return (circleCenter - segmentB).magnitude - circleRadius;
            }

            var sqrDistanceToA = segmentAToCenter.sqrMagnitude;
            var sqrDistanceToLine = sqrDistanceToA - centerProjection * centerProjection;
            var sqrDistanceToIntersection = circleRadius * circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon) return Mathf.Sqrt(sqrDistanceToA) - circleRadius;
                if (centerProjection > segmentLength + Geometry.Epsilon)
                    return (circleCenter - segmentB).magnitude - circleRadius;
                return Mathf.Sqrt(sqrDistanceToLine) - circleRadius;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                    // No intersection
                    return Mathf.Sqrt(sqrDistanceToA) - circleRadius;
                if (centerProjection > segmentLength + Geometry.Epsilon)
                    // No intersection
                    return (circleCenter - segmentB).magnitude - circleRadius;
                // Point intersection
                return 0;
            }

            // Line intersection
            var distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            var distanceA = centerProjection - distanceToIntersection;
            var distanceB = centerProjection + distanceToIntersection;

            var pointAIsAfterSegmentA = distanceA > -Geometry.Epsilon;
            var pointBIsBeforeSegmentB = distanceB < segmentLength + Geometry.Epsilon;

            if (pointAIsAfterSegmentA && pointBIsBeforeSegmentB)
                // Two points intersection
                return 0;
            if (!pointAIsAfterSegmentA && !pointBIsBeforeSegmentB)
            {
                // The segment is inside, but no intersection
                distanceB = -(distanceB - segmentLength);
                return distanceA > distanceB ? distanceA : distanceB;
            }

            var pointAIsBeforeSegmentB = distanceA < segmentLength + Geometry.Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
                // Point A intersection
                return 0;
            var pointBIsAfterSegmentA = distanceB > -Geometry.Epsilon;
            if (pointBIsAfterSegmentA && pointBIsBeforeSegmentB)
                // Point B intersection
                return 0;

            // No intersection
            if (centerProjection < 0) return Mathf.Sqrt(sqrDistanceToA) - circleRadius;
            return (circleCenter - segmentB).magnitude - circleRadius;
        }

        #endregion Segment-Circle

        #region Circle-Circle

        /// <summary>
        ///     Returns the distance between the closest points on the circles
        /// </summary>
        /// <returns>
        ///     Positive value if the circles do not intersect, negative otherwise.
        ///     Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float CircleCircle(Circle2 circleA, Circle2 circleB)
        {
            return CircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius);
        }

        /// <summary>
        ///     Returns the distance between the closest points on the circles
        /// </summary>
        /// <returns>
        ///     Positive value if the circles do not intersect, negative otherwise.
        ///     Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            return Vector2.Distance(centerA, centerB) - radiusA - radiusB;
        }

        #endregion Circle-Circle
    }
}