using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCollectable : MonoBehaviour, ICollectable
{
    public Dictionary<string, int> returnMaterials { get; set; }
    private bool onCollectedInitialized = false;
    private float alphaCT = 0f;

    void Awake()
    {
        returnMaterials = new Dictionary<string, int>()
        {
            {"Crystal", 5},
            {"Gold", 5}
        };
    }

    void Initialize()
    {
        // After done initialized
        onCollectedInitialized = true;

        for (int i = 0; i < returnMaterials.Count; i++)
        {
            
        }
    }
    
    public void Collect()
    {
        // if (!onCollectedInitialized) Initialize();
        while (alphaCT < 0.8f)
        {
            alphaCT += 0.01f;
            transform.GetComponent<MeshRenderer>().material.SetFloat("Alpha_Clip_Threshold", alphaCT);
            return;
        }
        
        Return();
        Destroy(gameObject);
    }

    public void Return()
    {
        
    }
}