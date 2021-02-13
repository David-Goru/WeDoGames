using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingInfo", order = 0)]
public class BuildingInfo : ScriptableObject
{
    public List<Stat> Stats;
    [SerializeField] protected Pool buildingPool = null;
    [SerializeField] protected Pool buildingBlueprintPool = null;

    public Pool GetBuildingPool() { return buildingPool; }
    public Pool GetBuildingBlueprintPool() { return buildingBlueprintPool; }
    public float GetStat(StatType type) {
       Stat stat = Stats.Find(x => x.Type == type);
       return stat.Value;
    }
}
