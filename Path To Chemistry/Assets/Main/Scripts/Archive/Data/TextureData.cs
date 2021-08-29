﻿using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TextureData : UpdatableData
{
    private const int textureSize = 512;
    private const TextureFormat textureFormat = TextureFormat.RGB565;

    public Layer[] layers;
    private float savedMaxHeight;

    private float savedMinHeight;

    public void ApplyToMaterial(Material material)
    {
        material.SetInt("layerCount", layers.Length);
        material.SetColorArray("baseColours", layers.Select(x => x.tint).ToArray());
        material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
        material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        material.SetFloatArray("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
        material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
        var texturesArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);

        UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }

    private Texture2DArray GenerateTextureArray(Texture2D[] textures)
    {
        var textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        for (var i = 0; i < textures.Length; i++) textureArray.SetPixels(textures[i].GetPixels(), i);
        textureArray.Apply();
        return textureArray;
    }

    [Serializable]
    public class Layer
    {
        public Texture2D texture;
        public Color tint;

        [Range(0, 1)] public float tintStrength;

        [Range(0, 1)] public float startHeight;

        [Range(0, 1)] public float blendStrength;

        public float textureScale;
    }
}