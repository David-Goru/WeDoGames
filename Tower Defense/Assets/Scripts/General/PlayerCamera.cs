using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    bool isRotating = false;

    void Update()
    {
        if (Input.GetMouseButton(2) && Mathf.Abs(Input.GetAxis("Mouse X")) > 0.2f) rotate();
    }

    void rotate()
    {
        if (isRotating) return;

        int direction = Input.GetAxis("Mouse X") > 0 ? 1 : -1;
        StartCoroutine(snapRotation(direction));
    }

    IEnumerator snapRotation(int direction)
    {
        isRotating = true;

        int degreesLeft = 90;
        while (degreesLeft > 0)
        {
            degreesLeft--;
            int yRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.y + direction);
            transform.eulerAngles = new Vector3(30, yRotation, 0);
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(0.1f);
        isRotating = false;
    }
}