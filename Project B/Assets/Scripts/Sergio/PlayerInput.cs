using UnityEngine;

public class PlayerInput : MonoBehaviour, IMovementInput
{
    Vector3 forward, right;

    void Start()
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

    public bool moveInputRecieved()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }

    public Vector3 horizontalDirection()
    {
        return right * Input.GetAxis("Horizontal");
    }

    public Vector3 verticalDirection()
    {
        return forward * Input.GetAxis("Vertical");
    }
}
