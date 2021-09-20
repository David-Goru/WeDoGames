using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}