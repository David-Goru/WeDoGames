using UnityEngine;

public class ASPlayStopSound : MonoBehaviour, ITurretAttackState
{
    [SerializeField] bool stopOnExit = false;
    [SerializeField] AudioSource audioSource = null;

    public void OnAttackEnter()
    {
        audioSource.Play();
    }

    public void OnAttackExit()
    {
        if(stopOnExit) audioSource.Stop();
    }
}
