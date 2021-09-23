using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    public Transform _attractorTransform;
    private readonly ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[1000];
    public GameObject marker;

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    void Awake()
    {
        // _attractorTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Particle System"))
            {
                particleSystems.Add(child.GetComponent<ParticleSystem>());
            }
        }
    }

    public void LateUpdate()
    {
        foreach (var _particleSystem in particleSystems) {
            if (_particleSystem.isPlaying && _particleSystem != null)
            {
                var length = _particleSystem.GetParticles(_particles);
                var attractorPosition = _attractorTransform.position;

                for (var i = 0; i < length; i++)
                {
                    _particles[i].position = _particles[i].position + (attractorPosition - _particles[i].position) /
                        _particles[i].remainingLifetime * Time.deltaTime;
                }
                
                _particleSystem.SetParticles(_particles, length);
            }
        }
    }
}