using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour, ITurretDamage
{
    public void OnTurretHit(BuildingRange turretTransform, float damage, IEnemyDamageHandler enemyDamage)
    {
        print("Ma dao");
    }
}
