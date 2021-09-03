using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    Coroutine rotating;
    int currentDirection;
    int currentRotation = 45;
    int rotationObjective = 45;

    void Update()
    {
        if (Input.GetMouseButton(2) && Mathf.Abs(Input.GetAxis("Mouse X")) > 0.2f) rotate();
    }

    void rotate()
    {
        int newDirection = Input.GetAxis("Mouse X") > 0 ? 1 : -1;

        if (newDirection == currentDirection) return;
        if (rotating != null)
        {
            StopCoroutine(rotating);
            rotating = null;
        }
        rotating = StartCoroutine(snapRotation(newDirection));
    }

    IEnumerator snapRotation(int direction)
    {
        rotationObjective = rotationObjective + 90 * direction;
        if (rotationObjective > 360) rotationObjective -= 360;
        currentDirection = direction;

        while (currentRotation != rotationObjective)
        {
            int yRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.y + direction);
            transform.eulerAngles = new Vector3(30, yRotation, 0);
            currentRotation += direction;
            if (currentRotation > 360) currentRotation -= 360;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(0.1f);
        currentDirection = 0;
    }
}