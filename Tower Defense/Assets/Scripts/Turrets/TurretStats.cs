using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores all the stats of the turret
/// </summary>
public class TurretStats : MonoBehaviour
{
    [SerializeField] BuildingInfo buildingInfo = null;
    Dictionary<StatType, float> TypeValueDictionary = new Dictionary<StatType, float>();

    public float currentHp;

    private void Start()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        foreach (Stat stat in buildingInfo.Stats)
        {
            TypeValueDictionary[stat.Type] = stat.Value;
        }
        currentHp = TypeValueDictionary[StatType.MAXHEALTH];
    }

    public float GetStatValue(StatType type)
    {
        return TypeValueDictionary[type];
    }

    public void IncrementValue(StatType type, float increment)
    {
        TypeValueDictionary[type] += increment;
    }

}