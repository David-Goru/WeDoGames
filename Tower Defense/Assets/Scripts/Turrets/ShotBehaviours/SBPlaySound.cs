using UnityEngine;

public class SBPlaySound : MonoBehaviour, ITurretShotBehaviour
{
    [SerializeField] AudioSource audioSource = null;

    public void OnShot()
    {
        audioSource.Play();
    }
}