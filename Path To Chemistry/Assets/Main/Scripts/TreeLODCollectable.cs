using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class TreeLODCollectable : MonoBehaviour, ICollectable
{
    public Dictionary<string, int> returnMaterials { get; set; }
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private List<GameObject> LODGameObjects = new List<GameObject>();
    private bool onCollectedInitialized = false;
    private float alphaCT = 0f;
    private bool called = false;

    void Awake()
    {
        returnMaterials = new Dictionary<string, int>()
        {
            {"Wood", Random.Range(1, 3)},
        };
    }

    void Initialize()
    {
        onCollectedInitialized = true;
        
        Material currentMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        Material newMaterial = new Material(Shader.Find("Shader Graphs/Collectable Dissolve"));
        newMaterial.SetTexture("Texture", currentMaterial.GetTexture("_MainTex"));
        newMaterial.SetTexture("Normal_Map", currentMaterial.GetTexture("_BumpMap"));
        newMaterial.SetTexture("Occlusion_Map", currentMaterial.GetTexture("_OcclusionMap"));
        newMaterial.SetTexture("Metallic_Map", currentMaterial.GetTexture("_MetallicGlossMap"));
        newMaterial.SetFloat("Noise_Scale", 400f);

        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material = newMaterial;
        }
        
        foreach (var material in returnMaterials)
        {
            var imageName = PlayerData.Instance().survivalMaterials[material.Key]["image"];
            var image = Resources.Load<Sprite>("Sprites/" + imageName);
            var newSpriteMaterial = new Material(Resources.Load<Material>("Materials/Particle Mat"));
            var newGameObject = new GameObject(material.Key + " Particle System");
            newSpriteMaterial.SetTexture("_BaseMap", image.texture);
            var newParticleSystem = newGameObject.AddComponent<ParticleSystem>();
            particleSystems.Add(newParticleSystem);
            newParticleSystem.Stop();
            var psMain = newParticleSystem.main;
            psMain.duration = 1f;
            psMain.loop = false;
            psMain.startLifetime = 2f;
            newGameObject.GetComponent<ParticleSystemRenderer>().material = newSpriteMaterial;
            newGameObject.transform.SetParent(transform, false);
        }
    }

    public void Collect()
    {
        if (!onCollectedInitialized) Initialize();
        
        called = true;
        while (alphaCT < 0.8f)
        {
            alphaCT += 0.01f;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Alpha_Clip_Threshold", alphaCT);
            
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Play();
            }

            return;
        }
        
        Return();
        Destroy(gameObject);
    }

    public void Return()
    {
        
    }
}