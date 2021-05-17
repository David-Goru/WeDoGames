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
        timer = 0f;
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
            resetTimer();
            return;
        }
        if(timer >= turretStats.GetStatValue(StatType.ATTACKRATE))
        {
            obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
            obj.GetComponent<Projectile>().SetInfo(enemy, transform, turretStats.GetStatValue(StatType.DAMAGE), enemyDamageHandler);
            resetTimer();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void resetTimer()
    {
        timer = 0;
    }

}
