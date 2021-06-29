using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores all the stats of the turret
/// </summary>
public class TurretStats : MonoBehaviour, IHealable
{
    [SerializeField] BuildingInfo buildingInfo = null;

    public float currentHp;

    private void Start()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        currentHp = buildingInfo.GetStat(StatType.MAXHEALTH);

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

    public void Heal(float healValue)
    {
        currentHp = Mathf.Clamp(currentHp + healValue, 0, buildingInfo.GetStat(StatType.MAXHEALTH));
    }
}
