using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu()]
public class GrassGenerationSettings : UpdatableData {
    public Object[] objects;

    [System.Serializable]
    public class Object {
        public GameObject gameObject;
        public int minimumAmount;
        public int maximumAmount;
        public float minimumHeight;
        public float maximumHeight;
    }
}