using UnityEngine;

[CreateAssetMenu]
public class GrassGenerationSettings : UpdatableData
{
    [Range(1, 10)] public int grassDensity;
}