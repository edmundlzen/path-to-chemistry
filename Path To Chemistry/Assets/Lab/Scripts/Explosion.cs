using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float Delay = 5f;
    bool Exploded = false;

    void Start()
    {
        //gameObject.GetComponent<Animator>().Play("Component1");
        //gameObject.GetComponent<Animator>().Play("Component2");
    }
    void Update()
    {
        Delay -= Time.deltaTime;
        if (Delay <= 0 && !Exploded)
        {
            Explode();
            Exploded = true;
        }
    }
    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
