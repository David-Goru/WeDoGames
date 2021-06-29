using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToEnemy : EffectComponent
{
    [SerializeField] float rotationSpeed = 300f;
    [SerializeField] Transform objectToRotate = null;
    [SerializeField] CurrentTargetsOnRange enemyDetection = null;

    public Transform ObjectToRotate { get { return objectToRotate; } set { objectToRotate = value; } }

    public override void InitializeComponent()
    {
    }

    public override void UpdateComponent()
    {
        RotateToEnemy();
    }

    public void RotateToEnemy()
    {
        if(ReferenceEquals(enemyDetection, null))
        {
            Debug.LogError("You don't have an enemy detector on " + transform.gameObject.name);
            return;
        }
        if (!isEnemyTargetable()) return;

        //Create a variable with the same enemy position but putting the same y than the turret so it only rotates on the y-axis.
        Vector3 enemyPosition = new Vector3(enemyDetection.CurrentTargets[0].position.x, objectToRotate.position.y, enemyDetection.CurrentTargets[0].position.z);

        //Create a Quaternion with the final rotation of the turret looking at the enemy
        Quaternion lookRotation = Quaternion.LookRotation(enemyPosition - objectToRotate.position);

        //Interpolate the rotation of the turret towards the final rotation looking at the enemy
        objectToRotate.rotation = Quaternion.RotateTowards(objectToRotate.rotation, lookRotation, rotationSpeed * Time.deltaTime);

    }

    bool isEnemyTargetable()
    {
        return (enemyDetection.CurrentTargets.Count > 0 && enemyDetection.CurrentTargets[0] != null);
    }
}
