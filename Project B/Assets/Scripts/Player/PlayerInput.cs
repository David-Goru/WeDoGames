using UnityEngine;

public class PlayerInput : MonoBehaviour, IMovementInput
{
    private Vector3 forward, right;

    private void Start()
    {
        calculateInputFromCamera();
    }

    private void calculateInputFromCamera()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * forward;
    }

    public bool MoveInputReceived()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }

    public Vector3 HorizontalDirection()
    {
        return right * Input.GetAxis("Horizontal");
    }

    public Vector3 VerticalDirection()
    {
        return forward * Input.GetAxis("Vertical");
    }
}