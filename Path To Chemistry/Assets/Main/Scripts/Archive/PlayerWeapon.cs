using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float duration = 1;

    public Transform firePoint;
    public GameObject laserHead;
    public GameObject laserFizz;
    public GameObject laserBeam;

    public AudioSource laserSound;
    private bool shrinkBeam;
    private float t;

    // Start is called before the first frame update
    private void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    private void Update()
    {
        if (shrinkBeam && laserBeam.GetComponent<LineRenderer>().GetPosition(1).z <= 0.1f)
        {
            shrinkBeam = false;
            laserBeam.SetActive(false);
            laserHead.SetActive(false);
            t = 0f;
        }
        else if (shrinkBeam)
        {
            t += Time.deltaTime / duration;
            var zDistance = laserBeam.GetComponent<LineRenderer>().GetPosition(1).z;
            float newZDistance = 0;
            var lerpedZDistance = Mathf.Lerp(zDistance, newZDistance, t);
            laserBeam.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, lerpedZDistance));
        }
    }

    public void UseLaser(RaycastHit hit, Transform playerTransform)
    {
        laserHead.transform.LookAt(hit.point);
        ActivateLaser(hit);
        laserSound.Play();
        if (hit.transform.TryGetComponent(out Suckable suckable)) suckable.Suck(hit, playerTransform);
    }

    private void ActivateLaser(RaycastHit raycastHit)
    {
        shrinkBeam = false;
        t += Time.deltaTime / duration;
        var zDistance = laserBeam.GetComponent<LineRenderer>().GetPosition(1).z;
        var newZDistance = Vector3.Distance(laserBeam.transform.position, raycastHit.point);
        var lerpedZDistance = Mathf.Lerp(zDistance, newZDistance, t);
        laserBeam.SetActive(true);
        laserHead.SetActive(true);
        laserBeam.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, lerpedZDistance));
        laserFizz.GetComponent<ParticleSystem>().Play();
        // UpdateLaser(hitTransform);
    }

    private void UpdateLaser(Transform hitTransform)
    {
        if (firePoint != null)
            laserHead.transform.position = firePoint.transform.position;
        // spawnedLaser.transform.LookAt(hitTransform);
    }

    public void DisableLaser()
    {
        t = 0f;
        shrinkBeam = true;
        laserFizz.GetComponent<ParticleSystem>().Stop();
        laserSound.Stop();
    }
}