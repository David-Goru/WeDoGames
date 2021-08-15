using System;
using UnityEngine;

public class ConstantSlowToEnemyInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    TurretStats turretStats;
    Transform enemy;
    ISlowable enemyISlowable;
    
    float slowDuration;
    float slowReduction;

    public override void InitializeComponent()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }

    public override void UpdateComponent()
    {
        getStats();
        checkCurrentEnemyValidity();
        slowEnemy();
    }

    void getStats()
    {
        slowDuration = turretStats.GetStatValue(StatType.EFFECTDURATION);
        slowReduction = turretStats.GetStatValue(StatType.SLOWREDUCTION);
    }

    void checkCurrentEnemyValidity()
    {
        if(isThereAnEnemyNearby()) checkIfEnemyChanged();
        else
        {
            enemy = null;
            enemyISlowable = null;
        }
    }

    bool isThereAnEnemyNearby()
    {
        return targetDetection.CurrentTargets.Count > 0;
    }

    void checkIfEnemyChanged()
    {
        Transform currentEnemy = targetDetection.CurrentTargets[0];
        if (enemy != currentEnemy) changeCurrentEnemy(currentEnemy);
    }

    void changeCurrentEnemy(Transform currentEnemy)
    {
        enemy = currentEnemy;
        enemyISlowable = enemy.GetComponent<ISlowable>();
    }

    void slowEnemy()
    {
        if(enemyISlowable != null) enemyISlowable.Slow(slowDuration, slowReduction);
    }
}

public class ConstantDamageReductionInRange : EffectComponent
{
    [SerializeField] CurrentTargetsOnRange targetDetection = null;
    TurretStats turretStats;
    Transform enemy;
    IDamageReductible enemyIDamageReductible;

    float damageReductionDuration;
    float damageReduction;

    public override void InitializeComponent()
    {
        turretStats = GetComponentInParent<TurretStats>();
    }

    public override void UpdateComponent()
    {
        getStats();
        //reduceDamage();
    }

    void getStats()
    {
        damageReductionDuration = turretStats.GetStatValue(StatType.EFFECTDURATION);
        damageReduction = turretStats.GetStatValue(StatType.SLOWREDUCTION);
    }

}