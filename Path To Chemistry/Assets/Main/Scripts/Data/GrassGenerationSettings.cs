using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu()]
public class GrassGenerationSettings : UpdatableData
{
    [Range(0,10)] public int grassDensity;
}