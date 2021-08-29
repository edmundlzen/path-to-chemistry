using UnityEngine;
using UnityEngine.XR.iOS;

public class PointCloudParticleExample : MonoBehaviour
{
    public ParticleSystem pointCloudParticlePrefab;
    public int maxPointsToShow;
    public float particleSize = 1.0f;
    private ParticleSystem currentPS;
    private bool frameUpdated;
    private Vector3[] m_PointCloudData;
    private ParticleSystem.Particle[] particles;

    // Use this for initialization
    private void Start()
    {
        UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
        currentPS = Instantiate(pointCloudParticlePrefab);
        frameUpdated = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (frameUpdated)
        {
            if (m_PointCloudData != null && m_PointCloudData.Length > 0)
            {
                var numParticles = Mathf.Min(m_PointCloudData.Length, maxPointsToShow);
                var particles = new ParticleSystem.Particle[numParticles];
                var index = 0;
                foreach (var currentPoint in m_PointCloudData)
                {
                    particles[index].position = currentPoint;
                    particles[index].startColor = new Color(1.0f, 1.0f, 1.0f);
                    particles[index].startSize = particleSize;
                    index++;
                }

                currentPS.SetParticles(particles, numParticles);
            }
            else
            {
                var particles = new ParticleSystem.Particle[1];
                particles[0].startSize = 0.0f;
                currentPS.SetParticles(particles, 1);
            }

            frameUpdated = false;
        }
    }

    public void ARFrameUpdated(UnityARCamera camera)
    {
        m_PointCloudData = camera.pointCloudData;
        frameUpdated = true;
    }
}