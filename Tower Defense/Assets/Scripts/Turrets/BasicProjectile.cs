using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour, IPooledObject
{
    float damage;
    Transform target;
    TurretBehaviour turret;

    public void SetInfo(Transform target, float damage, TurretBehaviour turret)
    {
        this.target = target;
        this.damage = damage;
        this.turret = turret;
    }

    void Update()
    {
        transform.LookAt(target);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.transform == target)
        {
            ObjectPooler.GetInstance().ReturnToThePool(this.transform);
        }
    }

    public void OnObjectSpawn()
    {

    }
}
