using System;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackProjectile : Projectile
{
    Vector3 moveDirection;
    int remainingObjectives = 0;
    List<Collider> enemiesCollided = new List<Collider>();
    float timer = 0f;
    float lifeSpan = 0f;

    public override void SetInfo(Transform target, Transform turret, TurretStats turretStats, IEnemyDamageHandler enemyDamageHandler)
    {
        base.SetInfo(target, turret, turretStats, enemyDamageHandler);
        calculateDirection();
        initMembers();
    }

    void calculateDirection()
    {
        Vector3 enemyDirection = (target.position - transform.position).normalized;
        moveDirection = new Vector3(enemyDirection.x, 0, enemyDirection.z).normalized;
    }

    void initMembers()
    {
        lifeSpan = turretStats.GetStatValue(StatType.EFFECTDURATION);
        timer = 0f;
        remainingObjectives = (int)turretStats.GetStatValue(StatType.OBJECTIVESTOHIT);
        enemiesCollided.Clear();
    }

    protected override void updateProjectile()
    {
        moveTowardsDirection();
        checkLifeSpan();
    }

    private void checkLifeSpan()
    {
        if (timer >= lifeSpan)
            disable();
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void moveTowardsDirection()
    {
        float speed = turretStats.GetStatValue(StatType.PROJECTILESPEED);
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !enemiesCollided.Contains(other))
        {
            OnEnemyCollision(other);
        }
    }

    protected override void OnEnemyCollision(Collider enemy)
    {
        enemiesCollided.Add(enemy);
        damageEnemy(enemy);
        knockbackEnemy(enemy);
        CheckRemainingObjectives();
    }

    void CheckRemainingObjectives()
    {
        remainingObjectives--;
        if (remainingObjectives <= 0)
            disable();
    }

    void knockbackEnemy(Collider enemy)
    {
        float knockbackDistance = turretStats.GetStatValue(StatType.KNOCKBACKDISTANCE);
        enemy.GetComponent<IKnockbackable>().Knockback(knockbackDistance, moveDirection);
    }
}
