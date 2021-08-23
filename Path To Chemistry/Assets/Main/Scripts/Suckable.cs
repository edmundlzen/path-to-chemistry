using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suckable : MonoBehaviour
{
    private Transform particleSystem;

    private bool oneShotSetMeshMat = true;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = transform.Find("Particle System");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Suck(RaycastHit hit, Transform playerTransform)
    {
        if (oneShotSetMeshMat)
        {
            SetParticles();
            oneShotSetMeshMat = false;
        }

        particleSystem.GetComponent<ParticleSystem>().Play();
        particleSystem.LookAt(playerTransform);
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
