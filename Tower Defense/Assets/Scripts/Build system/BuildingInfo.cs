using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingInfo", order = 0)]
public class BuildingInfo : ScriptableObject
{
    [SerializeField] protected float hp = 0f;
    [SerializeField] protected float price = 0f;
    [SerializeField] protected Pool buildingPool = null;
    [SerializeField] protected Pool buildingBlueprintPool = null;

    public float GetHp() { return hp; }
    public float GetPrice() { return price; }
    public Pool GetBuildingPool() { return buildingPool; }
    public Pool GetBuildingBlueprintPool() { return buildingBlueprintPool; }
}
