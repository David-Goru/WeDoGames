using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] protected float speed;

    protected float damage;
    protected Transform target;
    protected Turret turret;

    protected ITurretDamage turretDamageable;

    public virtual void SetInfo(Transform target, float damage, Turret turret)
    {
        this.target = target;
        this.damage = damage;
        this.turret = turret;
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
            turretDamageable.OnTurretHit(turret.transform, damage, (IEnemyDamage)turret);
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
