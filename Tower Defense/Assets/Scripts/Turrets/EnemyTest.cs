﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour, ITurretDamage
{
    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamage enemyDamage)
    {
        enemyDamage.OnEnemyHit(20f);
    }
}
