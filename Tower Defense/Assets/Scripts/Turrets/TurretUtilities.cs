using UnityEngine;

public static class TurretUtilities
{
    public static bool IsEnemyDying(Transform enemy)
    {
        BaseAI enemyAI = enemy.GetComponent<BaseAI>();
        if (enemyAI != null) return enemyAI.IsDying;
        return false;
    }
}