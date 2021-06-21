using UnityEngine;

public class KnockBackProjectile : Projectile
{
    Vector3 moveDirection;

    public override void SetInfo(Transform target, Transform turret, TurretStats turretStats, IEnemyDamageHandler enemyDamageHandler)
    {
        base.SetInfo(target, turret, turretStats, enemyDamageHandler);
        Vector3 enemyDirection = (target.position - transform.position).normalized;
        moveDirection = new Vector3(enemyDirection.x, 0, enemyDirection.z).normalized;
    }

    protected override void updateProjectile()
    {
        moveTowardsDirection();
    }

    private void moveTowardsDirection()
    {
        //Hacer que la speed se elija desde el inspector
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    protected override void OnEnemyCollision(Collider enemy)
    {
        //Mirar el numero de colisiones que quedan y guardarte los enemigos con los que ya has colisionado para no colisionar con ellos otra vez
        base.OnEnemyCollision(enemy);
        knockbackEnemy(enemy);
    }

    void knockbackEnemy(Collider enemy)
    {
        //hacer el knockback
        /*
        float poisonDamage = turretStats.SearchStatValue(StatType.POISONDAMAGEPERSECOND);
        float poisonDuration = turretStats.SearchStatValue(StatType.EFFECTDURATION);
        enemy.GetComponent<IPoisonable>().Poison(poisonDamage, poisonDuration);*/
    }
}
