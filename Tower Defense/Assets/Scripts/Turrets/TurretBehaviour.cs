using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour, IPooledObject, IEnemyDamage
{
    [SerializeField] TurretInfo turretInfo = null;
    [SerializeField] EnemyDetection enemyDetection = null;
    [SerializeField] RotationToEnemy rotationToEnemy = null;
    [SerializeField] ShootToEnemy shootToEnemy = null;
    TurretStats turretStats;

    Transform currentEnemy;

    void Start()
    {
        turretStats = new TurretStats(turretInfo);
        enemyDetection.SetRange(turretStats.AttackRange);
        shootToEnemy.SetAttackRate(turretStats.AttackRate);
    }

    void Update()
    {
        updateTarget();
        updateRotation();
        shoot();
    }

    /// <summary>
    /// Call EnemyDetection function to handle the current target of the turret
    /// </summary>
    void updateTarget()
    {
        currentEnemy = enemyDetection.UpdateTarget();
    }

    /// <summary>
    /// Call RotationToEnemy function to handle the rotation of the turret
    /// </summary>
    void updateRotation()
    {
        if (currentEnemy != null)
            rotationToEnemy.RotateToEnemy(currentEnemy);
    }

    /// <summary>
    /// Call ShootEnemy function to handle the shots of the turret
    /// </summary>
    void shoot()
    {
        if (currentEnemy != null)
            shootToEnemy.ShootEnemy(currentEnemy, turretStats.AttackDamage, this);
        else
        {
            shootToEnemy.ResetTimer();
        }
    }

    /// <summary>
    /// Called when turret spawns. It resets its values.
    /// </summary>
    public void OnObjectSpawn()
    {
        turretStats = new TurretStats(turretInfo);
        enemyDetection.SetRange(turretStats.AttackRange);
        shootToEnemy.SetAttackRate(turretStats.AttackRate);
    }

    public void OnEnemyHit(float damage)
    {
        turretStats.currentHp -= damage;
        if(turretStats.currentHp <= 0)
        {
            Disable();
        }
    }

    void Disable()
    {
        ObjectPooler.GetInstance().ReturnToThePool(transform);
    }
}
