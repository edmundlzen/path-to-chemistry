using System;
using UnityEngine;

public class TerrainChunk
{
    private const float colliderGenerationDistanceThreshold = 5;
    private Bounds bounds;
    private readonly int colliderLODIndex;
    public Vector2 coord;

    private readonly LODInfo[] detailLevels;
    private bool hasSetCollider;

    private HeightMap heightMap;
    private bool heightMapReceived;

    private readonly HeightMapSettings heightMapSettings;
    private readonly LODMesh[] lodMeshes;
    private readonly float maxViewDst;
    private readonly MeshCollider meshCollider;
    private readonly MeshFilter meshFilter;
    private GenerateGrass meshGrassGenerator;

    private readonly GameObject meshObject;
    private GenerateObjects meshObjectGenerator;

    private readonly MeshRenderer meshRenderer;
    private readonly MeshSettings meshSettings;
    private int previousLODIndex = -1;
    private readonly Vector2 sampleCentre;
    private readonly Transform viewer;

    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings,
        LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material,
        ObjectGenerationSettings objectGenerationSettings, GrassGenerationSettings grassGenerationSettings,
        GrassPainter grassPainter, Transform particleAttractor, Transform playerTransform)
    {
        this.coord = coord;
        this.detailLevels = detailLevels;
        this.colliderLODIndex = colliderLODIndex;
        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        this.viewer = viewer;

        sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        var position = coord * meshSettings.meshWorldSize;
        bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);

        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshObjectGenerator = meshObject.AddComponent<GenerateObjects>();
        meshObject.GetComponent<GenerateObjects>().objectGenerationSettings = objectGenerationSettings;
        meshObject.GetComponent<GenerateObjects>().particleAttractor = particleAttractor;
        meshGrassGenerator = meshObject.AddComponent<GenerateGrass>();
        meshObject.GetComponent<GenerateGrass>().grassGenerationSettings = grassGenerationSettings;
        meshObject.GetComponent<GenerateGrass>().playerTransform = playerTransform;
        meshObject.GetComponent<GenerateGrass>().grassPainter = grassPainter;
        meshObject.GetComponent<GenerateGrass>().grassComputeScript = grassPainter.GetComponent<GrassComputeScript>();
        meshRenderer.material = material;
        meshObject.transform.position = new Vector3(position.x, 0, position.y);
        meshObject.transform.parent = parent;
        SetVisible(false);

        lodMeshes = new LODMesh[detailLevels.Length];
        for (var i = 0; i < detailLevels.Length; i++)
        {
            lodMeshes[i] = new LODMesh(detailLevels[i].lod);
            lodMeshes[i].updateCallback += UpdateTerrainChunk;
            if (i == colliderLODIndex) lodMeshes[i].updateCallback += UpdateCollisionMesh;
        }

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
    }

    private Vector2 viewerPosition => new Vector2(viewer.position.x, viewer.position.z);

    public event Action<TerrainChunk, bool> onVisibilityChanged;

    public void Load()
    {
        ThreadedDataRequester.RequestData(
            () => HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine,
                heightMapSettings, sampleCentre), OnHeightMapReceived);
    }


    private void OnHeightMapReceived(object heightMapObject)
    {
        heightMap = (HeightMap) heightMapObject;
        heightMapReceived = true;

        UpdateTerrainChunk();
    }


    public void UpdateTerrainChunk()
    {
        if (heightMapReceived)
        {
            var viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            var wasVisible = IsVisible();
            var visible = viewerDstFromNearestEdge <= maxViewDst;

            if (visible)
            {
                var lodIndex = 0;

                for (var i = 0; i < detailLevels.Length - 1; i++)
                    if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                        lodIndex = i + 1;
                    else
                        break;

                if (lodIndex != previousLODIndex)
                {
                    var lodMesh = lodMeshes[lodIndex];
                    if (lodMesh.hasMesh)
                    {
                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                    }
                    else if (!lodMesh.hasRequestedMesh)
                    {
                        lodMesh.RequestMesh(heightMap, meshSettings);
                    }
                }
            }

            if (wasVisible != visible)
            {
                SetVisible(visible);
                if (onVisibilityChanged != null) onVisibilityChanged(this, visible);
            }
        }
    }

    public void UpdateCollisionMesh()
    {
        if (!hasSetCollider)
        {
            var sqrDstFromViewerToEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            if (sqrDstFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold)
                if (!lodMeshes[colliderLODIndex].hasRequestedMesh)
                    lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);

            if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold)
                if (lodMeshes[colliderLODIndex].hasMesh)
                {
                    meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                    hasSetCollider = true;
                }
        }
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    public bool IsVisible()
    {
        return meshObject.activeSelf;
    }
}

internal class LODMesh
{
    public bool hasMesh;
    public bool hasRequestedMesh;
    private readonly int lod;

    public Mesh mesh;

    public LODMesh(int lod)
    {
        this.lod = lod;
    }

    public event Action updateCallback;

    private void OnMeshDataReceived(object meshDataObject)
    {
        mesh = ((MeshData) meshDataObject).CreateMesh();
        hasMesh = true;

        updateCallback();
    }

    public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
    {
        hasRequestedMesh = true;
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod),
            OnMeshDataReceived);
    }
}