using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour, ILoadable
{
    [SerializeField] private List<AudioClip> walkSounds; //Basic implementation for now
    [SerializeField] private float secondsBetweenFootsteps;

    private AudioSource audioSource;
    private float seconds;

    private IMovementInput input;

    private bool isCreated; //Temporal

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        input = GetComponent<IMovementInput>();
    }

    private void Update()
    {
        if (isCreated)
        {
            seconds -= Time.deltaTime;
            if(input.MoveInputReceived() && seconds <= 0f)
            {
                audioSource.clip = walkSounds[Random.Range(0, walkSounds.Count)];
                audioSource.pitch = Random.Range(0.75f, 1.25f);
                audioSource.Play();
                seconds = secondsBetweenFootsteps;
            }
        }
    }

    public void Create()
    {
        isCreated = true;
    }
}