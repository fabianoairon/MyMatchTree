using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleSystem[] _particleSystems;

    private void Start()
    {
        PlayParticleSystem();
    }

    private void PlayParticleSystem()
    {
        float maxDuration = 0;
        foreach (var ps in _particleSystems)
        {
            ParticleSystem.MainModule mainModule = ps.main;
            float duration = mainModule.startDelay.constant + mainModule.duration;

            ps.Play();
            Destroy(ps.gameObject, duration);
            maxDuration = duration > maxDuration ? duration : maxDuration;

        }

        Destroy(gameObject, maxDuration);
    }
}
