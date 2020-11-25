using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToEnemy : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 300f;

    /// <summary>
    /// Rotate the turret towards the enemy
    /// </summary>
    /// <param name="enemy"> the transform of the enemy</param>
    public void RotateToEnemy(Transform enemy)
    {
        //Create a variable with the same enemy position but putting the same y than the turret so it only rotates on the y-axis.
        Vector3 enemyPosition = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);

        //Create a Quaternion with the final rotation of the turret looking at the enemy
        Quaternion lookRotation = Quaternion.LookRotation(enemyPosition - transform.position);

        //Interpolate the rotation of the turret towards the final rotation looking at the enemy
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

    }
}
