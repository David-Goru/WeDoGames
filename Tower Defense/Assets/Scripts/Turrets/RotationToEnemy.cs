using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToEnemy : MonoBehaviour
{
    public void RotateToEnemy(Transform enemy)
    {
        this.transform.LookAt(enemy);
    }
}
