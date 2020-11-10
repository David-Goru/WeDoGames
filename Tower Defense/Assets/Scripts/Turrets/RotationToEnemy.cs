using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToEnemy : MonoBehaviour
{
    public void RotateToEnemy(Transform enemy)
    {
        float xRot = transform.rotation.eulerAngles.x; 
        float zRot = transform.rotation.eulerAngles.z;
        this.transform.LookAt(enemy);
        transform.rotation = Quaternion.Euler(xRot, transform.rotation.eulerAngles.y, zRot);
    }
}
