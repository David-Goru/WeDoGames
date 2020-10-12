using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField] TurretInfo turretInfo;
    [SerializeField] EnemyDetection enemyDetection;
    [SerializeField] RotationToEnemy rotationToEnemy;
    TurretStats turretStats;

    Transform currentEnemy;

    void Start()
    {
        turretStats = new TurretStats(turretInfo);
        enemyDetection.SetRange(turretStats.AttackRange);
    }

    void Update()
    {
        updateTarget();
        updateRotation();
    }

    void updateTarget()
    {
        currentEnemy = enemyDetection.UpdateTarget();
    }

    void updateRotation()
    {
        if (currentEnemy != null)
            rotationToEnemy.RotateToEnemy(currentEnemy);
    }
}
