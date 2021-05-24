using UnityEngine;

public class EstoSeBorra4 : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    bool isMoving = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            isMoving = true;
        if(isMoving)
            transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
}
