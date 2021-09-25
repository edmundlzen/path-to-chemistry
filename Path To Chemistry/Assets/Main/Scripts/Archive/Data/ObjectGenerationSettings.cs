using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectGenerationSettings : UpdatableData
{
    public Object[] objects;
    public float despawnDistance;

    [Serializable]
    public class Object
    {
        public GameObject gameObject;
        public int minimumAmount;
        public int maximumAmount;
        public float minimumHeight;
        public float maximumHeight;
        public bool followRotation;
        public float minimumDistance;
        public bool followLayers;
        public List<int> layers;
    }
}