using UnityEngine;

[CreateAssetMenu]
public class HeightMapSettings : UpdatableData
{
    public NoiseSettings noiseSettings;

    public bool useFalloff;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public float minHeight => heightMultiplier * heightCurve.Evaluate(0);

    public float maxHeight => heightMultiplier * heightCurve.Evaluate(1);

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        noiseSettings.ValidateValues();
        base.OnValidate();
    }
#endif
}