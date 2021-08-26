using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suckable : MonoBehaviour
{
    new Transform particleSystem;
    
    public float HP = 1f;
    private float startHP;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = transform.Find("Particle System");
        SetParticles();
        transform.GetComponent<Renderer>().material = new Material(transform.GetComponent<Renderer>().material);
        startHP = HP;
    }

    public void Suck(RaycastHit hit, Transform playerTransform)
    {
        if (HP <= 0f)
        {
            Destroy(gameObject);
        }

        if (particleSystem.GetComponent<ParticleSystem>().time > .1f)
        {
            particleSystem.GetComponent<ParticleSystem>().time = 0;
        }
        particleSystem.GetComponent<ParticleSystem>().Play();
        particleSystem.LookAt(playerTransform);

        HP -= 0.3f * Time.deltaTime;
        float newThreshold = startHP - HP;
        GetComponent<Renderer>().material.SetFloat("Alpha_Clip_Threshold", newThreshold);
    }

    void SetParticles()
    {
        // particleSystem.gameObject.SetActive(true);
        particleSystem.GetComponent<ParticleSystemRenderer>().mesh =
            transform.GetComponent<MeshFilter>().mesh;
        particleSystem.GetComponent<ParticleSystemRenderer>().material =
            transform.GetComponent<Renderer>().material;
    }
}
