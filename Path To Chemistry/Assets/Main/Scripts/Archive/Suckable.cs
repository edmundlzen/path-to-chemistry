using UnityEngine;

public class Suckable : MonoBehaviour
{
    public float HP = 1f;
    private new Transform particleSystem;
    private float startHP;

    // Start is called before the first frame update
    private void Start()
    {
        particleSystem = transform.Find("Particle System");
        SetParticles();
        transform.GetComponent<Renderer>().material = new Material(transform.GetComponent<Renderer>().material);
        startHP = HP;
    }

    public void Suck(RaycastHit hit, Transform playerTransform)
    {
        if (HP <= 0f) Destroy(gameObject);

        if (particleSystem.GetComponent<ParticleSystem>().time > .1f)
            particleSystem.GetComponent<ParticleSystem>().time = 0;
        particleSystem.GetComponent<ParticleSystem>().Play();
        particleSystem.LookAt(playerTransform);

        HP -= 0.3f * Time.deltaTime;
        var newThreshold = startHP - HP;
        GetComponent<Renderer>().material.SetFloat("Alpha_Clip_Threshold", newThreshold);
    }

    private void SetParticles()
    {
        // particleSystem.gameObject.SetActive(true);
        particleSystem.GetComponent<ParticleSystemRenderer>().mesh =
            transform.GetComponent<MeshFilter>().mesh;
        particleSystem.GetComponent<ParticleSystemRenderer>().material =
            transform.GetComponent<Renderer>().material;
    }
}