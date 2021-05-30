using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToEnemy : EffectComponent
{
    [SerializeField] Pool projectile = null;
    [SerializeField] Transform spawnPosition = null;
    ObjectPooler objectPooler;
    GameObject obj;

    float timer = 0;

    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    [SerializeField] CurrentTargetsOnRange targetDetection;

    public CurrentTargetsOnRange TargetDetection { set { targetDetection = value; } get { return targetDetection; } }


    public override void InitializeComponent()
    {
        GetDependencies();
        timer = 0f;
    }

    public override void UpdateComponent()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        foreach (Transform target in targetDetection.CurrentTargets)
        {
            ShootEnemy(target);
        }
    }

    void GetDependencies()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = GetComponentInParent<TurretStats>();
        targetDetection = GetComponent<CurrentTargetsOnRange>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
    }

    public void ShootEnemy(Transform enemy)
    {
        if(enemy == null)
        {
            resetTimer();
            return;
        }
        if(timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
            obj.GetComponent<Projectile>().SetInfo(enemy, transform.parent, turretStats, enemyDamageHandler);
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
