using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollectorWeapon : MonoBehaviour, IUsable
{
    public int damage = 5;
    public float fireRate = 0.1f;
    public AudioClip laserSound;
    public AudioSource audioSource;
    private Transform laserComponents;
    private Transform laserFizz;
    private Transform laserHead;
    private Transform laserBeam;
    private float lastShot;

    private Transform playerCamera;

    private bool gunEnabled;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserSound;
        laserComponents = transform.Find("R_Head").Find("Laser");
        laserFizz = laserComponents.Find("Laser Fizz");
        laserHead = laserComponents.Find("Laser Head");
        laserBeam = laserComponents.Find("Laser Beam");
    }
    
    public void Use(Transform pCamera)
    {
        gunEnabled = true;
        playerCamera = pCamera;
    }
    
    void Update()
    {
        if (gunEnabled)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit) && (Time.time - lastShot) > fireRate)
            {
                lastShot = Time.time;
                EnableLaser(hit.point);
                if (hit.transform.TryGetComponent(out ICollectable collectable))
                {
                    collectable.Collect();
                } else if (hit.transform.TryGetComponent(out IEntity entity))
                {
                    entity.Attacked(damage);
                }
            }
        }
        else
        {
            DisableLaser();
        }

        gunEnabled = false;
    }

    void EnableLaser(Vector3 hit)
    {
        laserHead.gameObject.SetActive(true);
        
        laserBeam.gameObject.SetActive(true);
        laserBeam.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0,0 ,Vector3.Distance(laserBeam.position, hit)));
        laserBeam.LookAt(hit);
    }
    
    void DisableLaser()
    {
        laserHead.gameObject.SetActive(false);
        laserBeam.gameObject.SetActive(false);
        audioSource.Stop();
    }
}
