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
    BuildingRange turretBuildingRange;

    public ICurrentTargetsOnRange TargetDetection { set { targetDetection = value; } get { return targetDetection; } }

    public void InitializeBehaviour()
    {
        GetDependencies();
    }

    void GetDependencies()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = GetComponent<TurretStats>();
        targetDetection = GetComponent<ICurrentTargetsOnRange>();
        enemyDamageHandler = GetComponent<IEnemyDamageHandler>();
        turretBuildingRange = GetComponent<BuildingRange>();
    }

    public void UpdateBehaviour()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ShootEnemy(target);
        }
    }

    public void ShootEnemy(Transform enemy)
    {
        if(enemy == null)
        {
            ResetTimer();
            return;
        }
        if(timer >= turretStats.AttackRate)
        {
            obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
            obj.GetComponent<Projectile>().SetInfo(enemy, turretBuildingRange, turretStats.AttackDamage, enemyDamageHandler);
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
