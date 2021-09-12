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
    List<ITurretShotBehaviour> shotBehaviours = new List<ITurretShotBehaviour>();

    ObjectPooler objectPooler;
    TurretStats turretStats;
    IEnemyDamageHandler enemyDamageHandler;

    float timer = 0;
    bool areTargetsInRange = false;

    public CurrentTargetsOnRange TargetDetection { set { targetDetection = value; } get { return targetDetection; } }

    public override void InitializeComponent()
    {
        areTargetsInRange = false;
        getDependencies();
        resetTimer();
    }

    public override void UpdateComponent()
    {
        checkIfAreTargetsInRangeHasChanged();
        if(areTargetsInRange) shootEnemies();
    }

    void getDependencies()
    {
        objectPooler = ObjectPooler.GetInstance();
        turretStats = GetComponentInParent<TurretStats>();
        targetDetection = GetComponent<CurrentTargetsOnRange>();
        enemyDamageHandler = transform.parent.GetComponentInChildren<IEnemyDamageHandler>();
        attackStateBehaviours = GetComponents<ITurretAttackState>().ToList();
        shotBehaviours = GetComponents<ITurretShotBehaviour>().ToList();
    }

    void checkIfAreTargetsInRangeHasChanged()
    {
        if (areTargetsInRange != targetDetection.AreTargetsInRange)
        {
            areTargetsInRange = targetDetection.AreTargetsInRange;
            if (areTargetsInRange) callAttackStateBehaviours(true);
            else
            {
                callAttackStateBehaviours(false);
                resetTimer();
            }
        }
    }

    void callAttackStateBehaviours(bool enter)
    {
        foreach (ITurretAttackState attackStateBehaviour in attackStateBehaviours)
        {
            if (enter) attackStateBehaviour.OnAttackEnter();
            else attackStateBehaviour.OnAttackExit();
        }
    }

    void shootEnemies()
    {
        if (ReferenceEquals(targetDetection, null)) return;
        if (timer >= turretStats.GetStatValue(StatType.ATTACKSPEED))
        {
            foreach (Transform target in targetDetection.CurrentTargets) spawnAndInitializeProjectile(target);
            callShotBehaviours();
            resetTimer();
        }
        else timer += Time.deltaTime;
    }

    void callShotBehaviours()
    {
        foreach (ITurretShotBehaviour shotBehaviour in shotBehaviours) shotBehaviour.OnShot();
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