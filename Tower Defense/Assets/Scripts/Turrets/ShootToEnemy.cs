using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToEnemy : MonoBehaviour
{
    [SerializeField] Pool projectile;
    [SerializeField] Transform spawnPosition;
    ObjectPooler objectPooler;
    GameObject obj;

    float attackRate = 0;

    float timer = 0;

    private void Awake()
    {
        objectPooler = ObjectPooler.GetInstance();
    }

    public void SetAttackRate(float attackRate)
    {
        this.attackRate = attackRate;
    }

    public void ShootEnemy(Transform enemy, float damage, TurretBehaviour turret)
    {
        if(timer >= attackRate)
        {
            obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
            obj.GetComponent<BasicProjectile>().SetInfo(enemy, damage, turret);
            ResetTimer();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        timer = 0;
    }
}
