using UnityEngine;

public class PlayerMovement : MonoBehaviour, ILoadable
{
    [SerializeField] private float speed;
    [SerializeField] private float smoothRotation;

    private IMovementInput input;

    private Quaternion newRotation;
    private Vector3 heading;

    private bool isCreated; //Temporal

    private void Start()
    {
        input = GetComponent<IMovementInput>();
    }

    public void Create()
    {
        isCreated = true;
    }

    private void Update()
    {
        if (isCreated)
        {
            if (input.MoveInputRecieved()) movePlayer();
        }
    }

    private void movePlayer()
    {
        calculateMoveDirection();

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothRotation * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void calculateMoveDirection()
    {
        heading = Vector3.Normalize(input.HorizontalDirection() + input.VerticalDirection());
        newRotation = Quaternion.LookRotation(heading);
    }
}