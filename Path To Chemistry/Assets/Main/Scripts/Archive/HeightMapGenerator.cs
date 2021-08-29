using UnityEngine;

public static class HeightMapGenerator
{
    private static float[,] falloffMap;

    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre)
    {
        var values = Noise.GenerateNoiseMap(width, height, settings.noiseSettings, sampleCentre);

        var heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);

        var minValue = float.MaxValue;
        var maxValue = float.MinValue;

        if (settings.useFalloff)
            if (falloffMap == null)
                falloffMap = FalloffGenerator.GenerateFalloffMap(width);

        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
            values[i, j] *=
                heightCurve_threadsafe.Evaluate(values[i, j] - (settings.useFalloff ? falloffMap[i, j] : 0)) *
                settings.heightMultiplier;

            if (values[i, j] > maxValue) maxValue = values[i, j];
            if (values[i, j] < minValue) minValue = values[i, j];
        }

        return new HeightMap(values, minValue, maxValue);
    }
}

public struct HeightMap
{
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;

    public HeightMap(float[,] values, float minValue, float maxValue)
    {
        this.values = values;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}