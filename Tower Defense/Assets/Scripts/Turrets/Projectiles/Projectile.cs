using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] protected float speed;

    protected TurretStats turretStats;
    protected Transform target;
    protected Transform turret;
    protected IEnemyDamageHandler enemyDamageHandler;

    protected ITurretDamage turretDamageable;

    public virtual void SetInfo(Transform target, Transform turret, TurretStats turretStats, IEnemyDamageHandler enemyDamageHandler)
    {
        this.target = target;
        this.turretStats = turretStats;
        this.turret = turret;
        this.enemyDamageHandler = enemyDamageHandler;
    }

    void Update()
    {
        updateProjectile();
    }

    protected virtual void updateProjectile()
    {

    }

    protected virtual void OnEnemyCollision(Collider other)
    {

    }

    protected void damageEnemy(Collider enemy)
    {
        turretDamageable = enemy.GetComponent<ITurretDamage>();
        if (turretDamageable != null)
        {
            turretDamageable.OnTurretHit(turret, turretStats.SearchStatValue(StatType.DAMAGE), enemyDamageHandler);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }

    protected void disable()
    {
        ObjectPooler.GetInstance().ReturnToThePool(this.transform);
    }

    public void OnObjectSpawn()
    {

    }
}
