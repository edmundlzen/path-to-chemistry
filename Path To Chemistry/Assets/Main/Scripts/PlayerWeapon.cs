using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public Transform cameraTransform;
    public GameObject laserPrefab;

    private GameObject spawnedLaser;
    // Start is called before the first frame update
    void Start()
    {
        spawnedLaser = Instantiate(laserPrefab, firePoint.transform);
        spawnedLaser.transform.rotation.SetLookRotation(cameraTransform.forward);
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseLaser(RaycastHit hit, Transform playerTransform)
    {
        ActivateLaser(hit.transform);
        if (hit.transform.TryGetComponent(out Suckable suckable))
        {
            suckable.Suck(hit, playerTransform);
        }
    }

    void ActivateLaser(Transform hitTransform)
    {
        spawnedLaser.SetActive(true);
        UpdateLaser(hitTransform);
    }

    void UpdateLaser(Transform hitTransform)
    {
        if (firePoint != null)
        {
            spawnedLaser.transform.position = firePoint.transform.position;
            // spawnedLaser.transform.LookAt(hitTransform);
        }
    }

    public void DisableLaser()
    {
        spawnedLaser.SetActive(false);
    }
}
