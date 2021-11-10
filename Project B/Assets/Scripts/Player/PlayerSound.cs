using UnityEngine;

public class PlayerSound : MonoBehaviour, ILoadable
{
    private AudioSource walkSound; //Basic implementation for now

    [SerializeField] private float secondsBetweenFootsteps;
    private float seconds;

    private IMovementInput input;

    private bool isCreated; //Temporal

    private void Start()
    {
        walkSound = GetComponentInChildren<AudioSource>();
        input = GetComponent<IMovementInput>();
    }

    private void Update()
    {
        if (isCreated)
        {
            seconds -= Time.deltaTime;
            if(input.MoveInputRecieved() && seconds <= 0f)
            {
                walkSound.Play();
                seconds = secondsBetweenFootsteps;
            }
        }
    }

    public void Create()
    {
        isCreated = true;
    }
}