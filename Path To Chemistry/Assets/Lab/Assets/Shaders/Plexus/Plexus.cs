using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class Plexus : MonoBehaviour
{
    public ComputeShader plexus;
    public int amountOfPoints = 100;
    public int PPPS = 2;
    public float lineWidth = 0.02f;
    public Material lineMaterial;
    public Vector3 box = new Vector3(4, 4, 4);
    public float particleSpeed = 1.0f;
    public float maxConnDistance = 3.0f;
    private float maxConnDistanceSqr;
    private Vector3[] defaultPositions;
    private Vector3[] velocities;
    private Vector3[] positions;
    private Mesh lineMesh;
    private void Start()
    {
        lineMaterial.SetVector("_BoxDims", new Vector4(box.x, box.y, box.z, 1));
        positions = new Vector3[amountOfPoints];
        defaultPositions = new Vector3[amountOfPoints];
        for (int i = 0; i < amountOfPoints; ++i)
        {
            positions[i] = new Vector3(
                Random.Range(-box.x, box.x),
                Random.Range(-box.y, box.y),
                Random.Range(-box.z, box.z));
            defaultPositions[i] = positions[i];
        }
        lineMesh = new Mesh();
        int[] trigs = new int[6];
        trigs[0] = 0;
        trigs[1] = 1;
        trigs[2] = 2;
        trigs[3] = 3;
        trigs[4] = 2;
        trigs[5] = 1;
        lineMesh.vertices = verts;
        lineMesh.triangles = trigs;
        velocities = new Vector3[amountOfPoints];
        StartCoroutine(ConnectDots());
    }
    private void Update()
    {
        MovePoints();
        RenderLines();
    }
    private void MovePoints()
    {
        int kernelIndex = plexus.FindKernel("MoveParticels");
        ComputeBuffer positionsBuffer = new ComputeBuffer(positions.Length, 12);
        positionsBuffer.SetData(positions);
        plexus.SetBuffer(kernelIndex, "positions", positionsBuffer);
        ComputeBuffer defaultPositionsBuffer = new ComputeBuffer(defaultPositions.Length, 12);
        defaultPositionsBuffer.SetData(defaultPositions);
        plexus.SetBuffer(kernelIndex, "defaultPositions", defaultPositionsBuffer);
        ComputeBuffer velocitiesBuffer = new ComputeBuffer(velocities.Length, 12);
        velocitiesBuffer.SetData(velocities);
        plexus.SetBuffer(kernelIndex, "velocities", velocitiesBuffer);
        plexus.SetFloat("deltaTime", Time.deltaTime);
        plexus.SetFloat("elapsedTime", Time.time);
        plexus.SetFloat("particleSpeed", particleSpeed);
        plexus.Dispatch(kernelIndex, positions.Length, 1, 1);
        positionsBuffer.GetData(positions);
        positionsBuffer.Release();
        defaultPositionsBuffer.Release();
        velocitiesBuffer.Release();
    }

    private static float DistanceSqr(Vector3 p1, Vector3 p2)
    {
        return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z);
    }

    Vector3 normal, side, p1, p2;
    int startingVerticesIndex = 0;
    List<int> lineTrigs = new List<int>();
    List<Vector3> lineVerts = new List<Vector3>();
    Vector3[] verts = new Vector3[4];
    int[] trigs = new int[6];
    private void RenderLines()
    {
        lineMesh = new Mesh();
        for (int i = 0; i < connected.Count; ++i)
        {
            p1 = positions[connected[i].Key];
            p2 = positions[connected[i].Value];
            normal = Vector3.Cross(p1, p2);
            side = Vector3.Cross(normal, p2 - p1);
            side.Normalize();
            startingVerticesIndex = lineVerts.Count;
            verts[0] = p1 + side * (lineWidth / 2);
            verts[1] = p1 + side * (lineWidth / -2);
            verts[2] = p2 + side * (lineWidth / 2);
            verts[3] = p2 + side * (lineWidth / -2);
            trigs[0] = startingVerticesIndex;
            trigs[1] = trigs[5] = startingVerticesIndex + 1;
            trigs[2] = trigs[4] = startingVerticesIndex + 2;
            trigs[3] = startingVerticesIndex + 3;
            lineVerts.AddRange(verts);
            lineTrigs.AddRange(trigs);
        }
        lineMesh.vertices = lineVerts.ToArray();
        lineMesh.triangles = lineTrigs.ToArray();
        lineMesh.RecalculateBounds();
        Graphics.DrawMesh(lineMesh, transform.localToWorldMatrix, lineMaterial, 0);
        lineTrigs.Clear();
        lineVerts.Clear();
    }
    [HideInInspector]
    public bool isEnabled = false;
    List<KeyValuePair<int, int>> connected = new List<KeyValuePair<int, int>>();
    HashSet<KeyValuePair<int, int>> connectedHashSet = new HashSet<KeyValuePair<int, int>>();
    private IEnumerator ConnectDots()
    {
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        int indx = 0, i = 0, j = 0;
        Vector3 currentPos;
        maxConnDistanceSqr = maxConnDistance * maxConnDistance;
        do
        {
            yield return wfeof;
            for (j = 0; j < PPPS; ++j)
            {
                currentPos = positions[indx];
                connected.RemoveAll(x => x.Key == indx || x.Value == indx);
                connectedHashSet.RemoveWhere(x => x.Key == indx || x.Value == indx);
                for (i = 0; i < amountOfPoints; ++i)
                {
                    if (i == indx)
                        continue;
                    if (DistanceSqr(currentPos, positions[i]) < maxConnDistanceSqr)
                    {
                        KeyValuePair<int, int> k = new KeyValuePair<int, int>(indx, i);
                        if(connectedHashSet.Add(k))
                            connected.Add(new KeyValuePair<int, int>(indx, i));
                    }
                }
                ++indx;
                if (indx >= amountOfPoints)
                    indx = 0;
            }
        } while (!isEnabled);
    }
}
