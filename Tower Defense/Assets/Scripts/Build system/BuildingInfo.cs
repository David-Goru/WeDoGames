using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingInfo", order = 0)]
public class BuildingInfo : ScriptableObject
{
    [SerializeField] string description = "";
    [SerializeField] TurretElement turretElement = TurretElement.NONE;
    [SerializeField] TurretTier turretTier = TurretTier.FIRST;
    [SerializeField] List<Stat> Stats = null;
    [SerializeField] protected Pool buildingPool = null;
    [SerializeField] protected Pool buildingBlueprintPool = null;

    [Header("Debug")]
    [SerializeField] public List<Stat> currentStats = new List<Stat>();

    public string Description { get => description; }
    public TurretElement TurretElement { get => turretElement; }
    public TurretTier TurretTier { get => turretTier; set => turretTier = value; }
    public Pool GetBuildingPool() { return buildingPool; }
    public Pool GetBuildingBlueprintPool() { return buildingBlueprintPool; }

    public float GetStat(StatType type) 
    {
        for (int i = 0; i < currentStats.Count; i++)
        {
            if (currentStats[i].Type == type)
            {
                return currentStats[i].Value;
            }
        }
        Debug.LogError("There is no " + type + " on " + name);
        return 0f;
    }

    public void IncrementStat(StatType type, float increment)
    {
        for (int i = 0; i < currentStats.Count; i++)
        {
            if(currentStats[i].Type == type)
            {
                if(currentStats[i].Value + increment > Stats[i].Value * 0.1f)
                    currentStats[i] = new Stat(type, currentStats[i].Value + increment);
            }
        }
    }

    void OnEnable()
    {
        resetCurrentStats();
    }

    void resetCurrentStats()
    {
        currentStats.Clear();
        foreach (Stat stat in Stats)
        {
            currentStats.Add(stat);
        }
    }
}