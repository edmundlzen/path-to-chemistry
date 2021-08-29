using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private const float viewerMoveThresholdForChunkUpdate = 25f;

    private const float sqrViewerMoveThresholdForChunkUpdate =
        viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;


    public int colliderLODIndex;
    public LODInfo[] detailLevels;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureSettings;
    public ObjectGenerationSettings objectGenerationSettings;
    public GrassGenerationSettings grassGenerationSettings;
    public GrassPainter grassPainter;
    public Transform particleAttractor;

    public Transform viewer;
    public Material mapMaterial;
    private int chunksVisibleInViewDst;

    private float meshWorldSize;

    private readonly Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();

    private Vector2 viewerPosition;
    private Vector2 viewerPositionOld;
    private readonly List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    private void Start()
    {
        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        var maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        meshWorldSize = meshSettings.meshWorldSize;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

        UpdateVisibleChunks();
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if (viewerPosition != viewerPositionOld)
            foreach (var chunk in visibleTerrainChunks)
                chunk.UpdateCollisionMesh();

        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    private void UpdateVisibleChunks()
    {
        var alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for (var i = visibleTerrainChunks.Count - 1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();
        }

        var currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldSize);
        var currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldSize);

        for (var yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        for (var xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
        {
            var viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
            if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
            {
                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                }
                else
                {
                    var newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings, detailLevels,
                        colliderLODIndex, transform, viewer, mapMaterial, objectGenerationSettings,
                        grassGenerationSettings, grassPainter, particleAttractor, viewer);
                    terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                    newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                    newChunk.Load();
                }
            }
        }
    }

    private void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
            visibleTerrainChunks.Add(chunk);
        else
            visibleTerrainChunks.Remove(chunk);
    }
}

[Serializable]
public struct LODInfo
{
    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int lod;

    public float visibleDstThreshold;


    public float sqrVisibleDstThreshold => visibleDstThreshold * visibleDstThreshold;
}