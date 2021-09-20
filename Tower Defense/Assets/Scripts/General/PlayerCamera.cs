using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] KeyCode rotateLeftButton = KeyCode.A;
    [SerializeField] KeyCode rotateRightButton = KeyCode.D;

    Coroutine rotating;
    int currentDirection;
    public static int currentRotation = 45;
    int rotationObjective = 45;

    private void Start()
    {
        currentRotation = 45;
    }

    void Update()
    {
        if (UI.Instance.Chat.gameObject.activeSelf) return;

        if (Input.GetMouseButton(2) && Mathf.Abs(Input.GetAxis("Mouse X")) > 0.2f) rotate(Input.GetAxis("Mouse X"));
        else if (Input.GetKey(rotateLeftButton)) rotate(-1);
        else if (Input.GetKey(rotateRightButton)) rotate(1);
    }

    public void Rotate(int direction)
    {
        rotate(direction);
    }

    void rotate(float direction)
    {
        UI.Instance.EntityInfo.HideUI();
        int newDirection = direction > 0 ? -1 : 1;

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
            int yRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.y + direction * 2);
            transform.eulerAngles = new Vector3(30, yRotation, 0);
            currentRotation += direction * 2;
            if (currentRotation > 360) currentRotation -= 360;
            foreach (GameObject turret in Master.Instance.ActiveTurrets)
            {
                turret.GetComponentInChildren<RectTransform>().Rotate(-Vector3.forward * direction * 2);
            }
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(0.1f);
        currentDirection = 0;
    }
}