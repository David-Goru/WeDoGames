using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretInfo", menuName = "ScriptableObjects/TurretInfo", order = 0)]
public class TurretInfo : ScriptableObject
{
    [Space(10)] [Header("Building System Stuff")] [Space(15)]
    [SerializeField] protected Pool turretPool = null;
    [SerializeField] protected Pool turretBlueprintPool = null;

    [Space(10)] [Header("Upgrade System Stuff")] [Space(15)]
    [SerializeField] string description = "";
    [SerializeField] Sprite icon = null;
    [SerializeField] TurretElement turretElement = TurretElement.NONE;
    [SerializeField] TurretTier turretTier = TurretTier.FIRST;

    [Space(10)] [Header("Design Stuff")] [Space(15)]
    [SerializeField] List<Stat> Stats = null;

    [Header("Debug")]
    [SerializeField] public List<Stat> currentStats = new List<Stat>();
    int numberOfTurretsPlaced = 0;

    public string Description { get => description; }
    public Sprite Icon { get => icon; set => icon = value; }
    public TurretElement TurretElement { get => turretElement; }
    public TurretTier TurretTier { get => turretTier; set => turretTier = value; }
    public int NumberOfTurretsPlaced { get => numberOfTurretsPlaced; set => numberOfTurretsPlaced = value; }

    public Pool GetBuildingPool() { return turretPool; }
    public Pool GetBuildingBlueprintPool() { return turretBlueprintPool; }

    public float GetStat(StatType type) 
    {
        for (int i = 0; i < currentStats.Count; i++)
        {
            if (currentStats[i].Type == type) return currentStats[i].Value;
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
        NumberOfTurretsPlaced = 0;
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