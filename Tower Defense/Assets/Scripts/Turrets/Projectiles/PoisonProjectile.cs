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
        float poisonDuration = turretStats.SearchStatValue(StatType.EFFECTDURATION);
        int poisonDamage = (int)turretStats.SearchStatValue(StatType.POISONDAMAGEPERSECOND);
        enemy.GetComponent<IPoisonable>().Poison(poisonDuration, poisonDamage);
    }
}
