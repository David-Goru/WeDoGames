using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    float sensitivity = 5.0f;

    void Update()
    {
        if (Input.GetMouseButton(2)) transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0), Space.World);
    }
}