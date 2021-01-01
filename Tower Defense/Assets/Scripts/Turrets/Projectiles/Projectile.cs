using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] protected float speed;

    protected float damage;
    protected Transform target;
    protected BuildingRange turretBuildingRange;
    protected IEnemyDamageHandler enemyDamageHandler;

    protected ITurretDamage turretDamageable;

    public virtual void SetInfo(Transform target, BuildingRange turretBuildingRange, float damage, IEnemyDamageHandler enemyDamageHandler)
    {
        this.target = target;
        this.damage = damage;
        this.turretBuildingRange = turretBuildingRange;
        this.enemyDamageHandler = enemyDamageHandler;
    }

    void Update()
    {
        updateProjectile();
    }

    protected virtual void updateProjectile()
    {

    }

    protected void damageEnemy(Collider enemy)
    {
        turretDamageable = enemy.GetComponent<ITurretDamage>();
        if (turretDamageable != null)
        {
            turretDamageable.OnTurretHit(turretBuildingRange, damage, enemyDamageHandler);
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
