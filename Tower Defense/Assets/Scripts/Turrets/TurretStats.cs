using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores all the stats of the turret
/// </summary>
public class TurretStats : Entity, IHealable
{
    [SerializeField] TurretInfo buildingInfo = null;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        title = buildingInfo.name;
        currentHP = (int)GetStatValue(StatType.MAXHEALTH);
        maxHP = currentHP;
    }

    public float SearchStatValue(StatType statType)
    {
        for (int i = 0; i < buildingInfo.currentStats.Count; i++)
        {
            if (buildingInfo.currentStats[i].Type == statType) return buildingInfo.currentStats[i].Value;
        }
        Debug.LogError("There is no " + statType + "on " + buildingInfo.name);
        return 0f;
    }

    public float GetStatValue(StatType type)
    {
        return buildingInfo.GetStat(type);
    }

    public void Heal(int healValue)
    {
        currentHP = Mathf.Clamp(currentHP + healValue, 0, maxHP);
    }
}
