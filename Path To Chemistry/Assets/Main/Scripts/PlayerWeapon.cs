using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public Transform cameraTransform;
    public GameObject laserHead;
    public GameObject laserFizz;

    public AudioSource laserSound;

    // Start is called before the first frame update
    void Start()
    {
        // spawnedLaser.transform.rotation.SetLookRotation(cameraTransform.forward);
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseLaser(RaycastHit hit, Transform playerTransform)
    {
        laserHead.transform.LookAt(hit.point);
        ActivateLaser(hit.transform);
        laserSound.Play();
        if (hit.transform.TryGetComponent(out Suckable suckable))
        {
            suckable.Suck(hit, playerTransform);
        }
    }

    void ActivateLaser(Transform hitTransform)
    {
        laserHead.SetActive(true);
        laserFizz.GetComponent<ParticleSystem>().Play();
        // UpdateLaser(hitTransform);
    }

    void UpdateLaser(Transform hitTransform)
    {
        if (firePoint != null)
        {
            laserHead.transform.position = firePoint.transform.position;
            // spawnedLaser.transform.LookAt(hitTransform);
        }
    }

    public void DisableLaser()
    {
        laserHead.SetActive(false);
        laserFizz.GetComponent<ParticleSystem>().Stop();
        laserSound.Stop();
    }
}
