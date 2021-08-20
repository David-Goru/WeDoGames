using UnityEngine;

public class ASPlayStopParticles : MonoBehaviour, ITurretAttackState
{
    [SerializeField] bool stopOnExit = false;
    [SerializeField] ParticleSystem particles = null;

    public void OnAttackEnter()
    {
        particles.Play();
    }

    public void OnAttackExit()
    {
        if (stopOnExit) particles.Stop();
    }
}
