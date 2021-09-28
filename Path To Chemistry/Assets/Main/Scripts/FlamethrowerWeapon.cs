using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerWeapon: MonoBehaviour, IUsable
{
    public int damage = 5;
    public float fireRate = 0.1f;
    public AudioClip laserSound;
    public AudioSource audioSource;
    
    private Transform playerCamera;
    private GameObject PFlame;
    private GameObject PSmoke;
    private GameObject PSpark;
    private float lastShot;

    private bool gunEnabled;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserSound;
        PFlame = transform.Find("Flamethrower").Find("Fire Point").Find("PFlame").gameObject;
        PSmoke = PFlame.transform.Find("PSmoke").gameObject;
        PSpark = PSmoke.transform.Find("PSpark").gameObject;
    }
    
    public void Use(Transform pCamera)
    {
        gunEnabled = true;
        playerCamera = pCamera;
    }
    
    void LateUpdate()
    {
        if (gunEnabled)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            
            EnableFlame();
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit) && (Time.time - lastShot) > fireRate)
            {
                lastShot = Time.time;
                if (hit.transform.TryGetComponent(out IEntity entity))
                {
                    entity.Attacked(damage);
                }
            }
        }
        else
        {
            DisableFlame();
        }

        gunEnabled = false;
    }

    void EnableFlame()
    {
        PFlame.GetComponent<ParticleSystem>().Play();
        PSmoke.GetComponent<ParticleSystem>().Play();
        PSpark.GetComponent<ParticleSystem>().Play();
    }
    
    void DisableFlame()
    {
        PFlame.GetComponent<ParticleSystem>().Stop();
        PSmoke.GetComponent<ParticleSystem>().Stop();
        PSpark.GetComponent<ParticleSystem>().Stop();
        audioSource.Stop();
    }
}
