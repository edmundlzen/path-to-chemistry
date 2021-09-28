using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class S4BCollectable : MonoBehaviour, ICollectable
{
    public Dictionary<string, int> returnMaterials { get; set; }
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private bool onCollectedInitialized = false;
    private float alphaCT = 0f;

    void Awake()
    {
        returnMaterials = new Dictionary<string, int>()
        {
            {"Stone", 2},
            {"Coal Ore", Random.Range(0f, 1f) < 0.5f ? 2 : 0},
            {"Diamond Ore", Random.Range(0f, 1f) < 0.1f ? 2 : 0},
            {"Emerald Ore", Random.Range(0f, 1f) < 0.05f ? 2 : 0},
            {"Gold Ore", Random.Range(0f, 1f) < 0.2f ? 2 : 0},
            {"Iron Ore", Random.Range(0f, 1f) < 0.3f ? 2 : 0},
            {"Lapis Lazuli Ore", Random.Range(0f, 1f) < 0.4f ? 2 : 0},
        };
    }

    void Initialize()
    {
        onCollectedInitialized = true;
        
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
            newGameObject.GetComponent<ParticleSystemRenderer>().material = newSpriteMaterial;
            newGameObject.transform.SetParent(transform, false);
        }
        
        Material currentMaterial = transform.GetComponent<Renderer>().material;
        Material newMaterial = new Material(Shader.Find("Shader Graphs/Collectable Dissolve"));
        newMaterial.SetTexture("Texture", currentMaterial.GetTexture("_MainTex"));
        newMaterial.SetTexture("Normal_Map", currentMaterial.GetTexture("_BumpMap"));
        newMaterial.SetTexture("Occlusion_Map", currentMaterial.GetTexture("_OcclusionMap"));
        newMaterial.SetTexture("Metallic_Map", currentMaterial.GetTexture("_MetallicGlossMap"));
        newMaterial.SetFloat("Noise_Scale", 400f);
        transform.GetComponent<Renderer>().material = newMaterial;
    }

    public void Collect()
    {
        if (!onCollectedInitialized) Initialize();
        while (alphaCT < 0.8f)
        {
            alphaCT += 0.01f;
            transform.GetComponent<MeshRenderer>().material.SetFloat("Alpha_Clip_Threshold", alphaCT);
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