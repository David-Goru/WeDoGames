using UnityEngine;

public class MoveX : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    bool isMoving = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isMoving = true;
            Time.timeScale = 0.5f;
        }
        else if (Input.GetKeyDown(KeyCode.D)) isMoving = false;
        if (isMoving)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        }
    }
}