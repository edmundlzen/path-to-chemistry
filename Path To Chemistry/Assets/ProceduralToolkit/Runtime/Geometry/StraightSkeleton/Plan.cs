using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Skeleton
{
    /// <summary>
    ///     Representation of the active plan during generation process
    /// </summary>
    public class Plan : IEnumerable<Plan.Vertex>
    {
        private readonly List<Vertex> vertices = new List<Vertex>();

        private Plan()
        {
        }

        public Plan(IEnumerable<Vector2> polygon)
        {
            foreach (var vertex in polygon) vertices.Add(new Vertex(vertex));
            for (var i = 0; i < Count; i++)
            {
                var vertex = vertices[i];
                vertex.previous = vertices.GetLooped(i - 1);
                vertex.next = vertices.GetLooped(i + 1);
            }
        }

        public int Count => vertices.Count;
        public Vertex First => vertices[0];

        public IEnumerator<Vertex> GetEnumerator()
        {
            if (Count == 0) yield break;
            var startVertex = vertices[0];
            var currentVertex = startVertex;
            var i = 0;
            var max = Count;
            do
            {
                if (i >= max)
                {
                    Debug.LogError("Invalid connectivity");
                    yield break;
                }

                yield return currentVertex;
                currentVertex = currentVertex.next;
                i++;
            } while (!currentVertex.Equals(startVertex));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Add(Vertex vertex)
        {
            vertices.Add(vertex);
        }

        public void Insert(Vertex vertex, Vertex previous, Vertex next)
        {
            vertices.Add(vertex);
            LinkVertices(previous, vertex, next);
        }

        public bool Remove(Vertex vertex)
        {
            return vertices.Remove(vertex);
        }

        public void Offset(float offset)
        {
            foreach (var vertex in vertices)
                vertex.position -= vertex.bisector * Geometry.GetAngleOffset(offset, vertex.angle);
        }

        public List<Plan> Split()
        {
            var plans = new List<Plan>();
            while (Count > 0)
            {
                var i = 0;
                var max = Count;
                var plan = new Plan();
                var startVertex = First;
                var currentVertex = startVertex;
                do
                {
                    if (i >= max)
                    {
                        Debug.LogError("Invalid connectivity");
                        break;
                    }

                    Remove(currentVertex);
                    plan.Add(currentVertex);
                    currentVertex = currentVertex.next;
                    i++;
                } while (!currentVertex.Equals(startVertex));

                plans.Add(plan);
            }

            return plans;
        }

        private static void LinkVertices(Vertex a, Vertex b)
        {
            a.next = b;
            b.previous = a;
        }

        private static void LinkVertices(Vertex a, Vertex b, Vertex c)
        {
            LinkVertices(a, b);
            LinkVertices(b, c);
        }

        public class Vertex
        {
            public float angle;
            public Vector2 bisector;
            public bool inEvent;
            public Vertex next;
            public int nextPolygonIndex;
            public Vector2 position;
            public Vertex previous;
            public int previousPolygonIndex;

            public Vertex(Vector2 position)
            {
                this.position = position;
            }

            public bool reflect => angle >= 180;

            public override string ToString()
            {
                return string.Format("{0} inEvent: {1}", position, inEvent);
            }
        }
    }
}