using UnityEngine;

public class SBPlayParticles : MonoBehaviour, ITurretShotBehaviour
{
    [SerializeField] ParticleSystem particles = null;

    public void OnShot()
    {
        particles.Play();
    }
}
