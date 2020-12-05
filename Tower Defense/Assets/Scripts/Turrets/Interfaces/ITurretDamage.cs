using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretDamage
{
    void OnTurretHit(Transform turretTransform, float damage, IEnemyDamage enemyDamage);
}
