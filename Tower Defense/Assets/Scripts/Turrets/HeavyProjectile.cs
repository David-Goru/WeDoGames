using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyProjectile : Projectile
{
    Vector3 lastEnemyPosition;

    public override void SetInfo(Transform target, float damage, TurretBehaviour turret)
    {
        base.SetInfo(target, damage, turret);
        lastEnemyPosition = target.position;
    }

    protected override void updateProjectile()
    {
        base.updateProjectile();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
