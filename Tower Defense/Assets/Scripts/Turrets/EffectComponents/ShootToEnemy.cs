using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootToEnemy : EffectComponent
{
    [SerializeField] Pool projectile = null;
    [SerializeField] Transform spawnPosition = null;
    [SerializeField] CurrentTargetsOnRange targetDetection = null;

    List<ITurretAttackState> attackStateBehaviours = new List<ITurretAttackState>();
    ObjectPooler objectPooler;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;
    float timer = 0;

    public CurrentTargetsOnRange TargetDetection { set { targetDetection = value; } get { return targetDetection; } }

    public override void InitializeComponent()
    {
        getDependencies();
        resetTimer();
    }

    public override void UpdateComponent()
    {
        shootEnemies();
    }

    void getDependencies()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = GetComponentInParent<TurretStats>();
        targetDetection = GetComponent<CurrentTargetsOnRange>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
        attackStateBehaviours = GetComponents<ITurretAttackState>().ToList();
    }

    void shootEnemies()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        foreach (Transform target in targetDetection.CurrentTargets) shootEnemy(target);
    }

    void shootEnemy(Transform enemy)
    {
        if(enemy == null)  resetTimer();
        else if(timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            spawnAndInitializeProjectile(enemy);
            callAttackStateBehaviours();
            resetTimer();
        }
        else timer += Time.deltaTime;
    }

    void callAttackStateBehaviours()
    {
        foreach (ITurretAttackState attackStateBehaviour in attackStateBehaviours) attackStateBehaviour.OnAttackEnter();
    }

    void spawnAndInitializeProjectile(Transform enemy)
    {
        GameObject obj = objectPooler.SpawnObject(projectile.tag, spawnPosition.position);
        obj.GetComponent<Projectile>().SetInfo(enemy, transform.parent, turretStats, enemyDamageHandler);
    }

    void resetTimer()
    {
        timer = 0;
    }
}