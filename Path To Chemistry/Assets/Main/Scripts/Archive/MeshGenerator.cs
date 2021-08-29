using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, MeshSettings meshSettings, int levelOfDetail)
    {
        var skipIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        var numVertsPerLine = meshSettings.numVertsPerLine;

        var topLeft = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

        var meshData = new MeshData(numVertsPerLine, skipIncrement, meshSettings.useFlatShading);

        var vertexIndicesMap = new int[numVertsPerLine, numVertsPerLine];
        var meshVertexIndex = 0;
        var outOfMeshVertexIndex = -1;

        for (var y = 0; y < numVertsPerLine; y++)
        for (var x = 0; x < numVertsPerLine; x++)
        {
            var isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
            var isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 &&
                                  ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);
            if (isOutOfMeshVertex)
            {
                vertexIndicesMap[x, y] = outOfMeshVertexIndex;
                outOfMeshVertexIndex--;
            }
            else if (!isSkippedVertex)
            {
                vertexIndicesMap[x, y] = meshVertexIndex;
                meshVertexIndex++;
            }
        }

        for (var y = 0; y < numVertsPerLine; y++)
        for (var x = 0; x < numVertsPerLine; x++)
        {
            var isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 &&
                                  ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

            if (!isSkippedVertex)
            {
                var isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
                var isMeshEdgeVertex = (y == 1 || y == numVertsPerLine - 2 || x == 1 || x == numVertsPerLine - 2) &&
                                       !isOutOfMeshVertex;
                var isMainVertex = (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMeshVertex &&
                                   !isMeshEdgeVertex;
                var isEdgeConnectionVertex =
                    (y == 2 || y == numVertsPerLine - 3 || x == 2 || x == numVertsPerLine - 3) && !isOutOfMeshVertex &&
                    !isMeshEdgeVertex && !isMainVertex;

                var vertexIndex = vertexIndicesMap[x, y];
                var percent = new Vector2(x - 1, y - 1) / (numVertsPerLine - 3);
                var vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * meshSettings.meshWorldSize;
                var height = heightMap[x, y];

                if (isEdgeConnectionVertex)
                {
                    var isVertical = x == 2 || x == numVertsPerLine - 3;
                    var dstToMainVertexA = (isVertical ? y - 2 : x - 2) % skipIncrement;
                    var dstToMainVertexB = skipIncrement - dstToMainVertexA;
                    var dstPercentFromAToB = dstToMainVertexA / (float) skipIncrement;

                    var heightMainVertexA = heightMap[isVertical ? x : x - dstToMainVertexA,
                        isVertical ? y - dstToMainVertexA : y];
                    var heightMainVertexB = heightMap[isVertical ? x : x + dstToMainVertexB,
                        isVertical ? y + dstToMainVertexB : y];

                    height = heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB;
                }

                meshData.AddVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), percent, vertexIndex);

                var createTriangle = x < numVertsPerLine - 1 && y < numVertsPerLine - 1 &&
                                     (!isEdgeConnectionVertex || x != 2 && y != 2);

                if (createTriangle)
                {
                    var currentIncrement = isMainVertex && x != numVertsPerLine - 3 && y != numVertsPerLine - 3
                        ? skipIncrement
                        : 1;

                    var a = vertexIndicesMap[x, y];
                    var b = vertexIndicesMap[x + currentIncrement, y];
                    var c = vertexIndicesMap[x, y + currentIncrement];
                    var d = vertexIndicesMap[x + currentIncrement, y + currentIncrement];
                    meshData.AddTriangle(a, d, c);
                    meshData.AddTriangle(d, a, b);
                }
            }
        }

        meshData.ProcessMesh();

        return meshData;
    }
}

public class MeshData
{
    private Vector3[] bakedNormals;
    private int outOfMeshTriangleIndex;
    private readonly int[] outOfMeshTriangles;

    private readonly Vector3[] outOfMeshVertices;

    private int triangleIndex;
    private readonly int[] triangles;

    private readonly bool useFlatShading;
    private Vector2[] uvs;
    private Vector3[] vertices;

    public MeshData(int numVertsPerLine, int skipIncrement, bool useFlatShading)
    {
        this.useFlatShading = useFlatShading;

        var numMeshEdgeVertices = (numVertsPerLine - 2) * 4 - 4;
        var numEdgeConnectionVertices = (skipIncrement - 1) * (numVertsPerLine - 5) / skipIncrement * 4;
        var numMainVerticesPerLine = (numVertsPerLine - 5) / skipIncrement + 1;
        var numMainVertices = numMainVerticesPerLine * numMainVerticesPerLine;

        vertices = new Vector3[numMeshEdgeVertices + numEdgeConnectionVertices + numMainVertices];
        uvs = new Vector2[vertices.Length];

        var numMeshEdgeTriangles = 8 * (numVertsPerLine - 4);
        var numMainTriangles = (numMainVerticesPerLine - 1) * (numMainVerticesPerLine - 1) * 2;
        triangles = new int[(numMeshEdgeTriangles + numMainTriangles) * 3];

        outOfMeshVertices = new Vector3[numVertsPerLine * 4 - 4];
        outOfMeshTriangles = new int[24 * (numVertsPerLine - 2)];
    }

    public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
        if (vertexIndex < 0)
        {
            outOfMeshVertices[-vertexIndex - 1] = vertexPosition;
        }
        else
        {
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }
    }

    public void AddTriangle(int a, int b, int c)
    {
        if (a < 0 || b < 0 || c < 0)
        {
            outOfMeshTriangles[outOfMeshTriangleIndex] = a;
            outOfMeshTriangles[outOfMeshTriangleIndex + 1] = b;
            outOfMeshTriangles[outOfMeshTriangleIndex + 2] = c;
            outOfMeshTriangleIndex += 3;
        }
        else
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }
    }

    private Vector3[] CalculateNormals()
    {
        var vertexNormals = new Vector3[vertices.Length];
        var triangleCount = triangles.Length / 3;
        for (var i = 0; i < triangleCount; i++)
        {
            var normalTriangleIndex = i * 3;
            var vertexIndexA = triangles[normalTriangleIndex];
            var vertexIndexB = triangles[normalTriangleIndex + 1];
            var vertexIndexC = triangles[normalTriangleIndex + 2];

            var triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        var borderTriangleCount = outOfMeshTriangles.Length / 3;
        for (var i = 0; i < borderTriangleCount; i++)
        {
            var normalTriangleIndex = i * 3;
            var vertexIndexA = outOfMeshTriangles[normalTriangleIndex];
            var vertexIndexB = outOfMeshTriangles[normalTriangleIndex + 1];
            var vertexIndexC = outOfMeshTriangles[normalTriangleIndex + 2];

            var triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            if (vertexIndexA >= 0) vertexNormals[vertexIndexA] += triangleNormal;
            if (vertexIndexB >= 0) vertexNormals[vertexIndexB] += triangleNormal;
            if (vertexIndexC >= 0) vertexNormals[vertexIndexC] += triangleNormal;
        }


        for (var i = 0; i < vertexNormals.Length; i++) vertexNormals[i].Normalize();

        return vertexNormals;
    }

    private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        var pointA = indexA < 0 ? outOfMeshVertices[-indexA - 1] : vertices[indexA];
        var pointB = indexB < 0 ? outOfMeshVertices[-indexB - 1] : vertices[indexB];
        var pointC = indexC < 0 ? outOfMeshVertices[-indexC - 1] : vertices[indexC];

        var sideAB = pointB - pointA;
        var sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public void ProcessMesh()
    {
        if (useFlatShading)
            FlatShading();
        else
            BakeNormals();
    }

    private void BakeNormals()
    {
        bakedNormals = CalculateNormals();
    }

    private void FlatShading()
    {
        var flatShadedVertices = new Vector3[triangles.Length];
        var flatShadedUvs = new Vector2[triangles.Length];

        for (var i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
    }

    public Mesh CreateMesh()
    {
        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (useFlatShading)
            mesh.RecalculateNormals();
        else
            mesh.normals = bakedNormals;
        return mesh;
    }
}