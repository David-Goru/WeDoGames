using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToEnemy : MonoBehaviour, ITurretBehaviour
{
    [SerializeField] Pool projectile = null;
    [SerializeField] Transform spawnPosition = null;
    ObjectPooler objectPooler;
    GameObject obj;

    float timer = 0;

    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    ICurrentTargetsOnRange targetDetection;

    public ICurrentTargetsOnRange TargetDetection { set { targetDetection = value; } get { return targetDetection; } }

    public void InitializeBehaviour()
    {
        GetDependencies();
    }

    void GetDependencies()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = transform.GetComponent<TurretStats>();
        targetDetection = transform.GetComponent<ICurrentTargetsOnRange>();
        enemyDamageHandler = transform.GetComponent<IEnemyDamageHandler>();
    }

    public void UpdateBehaviour()
    {
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ShootEnemy(target);
        }
    }

    public void ShootEnemy(Transform enemy)
    {
        if(targetDetection.CurrentTargets[0] == null)
        {
            ResetTimer();
            return;
        }
        if(timer >= turretStats.AttackRate)
        {
            obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
            obj.GetComponent<Projectile>().SetInfo(enemy, transform, turretStats.AttackDamage, enemyDamageHandler);
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
