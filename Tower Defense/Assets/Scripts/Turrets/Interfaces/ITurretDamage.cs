using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretDamage
{
    void OnTurretHit(BuildingRange turretTransform, float damage, IEnemyDamageHandler enemyDamage);
}
