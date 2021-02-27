using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information of the game loop
/// </summary>
[CreateAssetMenu(fileName = "MasterInfo", menuName = "ScriptableObjects/MasterInfo", order = 0)]
public class MasterInfo : ScriptableObject
{
    [SerializeField] float balance = 0;
    [SerializeField] BuildingInfo[] buildingsSet = null;
    [SerializeField] Upgrade[] upgradesSet = null;
    [SerializeField] Pool[] enemiesSet = null;

    public float Balance { get => balance; set => balance = value; }
    public Upgrade[] UpgradesSet { get => upgradesSet; }

    public BuildingInfo[] GetBuildingsSet() { return buildingsSet; }
    public Pool[] GetEnemiesSet() { return enemiesSet; }
}