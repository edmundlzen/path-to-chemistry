using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class SmallStoneCollectable : MonoBehaviour, ICollectable
{
    public Dictionary<string, int> returnMaterials { get; set; }
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private NotificationsController notificationsController;
    private bool onCollectedInitialized = false;
    private float alphaCT = 0f;

    void Start()
    {
        notificationsController = GameObject.FindGameObjectsWithTag("NotificationsController")[0].transform.GetComponent<NotificationsController>();

        returnMaterials = new Dictionary<string, int>()
        {
            {"Stone", 1},
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
        var survivalMaterials = PlayerData.Instance().survivalMaterials;
        foreach (var material in returnMaterials)
        {
            if (material.Value == 0) return;
            notificationsController.SendImageNotification(Resources.Load<Sprite>("Sprites/" + survivalMaterials[material.Key]["image"]), "+ " + material.Value, "green");
            survivalMaterials[material.Key]["quantity"] = Int32.Parse(survivalMaterials[material.Key]["quantity"].ToString()) + material.Value;
        }
    }
}