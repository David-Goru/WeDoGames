﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SporeProjectile : Projectile
{
    [SerializeField] LayerMask enemyLayer;
    Action onDisableAction;
    ITargetsDetector targetsDetector;
    bool hasTarget = false;

    void Awake()
    {
        targetsDetector = GetComponent<ITargetsDetector>();
    }

    public override void SetInfo(Transform target, Transform turret, TurretStats turretStats, IEnemyDamageHandler enemyDamageHandler)
    {
        base.SetInfo(target, turret, turretStats, enemyDamageHandler);
        hasTarget = false;
    }

    public override void SetInfo(Transform turret, TurretStats turretStats)
    {
        base.SetInfo(turret, turretStats);
        hasTarget = false;
    }

    public void SetSpawnerInfo(Action onDisableAction)
    {
        this.onDisableAction = onDisableAction;
    }

    protected override void updateProjectile()
    {
        if (!hasTarget)
        {
            rotateAroundTurret();
            detectEnemies();
        }
        else chaseEnemy();
    }

    private void detectEnemies()
    {
        float range = turretStats.GetStatValue(StatType.ATTACKRANGE);
        List<Transform> targets = targetsDetector.GetTargets(range, enemyLayer);
        if(targets.Count > 0)
        {
            Transform nearestTarget = getNearestTarget(targets);
            target = nearestTarget;
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
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        bool isEnemy = LayerMaskUtilities.ContainsLayer(enemyLayer, other.gameObject.layer);
        if (isEnemy) OnEnemyCollision(other);
    }

    protected override void OnEnemyCollision(Collider other)
    {
        damageEnemy(other);
        disable();
    }

    private void rotateAroundTurret()
    {
        transform.RotateAround(turret.position, Vector3.up, speed * Time.deltaTime);
    }
}