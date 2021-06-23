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
        checkIfCurrentEnemyChanged();
        slowEnemy();
    }

    private void getStats()
    {
        slowDuration = turretStats.GetStatValue(StatType.EFFECTDURATION);
        slowReduction = turretStats.GetStatValue(StatType.SLOWREDUCTION);
    }

    private void checkIfCurrentEnemyChanged()
    {
        if(targetDetection.CurrentTargets.Count > 0)
        {
            Transform currentEnemy = targetDetection.CurrentTargets[0];
            if (enemy != currentEnemy)
            {
                enemy = currentEnemy;
                enemyISlowable = enemy.GetComponent<ISlowable>();
            }
        }
        else
        {
            enemy = null;
            enemyISlowable = null;
        }
    }

    private void slowEnemy()
    {
        if(enemyISlowable != null)
            enemyISlowable.Slow(slowDuration, slowReduction);
    }
}