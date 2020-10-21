using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour, IPooledObject
{
    [SerializeField] float speed;

    float damage;
    Transform target;
    TurretBehaviour turret;

    ITurretDamage turretDamageable;

    public void SetInfo(Transform target, float damage, TurretBehaviour turret)
    {
        this.target = target;
        this.damage = damage;
        this.turret = turret;
    }

    void Update()
    {
        if (!target.gameObject.activeSelf) //If enemy has died before projectile reaches it
            disable();
        ChaseEnemy();
    }

    void ChaseEnemy()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform == target) //If projectile collides with the enemy is chasing
        {
            damageEnemy(other);
            disable();
        }
    }

    void damageEnemy(Collider enemy)
    {
        turretDamageable = enemy.GetComponent<ITurretDamage>();
        if (turretDamageable != null)
        {
            turretDamageable.OnTurretHit(turret.transform, damage, (IEnemyDamage)turret);
        }
    }

    void disable()
    {
        ObjectPooler.GetInstance().ReturnToThePool(this.transform);
    }

    public void OnObjectSpawn()
    {

    }
}
