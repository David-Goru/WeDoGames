using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStats
{
    float maxHp;
    public float AttackRate;
    public float AttackDamage;
    public float AttackRange;
    public float currentHp;

    public TurretStats(TurretInfo turretInfo)
    {
        maxHp = turretInfo.GetHp(); ;
        AttackRate = turretInfo.GetAttackRate();
        AttackDamage = turretInfo.GetAttackDamage();
        AttackRange = turretInfo.GetAttackRange();
        currentHp = maxHp;
    }
}
