using UnityEngine;

public class SlowDownProjectile : BasicProjectile
{
    protected override void OnEnemyCollision(Collider enemy)
    {
        base.OnEnemyCollision(enemy);
        slowEnemy(enemy);
    }

    void slowEnemy(Collider enemy)
    {
        float slowReduction = turretStats.SearchStatValue(StatType.SLOWREDUCTION);
        float slowDuration = turretStats.SearchStatValue(StatType.EFFECTDURATION);
        enemy.GetComponent<ISlowable>().Slow(slowDuration, slowReduction);
    }
}