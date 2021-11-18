using UnityEngine;

public class PlayerMovement : MonoBehaviour, ILoadable
{
    [SerializeField] private float speed;
    [SerializeField] private float smoothRotation;

    private IMovementInput input;

    private Quaternion newRotation;
    private Vector3 heading;
    private Vector3 lastFrameMovement;

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
        if (isCreated) checkForPlayerInput();
    }

    private void FixedUpdate()
    {
        if (isMoving()) movePlayer();
    }

    private void checkForPlayerInput()
    {
        if (input.MoveInputReceived()) calculateLastFramePos();
        else if (lastFrameMovement != Vector3.zero) lastFrameMovement = Vector3.zero;
    }

    private void calculateLastFramePos()
    {
        Vector3 movementDirectionAndMagnitude = input.HorizontalDirection() + input.VerticalDirection();
        lastFrameMovement = Vector3.ClampMagnitude(movementDirectionAndMagnitude, 1.0f);
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

    private bool isMoving()
    {
        return lastFrameMovement != Vector3.zero;
    }
}