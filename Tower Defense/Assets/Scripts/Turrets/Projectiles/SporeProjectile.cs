using System;
using System.Collections.Generic;
using UnityEngine;

public class SporeProjectile : Projectile
{
    [SerializeField] LayerMask enemyLayer = 0;
    [SerializeField] float rotationSpeed = 1f;
    Action onDisableAction;
    ITargetsDetector targetsDetector;
    bool hasTarget = false;
    bool isTargetAlive = true;

    Vector3 lastEnemyPos = Vector3.zero;
    Collider enemyCol;

    void Awake()
    {
        targetsDetector = GetComponent<ITargetsDetector>();
    }

    protected override void initializate()
    {
        hasTarget = false;
        isTargetAlive = true;
        lastEnemyPos = Vector3.zero;
    }

    public void SetSpawnerInfo(Action onDisableAction)
    {
        this.onDisableAction = onDisableAction;
    }

    protected override void updateProjectile()
    {
        if (!turret.gameObject.activeSelf) disable();
        if (!hasTarget)
        {
            rotateAroundTurret();
            detectEnemies();
        }
        else
        {
            if (!target.gameObject.activeSelf) isTargetAlive = false;
            goToTarget();
        }
    }

    private void goToTarget()
    {
        if (isTargetAlive)
        {
            chaseEnemy();
            lastEnemyPos = enemyCol.bounds.center;
        }
        else
        {
            goToLastEnemyPos();
            if (Vector3.Distance(transform.position, lastEnemyPos) <= 0.25f)
            {
                damageSurroundingEnemies(null);
                disable();
                onDisableAction.Invoke();
            }
        }
    }

    private void detectEnemies()
    {
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        List<Transform> targets = targetsDetector.GetTargets(range, enemyLayer);
        targets.RemoveAll((Transform enemy) => TurretUtilities.IsEnemyDying(enemy));
        if (targets.Count > 0)
        {
            Transform nearestTarget = getNearestTarget(targets);
            target = nearestTarget;
            enemyCol = target.GetComponent<Collider>();
            hasTarget = true;
        }
    }

    private Transform getNearestTarget(List<Transform> targets)
    {
        float minDistance = float.MaxValue;
        Transform closestTarget = null;
        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    void chaseEnemy()
    {
        transform.LookAt(enemyCol.bounds.center);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void goToLastEnemyPos()
    {
        transform.position = Vector3.MoveTowards(transform.position, lastEnemyPos, speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform == target) OnEnemyCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == target) OnEnemyCollision(other);
    }

    protected override void OnEnemyCollision(Collider other)
    {
        damageEnemy(other);
        damageSurroundingEnemies(other.transform);
        disable();
        onDisableAction.Invoke();
    }

    private void damageSurroundingEnemies(Transform mainEnemyTransform)
    {
        float range = turretStats.GetStatValue(StatType.PROJECTILEEXPLOSIONRADIUS);
        int damage = (int)turretStats.GetStatValue(StatType.REDUCEDDAMAGE);
        List<Transform> targets = targetsDetector.GetTargets(range, enemyLayer);
        foreach (Transform target in targets)
        {
            if (target != mainEnemyTransform) target.GetComponent<ITurretDamage>().OnTurretHit(turret, damage, enemyDamageHandler);
        }
    }

    private void rotateAroundTurret()
    {
        transform.RotateAround(turret.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugRange = 2f;
    [SerializeField] float debugExplosionRange = 1f;
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (turretStats != null)
        {
            debugRange = turretStats.GetStatValue(StatType.ATTACKRANGE);
            debugExplosionRange = turretStats.GetStatValue(StatType.PROJECTILEEXPLOSIONRADIUS);
        }
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, debugExplosionRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////
}