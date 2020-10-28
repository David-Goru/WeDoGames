using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{

    protected override void updateProjectile() //Function called on Update
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

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.transform == target) //If projectile collides with the enemy is chasing
        {
            damageEnemy(other);
            disable();
        }
    }

}
