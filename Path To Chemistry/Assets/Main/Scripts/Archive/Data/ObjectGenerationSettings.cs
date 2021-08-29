using System;
using UnityEngine;

[CreateAssetMenu]
public class ObjectGenerationSettings : UpdatableData
{
    public Object[] objects;

    [Serializable]
    public class Object
    {
        public GameObject gameObject;
        public int minimumAmount;
        public int maximumAmount;
        public float minimumHeight;
        public float maximumHeight;
        public bool followRotation;
    }
}