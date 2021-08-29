using UnityEngine;

namespace ProceduralToolkit.Skeleton
{
    public class StraightSkeletonExample : MonoBehaviour
    {
        private const float skeletonPolygonOffset = 0.1f;

        private StraightSkeleton skeleton;

        private void Awake()
        {
            var input = Geometry.StarPolygon2(5, 1, 2);
            var generator = new StraightSkeletonGenerator();
            skeleton = generator.Generate(input);
        }

        private void OnRenderObject()
        {
            if (skeleton == null) return;

            GLE.BeginLines();
            {
                DrawSkeleton(skeleton);
            }
            GL.End();
        }

        private static void DrawSkeleton(StraightSkeleton skeleton)
        {
            GL.Color(Color.white);
            for (var polygonIndex = 0; polygonIndex < skeleton.polygons.Count; polygonIndex++)
            {
                var zOffset = Vector3.back * (skeletonPolygonOffset * polygonIndex);

                var skeletonPolygon = skeleton.polygons[polygonIndex];
                for (var vertexIndex = 0; vertexIndex < skeletonPolygon.Count; vertexIndex++)
                {
                    var current = skeletonPolygon[vertexIndex];
                    var next = skeletonPolygon.GetLooped(vertexIndex + 1);
                    GLE.DrawLine((Vector3) current + zOffset, (Vector3) next + zOffset);
                }
            }
        }
    }
}