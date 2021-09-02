using System;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : BasicProjectile
{
    [SerializeField] LayerMask enemyLayer = 0;
    List<Transform> enemiesCollided = new List<Transform>();
    ITargetsDetector targetsDetector;

    Vector3 lastEnemyPos = Vector3.zero;
    bool isTargetAlive = true;
    int nEnemiesCollided = 0;

    private void Awake()
    {
        targetsDetector = GetComponent<ITargetsDetector>();
    }

    protected override void initializate()
    {
        base.initializate();
        enemiesCollided.Clear();
        isTargetAlive = true;
        nEnemiesCollided = 0;
    }

    protected override void updateProjectile()
    {
        isTargetAlive = target.gameObject.activeSelf;
        if (isTargetAlive)
        {
            chaseEnemy();
            lastEnemyPos = enemyCol.bounds.center;
        }
        else
        {
            goToLastEnemyPos();
            checkDistanceToLastEnemyPos();
        }
    }

    void checkDistanceToLastEnemyPos()
    {
        float distanceToEnemy = Vector3.Distance(transform.position, lastEnemyPos);
        if (distanceToEnemy <= 0.25f) checkIfCanSearchAnotherTarget();
    }

    void goToLastEnemyPos()
    {
        transform.position = Vector3.MoveTowards(transform.position, lastEnemyPos, speed * Time.deltaTime);
    }

    protected override void OnEnemyCollision(Collider enemy)
    {
        damageEnemy(enemy);
        enemiesCollided.Add(enemy.transform);
        checkIfCanSearchAnotherTarget();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == target) OnEnemyCollision(other);
    }

    private void checkIfCanSearchAnotherTarget()
    {
        nEnemiesCollided++;
        int maxObjectivesToHit = (int)turretStats.GetStatValue(StatType.OBJECTIVESTOHIT);
        if (nEnemiesCollided < maxObjectivesToHit) searchAnotherTarget();
        else disable();
    }

    private void searchAnotherTarget()
    {
        float range = turretStats.GetStatValue(StatType.PROJECTILERANGE);
        List<Transform> targets = targetsDetector.GetTargets(range, enemyLayer);
        Transform target = getNearestTarget(targets);
        if (target != null) 
        { 
            this.target = target;
            enemyCol = target.GetComponent<Collider>();
            lastEnemyPos = enemyCol.bounds.center;
            isTargetAlive = true;
        }   
        else disable();
    }

    private Transform getNearestTarget(List<Transform> targets)
    {
        float minDistance = float.MaxValue;
        Transform closestTarget = null;
        foreach (Transform target in targets)
        {
            if (enemiesCollided.Contains(target)) continue;
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    ////////////////////////////DEBUG////////////////////////////////////
    [SerializeField] float debugRange = 2f;
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (turretStats != null) debugRange = turretStats.GetStatValue(StatType.PROJECTILERANGE);
        Gizmos.DrawWireSphere(this.transform.position, debugRange);
    }
    ////////////////////////////DEBUG////////////////////////////////////

}
