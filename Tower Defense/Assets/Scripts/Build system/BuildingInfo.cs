using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingInfo", order = 0)]
public class BuildingInfo : ScriptableObject
{
    [SerializeField] protected float hp;
    [SerializeField] protected float price;
    [SerializeField] protected Pool turretPool;

    public float GetHp() { return hp; }
    public float GetPrice() { return price; }
    public Pool GetTurretPool() { return turretPool; }
}
