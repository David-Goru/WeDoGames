using UnityEngine;

public class EstoSeBorra3 : MonoBehaviour
{
    bool isRotating = false;
    [SerializeField] float speed = 60f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isRotating = true;
        }
        if (isRotating)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
