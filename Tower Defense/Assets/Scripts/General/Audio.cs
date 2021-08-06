using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource = null;

    public void RunSound(AudioClip clip)
    {
        if (clip == null) Debug.Log("No clip found");
        if (audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}