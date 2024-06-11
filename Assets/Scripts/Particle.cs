using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleSystem[] _particleSystems;
    private Color _particleColor;
    private bool isPieceParticles = false;

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

            if (isPieceParticles)
            {
                ps.GetComponent<ParticleSystemRenderer>().material.color = _particleColor;
            }

            ps.Play();
            Destroy(ps.gameObject, duration);
            maxDuration = duration > maxDuration ? duration : maxDuration;
        }

        Destroy(gameObject, maxDuration);
    }

    public void SetPieceSpriteColor(Color color)
    {
        _particleColor = color;
        isPieceParticles = true;
    }
}
