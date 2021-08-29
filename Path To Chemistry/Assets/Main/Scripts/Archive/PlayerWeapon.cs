using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private float t = 0;
    public float duration = 1;
    private bool shrinkBeam = false;
    
    public Transform firePoint;
    public GameObject laserHead;
    public GameObject laserFizz;
    public GameObject laserBeam;

    public AudioSource laserSound;

    // Start is called before the first frame update
    void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (shrinkBeam && laserBeam.GetComponent<LineRenderer>().GetPosition(1).z <= 0.1f)
        {
            shrinkBeam = false;
            laserBeam.SetActive(false);
            laserHead.SetActive(false);
            t = 0f;
        } else if (shrinkBeam)
        {
            t += Time.deltaTime / duration;
            float zDistance = laserBeam.GetComponent<LineRenderer>().GetPosition(1).z;
            float newZDistance = 0;
            float lerpedZDistance = Mathf.Lerp(zDistance, newZDistance, t);
            laserBeam.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0,0, lerpedZDistance));
        }
    }

    public void UseLaser(RaycastHit hit, Transform playerTransform)
    {
        laserHead.transform.LookAt(hit.point);
        ActivateLaser(hit);
        laserSound.Play();
        if (hit.transform.TryGetComponent(out Suckable suckable))
        {
            suckable.Suck(hit, playerTransform);
        }
    }

    void ActivateLaser(RaycastHit raycastHit)
    {
        shrinkBeam = false;
        t += Time.deltaTime / duration;
        float zDistance = laserBeam.GetComponent<LineRenderer>().GetPosition(1).z;
        float newZDistance = Vector3.Distance(laserBeam.transform.position, raycastHit.point);
        float lerpedZDistance = Mathf.Lerp(zDistance, newZDistance, t);
        laserBeam.SetActive(true);
        laserHead.SetActive(true);
        laserBeam.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0,0, lerpedZDistance));
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
        t = 0f;
        shrinkBeam = true;
        laserFizz.GetComponent<ParticleSystem>().Stop();
        laserSound.Stop();
    }
}
