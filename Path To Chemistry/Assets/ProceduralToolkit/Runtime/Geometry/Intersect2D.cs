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
        public static bool PointLine(Vector2 point, Line2 line)
        {
            return PointLine(point, line.origin, line.direction);
        }

        /// <summary>
        ///     Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        ///     -1 if the point is to the left of the line,
        ///     0 if it is on the line,
        ///     1 if it is to the right of the line
        /// </param>
        public static bool PointLine(Vector2 point, Line2 line, out int side)
        {
            return PointLine(point, line.origin, line.direction, out side);
        }

        /// <summary>
        ///     Tests if the point lies on the line
        /// </summary>
        public static bool PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection)
        {
            var perpDot = VectorE.PerpDot(point - lineOrigin, lineDirection);
            return -Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon;
        }

        /// <summary>
        ///     Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        ///     -1 if the point is to the left of the line,
        ///     0 if it is on the line,
        ///     1 if it is to the right of the line
        /// </param>
        public static bool PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection, out int side)
        {
            var perpDot = VectorE.PerpDot(point - lineOrigin, lineDirection);
            if (perpDot < -Geometry.Epsilon)
            {
                side = -1;
                return false;
            }

            if (perpDot > Geometry.Epsilon)
            {
                side = 1;
                return false;
            }

            side = 0;
            return true;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        ///     Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector2 point, Ray2D ray)
        {
            return PointRay(point, ray.origin, ray.direction);
        }

        /// <summary>
        ///     Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        ///     -1 if the point is to the left of the ray,
        ///     0 if it is on the line,
        ///     1 if it is to the right of the ray
        /// </param>
        public static bool PointRay(Vector2 point, Ray2D ray, out int side)
        {
            return PointRay(point, ray.origin, ray.direction, out side);
        }

        /// <summary>
        ///     Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
        {
            var toPoint = point - rayOrigin;
            var perpDot = VectorE.PerpDot(toPoint, rayDirection);
            return -Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon &&
                   Vector2.Dot(rayDirection, toPoint) > -Geometry.Epsilon;
        }

        /// <summary>
        ///     Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        ///     -1 if the point is to the left of the ray,
        ///     0 if it is on the line,
        ///     1 if it is to the right of the ray
        /// </param>
        public static bool PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection, out int side)
        {
            var toPoint = point - rayOrigin;
            var perpDot = VectorE.PerpDot(toPoint, rayDirection);
            if (perpDot < -Geometry.Epsilon)
            {
                side = -1;
                return false;
            }

            if (perpDot > Geometry.Epsilon)
            {
                side = 1;
                return false;
            }

            side = 0;
            return Vector2.Dot(rayDirection, toPoint) > -Geometry.Epsilon;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        ///     Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector2 point, Segment2 segment)
        {
            return PointSegment(point, segment.a, segment.b);
        }

        /// <summary>
        ///     Tests if the point lies on the segment
        /// </summary>
        /// <param name="side">
        ///     -1 if the point is to the left of the segment,
        ///     0 if it is on the line,
        ///     1 if it is to the right of the segment
        /// </param>
        public static bool PointSegment(Vector2 point, Segment2 segment, out int side)
        {
            return PointSegment(point, segment.a, segment.b, out side);
        }

        /// <summary>
        ///     Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            var fromAToB = segmentB - segmentA;
            var sqrSegmentLength = fromAToB.sqrMagnitude;
            if (sqrSegmentLength < Geometry.Epsilon)
                // The segment is a point
                return point == segmentA;
            // Normalized direction gives more stable results
            var segmentDirection = fromAToB.normalized;
            var toPoint = point - segmentA;
            var perpDot = VectorE.PerpDot(toPoint, segmentDirection);
            if (-Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon)
            {
                var pointProjection = Vector2.Dot(segmentDirection, toPoint);
                return pointProjection > -Geometry.Epsilon &&
                       pointProjection < Mathf.Sqrt(sqrSegmentLength) + Geometry.Epsilon;
            }

            return false;
        }

        /// <summary>
        ///     Tests if the point lies on the segment
        /// </summary>
        /// <param name="side">
        ///     -1 if the point is to the left of the segment,
        ///     0 if it is on the line,
        ///     1 if it is to the right of the segment
        /// </param>
        public static bool PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, out int side)
        {
            var fromAToB = segmentB - segmentA;
            var sqrSegmentLength = fromAToB.sqrMagnitude;
            if (sqrSegmentLength < Geometry.Epsilon)
            {
                // The segment is a point
                side = 0;
                return point == segmentA;
            }

            // Normalized direction gives more stable results
            var segmentDirection = fromAToB.normalized;
            var toPoint = point - segmentA;
            var perpDot = VectorE.PerpDot(toPoint, segmentDirection);
            if (perpDot < -Geometry.Epsilon)
            {
                side = -1;
                return false;
            }

            if (perpDot > Geometry.Epsilon)
            {
                side = 1;
                return false;
            }

            side = 0;
            var pointProjection = Vector2.Dot(segmentDirection, toPoint);
            return pointProjection > -Geometry.Epsilon &&
                   pointProjection < Mathf.Sqrt(sqrSegmentLength) + Geometry.Epsilon;
        }

        private static bool PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentDirection,
            float sqrSegmentLength)
        {
            var segmentLength = Mathf.Sqrt(sqrSegmentLength);
            segmentDirection /= segmentLength;
            var toPoint = point - segmentA;
            var perpDot = VectorE.PerpDot(toPoint, segmentDirection);
            if (-Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon)
            {
                var pointProjection = Vector2.Dot(segmentDirection, toPoint);
                return pointProjection > -Geometry.Epsilon &&
                       pointProjection < segmentLength + Geometry.Epsilon;
            }

            return false;
        }

        public static bool PointSegmentCollinear(Vector2 segmentA, Vector2 segmentB, Vector2 point)
        {
            if (Mathf.Abs(segmentA.x - segmentB.x) < Geometry.Epsilon)
            {
                // Vertical
                if (segmentA.y <= point.y && point.y <= segmentB.y) return true;
                if (segmentA.y >= point.y && point.y >= segmentB.y) return true;
            }
            else
            {
                // Not vertical
                if (segmentA.x <= point.x && point.x <= segmentB.x) return true;
                if (segmentA.x >= point.x && point.x >= segmentB.x) return true;
            }

            return false;
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        ///     Tests if the point is inside the circle
        /// </summary>
        public static bool PointCircle(Vector2 point, Circle2 circle)
        {
            return PointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        ///     Tests if the point is inside the circle
        /// </summary>
        public static bool PointCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            // For points on the circle's edge magnitude is more stable than sqrMagnitude
            return (point - circleCenter).magnitude < circleRadius + Geometry.Epsilon;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line2 lineA, Line2 lineB)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line2 lineA, Line2 lineB, out IntersectionLineLine2 intersection)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            return LineLine(originA, directionA, originB, directionB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out IntersectionLineLine2 intersection)
        {
            var originBToA = originA - originB;
            var denominator = VectorE.PerpDot(directionA, directionB);
            var perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                var perpDotA = VectorE.PerpDot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionLineLine2.None();
                    return false;
                }

                // Collinear
                intersection = IntersectionLineLine2.Line(originA);
                return true;
            }

            // Not parallel
            intersection = IntersectionLineLine2.Point(originA + directionA * (perpDotB / denominator));
            return true;
        }

        #endregion Line-Line

        #region Line-Ray

        /// <summary>
        ///     Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Line2 line, Ray2D ray)
        {
            return LineRay(line.origin, line.direction, ray.origin, ray.direction, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Line2 line, Ray2D ray, out IntersectionLineRay2 intersection)
        {
            return LineRay(line.origin, line.direction, ray.origin, ray.direction, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection)
        {
            return LineRay(lineOrigin, lineDirection, rayOrigin, rayDirection, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection,
            out IntersectionLineRay2 intersection)
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
                    intersection = IntersectionLineRay2.None();
                    return false;
                }

                // Collinear
                intersection = IntersectionLineRay2.Ray(rayOrigin);
                return true;
            }

            // Not parallel
            var rayDistance = perpDotA / denominator;
            if (rayDistance > -Geometry.Epsilon)
            {
                intersection = IntersectionLineRay2.Point(rayOrigin + rayDirection * rayDistance);
                return true;
            }

            intersection = IntersectionLineRay2.None();
            return false;
        }

        #endregion Line-Ray

        #region Line-Segment

        /// <summary>
        ///     Computes an intersection of the line and the segment
        /// </summary>
        public static bool LineSegment(Line2 line, Segment2 segment)
        {
            return LineSegment(line.origin, line.direction, segment.a, segment.b, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the segment
        /// </summary>
        public static bool LineSegment(Line2 line, Segment2 segment, out IntersectionLineSegment2 intersection)
        {
            return LineSegment(line.origin, line.direction, segment.a, segment.b, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the segment
        /// </summary>
        public static bool LineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB)
        {
            return LineSegment(lineOrigin, lineDirection, segmentA, segmentB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the segment
        /// </summary>
        public static bool LineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB,
            out IntersectionLineSegment2 intersection)
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
                    intersection = IntersectionLineSegment2.None();
                    return false;
                }

                // Collinear
                var segmentIsAPoint = segmentDirection.sqrMagnitude < Geometry.Epsilon;
                if (segmentIsAPoint)
                {
                    intersection = IntersectionLineSegment2.Point(segmentA);
                    return true;
                }

                var codirected = Vector2.Dot(lineDirection, segmentDirection) > 0;
                if (codirected)
                    intersection = IntersectionLineSegment2.Segment(segmentA, segmentB);
                else
                    intersection = IntersectionLineSegment2.Segment(segmentB, segmentA);
                return true;
            }

            // Not parallel
            var segmentDistance = perpDotA / denominator;
            if (segmentDistance > -Geometry.Epsilon && segmentDistance < 1 + Geometry.Epsilon)
            {
                intersection = IntersectionLineSegment2.Point(segmentA + segmentDirection * segmentDistance);
                return true;
            }

            intersection = IntersectionLineSegment2.None();
            return false;
        }

        #endregion Line-Segment

        #region Line-Circle

        /// <summary>
        ///     Computes an intersection of the line and the circle
        /// </summary>
        public static bool LineCircle(Line2 line, Circle2 circle)
        {
            return LineCircle(line.origin, line.direction, circle.center, circle.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the circle
        /// </summary>
        public static bool LineCircle(Line2 line, Circle2 circle, out IntersectionLineCircle intersection)
        {
            return LineCircle(line.origin, line.direction, circle.center, circle.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the circle
        /// </summary>
        public static bool LineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter,
            float circleRadius)
        {
            return LineCircle(lineOrigin, lineDirection, circleCenter, circleRadius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the line and the circle
        /// </summary>
        public static bool LineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter,
            float circleRadius,
            out IntersectionLineCircle intersection)
        {
            var originToCenter = circleCenter - lineOrigin;
            var centerProjection = Vector2.Dot(lineDirection, originToCenter);
            var sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection * centerProjection;

            var sqrDistanceToIntersection = circleRadius * circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionLineCircle.None();
                return false;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                intersection = IntersectionLineCircle.Point(lineOrigin + lineDirection * centerProjection);
                return true;
            }

            var distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            var distanceA = centerProjection - distanceToIntersection;
            var distanceB = centerProjection + distanceToIntersection;

            var pointA = lineOrigin + lineDirection * distanceA;
            var pointB = lineOrigin + lineDirection * distanceB;
            intersection = IntersectionLineCircle.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Line-Circle

        #region Ray-Ray

        /// <summary>
        ///     Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Ray2D rayA, Ray2D rayB)
        {
            return RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Ray2D rayA, Ray2D rayB, out IntersectionRayRay2 intersection)
        {
            return RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            return RayRay(originA, directionA, originB, directionB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out IntersectionRayRay2 intersection)
        {
            var originBToA = originA - originB;
            var denominator = VectorE.PerpDot(directionA, directionB);
            var perpDotA = VectorE.PerpDot(directionA, originBToA);
            var perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionRayRay2.None();
                    return false;
                }
                // Collinear

                var codirected = Vector2.Dot(directionA, directionB) > 0;
                var originBProjection = -Vector2.Dot(directionA, originBToA);
                if (codirected)
                {
                    intersection = IntersectionRayRay2.Ray(originBProjection > 0 ? originB : originA, directionA);
                    return true;
                }

                if (originBProjection < -Geometry.Epsilon)
                {
                    intersection = IntersectionRayRay2.None();
                    return false;
                }

                if (originBProjection < Geometry.Epsilon)
                {
                    intersection = IntersectionRayRay2.Point(originA);
                    return true;
                }

                intersection = IntersectionRayRay2.Segment(originA, originB);
                return true;
            }

            // Not parallel
            var distanceA = perpDotB / denominator;
            if (distanceA < -Geometry.Epsilon)
            {
                intersection = IntersectionRayRay2.None();
                return false;
            }

            var distanceB = perpDotA / denominator;
            if (distanceB < -Geometry.Epsilon)
            {
                intersection = IntersectionRayRay2.None();
                return false;
            }

            intersection = IntersectionRayRay2.Point(originA + directionA * distanceA);
            return true;
        }

        #endregion Ray-Ray

        #region Ray-Segment

        /// <summary>
        ///     Computes an intersection of the ray and the segment
        /// </summary>
        public static bool RaySegment(Ray2D ray, Segment2 segment)
        {
            return RaySegment(ray.origin, ray.direction, segment.a, segment.b, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the segment
        /// </summary>
        public static bool RaySegment(Ray2D ray, Segment2 segment, out IntersectionRaySegment2 intersection)
        {
            return RaySegment(ray.origin, ray.direction, segment.a, segment.b, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the segment
        /// </summary>
        public static bool RaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB)
        {
            return RaySegment(rayOrigin, rayDirection, segmentA, segmentB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the segment
        /// </summary>
        public static bool RaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB,
            out IntersectionRaySegment2 intersection)
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
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionRaySegment2.None();
                    return false;
                }
                // Collinear

                var segmentIsAPoint = segmentDirection.sqrMagnitude < Geometry.Epsilon;
                var segmentAProjection = Vector2.Dot(rayDirection, segmentA - rayOrigin);
                if (segmentIsAPoint)
                {
                    if (segmentAProjection > -Geometry.Epsilon)
                    {
                        intersection = IntersectionRaySegment2.Point(segmentA);
                        return true;
                    }

                    intersection = IntersectionRaySegment2.None();
                    return false;
                }

                var segmentBProjection = Vector2.Dot(rayDirection, segmentB - rayOrigin);
                if (segmentAProjection > -Geometry.Epsilon)
                {
                    if (segmentBProjection > -Geometry.Epsilon)
                    {
                        if (segmentBProjection > segmentAProjection)
                            intersection = IntersectionRaySegment2.Segment(segmentA, segmentB);
                        else
                            intersection = IntersectionRaySegment2.Segment(segmentB, segmentA);
                    }
                    else
                    {
                        if (segmentAProjection > Geometry.Epsilon)
                            intersection = IntersectionRaySegment2.Segment(rayOrigin, segmentA);
                        else
                            intersection = IntersectionRaySegment2.Point(rayOrigin);
                    }

                    return true;
                }

                if (segmentBProjection > -Geometry.Epsilon)
                {
                    if (segmentBProjection > Geometry.Epsilon)
                        intersection = IntersectionRaySegment2.Segment(rayOrigin, segmentB);
                    else
                        intersection = IntersectionRaySegment2.Point(rayOrigin);
                    return true;
                }

                intersection = IntersectionRaySegment2.None();
                return false;
            }

            // Not parallel
            var rayDistance = perpDotB / denominator;
            var segmentDistance = perpDotA / denominator;
            if (rayDistance > -Geometry.Epsilon &&
                segmentDistance > -Geometry.Epsilon && segmentDistance < 1 + Geometry.Epsilon)
            {
                intersection = IntersectionRaySegment2.Point(segmentA + segmentDirection * segmentDistance);
                return true;
            }

            intersection = IntersectionRaySegment2.None();
            return false;
        }

        #endregion Ray-Segment

        #region Ray-Circle

        /// <summary>
        ///     Computes an intersection of the ray and the circle
        /// </summary>
        public static bool RayCircle(Ray2D ray, Circle2 circle)
        {
            return RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the circle
        /// </summary>
        public static bool RayCircle(Ray2D ray, Circle2 circle, out IntersectionRayCircle intersection)
        {
            return RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the circle
        /// </summary>
        public static bool RayCircle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleCenter, float circleRadius)
        {
            return RayCircle(rayOrigin, rayDirection, circleCenter, circleRadius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the ray and the circle
        /// </summary>
        public static bool RayCircle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleCenter, float circleRadius,
            out IntersectionRayCircle intersection)
        {
            var originToCenter = circleCenter - rayOrigin;
            var centerProjection = Vector2.Dot(rayDirection, originToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon)
            {
                intersection = IntersectionRayCircle.None();
                return false;
            }

            var sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection * centerProjection;
            var sqrDistanceToIntersection = circleRadius * circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionRayCircle.None();
                return false;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    intersection = IntersectionRayCircle.None();
                    return false;
                }

                intersection = IntersectionRayCircle.Point(rayOrigin + rayDirection * centerProjection);
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
                    intersection = IntersectionRayCircle.None();
                    return false;
                }

                intersection = IntersectionRayCircle.Point(rayOrigin + rayDirection * distanceB);
                return true;
            }

            var pointA = rayOrigin + rayDirection * distanceA;
            var pointB = rayOrigin + rayDirection * distanceB;
            intersection = IntersectionRayCircle.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Ray-Circle

        #region Segment-Segment

        /// <summary>
        ///     Computes an intersection of the segments
        /// </summary>
        public static bool SegmentSegment(Segment2 segment1, Segment2 segment2)
        {
            return SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segments
        /// </summary>
        public static bool SegmentSegment(Segment2 segment1, Segment2 segment2,
            out IntersectionSegmentSegment2 intersection)
        {
            return SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segments
        /// </summary>
        public static bool SegmentSegment(Vector2 segment1A, Vector2 segment1B, Vector2 segment2A, Vector2 segment2B)
        {
            return SegmentSegment(segment1A, segment1B, segment2A, segment2B, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segments
        /// </summary>
        public static bool SegmentSegment(Vector2 segment1A, Vector2 segment1B, Vector2 segment2A, Vector2 segment2B,
            out IntersectionSegmentSegment2 intersection)
        {
            var from2ATo1A = segment1A - segment2A;
            var direction1 = segment1B - segment1A;
            var direction2 = segment2B - segment2A;

            var sqrSegment1Length = direction1.sqrMagnitude;
            var sqrSegment2Length = direction2.sqrMagnitude;
            var segment1IsAPoint = sqrSegment1Length < Geometry.Epsilon;
            var segment2IsAPoint = sqrSegment2Length < Geometry.Epsilon;
            if (segment1IsAPoint && segment2IsAPoint)
            {
                if (segment1A == segment2A)
                {
                    intersection = IntersectionSegmentSegment2.Point(segment1A);
                    return true;
                }

                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            if (segment1IsAPoint)
            {
                if (PointSegment(segment1A, segment2A, direction2, sqrSegment2Length))
                {
                    intersection = IntersectionSegmentSegment2.Point(segment1A);
                    return true;
                }

                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            if (segment2IsAPoint)
            {
                if (PointSegment(segment2A, segment1A, direction1, sqrSegment1Length))
                {
                    intersection = IntersectionSegmentSegment2.Point(segment2A);
                    return true;
                }

                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            var denominator = VectorE.PerpDot(direction1, direction2);
            var perpDot1 = VectorE.PerpDot(direction1, from2ATo1A);
            var perpDot2 = VectorE.PerpDot(direction2, from2ATo1A);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDot1) > Geometry.Epsilon || Mathf.Abs(perpDot2) > Geometry.Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionSegmentSegment2.None();
                    return false;
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
                        return SegmentSegmentCollinear(segment1A, segment1B, sqrSegment1Length, segment2A, segment2B,
                            out intersection);
                    return SegmentSegmentCollinear(segment2A, segment2B, sqrSegment2Length, segment1A, segment1B,
                        out intersection);
                }

                // Contradirected
                var segment2BProjection = Vector2.Dot(direction1, segment2B - segment1A);
                if (segment2BProjection > -Geometry.Epsilon)
                    // 1A------1B
                    //     2B------2A
                    return SegmentSegmentCollinear(segment1A, segment1B, sqrSegment1Length, segment2B, segment2A,
                        out intersection);
                return SegmentSegmentCollinear(segment2B, segment2A, sqrSegment2Length, segment1A, segment1B,
                    out intersection);
            }

            // Not parallel
            var distance1 = perpDot2 / denominator;
            if (distance1 < -Geometry.Epsilon || distance1 > 1 + Geometry.Epsilon)
            {
                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            var distance2 = perpDot1 / denominator;
            if (distance2 < -Geometry.Epsilon || distance2 > 1 + Geometry.Epsilon)
            {
                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            intersection = IntersectionSegmentSegment2.Point(segment1A + direction1 * distance1);
            return true;
        }

        private static bool SegmentSegmentCollinear(Vector2 leftA, Vector2 leftB, float sqrLeftLength, Vector2 rightA,
            Vector2 rightB,
            out IntersectionSegmentSegment2 intersection)
        {
            var leftDirection = leftB - leftA;
            var rightAProjection = Vector2.Dot(leftDirection, rightA - leftB);
            if (Mathf.Abs(rightAProjection) < Geometry.Epsilon)
            {
                // LB == RA
                // LA------LB
                //         RA------RB
                intersection = IntersectionSegmentSegment2.Point(leftB);
                return true;
            }

            if (rightAProjection < 0)
            {
                // LB > RA
                // LA------LB
                //     RARB
                //     RA--RB
                //     RA------RB
                Vector2 pointB;
                var rightBProjection = Vector2.Dot(leftDirection, rightB - leftA);
                if (rightBProjection > sqrLeftLength)
                    pointB = leftB;
                else
                    pointB = rightB;
                intersection = IntersectionSegmentSegment2.Segment(rightA, pointB);
                return true;
            }

            // LB < RA
            // LA------LB
            //             RA------RB
            intersection = IntersectionSegmentSegment2.None();
            return false;
        }

        #endregion Segment-Segment

        #region Segment-Circle

        /// <summary>
        ///     Computes an intersection of the segment and the circle
        /// </summary>
        public static bool SegmentCircle(Segment2 segment, Circle2 circle)
        {
            return SegmentCircle(segment.a, segment.b, circle.center, circle.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segment and the circle
        /// </summary>
        public static bool SegmentCircle(Segment2 segment, Circle2 circle, out IntersectionSegmentCircle intersection)
        {
            return SegmentCircle(segment.a, segment.b, circle.center, circle.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segment and the circle
        /// </summary>
        public static bool SegmentCircle(Vector2 segmentA, Vector2 segmentB, Vector2 circleCenter, float circleRadius)
        {
            return SegmentCircle(segmentA, segmentB, circleCenter, circleRadius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the segment and the circle
        /// </summary>
        public static bool SegmentCircle(Vector2 segmentA, Vector2 segmentB, Vector2 circleCenter, float circleRadius,
            out IntersectionSegmentCircle intersection)
        {
            var segmentAToCenter = circleCenter - segmentA;
            var fromAtoB = segmentB - segmentA;
            var segmentLength = fromAtoB.magnitude;
            if (segmentLength < Geometry.Epsilon)
            {
                var distanceToPoint = segmentAToCenter.magnitude;
                if (distanceToPoint < circleRadius + Geometry.Epsilon)
                {
                    if (distanceToPoint > circleRadius - Geometry.Epsilon)
                    {
                        intersection = IntersectionSegmentCircle.Point(segmentA);
                        return true;
                    }

                    intersection = IntersectionSegmentCircle.None();
                    return true;
                }

                intersection = IntersectionSegmentCircle.None();
                return false;
            }

            var segmentDirection = fromAtoB.normalized;
            var centerProjection = Vector2.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon ||
                centerProjection - circleRadius > segmentLength + Geometry.Epsilon)
            {
                intersection = IntersectionSegmentCircle.None();
                return false;
            }

            var sqrDistanceToLine = segmentAToCenter.sqrMagnitude - centerProjection * centerProjection;
            var sqrDistanceToIntersection = circleRadius * circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionSegmentCircle.None();
                return false;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon ||
                    centerProjection > segmentLength + Geometry.Epsilon)
                {
                    intersection = IntersectionSegmentCircle.None();
                    return false;
                }

                intersection = IntersectionSegmentCircle.Point(segmentA + segmentDirection * centerProjection);
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
                intersection = IntersectionSegmentCircle.TwoPoints(pointA, pointB);
                return true;
            }

            if (!pointAIsAfterSegmentA && !pointBIsBeforeSegmentB)
            {
                // The segment is inside, but no intersection
                intersection = IntersectionSegmentCircle.None();
                return true;
            }

            var pointAIsBeforeSegmentB = distanceA < segmentLength + Geometry.Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
            {
                // Point A intersection
                intersection = IntersectionSegmentCircle.Point(segmentA + segmentDirection * distanceA);
                return true;
            }

            var pointBIsAfterSegmentA = distanceB > -Geometry.Epsilon;
            if (pointBIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                // Point B intersection
                intersection = IntersectionSegmentCircle.Point(segmentA + segmentDirection * distanceB);
                return true;
            }

            intersection = IntersectionSegmentCircle.None();
            return false;
        }

        #endregion Segment-Circle

        #region Circle-Circle

        /// <summary>
        ///     Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Circle2 circleA, Circle2 circleB)
        {
            return CircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Circle2 circleA, Circle2 circleB, out IntersectionCircleCircle intersection)
        {
            return CircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius, out intersection);
        }

        /// <summary>
        ///     Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            return CircleCircle(centerA, radiusA, centerB, radiusB, out var intersection);
        }

        /// <summary>
        ///     Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out IntersectionCircleCircle intersection)
        {
            var fromBtoA = centerA - centerB;
            var distanceFromBtoASqr = fromBtoA.sqrMagnitude;
            if (distanceFromBtoASqr < Geometry.Epsilon)
            {
                if (Mathf.Abs(radiusA - radiusB) < Geometry.Epsilon)
                {
                    // Circles are coincident
                    intersection = IntersectionCircleCircle.Circle();
                    return true;
                }

                // One circle is inside the other
                intersection = IntersectionCircleCircle.None();
                return true;
            }

            // For intersections on the circle's edge magnitude is more stable than sqrMagnitude
            var distanceFromBtoA = Mathf.Sqrt(distanceFromBtoASqr);

            var sumOfRadii = radiusA + radiusB;
            if (Mathf.Abs(distanceFromBtoA - sumOfRadii) < Geometry.Epsilon)
            {
                // One intersection outside
                intersection = IntersectionCircleCircle.Point(centerB + fromBtoA * (radiusB / sumOfRadii));
                return true;
            }

            if (distanceFromBtoA > sumOfRadii)
            {
                // No intersections, circles are separate
                intersection = IntersectionCircleCircle.None();
                return false;
            }

            var differenceOfRadii = radiusA - radiusB;
            var differenceOfRadiiAbs = Mathf.Abs(differenceOfRadii);
            if (Mathf.Abs(distanceFromBtoA - differenceOfRadiiAbs) < Geometry.Epsilon)
            {
                // One intersection inside
                intersection = IntersectionCircleCircle.Point(centerB - fromBtoA * (radiusB / differenceOfRadii));
                return true;
            }

            if (distanceFromBtoA < differenceOfRadiiAbs)
            {
                // One circle is contained within the other
                intersection = IntersectionCircleCircle.None();
                return true;
            }

            // Two intersections
            var radiusASqr = radiusA * radiusA;
            var distanceToMiddle = 0.5f * (radiusASqr - radiusB * radiusB) / distanceFromBtoASqr + 0.5f;
            var middle = centerA - fromBtoA * distanceToMiddle;

            var discriminant = radiusASqr / distanceFromBtoASqr - distanceToMiddle * distanceToMiddle;
            var offset = fromBtoA.RotateCCW90() * Mathf.Sqrt(discriminant);

            intersection = IntersectionCircleCircle.TwoPoints(middle + offset, middle - offset);
            return true;
        }

        #endregion Circle-Circle
    }
}