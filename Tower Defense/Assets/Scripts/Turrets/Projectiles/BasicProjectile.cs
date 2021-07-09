using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{

    protected override void updateProjectile()
    {
        if (!target.gameObject.activeSelf) disable();
        chaseEnemy();
    }

    void chaseEnemy()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform == target) OnEnemyCollision(other);
    }

    protected override void OnEnemyCollision(Collider other)
    {
        damageEnemy(other);
        disable();
    }
}