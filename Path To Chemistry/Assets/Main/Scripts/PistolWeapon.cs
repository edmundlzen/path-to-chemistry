using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PistolWeapon: MonoBehaviour, IUsable
{
    public int damage = 5;
    public float fireRate = 0.5f;
    
    private Transform playerCamera;
    private GameObject firePoint;
    private GameObject PSpark;
    private float lastShot;

    private bool gunEnabled;

    void Awake()
    {
        firePoint = transform.Find("Fire Point").gameObject;
        PSpark = firePoint.transform.Find("PSpark").gameObject;
        lastShot = Time.time;
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
            RaycastHit hit;
            if (Time.time - lastShot > fireRate) EnableSparks();
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit) && (Time.time - lastShot) > fireRate)
            {
                lastShot = Time.time;
                new Task(DrawSmoke(hit.point));
                if (hit.transform.TryGetComponent(out IEntity entity))
                {
                    entity.Attacked(damage);
                }
            }
        }
        else
        {
            DisableSparks();
        }

        gunEnabled = false;
    }

    void EnableSparks()
    {
        PSpark.GetComponent<ParticleSystem>().Play();
    }
    
    void DisableSparks()
    {
        // PSpark.GetComponent<ParticleSystem>().Stop();
    }

    IEnumerator DrawSmoke(Vector3 hitPoint)
    {
        GameObject newEmpty = new GameObject("Smoke " + Time.time);
        newEmpty.AddComponent<LineRenderer>();
        LineRenderer newLineRenderer = newEmpty.GetComponent<LineRenderer>();
        newLineRenderer.startWidth = 0.1f;
        newLineRenderer.material = Resources.Load<Material>("Materials/Gun Trail");
        newLineRenderer.SetPosition(0, firePoint.transform.position);
        newLineRenderer.SetPosition(1, hitPoint);
        yield return new WaitForSeconds(0.2f);
        Destroy(newEmpty);
    }
}