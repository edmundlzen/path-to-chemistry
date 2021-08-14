using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu()]
public class ObjectGenerationSettings : UpdatableData {
    public Object[] objects;

    [System.Serializable]
    public class Object {
        public GameObject gameObject;
        public int minimumAmount;
        public int maximumAmount;
    }
}