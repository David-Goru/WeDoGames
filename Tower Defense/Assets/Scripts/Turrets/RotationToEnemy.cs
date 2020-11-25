using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToEnemy : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;

    /// <summary>
    /// Rotate the turret towards the enemy
    /// </summary>
    /// <param name="enemy"> the transform of the enemy</param>
    public void RotateToEnemy(Transform enemy)
    {
        float xRot = transform.rotation.eulerAngles.x; 
        float zRot = transform.rotation.eulerAngles.z;
        Vector3 enemyPosition = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(enemyPosition - transform.position);
        //this.transform.LookAt(enemy);
        Quaternion rotateTowards = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotateTowards;
        //transform.rotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, new Vector3(xRot, rotateTowards.eulerAngles.y, zRot));
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50f);
    }
}
