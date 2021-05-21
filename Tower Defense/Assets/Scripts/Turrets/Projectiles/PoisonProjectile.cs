using UnityEngine;

public class PoisonProjectile : BasicProjectile
{
    protected override void OnEnemyCollision(Collider enemy)
    {
        base.OnEnemyCollision(enemy);
        poisonEnemy(enemy);
    }

    void poisonEnemy(Collider enemy)
    {
        float poisonDamage = turretStats.SearchStatValue(StatType.POISONDAMAGEPERSECOND);
        float poisonDuration = turretStats.SearchStatValue(StatType.EFFECTDURATION);
        enemy.GetComponent<IPoisonable>().Poison(poisonDamage, poisonDuration);
    }
}
