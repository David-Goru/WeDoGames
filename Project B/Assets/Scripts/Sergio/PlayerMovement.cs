using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float smoothRotation;

    IMovementInput input;

    Quaternion newRotation;
    Vector3 heading;

    void Start()
    {
        input = GetComponent<IMovementInput>();
    }

    void Update()
    {
        if (input.moveInputRecieved()) movePlayer();
    }

    private void movePlayer()
    {
        calculateMoveDirection();

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothRotation * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void calculateMoveDirection()
    {
        heading = Vector3.Normalize(input.horizontalDirection() + input.verticalDirection());
        newRotation = Quaternion.LookRotation(heading);
    }
}
